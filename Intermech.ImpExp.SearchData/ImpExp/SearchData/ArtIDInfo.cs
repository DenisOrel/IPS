// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ArtIDInfo
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class ArtIDInfo
{
  internal int PartID = -1;
  internal int PartVerID = -1;

  public ArtIDInfo(int partID, int partVerID)
  {
    this.PartID = partID;
    this.PartVerID = partVerID;
  }
}
