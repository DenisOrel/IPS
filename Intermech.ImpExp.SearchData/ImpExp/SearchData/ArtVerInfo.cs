// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ArtVerInfo
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class ArtVerInfo(int id, int verID, int actualVerID = -1) : ObjVerInfo(id, verID, actualVerID)
{
  public int DocID = -2;
  public int DocVerID = -1;
  public long NewArtObjectID;
  public ArticleFlag Flags;
  public DocsLinks DocsLinks;
  public string ExtInfo = "";
  public int SectID = -1;
  public string ImbaseKey = "";
  public string Name = "";
  public ArtVerInfo MainVI;

  public bool IsDocumentation => (this.Flags & ArticleFlag.Documentation) != 0;
}
