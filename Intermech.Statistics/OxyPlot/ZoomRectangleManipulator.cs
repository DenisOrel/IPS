// Decompiled with JetBrains decompiler
// Type: OxyPlot.ZoomRectangleManipulator
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public class ZoomRectangleManipulator(IPlotView plotView) : MouseManipulator(plotView)
{
  private OxyRect zoomRectangle;

  private bool IsZoomEnabled { get; set; }

  public override void Completed(OxyMouseEventArgs e)
  {
    base.Completed(e);
    if (!this.IsZoomEnabled)
      return;
    this.PlotView.SetCursorType(CursorType.Default);
    this.PlotView.HideZoomRectangle();
    if (this.zoomRectangle.Width > 10.0 && this.zoomRectangle.Height > 10.0)
    {
      DataPoint dataPoint1 = this.InverseTransform(this.zoomRectangle.Left, this.zoomRectangle.Top);
      DataPoint dataPoint2 = this.InverseTransform(this.zoomRectangle.Right, this.zoomRectangle.Bottom);
      if (this.XAxis != null)
        this.XAxis.Zoom(dataPoint1.X, dataPoint2.X);
      if (this.YAxis != null)
        this.YAxis.Zoom(dataPoint1.Y, dataPoint2.Y);
      this.PlotView.InvalidatePlot();
    }
    e.Handled = true;
  }

  public override void Delta(OxyMouseEventArgs e)
  {
    base.Delta(e);
    if (!this.IsZoomEnabled)
      return;
    OxyRect plotArea = this.PlotView.ActualModel.PlotArea;
    double left = Math.Min(this.StartPosition.X, e.Position.X);
    double width = Math.Abs(this.StartPosition.X - e.Position.X);
    double top = Math.Min(this.StartPosition.Y, e.Position.Y);
    double height = Math.Abs(this.StartPosition.Y - e.Position.Y);
    if (this.XAxis == null || !this.XAxis.IsZoomEnabled)
    {
      left = plotArea.Left;
      width = plotArea.Width;
    }
    if (this.YAxis == null || !this.YAxis.IsZoomEnabled)
    {
      top = plotArea.Top;
      height = plotArea.Height;
    }
    this.zoomRectangle = new OxyRect(left, top, width, height);
    this.PlotView.ShowZoomRectangle(this.zoomRectangle);
    e.Handled = true;
  }

  public override void Started(OxyMouseEventArgs e)
  {
    base.Started(e);
    this.IsZoomEnabled = this.XAxis != null && this.XAxis.IsZoomEnabled || this.YAxis != null && this.YAxis.IsZoomEnabled;
    if (!this.IsZoomEnabled)
      return;
    this.zoomRectangle = new OxyRect(this.StartPosition.X, this.StartPosition.Y, 0.0, 0.0);
    this.PlotView.ShowZoomRectangle(this.zoomRectangle);
    this.PlotView.SetCursorType(this.GetCursorType());
    e.Handled = true;
  }

  private CursorType GetCursorType()
  {
    if (this.XAxis == null)
      return CursorType.ZoomVertical;
    return this.YAxis == null ? CursorType.ZoomHorizontal : CursorType.ZoomRectangle;
  }
}
