// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.DraftPump.DraftOLEPump
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
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.RecordParser;
using Intermech.ImpExp.TechCard.Pumpers.Data.DraftPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechDiff;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.DraftPump;

[TaskDescription("Инициализация данных для перекачки - Эскизы OLE", "Перекачка данных - Эскизы OLE")]
internal class DraftOLEPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private readonly Guid _guid = new Guid("{0E8A191F-620C-4447-8358-39A540A0BC5F}");
  private IObjectTypeItem _objTypeItem;

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "E";
    this._tableName = "TP_OLE";
    if (!this.plugin.Imdi.ObjectTypes.ExistsByGuid(TechcardConsts.TypeConsts.otDraftOLEGuid))
      return;
    this._objTypeItem = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otDraftOLEGuid);
    this.objTypeID = this._objTypeItem.ID;
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechDraftOLE;

  protected override void LoadMetaData4Pump()
  {
    if (this.plugin.Imdi == null)
      this.plugin.appManager.AddErrorMessage("Ошибка получения кэша метаданных");
    else
      base.LoadMetaData4Pump();
  }

  protected override void PumpDiffRec(TechObjectRecord record)
  {
    if (!this._allowDiffObjects)
      return;
    int key = record.Key;
    int int32 = Convert.ToInt32(record.Fields["F_RECORDKEY"]);
    TechDiffRec techDiffRec;
    if (!TechDiffCache.DiffRecList.TryGetValue(int32, out techDiffRec) || techDiffRec == null)
      return;
    List<int> intList = new List<int>();
    foreach (TechDiffElement techDiffElement in techDiffRec.Diff)
    {
      if (techDiffElement.EntType == key && !intList.Contains(techDiffElement.ArtTcKey))
      {
        TechObjectRecord cloneRecord = this.CreateCloneRecord(record);
        cloneRecord.Key = -techDiffElement.Key;
        cloneRecord.diff_ArtTcKey = techDiffElement.ArtTcKey;
        this.BeforePumpCloneRec(record, cloneRecord);
        this._techParmList = new TechParamList();
        this.LoadTechParams(cloneRecord);
        this.PumpBaseRec(cloneRecord);
        this.AfterPumpCloneRec(record, cloneRecord);
        intList.Add(techDiffElement.ArtTcKey);
      }
    }
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    string str = Convert.ToString(record.Fields["F_ENTITY"]);
    if (this._objTypeItem != null && objRecord.Caption == string.Empty)
      objRecord.Caption = this._objTypeItem.ObjectName.Truncate(Consts.MaxStringSize - 2);
    if (record.FieldExist("F_OLE"))
    {
      FileInfo field = (FileInfo) record.Fields["F_OLE"];
      if (TechPumpData.Entities.EntitiesList.TryGetValue(str, out Entity _))
        this._techParmList.AddEntity(str, (object) field);
    }
    base.FillTechObject(objRecord, record);
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    int int32 = Convert.ToInt32(recBase.Fields["F_RECORDID"]);
    DictionaryValue objectIdByRecord = this.GetObjectIdByRecord(Convert.ToInt32(recBase.Fields["F_RECORDKEY"]), int32);
    long ipsObjectB;
    if (recBase.diff_ArtTcKey == 0)
    {
      ipsObjectB = objectIdByRecord != null ? objectIdByRecord.NewObjectID : 0L;
    }
    else
    {
      TechDiffTag diffTag = TechDiffTag.GetDiffTag(objectIdByRecord);
      if (diffTag == null || diffTag.IsCloneListEmpty || !diffTag.CloneList.TryGetValue(recBase.diff_ArtTcKey, out ipsObjectB))
        ipsObjectB = 0L;
    }
    if (ipsObjectB == 0L)
      return base.CreateTechRelList(recBase, ipsObjId);
    int result = -1;
    if (int32 == 8)
    {
      if (objectIdByRecord?.Tag is TechRecordObjectTag tag)
      {
        object obj = tag.Object;
        if (obj is TechProcCacheInfo techProcCacheInfo)
          result = techProcCacheInfo.ObjTypeId;
        else
          int.TryParse(obj.ToString(), out result);
      }
    }
    else
      result = TechPumpData.TechType.TechTypeList.GetObjTypeId(int32);
    return new List<TechRelParam>()
    {
      new TechRelParam(ipsObjectB, ipsObjId, this._relTechRelationID, result, this.objTypeID)
    };
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new DraftOLEObject((TechRecordParser) DraftOLEParser.GetInstance(this.GUID, "TP_OLE"));
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) => TechDataBuilder<PumpClass>.GetPumpModeCond("F_TCKEY", -2));
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override void PumpLoadTechDiffData()
  {
    if (TechDiffCache.DiffPumper != null)
    {
      string condition = string.Format(" {0} IN \r\n                                                 (\r\n                                                   SELECT \r\n                                                     {0}\r\n                                                   FROM\r\n                                                     {1} \r\n                                                 )\r\n                                                ", (object) "F_ENTITY", (object) "TP_OLE");
      TechDiffCache.DiffPumper.LoadDiffData(condition);
      this._allowDiffObjects = TechDiffCache.DiffRecList.Count > 0;
      if (!this._allowDiffObjects)
        return;
      TechDiffRecList techDiffRecList = new TechDiffRecList();
      char[] chArray = new char[1]{ ',' };
      foreach (KeyValuePair<int, TechDiffRec> diffRec in (Dictionary<int, TechDiffRec>) TechDiffCache.DiffRecList)
      {
        foreach (TechDiffElement techDiffElement in diffRec.Value.Diff)
        {
          foreach (string s in techDiffElement.StrValue.Split(chArray))
          {
            int result;
            if (int.TryParse(s, out result))
              techDiffRecList.Add(techDiffElement.Key, diffRec.Value.RecKey, techDiffElement.DocTcKEy, techDiffElement.ArtTcKey, string.Empty, 0, string.Empty, 0.0, result);
          }
        }
      }
      TechDiffCache.DiffRecList.Clear();
      TechDiffCache.DiffRecList = techDiffRecList;
    }
    else
      this._allowDiffObjects = false;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[17]
    {
      ImportingCategory.TechOperation,
      ImportingCategory.TechOutfitPump,
      ImportingCategory.TechManufacturingRouting,
      ImportingCategory.TechAddMovement,
      ImportingCategory.TechRouteTemplate,
      ImportingCategory.TechProcessPump,
      ImportingCategory.TechComment,
      ImportingCategory.TechOsnPos,
      ImportingCategory.TechMatPump,
      ImportingCategory.TechArticlesPump,
      ImportingCategory.TechPerehPump,
      ImportingCategory.TechRezPump,
      ImportingCategory.TechToolsPump,
      ImportingCategory.TechTPOverpatching,
      ImportingCategory.TechRouteElem,
      ImportingCategory.TechZagot,
      ImportingCategory.TechMatGrPump
    };
  }

  private DictionaryValue GetObjectIdByRecord(int recordId, int recordTypeId)
  {
    DictionaryValue objectIdByRecord = (DictionaryValue) null;
    try
    {
      ImportingCategory categoryByRecordTypeId = TechcardConsts.TechCacheConsts.GetImportingCategoryByRecordTypeId(recordTypeId);
      if (categoryByRecordTypeId == ImportingCategory.None)
      {
        this.plugin.appManager.AddWarningMessage($"Невозможно получить идентификатор кэша по идентификатору типа записи ТП: {recordTypeId}");
        return (DictionaryValue) null;
      }
      object oldKey = this.ConvertOldKeyByType(categoryByRecordTypeId, recordId);
      objectIdByRecord = this._import_data_main.GetValue(categoryByRecordTypeId, oldKey);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Ошибка получения идентификатора объекта IPS из кэша. Идентификатор типа записи ТП: {recordTypeId}; Идентификатор записи ТП: {recordId} : {ex.Message} ");
    }
    return objectIdByRecord;
  }

  private object ConvertOldKeyByType(ImportingCategory category, int recordId) => (object) recordId;

  public override void Exam() => base.Exam();

  public override void Pump()
  {
    if (this.plugin.Imdi.ObjectTypes.ExistsByGuid(TechcardConsts.TypeConsts.otDraftOLEGuid))
      this.objTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otDraftOLEGuid).ID;
    base.Pump();
  }
}
