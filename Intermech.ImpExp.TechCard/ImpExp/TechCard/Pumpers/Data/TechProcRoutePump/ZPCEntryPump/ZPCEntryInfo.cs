// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ZPCEntryPump.ZPCEntryInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ZPCEntryPump;

internal class ZPCEntryInfo : IEquatable<ZPCEntryInfo>
{
  public ZPCEntryInfo(
    int artId,
    int artPrjLinkId,
    int sbId,
    int sbPrjLinkId,
    int zakId,
    int exitPrjLinkId)
  {
    this.ZakId = zakId;
    this.ExitPrjLinkId = exitPrjLinkId;
    this.ArtId = artId;
    this.ArtPrjLinkId = artPrjLinkId;
    this.SbId = sbId;
    this.SbPrjLinkId = sbPrjLinkId;
  }

  public int ArtId { get; }

  public int ArtPrjLinkId { get; }

  public int SbId { get; }

  public int SbPrjLinkId { get; }

  public int ZakId { get; }

  public int ExitPrjLinkId { get; }

  public EntryInfo TcEntryInfo { get; set; }

  public bool Equals(ZPCEntryInfo other)
  {
    if (other == null)
      return false;
    if (this == other)
      return true;
    return this.ZakId == other.ZakId && this.ExitPrjLinkId == other.ExitPrjLinkId && this.ArtId == other.ArtId && this.ArtPrjLinkId == other.ArtPrjLinkId && this.SbId == other.SbId && this.SbPrjLinkId == other.SbPrjLinkId;
  }

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;
    if (this == obj)
      return true;
    return !(obj.GetType() != this.GetType()) && this.Equals((ZPCEntryInfo) obj);
  }

  public override int GetHashCode()
  {
    return this.ZakId ^ this.ExitPrjLinkId ^ this.ArtId ^ this.ArtPrjLinkId ^ this.SbId ^ this.SbPrjLinkId;
  }
}
