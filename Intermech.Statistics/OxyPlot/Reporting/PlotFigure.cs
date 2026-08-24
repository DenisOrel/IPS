// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.PlotFigure
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Reporting;

public class PlotFigure : Figure
{
  public double Height { get; set; }

  public PlotModel PlotModel { get; set; }

  public double Width { get; set; }

  public override void WriteContent(IReportWriter w) => w.WritePlot(this);
}
