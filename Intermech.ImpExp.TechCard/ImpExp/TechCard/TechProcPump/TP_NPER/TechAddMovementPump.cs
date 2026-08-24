// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_NPER.TechAddMovementPump
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
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_NPER;

[TaskDescription("Инициализация данных для перекачки - Доп. приемы", "Перекачка данных - Доп. приемы")]
internal class TechAddMovementPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private readonly Guid _guid = new Guid("{431F525F-92AA-4c1a-8F69-807065A0C11F}");
  protected IAttributeTypeItem _atNonNumerationFlag;

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "E";
    this._recTypeID = 5;
    this._tableName = "TP_NPER";
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
      IObjectTypeItem byGuid1 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechPerehodObjTypeGuid);
      if (byGuid1 != null)
        this._otPerehTypeID = byGuid1.ID;
      IObjectTypeItem byGuid2 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otOperationObjTypeGuid);
      if (byGuid2 != null)
        this._otOperTypeID = byGuid2.ID;
      IAttributeTypeItem byGuid3 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atNonNumerateFlagAttrGuid);
      if (byGuid3 != null)
        this._atNonNumerationFlag = byGuid3;
      base.LoadMetaData4Pump();
    }
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechAddMovement;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[2]
    {
      ImportingCategory.TechPerehPump,
      ImportingCategory.TechOperation
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[2]
    {
      ImportingCategory.ImbaseFolders,
      ImportingCategory.TechCeh
    };
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic("TP_NPER");
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) => TechDataBuilder<PumpClass>.GetPumpModeCond(dopType == "" ? "F_DOCTCKEY" : "F_TCKEY", -2));
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    int relTechRelationId = this._relTechRelationID;
    List<TechRelParam> techRelList = new List<TechRelParam>();
    int int32_1 = Convert.ToInt32(recBase.Fields["F_PEREHKEY"]);
    int int32_2 = Convert.ToInt32(recBase.Fields["F_OPERKEY"]);
    if (int32_1 != 0)
    {
      long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechPerehPump, (object) int32_1);
      if (newKey == 0L)
        return techRelList;
      if (this.IsCloneRecord(recBase))
      {
        TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechPerehPump, (object) int32_1, relTechRelationId, recBase, ipsObjId, this._otPerehTypeID, this.objTypeID);
        if (techRelParam != null)
          techRelList.Add(techRelParam);
      }
      else
      {
        TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, this._otPerehTypeID, this.objTypeID);
        techRelList.Add(techRelParam);
      }
    }
    else
    {
      long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechOperation, (object) int32_2);
      if (newKey == 0L)
        return techRelList;
      if (this.IsCloneRecord(recBase))
      {
        TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechOperation, (object) int32_2, relTechRelationId, recBase, ipsObjId, this._otOperTypeID, this.objTypeID);
        if (techRelParam != null)
          techRelList.Add(techRelParam);
      }
      else
      {
        TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, this._otOperTypeID, this.objTypeID);
        techRelList.Add(techRelParam);
      }
    }
    return techRelList;
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    int int32 = Convert.ToInt32(record.Fields["F_FLAGS"]);
    ITechParamEntity entity = this._techParmList.GetEntity("Тдпр");
    if (entity != null && this._entityConverter != null)
    {
      ITechParamAttribute techParamAttribute = this._entityConverter.Convert((TechObjectRecordBase) record, this._techParmList, entity, this.GetEntityByCode(entity.Code));
      objRecord.Caption = techParamAttribute != null ? Convert.ToString(techParamAttribute.Value).Truncate(Consts.MaxStringSize - 2) : string.Empty;
    }
    if (int32 == 0 && this._atNonNumerationFlag != null)
    {
      this._techParmList.AddOrUpdateEntity("%Ndp", (object) string.Empty);
      this._techParmList.AddAttribute(this._atNonNumerationFlag, (object) true);
    }
    base.FillTechObject(objRecord, record);
  }

  public override void Pump() => base.Pump();

  public override void Exam() => base.Exam();
}
