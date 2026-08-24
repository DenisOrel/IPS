// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ZPCEntryPump.ZPCProcRouteEntryObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ZPCEntryPump;

internal class ZPCProcRouteEntryObject : ProcRouteEntryObject
{
  public ZPCProcRouteEntryObject(long artIpsObjId, int exitSbPrjLinkId)
  {
    this.ArtIpsObjId = artIpsObjId;
    this.ExitSbPrjLinkId = exitSbPrjLinkId;
  }

  public long ArtIpsObjId { get; }

  public int ExitSbPrjLinkId { get; }

  public TechParamList ParamList { get; } = new TechParamList();

  public override void Clear() => base.Clear();

  public override void Assign(object source) => base.Assign(source);

  public override bool Equals(TechObjectRecordBase other)
  {
    return other is ZPCProcRouteEntryObject routeEntryObject && this.ArtIpsObjId == routeEntryObject.ArtIpsObjId && this.ExitSbPrjLinkId == routeEntryObject.ExitSbPrjLinkId && base.Equals(other);
  }

  public override int GetHashCode()
  {
    return base.GetHashCode() ^ (int) this.ArtIpsObjId ^ this.ExitSbPrjLinkId;
  }
}
