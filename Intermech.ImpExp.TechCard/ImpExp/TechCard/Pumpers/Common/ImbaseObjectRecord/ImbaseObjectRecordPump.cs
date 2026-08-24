// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord.ImbaseObjectRecordPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord;

internal abstract class ImbaseObjectRecordPump(PluginClass plugin) : TechPumpBase(plugin)
{
  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
      this._dataSource = new TechDataSource((ITechDataBuilder) new ImbaseObjectRecordDataBuilder<TechPumpBase>((TechPumpBase) this));
    return this._dataSource;
  }
}
