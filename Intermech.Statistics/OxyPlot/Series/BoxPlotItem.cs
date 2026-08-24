// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.BoxPlotItem
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class BoxPlotItem
{
  public BoxPlotItem(
    double x,
    double lowerWhisker,
    double boxBottom,
    double median,
    double boxTop,
    double upperWhisker)
  {
    this.X = x;
    this.LowerWhisker = lowerWhisker;
    this.BoxBottom = boxBottom;
    this.Median = median;
    this.BoxTop = boxTop;
    this.UpperWhisker = upperWhisker;
    this.Mean = double.NaN;
    this.Outliers = (IList<double>) new List<double>();
  }

  public double BoxBottom { get; set; }

  public double BoxTop { get; set; }

  public double LowerWhisker { get; set; }

  public double Median { get; set; }

  public double Mean { get; set; }

  public IList<double> Outliers { get; set; }

  public object Tag { get; set; }

  public double UpperWhisker { get; set; }

  public IList<double> Values
  {
    get
    {
      List<double> values = new List<double>()
      {
        this.LowerWhisker,
        this.BoxBottom,
        this.Median,
        this.BoxTop,
        this.UpperWhisker
      };
      if (!double.IsNaN(this.Mean))
        values.Add(this.Mean);
      values.AddRange((IEnumerable<double>) this.Outliers);
      return (IList<double>) values;
    }
  }

  public double X { get; set; }

  public override string ToString()
  {
    return $"{this.X} {this.LowerWhisker} {this.BoxBottom} {this.Median} {this.Mean} {this.BoxTop} {this.UpperWhisker} ";
  }
}
