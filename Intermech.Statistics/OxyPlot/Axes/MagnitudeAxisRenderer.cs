// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.MagnitudeAxisRenderer
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Axes;

public class MagnitudeAxisRenderer(IRenderContext rc, PlotModel plot) : AxisRendererBase(rc, plot)
{
  public override void Render(Axis axis, int pass)
  {
    base.Render(axis, pass);
    AngleAxis defaultAngleAxis = this.Plot.DefaultAngleAxis;
    if (defaultAngleAxis == null)
      throw new NullReferenceException("Angle axis should not be null.");
    defaultAngleAxis.UpdateActualMaxMin();
    double[] majorTicks = this.MajorTickValues.Where<double>((Func<double, bool>) (x => x > axis.ActualMinimum && x <= axis.ActualMaximum)).ToArray<double>();
    if (pass == 0 && this.MinorPen != null)
    {
      foreach (double x in this.MinorTickValues.Where<double>((Func<double, bool>) (x => x >= axis.ActualMinimum && x <= axis.ActualMaximum && !((IEnumerable<double>) majorTicks).Contains<double>(x))).ToArray<double>())
        this.RenderTick(axis, defaultAngleAxis, x, this.MinorPen);
    }
    if (pass == 0 && this.MajorPen != null)
    {
      foreach (double x in majorTicks)
        this.RenderTick(axis, defaultAngleAxis, x, this.MajorPen);
    }
    if (pass != 1)
      return;
    foreach (double x in majorTicks)
      this.RenderTickText(axis, x, (Axis) defaultAngleAxis);
  }

  private static double GetActualAngle(Axis axis, Axis angleAxis)
  {
    ScreenPoint screenPoint1 = axis.Transform(0.0, angleAxis.Angle, angleAxis);
    ScreenPoint screenPoint2 = axis.Transform(1.0, angleAxis.Angle, angleAxis);
    return Math.Atan2(screenPoint2.y - screenPoint1.y, screenPoint2.x - screenPoint1.x);
  }

  private static void GetTickTextAligment(
    double actualAngle,
    out HorizontalAlignment ha,
    out VerticalAlignment va)
  {
    if (actualAngle > 3.0 * Math.PI / 4.0 || actualAngle < -3.0 * Math.PI / 4.0)
    {
      ha = HorizontalAlignment.Center;
      va = VerticalAlignment.Top;
    }
    else if (actualAngle < -1.0 * Math.PI / 4.0)
    {
      ha = HorizontalAlignment.Right;
      va = VerticalAlignment.Middle;
    }
    else if (actualAngle > Math.PI / 4.0)
    {
      ha = HorizontalAlignment.Left;
      va = VerticalAlignment.Middle;
    }
    else
    {
      ha = HorizontalAlignment.Center;
      va = VerticalAlignment.Bottom;
    }
  }

  private void RenderTick(Axis axis, AngleAxis angleAxis, double x, OxyPen pen)
  {
    if (Math.Abs(Math.Abs(angleAxis.EndAngle - angleAxis.StartAngle) - 360.0) < 1E-06 && pen.ActualDashArray == null)
      this.RenderTickCircle(axis, (Axis) angleAxis, x, pen);
    else
      this.RenderTickArc(axis, angleAxis, x, pen);
  }

  private void RenderTickCircle(Axis axis, Axis angleAxis, double x, OxyPen pen)
  {
    double offset = angleAxis.Offset;
    ScreenPoint screenPoint = axis.Transform(axis.ActualMinimum, offset, angleAxis);
    double x1 = axis.Transform(x, offset, angleAxis).X;
    double num = x1 - screenPoint.X;
    double width = num * 2.0;
    double left = x1 - width;
    double top = screenPoint.Y - num;
    double height = width;
    this.RenderContext.DrawEllipse(new OxyRect(left, top, width, height), OxyColors.Undefined, pen.Color, pen.Thickness);
  }

  private void RenderTickArc(Axis axis, AngleAxis angleAxis, double x, OxyPen pen)
  {
    double actualMinimum = angleAxis.ActualMinimum;
    double actualMaximum = angleAxis.ActualMaximum;
    int num1 = (int) (90.0 * Math.Abs(angleAxis.EndAngle - angleAxis.StartAngle) / 360.0);
    double num2 = actualMinimum;
    double num3 = (actualMaximum - num2) / (double) (num1 - 1);
    List<ScreenPoint> points = new List<ScreenPoint>();
    for (int index = 0; index < num1; ++index)
    {
      double y = actualMinimum + (double) index * num3;
      points.Add(axis.Transform(x, y, (Axis) angleAxis));
    }
    this.RenderContext.DrawLine((IList<ScreenPoint>) points, pen.Color, pen.Thickness, pen.ActualDashArray);
  }

  private void RenderTickText(Axis axis, double x, Axis angleAxis)
  {
    double actualAngle = MagnitudeAxisRenderer.GetActualAngle(axis, angleAxis);
    double num1 = axis.AxisTickToLabelDistance * Math.Sin(actualAngle);
    double num2 = -axis.AxisTickToLabelDistance * Math.Cos(actualAngle);
    HorizontalAlignment ha;
    VerticalAlignment va;
    MagnitudeAxisRenderer.GetTickTextAligment(actualAngle, out ha, out va);
    ScreenPoint pt = axis.Transform(x, angleAxis.Angle, angleAxis);
    pt = new ScreenPoint(pt.X + num1, pt.Y + num2);
    string text = axis.FormatValue(x);
    this.RenderContext.DrawMathText(pt, text, axis.ActualTextColor, axis.ActualFont, axis.ActualFontSize, axis.ActualFontWeight, axis.Angle, ha, va);
  }
}
