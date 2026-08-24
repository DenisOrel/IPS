// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump.EntryInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump;

internal readonly struct EntryInfo(
  int artTcKey,
  int artId,
  int sbArtTcKey,
  int sbArtId,
  int zakArtTcKey,
  int zakArtId) : IEquatable<EntryInfo>
{
  public int ArtTcKey { get; } = artTcKey;

  public int ArtId { get; } = artId;

  public int SbArtTcKey { get; } = sbArtTcKey;

  public int SbArtId { get; } = sbArtId;

  public int ZakArtTcKey { get; } = zakArtTcKey;

  public int ZakArtId { get; } = zakArtId;

  public bool Equals(EntryInfo other)
  {
    return this.ArtTcKey == other.ArtTcKey && this.SbArtTcKey == other.SbArtTcKey && this.ZakArtTcKey == other.ZakArtTcKey && this.ArtId == other.ArtId && this.SbArtId == other.SbArtId && this.ZakArtId == other.ZakArtId;
  }

  public override bool Equals(object obj) => obj is EntryInfo other && this.Equals(other);

  public override int GetHashCode()
  {
    return this.ArtTcKey ^ this.SbArtTcKey ^ this.ZakArtTcKey ^ this.ArtId ^ this.SbArtId ^ this.ZakArtId;
  }
}
