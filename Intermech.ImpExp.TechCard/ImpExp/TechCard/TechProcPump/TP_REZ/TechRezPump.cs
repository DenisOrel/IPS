// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_REZ.TechRezPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_OPER;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_REZ;

[TaskDescription("Инициализация данных для перекачки - Режимы", "Перекачка данных - Режимы")]
internal class TechRezPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private readonly Guid _guid = new Guid("{C758FB63-CFB0-4ccf-3F51-8A45Ecc45A9A}");

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "R";
    this._recTypeID = 16 /*0x10*/;
    this._tableName = "TP_REZ";
    this._dopTypes.Add("S");
    this._dopTypes.Add("I");
    this._dopTypes.Add("F");
    this._dopTypes.Add("D");
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechRezPump;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.TechPerehPump
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.TechOperation
    };
  }

  protected override void CheckBaseRecords()
  {
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
    List<TechRelParam> techRelList = new List<TechRelParam>();
    long int32_1 = (long) Convert.ToInt32(recBase.Fields["F_PEREHKEY"]);
    long int32_2 = (long) Convert.ToInt32(recBase.Fields["F_OPERKEY"]);
    int relTechRelationId = this._relTechRelationID;
    if (int32_1 != 0L)
    {
      if (!this.IsCloneRecord(recBase))
      {
        long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechPerehPump, (object) int32_1);
        if (newKey != 0L)
        {
          TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, this._otPerehTypeID, this.objTypeID);
          techRelList.Add(techRelParam);
        }
      }
      else
      {
        TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechPerehPump, (object) int32_1, relTechRelationId, recBase, ipsObjId, this._otPerehTypeID, this.objTypeID);
        if (techRelParam != null)
          techRelList.Add(techRelParam);
      }
    }
    else if (!this.IsCloneRecord(recBase))
    {
      long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechOperation, (object) int32_2);
      if (newKey != 0L)
      {
        TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, this._otOperTypeID, this.objTypeID);
        techRelList.Add(techRelParam);
      }
    }
    else
    {
      TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechOperation, (object) int32_2, relTechRelationId, recBase, ipsObjId, this._otOperTypeID, this.objTypeID);
      if (techRelParam != null)
        techRelList.Add(techRelParam);
    }
    return techRelList;
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic("TP_REZ");
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    int int32_1 = Convert.ToInt32(record.Fields["F_ORDER"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_OPERKEY"]);
    DateTime dateTime = Convert.ToDateTime(record.Fields["F_DATE"]);
    objRecord.Caption = ("Режим " + Convert.ToString(int32_1 + 1)).Truncate(Consts.MaxStringSize - 2);
    if (dateTime != DateTime.MinValue)
      objRecord.ObjCreate = dateTime.ToUniversalTime();
    if (this._import_data_main.GetValue(ImportingCategory.TechOperation, (object) int32_2)?.Tag is TechRecordObjectTag tag && tag.Object is TechOperationCacheInfo operationCacheInfo)
    {
      if (operationCacheInfo.OwnerGuid != Guid.Empty)
        objRecord.OwnerGuid = (object) operationCacheInfo.OwnerGuid;
      if (operationCacheInfo.OwnerId != 0L)
        objRecord.OwnerId = operationCacheInfo.OwnerId;
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
