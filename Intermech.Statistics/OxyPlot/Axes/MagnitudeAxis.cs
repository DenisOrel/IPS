// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.MagnitudeAxis
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot.Axes;

public class MagnitudeAxis : LinearAxis
{
  public MagnitudeAxis()
  {
    this.Position = AxisPosition.None;
    this.IsPanEnabled = false;
    this.IsZoomEnabled = false;
    this.MajorGridlineStyle = LineStyle.Solid;
    this.MinorGridlineStyle = LineStyle.Solid;
  }

  internal ScreenPoint MidPoint { get; set; }

  public override DataPoint InverseTransform(double x, double y, Axis yaxis)
  {
    if (!(yaxis is AngleAxis angleAxis))
      throw new InvalidOperationException("Polar angle axis not defined!");
    x -= this.MidPoint.x;
    y -= this.MidPoint.y;
    y *= -1.0;
    double num = Math.Atan2(y, x);
    x = Math.Sqrt(x * x + y * y) / this.Scale + this.Offset;
    double scale = angleAxis.Scale;
    y = num / scale + angleAxis.Offset;
    return new DataPoint(x, y);
  }

  public override bool IsXyAxis() => false;

  public override void Render(IRenderContext rc, int pass)
  {
    new MagnitudeAxisRenderer(rc, this.PlotModel).Render((Axis) this, pass);
  }

  public override ScreenPoint Transform(double x, double y, Axis yaxis)
  {
    if (!(yaxis is AngleAxis angleAxis))
      throw new InvalidOperationException("Polar angle axis not defined!");
    double num1 = (x - this.Offset) * this.Scale;
    double num2 = (y - angleAxis.Offset) * angleAxis.Scale;
    return new ScreenPoint(this.MidPoint.x + num1 * Math.Cos(num2 / 180.0 * Math.PI), this.MidPoint.y - num1 * Math.Sin(num2 / 180.0 * Math.PI));
  }

  internal override void UpdateTransform(OxyRect bounds)
  {
    double left = bounds.Left;
    double right = bounds.Right;
    double bottom = bounds.Bottom;
    double top = bounds.Top;
    this.ScreenMin = new ScreenPoint(left, top);
    this.ScreenMax = new ScreenPoint(right, bottom);
    this.MidPoint = new ScreenPoint((left + right) / 2.0, (bottom + top) / 2.0);
    this.SetTransform(0.5 * Math.Min(Math.Abs(right - left), Math.Abs(top - bottom)) / (this.ActualMaximum - this.ActualMinimum), this.ActualMinimum);
  }
}
