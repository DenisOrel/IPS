// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.TablesPump.TablePump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using Intermech.Expert.Table;
using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common;
using Intermech.ImpExp.TechCard.TechExpPump.Common;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Expert;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.TablesPump;

[TaskDescription("Инициализация перекачки таблиц экспертной системы", "Перекачка таблиц экспертной системы")]
[TaskType(PumperType.MetaData)]
internal class TablePump : TechExpFolderSupportPump
{
  private readonly Guid _guid = new Guid("{213034DE-697F-49bb-8F09-D710BA9BE3E6}");
  private const int CheckCount = 1;
  private readonly Dictionary<TechExpObject, Dictionary<TableInfo, TableBody>> _expObj2DataCache;
  private string _tableFileName = string.Empty;
  private readonly List<string> _tmpFiles;
  private readonly Dictionary<int, TechExpFolderObject> _tableImportList;
  private readonly Dictionary<TechExpKey, TechExpKey> _tableKey2OwnerKeyCache;

  protected override Guid GUID => this._guid;

  protected virtual void TablePump_AfterImportEvent(object sender, EventArgs e)
  {
    try
    {
      if (!(sender is IImportedObjectList importedObjectList))
        return;
      Dictionary<TechExpKey, long> dictionary = new Dictionary<TechExpKey, long>();
      TechExpKey key1;
      for (int index = 0; index < importedObjectList.Items.Count; ++index)
      {
        ImportingObject importingObject = importedObjectList.Items[index];
        TechExpFolderObject folderObject;
        if (this._tableImportList.TryGetValue(index, out folderObject))
        {
          Exception importError = importedObjectList.GetImportError(index);
          if (importError != null)
          {
            IAppManager appManager = this.plugin.appManager;
            key1 = folderObject.Key;
            string Message = $"Объект идентичный записи {key1.Value} из таблицы {"TC_EXPERT"} не импортирован, по причине: {importError.Message}";
            appManager.AddWarningMessage(Message);
          }
          if (importingObject != null)
          {
            long objectId = importingObject.Object.Object_id;
            folderObject.ImportedObjectInfo = new QuickObjectInfo(importingObject.Object.Object_id, importingObject.Object.Caption, importingObject.Object.ObjectType, Guid.Empty, importingObject.Object.Id);
            if (objectId != 0L)
            {
              dictionary.Add(folderObject.Key, objectId);
              this.TableRegisterInExpert(objectId);
              this.PumpFolderRelation((TableInfo) null, folderObject);
            }
          }
        }
      }
      if (this._importingData != null)
      {
        foreach (KeyValuePair<TechExpKey, long> keyValuePair in dictionary)
        {
          IImportingData importingData = this._importingData;
          // ISSUE: variable of a boxed type
          __Boxed<int> oldKey = (System.ValueType) -1;
          key1 = keyValuePair.Key;
          long newKey = key1.Value;
          importingData.SetNewKey(ImportingCategory.TechExpObjStruct, (object) oldKey, newKey);
        }
      }
      for (int key2 = 0; key2 < importedObjectList.Items.Count; ++key2)
        this._tableImportList.Remove(key2);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Ошибка импорта текущего пакета: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
    finally
    {
      if (this._tableImportList.Count > 1024 /*0x0400*/)
      {
        string Message = "TablePump._tableImportList - слишком много объектов!";
        TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
      }
    }
  }

  protected virtual void TableRegisterInExpert(long objectId)
  {
    IUserSession userSession = TechcardConsts.Plugin.Idw.GetUserSession();
    if (!(userSession.GetCustomService(typeof (IExpertServer)) is IExpertServer customService))
      throw new ServiceNotFoundException($"Служба {"IExpertServer"} не найдена.");
    try
    {
      customService.ReflectObjUpdate(userSession.SessionGUID, objectId, ExpertTraceFlags.None, (TempFormula) null, out byte[] _);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Ошибка регистрации таблицы экспертной системы objectID = \"{objectId}\" : \"{ex.Message}\" ");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
  }

  public TablePump(PluginClass plugin)
    : base(plugin)
  {
    this._impExpObjType = -1;
    this._expObj2DataCache = new Dictionary<TechExpObject, Dictionary<TableInfo, TableBody>>();
    this._tableKey2OwnerKeyCache = new Dictionary<TechExpKey, TechExpKey>();
    this._tmpFiles = new List<string>();
    this._tableImportList = new Dictionary<int, TechExpFolderObject>();
  }

  public override void Exam()
  {
    this.ExamCheckPoint("Подготовка к закачке таблиц экспертной системы:", 0);
    if (!this.TableExists("TC_EXPERT"))
      this.plugin.appManager.AddWarningMessage($"Таблица '{"TC_EXPERT"}' не найдена.");
    else
      this.ExamCheckPoint("Подготовка к закачке таблиц экспертной системы: успешно завершена", 100);
  }

  protected override bool NeedPumpExpData()
  {
    if (TechSettingsHelper.PumpMetaDataType.HasFlag((Enum) TechPumpMetaDataType.ExpertTables))
      return true;
    this.plugin.appManager.AddInfoMessage("Перекачка таблиц ЭС отключена в настройках");
    this.PumpCheckPoint("Перекачка данных отключена", 0);
    return false;
  }

  protected override void LoadExpertObjData()
  {
    this.PumpCheckPoint("Считывание структуры таблиц", 0);
    long expertObjectKey;
    TechExpKeyConverter.ConvertFrom(new TechExpKey(this._lastObjId), out expertObjectKey);
    string str1 = expertObjectKey == 0L ? string.Empty : " AND F_KEY >= " + (object) expertObjectKey;
    string sqlText = $" SELECT * FROM TC_EXPERT WHERE F_TYPE   IN ({"'T', 'A'"}) {str1} ORDER BY F_KEY";
    int recordsCount = this.GetRecordsCount($"SELECT COUNT(*) FROM TC_EXPERT WHERE F_TYPE IN ({"'T', 'A'"}) {str1} ");
    int index1 = 0;
    IDataReader dataReader = this.GetDataReader(sqlText);
    try
    {
      List<TechExpObject> techExpObjectList = new List<TechExpObject>();
      TechExpObject.ParseSchema(this.GetTableColumns(dataReader));
      while (dataReader.Read())
      {
        ++index1;
        TechExpObject key = new TechExpObject(dataReader);
        switch (key.Type)
        {
          case TechExpObjType.AutoSelection:
          case TechExpObjType.Table:
            this._expObj2DataCache.Add(key, (Dictionary<TableInfo, TableBody>) null);
            techExpObjectList.Add(key);
            break;
        }
        if (index1 % 1 == 0 || index1 == recordsCount)
          this.PumpCheckPoint($"Считывание таблиц экспертной системы ({index1} из {recordsCount})", this.CalculatePercent(recordsCount, index1, 1, 24));
      }
      int index2 = 0;
      foreach (TechExpObject key1 in techExpObjectList)
      {
        ++index2;
        BinaryReader br = new BinaryReader((Stream) new MemoryStream(key1.Body), Encoding.Default);
        TableHeader tableHeader = new TableHeader();
        if (key1.Type == TechExpObjType.AutoSelection)
        {
          tableHeader.Signature = br.ReadChars(4);
          string str2 = new string(tableHeader.Signature);
          if (!str2.Equals("AOTF"))
          {
            this.plugin.appManager.AddWarningMessage($"Wrong Table File Header: '{str2}' for table : '{key1.Name}'.");
            continue;
          }
          tableHeader.HeaderLen = br.ReadUInt16();
          tableHeader.VersionNum = br.ReadUInt16();
          tableHeader.TableCount = br.ReadUInt16();
          br.BaseStream.Position = (long) tableHeader.HeaderLen;
          Dictionary<TableInfo, TableBody> dictionary = new Dictionary<TableInfo, TableBody>();
          List<TableInfo> tableInfoList = new List<TableInfo>();
          for (int index3 = 0; index3 < (int) tableHeader.TableCount; ++index3)
          {
            TableInfo key2 = new TableInfo();
            key2.Load(br, tableHeader.VersionNum);
            key2.TableIdx = index3;
            dictionary.Add(key2, (TableBody) null);
            tableInfoList.Add(key2);
          }
          foreach (TableInfo tableInfo in tableInfoList)
          {
            if (tableInfo.HasBody)
            {
              TableBody tableBody = new TableBody();
              tableBody.Load(br, tableHeader.VersionNum, tableInfo, false);
              dictionary[tableInfo] = tableBody;
            }
          }
          this._expObj2DataCache[key1] = dictionary;
        }
        else if (key1.Type == TechExpObjType.Table)
        {
          tableHeader.Load(br);
          string str3 = new string(tableHeader.Signature);
          if (!str3.Equals("TCTF"))
          {
            this.plugin.appManager.AddWarningMessage($"Wrong Table File Header: '{str3}' for table : '{key1.Name}'.");
            continue;
          }
          Dictionary<TableInfo, TableBody> dictionary = new Dictionary<TableInfo, TableBody>();
          List<TableInfo> tableInfoList = new List<TableInfo>();
          for (int index4 = 0; index4 < (int) tableHeader.TableCount; ++index4)
          {
            TableInfo key3 = new TableInfo();
            key3.Load(br, tableHeader.VersionNum);
            key3.TableIdx = index4;
            dictionary.Add(key3, (TableBody) null);
            tableInfoList.Add(key3);
          }
          foreach (TableInfo tableInfo in tableInfoList)
          {
            if (tableInfo.HasBody)
            {
              TableBody tableBody = new TableBody();
              tableBody.Load(br, tableHeader.VersionNum, tableInfo);
              dictionary[tableInfo] = tableBody;
            }
          }
          this._expObj2DataCache[key1] = dictionary;
        }
        if (index2 % 100 == 0 || index2 == recordsCount - 1)
          this.PumpCheckPoint($"Загрузка таблиц экспертной системы ({index2} из {this._expObj2DataCache.Keys.Count})", this.CalculatePercent(this._expObj2DataCache.Keys.Count, index2, 25, 49));
      }
    }
    finally
    {
      dataReader.Close();
    }
  }

  protected override void PumpExpertObjData()
  {
    this.PumpCheckPoint("Подготовка к закачке таблиц экспертной системы", 50);
    if (ServicesManager.GetService(typeof (ICache)) is ICache service)
      this._importingData = service.GetCache(ImportingCategory.TechExpTables, ImportingCategory.TechExpObjStruct, ImportingCategory.ImbaseFolders, ImportingCategory.TechCeh, ImportingCategory.ImbaseTableLinksKeyToObjectID);
    this._impObjList = this.plugin.Idw.CreateImportedObjectListWithStatistics(this.GUID);
    this._impRelList = this.plugin.Idw.CreateImportedRelationListWithStatistics(this.GUID);
    this._impObjList.AfterImportEvent += new AfterImportEventDelegate(this.TablePump_AfterImportEvent);
    TechExpTablesConst.Initialize();
    this.PumpCheckPoint("Обработка таблиц экспертной системы", 51);
    int num = 0;
    int index = 0;
    int count = this._expObj2DataCache.Keys.Count;
    try
    {
      foreach (KeyValuePair<TechExpObject, Dictionary<TableInfo, TableBody>> keyValuePair in this._expObj2DataCache)
      {
        TechExpObject key = keyValuePair.Key;
        Dictionary<TableInfo, TableBody> tableInfoCache = keyValuePair.Value;
        if (key == null || tableInfoCache == null)
        {
          ++index;
        }
        else
        {
          this._tableFileName = key.Name;
          switch (key.Type)
          {
            case TechExpObjType.AutoSelection:
              int recordsWithError1;
              if (!this.PumpExpertObjAutoSelection(key, tableInfoCache, out recordsWithError1))
              {
                num += recordsWithError1;
                break;
              }
              break;
            case TechExpObjType.Table:
              int recordsWithError2;
              if (!this.PumpExpertObjTable(key, tableInfoCache, out recordsWithError2))
              {
                num += recordsWithError2;
                break;
              }
              break;
          }
          ++index;
          if (index % 100 == 0 || index == count - 1)
            this.PumpCheckPoint($"Обработка таблиц экспертной системы ({index} из {count})", this.CalculatePercent(count, index, 52, 99));
        }
      }
      this._impObjList.Import();
      this._impRelList.Import();
      foreach (string tmpFile in this._tmpFiles)
      {
        if (System.IO.File.Exists(tmpFile))
          System.IO.File.Delete(tmpFile);
      }
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.TechExpTables, ImportingCategory.TechExpObjStruct, ImportingCategory.ImbaseFolders, ImportingCategory.TechCeh, ImportingCategory.ImbaseTableLinksKeyToObjectID);
    }
    this.PumpCheckPoint(num.Equals(0) ? "Закачка таблиц экспертной системы завершена успешно" : $"Закачка таблиц экспертной системы завершена c ошибками, \"{num}\" таблиц не закачано", 100);
  }

  private bool PumpExpertObjTable(
    TechExpObject expertObject,
    Dictionary<TableInfo, TableBody> tableInfoCache,
    out int recordsWithError)
  {
    recordsWithError = 0;
    this._techFolderCache.Clear();
    this._tableKey2OwnerKeyCache.Clear();
    TechExpFolderObject techExpFolder;
    if (this.PumpRootFolderObject(expertObject, out techExpFolder))
      this._techFolderCache[techExpFolder.Key] = techExpFolder;
    List<Tuple<TableInfo, TechExpFolderObject>> tupleList = new List<Tuple<TableInfo, TechExpFolderObject>>();
    int recordId1 = 0;
    foreach (KeyValuePair<TableInfo, TableBody> keyValuePair in tableInfoCache)
    {
      try
      {
        TechExpKey key1 = TechExpKeyConverter.ConvertTo((long) expertObject.Key, (long) recordId1);
        TechExpFolderObject folderObject = TechExpFolderObjectFactory.Instance.CreateFolderObject(key1, keyValuePair.Key);
        this._techFolderCache[key1] = folderObject;
        this.ConvertFolderCondition(expertObject, folderObject);
        int index = recordId1;
        if (tupleList.Count > 0 && index >= tupleList.Count)
          index = tupleList.Count - 1;
        while (index > 0 && (int) tupleList[index].Item1.HierLevel > (int) keyValuePair.Key.HierLevel)
          --index;
        if (index >= tupleList.Count)
          this._tableKey2OwnerKeyCache[folderObject.Key] = techExpFolder.Key;
        else if ((int) tupleList[index].Item1.HierLevel == (int) keyValuePair.Key.HierLevel)
        {
          TechExpKey techExpKey = this._tableKey2OwnerKeyCache[tupleList[index].Item2.Key];
          this._tableKey2OwnerKeyCache[folderObject.Key] = techExpKey;
        }
        else
        {
          TechExpKey key2 = tupleList[index].Item2.Key;
          this._tableKey2OwnerKeyCache[folderObject.Key] = key2;
        }
        tupleList.Add(new Tuple<TableInfo, TechExpFolderObject>(keyValuePair.Key, folderObject));
        if (this._lastObjId != 0L)
        {
          if (key1.Value <= this._lastObjId)
            continue;
        }
        if (keyValuePair.Value == null)
        {
          if (this.PumpFolderObject(folderObject))
            this._importingData.SetNewKey(ImportingCategory.TechExpObjStruct, (object) -1, key1.Value);
        }
      }
      finally
      {
        ++recordId1;
      }
    }
    this._impObjList.Import();
    int recordId2 = 0;
    foreach (KeyValuePair<TableInfo, TableBody> keyValuePair in tableInfoCache)
    {
      try
      {
        TechExpKey key = TechExpKeyConverter.ConvertTo((long) expertObject.Key, (long) recordId2);
        if (this._lastObjId != 0L)
        {
          if (key.Value <= this._lastObjId)
            continue;
        }
        TechExpFolderObject folderObject;
        if (this._techFolderCache.TryGetValue(key, out folderObject))
        {
          if (keyValuePair.Value == null)
            this.PumpFolderRelation(keyValuePair.Key, folderObject);
          else if (!this.PumpExpertTableData(expertObject, keyValuePair.Key, keyValuePair.Value, folderObject))
            ++recordsWithError;
        }
      }
      finally
      {
        ++recordId2;
      }
    }
    return recordsWithError == 0;
  }

  private void PumpFolderRelation(TableInfo tableInfo, TechExpFolderObject folderObject)
  {
    TechExpKey key;
    TechExpFolderObject techExpFolderObject;
    if (!this._tableKey2OwnerKeyCache.TryGetValue(folderObject.Key, out key) || !this._techFolderCache.TryGetValue(key, out techExpFolderObject) || techExpFolderObject.ImportedObjectInfo.Empty || this._impRelList.AddRelationFromID(techExpFolderObject.ImportedObjectInfo.ObjectID, folderObject.ImportedObjectInfo.ID, ExpertConsts.Consts.linkSimpleSortId) == null)
      return;
    if (tableInfo != null)
      this._impRelList.AddAttributeInt(ExpertConsts.Consts.attrSorting, folderObject.Key.Value);
    else
      this._impRelList.AddAttributeInt(ExpertConsts.Consts.attrSorting, folderObject.Key.Value);
  }

  private bool PumpExpertObjAutoSelection(
    TechExpObject expertObject,
    Dictionary<TableInfo, TableBody> tableInfoCache,
    out int recordsWithError)
  {
    recordsWithError = 0;
    int recordId = 0;
    foreach (KeyValuePair<TableInfo, TableBody> keyValuePair in tableInfoCache)
    {
      try
      {
        TechExpKey key = TechExpKeyConverter.ConvertTo((long) expertObject.Key, (long) recordId);
        if (this._lastObjId != 0L)
        {
          if (key.Value <= this._lastObjId)
            continue;
        }
        TechExpFolderObject folderObject = TechExpFolderObjectFactory.Instance.CreateFolderObject(key, keyValuePair.Key);
        this._techFolderCache[key] = folderObject;
        if (keyValuePair.Value != null)
        {
          if (!this.PumpExpertTableData(expertObject, keyValuePair.Key, keyValuePair.Value, folderObject))
            ++recordsWithError;
        }
      }
      finally
      {
        ++recordId;
      }
    }
    return recordsWithError == 0;
  }

  private bool PumpExpertTableData(
    TechExpObject techExpObject,
    TableInfo tableInfo,
    TableBody tableBody,
    TechExpFolderObject folderObject)
  {
    bool flag = true;
    try
    {
      TableInfo tableInfo1 = tableInfo;
      TableBody tableBody1 = tableBody;
      eTableType entries = (eTableType) tableInfo1.Entries;
      List<eTable> tableList = new List<eTable>();
      for (int index1 = 0; index1 < tableBody1.LayerList.Count; ++index1)
      {
        eTable eTable = new eTable(entries)
        {
          Name = tableInfo1.Name,
          ValuesTable = new eValuesTable(tableBody1.LayerList[index1].Rows, tableBody1.LayerList[index1].Cols)
        };
        Dictionary<string, IAttributeTypeItem> dictionary = new Dictionary<string, IAttributeTypeItem>();
        for (int index2 = 0; index2 < 3; ++index2)
        {
          foreach (string str in tableInfo1.Dims[index2].ArgList)
          {
            IAttributeTypeItem attributeItemByCode;
            if (!dictionary.TryGetValue(str, out attributeItemByCode) || attributeItemByCode == null)
            {
              string errorMsg;
              attributeItemByCode = TechExpert.TypeConverter.GetAttributeItemByCode(str, this.plugin, out errorMsg);
              if (attributeItemByCode == null)
                throw new EntitySettNotExistException(errorMsg);
              dictionary.Add(str, attributeItemByCode);
            }
          }
        }
        switch (entries)
        {
          case eTableType.NoEntry:
            eRow eRow1 = new eRow();
            List<CommonTypeHolder> commonTypeHolderList1 = new List<CommonTypeHolder>();
            for (int index3 = 0; index3 < tableInfo1.Dims[1].ArgList.Count; ++index3)
            {
              string str = tableInfo1.Dims[1].ArgList[index3];
              IAttributeTypeItem attrItem = dictionary[str];
              DataType dataType;
              CommonTypeHolder cth;
              this.GetAttributeTypeItemParams(str, attrItem, out dataType, out cth);
              eCell cell = new eCell(eCellDestination.Header, cth);
              if (!tableInfo1.Dims[1].IsCond[index3])
              {
                cell.CellDestination = eCellDestination.Result;
                commonTypeHolderList1.Add(cell.CommonType);
              }
              eRow1.Add(cell);
              for (int row = 0; row < tableBody1.LayerList[index1].Rows; ++row)
                eTable.ValuesTable[row, index3] = new eCell(eCellDestination.Data, cth)
                {
                  CellValue = this.UnpackValue(dataType, tableBody1.LayerList[index1].Data[index3, row], str)
                };
            }
            eTable.FixedRows.Add(eRow1);
            eTable.Result = commonTypeHolderList1.ToArray();
            break;
          case eTableType.SingleEntry:
            eRow eRow2 = new eRow();
            List<CommonTypeHolder> commonTypeHolderList2 = new List<CommonTypeHolder>();
            for (int index4 = 0; index4 < tableInfo1.Dims[1].ArgList.Count; ++index4)
            {
              string str = tableInfo1.Dims[1].ArgList[index4];
              IAttributeTypeItem attrItem = dictionary[str];
              DataType dataType;
              CommonTypeHolder cth;
              this.GetAttributeTypeItemParams(str, attrItem, out dataType, out cth);
              eCell cell = new eCell(eCellDestination.Result, cth);
              commonTypeHolderList2.Add(cth);
              eRow2.Add(cell);
              for (int row = 0; row < tableBody1.LayerList[index1].Rows; ++row)
                eTable.ValuesTable[row, index4] = new eCell(eCellDestination.Data, cth)
                {
                  CellValue = this.UnpackValue(dataType, tableBody1.LayerList[index1].Data[index4, row], str)
                };
            }
            eTable.FixedRows.Add(eRow2);
            for (int index5 = 0; index5 < tableInfo1.Dims[0].ArgList.Count; ++index5)
            {
              eColumn eColumn = new eColumn();
              string str = tableInfo1.Dims[0].ArgList[index5];
              IAttributeTypeItem attrItem = dictionary[str];
              DataType dataType;
              CommonTypeHolder cth;
              this.GetAttributeTypeItemParams(str, attrItem, out dataType, out cth);
              eColumn.Header = new eCell(eCellDestination.Header, cth)
              {
                CellSymbol = TechExpert.DataConverter.ConvertSymbol(tableBody1.ColDefSigns[index5])
              };
              for (int index6 = 0; index6 < tableBody1.VCond.Rows; ++index6)
              {
                eCellSymbol cellSymbol;
                eColumn.Add(new eCell(eCellDestination.HeaderData, cth)
                {
                  CellValue = this.ParseHeaderData(tableBody1.VCond.Data[index5, index6], dataType, eColumn.Header.CellSymbol, out cellSymbol, str),
                  CellSymbol = cellSymbol
                });
              }
              eTable.FixedColumns.Add(eColumn);
            }
            eTable.Result = commonTypeHolderList2.ToArray();
            break;
          case eTableType.DoubleEntry:
            string str1 = tableInfo1.Dims[2].ArgList[index1];
            IAttributeTypeItem attrItem1 = dictionary[str1];
            DataType dataType1;
            CommonTypeHolder cth1;
            this.GetAttributeTypeItemParams(str1, attrItem1, out dataType1, out cth1);
            eTable.Result = new CommonTypeHolder[1]{ cth1 };
            eRow eRow3 = new eRow();
            eRow3.Add(new eCell(eCellDestination.Result, eCellType.Text)
            {
              ColSpan = tableBody1.HCond.Cols,
              CellValue = new ExpertValue($"{cth1.ObjectType.Name} - {cth1.AttributeType.Name}")
            });
            for (int column = 0; column < tableBody1.LayerList[index1].Cols; ++column)
            {
              for (int row = 0; row < tableBody1.LayerList[index1].Rows; ++row)
                eTable.ValuesTable[row, column] = new eCell(eCellDestination.Data, cth1)
                {
                  CellValue = this.UnpackValue(dataType1, tableBody1.LayerList[index1].Data[column, row], str1)
                };
            }
            for (int index7 = 0; index7 < tableInfo1.Dims[1].ArgList.Count; ++index7)
            {
              eRow eRow4 = new eRow();
              string str2 = tableInfo1.Dims[1].ArgList[index7];
              IAttributeTypeItem attrItem2 = dictionary[str2];
              this.GetAttributeTypeItemParams(str2, attrItem2, out dataType1, out cth1);
              eRow4.Header = new eCell(eCellDestination.Header, cth1)
              {
                CellSymbol = TechExpert.DataConverter.ConvertSymbol(tableBody1.RowDefSigns[index7])
              };
              for (int index8 = 0; index8 < tableBody1.HCond.Cols; ++index8)
              {
                eCellSymbol cellSymbol;
                eRow4.Add(new eCell(eCellDestination.HeaderData, cth1)
                {
                  CellValue = this.ParseHeaderData(tableBody1.HCond.Data[index8, index7], dataType1, eRow4.Header.CellSymbol, out cellSymbol, str2),
                  CellSymbol = cellSymbol
                });
              }
              eTable.FixedRows.Add(eRow4);
            }
            eTable.FixedRows.Add(eRow3);
            for (int index9 = 0; index9 < tableInfo1.Dims[0].ArgList.Count; ++index9)
            {
              eColumn eColumn = new eColumn();
              string str3 = tableInfo1.Dims[0].ArgList[index9];
              IAttributeTypeItem attrItem3 = dictionary[str3];
              this.GetAttributeTypeItemParams(str3, attrItem3, out dataType1, out cth1);
              eColumn.Header = new eCell(eCellDestination.Header, cth1)
              {
                CellSymbol = TechExpert.DataConverter.ConvertSymbol(tableBody1.ColDefSigns[index9])
              };
              for (int index10 = 0; index10 < tableBody1.VCond.Rows; ++index10)
              {
                eCellSymbol cellSymbol;
                eColumn.Add(new eCell(eCellDestination.HeaderData, cth1)
                {
                  CellValue = this.ParseHeaderData(tableBody1.VCond.Data[index9, index10], dataType1, eColumn.Header.CellSymbol, out cellSymbol, str3),
                  CellSymbol = cellSymbol
                });
              }
              eTable.FixedColumns.Add(eColumn);
            }
            break;
        }
        tableList.Add(eTable);
      }
      if (this.SaveTableData(tableInfo, tableList) != null)
        this._tableImportList[this._impObjList.Items.Count - 1] = folderObject;
      if (techExpObject.Type == TechExpObjType.AutoSelection)
      {
        long expertObjectKey;
        TechExpKeyConverter.ConvertFrom(folderObject.Key, out expertObjectKey);
        if (this._importingData.GetValue(ImportingCategory.TechExpTables, (object) expertObjectKey) == null)
        {
          TechObjectTag tag = new TechObjectTag((object) tableList.ToArray());
          this._importingData.AddValue(ImportingCategory.TechExpTables, (object) expertObjectKey, -1L, (ITagImportObject) tag);
        }
      }
    }
    catch (Exception ex)
    {
      switch (ex)
      {
        case AttributeNotExistsException _:
        case CommonDataTypeConvertException _:
        case EntitySettNotExistException _:
          flag = false;
          string message = ex.Message;
          this.plugin.appManager.AddWarningMessage($"Ошибка закачки таблицы (файл \"{this._tableFileName}\", таблица (Код = \"{tableInfo.Code}\" имя = \"{tableInfo.Name}\") (Сообщение: {message})");
          break;
        default:
          throw;
      }
    }
    return flag;
  }

  protected override void ReleasePumpData()
  {
    base.ReleasePumpData();
    this._expObj2DataCache.Clear();
  }

  internal ObjectRecord SaveTableData(TableInfo tableInfo, List<eTable> tableList)
  {
    if (tableList == null)
      return (ObjectRecord) null;
    eTableCollection eTableCollection = new eTableCollection(tableList.ToArray());
    string str = (!string.IsNullOrEmpty(tableInfo.Code) ? $"({tableInfo.Code}) " : string.Empty) + tableInfo.Name;
    ObjectRecord objectRecord1 = this._impObjList.AddObject(TechExpTablesConst.DBTableObjectTypeID, 0);
    if (objectRecord1 != null)
    {
      objectRecord1.Caption = str.Truncate(Intermech.Consts.MaxStringSize - 2);
      if (tableInfo.TableIdx != -1)
      {
        ObjectRecord objectRecord2 = objectRecord1;
        objectRecord2.Caption = $"{objectRecord2.Caption} [{(object) tableInfo.TableIdx}]";
      }
      objectRecord1.IdGuid = (object) TechExpert.Utils.GetGuid4Import();
    }
    this.SaveTableBodyData(tableList);
    this._impObjList.AddAttributeStr(this._atNaimAttrTypeId, str);
    this._impObjList.AddAttributeStr(TechExpTablesConst.DBNameAttributeTypeID, str);
    this._impObjList.AddAttributeInt(TechExpTablesConst.DBEntriesAttributeTypeID, (long) eTableCollection.Tables[0].TableType);
    this._impObjList.AddAttributeInt(TechExpTablesConst.DBColumnsAttributeTypeID, (long) eTableCollection.Tables[0].ValuesTable.ColumnsCount);
    this._impObjList.AddAttributeInt(TechExpTablesConst.DBRowsAttributeTypeID, (long) eTableCollection.Tables[0].ValuesTable.RowsCount);
    this._impObjList.AddAttributeInt(TechExpTablesConst.DBLayersAttributeTypeID, (long) eTableCollection.Tables.Length);
    eTable table1 = eTableCollection.Tables[0];
    List<CommonTypeHolder> comms = new List<CommonTypeHolder>();
    List<string> roles = new List<string>();
    foreach (eRow fixedRow in (IEnumerable<eRow>) table1.FixedRows)
    {
      if (fixedRow.Header != null)
        TablePump.AddCellToList(table1, fixedRow.Header, comms, roles, AttributeRoles.argHorz);
      foreach (eCell cell in fixedRow)
        TablePump.AddCellToList(table1, cell, comms, roles, AttributeRoles.argHorz);
    }
    foreach (eColumn fixedColumn in (IEnumerable<eColumn>) table1.FixedColumns)
    {
      if (fixedColumn.Header != null)
        TablePump.AddCellToList(table1, fixedColumn.Header, comms, roles, AttributeRoles.argVert);
      foreach (eCell cell in fixedColumn)
        TablePump.AddCellToList(table1, cell, comms, roles, AttributeRoles.argVert);
    }
    if (table1.TableType.Equals((object) eTableType.DoubleEntry))
    {
      foreach (eTable table2 in eTableCollection.Tables)
      {
        foreach (CommonTypeHolder commonTypeHolder in table2.Result)
        {
          comms.Add(commonTypeHolder);
          roles.Add(EnumTypeHelper.GetCaption((Enum) AttributeRoles.Result));
        }
      }
    }
    List<Guid> guidList1 = new List<Guid>();
    List<Guid> guidList2 = new List<Guid>();
    foreach (CommonTypeHolder commonTypeHolder in comms)
    {
      guidList1.Add(commonTypeHolder.ObjectType.Guid);
      guidList2.Add(commonTypeHolder.AttributeType.Guid);
    }
    for (int index = 0; index < roles.Count; ++index)
      this._impObjList.AddAttribute(TechExpTablesConst.DBRolesAttributeTypeID, AttrValueType.stringVal, (object) roles[index], index);
    for (int index = 0; index < guidList1.Count; ++index)
      this._impObjList.AddAttribute(TechExpTablesConst.DBObjTypesListAttributeTypeID, AttrValueType.stringVal, (object) guidList1[index].ToString(), index);
    for (int index = 0; index < guidList2.Count; ++index)
      this._impObjList.AddAttribute(TechExpTablesConst.DBAttrTypesListAttributeTypeID, AttrValueType.stringVal, (object) guidList2[index].ToString(), index);
    List<long> links = new List<long>();
    foreach (eTable table3 in eTableCollection.Tables)
    {
      foreach (eRow fixedRow in (IEnumerable<eRow>) table3.FixedRows)
      {
        foreach (eCell eCell in fixedRow)
          TablePump.AddLinkToList(eCell.CellValue, links);
      }
      foreach (eColumn fixedColumn in (IEnumerable<eColumn>) table3.FixedColumns)
      {
        foreach (eCell eCell in fixedColumn)
          TablePump.AddLinkToList(eCell.CellValue, links);
      }
      for (int row = 0; row < table3.ValuesTable.RowsCount; ++row)
      {
        for (int column = 0; column < table3.ValuesTable.ColumnsCount; ++column)
          TablePump.AddLinkToList(table3.ValuesTable[row, column].CellValue, links);
      }
    }
    for (int index = 0; index < links.Count; ++index)
      this._impObjList.AddAttribute(TechExpTablesConst.DBObjLinksListAttributeTypeID, AttrValueType.integerVal, (object) links[index], index);
    if (tableInfo.Cond != null && tableInfo.Cond.Data.Count > 0)
      this.SaveTableCondData(tableInfo);
    AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Imdi.UserSession, this._impObjList);
    return objectRecord1;
  }

  internal void SaveTableBodyData(List<eTable> tableList)
  {
    eTableCollection graph = new eTableCollection(tableList.ToArray());
    string tempFileName = Path.GetTempFileName();
    IPackedStream service = (IPackedStream) ServicesManager.ServiceContainer.GetService(typeof (IPackedStream));
    try
    {
      FileStream outStream = new FileStream(tempFileName, FileMode.Create);
      long length;
      try
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          new BinaryFormatter().Serialize((Stream) memoryStream, (object) graph);
          memoryStream.Position = 0L;
          service.PackStream((Stream) outStream, (Stream) memoryStream, 9);
          outStream.Flush();
          length = outStream.Length;
        }
      }
      finally
      {
        outStream.Close();
      }
      this._impObjList.AddAttributeBlob(length > (long) Intermech.Consts.MaxShortBlobSize ? TechExpTablesConst.DBBigDataAttributeTypeID : TechExpTablesConst.DBDataAttributeTypeID, tempFileName, length, string.Empty, ArcMethods.ZLibPacked);
    }
    finally
    {
      this._tmpFiles.Add(tempFileName);
    }
  }

  internal TempFormula SaveTableCondData(TableInfo tableInfo)
  {
    TempFormula ipsFormulaData = (TempFormula) null;
    if (tableInfo.Cond == null || tableInfo.Cond.Data.Count == 0)
      return (TempFormula) null;
    Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.FormulaPump formulaPump = new Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.FormulaPump(this.plugin);
    string comments = string.Empty;
    try
    {
      formulaPump._importingData = this._importingData;
      formulaPump.ConvertExpertData((short) 3, tableInfo.Cond.Data, tableInfo.Cond.ID, out ipsFormulaData);
      if (ipsFormulaData == null)
        this.plugin.appManager.AddWarningMessage($"Ошибка закачки условий (файл \"{this._tableFileName}\" таблица (Код = \"{tableInfo.Code}\" имя = \"{tableInfo.Name}\")");
    }
    catch (Exception ex)
    {
      switch (ex)
      {
        case TokenConvertException _:
        case CommonDataTypeCheckFailException _:
        case CommonDataTypeConvertException _:
        case EntitySettNotExistException _:
        case FormulaConvertException _:
          ipsFormulaData = (TempFormula) null;
          this.plugin.appManager.AddWarningMessage($"Ошибка закачки условий (файл \"{this._tableFileName}\" таблицы (Код = \"{tableInfo.Code}\" имя = \"{tableInfo.Name}\") (Сообщение: {ex.Message})");
          break;
        case FormulaCompileException _:
          string message = ex.Message;
          comments = message;
          this.plugin.appManager.AddWarningMessage(message);
          break;
        default:
          throw;
      }
    }
    if (ipsFormulaData == null)
      return (TempFormula) null;
    this.AddAttributeCondition(ExpertConsts.Consts.attrCondObj, ipsFormulaData);
    if (comments != string.Empty)
      this.AddAttributeComments(comments);
    return ipsFormulaData;
  }

  private static void AddCellToList(
    eTable table,
    eCell cell,
    List<CommonTypeHolder> comms,
    List<string> roles,
    AttributeRoles defRole)
  {
    if (cell == null || cell.CommonType == null || comms.Contains(cell.CommonType))
      return;
    switch (cell.CellDestination)
    {
      case eCellDestination.Header:
        comms.Add(cell.CommonType);
        roles.Add(EnumTypeHelper.GetCaption((Enum) defRole));
        break;
      case eCellDestination.Result:
        comms.Add(cell.CommonType);
        if (table.TableType.Equals((object) eTableType.NoEntry))
        {
          roles.Add(EnumTypeHelper.GetCaption((Enum) AttributeRoles.argResult));
          break;
        }
        roles.Add(EnumTypeHelper.GetCaption((Enum) AttributeRoles.Result));
        break;
    }
  }

  private static void AddLinkToList(ExpertValue expValue, List<long> links)
  {
    if (expValue == null)
      return;
    switch (expValue.ValueType)
    {
      case DataType.ObjectLink:
        long int64 = Convert.ToInt64(expValue.Value);
        if (links.Contains(int64))
          break;
        links.Add(int64);
        break;
      case DataType.Packet:
        if (!(expValue.Value is PacketValue packetValue))
          break;
        for (int index = 0; index < packetValue.Count; ++index)
          TablePump.AddLinkToList(packetValue[index], links);
        break;
    }
  }

  private void GetAttributeTypeItemParams(
    string parmCode,
    IAttributeTypeItem attrItem,
    out DataType dataType,
    out CommonTypeHolder cth)
  {
    FieldTypes attrType = attrItem != null ? (FieldTypes) attrItem.AttrValueType : throw new AttributeNotExistsException();
    dataType = attrType != FieldTypes.ftUnknown ? DataTypeConvertor.AttrType2DataType(attrType) : throw new CommonDataTypeCheckFailException($"Для атрибута \"{attrItem.Name}\" не определен тип значения");
    TechExpert.TypeConverter.CheckDataType(parmCode, attrItem, dataType);
    Guid guid = Guid.Empty;
    string objectTypeName = string.Empty;
    Entity entityByCode = TechExpert.TypeConverter.GetEntityByCode(parmCode);
    if (entityByCode != null)
    {
      if (entityByCode.Settings.ObjectType != Guid.Empty)
        guid = entityByCode.Settings.ObjectType;
      else if (entityByCode.RecordID != 0)
      {
        TechTypeInfo techTypeInfo;
        TechPumpData.TechType.TechTypeList.TryGetValue(entityByCode.RecordID, out techTypeInfo);
        TechTypeSett typeSett = techTypeInfo?.TypeSett;
        if (typeSett != null)
          guid = typeSett.ObjType;
      }
      if (guid != Guid.Empty)
      {
        IObjectTypeItem byGuid = this.plugin.Imdi.ObjectTypes.GetByGuid(guid);
        if (byGuid != null)
        {
          guid = byGuid.GUID;
          objectTypeName = byGuid.Name;
        }
      }
    }
    ObjectTypeHolder objectTypeHolder = new ObjectTypeHolder(guid, objectTypeName);
    AttributeTypeHolder attributeTypeHolder = new AttributeTypeHolder(attrItem.ID, TechcardConsts.Plugin.Idw.GetUserSession());
    cth = new CommonTypeHolder(objectTypeHolder, attributeTypeHolder);
  }

  private ExpertValue ParseHeaderData(
    string dataString,
    DataType dataType,
    eCellSymbol defSymbol,
    out eCellSymbol cellSymbol,
    string parmCode)
  {
    cellSymbol = eCellSymbol.None;
    if (dataString.Length.Equals(0))
      return (ExpertValue) null;
    string dataString1 = dataString;
    if (dataString.StartsWith("<"))
    {
      if (dataString.StartsWith("<="))
      {
        dataString1 = dataString.Substring(3);
        cellSymbol = eCellSymbol.LessOrEqual;
      }
      else
      {
        dataString1 = dataString.Substring(2);
        cellSymbol = eCellSymbol.Less;
      }
    }
    else if (dataString.StartsWith("="))
    {
      dataString1 = dataString.Substring(2);
      cellSymbol = eCellSymbol.Equal;
    }
    else if (dataString.StartsWith("<>"))
    {
      dataString1 = dataString.Substring(3);
      cellSymbol = eCellSymbol.NotEqual;
    }
    else if (dataString.StartsWith(">"))
    {
      if (dataString.StartsWith(">="))
      {
        dataString1 = dataString.Substring(3);
        cellSymbol = eCellSymbol.MoreOrEqual;
      }
      else
      {
        dataString1 = dataString.Substring(2);
        cellSymbol = eCellSymbol.More;
      }
    }
    else
    {
      if (dataString.StartsWith("{}"))
      {
        string dataString2 = dataString.Substring(3);
        cellSymbol = eCellSymbol.Set;
        return this.ParseHeaderDataPack(dataType, dataString2, parmCode);
      }
      if (dataString.StartsWith("+"))
      {
        dataString1 = string.Empty;
        cellSymbol = eCellSymbol.Other;
      }
      else if (dataString.Trim().Equals(string.Empty))
      {
        dataString1 = string.Empty;
        cellSymbol = eCellSymbol.None;
      }
    }
    if (!cellSymbol.Equals((object) eCellSymbol.None))
    {
      ExpertValue headerData;
      switch (cellSymbol)
      {
        case eCellSymbol.None:
        case eCellSymbol.Other:
          headerData = ExpertValue.Empty(dataType);
          break;
        default:
          headerData = this.UnpackValue(dataType, dataString1, parmCode);
          break;
      }
      return headerData;
    }
    return defSymbol.Equals((object) eCellSymbol.Set) ? this.ParseHeaderDataPack(dataType, dataString, parmCode) : this.UnpackValue(dataType, dataString, parmCode);
  }

  private ExpertValue ParseHeaderDataPack(DataType dataType, string dataString, string parmCode)
  {
    string[] strArray1 = dataString.Split(',');
    PacketValue packetValue = new PacketValue();
    foreach (string dataString1 in strArray1)
    {
      if (dataString1.Contains(":"))
      {
        DiapValue diapValue = new DiapValue();
        string[] strArray2 = dataString1.Split(':');
        diapValue.Low = this.UnpackValue(dataType, strArray2[0], parmCode);
        diapValue.High = this.UnpackValue(dataType, strArray2[1], parmCode);
        packetValue.Add(new ExpertValue(diapValue));
      }
      else
        packetValue.Add(this.UnpackValue(dataType, dataString1, parmCode));
    }
    return new ExpertValue(packetValue);
  }

  private ExpertValue UnpackValue(DataType dataType, string dataString, string parmCode)
  {
    ExpertValue expertValue = (ExpertValue) null;
    object obj = (object) null;
    if (dataString.Length.Equals(0))
      return (ExpertValue) null;
    try
    {
      switch (dataType)
      {
        case DataType.Integer:
          int intValue;
          if (!DataConvertor.ConvertStrToInt(dataString, out intValue))
            throw new CommonDataTypeConvertException();
          obj = (object) intValue;
          break;
        case DataType.Float:
          double dblValue;
          if (!DataConvertor.ConvertStrToDouble(dataString, out dblValue))
            throw new CommonDataTypeConvertException();
          obj = (object) dblValue;
          break;
        case DataType.Measured:
          obj = (object) this.UnkPackMeasuredValue(dataType, dataString, parmCode);
          break;
        case DataType.String:
          obj = (object) dataString;
          break;
        case DataType.Date:
          DateTime result1;
          if (!DateTime.TryParse(dataString, out result1))
            throw new CommonDataTypeConvertException();
          obj = (object) result1.ToUniversalTime();
          break;
        case DataType.Boolean:
          bool result2;
          if (!bool.TryParse(dataString, out result2))
            throw new CommonDataTypeConvertException();
          obj = (object) result2;
          break;
        case DataType.ObjectLink:
          obj = (object) this.UnkPackObjLinkValue(dataType, dataString, parmCode);
          break;
      }
    }
    catch (Exception ex)
    {
      string str = ex.GetType().ToString();
      if (ex.Message != string.Empty && ex.Message.IndexOf(str, StringComparison.Ordinal) == -1)
        throw;
      if (ex is CommonDataTypeConvertException)
        throw new CommonDataTypeConvertException($"Невозможно привести понятие \"{parmCode}\"=\"{dataString}\"  к типу данных \"{EnumTypeHelper.GetCaption((Enum) dataType)}\"");
      throw;
    }
    if (obj != null)
      expertValue = new ExpertValue(dataType, obj);
    return expertValue;
  }

  private MeasuredValue UnkPackMeasuredValue(DataType dataType, string dataString, string parmCode)
  {
    if (parmCode == string.Empty)
      throw new AttributeNotExistsException();
    double dblValue;
    if (!DataConvertor.ConvertStrToDouble(dataString, out dblValue))
      throw new MeasureTypeConvertException();
    MeasuredValue measuredValue;
    TechExpert.DataConverter.ConvertValue2Measured(TechExpert.TypeConverter.GetEntityByCode(parmCode), dblValue, 0, out measuredValue, true);
    return measuredValue;
  }

  private long UnkPackObjLinkValue(DataType dataType, string dataString, string parmCode)
  {
    Entity entity = !(parmCode == string.Empty) ? TechExpert.TypeConverter.GetEntityByCode(parmCode) : throw new AttributeNotExistsException();
    int intValue;
    if (!DataConvertor.ConvertStrToInt(dataString, out intValue))
    {
      long num = TechExpert.DataConverter.ConvertImbaseCode2ObjectLink(entity, dataString, this._importingData);
      return num != 0L ? num : throw new ObjectLinkTypeConvertException();
    }
    long num1;
    try
    {
      num1 = TechExpert.DataConverter.ConvertValue2ObjectLink(entity, intValue, this._importingData);
    }
    catch (Exception ex)
    {
      if (ex is ObjectLinkTypeConvertException)
      {
        num1 = 0L;
        this.plugin.appManager.AddWarningMessage(ex.Message);
      }
      else
        throw;
    }
    return num1;
  }
}
