// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.CategorizedItem
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Series;

public abstract class CategorizedItem
{
  protected CategorizedItem() => this.CategoryIndex = -1;

  public int CategoryIndex { get; set; }

  internal int GetCategoryIndex(int defaultIndex)
  {
    return this.CategoryIndex < 0 ? defaultIndex : this.CategoryIndex;
  }
}
