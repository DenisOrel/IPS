// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_TOOL.TechToolsPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechProcPump.TP_OSNPOS;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_TOOL;

[TaskDescription("Инициализация данных для перекачки - Оснастка", "Перекачка данных - Оснастка")]
internal class TechToolsPump(PluginClass plugin) : TechBaseUniquePump(plugin)
{
  private readonly Guid _guid = new Guid("{C758FB63-CFB0-48cf-3F51-8A45E0345A9A}");
  protected int _rtTechCollectRelationID = -1;
  protected int _otTechInstrumentObjTypeID = -1;
  protected int _otTechPositionObjTypeId = -1;
  protected int _otToolRequestObjTypeId = -1;

  protected override void InitData()
  {
    base.InitData();
    this._sortFieldName = "F_ORDER";
    this._recType = "T";
    this._recTypeID = 18;
    this._tableName = "TP_TOOL";
    this._dopTypes.Add("S");
    this._dopTypes.Add("I");
    this._dopTypes.Add("F");
    this._dopTypes.Add("D");
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
      IRelationTypeItem byGuid1 = imdi.RelationTypes.GetByGuid(TechcardConsts.TypeConsts.rtTechCollectRelationGuid);
      if (byGuid1 != null)
        this._rtTechCollectRelationID = byGuid1.ID;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechProcGroupRelAttrGUID);
      if (byGuid2 != null)
        this._atTechLinkAtRelGTPRelation = byGuid2;
      IObjectTypeItem byGuid3 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechInstrumentObjTypeGuid);
      if (byGuid3 != null)
        this._otTechInstrumentObjTypeID = byGuid3.ID;
      IObjectTypeItem byGuid4 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otInstrumentalPositionObjTypeGuid);
      if (byGuid4 != null)
        this._otTechPositionObjTypeId = byGuid4.ID;
      IObjectTypeItem byGuid5 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otToolRequestGuid);
      if (byGuid5 != null)
        this._otToolRequestObjTypeId = byGuid5.ID;
      base.LoadMetaData4Pump();
    }
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    int int32_1 = Convert.ToInt32(record.Fields["F_RECKEY"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_TBLKEY"]);
    if (int32_1 == 0 && int32_2 == 0)
      record.Fields["F_TBLKEY"] = (object) record.Key;
    return base.GetRecordPumpMode(record);
  }

  protected override string GetUniqueRecordHash(TechObjectRecordBase record)
  {
    if (!(record is TechToolsObject techToolsObject))
      return base.GetUniqueRecordHash(record);
    if (!string.IsNullOrEmpty(techToolsObject.UniqueRecordHash))
      return techToolsObject.UniqueRecordHash;
    string uniqueRecordHash = $"{base.GetUniqueRecordHash(record)}:{this.NormalizeEntityValue(this._techParmList.GetEntityValue("ОСН"))}:{this.NormalizeEntityValue(this._techParmList.GetEntityValue("Н_ВО"))}:{this.NormalizeEntityValue(this._techParmList.GetEntityValue("О_ВО"))}";
    if (this._techParmList.Count != 0)
      techToolsObject.UniqueRecordHash = uniqueRecordHash;
    return uniqueRecordHash;
  }

  protected override string GetRecordRecKey(TechObjectRecordBase record)
  {
    return Convert.ToString(this._techParmList.GetEntityValue("OsRc"));
  }

  private int GetEntNOSNValue(TechParamList paramList, TechObjectRecordBase record)
  {
    int result = 0;
    object entityValue = paramList.GetEntityValue("НОСН");
    if (entityValue == null)
      return result;
    int.TryParse(entityValue.ToString(), out result);
    return result;
  }

  private int GetEntNPOZValue(TechParamList paramList, TechObjectRecordBase record)
  {
    int result = 0;
    object entityValue = paramList.GetEntityValue("НПОЗ");
    if (entityValue == null)
      return result;
    int.TryParse(entityValue.ToString(), out result);
    return result;
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechToolsPump;

  protected override ImportingCategory GetTechUniqueCategory()
  {
    return ImportingCategory.TechToolsUniquePump;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[5]
    {
      ImportingCategory.TechOperation,
      ImportingCategory.TechPerehPump,
      ImportingCategory.TechProcessPump,
      ImportingCategory.TechOsnPosUnicalKeys,
      ImportingCategory.TechOsnPos
    };
  }

  protected override string GetTechcardObjectCompareIndex()
  {
    string objectName = ImbaseObjectNameParser.ParseCompositeObjName(Convert.ToString(this._techParmList.GetEntityValue("ОСН"))).ObjectName;
    if (string.IsNullOrEmpty(objectName))
    {
      string sourceImbaseObjName = Convert.ToString(this._techParmList.GetEntityValue("Н_ВО"));
      if (!string.IsNullOrEmpty(sourceImbaseObjName))
        objectName = ImbaseObjectNameParser.ParseCompositeObjName(sourceImbaseObjName).ObjectName;
    }
    return objectName.Truncate(Intermech.Consts.MaxStringSize - 2);
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new List<ImportingCategory>((IEnumerable<ImportingCategory>) base.GetCategoriesByNeed2FillTechObject())
    {
      ImportingCategory.TechOsnPos,
      ImportingCategory.Articles
    }.ToArray();
  }

  public override bool CheckObjTypeOrParamType(string entCode, Guid attrGuid)
  {
    return base.CheckObjTypeOrParamType(entCode, attrGuid);
  }

  public override bool CheckObjLinkOrParamType(string entCode, Guid attrGuid)
  {
    return base.CheckObjLinkOrParamType(entCode, attrGuid);
  }

  public override void FillObjСhangedOrParamType(TechObjectRecord record, ITechParamBase techParm)
  {
  }

  public override void FillLinkChangedOrParamType(
    TechObjectRecordBase recBase,
    RelationRecord relRecord,
    ITechParamBase techParm)
  {
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null)
      return;
    if (!record.FieldExist("F_INSTRUM"))
    {
      int entNosnValue = this.GetEntNOSNValue(this._techParmList, (TechObjectRecordBase) record);
      record.AddFieldValue("F_INSTRUM", (object) entNosnValue);
    }
    if (!record.FieldExist("F_POS"))
    {
      int entNpozValue = this.GetEntNPOZValue(this._techParmList, (TechObjectRecordBase) record);
      record.AddFieldValue("F_POS", (object) entNpozValue);
    }
    if (record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    DateTime dateTime = Convert.ToDateTime(record.Fields["F_DATE"]);
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    if (dateTime != DateTime.MinValue)
      objRecord.ObjCreate = dateTime.ToUniversalTime();
    object entityValue = this._techParmList.GetEntityValue("ОСН");
    if (entityValue == null || entityValue.Equals((object) string.Empty))
      entityValue = this._techParmList.GetEntityValue("Н_ВО");
    if (entityValue != null)
      objRecord.Caption = entityValue.ToString().Truncate(Intermech.Consts.MaxStringSize - 2);
    base.FillTechObject(objRecord, record);
  }

  protected override void FillRecordParams2NewObject(
    TechObjectRecord record,
    TechParamList techParmList,
    ObjectRecord objRecord)
  {
    this.PasteInstrumentObjectIfNeed(objRecord, (TechObjectRecordBase) record);
    base.FillRecordParams2NewObject(record, techParmList, objRecord);
  }

  protected override bool PumpLoadSubData_Loaded(
    string dopType,
    TechObjectRecordBase recBase,
    TechObjectRecordSub dopRecord)
  {
    int int32_1 = Convert.ToInt32(dopRecord.ExFields["F_RECKEY"]);
    int int32_2 = Convert.ToInt32(dopRecord.ExFields["F_TBLKEY"]);
    if (int32_1 == 0 && int32_2 == 0)
      dopRecord.ExFields["F_TBLKEY"] = (object) dopRecord.ParentKey;
    return base.PumpLoadSubData_Loaded(dopType, recBase, dopRecord);
  }

  protected override int GetObjectType(TechObjectRecordBase record)
  {
    return (Convert.ToInt32(record.Fields["F_FLAGS"]) & 1) != 0 ? this._otToolRequestObjTypeId : this.objTypeID;
  }

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new TechToolsObject();

  protected TechDiffTag GetTechDiffTagByValue(ImportingCategory category, object dValue)
  {
    Dictionary<object, DictionaryValue> category1 = this._import_data_main.GetCategory(category);
    if (category1 == null)
      return (TechDiffTag) null;
    foreach (KeyValuePair<object, DictionaryValue> keyValuePair in category1)
    {
      if (keyValuePair.Value != null && keyValuePair.Value.NewObjectID.Equals(dValue))
        return TechDiffTag.GetDiffTag(keyValuePair.Value);
    }
    return (TechDiffTag) null;
  }

  protected override TechDiffTag GetTechDiffTagByOldKey(ImportingCategory category, object oldKey)
  {
    return category == ImportingCategory.TechOsnPos ? this.GetTechDiffTagByValue(category, oldKey) : base.GetTechDiffTagByOldKey(category, oldKey);
  }

  protected DictionaryValue GetTechToolParentObj(TechObjectRecordBase recBase)
  {
    string techToolParentKey = TechOsnPosPump.GenerateTechToolParentKey(recBase);
    return techToolParentKey.Equals(string.Empty) ? (DictionaryValue) null : this._import_data_main.GetValue(ImportingCategory.TechOsnPosUnicalKeys, (object) techToolParentKey);
  }

  protected override ObjectRecord CreateTechObject(TechObjectRecord record)
  {
    this.GetRecordPumpMode(record);
    this.GetRecordWithParamsPumpMode(record);
    if (TechCardPlugin.Configuration.SpecToolDirectLinkPumpMode && (record.RecMode == TechObjectRecord.PumpMode.ObjectOnly || record.RecMode == TechObjectRecord.PumpMode.ObjectAndLinks))
    {
      int int32 = Convert.ToInt32(this._techParmList.GetEntityValue("_ART") ?? (object) 0);
      if (int32 > 0)
      {
        long objVerId = this.GetArticleInfoByKey(int32).ObjVerId;
        if (objVerId != 0L)
        {
          try
          {
            this._impObjList.UseObject(objVerId);
          }
          catch (Exception ex)
          {
            this.RemoveBaseRec((TechObjectRecordBase) record);
            this.plugin.appManager.AddWarningMessage($"Невозможно использовать существующий объект оснастки из Search \"{objVerId}\" по причине: {ex.Message}{Environment.NewLine + ex.StackTrace}");
            if (ex is OutOfMemoryException)
              throw;
            this.DoHandleImportObjectsException(ex);
            return (ObjectRecord) null;
          }
          ObjectRecord objectRec = new ObjectRecord()
          {
            ObjectGuid = (object) Guid.Empty,
            Object_id = objVerId,
            ObjectType = TechCardConsts.ObjectTypes.SpecialToolID
          };
          int currentIndex = this._impObjList.Items.CurrentIndex;
          this._techBaseImportList.Add((TechObjectRecordBase) record, currentIndex);
          this.FillTechObject(objectRec, record);
          return objectRec;
        }
      }
    }
    return base.CreateTechObject(record);
  }

  protected override bool CreateTechCustomRelations(
    TechObjectRecordBase recBase,
    long ipsObjId,
    List<TechRelParam> relParmList)
  {
    bool techCustomRelations = true;
    DictionaryValue techToolParentObj = this.GetTechToolParentObj(recBase);
    if (techToolParentObj != null)
    {
      techCustomRelations = false;
      int ipsObjTypeB = techToolParentObj.Caption != string.Empty ? this._otTechInstrumentObjTypeID : this._otTechPositionObjTypeId;
      if (!this.IsCloneRecord(recBase))
      {
        TechRelParam techRelParam = new TechRelParam(techToolParentObj.NewObjectID, ipsObjId, this._relTechRelationID, ipsObjTypeB, this.objTypeID);
        relParmList.Add(techRelParam);
      }
      else
      {
        long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechOsnPosUnicalKeys, (object) TechOsnPosPump.GenerateTechToolParentKey(recBase));
        if (newKey != 0L)
        {
          RelationRecord relationRecord = this._impRelList.AddRelation(newKey, ipsObjId, this._relTechRelationID);
          relParmList.Add(new TechRelParam(newKey, ipsObjId, this._relTechRelationID, ipsObjTypeB, this.objTypeID)
          {
            RelRec = relationRecord
          });
        }
      }
    }
    return techCustomRelations;
  }

  protected void PasteInstrumentObjectIfNeed(ObjectRecord objRec, TechObjectRecordBase recBase)
  {
  }

  public override void Exam() => base.Exam();

  public override void Pump()
  {
    if (TechCardPlugin.Configuration.SpecToolDirectLinkPumpMode)
      this.plugin.appManager.AddWarningMessage("Включен режим миграции спецоснастки непосредственно в спецоснастку.");
    base.Pump();
  }

  protected override void ClearTmpData() => base.ClearTmpData();

  protected override Guid GUID => this._guid;
}
