// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechRoutePump.Common.TechRouteCommonPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.Common;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechRoutePump.Common;

internal abstract class TechRouteCommonPump(PluginClass plugin) : TechPumpBase(plugin)
{
  protected override void InitData()
  {
    base.InitData();
    this._dopTypes.Add("D");
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) => dopType == string.Empty ? "F_KEY" : "F_PARENTKEY");
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override TechObjectRecordSub GetTpObjRecDop(string dopType)
  {
    return (TechObjectRecordSub) new RouteObjRecDop_D();
  }

  protected override void PumpLoadSubData(TechObjectRecordBase recBase, string dopType)
  {
    base.PumpLoadSubData(recBase, dopType);
  }
}
