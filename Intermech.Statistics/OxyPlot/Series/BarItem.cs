// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.BarItem
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Series;

public class BarItem : BarItemBase
{
  public BarItem()
  {
  }

  public BarItem(double value, int categoryIndex = -1)
  {
    this.Value = value;
    this.CategoryIndex = categoryIndex;
  }
}
