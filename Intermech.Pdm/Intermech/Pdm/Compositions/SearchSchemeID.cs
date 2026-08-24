// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.SearchSchemeID
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class SearchSchemeID
{
  public long SchemeID { get; protected set; }

  public string Name { get; protected set; }

  public SearchSchemeID(string name, long schemeID)
  {
    this.Name = name;
    this.SchemeID = schemeID;
  }

  public override string ToString() => this.Name;
}
