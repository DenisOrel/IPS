// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.FormulaPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common;
using Intermech.ImpExp.TechCard.TechExpPump.Common;
using Intermech.ImpExp.TechCard.TechExpPump.TablesPump;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Expert;
using Intermech.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

[TaskDescription("Инициализация перекачки формул экспертной системы", "Перекачка формул экспертной системы")]
[TaskType(PumperType.MetaData)]
internal class FormulaPump : TechExpFolderSupportPump
{
  private const int ReadPacketSize = 100;
  private readonly Guid _guid = new Guid("{4D9DAE33-1C05-4d8a-B36A-5E3306915DA3}");
  private Dictionary<TechExpObject, EntryKeeper> _expertObj2DataCache;
  private Dictionary<int, QuickObjectInfo> _memberNum2CreatedObjIdCache;

  protected override Guid GUID => this._guid;

  protected override void LoadExpertObjData()
  {
    List<TechExpObject> techExpObjectList = new List<TechExpObject>();
    long expertObjectKey;
    TechExpKeyConverter.ConvertFrom(new TechExpKey(this._lastObjId), out expertObjectKey);
    string str = expertObjectKey == 0L ? string.Empty : " AND F_KEY >= " + (object) expertObjectKey;
    this.PumpCheckPoint("Считывание структуры формул", 0);
    string sqlText = $" SELECT * FROM TC_EXPERT WHERE F_TYPE   IN ({"'F'"}) {str} ORDER BY F_KEY";
    int recordsCount = this.GetRecordsCount($"SELECT COUNT(*) FROM TC_EXPERT WHERE F_TYPE IN ({"'F'"}) {str} ");
    int index1 = 0;
    IDataReader dataReader = this.GetDataReader(sqlText);
    try
    {
      TechExpObject.ParseSchema(this.GetTableColumns(dataReader));
      while (dataReader.Read())
      {
        ++index1;
        TechExpObject key = new TechExpObject(dataReader);
        if (key.Type == TechExpObjType.Formula)
        {
          this._expertObj2DataCache.Add(key, (EntryKeeper) null);
          techExpObjectList.Add(key);
        }
        if (index1 % 100 == 0 || index1 == recordsCount)
          this.PumpCheckPoint($"Считывание формул экспертной системы ({index1} из {recordsCount})", this.CalculatePercent(recordsCount, index1, 1, 10));
      }
    }
    finally
    {
      dataReader.Close();
    }
    int index2 = 0;
    FormulaDataProc formulaDataProc = new FormulaDataProc();
    foreach (TechExpObject key in techExpObjectList)
    {
      ++index2;
      try
      {
        ImChunkedStream input = new ImChunkedStream();
        input.Write(key.Body, 0, key.Body.Length);
        input.Position = 0L;
        FormulaHeader formulaHeader = new FormulaHeader();
        using (BinaryReader reader = new BinaryReader((Stream) input, Encoding.Default))
        {
          if (formulaHeader.Load(reader))
          {
            if (formulaHeader.Key.Equals("TCFF"))
            {
              List<EntryHead> entryList;
              List<EntryMember> maxLevInd;
              if (formulaDataProc.Open_FormulaData(reader, out entryList, out maxLevInd))
                this._expertObj2DataCache[key] = new EntryKeeper(entryList, maxLevInd);
              else
                continue;
            }
          }
        }
      }
      catch (Exception ex)
      {
        this.plugin.appManager.AddWarningMessage($"Ошибка загрузки файла формул \"{key.Name}\". Сообщение: {ex.Message}");
      }
      if (index2 % 100 == 0 || index2 == this._expertObj2DataCache.Keys.Count - 1)
        this.PumpCheckPoint($"Загрузка формул экспертной системы ({index2} из {this._expertObj2DataCache.Keys.Count})", this.CalculatePercent(this._expertObj2DataCache.Keys.Count, index2, 11, 19));
    }
    this.PumpCheckPoint("Считывание и загрузка формул завершена успешно", 20);
  }

  protected override void PumpExpertObjData()
  {
    this.PumpCheckPoint("Подготовка к закачке формул экспертной системы", 21);
    TechExpTablesConst.Initialize();
    this.PumpCheckPoint("Подготовка к закачке формул экспертной системы завершена", 22);
    this.PumpCheckPoint("Обработка формул экспертной системы", 23);
    int index = 0;
    int count = this._expertObj2DataCache.Keys.Count;
    int num = 0;
    if (ServicesManager.GetService(typeof (ICache)) is ICache service)
      this._importingData = service.GetCache(ImportingCategory.ImbaseFolders, ImportingCategory.TechCeh, ImportingCategory.ImbaseTableLinksKeyToObjectID, ImportingCategory.TechExpObjStruct);
    this._impObjList = this.plugin.Idw.CreateImportedObjectListWithStatistics(this.GUID);
    this._impRelList = this.plugin.Idw.CreateImportedRelationListWithStatistics(this.GUID);
    try
    {
      foreach (KeyValuePair<TechExpObject, EntryKeeper> keyValuePair in this._expertObj2DataCache)
      {
        if (keyValuePair.Key == null || keyValuePair.Value == null)
        {
          ++index;
        }
        else
        {
          this._techFolderCache.Clear();
          this._memberNum2CreatedObjIdCache.Clear();
          TechExpFolderObject techExpFolder1;
          if (this.PumpRootFolderObject(keyValuePair.Key, out techExpFolder1))
          {
            this._techFolderCache[techExpFolder1.Key] = techExpFolder1;
            this._memberNum2CreatedObjIdCache[-1] = techExpFolder1.ImportedObjectInfo;
          }
          int recordId1 = 0;
          foreach (EntryMember member in keyValuePair.Value.MemberList)
          {
            try
            {
              TechExpObject key1 = keyValuePair.Key;
              TechExpKey key2 = TechExpKeyConverter.ConvertTo((long) key1.Key, (long) recordId1);
              if (this._lastObjId != 0L)
              {
                if (key2.Value <= this._lastObjId)
                  continue;
              }
              TechExpFolderObject folderObject = TechExpFolderObjectFactory.Instance.CreateFolderObject(key2, member);
              this._techFolderCache[key2] = folderObject;
              this.ConvertFolderCondition(key1, folderObject);
              if (member.MemberType != EntryMemberType.Formula)
              {
                if (this.PumpFolderObject(folderObject))
                {
                  this._importingData.SetNewKey(ImportingCategory.TechExpObjStruct, (object) -2, key2.Value);
                  this._memberNum2CreatedObjIdCache[member.MemberNo] = folderObject.ImportedObjectInfo;
                }
              }
            }
            finally
            {
              ++recordId1;
            }
          }
          this._impObjList.Import();
          int recordId2 = 0;
          foreach (EntryMember member in keyValuePair.Value.MemberList)
          {
            try
            {
              TechExpKey key = TechExpKeyConverter.ConvertTo((long) keyValuePair.Key.Key, (long) recordId2);
              if (this._lastObjId != 0L)
              {
                if (key.Value <= this._lastObjId)
                  continue;
              }
              TechExpFolderObject techExpFolder2;
              if (this._techFolderCache.TryGetValue(key, out techExpFolder2))
              {
                if (member.MemberType != EntryMemberType.Formula)
                  this.PumpFolderRelation(member, techExpFolder2);
                else if (!this.PumpExpertFormulaData(keyValuePair.Key, keyValuePair.Value, member, techExpFolder2))
                  ++num;
              }
            }
            finally
            {
              ++recordId2;
            }
          }
          ++index;
          if (index % 100 == 0 || index == count - 1)
            this.PumpCheckPoint($"Обработка формул экспертной системы ({index} из {count})", this.CalculatePercent(count, index, 24, 99));
        }
      }
      this._impObjList.Import();
      this._impRelList.Import();
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ImbaseFolders, ImportingCategory.TechCeh, ImportingCategory.ImbaseTableLinksKeyToObjectID, ImportingCategory.TechExpObjStruct);
    }
    this.PumpCheckPoint(num == 0 ? "Закачка формул экспертной системы завершена успешно" : $"Закачка формул экспертной системы завершена c ошибками, \"{num}\" формул не закачано", 100);
  }

  private bool PumpExpertFormulaData(
    TechExpObject techExpObject,
    EntryKeeper entryKeeper,
    EntryMember entryMember,
    TechExpFolderObject techExpFolder)
  {
    if (techExpObject == null || entryKeeper == null || entryMember == null)
      return false;
    EntryHead headByName = entryKeeper.GetHeadByName(entryMember.Code);
    if (headByName == null)
      return false;
    ListMember listMember1 = (ListMember) null;
    try
    {
      string errorMsg;
      IAttributeTypeItem attributeItemByCode = TechExpert.TypeConverter.GetAttributeItemByCode(entryMember.Code, this.plugin, out errorMsg);
      if (attributeItemByCode == null)
      {
        this.plugin.appManager.AddWarningMessage($"Ошибка закачки формул. Файл \"{techExpObject.Name}\", группа \"{entryMember.Code}\" Сообщение: {errorMsg}");
        return false;
      }
      foreach (ListMember listMember2 in (List<ListMember>) headByName)
      {
        string errorDataMsg = string.Empty;
        listMember1 = listMember2;
        if (listMember2.MemberNo == entryMember.MemberNo)
        {
          List<TempFormula> condList = new List<TempFormula>();
          if (listMember2.Conditions != null && listMember2.Conditions.Count != 0)
          {
            TempFormula ipsFormulaData = (TempFormula) null;
            try
            {
              this.ConvertExpertData((short) 3, listMember2.Conditions, (List<string>) listMember2, out ipsFormulaData);
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
                  string message1 = ex.Message;
                  this.plugin.appManager.AddWarningMessage($"Ошибка закачки формул. Файл \"{techExpObject.Name}\", группа \"{entryMember.Code}\", No записи \"{listMember2.Number}\". атрибут ID = \"{attributeItemByCode.ID}\" Сообщение: {message1}");
                  break;
                case FormulaCompileException _:
                  string message2 = ex.Message;
                  string Message = $"Ошибка компиляции формул. Файл \"{techExpObject.Name}\", группа \"{entryMember.Code}\", No записи \"{listMember2.Number}\". атрибут ID = \"{attributeItemByCode.ID}\" Сообщение: {message2}";
                  errorDataMsg = errorDataMsg + Message + Environment.NewLine;
                  this.plugin.appManager.AddWarningMessage(Message);
                  break;
                default:
                  throw;
              }
            }
            if (ipsFormulaData != null)
              condList.Add(ipsFormulaData);
          }
          for (EntryMember entryMember1 = entryMember; entryMember1 != null; entryMember1 = (EntryMember) null)
          {
            if (entryMember1.Conditions != null && entryMember1.Conditions.Count != 0)
            {
              TempFormula ipsCondition = techExpFolder.IpsCondition;
              if (ipsCondition != null)
              {
                condList.Add(ipsCondition);
              }
              else
              {
                string str = $"Невозможно закачать формулу ЭС. Условие для записи (name = {entryMember1.Code}) не было закачано.";
                throw new FormulaConvertException($"Ошибка закачки формул. Файл \"{techExpObject.Name}\", группа \"{entryMember.Code}\" Сообщение: {str}");
              }
            }
          }
          TempFormula ipsCondData = (TempFormula) null;
          try
          {
            ipsCondData = this.CombineCondData(condList);
          }
          catch (Exception ex)
          {
            if (ex is FormulaCompileException)
            {
              string message = ex.Message;
              string Message = $"Ошибка компиляции формул. Файл \"{techExpObject.Name}\", группа \"{entryMember.Code}\", No записи \"{listMember2.Number}\". атрибут ID = \"{attributeItemByCode.ID}\" Сообщение: {message}";
              errorDataMsg = errorDataMsg + Message + Environment.NewLine;
              this.plugin.appManager.AddWarningMessage(Message);
            }
            else
              throw;
          }
          TempFormula ipsFormulaData1 = (TempFormula) null;
          try
          {
            this.ConvertExpertData(headByName.Cont.ResType, entryMember.Code, listMember2.Formula, (List<string>) listMember2, out ipsFormulaData1);
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
                ipsFormulaData1 = (TempFormula) null;
                this.plugin.appManager.AddWarningMessage(ex.Message);
                break;
              case FormulaCompileException _:
                string message = ex.Message;
                string Message = $"Ошибка компиляции формул. Файл \"{techExpObject.Name}\", группа \"{entryMember.Code}\", No записи \"{listMember2.Number}\". атрибут ID = \"{attributeItemByCode.ID}\" Сообщение: {message}";
                errorDataMsg = errorDataMsg + Message + Environment.NewLine;
                this.plugin.appManager.AddWarningMessage(Message);
                break;
              default:
                throw;
            }
          }
          if (ipsFormulaData1 == null)
            throw new FormulaConvertException($"Ошибка конвертации формулы ЭС. Файл \"{techExpObject.Name}\", группа \"{entryMember.Code}\", No записи \"{listMember2.Number}\". код объекта = \"{listMember2.MemberNo}\"");
          string code = entryMember.Code;
          techExpFolder.ImportedObjectInfo = this.SaveFormulaData(code, listMember2, attributeItemByCode, ipsFormulaData1, ipsCondData, errorDataMsg);
          this._importingData.SetNewKey(ImportingCategory.TechExpObjStruct, (object) -2, techExpFolder.Key.Value);
          if (!techExpFolder.ImportedObjectInfo.Empty)
            this.PumpFolderRelation(entryMember, techExpFolder, listMember2);
        }
      }
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
          string message = ex.Message;
          this.plugin.appManager.AddWarningMessage($"Ошибка закачки формул. Файл \"{techExpObject.Name}\", группа \"{entryMember.Code}\", No записи \"{(listMember1 != null ? (object) listMember1.Number : (object) string.Empty)}\" Сообщение: {message}");
          break;
        default:
          throw;
      }
    }
    return true;
  }

  private void PumpFolderRelation(
    EntryMember entryMember,
    TechExpFolderObject techExpFolder,
    ListMember listMember = null)
  {
    QuickObjectInfo importedObjectInfo = techExpFolder.ImportedObjectInfo;
    QuickObjectInfo quickObjectInfo;
    if (!this._memberNum2CreatedObjIdCache.TryGetValue(entryMember.Owner != null ? entryMember.Owner.MemberNo : -1, out quickObjectInfo) || this._impRelList.AddRelationFromID(quickObjectInfo.ObjectID, importedObjectInfo.ID, ExpertConsts.Consts.linkSimpleSortId) == null)
      return;
    if (listMember != null)
    {
      int result;
      int.TryParse(listMember.Number, out result);
      this._impRelList.AddAttributeInt(ExpertConsts.Consts.attrSorting, (long) result);
    }
    else
      this._impRelList.AddAttributeInt(ExpertConsts.Consts.attrSorting, techExpFolder.Key.Value);
  }

  protected override IDataReader GetDataReader(string sqlText, CommandBehavior commandBehavior)
  {
    IDbCommand command = this.plugin.idb.CreateCommand();
    command.CommandText = sqlText;
    return command.ExecuteReader(commandBehavior);
  }

  protected override IDataReader GetBehaviorDataReader(
    string tableName,
    string tableColumns,
    CommandBehavior commandBehavior)
  {
    if (!this.TableExists(tableName))
      return (IDataReader) null;
    IDbCommand command = this.plugin.idb.CreateCommand();
    command.CommandText = $"SELECT {tableColumns} FROM {tableName.ToUpper()}";
    if (commandBehavior == CommandBehavior.SchemaOnly)
      command.CommandText += " WHERE 1=0";
    return command.ExecuteReader(commandBehavior);
  }

  protected override void LoadMetaData4Pump()
  {
    base.LoadMetaData4Pump();
    this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0147c-306c-11d8-b4e9-00304f19f545"));
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    if (userSession == null)
      return;
    IDBObjectType objectType = userSession.GetObjectType(ExpertConsts.Consts.objFormula);
    if (objectType == null || objectType.AnyAttributes)
      return;
    objectType.AnyAttributes = true;
  }

  public FormulaPump(PluginClass plugin)
    : base(plugin)
  {
    this._impExpObjType = -2;
    this._expertObj2DataCache = new Dictionary<TechExpObject, EntryKeeper>();
    this._memberNum2CreatedObjIdCache = new Dictionary<int, QuickObjectInfo>();
  }

  public override void Exam()
  {
    this.ExamCheckPoint("Подготовка к закачке формул экспертной системы:", 0);
    if (!this.TableExists("TC_EXPERT"))
      this.plugin.appManager.AddWarningMessage($"Таблица '{"TC_EXPERT"}' не найдена.");
    else
      this.ExamCheckPoint("Подготовка к закачке формул экспертной системы: успешно завершена", 100);
  }

  protected override bool NeedPumpExpData()
  {
    if (TechSettingsHelper.PumpMetaDataType.HasFlag((Enum) TechPumpMetaDataType.ExpertFormula))
      return true;
    this.plugin.appManager.AddInfoMessage("Перекачка формул ЭС отключена в настройках");
    this.PumpCheckPoint("Перекачка данных отключена", 0);
    return false;
  }

  protected override void ReleasePumpData()
  {
    base.ReleasePumpData();
    this._expertObj2DataCache.Clear();
    this._expertObj2DataCache = (Dictionary<TechExpObject, EntryKeeper>) null;
    this._memberNum2CreatedObjIdCache.Clear();
    this._memberNum2CreatedObjIdCache = (Dictionary<int, QuickObjectInfo>) null;
  }

  internal QuickObjectInfo SaveFormulaData(
    string entCode,
    ListMember listMember,
    IAttributeTypeItem attrTypeItem,
    TempFormula ipsFormulaData,
    TempFormula ipsCondData,
    string errorDataMsg)
  {
    QuickObjectInfo quickObjectInfo = new QuickObjectInfo()
    {
      ObjectID = -1
    };
    if (entCode == string.Empty || ipsFormulaData == null)
      return quickObjectInfo;
    Entity entityByCode = TechExpert.TypeConverter.GetEntityByCode(entCode);
    if (entityByCode == null)
      return quickObjectInfo;
    Guid guid = Guid.Empty;
    IObjectTypeItem objectTypeItem = (IObjectTypeItem) null;
    if (entityByCode.Settings.ObjectType != Guid.Empty)
    {
      guid = entityByCode.Settings.ObjectType;
    }
    else
    {
      TechTypeInfo techTypeInfo;
      TechPumpData.TechType.TechTypeList.TryGetValue(entityByCode.RecordID, out techTypeInfo);
      string str = techTypeInfo == null || techTypeInfo.TypeSett == null || !(techTypeInfo.TypeSett.ObjType != Guid.Empty) ? string.Empty : techTypeInfo.TypeSett.ObjType.ToString();
      if (str != string.Empty && GuidHelper.IsGuid(str))
        guid = new Guid(str);
    }
    if (guid != Guid.Empty)
      objectTypeItem = this.plugin.Imdi.ObjectTypes.GetByGuid(guid);
    IUserSession userSession = TechcardConsts.Plugin.Idw.GetUserSession();
    if (!(userSession.GetCustomService(typeof (IExpertServer)) is IExpertServer customService))
      throw new ServiceNotFoundException($"Служба {"IExpertServer"} не найдена.");
    try
    {
      int att = 0;
      int num = -1;
      string str1 = string.Empty;
      string str2 = string.Empty;
      string str3 = errorDataMsg;
      if (objectTypeItem != null)
      {
        num = objectTypeItem.ID;
        str1 = objectTypeItem.ObjectName;
      }
      if (attrTypeItem != null)
      {
        att = attrTypeItem.ID;
        str2 = attrTypeItem.Name;
      }
      AttribPair attribPair = new AttribPair(att, num);
      string str4 = "Расчет ";
      if (str1 != "")
        str4 = $"{str4}<{str1}>.";
      string str5 = $"{str4}<{str2}>";
      if (listMember != null)
        str5 = $"{str5} ({listMember.Number} - {listMember.Remark})";
      string str6 = guid != Guid.Empty ? guid.ToString() : string.Empty;
      Guid sessionGuid = userSession.SessionGUID;
      AttribPair ap = attribPair;
      string resAttrGuid = attrTypeItem != null ? attrTypeItem.GUID.ToString() : Guid.Empty.ToString();
      string resObjTypeGuid = str6;
      string Name = str5;
      TempFormula cond = ipsCondData;
      TempFormula formTF = ipsFormulaData;
      IDBObject expertFormula = customService.CreateExpertFormula(sessionGuid, (object) ap, resAttrGuid, resObjTypeGuid, Name, (object) cond, (object) formTF);
      if (expertFormula != null)
      {
        expertFormula.GUID = TechExpert.Utils.GetGuid4Import();
        if (str3 != string.Empty)
        {
          if (this._atCommentTextAtrId != 0)
          {
            try
            {
              AttributeValues attributeValues = new AttributeValues(this._atCommentTextAtrId, (object) errorDataMsg);
              expertFormula.SetAttributesValues(new AttributeValues[1]
              {
                attributeValues
              });
            }
            catch (Exception ex)
            {
              this.plugin.appManager.AddWarningMessage($"Ошибка создания атрибута комментария {this._atCommentTextAtrId}: {ex.Message}");
              if (ex is OutOfMemoryException)
                throw;
            }
          }
        }
        return new QuickObjectInfo(expertFormula.ObjectID, string.Empty, expertFormula.ObjectType, expertFormula.ObjectGUID, expertFormula.ID);
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Ошибка сохранения формулы для понятия = \"{entCode}\" : \"{ex.Message}\" ");
      if (ex is OutOfMemoryException)
        throw;
    }
    return quickObjectInfo;
  }
}
