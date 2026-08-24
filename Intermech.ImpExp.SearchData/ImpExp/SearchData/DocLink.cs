// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.DocLink
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class DocLink
{
  public readonly int DocID;
  public int VerID = -1;
  public readonly int UserID;

  public DocLink(int docID, int userID)
  {
    this.DocID = docID;
    this.UserID = userID;
  }
}
