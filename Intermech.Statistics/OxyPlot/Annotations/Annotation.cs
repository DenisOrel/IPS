// Decompiled with JetBrains decompiler
// Type: OxyPlot.Annotations.Annotation
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;

#nullable disable
namespace OxyPlot.Annotations;

public abstract class Annotation : PlotElement
{
  protected Annotation() => this.Layer = AnnotationLayer.AboveSeries;

  public AnnotationLayer Layer { get; set; }

  public Axis XAxis { get; private set; }

  public string XAxisKey { get; set; }

  public Axis YAxis { get; private set; }

  public string YAxisKey { get; set; }

  public void EnsureAxes()
  {
    this.XAxis = this.PlotModel.GetAxisOrDefault(this.XAxisKey, this.PlotModel.DefaultXAxis);
    this.YAxis = this.PlotModel.GetAxisOrDefault(this.YAxisKey, this.PlotModel.DefaultYAxis);
  }

  public virtual void Render(IRenderContext rc)
  {
  }

  public ScreenPoint Transform(double x, double y) => this.XAxis.Transform(x, y, this.YAxis);

  public ScreenPoint Transform(DataPoint p) => this.XAxis.Transform(p.X, p.Y, this.YAxis);

  public DataPoint InverseTransform(ScreenPoint position)
  {
    return this.XAxis.InverseTransform(position.X, position.Y, this.YAxis);
  }

  protected override HitTestResult HitTestOverride(HitTestArguments args) => (HitTestResult) null;

  protected OxyRect GetClippingRect()
  {
    ScreenPoint screenPoint1 = this.XAxis.ScreenMin;
    double x1 = screenPoint1.X;
    screenPoint1 = this.XAxis.ScreenMax;
    double x2 = screenPoint1.X;
    double left = Math.Min(x1, x2);
    ScreenPoint screenPoint2 = this.YAxis.ScreenMin;
    double y1 = screenPoint2.Y;
    screenPoint2 = this.YAxis.ScreenMax;
    double y2 = screenPoint2.Y;
    double top = Math.Min(y1, y2);
    ScreenPoint screenPoint3 = this.XAxis.ScreenMin;
    double x3 = screenPoint3.X;
    screenPoint3 = this.XAxis.ScreenMax;
    double x4 = screenPoint3.X;
    double num1 = Math.Max(x3, x4);
    ScreenPoint screenPoint4 = this.YAxis.ScreenMin;
    double y3 = screenPoint4.Y;
    screenPoint4 = this.YAxis.ScreenMax;
    double y4 = screenPoint4.Y;
    double num2 = Math.Max(y3, y4);
    return new OxyRect(left, top, num1 - left, num2 - top);
  }
}
