// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_SKETCH.TechSketchPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.RecordParser;
using Intermech.ImpExp.TechCard.Pumpers.Data.DraftPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_SKETCH;

[TaskDescription("Инициализация данных для перекачки - Эскизы", "Перекачка данных - Эскизы")]
internal class TechSketchPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private readonly Guid _guid = new Guid("{54F23BF4-101C-4F11-8A8F-C56C3E0F11C3}");
  private int _otDraftOleObjTypeId = -1;
  private int _otDraftDwgObjTypeId = -1;
  private int _atFileAttrId = -1;
  private int _atOleDataAttrId = -1;

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "4";
    this._recTypeID = 36;
    this._tableName = "TP_SKETCH";
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
      IObjectTypeItem byGuid1 = imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.DraftOLEGUID);
      if (byGuid1 != null)
        this._otDraftOleObjTypeId = byGuid1.ID;
      IObjectTypeItem byGuid2 = imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.DraftCadmechGUID);
      if (byGuid2 != null)
        this._otDraftDwgObjTypeId = byGuid2.ID;
      IObjectTypeItem byGuid3 = imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.DraftGUID);
      if (byGuid3 != null)
        this.objTypeID = byGuid3.ID;
      IAttributeTypeItem byGuid4 = imdi.AttributeTypes.GetByGuid(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545"));
      if (byGuid4 != null)
        this._atFileAttrId = byGuid4.ID;
      IAttributeTypeItem byGuid5 = imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.OLEObjectAttrGuid);
      if (byGuid5 != null)
        this._atOleDataAttrId = byGuid5.ID;
      base.LoadMetaData4Pump();
    }
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechSketch;

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

  protected override TechObjectRecordSub GetTpObjRecDop(string dopType)
  {
    return TechObjectRecordSubFactory.Create(dopType, true);
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    if (this.GetRecordType((TechObjectRecordBase) record) != TechSketchType.Layer)
      return base.GetRecordPumpMode(record);
    record.RecMode = TechObjectRecord.PumpMode.NotPump;
    return string.Empty;
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechSketchDataBuilder<TechPumpBase> dataBuilder = new TechSketchDataBuilder<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) => TechDataBuilder<PumpClass>.GetPumpModeCond(dopType == "" ? "F_DOCTCKEY" : "F_TCKEY", -2));
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  private TechSketchType GetRecordType(TechObjectRecordBase recBase)
  {
    return !(recBase is TechSketchObject techSketchObject) ? TechSketchType.None : techSketchObject.SketchType;
  }

  protected override int GetObjectType(TechObjectRecordBase record)
  {
    switch (this.GetRecordType(record))
    {
      case TechSketchType.Ole:
        return this._otDraftOleObjTypeId;
      case TechSketchType.Dwg:
        return this._otDraftDwgObjTypeId;
      default:
        return this.objTypeID;
    }
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    int int32 = Convert.ToInt32(recBase.Fields["F_RECORDID"]);
    DictionaryValue objectIdByRecord = this.GetObjectIdByRecord(Convert.ToInt32(recBase.Fields["F_RECORDKEY"]), int32);
    long ipsObjectB;
    if (!this.IsCloneRecord(recBase))
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
      if (objectIdByRecord != null && objectIdByRecord.Tag is TechRecordObjectTag)
      {
        object obj = ((TechRecordObjectTag) objectIdByRecord.Tag).Object;
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
      new TechRelParam(ipsObjectB, ipsObjId, this._relTechRelationID, result, this.GetObjectType(recBase))
    };
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechSketchObject((TechRecordParser) DraftOLEParser.GetInstance(this.GUID, "TP_SKETCH"));
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    int int32 = Convert.ToInt32(record.Fields["F_ORDER"]);
    DateTime dateTime = Convert.ToDateTime(record.Fields["F_DATE"]);
    string str = Convert.ToString(this._techParmList.GetEntityValue("#snm"));
    if (string.IsNullOrEmpty(str))
    {
      object entityValue = this._techParmList.GetEntityValue("#sn");
      str = "Эскиз " + (object) (entityValue != null ? Convert.ToInt32(entityValue) : int32 + 1);
    }
    objRecord.Caption = str.Truncate(Intermech.Consts.MaxStringSize - 2);
    if (dateTime != DateTime.MinValue)
      objRecord.ObjCreate = dateTime.ToUniversalTime();
    if (record.FieldExist("F_BLOB") && record.Fields["F_BLOB"] is FileInfo field)
    {
      int attrType = this.GetRecordType((TechObjectRecordBase) record) != TechSketchType.Ole ? this._atFileAttrId : this._atOleDataAttrId;
      if (attrType != 0)
      {
        string fileNote = Convert.ToString(this._techParmList.GetEntityValue("#sfn"));
        if (string.IsNullOrEmpty(fileNote))
          fileNote = Convert.ToString(record.Fields["F_SOURCE"]);
        this._impObjList.AddAttributeBlob(attrType, field.FullName, field.Length, fileNote, ArcMethods.NotPacked);
      }
    }
    base.FillTechObject(objRecord, record);
  }

  public override void Exam() => base.Exam();

  public override void Pump()
  {
    this.LoadMetaData4Pump();
    base.Pump();
  }
}
