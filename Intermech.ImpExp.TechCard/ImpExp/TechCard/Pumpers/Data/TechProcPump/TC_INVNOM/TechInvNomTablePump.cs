// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TC_INVNOM.TechInvNomTablePump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Imbase;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechProcPump.TC_INVNOM;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TC_INVNOM;

[TaskDescription("Инициализация данных для перекачки - Таблицы инвентарных номеров", "Перекачка данных - Таблицы инвентарных номеров")]
[TaskType(PumperType.MetaData)]
internal class TechInvNomTablePump : TechPumpBase
{
  private readonly Guid _guid = new Guid("{61B43443-1469-49F8-82E0-B98D823CE2E7}");
  private int _imbaseEqCatalodId;
  private int _imbaseEqCatalogLevelCode;
  private string _field4Model = string.Empty;
  private readonly List<InvNomStructRec> _structList = new List<InvNomStructRec>();
  private DataTable _imbaseTableAttributeTemplate;
  private DataTable _imbaseTableDataTemplate;
  private readonly IDictionary<int, string> _model2NameCache = (IDictionary<int, string>) new Dictionary<int, string>();
  private readonly IDictionary<string, IList<TechObjectRecord>> _model2tableRecordList = (IDictionary<string, IList<TechObjectRecord>>) new Dictionary<string, IList<TechObjectRecord>>();
  private IDictionary<string, IMSAttributeType> _field2ImsAttributeCache = (IDictionary<string, IMSAttributeType>) new Dictionary<string, IMSAttributeType>();
  private ISelectionsService _selectionService;
  private static readonly string TypeName = "Инвентарные номера";

  protected override Guid GUID => this._guid;

  private void CreateImbaseTableTemplate()
  {
    this._imbaseTableAttributeTemplate = ImbaseImpHelper.GetTableAttributes();
    this._imbaseTableDataTemplate = ImbaseImpHelper.GetTableData();
    List<InvNomStructRec> list = this._structList.Where<InvNomStructRec>((System.Func<InvNomStructRec, bool>) (item =>
    {
      if (item.DataType == -101)
        return true;
      return item.DataType > 0 && item.DataType < 10;
    })).ToList<InvNomStructRec>();
    foreach (InvNomStructRec invNomStructRec in list)
    {
      InvNomStructRec dataField = invNomStructRec;
      if (dataField.Sort == 0)
        dataField.Sort = this._structList.Where<InvNomStructRec>((System.Func<InvNomStructRec, bool>) (item => item.KeyField == dataField.FieldName)).FindMin<InvNomStructRec, int, int>((System.Func<InvNomStructRec, int>) (item => item.Sort), (System.Func<InvNomStructRec, int>) (item => item.Sort));
    }
    ImbaseImpHelper.GetTableAttributes();
    foreach (InvNomStructRec invNomStructRec in (IEnumerable<InvNomStructRec>) list.OrderBy<InvNomStructRec, int>((System.Func<InvNomStructRec, int>) (item => item.Sort)))
    {
      Entity tcInvnomFieldName = this.GetEntityByTC_INVNOM_FieldName(invNomStructRec.FieldName);
      Guid attrTypeGuid;
      if (tcInvnomFieldName != null && TechcardConsts.TechcardCommon.Code2AttributeGuid.TryGetValue(tcInvnomFieldName.Code, out attrTypeGuid) && !(attrTypeGuid == Guid.Empty))
      {
        IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attrTypeGuid);
        if (attributeType != null)
        {
          this._field2ImsAttributeCache[invNomStructRec.FieldName] = attributeType;
          DataRow row = this._imbaseTableAttributeTemplate.NewRow();
          row["F_ATTRIBUTE_GUID"] = (object) attrTypeGuid;
          row["F_REQUIRED"] = (object) 2;
          row["F_COMPUTED"] = (object) 0;
          row["F_FORMULA"] = (object) string.Empty;
          row["F_UNIQUE"] = (object) 0;
          row["F_DEFAULT_VALUE"] = (object) invNomStructRec.Data;
          int num = 0;
          if (invNomStructRec.Stat == InvNomStructRec.Status.ReadOnly)
            num |= 128 /*0x80*/;
          switch (invNomStructRec.DataType)
          {
            case 1:
            case 2:
            case 3:
              if (invNomStructRec.Flag.HasFlag((Enum) InvNomStructRec.Flags.NotEmpty))
              {
                num |= 8;
                break;
              }
              break;
            case 4:
              if (invNomStructRec.Flag.HasFlag((Enum) InvNomStructRec.Flags.BoolYesNo) || !invNomStructRec.Flag.HasFlag((Enum) InvNomStructRec.Flags.BoolTrueFalse))
                break;
              break;
            case 7:
              invNomStructRec.Flag.HasFlag((Enum) InvNomStructRec.Flags.FullDateFormat);
              break;
          }
          row["F_OPTIONS"] = (object) num;
          row["F_MASK"] = (object) string.Empty;
          row["F_DISPLAY"] = (object) invNomStructRec.Name;
          this._imbaseTableAttributeTemplate.Rows.Add(row);
          this._imbaseTableDataTemplate.Columns.Add(new DataColumn(attrTypeGuid.ToString(), Intermech.Imbase.ImbaseHelper.AttTypeToType(attributeType.FieldType)));
        }
      }
    }
    this._imbaseTableAttributeTemplate.AcceptChanges();
    this._imbaseTableDataTemplate.AcceptChanges();
  }

  private void CreateImbaseObjects(IList<TechObjectRecord> tableRecords)
  {
    if (!tableRecords.Any<TechObjectRecord>())
      return;
    ObjectRecord imbaseTableObject = this.CreateImbaseTableObject(tableRecords);
    if (imbaseTableObject == null)
      return;
    ObjectRecord imbaseLinkObject = this.CreateImbaseLinkObject(imbaseTableObject, tableRecords);
    if (imbaseLinkObject == null)
      return;
    this.CreateImbaseRelationWithObjectLink(imbaseLinkObject, tableRecords);
  }

  private ObjectRecord CreateImbaseLinkObject(
    ObjectRecord importedTableRecord,
    IList<TechObjectRecord> tableRecords)
  {
    ObjectRecord imbaseLinkObject1 = (ObjectRecord) null;
    if (importedTableRecord == null || importedTableRecord.Object_id == 0L || !tableRecords.Any<TechObjectRecord>())
      return imbaseLinkObject1;
    int int32Value = DataSetProcessor.GetInt32Value(tableRecords[0].GetFieldValue(this._field4Model), 0);
    string str1;
    this._model2NameCache.TryGetValue(int32Value, out str1);
    string str2 = str1;
    string attrVal = $"Инвентарные номера для модели \"{str1}\"";
    int owner = 0;
    IImportedObjectList impObjList = this._impObjList;
    impObjList.AddObject(ImbaseIDHelper.ObjTypeIdImTabLink, owner, str2);
    impObjList.AddAttribute(ImbaseIDHelper.AttrIdName, AttrValueType.stringVal, (object) str2, 0);
    impObjList.AddAttribute(ImbaseIDHelper.AttrIdDescription, AttrValueType.stringVal, (object) attrVal, 0);
    impObjList.AddAttributeLink(ImbaseIDHelper.AttrIdImLinkTable, importedTableRecord.Object_id, importedTableRecord.Caption, 0);
    impObjList.AddAttributeInt(this._atTechTypeKeyAttr.AttrValueType, (long) int32Value);
    impObjList.AddAttributeStr(Intermech.Imbase.Consts.CreatedObjectAttID, TechCardConsts.ObjectTypes.OborudGUID.ToString());
    impObjList.AddAttributeInt(Intermech.Imbase.Consts.CreateNewObjectAttID, 0L);
    long oldKey = TechcardConsts.Utils.CodeHashCode(this._imbaseEqCatalodId, int32Value != 0 ? int32Value : this._imbaseEqCatalogLevelCode);
    DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.ImbaseFoldersClassificators, (object) oldKey);
    string parentKey = dictionaryValue != null ? Convert.ToString(dictionaryValue.Caption) : string.Empty;
    if (string.IsNullOrEmpty(parentKey))
    {
      if (dictionaryValue == null)
        dictionaryValue = this._import_data_main.GetValue(ImportingCategory.ImbaseFoldersGuids, (object) oldKey);
      if (dictionaryValue != null)
        parentKey = this.plugin.Idw.GetUserSession().GetObjectAttribute(dictionaryValue.NewObjectID, (object) ImbaseIDHelper.AttrIdClassifierKey, false, false)?.AsString ?? string.Empty;
    }
    if (!string.IsNullOrEmpty(parentKey))
    {
      string nextClassifierKey = this._selectionService.GenerateNextClassifierKey((object) this.plugin.Idw.GetUserSession(), ImbaseIDHelper.ObjTypeIdImCtl, parentKey, ImbaseIDHelper.ObjTypeIdImFolder);
      impObjList.AddAttributeStr(ImbaseIDHelper.AttrIdClassifierKey, nextClassifierKey);
    }
    AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Idw.GetUserSession(), impObjList);
    ObjectRecord imbaseLinkObject2 = impObjList.Items[0].Object;
    impObjList.Import();
    impObjList.Items.Clear();
    if (this._import_data_main.GetValue(ImportingCategory.TechInvNomTablePump, (object) int32Value) == null)
      this._import_data_main.AddValue(ImportingCategory.TechInvNomTablePump, (object) int32Value, imbaseLinkObject2.Object_id);
    else
      this._import_data_main.SetNewKey(ImportingCategory.TechInvNomTablePump, (object) int32Value, imbaseLinkObject2.Object_id);
    return imbaseLinkObject2;
  }

  private void CreateImbaseRelationWithObjectLink(
    ObjectRecord importedLinkRecord,
    IList<TechObjectRecord> tableRecords)
  {
    if (importedLinkRecord == null || importedLinkRecord.Object_id == 0L)
      return;
    int recKey = DataSetProcessor.GetInt32Value(tableRecords[0].GetFieldValue(this._field4Model), 0);
    if (recKey == 0)
      recKey = this._imbaseEqCatalogLevelCode;
    DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.ImbaseFoldersGuids, (object) TechcardConsts.Utils.CodeHashCode(this._imbaseEqCatalodId, recKey));
    if (dictionaryValue == null || dictionaryValue.NewObjectID == 0L)
    {
      string message = $"Ошибка создание связи - не найдена ссылка на папку Imbase (каталог {this._imbaseEqCatalodId}, папка F_LEVEL={recKey})";
      TechcardConsts.Plugin.appManager.AddNewWarningMessage(message);
    }
    else
    {
      this._impRelList.AddRelation(dictionaryValue.NewObjectID, importedLinkRecord.Object_id, TechCardConsts.RelTypes.SortedRelationID);
      AttributesHelper.AddObligatoryRelationAttributes(this.plugin.Idw, this._impRelList);
    }
  }

  private ObjectRecord CreateImbaseTableObject(IList<TechObjectRecord> tableRecords)
  {
    ObjectRecord imbaseTableObject1 = (ObjectRecord) null;
    if (!tableRecords.Any<TechObjectRecord>())
      return imbaseTableObject1;
    int int32Value = DataSetProcessor.GetInt32Value(tableRecords[0].GetFieldValue(this._field4Model), 0);
    string tableName = $"tbl_inv.modelcode_{int32Value}";
    string str1;
    this._model2NameCache.TryGetValue(int32Value, out str1);
    string str2 = $"Инв. ном. {str1}";
    string attrVal = $"Таблица инвентарных номеров для модели \"{str1}\"";
    DictionaryValue dictionaryValue;
    if ((dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechInvNomTablePump, (object) int32Value)) != null)
    {
      string message = $"{attrVal} исключена из импорта. Таблица была импортирована ранее (ObjectId = {dictionaryValue.NewObjectID})";
      TechcardConsts.Plugin.appManager.AddNewInfoMessage(message);
      return imbaseTableObject1;
    }
    int owner = 0;
    IImportedObjectList impObjList = this._impObjList;
    impObjList.AddObject(ImbaseIDHelper.ObjTypeIdImTab, owner, str2);
    impObjList.AddAttribute(ImbaseIDHelper.AttrIdName, AttrValueType.stringVal, (object) str2, 0);
    impObjList.AddAttribute(ImbaseIDHelper.AttrIdDescription, AttrValueType.stringVal, (object) attrVal, 0);
    impObjList.AddAttributeStr(ImbaseIDHelper.AttrIdTableName, tableName);
    impObjList.AddAttributeInt(this._atTechTypeKeyAttr.AttrValueType, (long) int32Value);
    this.CreateImbaseTableBlob(tableRecords, tableName);
    AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Idw.GetUserSession(), impObjList);
    ObjectRecord imbaseTableObject2 = impObjList.Items[0].Object;
    impObjList.Import();
    impObjList.Items.Clear();
    return imbaseTableObject2;
  }

  private void CreateImbaseTableBlob(IList<TechObjectRecord> tableRecords, string tableName)
  {
    if (!tableRecords.Any<TechObjectRecord>())
      return;
    int num = 0;
    DataSet graph = new DataSet("IMS_TABLE_RECORDS");
    DataTable dataTable1 = this._imbaseTableAttributeTemplate.Copy();
    DataTable dataTable2 = this._imbaseTableDataTemplate.Copy();
    Guid empty = Guid.Empty;
    List<Guid> guidList = new List<Guid>();
    foreach (TechObjectRecord tableRecord in (IEnumerable<TechObjectRecord>) tableRecords)
    {
      DataRow row = dataTable2.NewRow();
      Guid guid = Guid.NewGuid();
      this._techParmList.Clear();
      foreach (string key in (IEnumerable<string>) tableRecord.Fields.Keys)
      {
        string fieldName = key;
        Entity tcInvnomFieldName = this.GetEntityByTC_INVNOM_FieldName(fieldName);
        IMSAttributeType imsAttributeType;
        if (tcInvnomFieldName != null && this._field2ImsAttributeCache.TryGetValue(fieldName, out imsAttributeType))
        {
          object obj = tableRecord.GetFieldValue(fieldName);
          Type dataType = dataTable2.Columns[imsAttributeType.AttributeGuid.ToString()].DataType;
          if (obj != null)
          {
            this._techParmList.AddEntity(tcInvnomFieldName.Code, obj);
            if (imsAttributeType.FieldType == FieldTypes.ftObjectLink)
            {
              int result;
              if (int.TryParse(Convert.ToString(obj), out result))
              {
                if (result == 0)
                {
                  obj = (object) string.Empty;
                }
                else
                {
                  InvNomStructRec invNomStructRec = this._structList.First<InvNomStructRec>((System.Func<InvNomStructRec, bool>) (item => item.FieldName == fieldName));
                  string caption = this._import_data_main.GetCaption(ImportingCategory.ImbaseFoldersGuids, (object) TechcardConsts.Utils.CodeHashCode(invNomStructRec.TableId, result));
                  if (!string.IsNullOrEmpty(caption) && GuidHelper.IsGuid(caption))
                  {
                    obj = (object) caption;
                  }
                  else
                  {
                    string message = $"Не найдена ссылка на папку Imbase (каталог {invNomStructRec.TableId}, папка F_LEVEL={result}) для записи таблицы инв. номеров {tableRecord.Key}";
                    TechcardConsts.Plugin.appManager.AddNewWarningMessage(message);
                  }
                }
              }
            }
            else if (this._entityConverter != null)
            {
              ITechParamAttribute techParamAttribute = this._entityConverter.Convert((TechObjectRecordBase) tableRecord, this._techParmList, (ITechParamEntity) new TechParamEntity(tcInvnomFieldName.Code, obj), tcInvnomFieldName);
              if (techParamAttribute != null)
                obj = techParamAttribute.Value;
            }
            else if (obj.GetType() == typeof (string) && AttributesHelper.IsNumericType(dataType))
              obj = CompareValuesHelper.NormalizedValue(obj) != null ? (object) Convert.ToString(obj).Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) : (object) 0;
            this.SetValueToRow(row, imsAttributeType.AttributeGuid, obj, dataType, tableName);
          }
        }
      }
      row["F_GUID"] = (object) guid;
      row["F_KEY"] = (object) tableRecord.Key;
      dataTable2.Rows.Add(row);
      if (tableRecord.Key >= num)
        num = tableRecord.Key + 1;
      dataTable2.AcceptChanges();
      dataTable2.Columns["F_KEY"].AutoIncrementSeed = (long) num;
    }
    graph.Tables.AddRange(new DataTable[2]
    {
      dataTable1,
      dataTable2
    });
    if (graph.Tables == null || graph.Tables.Count <= 0)
      return;
    string tmpFileName = this.GetTmpFileName();
    long fileSize = 0;
    using (MemoryStream memoryStream = new MemoryStream())
    {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      graph.RemotingFormat = SerializationFormat.Binary;
      FileStream outStream = new FileStream(tmpFileName, FileMode.Create, FileAccess.Write);
      try
      {
        binaryFormatter.Serialize((Stream) memoryStream, (object) graph);
        memoryStream.Position = 0L;
        new PackedStream().PackStream((Stream) outStream, (Stream) memoryStream, 9);
      }
      finally
      {
        outStream.Flush();
        fileSize = outStream.Length;
        outStream.Close();
      }
    }
    this._impObjList.AddAttributeBlob(ImbaseIDHelper.AttrTableDataLength < fileSize ? ImbaseIDHelper.AttrLongTableData : ImbaseIDHelper.AttrTableData, tmpFileName, fileSize, $"Записи таблицы инв. номеров {tableName}", ArcMethods.ZLibPacked);
    if (!(ServicesManager.GetService(typeof (ILogFile)) is ILogFile service))
      return;
    service.WriteMessage($"inv_table:{tableName} file:{tmpFileName} size:{fileSize}");
  }

  private void SetValueToRow(
    DataRow row,
    Guid nameKey,
    object value,
    Type type,
    string tableName)
  {
    try
    {
      if (type != typeof (string) && CompareValuesHelper.NormalizedValue(value) == null)
        row[nameKey.ToString()] = (object) DBNull.Value;
      else
        row[nameKey.ToString()] = Convert.ChangeType(value, type);
    }
    catch (FormatException ex1)
    {
      if (AttributesHelper.IsNumericType(type))
      {
        char[] chArray = new char[10]
        {
          '0',
          '1',
          '2',
          '3',
          '4',
          '5',
          '6',
          '7',
          '8',
          '9'
        };
        string str = Convert.ToString(value);
        bool flag = false;
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < str.Length; ++index)
        {
          if (chArray.Equals((object) str[index]))
            stringBuilder.Append(str[index]);
          else if (Convert.ToString(str[index]).Equals(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) && !flag)
          {
            flag = true;
            stringBuilder.Append(str[index]);
          }
        }
        value = stringBuilder.Length <= 0 ? (object) 0 : (object) stringBuilder.ToString();
        try
        {
          row[nameKey.ToString()] = Convert.ChangeType(value, type);
        }
        catch (FormatException ex2)
        {
          this.plugin.appManager.AddWarningMessage($"Ошибка при приведении обработанного значения \"{value}\" к типу {type} в колонке {nameKey} таблицы {tableName} : {ex2.Message}");
        }
      }
      else
        this.plugin.appManager.AddWarningMessage($"Ошибка при приведении значения \"{value}\" к типу {type} в колонке {nameKey} таблицы {tableName} : {ex1.Message}");
    }
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic("TC_INVNOM");
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override string GetRecordWithParamsPumpMode(TechObjectRecord record)
  {
    string withParamsPumpMode = base.GetRecordWithParamsPumpMode(record);
    if (record.RecMode == TechObjectRecord.PumpMode.LinkOnly)
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
    record.RecMode = TechObjectRecord.PumpMode.NotPump;
    string key = Convert.ToString(record.GetFieldValue(this._field4Model));
    IList<TechObjectRecord> techObjectRecordList;
    if (!this._model2tableRecordList.TryGetValue(key, out techObjectRecordList))
    {
      techObjectRecordList = (IList<TechObjectRecord>) new List<TechObjectRecord>();
      this._model2tableRecordList.Add(key, techObjectRecordList);
    }
    techObjectRecordList.Add(record);
    return withParamsPumpMode;
  }

  protected override TechDataSource GetDataSource()
  {
    TechDataSource dataSource = this._dataSource;
    if (dataSource != null)
      return dataSource;
    TechDataBuilderSimple<TechInvNomTablePump> dataBuilder = new TechDataBuilderSimple<TechInvNomTablePump>(this);
    dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) => string.Empty);
    return this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
  }

  protected override void InitData()
  {
    this._recType = TechInvNomTablePump.TypeName;
    this._recTypeID = -2;
    this.objTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otInstrumentationObjTypeGuid).ID;
    this._tableName = "TC_INVNOM";
    if (TechPumpData.TechType.TechTypeList.ContainsKey(this._recTypeID))
      return;
    TechTypeInfo techTypeInfo = new TechTypeInfo()
    {
      RecordID = this._recTypeID,
      Name = this._recType,
      Type = "-2"
    };
    techTypeInfo.TypeSett = new TechTypeSett()
    {
      ObjType = TechcardConsts.TypeConsts.otInstrumentationObjTypeGuid,
      Mode = TechTypePumpMode.ExistObjType
    };
    TechPumpData.TechType.TechTypeList.Add(this._recTypeID, techTypeInfo);
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechInvNomTablePump;

  protected override ImportingCategory GetTechUniqueCategory() => ImportingCategory.None;

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    return new List<TechRelParam>();
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[2]
    {
      ImportingCategory.ImbaseFoldersGuids,
      ImportingCategory.ImbaseFoldersClassificators
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.ImbaseFoldersGuids
    };
  }

  private Entity GetEntityByTC_INVNOM_FieldName(string fieldName)
  {
    Entity tcInvnomFieldName;
    TechPumpData.Entities.EntitiesList.TryGetValue(InvNomStructRec.GenerateEntityName(fieldName), out tcInvnomFieldName);
    return tcInvnomFieldName;
  }

  public override void LoadTechParams(TechObjectRecord record)
  {
    this._techParmList.Clear();
    if (record == null)
      return;
    foreach (string key in (IEnumerable<string>) record.Fields.Keys)
    {
      Entity tcInvnomFieldName = this.GetEntityByTC_INVNOM_FieldName(key);
      if (tcInvnomFieldName != null)
      {
        object fieldValue = record.GetFieldValue(key);
        if (fieldValue != null)
          this._techParmList.AddEntity(tcInvnomFieldName.Code, fieldValue);
      }
    }
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    object obj1 = (object) null;
    object obj2 = (object) null;
    if (obj1 != null || obj2 != null)
    {
      string str = obj2 != null ? obj2.ToString() : string.Empty;
      if (obj1 != null)
        str = $"{str} Инв.N {obj1}";
      objRecord.Caption = str.Truncate(Intermech.Consts.MaxStringSize - 2);
    }
    base.FillTechObject(objRecord, record);
  }

  private Entity GetEntityByInvNomStructRec(InvNomStructRec rec)
  {
    Entity byInvNomStructRec = new Entity(InvNomStructRec.GenerateEntityName(rec.FieldName), rec.Name, this._recType, this._recTypeID);
    byInvNomStructRec.IsPermisibleAttr2TypeObj = true;
    byInvNomStructRec.Type = rec.TypeString;
    EntityReference entityReference = new EntityReference();
    entityReference.Reference = rec.TableId;
    entityReference.Field = rec.ImbaseRecId;
    entityReference.Code = byInvNomStructRec.Code;
    entityReference.MasterCode = byInvNomStructRec.Code;
    byInvNomStructRec.EntityReference = entityReference;
    Entity entity1;
    if (!string.IsNullOrEmpty(rec.Entity) && TechPumpData.Entities.EntitiesList.TryGetValue(rec.Entity, out entity1))
    {
      byInvNomStructRec.Settings.PumpTo = (object) entity1;
      byInvNomStructRec.Settings.PumpMode = EntityPumModes.ExistEntity;
    }
    switch (rec.DataType)
    {
      case -101:
        byInvNomStructRec.IsMasterAttr = true;
        byInvNomStructRec.Type = "I";
        byInvNomStructRec.InitializeSetting((IEnumerable<Entity>) TechPumpData.Entities.EntitiesList.Values);
        byInvNomStructRec.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
        break;
      case 111:
      case 121:
      case 131:
        if (rec.DataType == 111 && entityReference.Field == 0)
          entityReference.Field = -1;
        string keyField = rec.KeyField;
        entityReference.MasterCode = InvNomStructRec.GenerateEntityName(keyField);
        Entity entity2;
        if (TechPumpData.Entities.EntitiesList.TryGetValue(entityReference.MasterCode, out entity2))
        {
          if (!entity2.IsMasterAttr)
          {
            entity2.IsMasterAttr = true;
            entity2.Type = "I";
          }
          if (entity2.Settings.Properties.FieldType != FieldTypes.ftObjectLink)
          {
            entity2.InitializeSetting((IEnumerable<Entity>) TechPumpData.Entities.EntitiesList.Values);
            entity2.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
            break;
          }
          break;
        }
        break;
    }
    byInvNomStructRec.InitializeSetting((IEnumerable<Entity>) TechPumpData.Entities.EntitiesList.Values);
    return byInvNomStructRec;
  }

  private void LoadCatalogStruct()
  {
    int sqlRecordsCount = this.GetSqlRecordsCount($"select count(*) from {"TC_INVNOMSTRUCT"}");
    this.ExamCheckPoint("Структуры инструментальных номеров", 0);
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      int val1 = 0;
      EntityTypeRec entityTypeRec;
      if (!TechPumpData.EntTypeList.TryGetValue(this._recTypeID, out entityTypeRec))
      {
        entityTypeRec = new EntityTypeRec();
        TechPumpData.EntTypeList.Add(this._recTypeID, entityTypeRec);
      }
      command.CommandText = string.Format("SELECT DISTINCT \r\n                                                    a.*, \r\n                                                    b.{1} \r\n                                                  FROM \r\n                                                    {0} a left join {2} b \r\n                                                    on a.{3} = b.{4} \r\n                                                  ORDER BY \r\n                                                    {5} DESC", (object) "TC_INVNOMSTRUCT", (object) "F_TABLE", (object) "IM_TABLES", (object) "F_LU_TABLE_ID", (object) "F_KEY", (object) "F_KEYFIELD");
      int modelCatalogId = TechInvNomPump.GetModelCatalogId();
      using (IDataReader dbReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        this._structList.Clear();
        while (dbReader.Read())
        {
          ++val1;
          if (val1 % 100 == 0)
            this.ExamCheckPoint($"Получение структуры инструментальных номеров {val1} из {sqlRecordsCount}", this.CalculatePercent(sqlRecordsCount, Math.Min(val1, sqlRecordsCount), 2));
          if (!dbReader.IsDBNull(dbReader.GetOrdinal("F_KEYFIELD")))
          {
            dbReader.GetString(dbReader.GetOrdinal("F_KEYFIELD"));
          }
          else
          {
            string empty = string.Empty;
          }
          int int32_1 = BasePumpHelper.ToInt32(dbReader[dbReader.GetOrdinal("F_LU_TABLE_ID")]);
          int int32_2 = BasePumpHelper.ToInt32(dbReader[dbReader.GetOrdinal("F_DATATYPE")]);
          string str1 = dbReader.IsDBNull(dbReader.GetOrdinal("F_TABLE")) ? string.Empty : dbReader.GetString(dbReader.GetOrdinal("F_TABLE"));
          string str2 = dbReader.GetString(dbReader.GetOrdinal("F_FIELDNAME"));
          if (int32_1 > 0 && str1.Equals(string.Empty))
          {
            this.plugin.appManager.AddWarningMessage($"Ошибка получение структуры инструментального номера F_FIELDNAME=\"{str2}\". Имя таблицы не найдено в таблице IM_TABLES");
          }
          else
          {
            InvNomStructRec rec = new InvNomStructRec(dbReader);
            this._structList.Add(rec);
            if (modelCatalogId != 0 && int32_1 == modelCatalogId && int32_2 == -101)
              this._field4Model = str2;
            Entity byInvNomStructRec = this.GetEntityByInvNomStructRec(rec);
            if (!TechPumpData.Entities.EntitiesList.ContainsKey(byInvNomStructRec.Code))
              TechPumpData.Entities.EntitiesList.Add(byInvNomStructRec.Code, byInvNomStructRec);
            entityTypeRec.AddEntity(byInvNomStructRec);
          }
        }
        dbReader.Close();
      }
    }
    this.ExamCheckPoint("Получение структуры инструментальных номеров завершено", 100);
  }

  private void LoadModelInfo()
  {
    int modelCatalogId = TechInvNomPump.GetModelCatalogId();
    string str1;
    if (modelCatalogId == 0)
    {
      str1 = "TC_OBORUD";
    }
    else
    {
      str1 = TechPumpData.Tables.ImTablesData.GetTableName(modelCatalogId);
      this._imbaseEqCatalodId = modelCatalogId;
    }
    this.ExamCheckPoint("Загрузка информации по кодам оборудования", 0);
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      command.CommandText = string.Format("SELECT \r\n                                                    {0}, \r\n                                                    {1},\r\n                                                    {2}     \r\n                                                  FROM \r\n                                                    {3} \r\n                                                  ORDER BY \r\n                                                    {0} ", (object) "F_LEVEL", (object) "F_NAME", (object) "F_OWNER", (object) str1);
      using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        int ordinal1 = dataReader.GetOrdinal("F_LEVEL");
        int ordinal2 = dataReader.GetOrdinal("F_OWNER");
        int ordinal3 = dataReader.GetOrdinal("F_NAME");
        while (dataReader.Read())
        {
          int int32_1 = BasePumpHelper.ToInt32(dataReader[ordinal1]);
          int int32_2 = BasePumpHelper.ToInt32(dataReader[ordinal2]);
          string str2 = Convert.ToString(dataReader[ordinal3]);
          if (int32_2 == 0)
            this._imbaseEqCatalogLevelCode = int32_1;
          this._model2NameCache[int32_1] = str2;
        }
        dataReader.Close();
      }
    }
    this.ExamCheckPoint("Загрузка информации по кодам оборудования завершена", 100);
  }

  protected override void LoadMetaData4StoppedPump()
  {
    this.LoadCatalogStruct();
    this.LoadModelInfo();
    base.LoadMetaData4StoppedPump();
  }

  public TechInvNomTablePump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
  }

  protected override void AnalyzeStoppedData()
  {
  }

  protected override void ReleasePumpData()
  {
    base.ReleasePumpData();
    this._structList.Clear();
  }

  protected override void LoadMetaData4Pump()
  {
    ImbaseIDHelper.Initialize(this.Plugin.Imdi);
    base.LoadMetaData4Pump();
  }

  protected override void LoadEntityMetaData()
  {
    this.CreateImbaseTableTemplate();
    base.LoadEntityMetaData();
  }

  protected override void PumpLoadData()
  {
    base.PumpLoadData();
    foreach (IList<TechObjectRecord> tableRecords in (IEnumerable<IList<TechObjectRecord>>) this._model2tableRecordList.Values)
      this.CreateImbaseObjects(tableRecords);
  }

  public override void Pump()
  {
    this._selectionService = ServiceUtils.GetService<ISelectionsService>((object) this.plugin.Idw.GetUserSession(), true);
    base.Pump();
  }

  public override void Exam()
  {
    this.LoadCatalogStruct();
    this.LoadModelInfo();
    base.Exam();
  }
}
