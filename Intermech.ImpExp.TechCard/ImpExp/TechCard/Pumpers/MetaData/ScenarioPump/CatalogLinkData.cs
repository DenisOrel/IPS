// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump.CatalogLinkData
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;

[Serializable]
internal class CatalogLinkData
{
  public CatalogLinkData() => this.FoldersId = new List<int>();

  public CatalogLinkData(int catalogId)
    : this()
  {
    this.CatalogId = catalogId;
  }

  public CatalogLinkData(int catalogId, int level)
    : this(catalogId)
  {
    this.FoldersId.Add(level);
  }

  public int Production { get; set; }

  public List<int> FoldersId { get; private set; }

  public int CatalogId { get; set; }
}
