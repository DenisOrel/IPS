// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.LinearColorAxis
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Axes;

public class LinearColorAxis : LinearAxis, IColorAxis, IPlotElement
{
  public LinearColorAxis()
  {
    this.Position = AxisPosition.None;
    this.AxisDistance = 20.0;
    this.IsPanEnabled = false;
    this.IsZoomEnabled = false;
    this.Palette = OxyPalettes.Jet(200);
    this.LowColor = OxyColors.Undefined;
    this.HighColor = OxyColors.Undefined;
    this.InvalidNumberColor = OxyColors.Gray;
  }

  public OxyColor InvalidNumberColor { get; set; }

  public OxyColor HighColor { get; set; }

  public OxyColor LowColor { get; set; }

  public OxyPalette Palette { get; set; }

  public bool RenderAsImage { get; set; }

  public override bool IsXyAxis() => false;

  public OxyColor GetColor(int paletteIndex)
  {
    if (paletteIndex == int.MinValue)
      return this.InvalidNumberColor;
    if (paletteIndex == 0)
      return this.LowColor;
    return paletteIndex == this.Palette.Colors.Count + 1 ? this.HighColor : this.Palette.Colors[paletteIndex - 1];
  }

  public IEnumerable<OxyColor> GetColors()
  {
    yield return this.LowColor;
    foreach (OxyColor color in (IEnumerable<OxyColor>) this.Palette.Colors)
      yield return color;
    yield return this.HighColor;
  }

  public int GetPaletteIndex(double value)
  {
    if (double.IsNaN(value))
      return int.MinValue;
    if (!this.LowColor.IsUndefined() && value < this.ActualMinimum)
      return 0;
    if (!this.HighColor.IsUndefined() && value > this.ActualMaximum)
      return this.Palette.Colors.Count + 1;
    int paletteIndex = 1 + (int) ((value - this.ActualMinimum) / (this.ActualMaximum - this.ActualMinimum) * (double) this.Palette.Colors.Count);
    if (paletteIndex < 1)
      paletteIndex = 1;
    if (paletteIndex > this.Palette.Colors.Count)
      paletteIndex = this.Palette.Colors.Count;
    return paletteIndex;
  }

  public override void Render(IRenderContext rc, int pass)
  {
    if (this.Position == AxisPosition.None)
      return;
    if (this.Palette == null)
      throw new InvalidOperationException("No Palette defined for color axis.");
    if (pass == 0)
    {
      double axisDistance = this.AxisDistance;
      double left = this.PlotModel.PlotArea.Left;
      OxyRect plotArea = this.PlotModel.PlotArea;
      double top = plotArea.Top;
      double width = this.MajorTickSize - 2.0;
      double height = this.MajorTickSize - 2.0;
      switch (this.Position)
      {
        case AxisPosition.Left:
          plotArea = this.PlotModel.PlotArea;
          left = plotArea.Left - this.PositionTierMinShift - width - axisDistance;
          plotArea = this.PlotModel.PlotArea;
          top = plotArea.Top;
          break;
        case AxisPosition.Right:
          plotArea = this.PlotModel.PlotArea;
          left = plotArea.Right + this.PositionTierMinShift + axisDistance;
          plotArea = this.PlotModel.PlotArea;
          top = plotArea.Top;
          break;
        case AxisPosition.Top:
          plotArea = this.PlotModel.PlotArea;
          left = plotArea.Left;
          plotArea = this.PlotModel.PlotArea;
          top = plotArea.Top - this.PositionTierMinShift - height - axisDistance;
          break;
        case AxisPosition.Bottom:
          plotArea = this.PlotModel.PlotArea;
          left = plotArea.Left;
          plotArea = this.PlotModel.PlotArea;
          top = plotArea.Bottom + this.PositionTierMinShift + axisDistance;
          break;
      }
      if (this.RenderAsImage)
      {
        double num1 = this.Transform(this.ActualMaximum) - this.Transform(this.ActualMinimum);
        bool reverse = num1 > 0.0;
        double num2 = Math.Abs(num1);
        if (this.IsHorizontal())
        {
          OxyImage colorAxisImage = this.GenerateColorAxisImage(reverse);
          rc.DrawImage(colorAxisImage, left, top, num2, height, 1.0, true);
        }
        else
        {
          OxyImage colorAxisImage = this.GenerateColorAxisImage(reverse);
          rc.DrawImage(colorAxisImage, left, top, width, num2, 1.0, true);
        }
      }
      else
      {
        Action<double, double, OxyColor> action = (Action<double, double, OxyColor>) ((ylow, yhigh, color) =>
        {
          double num3 = Math.Min(ylow, yhigh);
          double num4 = Math.Max(ylow, yhigh) + 0.5;
          rc.DrawRectangle(this.IsHorizontal() ? new OxyRect(num3, top, num4 - num3, height) : new OxyRect(left, num3, width, num4 - num3), color, OxyColors.Undefined);
        });
        int count = this.Palette.Colors.Count;
        for (int index = 0; index < count; ++index)
        {
          double num5 = this.Transform(this.GetLowValue(index));
          double num6 = this.Transform(this.GetHighValue(index));
          action(num5, num6, this.Palette.Colors[index]);
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
    }
    base.Render(rc, pass);
  }

  protected double GetHighValue(int paletteIndex) => this.GetLowValue(paletteIndex + 1);

  protected double GetLowValue(int paletteIndex)
  {
    return (double) paletteIndex / (double) this.Palette.Colors.Count * (this.ActualMaximum - this.ActualMinimum) + this.ActualMinimum;
  }

  private OxyImage GenerateColorAxisImage(bool reverse)
  {
    int count = this.Palette.Colors.Count;
    OxyColor[,] pixels = this.IsHorizontal() ? new OxyColor[count, 1] : new OxyColor[1, count];
    for (int index1 = 0; index1 < count; ++index1)
    {
      OxyColor color = this.Palette.Colors[index1];
      int index2 = reverse ? count - 1 - index1 : index1;
      if (this.IsHorizontal())
        pixels[index2, 0] = color;
      else
        pixels[0, index2] = color;
    }
    return OxyImage.Create(pixels, ImageFormat.Png);
  }
}
