// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechCehZahodPump.TechCehZahodPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Common.TechCehZahodPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechDiff;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common.TechCehZahodPump;

[TaskDescription("Инициализация данных для перекачки - Цехозаход", "Перекачка данных - Цехозаход")]
internal class TechCehZahodPump : TechPumpBase
{
  private string _previousCehZahodCode;
  private int _previousCehZahodRecordKey = -1;
  private readonly Guid _guid = new Guid("{C758FB63-CFB0-48cf-9F51-8A95E0D0DA9A}");
  private IList<int> _attr2Ignore = (IList<int>) new List<int>();
  private IAttributeTypeItem _atTechObjectName;
  private IAttributeTypeItem _atCehAttr;
  private IAttributeTypeItem _atWorkPlace;
  protected IAttributeTypeItem _atProductionAttrType;

  public TechCehZahodPump(PluginClass plugin)
    : base(plugin)
  {
    this._recType = "Цехозаход";
    this._tableName = "TP_OPER";
  }

  protected override void InitData()
  {
    this._tableName = "TP_OPER";
    this.objTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.CehZahodObjectGUID).ID;
    this._sortFieldName = "F_ORDER";
    this._recType = "Цехозаход";
    this._recTypeID = 1;
    this._dopTypes.Add("D");
    this._dopTypes.Add("S");
    if (this._atImbaseKeyAttr == null)
      this._atImbaseKeyAttr = this.plugin.Imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.ImbaseKeyAttrGuid);
    if (this._atImbaseKeyAttr == null)
      return;
    this._attr2Ignore.Add(this._atImbaseKeyAttr.ID);
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
      IAttributeTypeItem byGuid1 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechObjectName);
      if (byGuid1 != null)
        this._atTechObjectName = byGuid1;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.CehRouteAttrGUID);
      if (byGuid2 != null)
        this._atCehAttr = byGuid2;
      IAttributeTypeItem byGuid3 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.AreaRouteAttrGUID);
      if (byGuid3 != null)
        this._atWorkPlace = byGuid3;
      IAttributeTypeItem byGuid4 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atProductionAttrTypeGuid);
      if (byGuid4 != null)
        this._atProductionAttrType = byGuid4;
      base.LoadMetaData4Pump();
    }
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechCehZahodPump;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.TechProcessPump
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[4]
    {
      ImportingCategory.TechProcessPump,
      ImportingCategory.TechCeh,
      ImportingCategory.TechArea,
      ImportingCategory.TechParentParametors
    };
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override bool CheckRecordLessThenLastKey(TechObjectRecord record) => false;

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    if (this.CheckRecordLessThenLastKey(record))
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    int oldKey = Convert.ToInt32(record.Fields["F_PLACE"]);
    int int32_1 = Convert.ToInt32(record.Fields["F_DOCTCKEY"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_KEY"]);
    TechProcCacheInfo techProcCacheInfo = (TechProcCacheInfo) null;
    if (this._import_data_main.GetValue(ImportingCategory.TechProcessPump, (object) int32_1)?.Tag is TechRecordObjectTag tag)
      techProcCacheInfo = tag.Object as TechProcCacheInfo;
    if (oldKey == 0 && techProcCacheInfo != null)
      record.Fields["F_PLACE"] = (object) (oldKey = techProcCacheInfo.CehCode);
    string str;
    if (TechCardPlugin.Configuration.CehZahodIgnoreAreaPumpMode)
    {
      DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechCeh, (object) oldKey);
      long num = dictionaryValue != null ? dictionaryValue.NewObjectID : (long) oldKey;
      str = $"{int32_1}_{num}";
    }
    else
      str = $"{int32_1}_{oldKey}";
    if (TechCardPlugin.Configuration.CehZahodProductionPumpMode)
    {
      int num = Convert.ToInt32(record.Fields["F_PRODUCTION"]);
      if (num == 0 && techProcCacheInfo != null)
        record.Fields["F_PRODUCTION"] = (object) (num = techProcCacheInfo.ProductionCode);
      str += $"_{num}";
    }
    if (str != this._previousCehZahodCode)
    {
      this._previousCehZahodCode = str;
      this._previousCehZahodRecordKey = int32_2;
      record.RecMode = TechObjectRecord.PumpMode.ObjectAndLinks;
      return string.Empty;
    }
    this.AddValue2Cache((object) int32_2, (long) -this._previousCehZahodRecordKey, (TechObjectRecordBase) record, (TechParamList) null);
    this._previousCehZahodCode = str;
    record.RecMode = TechObjectRecord.PumpMode.NotPump;
    return string.Empty;
  }

  protected override void ExamSubData(string dopType)
  {
  }

  protected override TechDataSource GetDataSource()
  {
    return this._dataSource ?? (this._dataSource = new TechDataSource((ITechDataBuilder) new TechCehZahodDataBuilder<TechPumpBase>((TechPumpBase) this)));
  }

  public override void FillObjectParams(
    TechObjectRecord record,
    TechParamList paramList,
    ObjectRecord objectRec)
  {
    if (paramList != null && objectRec != null && string.IsNullOrEmpty(objectRec.Caption))
    {
      string str = Convert.ToString(paramList.GetEntityValue("ЦЕХ"));
      if (!string.IsNullOrEmpty(str))
      {
        objectRec.Caption = str;
        for (int index = 0; index < paramList.Count; ++index)
        {
          if (paramList[index] is ITechParamAttribute techParamAttribute && (techParamAttribute.AttributeType == this._atNaimAttrType || techParamAttribute.AttributeType == this._atTechObjectName))
            paramList[index] = (ITechParamBase) new TechParamAttribute(techParamAttribute.AttributeType, (object) str, techParamAttribute.AttributeBelongs);
        }
      }
    }
    TechParamList paramList1 = new TechParamList();
    if (paramList != null)
    {
      foreach (ITechParamBase techParamBase in (List<ITechParamBase>) paramList)
      {
        switch (techParamBase.GetTechParamType())
        {
          case Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamType.Attribute:
            if (techParamBase is ITechParamAttribute techParamAttribute && techParamAttribute.AttributeType != null && !this._attr2Ignore.Contains(techParamAttribute.AttributeType.ID))
            {
              paramList1.AddAttribute(techParamAttribute.AttributeType, techParamAttribute.Value, techParamAttribute.Caption, techParamAttribute.AttributeBelongs);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    base.FillObjectParams(record, paramList1, objectRec);
  }

  public override void FillRecordParamsFixed(TechObjectRecord record, TechParamList paramList)
  {
    if (this._atTechTypeKeyAttr != null)
      paramList.AddAttribute(this._atTechTypeKeyAttr, (object) record.Key);
    if (this._atTechArtAttr == null)
      return;
    paramList.AddAttribute(this._atTechArtAttr, (object) record.diff_ArtTcKey);
  }

  public override void FillLinkParams(
    TechObjectRecordBase recBase,
    TechRelParam relRecord,
    TechParamList paramList)
  {
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    int int32_1 = Convert.ToInt32(record.Fields["F_PLACE"]);
    DictionaryValue dictionaryValue1 = this._import_data_main.GetValue(ImportingCategory.TechCeh, (object) int32_1);
    string caption;
    if (dictionaryValue1 != null)
    {
      caption = dictionaryValue1.Caption;
    }
    else
    {
      caption = Convert.ToString(this._techParmList.GetEntityValue("ЦЕХ"));
      this.plugin.appManager.AddWarningMessage($"Цех с кодом = {(object) int32_1} не найден");
    }
    if (!string.IsNullOrEmpty(caption))
      objRecord.Caption = caption.Truncate(Intermech.Consts.MaxStringSize - 2);
    if (this._atTechObjectName != null)
      this._techParmList.AddAttribute(this._atTechObjectName, (object) caption);
    if (this._atNaimAttrType != null)
      this._techParmList.AddAttribute(this._atNaimAttrType, (object) caption);
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    if (this._atCehAttr != null && dictionaryValue1 != null)
      this._techParmList.AddAttribute(this._atCehAttr, (object) dictionaryValue1.NewObjectID, dictionaryValue1.Caption);
    if (!TechCardPlugin.Configuration.CehZahodIgnoreAreaPumpMode && this._atWorkPlace != null)
    {
      DictionaryValue dictionaryValue2 = this._import_data_main.GetValue(ImportingCategory.TechArea, (object) int32_1);
      if (dictionaryValue2 != null)
        this._techParmList.AddAttribute(this._atWorkPlace, (object) dictionaryValue2.NewObjectID, dictionaryValue2.Caption);
    }
    int curentObjectLifeC = this.GetCurentObjectLifeC((TechObjectRecordBase) record);
    if (curentObjectLifeC != -1)
      objRecord.Lc_step = curentObjectLifeC;
    if (TechCardPlugin.Configuration.CehZahodProductionPumpMode)
    {
      int int32_2 = Convert.ToInt32(record.Fields["F_PRODUCTION"]);
      IpsProductionObj ipsProductionObj;
      if (this._atProductionAttrType != null && int32_2 != 0 && TechPumpData.Production.Productions.TryGetValue(int32_2, out ipsProductionObj))
      {
        if (ipsProductionObj == null)
          this.plugin.appManager.AddWarningMessage($"Вид производства № {int32_2} не найден");
        else
          this._techParmList.AddAttribute(this._atProductionAttrType, (object) ipsProductionObj.ObjID, ipsProductionObj.ProdInfo.Name);
      }
    }
    base.FillTechObject(objRecord, record);
  }

  protected override void PumpLoadTechDiffData()
  {
    int num = 1;
    if (TechDiffCache.DiffPumper != null)
    {
      string condition = string.Empty;
      if (this._lastObjID != 0L)
        condition = string.Format(" {0} = {1} AND \r\n                                         {2} >= \r\n                                         (\r\n                                           SELECT \r\n                                             MAX({2})\r\n                                           FROM \r\n                                             {3}\r\n                                           WHERE\r\n                                             {4} = {5}  \r\n                                         )\r\n                                      ", (object) "F_RECORDID", (object) num, (object) "F_DOCTCKEY", (object) "TP_OPER", (object) "F_KEY", (object) this._lastObjID);
      TechDiffCache.DiffPumper.LoadDiffData(condition);
      this._allowDiffObjects = TechDiffCache.DiffRecList.Count > 0;
    }
    else
      this._allowDiffObjects = false;
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic("TP_OPER");
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> techRelList = new List<TechRelParam>();
    int int32 = Convert.ToInt32(recBase.Fields["F_DOCTCKEY"]);
    int relTechRelationId = this._relTechRelationID;
    DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechProcessPump, (object) int32);
    int result = -1;
    if (dictionaryValue?.Tag is TechRecordObjectTag tag)
    {
      object obj = tag.Object;
      if (obj is TechProcCacheInfo techProcCacheInfo)
        result = techProcCacheInfo.ObjTypeId;
      else
        int.TryParse(obj.ToString(), out result);
    }
    if (!this.IsCloneRecord(recBase))
    {
      long newObjectId = dictionaryValue != null ? dictionaryValue.NewObjectID : 0L;
      if (newObjectId != 0L)
      {
        TechRelParam techRelParam = new TechRelParam(newObjectId, ipsObjId, relTechRelationId, result, this.objTypeID);
        techRelList.Add(techRelParam);
      }
    }
    else
    {
      TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechProcessPump, (object) int32, relTechRelationId, recBase, ipsObjId, result, this.objTypeID);
      if (techRelParam != null)
        techRelList.Add(techRelParam);
    }
    return techRelList;
  }

  public override void Exam() => base.Exam();

  public override void Pump()
  {
    if (TechCardPlugin.Configuration.CehZahodIgnoreAreaPumpMode)
      this.plugin.appManager.AddWarningMessage("Включен режим миграции цехозаходов только по цехам.");
    if (TechCardPlugin.Configuration.CehZahodProductionPumpMode)
      this.plugin.appManager.AddWarningMessage("Включен режим миграции цехозаходов с учетом вида производства!!");
    base.Pump();
  }

  protected override void ReleasePumpData()
  {
    base.ReleasePumpData();
    this._attr2Ignore.Clear();
    this._attr2Ignore = (IList<int>) null;
  }

  protected override Guid GUID => this._guid;
}
