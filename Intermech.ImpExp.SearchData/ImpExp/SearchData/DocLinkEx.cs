// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.DocLinkEx
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class DocLinkEx : DocLink
{
  public readonly int ArtVerID = -1;
  public readonly int DelVerID = -1;

  public DocLinkEx(int docID, int userID, int artVerID, int delVerID)
    : base(docID, userID)
  {
    this.ArtVerID = artVerID;
    this.DelVerID = delVerID;
  }
}
