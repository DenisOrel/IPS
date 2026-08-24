// Decompiled with JetBrains decompiler
// Type: OxyPlot.TrackerHitResult
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using OxyPlot.Series;

#nullable disable
namespace OxyPlot;

public class TrackerHitResult
{
  public DataPoint DataPoint { get; set; }

  public object Item { get; set; }

  public double Index { get; set; }

  public OxyRect LineExtents { get; set; }

  public PlotModel PlotModel { get; set; }

  public ScreenPoint Position { get; set; }

  public OxyPlot.Series.Series Series { get; set; }

  public string Text { get; set; }

  public Axis XAxis => !(this.Series is XYAxisSeries series) ? (Axis) null : series.XAxis;

  public Axis YAxis => !(this.Series is XYAxisSeries series) ? (Axis) null : series.YAxis;

  public override string ToString() => this.Text == null ? string.Empty : this.Text.Trim();
}
