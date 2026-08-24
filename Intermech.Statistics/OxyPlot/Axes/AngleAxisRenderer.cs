// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.AngleAxisRenderer
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Linq;

#nullable disable
namespace OxyPlot.Axes;

public class AngleAxisRenderer(IRenderContext rc, PlotModel plot) : AxisRendererBase(rc, plot)
{
  public override void Render(Axis axis, int pass)
  {
    AngleAxis angleAxis = (AngleAxis) axis;
    base.Render(axis, pass);
    MagnitudeAxis magnitudeAxis = this.Plot.DefaultMagnitudeAxis;
    if (magnitudeAxis == null)
      throw new InvalidOperationException("Magnitude axis not defined.");
    double scaledStartAngle = angleAxis.StartAngle / angleAxis.Scale;
    double scaledEndAngle = angleAxis.EndAngle / angleAxis.Scale;
    double num1 = Math.Abs(scaledEndAngle - scaledStartAngle);
    double eps = axis.MinorStep * 0.001;
    if (this.MinorPen != null)
    {
      int num2 = Math.Abs((int) (num1 / axis.ActualMinorStep));
      foreach (ScreenPoint screenPoint in this.MinorTickValues.Where<double>((Func<double, bool>) (x => x > Math.Min(scaledStartAngle, scaledEndAngle) - eps && x < Math.Max(scaledStartAngle, scaledEndAngle) + eps && !this.MajorTickValues.Contains(x))).Take<double>(num2 + 1).Select<double, ScreenPoint>((Func<double, ScreenPoint>) (x => magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, x, axis))))
        this.RenderContext.DrawLine(magnitudeAxis.MidPoint.x, magnitudeAxis.MidPoint.y, screenPoint.x, screenPoint.y, this.MinorPen, false);
    }
    int num3 = Math.Abs(Math.Abs(Math.Max(angleAxis.EndAngle, angleAxis.StartAngle) - Math.Min(angleAxis.StartAngle, angleAxis.EndAngle)) - 360.0) < 0.001 ? 1 : 0;
    int count = (int) (num1 / axis.ActualMajorStep);
    if (num3 == 0)
      ++count;
    if (this.MajorPen != null)
    {
      foreach (ScreenPoint screenPoint in this.MajorTickValues.Where<double>((Func<double, bool>) (x => x > Math.Min(scaledStartAngle, scaledEndAngle) - eps && x < Math.Max(scaledStartAngle, scaledEndAngle) + eps)).Take<double>(count).Select<double, ScreenPoint>((Func<double, ScreenPoint>) (x => magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, x, axis))).ToArray<ScreenPoint>())
        this.RenderContext.DrawLine(magnitudeAxis.MidPoint.x, magnitudeAxis.MidPoint.y, screenPoint.x, screenPoint.y, this.MajorPen, false);
    }
    foreach (double num4 in this.MajorLabelValues.Take<double>(count))
    {
      ScreenPoint pt = magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, num4, axis);
      double num5 = Math.Atan2(pt.y - magnitudeAxis.MidPoint.y, pt.x - magnitudeAxis.MidPoint.x);
      pt.x += Math.Cos(num5) * axis.AxisTickToLabelDistance;
      pt.y += Math.Sin(num5) * axis.AxisTickToLabelDistance;
      double angle = num5 * (180.0 / Math.PI);
      string text = axis.FormatValue(num4);
      HorizontalAlignment ha = HorizontalAlignment.Left;
      VerticalAlignment va = VerticalAlignment.Middle;
      if (Math.Abs(Math.Abs(angle) - 90.0) < 10.0)
      {
        ha = HorizontalAlignment.Center;
        va = angle >= 90.0 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
        angle = 0.0;
      }
      else if (angle > 90.0 || angle < -90.0)
      {
        angle -= 180.0;
        ha = HorizontalAlignment.Right;
      }
      this.RenderContext.DrawMathText(pt, text, axis.ActualTextColor, axis.ActualFont, axis.ActualFontSize, axis.ActualFontWeight, angle, ha, va);
    }
  }
}
