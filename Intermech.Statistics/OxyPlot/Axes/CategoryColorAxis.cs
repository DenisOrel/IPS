// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.CategoryColorAxis
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Axes;

public class CategoryColorAxis : CategoryAxis, IColorAxis, IPlotElement
{
  public CategoryColorAxis() => this.Palette = new OxyPalette();

  public OxyColor InvalidCategoryColor { get; set; }

  public OxyPalette Palette { get; set; }

  public OxyColor GetColor(int paletteIndex)
  {
    return paletteIndex == -1 || paletteIndex >= this.Palette.Colors.Count ? this.InvalidCategoryColor : this.Palette.Colors[paletteIndex];
  }

  public int GetPaletteIndex(double value) => (int) value;

  public override void Render(IRenderContext rc, int pass)
  {
    if (this.Position == AxisPosition.None)
      return;
    if (pass == 0)
    {
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
          left = plotArea.Left - this.PositionTierMinShift - width;
          plotArea = this.PlotModel.PlotArea;
          top = plotArea.Top;
          break;
        case AxisPosition.Right:
          plotArea = this.PlotModel.PlotArea;
          left = plotArea.Right + this.PositionTierMinShift;
          plotArea = this.PlotModel.PlotArea;
          top = plotArea.Top;
          break;
        case AxisPosition.Top:
          plotArea = this.PlotModel.PlotArea;
          left = plotArea.Left;
          plotArea = this.PlotModel.PlotArea;
          top = plotArea.Top - this.PositionTierMinShift - height;
          break;
        case AxisPosition.Bottom:
          plotArea = this.PlotModel.PlotArea;
          left = plotArea.Left;
          plotArea = this.PlotModel.PlotArea;
          top = plotArea.Bottom + this.PositionTierMinShift;
          break;
      }
      Action<double, double, OxyColor> action = (Action<double, double, OxyColor>) ((ylow, yhigh, color) =>
      {
        double num1 = Math.Min(ylow, yhigh);
        double num2 = Math.Max(ylow, yhigh);
        rc.DrawRectangle(this.IsHorizontal() ? new OxyRect(num1, top, num2 - num1, height) : new OxyRect(left, num1, width, num2 - num1), color, OxyColors.Undefined);
      });
      IList<double> majorLabelValues;
      this.GetTickValues(out majorLabelValues, out IList<double> _, out IList<double> _);
      int count = this.Palette.Colors.Count;
      for (int index = 0; index < count; ++index)
      {
        double num3 = this.Transform(this.GetLowValue(index, majorLabelValues));
        double num4 = this.Transform(this.GetHighValue(index, majorLabelValues));
        action(num3, num4, this.Palette.Colors[index]);
      }
    }
    base.Render(rc, pass);
  }

  protected double GetHighValue(int paletteIndex)
  {
    IList<double> majorLabelValues;
    this.GetTickValues(out majorLabelValues, out IList<double> _, out IList<double> _);
    return this.GetHighValue(paletteIndex, majorLabelValues);
  }

  private double GetHighValue(int paletteIndex, IList<double> majorLabelValues)
  {
    return paletteIndex < this.Palette.Colors.Count - 1 ? (majorLabelValues[paletteIndex] + majorLabelValues[paletteIndex + 1]) / 2.0 : this.ActualMaximum;
  }

  private double GetLowValue(int paletteIndex, IList<double> majorLabelValues)
  {
    return paletteIndex != 0 ? (majorLabelValues[paletteIndex - 1] + majorLabelValues[paletteIndex]) / 2.0 : this.ActualMinimum;
  }
}
