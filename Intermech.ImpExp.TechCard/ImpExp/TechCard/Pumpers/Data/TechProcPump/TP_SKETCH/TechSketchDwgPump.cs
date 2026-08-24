// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_SKETCH.TechSketchDwgPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.DraftPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.RecordParser;
using Intermech.ImpExp.TechCard.Pumpers.Data.DraftPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_SKETCH;

[TaskDescription("Инициализация данных для перекачки - Эскизы DWG", "Перекачка данных - Эскизы DWG")]
internal class TechSketchDwgPump(PluginClass plugin) : DraftDWGPump(plugin)
{
  private readonly Guid _guid = new Guid("{1D122990-FA86-409B-8861-FD29A1B691F2}");

  private TechSketchType GetRecordType(TechObjectRecordBase recBase)
  {
    return !(recBase is TechSketchObject techSketchObject) ? TechSketchType.None : techSketchObject.SketchType;
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechSketchObject((TechRecordParser) DraftOLEParser.GetInstance(this.GUID, "TP_SKETCH"));
  }

  protected override TechObjectRecordSub GetTpObjRecDop(string dopType)
  {
    return TechObjectRecordSubFactory.Create(dopType, true);
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
      this._dataSource = new TechDataSource((ITechDataBuilder) new TechSketchDwgDataBuilder<TechPumpBase>((TechPumpBase) this));
    return this._dataSource;
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    if (this.GetRecordType((TechObjectRecordBase) record) != TechSketchType.Layer)
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    record.RecMode = TechObjectRecord.PumpMode.ObjectAndLinks;
    return string.Empty;
  }

  protected override string GetRecordWithParamsPumpMode(TechObjectRecord record)
  {
    object entityValue1 = this._techParmList.GetEntityValue("#sn");
    record.SetFieldValue("F_NUMBER", (object) (entityValue1 != null ? Convert.ToInt32(entityValue1) : 0));
    object entityValue2 = this._techParmList.GetEntityValue("#snm");
    record.SetFieldValue("F_NAME", (object) Convert.ToString(entityValue2));
    record.SetFieldValue("F_OPERKEY", (object) (Convert.ToInt32(record.Fields["F_RECORDID"]) == 1 ? Convert.ToInt32(record.Fields["F_RECORDKEY"]) : 0));
    foreach (ITechParamBase techParm in (List<ITechParamBase>) this._techParmList)
    {
      if (techParm is TechParamEntity)
      {
        TechParamEntity techParamEntity = (TechParamEntity) techParm;
        record.SetFieldValue(techParamEntity.Code, techParamEntity.Value);
      }
    }
    return base.GetRecordPumpMode(record);
  }

  protected override Dictionary<int, List<int>> Load_Pict2PerehodLinksInfo()
  {
    return new Dictionary<int, List<int>>();
  }

  protected override Dictionary<int, List<int>> Load_Pict2ArtLinksInfo()
  {
    return new Dictionary<int, List<int>>();
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> techRelList = new List<TechRelParam>();
    int int32_1 = Convert.ToInt32(recBase.Fields["F_TCKEY"]);
    List<TechObjectRecordBase> objectRecordBaseList;
    if (!this._picList4Tp.TryGetValue(int32_1, out objectRecordBaseList))
      return techRelList;
    foreach (TechObjectRecordBase objectRecordBase in objectRecordBaseList)
    {
      int int32_2 = Convert.ToInt32(objectRecordBase.Fields["F_RECORDKEY"]);
      int int32_3 = Convert.ToInt32(objectRecordBase.Fields["F_RECORDID"]);
      this.IsCloneRecord(objectRecordBase);
      ImportingCategory categoryByRecordTypeId = TechcardConsts.TechCacheConsts.GetImportingCategoryByRecordTypeId(int32_3);
      if (categoryByRecordTypeId == ImportingCategory.None)
      {
        this.plugin.appManager.AddWarningMessage($"Невозможно получить идентификатор кэша по идентификатору типа записи ТП: {int32_3}");
      }
      else
      {
        DictionaryValue dictionaryValue;
        try
        {
          dictionaryValue = this._import_data_main.GetValue(categoryByRecordTypeId, (object) int32_2);
        }
        catch (Exception ex)
        {
          this.plugin.appManager.AddWarningMessage($"Ошибка получения идентификатора объекта IPS из кэша. Идентификатор типа записи ТП: {int32_3}; Идентификатор записи ТП: {int32_2} : {ex.Message} ");
          continue;
        }
        if (dictionaryValue != null)
        {
          RelationRecord relationRecord = this._impRelList.AddRelation(dictionaryValue.NewObjectID, ipsObjId, this._rtTechDraftRelationId);
          this.FillRelationAttributes(objectRecordBase, ipsObjId);
          int objTypeId = TechPumpData.TechType.TechTypeList.GetObjTypeId(int32_3);
          this.FillLinkSortParam(new TechRelParam(dictionaryValue.NewObjectID, ipsObjId, this._rtTechDraftRelationId, objTypeId, this.objTypeID)
          {
            RelRec = relationRecord
          }, objectRecordBase);
          this.FillLinkObligatoryAttributes();
        }
      }
    }
    int objTypeId1 = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.Passport);
    long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechProcessPump, (object) int32_1);
    if (newKey != 0L)
    {
      RelationRecord relationRecord = this._impRelList.AddRelation(newKey, ipsObjId, this._relTechRelationID);
      this.FillLinkSortParam(new TechRelParam(newKey, ipsObjId, this._relTechRelationID, objTypeId1, this.objTypeID)
      {
        RelRec = relationRecord
      }, recBase);
      this.FillLinkObligatoryAttributes();
    }
    TechDiffTag techDiffTagByOldKey = this.GetTechDiffTagByOldKey(ImportingCategory.TechProcessPump, (object) int32_1);
    if (techDiffTagByOldKey != null && !techDiffTagByOldKey.IsCloneListEmpty)
    {
      foreach (long num in techDiffTagByOldKey.CloneList.Values)
      {
        RelationRecord relationRecord = this._impRelList.AddRelation(num, ipsObjId, this._relTechRelationID);
        this.FillLinkSortParam(new TechRelParam(num, ipsObjId, this._relTechRelationID, objTypeId1, this.objTypeID)
        {
          RelRec = relationRecord
        }, recBase);
        this.FillLinkObligatoryAttributes();
      }
    }
    this._picList4Tp.Remove(int32_1);
    return techRelList;
  }

  protected override ObjectRecord CreateTechObject(TechObjectRecord record)
  {
    return base.CreateTechObject(record);
  }

  protected override Guid GUID => this._guid;

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechSketchDwg;

  protected override void InitData()
  {
    base.InitData();
    this._tableName = "TP_SKETCH";
    this._dopTypes.Add("D");
  }

  public override void Exam() => base.Exam();
}
