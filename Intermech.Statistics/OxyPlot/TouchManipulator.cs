// Decompiled with JetBrains decompiler
// Type: OxyPlot.TouchManipulator
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;

#nullable disable
namespace OxyPlot;

public class TouchManipulator(IPlotView plotView) : PlotManipulator<OxyTouchEventArgs>(plotView)
{
  private bool IsPanEnabled { get; set; }

  private bool IsZoomEnabled { get; set; }

  public override void Completed(OxyTouchEventArgs e)
  {
    base.Completed(e);
    OxyTouchEventArgs oxyTouchEventArgs = e;
    oxyTouchEventArgs.Handled = ((oxyTouchEventArgs.Handled ? 1 : 0) | (this.IsPanEnabled ? 1 : (this.IsZoomEnabled ? 1 : 0))) != 0;
  }

  public override void Delta(OxyTouchEventArgs e)
  {
    base.Delta(e);
    if (!this.IsPanEnabled && !this.IsZoomEnabled)
      return;
    ScreenPoint position = e.Position;
    ScreenPoint ppt = position - e.DeltaTranslation;
    if (this.XAxis != null)
      this.XAxis.Pan(ppt, position);
    if (this.YAxis != null)
      this.YAxis.Pan(ppt, position);
    DataPoint dataPoint = this.InverseTransform(position.X, position.Y);
    ScreenVector deltaScale;
    if (this.XAxis != null)
    {
      Axis xaxis = this.XAxis;
      deltaScale = e.DeltaScale;
      double x1 = deltaScale.X;
      double x2 = dataPoint.X;
      xaxis.ZoomAt(x1, x2);
    }
    if (this.YAxis != null)
    {
      Axis yaxis = this.YAxis;
      deltaScale = e.DeltaScale;
      double y1 = deltaScale.Y;
      double y2 = dataPoint.Y;
      yaxis.ZoomAt(y1, y2);
    }
    this.PlotView.InvalidatePlot(false);
    e.Handled = true;
  }

  public override void Started(OxyTouchEventArgs e)
  {
    this.AssignAxes(e.Position);
    base.Started(e);
    this.IsPanEnabled = this.XAxis != null && this.XAxis.IsPanEnabled || this.YAxis != null && this.YAxis.IsPanEnabled;
    this.IsZoomEnabled = this.XAxis != null && this.XAxis.IsZoomEnabled || this.YAxis != null && this.YAxis.IsZoomEnabled;
    OxyTouchEventArgs oxyTouchEventArgs = e;
    oxyTouchEventArgs.Handled = ((oxyTouchEventArgs.Handled ? 1 : 0) | (this.IsPanEnabled ? 1 : (this.IsZoomEnabled ? 1 : 0))) != 0;
  }
}
