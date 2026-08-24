// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.CategorizedSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public abstract class CategorizedSeries : XYAxisSeries
{
  protected const string DefaultCategoryAxisTitle = "Category";
  protected const string DefaultValueAxisTitle = "Value";

  internal abstract double GetBarWidth();

  protected internal abstract IList<CategorizedItem> GetItems();

  protected abstract double GetActualBarWidth();

  protected abstract CategoryAxis GetCategoryAxis();
}
