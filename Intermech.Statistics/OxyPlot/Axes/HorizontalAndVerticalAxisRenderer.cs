// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.HorizontalAndVerticalAxisRenderer
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Axes;

public class HorizontalAndVerticalAxisRenderer(IRenderContext rc, PlotModel plot) : AxisRendererBase(rc, plot)
{
  public override void Render(Axis axis, int pass)
  {
    base.Render(axis, pass);
    bool drawAxisLine = true;
    double num1 = axis.AxisDistance + axis.PositionTierMinShift;
    double num2 = axis.PositionTierSize - this.Plot.AxisTierDistance;
    double left = this.Plot.PlotArea.Left;
    double right = this.Plot.PlotArea.Right;
    double top = this.Plot.PlotArea.Top;
    double bottom = this.Plot.PlotArea.Bottom;
    double axisPosition = 0.0;
    double titlePosition = 0.0;
    switch (axis.Position)
    {
      case AxisPosition.Left:
        axisPosition = left - num1;
        break;
      case AxisPosition.Right:
        axisPosition = right + num1;
        break;
      case AxisPosition.Top:
        axisPosition = top - num1;
        break;
      case AxisPosition.Bottom:
        axisPosition = bottom + num1;
        break;
    }
    if (axis.PositionAtZeroCrossing)
    {
      Axis axis1 = axis.IsHorizontal() ? this.Plot.DefaultYAxis : this.Plot.DefaultXAxis;
      axisPosition = axis1.Transform(0.0);
      double val1_1 = axis1.Transform(axis1.ActualMinimum);
      double val2_1 = axis1.Transform(axis1.ActualMaximum);
      double val1_2 = Math.Min(val1_1, val2_1);
      double val1_3 = Math.Max(val1_1, val2_1);
      double val2_2 = axis.IsHorizontal() ? top : left;
      double val2_3 = axis.IsHorizontal() ? bottom : right;
      double num3 = Math.Max(val1_2, val2_2);
      double num4 = Math.Min(val1_3, val2_3);
      if (axisPosition < num3)
      {
        axisPosition = num3;
        if ((axis.IsHorizontal() ? this.Plot.PlotAreaBorderThickness.Top : this.Plot.PlotAreaBorderThickness.Left) > 0.0 && this.Plot.PlotAreaBorderColor.IsVisible())
          drawAxisLine = false;
      }
      if (axisPosition > num4)
      {
        axisPosition = num4;
        if ((axis.IsHorizontal() ? this.Plot.PlotAreaBorderThickness.Bottom : this.Plot.PlotAreaBorderThickness.Right) > 0.0 && this.Plot.PlotAreaBorderColor.IsVisible())
          drawAxisLine = false;
      }
    }
    switch (axis.Position)
    {
      case AxisPosition.Left:
        titlePosition = axisPosition - num2;
        break;
      case AxisPosition.Right:
        titlePosition = axisPosition + num2;
        break;
      case AxisPosition.Top:
        titlePosition = axisPosition - num2;
        break;
      case AxisPosition.Bottom:
        titlePosition = axisPosition + num2;
        break;
    }
    if (pass == 0)
      this.RenderMinorItems(axis, axisPosition);
    if (pass != 1)
      return;
    this.RenderMajorItems(axis, axisPosition, titlePosition, drawAxisLine);
    this.RenderAxisTitle(axis, titlePosition);
  }

  protected static double Lerp(double x0, double x1, double f) => x0 * (1.0 - f) + x1 * f;

  protected static void SnapTo(double target, ref double v, double eps = 0.5)
  {
    if (v <= target - eps || v >= target + eps)
      return;
    v = target;
  }

  protected virtual ScreenPoint GetAxisTitlePositionAndAlignment(
    Axis axis,
    double titlePosition,
    ref double angle,
    ref HorizontalAlignment halign,
    ref VerticalAlignment valign)
  {
    double num = axis.IsHorizontal() ? HorizontalAndVerticalAxisRenderer.Lerp(axis.ScreenMin.X, axis.ScreenMax.X, axis.TitlePosition) : HorizontalAndVerticalAxisRenderer.Lerp(axis.ScreenMax.Y, axis.ScreenMin.Y, axis.TitlePosition);
    if (axis.PositionAtZeroCrossing)
      num = HorizontalAndVerticalAxisRenderer.Lerp(axis.Transform(axis.ActualMaximum), axis.Transform(axis.ActualMinimum), axis.TitlePosition);
    switch (axis.Position)
    {
      case AxisPosition.Left:
        return new ScreenPoint(titlePosition, num);
      case AxisPosition.Right:
        valign = VerticalAlignment.Bottom;
        return new ScreenPoint(titlePosition, num);
      case AxisPosition.Top:
        halign = HorizontalAlignment.Center;
        valign = VerticalAlignment.Top;
        angle = 0.0;
        return new ScreenPoint(num, titlePosition);
      case AxisPosition.Bottom:
        halign = HorizontalAlignment.Center;
        valign = VerticalAlignment.Bottom;
        angle = 0.0;
        return new ScreenPoint(num, titlePosition);
      default:
        throw new ArgumentOutOfRangeException(nameof (axis));
    }
  }

  protected virtual void RenderAxisTitle(Axis axis, double titlePosition)
  {
    if (string.IsNullOrEmpty(axis.ActualTitle))
      return;
    bool flag = axis.IsHorizontal();
    OxySize? maxSize = new OxySize?();
    if (axis.ClipTitle)
    {
      double num;
      if (!flag)
      {
        ScreenPoint screenPoint = axis.ScreenMax;
        double y1 = screenPoint.Y;
        screenPoint = axis.ScreenMin;
        double y2 = screenPoint.Y;
        num = Math.Abs(y1 - y2);
      }
      else
      {
        ScreenPoint screenPoint = axis.ScreenMax;
        double x1 = screenPoint.X;
        screenPoint = axis.ScreenMin;
        double x2 = screenPoint.X;
        num = Math.Abs(x1 - x2);
      }
      maxSize = new OxySize?(new OxySize(num * axis.TitleClippingLength, double.MaxValue));
    }
    double angle = -90.0;
    HorizontalAlignment halign = HorizontalAlignment.Center;
    VerticalAlignment valign = VerticalAlignment.Top;
    this.RenderContext.DrawMathText(this.GetAxisTitlePositionAndAlignment(axis, titlePosition, ref angle, ref halign, ref valign), axis.ActualTitle, axis.ActualTitleColor, axis.ActualTitleFont, axis.ActualTitleFontSize, axis.ActualTitleFontWeight, angle, halign, valign, maxSize);
  }

  protected virtual void RenderMajorItems(
    Axis axis,
    double axisPosition,
    double titlePosition,
    bool drawAxisLine)
  {
    double num1 = axis.ActualMinorStep * 0.001;
    double actualMinimum = axis.ActualMinimum;
    double actualMaximum = axis.ActualMaximum;
    double left = this.Plot.PlotArea.Left;
    double right = this.Plot.PlotArea.Right;
    double top = this.Plot.PlotArea.Top;
    double bottom = this.Plot.PlotArea.Bottom;
    bool flag1 = axis.IsHorizontal();
    List<ScreenPoint> points1 = new List<ScreenPoint>();
    List<ScreenPoint> points2 = new List<ScreenPoint>();
    double x0;
    double x1;
    this.GetTickPositions(axis, axis.TickStyle, axis.MajorTickSize, axis.Position, out x0, out x1);
    Axis axis1 = axis.IsHorizontal() ? this.Plot.DefaultYAxis : this.Plot.DefaultXAxis;
    bool flag2 = axis.PositionAtZeroCrossing && axis1.PositionAtZeroCrossing;
    foreach (double majorTickValue in (IEnumerable<double>) this.MajorTickValues)
    {
      if (majorTickValue >= actualMinimum - num1 && majorTickValue <= actualMaximum + num1 && (!flag2 || Math.Abs(majorTickValue) >= num1))
      {
        double v = axis.Transform(majorTickValue);
        if (flag1)
        {
          HorizontalAndVerticalAxisRenderer.SnapTo(left, ref v);
          HorizontalAndVerticalAxisRenderer.SnapTo(right, ref v);
        }
        else
        {
          HorizontalAndVerticalAxisRenderer.SnapTo(top, ref v);
          HorizontalAndVerticalAxisRenderer.SnapTo(bottom, ref v);
        }
        if (this.MajorPen != null)
        {
          if (flag1)
          {
            points1.Add(new ScreenPoint(v, top));
            points1.Add(new ScreenPoint(v, bottom));
          }
          else
          {
            points1.Add(new ScreenPoint(left, v));
            points1.Add(new ScreenPoint(right, v));
          }
        }
        if (axis.TickStyle != TickStyle.None && axis.MajorTickSize > 0.0)
        {
          if (flag1)
          {
            points2.Add(new ScreenPoint(v, axisPosition + x0));
            points2.Add(new ScreenPoint(v, axisPosition + x1));
          }
          else
          {
            points2.Add(new ScreenPoint(axisPosition + x0, v));
            points2.Add(new ScreenPoint(axisPosition + x1, v));
          }
        }
      }
    }
    foreach (double majorLabelValue in (IEnumerable<double>) this.MajorLabelValues)
    {
      if (majorLabelValue >= actualMinimum - num1 && majorLabelValue <= actualMaximum + num1 && (!flag2 || Math.Abs(majorLabelValue) >= num1))
      {
        double v = axis.Transform(majorLabelValue);
        if (flag1)
        {
          HorizontalAndVerticalAxisRenderer.SnapTo(left, ref v);
          HorizontalAndVerticalAxisRenderer.SnapTo(right, ref v);
        }
        else
        {
          HorizontalAndVerticalAxisRenderer.SnapTo(top, ref v);
          HorizontalAndVerticalAxisRenderer.SnapTo(bottom, ref v);
        }
        ScreenPoint pt = new ScreenPoint();
        HorizontalAlignment ha = HorizontalAlignment.Right;
        VerticalAlignment va = VerticalAlignment.Middle;
        switch (axis.Position)
        {
          case AxisPosition.Left:
            pt = new ScreenPoint(axisPosition + x1 - axis.AxisTickToLabelDistance, v);
            this.GetRotatedAlignments(axis.Angle, -90.0, out ha, out va);
            break;
          case AxisPosition.Right:
            pt = new ScreenPoint(axisPosition + x1 + axis.AxisTickToLabelDistance, v);
            this.GetRotatedAlignments(axis.Angle, 90.0, out ha, out va);
            break;
          case AxisPosition.Top:
            pt = new ScreenPoint(v, axisPosition + x1 - axis.AxisTickToLabelDistance);
            this.GetRotatedAlignments(axis.Angle, 0.0, out ha, out va);
            break;
          case AxisPosition.Bottom:
            pt = new ScreenPoint(v, axisPosition + x1 + axis.AxisTickToLabelDistance);
            this.GetRotatedAlignments(axis.Angle, -180.0, out ha, out va);
            break;
        }
        string text = axis.FormatValue(majorLabelValue);
        this.RenderContext.DrawMathText(pt, text, axis.ActualTextColor, axis.ActualFont, axis.ActualFontSize, axis.ActualFontWeight, axis.Angle, ha, va);
      }
    }
    if (axis.ExtraGridlines != null && this.ExtraPen != null)
    {
      foreach (double extraGridline in axis.ExtraGridlines)
      {
        if (this.IsWithin(extraGridline, actualMinimum, actualMaximum))
        {
          double num2 = axis.Transform(extraGridline);
          if (flag1)
            this.RenderContext.DrawLine(num2, top, num2, bottom, this.ExtraPen);
          else
            this.RenderContext.DrawLine(left, num2, right, num2, this.ExtraPen);
        }
      }
    }
    if (drawAxisLine)
    {
      if (flag1)
        this.RenderContext.DrawLine(axis.Transform(actualMinimum), axisPosition, axis.Transform(actualMaximum), axisPosition, this.AxislinePen);
      else
        this.RenderContext.DrawLine(axisPosition, axis.Transform(actualMinimum), axisPosition, axis.Transform(actualMaximum), this.AxislinePen);
    }
    if (this.MajorPen != null)
      this.RenderContext.DrawLineSegments((IList<ScreenPoint>) points1, this.MajorPen);
    if (this.MajorTickPen == null)
      return;
    this.RenderContext.DrawLineSegments((IList<ScreenPoint>) points2, this.MajorTickPen);
  }

  protected virtual void RenderMinorItems(Axis axis, double axisPosition)
  {
    double num = axis.ActualMinorStep * 0.001;
    double actualMinimum = axis.ActualMinimum;
    double actualMaximum = axis.ActualMaximum;
    double left = this.Plot.PlotArea.Left;
    double right = this.Plot.PlotArea.Right;
    double top = this.Plot.PlotArea.Top;
    double bottom = this.Plot.PlotArea.Bottom;
    bool flag = axis.IsHorizontal();
    List<ScreenPoint> points1 = new List<ScreenPoint>();
    List<ScreenPoint> points2 = new List<ScreenPoint>();
    double x0;
    double x1;
    this.GetTickPositions(axis, axis.TickStyle, axis.MinorTickSize, axis.Position, out x0, out x1);
    foreach (double minorTickValue in (IEnumerable<double>) this.MinorTickValues)
    {
      if (minorTickValue >= actualMinimum - num && minorTickValue <= actualMaximum + num && !this.MajorTickValues.Contains(minorTickValue) && (!axis.PositionAtZeroCrossing || Math.Abs(minorTickValue) >= num))
      {
        double v = axis.Transform(minorTickValue);
        if (flag)
        {
          HorizontalAndVerticalAxisRenderer.SnapTo(left, ref v);
          HorizontalAndVerticalAxisRenderer.SnapTo(right, ref v);
        }
        else
        {
          HorizontalAndVerticalAxisRenderer.SnapTo(top, ref v);
          HorizontalAndVerticalAxisRenderer.SnapTo(bottom, ref v);
        }
        if (this.MinorPen != null)
        {
          if (flag)
          {
            points1.Add(new ScreenPoint(v, top));
            points1.Add(new ScreenPoint(v, bottom));
          }
          else
          {
            if (v >= top)
              ;
            points1.Add(new ScreenPoint(left, v));
            points1.Add(new ScreenPoint(right, v));
          }
        }
        if (axis.TickStyle != TickStyle.None && axis.MinorTickSize > 0.0)
        {
          if (flag)
          {
            points2.Add(new ScreenPoint(v, axisPosition + x0));
            points2.Add(new ScreenPoint(v, axisPosition + x1));
          }
          else
          {
            points2.Add(new ScreenPoint(axisPosition + x0, v));
            points2.Add(new ScreenPoint(axisPosition + x1, v));
          }
        }
      }
    }
    if (this.MinorPen != null)
      this.RenderContext.DrawLineSegments((IList<ScreenPoint>) points1, this.MinorPen);
    if (this.MinorTickPen == null)
      return;
    this.RenderContext.DrawLineSegments((IList<ScreenPoint>) points2, this.MinorTickPen);
  }

  private void GetRotatedAlignments(
    double boxAngle,
    double axisAngle,
    out HorizontalAlignment ha,
    out VerticalAlignment va)
  {
    double val2 = (axisAngle + 360.0) % 360.0 - 180.0;
    ha = boxAngle < Math.Min(axisAngle, val2) || boxAngle >= Math.Max(axisAngle, val2) ? HorizontalAlignment.Right : HorizontalAlignment.Left;
    if (axisAngle < 0.0)
      ha = (HorizontalAlignment) ((int) ha * -1);
    va = VerticalAlignment.Middle;
    if (Math.Abs(boxAngle - val2) < 10.0 || Math.Abs(boxAngle - axisAngle) < 10.0)
      ha = HorizontalAlignment.Center;
    if (Math.Abs(boxAngle - axisAngle) < 10.0)
      va = VerticalAlignment.Bottom;
    if (Math.Abs(boxAngle - val2) >= 10.0)
      return;
    va = VerticalAlignment.Top;
  }
}
