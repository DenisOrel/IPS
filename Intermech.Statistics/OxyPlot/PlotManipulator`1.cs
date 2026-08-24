// Decompiled with JetBrains decompiler
// Type: OxyPlot.PlotManipulator`1
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;

#nullable disable
namespace OxyPlot;

public abstract class PlotManipulator<T> : ManipulatorBase<T> where T : OxyInputEventArgs
{
  protected PlotManipulator(IPlotView view)
    : base((IView) view)
  {
    this.PlotView = view;
  }

  public IPlotView PlotView { get; private set; }

  protected Axis XAxis { get; set; }

  protected Axis YAxis { get; set; }

  protected DataPoint InverseTransform(double x, double y)
  {
    if (this.XAxis != null)
      return this.XAxis.InverseTransform(x, y, this.YAxis);
    return this.YAxis != null ? new DataPoint(0.0, this.YAxis.InverseTransform(y)) : new DataPoint();
  }

  protected void AssignAxes(ScreenPoint position)
  {
    Axis xaxis;
    Axis yaxis;
    if (this.PlotView.ActualModel != null)
    {
      this.PlotView.ActualModel.GetAxesFromPoint(position, out xaxis, out yaxis);
    }
    else
    {
      xaxis = (Axis) null;
      yaxis = (Axis) null;
    }
    this.XAxis = xaxis;
    this.YAxis = yaxis;
  }
}
