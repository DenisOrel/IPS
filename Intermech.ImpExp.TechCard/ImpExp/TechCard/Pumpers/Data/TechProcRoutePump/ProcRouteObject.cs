// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ProcRouteObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump;

internal class ProcRouteObject : ProcRouteEntryObject
{
  public Guid RouteId { get; set; }

  public TechParamList ParamList { get; private set; } = new TechParamList();

  public override bool Equals(TechObjectRecordBase other)
  {
    return other is ProcRouteObject procRouteObject && this.RouteId == procRouteObject.RouteId && base.Equals(other);
  }

  public override int GetHashCode() => base.GetHashCode() ^ this.RouteId.GetHashCode();

  public override void Clear()
  {
    base.Clear();
    this.RouteId = Guid.Empty;
    this.ParamList = (TechParamList) null;
  }

  public override void Assign(object source) => base.Assign(source);
}
