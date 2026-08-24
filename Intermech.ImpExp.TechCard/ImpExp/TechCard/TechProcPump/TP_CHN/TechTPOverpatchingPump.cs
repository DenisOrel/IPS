// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_CHN.TechTPOverpatchingPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_CHN;

[TaskDescription("Инициализация данных для перекачки - Изменения ТП", "Перекачка данных - Изменения ТП")]
internal class TechTPOverpatchingPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private Guid _guid = new Guid("{1231F47B-225D-4D19-8882-C542D3DDE111}");

  protected override void InitData()
  {
    this._recType = "V";
    this._recTypeID = 20;
    this._tableName = "TP_CHN";
    this._dopTypes.Add("S");
    this._dopTypes.Add("I");
    this._dopTypes.Add("F");
    this._dopTypes.Add("D");
  }

  protected override void LoadMetaData4Pump() => base.LoadMetaData4Pump();

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechTPOverpatching;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.TechProcessPump
    };
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic("TP_CHN");
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

  public override bool CheckObjTypeOrParamType(string entCode, Guid attrGuid)
  {
    return base.CheckObjTypeOrParamType(entCode, attrGuid);
  }

  public override bool CheckObjLinkOrParamType(string entCode, Guid attrGuid)
  {
    return base.CheckObjLinkOrParamType(entCode, attrGuid);
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    string str = Convert.ToString(record.Fields["F_DES"]);
    DateTime dateTime = Convert.ToDateTime(record.Fields["F_DATE"]);
    if (str != string.Empty)
      objRecord.Caption = str.Truncate(Consts.MaxStringSize - 2);
    if (!(dateTime != DateTime.MinValue))
      return;
    objRecord.ObjCreate = dateTime.ToUniversalTime();
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    long int32 = (long) Convert.ToInt32(recBase.Fields["F_DOCTCKEY"]);
    List<TechRelParam> techRelList = new List<TechRelParam>();
    DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechProcessPump, (object) int32);
    long newObjectId = dictionaryValue != null ? dictionaryValue.NewObjectID : 0L;
    if (newObjectId != 0L)
    {
      int result = -1;
      if (dictionaryValue != null && dictionaryValue.Tag is TechRecordObjectTag)
      {
        object obj = ((TechRecordObjectTag) dictionaryValue.Tag).Object;
        if (obj is TechProcCacheInfo techProcCacheInfo)
          result = techProcCacheInfo.ObjTypeId;
        else
          int.TryParse(obj.ToString(), out result);
      }
      TechRelParam techRelParam = new TechRelParam(newObjectId, ipsObjId, this._relTechRelationID, result, this.objTypeID);
      techRelList.Add(techRelParam);
    }
    return techRelList;
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();

  protected override Guid GUID => this._guid;
}
