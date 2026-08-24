// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TC_CEH.TechCehPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common.TC_CEH;

[TaskDescription("Инициализация данных для перекачки - Цех", "Перекачка данных - Цех")]
[TaskType(PumperType.MetaData)]
internal class TechCehPump(PluginClass plugin) : ImbaseObjectRecordMetaPump(plugin)
{
  private readonly Guid _guid = new Guid("{C788FB63-CFB0-48cf-3F51-8A45E834589A}");
  private readonly string[] _workAreaEntities = new string[3]
  {
    "УЧК",
    "Ush",
    "Нучк"
  };
  private int _otTechCehObjTypeId = -1;
  private int _otTechAreaObjTypeId = -1;
  private IAttributeTypeItem _atTechCehCodeAttr;
  private IAttributeTypeItem _atTechWorkAreaCodeAttr;
  private IAttributeTypeItem _atImbaseReferenceAttr;
  private IAttributeTypeItem _atCreateObjectCopy;

  protected override void InitData()
  {
    this._recType = "Цех";
    this._tableName = string.Empty;
    this.objTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechCehObjTypeGuid).ID;
    this._dopTypes.Add("REC");
  }

  private bool InitData4Pump()
  {
    ImTableInfo tableInfo = TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.Ceh);
    if (tableInfo == null)
    {
      this.plugin.appManager.AddErrorMessage("Таблица справочника \"Цеха\" не найдена. Дальнейшая закачка невозможна!");
      return false;
    }
    this._tableName = tableInfo.TableName;
    this._imTableCode = tableInfo.TableKey;
    return true;
  }

  protected override void LoadMetaData4Pump()
  {
    IMetadataInfo imdi = this.plugin.Imdi;
    if (imdi == null)
    {
      this.plugin.appManager.AddErrorMessage("Ошибка получения кэша метаданных");
    }
    else
    {
      IObjectTypeItem byGuid1 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechCehObjTypeGuid);
      if (byGuid1 != null)
        this._otTechCehObjTypeId = byGuid1.ID;
      IObjectTypeItem byGuid2 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechAreaObjTypeGuid);
      if (byGuid2 != null)
        this._otTechAreaObjTypeId = byGuid2.ID;
      IObjectTypeItem byGuid3 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechCehAreaObjTypeGuid);
      if (byGuid3 != null)
        this.objTypeID = byGuid3.ID;
      IAttributeTypeItem byGuid4 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechCehCodeAttrGuid);
      if (byGuid4 != null)
        this._atTechCehCodeAttr = byGuid4;
      IAttributeTypeItem byGuid5 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechWorkAreaCodeAttrGuid);
      if (byGuid5 != null)
        this._atTechWorkAreaCodeAttr = byGuid5;
      IAttributeTypeItem byGuid6 = imdi.AttributeTypes.GetByGuid(Intermech.Imbase.Consts.ImbaseObjectRefAttGUID);
      if (byGuid6 != null)
        this._atImbaseReferenceAttr = byGuid6;
      IAttributeTypeItem byGuid7 = imdi.AttributeTypes.GetByGuid(Intermech.Imbase.Consts.CreateNewObjectAttGUID);
      if (byGuid7 != null)
        this._atCreateObjectCopy = byGuid7;
      base.LoadMetaData4Pump();
    }
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechCeh;

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.TechArea
    };
  }

  protected override string GetDBMetaRecordHash(TechObjectRecord record)
  {
    return record == null ? string.Empty : $"({this.GetObjectType((TechObjectRecordBase) record)})_{Convert.ToString(record.Fields["F_NAME"])}";
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new ImbaseObjectRecordDynamic(this._tableName);
  }

  private TechCehObjectTypes GetCurObjectType(TechObjectRecordBase recBase)
  {
    if (recBase.FieldExist("#OBJECT_TYPE#"))
      return (TechCehObjectTypes) recBase.GetFieldValue("#OBJECT_TYPE#");
    TechCehObjectTypes fieldValue = TechCehObjectTypes.Ceh;
    foreach (string workAreaEntity in this._workAreaEntities)
    {
      object entityValue = this._techParmList.GetEntityValue(workAreaEntity);
      if (entityValue != null && !string.IsNullOrEmpty(Convert.ToString(entityValue)))
      {
        fieldValue = TechCehObjectTypes.Area;
        break;
      }
    }
    recBase.SetFieldValue("#OBJECT_TYPE#", (object) fieldValue);
    return fieldValue;
  }

  protected override int GetObjectType(TechObjectRecordBase record)
  {
    switch (this.GetCurObjectType(record))
    {
      case TechCehObjectTypes.Ceh:
        return this._otTechCehObjTypeId;
      case TechCehObjectTypes.Area:
        return this._otTechAreaObjTypeId;
      default:
        return -1;
    }
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> techRelList = new List<TechRelParam>();
    int sortedRelationId = TechCardConsts.RelTypes.SortedRelationID;
    switch (this.GetCurObjectType(recBase))
    {
      case TechCehObjectTypes.Area:
        long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechCeh, (object) Convert.ToInt32(recBase.Fields["F_OWNER"]));
        if (newKey != 0L)
        {
          TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, sortedRelationId, this._otTechCehObjTypeId, this.GetObjectType(recBase));
          techRelList.Add(techRelParam);
          break;
        }
        break;
    }
    return techRelList;
  }

  protected override void AddValue2Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase,
    TechParamList recParmList)
  {
    string str = Convert.ToString(recBase.Fields["F_NAME"]);
    int int32 = Convert.ToInt32(recBase.Fields["F_OWNER"]);
    if (this.GetCurObjectType(recBase) == TechCehObjectTypes.Ceh)
    {
      object techObject = (object) null;
      ITechParamBase techParamBase = (ITechParamBase) recParmList.GetEntity("Ceh") ?? (ITechParamBase) recParmList.GetEntity("ЦЕХ");
      if (techParamBase != null)
        techObject = techParamBase.Value;
      if (this._import_data_main.GetValue(this.GetTechCategory(), oldKey) != null)
      {
        this._import_data_main.SetNewKey(this.GetTechCategory(), oldKey, newKey);
      }
      else
      {
        ITagImportObject tag = techObject != null ? (ITagImportObject) new TechObjectTag(techObject) : (ITagImportObject) null;
        this._import_data_main.AddValue(this.GetTechCategory(), oldKey, newKey, techObject != null ? techObject.ToString() : str, tag);
      }
    }
    else
    {
      if (this._import_data_main.GetValue(this.GetTechCategory(), oldKey) == null)
      {
        DictionaryValue dictionaryValue = this._import_data_main.GetValue(this.GetTechCategory(), (object) int32);
        if (dictionaryValue != null)
          this._import_data_main.AddValue(this.GetTechCategory(), oldKey, dictionaryValue.NewObjectID, dictionaryValue.Caption);
      }
      object techObject = (object) null;
      ITechParamBase techParamBase = (ITechParamBase) recParmList.GetEntity("Ush") ?? (ITechParamBase) recParmList.GetEntity("УЧК");
      if (techParamBase != null)
        techObject = techParamBase.Value;
      ITagImportObject tag = techObject != null ? (ITagImportObject) new TechObjectTag(techObject) : (ITagImportObject) null;
      this._import_data_main.AddValue(ImportingCategory.TechArea, oldKey, newKey, techObject != null ? techObject.ToString() : str, tag);
    }
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    DateTime dateTime = Convert.ToDateTime(record.Fields["F_CREATED"]);
    string str = Convert.ToString(record.Fields["F_NAME"]);
    int int32 = Convert.ToInt32(record.Fields["F_KEY"]);
    object obj1 = (object) null;
    ITechParamBase techParamBase1 = (ITechParamBase) this._techParmList.GetEntity("Ceh") ?? (ITechParamBase) this._techParmList.GetEntity("ЦЕХ");
    if (techParamBase1 != null)
      obj1 = techParamBase1.Value;
    if (obj1 != null)
      this._techParmList.AddAttribute(this._atTechCehCodeAttr, obj1);
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    if (dateTime != DateTime.MinValue)
      objRecord.ObjCreate = dateTime.ToUniversalTime();
    if (this._atImbaseKeyAttr != null)
      this._techParmList.AddAttribute(this._atImbaseKeyAttr, (object) int32);
    int result;
    if (int.TryParse(Convert.ToString(record.Fields["F_LEVEL"]), out result) && result != 0)
    {
      DictionaryValue dictionaryValue = this._import_data_imbase.GetValue(ImportingCategory.ImbaseFolders, (object) TechcardConsts.Utils.CodeHashCode(this._imTableCode, result));
      if (dictionaryValue != null)
        this._techParmList.AddAttribute(this._atImbaseReferenceAttr, (object) dictionaryValue);
    }
    if (this._atCreateObjectCopy != null)
      this._techParmList.AddAttribute(this._atCreateObjectCopy, (object) false);
    switch (this.GetCurObjectType((TechObjectRecordBase) record))
    {
      case TechCehObjectTypes.Ceh:
        if (this._atNaimAttrType != null)
          this._techParmList.AddAttribute(this._atNaimAttrType, (object) str);
        if (this._atObozAttrType != null)
          this._techParmList.AddAttribute(this._atObozAttrType, obj1 ?? (object) str);
        objRecord.Caption = (obj1 != null ? obj1.ToString() : str).Truncate(Intermech.Consts.MaxStringSize - 2);
        break;
      case TechCehObjectTypes.Area:
        if (this._atNaimAttrType != null)
          this._techParmList.AddAttribute(this._atNaimAttrType, (object) str);
        object obj2 = (object) null;
        ITechParamBase techParamBase2 = (ITechParamBase) this._techParmList.GetEntity("Ush") ?? (ITechParamBase) this._techParmList.GetEntity("УЧК");
        if (techParamBase2 != null)
          obj2 = techParamBase2.Value;
        if (obj2 != null)
          this._techParmList.AddAttribute(this._atTechWorkAreaCodeAttr, obj2);
        if (this._atObozAttrType != null)
          this._techParmList.AddAttribute(this._atObozAttrType, obj2 ?? (object) str);
        objRecord.Caption = (obj2 != null ? obj2.ToString() : str).Truncate(Intermech.Consts.MaxStringSize - 2);
        break;
    }
    base.FillTechObject(objRecord, record);
  }

  public override void Exam() => base.Exam();

  public override void Pump()
  {
    if (!this.InitData4Pump())
      return;
    base.Pump();
  }

  protected override Guid GUID => this._guid;
}
