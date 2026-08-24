// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechCardSettings.ArtInfoLight
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common.TechCardSettings;

[Serializable]
public readonly struct ArtInfoLight(int artId, int artVer = -1, int vArtId = 0, int artTcKey = 0)
{
  public int ArtId { get; } = artId;

  public int ArtVer { get; } = artVer;

  public int VArtId { get; } = vArtId;

  public int ArtTCKey { get; } = artTcKey;
}
