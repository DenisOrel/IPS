// Decompiled with JetBrains decompiler
// Type: OxyPlot.PanManipulator
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class PanManipulator(IPlotView plotView) : MouseManipulator(plotView)
{
  private ScreenPoint PreviousPosition { get; set; }

  private bool IsPanEnabled { get; set; }

  public override void Completed(OxyMouseEventArgs e)
  {
    base.Completed(e);
    if (!this.IsPanEnabled)
      return;
    this.View.SetCursorType(CursorType.Default);
    e.Handled = true;
  }

  public override void Delta(OxyMouseEventArgs e)
  {
    base.Delta(e);
    if (!this.IsPanEnabled)
      return;
    if (this.XAxis != null)
      this.XAxis.Pan(this.PreviousPosition, e.Position);
    if (this.YAxis != null)
      this.YAxis.Pan(this.PreviousPosition, e.Position);
    this.PlotView.InvalidatePlot(false);
    this.PreviousPosition = e.Position;
    e.Handled = true;
  }

  public override void Started(OxyMouseEventArgs e)
  {
    base.Started(e);
    this.PreviousPosition = e.Position;
    this.IsPanEnabled = this.XAxis != null && this.XAxis.IsPanEnabled || this.YAxis != null && this.YAxis.IsPanEnabled;
    if (!this.IsPanEnabled)
      return;
    this.View.SetCursorType(CursorType.Pan);
    e.Handled = true;
  }
}
