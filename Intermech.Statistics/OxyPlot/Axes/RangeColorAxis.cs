// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.RangeColorAxis
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Axes;

public class RangeColorAxis : LinearAxis, IColorAxis, IPlotElement
{
  private readonly List<RangeColorAxis.ColorRange> ranges = new List<RangeColorAxis.ColorRange>();

  public RangeColorAxis()
  {
    this.Position = AxisPosition.None;
    this.AxisDistance = 20.0;
    this.LowColor = OxyColors.Undefined;
    this.HighColor = OxyColors.Undefined;
    this.InvalidNumberColor = OxyColors.Gray;
    this.IsPanEnabled = false;
    this.IsZoomEnabled = false;
  }

  public OxyColor InvalidNumberColor { get; set; }

  public OxyColor HighColor { get; set; }

  public OxyColor LowColor { get; set; }

  public void AddRange(double lowerBound, double upperBound, OxyColor color)
  {
    this.ranges.Add(new RangeColorAxis.ColorRange()
    {
      LowerBound = lowerBound,
      UpperBound = upperBound,
      Color = color
    });
  }

  public void ClearRanges() => this.ranges.Clear();

  public int GetPaletteIndex(double value)
  {
    if (!this.LowColor.IsUndefined() && value < this.ranges[0].LowerBound)
      return -1;
    if (!this.HighColor.IsUndefined() && value > this.ranges[this.ranges.Count - 1].UpperBound)
      return this.ranges.Count;
    for (int index = 0; index < this.ranges.Count; ++index)
    {
      RangeColorAxis.ColorRange range = this.ranges[index];
      if (range.LowerBound <= value && range.UpperBound > value)
        return index;
    }
    return int.MinValue;
  }

  public OxyColor GetColor(int paletteIndex)
  {
    if (paletteIndex == int.MinValue)
      return this.InvalidNumberColor;
    if (paletteIndex == -1)
      return this.LowColor;
    return paletteIndex == this.ranges.Count ? this.HighColor : this.ranges[paletteIndex].Color;
  }

  public override void Render(IRenderContext rc, int pass)
  {
    if (this.Position == AxisPosition.None)
      return;
    if (pass == 0)
    {
      double axisDistance = this.AxisDistance;
      OxyRect plotArea = this.PlotModel.PlotArea;
      double left = plotArea.Left;
      plotArea = this.PlotModel.PlotArea;
      double top = plotArea.Top;
      double width = this.MajorTickSize - 2.0;
      double height = this.MajorTickSize - 2.0;
      switch (this.Position)
      {
        case AxisPosition.Left:
          plotArea = this.PlotModel.PlotArea;
          left = plotArea.Left - 0.0 - width - axisDistance;
          plotArea = this.PlotModel.PlotArea;
          top = plotArea.Top;
          break;
        case AxisPosition.Right:
          plotArea = this.PlotModel.PlotArea;
          left = plotArea.Right + 0.0 + axisDistance;
          plotArea = this.PlotModel.PlotArea;
          top = plotArea.Top;
          break;
        case AxisPosition.Top:
          plotArea = this.PlotModel.PlotArea;
          left = plotArea.Left;
          plotArea = this.PlotModel.PlotArea;
          top = plotArea.Top - 0.0 - height - axisDistance;
          break;
        case AxisPosition.Bottom:
          plotArea = this.PlotModel.PlotArea;
          left = plotArea.Left;
          plotArea = this.PlotModel.PlotArea;
          top = plotArea.Bottom + 0.0 + axisDistance;
          break;
      }
      Action<double, double, OxyColor> action = (Action<double, double, OxyColor>) ((ylow, yhigh, color) =>
      {
        double num1 = Math.Min(ylow, yhigh);
        double num2 = Math.Max(ylow, yhigh);
        rc.DrawRectangle(this.IsHorizontal() ? new OxyRect(num1, top, num2 - num1, height) : new OxyRect(left, num1, width, num2 - num1), color, OxyColors.Undefined);
      });
      foreach (RangeColorAxis.ColorRange range in this.ranges)
      {
        double num3 = this.Transform(range.LowerBound);
        double num4 = this.Transform(range.UpperBound);
        double num5 = this.Transform(this.ActualMaximum);
        double num6 = this.Transform(this.ActualMinimum);
        if (num3 >= num5 && num4 <= num6)
        {
          if (num3 > num6)
            num3 = num6;
          if (num4 < num5)
            num4 = num5;
          action(num3, num4, range.Color);
        }
      }
      double num7 = 10.0;
      if (this.IsHorizontal())
        num7 *= -1.0;
      if (!this.LowColor.IsUndefined())
      {
        double num8 = this.Transform(this.ActualMinimum);
        action(num8, num8 + num7, this.LowColor);
      }
      if (!this.HighColor.IsUndefined())
      {
        double num9 = this.Transform(this.ActualMaximum);
        action(num9, num9 - num7, this.HighColor);
      }
    }
    new HorizontalAndVerticalAxisRenderer(rc, this.PlotModel).Render((Axis) this, pass);
  }

  private class ColorRange
  {
    public OxyColor Color { get; set; }

    public double LowerBound { get; set; }

    public double UpperBound { get; set; }
  }
}
