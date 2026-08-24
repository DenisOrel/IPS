// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2TpOld.TechRoute2TpOldPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.Interfaces.TechCard;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2TpOld;

[TaskDescription("Инициализация данных для перекачки - Связь расцеховки с техпроцессом через элементы РМ и операции", "Перекачка данных - Связь расцеховки с техпроцессом через элементы РМ и операции")]
internal class TechRoute2TpOldPump(PluginClass plugin) : TechLinkBasePump(plugin)
{
  private readonly Guid _guid = new Guid("{FEAE28BE-4248-4B09-83A9-BC9C86CDBD4B}");

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    this._relTypeGuid = TechCardConsts.RelTypes.TechRouteRelationGuid;
    this._categoryA = ImportingCategory.TechRoute;
    this._categoryB = ImportingCategory.TechProcessPump;
    this._fieldAName = TechDbConsts.ROUTE_KEY;
    this._fieldBName = TechDbConsts.TP_KEY;
    this._recType = "Связь расцеховки с техпроцессом через элементы РМ и операции";
  }

  protected override ImportingCategory GetTechCategory()
  {
    return ImportingCategory.LinksTechRoute2TpPumpOld;
  }

  protected override ImportingCategory GetTechUniqueCategory()
  {
    return ImportingCategory.LinksTechRoute2TpUniqueLinks;
  }

  protected override string GetUniqueRecordHash(TechObjectRecordBase record)
  {
    int int32_1 = Convert.ToInt32(record.Fields[this.GetFieldNameA(record)]);
    int int32_2 = Convert.ToInt32(record.Fields[this.GetFieldNameB(record)]);
    return $"{(object) this.GetNewKeyA(record, int32_1)}_{(object) this.GetNewKeyB(record, int32_2)}";
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechRoute2TpOldObject();
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource != null)
      return this._dataSource;
    this._dataSource = new TechDataSource((ITechDataBuilder) new TechRoute2TPOldDataBuilder<TechPumpBase>((TechPumpBase) this));
    return this._dataSource;
  }

  protected override void CheckBaseRecords()
  {
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();
}
