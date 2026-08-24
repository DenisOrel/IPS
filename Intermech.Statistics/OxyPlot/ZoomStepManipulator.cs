// Decompiled with JetBrains decompiler
// Type: OxyPlot.ZoomStepManipulator
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class ZoomStepManipulator(IPlotView plotView) : MouseManipulator(plotView)
{
  public bool FineControl { get; set; }

  public double Step { get; set; }

  public override void Started(OxyMouseEventArgs e)
  {
    base.Started(e);
    if ((this.XAxis == null || !this.XAxis.IsZoomEnabled ? (this.YAxis == null ? 0 : (this.YAxis.IsZoomEnabled ? 1 : 0)) : 1) == 0)
      return;
    DataPoint dataPoint = this.InverseTransform(e.Position.X, e.Position.Y);
    double step = this.Step;
    if (this.FineControl)
      step *= 3.0;
    double factor = 1.0 + step;
    if (factor < 0.1)
      factor = 0.1;
    if (this.XAxis != null)
      this.XAxis.ZoomAt(factor, dataPoint.X);
    if (this.YAxis != null)
      this.YAxis.ZoomAt(factor, dataPoint.Y);
    this.PlotView.InvalidatePlot(false);
    e.Handled = true;
  }
}
