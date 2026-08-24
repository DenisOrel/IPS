// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.ErrorColumnSeries
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public class ErrorColumnSeries : ColumnSeries
{
  public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}, Error: {Error:0.###}";

  public ErrorColumnSeries()
  {
    this.ErrorWidth = 0.4;
    this.ErrorStrokeThickness = 1.0;
    this.TrackerFormatString = "{0}\n{1}: {2}, Error: {Error:0.###}";
  }

  public double ErrorStrokeThickness { get; set; }

  public double ErrorWidth { get; set; }

  protected internal override void UpdateMaxMin()
  {
    base.UpdateMaxMin();
    if (this.ValidItems == null || this.ValidItems.Count == 0)
      return;
    CategoryAxis categoryAxis = this.GetCategoryAxis();
    double val1_1 = double.MaxValue;
    double val1_2 = double.MinValue;
    if (this.IsStacked)
    {
      List<string> actualLabels = this.GetCategoryAxis().ActualLabels;
      for (int i = 0; i < actualLabels.Count; i++)
      {
        int j = 0;
        List<BarItemBase> list1 = this.ValidItems.Where<BarItemBase>((Func<BarItemBase, bool>) (item => item.GetCategoryIndex(j++) == i)).ToList<BarItemBase>();
        List<double> list2 = list1.Select<BarItemBase, double>((Func<BarItemBase, double>) (item => item.Value)).Concat<double>((IEnumerable<double>) new double[1]).ToList<double>();
        double newValue1 = list2.Where<double>((Func<double, bool>) (v => v <= 0.0)).Sum();
        double newValue2 = list2.Where<double>((Func<double, bool>) (v => v >= 0.0)).Sum() + ((ErrorColumnItem) list1.Last<BarItemBase>()).Error;
        int stackIndex = categoryAxis.GetStackIndex(this.StackGroup);
        double currentMinValue = categoryAxis.GetCurrentMinValue(stackIndex, i);
        if (!double.IsNaN(currentMinValue))
          newValue1 += currentMinValue;
        categoryAxis.SetCurrentMinValue(stackIndex, i, newValue1);
        double currentMaxValue = categoryAxis.GetCurrentMaxValue(stackIndex, i);
        if (!double.IsNaN(currentMaxValue))
          newValue2 += currentMaxValue;
        categoryAxis.SetCurrentMaxValue(stackIndex, i, newValue2);
        val1_1 = Math.Min(val1_1, newValue1 + this.BaseValue);
        val1_2 = Math.Max(val1_2, newValue2 + this.BaseValue);
      }
    }
    else
    {
      List<double> list3 = this.ValidItems.Select<BarItemBase, double>((Func<BarItemBase, double>) (item => item.Value - ((ErrorColumnItem) item).Error)).Concat<double>((IEnumerable<double>) new double[1]).ToList<double>();
      List<double> list4 = this.ValidItems.Select<BarItemBase, double>((Func<BarItemBase, double>) (item => item.Value + ((ErrorColumnItem) item).Error)).Concat<double>((IEnumerable<double>) new double[1]).ToList<double>();
      val1_1 = list3.Min();
      val1_2 = list4.Max();
      if (this.BaseValue < val1_1)
        val1_1 = this.BaseValue;
      if (this.BaseValue > val1_2)
        val1_2 = this.BaseValue;
    }
    if (this.GetValueAxis().IsVertical())
    {
      this.MinY = val1_1;
      this.MaxY = val1_2;
    }
    else
    {
      this.MinX = val1_1;
      this.MaxX = val1_2;
    }
  }

  protected override void RenderItem(
    IRenderContext rc,
    OxyRect clippingRect,
    double topValue,
    double categoryValue,
    double actualBarWidth,
    BarItemBase item,
    OxyRect rect)
  {
    base.RenderItem(rc, clippingRect, topValue, categoryValue, actualBarWidth, item, rect);
    if (!(item is ErrorColumnItem errorColumnItem))
      return;
    double y1 = topValue - errorColumnItem.Error;
    double y2 = topValue + errorColumnItem.Error;
    double num1 = 0.5 - this.ErrorWidth / 2.0;
    double num2 = 0.5 + this.ErrorWidth / 2.0;
    double x1 = categoryValue + num1 * actualBarWidth;
    double x2 = categoryValue + 0.5 * actualBarWidth;
    double x3 = categoryValue + num2 * actualBarWidth;
    ScreenPoint screenPoint1 = this.Transform(x2, y1);
    ScreenPoint screenPoint2 = this.Transform(x2, y2);
    IRenderContext rc1 = rc;
    OxyRect clippingRectangle1 = clippingRect;
    List<ScreenPoint> points1 = new List<ScreenPoint>();
    points1.Add(screenPoint1);
    points1.Add(screenPoint2);
    OxyColor strokeColor1 = this.StrokeColor;
    double errorStrokeThickness1 = this.ErrorStrokeThickness;
    rc1.DrawClippedLine(clippingRectangle1, (IList<ScreenPoint>) points1, 0.0, strokeColor1, errorStrokeThickness1, (double[]) null, LineJoin.Miter, true);
    if (this.ErrorWidth <= 0.0)
      return;
    ScreenPoint screenPoint3 = this.Transform(x1, y1);
    ScreenPoint screenPoint4 = this.Transform(x3, y1);
    IRenderContext rc2 = rc;
    OxyRect clippingRectangle2 = clippingRect;
    List<ScreenPoint> points2 = new List<ScreenPoint>();
    points2.Add(screenPoint3);
    points2.Add(screenPoint4);
    OxyColor strokeColor2 = this.StrokeColor;
    double errorStrokeThickness2 = this.ErrorStrokeThickness;
    rc2.DrawClippedLine(clippingRectangle2, (IList<ScreenPoint>) points2, 0.0, strokeColor2, errorStrokeThickness2, (double[]) null, LineJoin.Miter, true);
    ScreenPoint screenPoint5 = this.Transform(x1, y2);
    ScreenPoint screenPoint6 = this.Transform(x3, y2);
    IRenderContext rc3 = rc;
    OxyRect clippingRectangle3 = clippingRect;
    List<ScreenPoint> points3 = new List<ScreenPoint>();
    points3.Add(screenPoint5);
    points3.Add(screenPoint6);
    OxyColor strokeColor3 = this.StrokeColor;
    double errorStrokeThickness3 = this.ErrorStrokeThickness;
    rc3.DrawClippedLine(clippingRectangle3, (IList<ScreenPoint>) points3, 0.0, strokeColor3, errorStrokeThickness3, (double[]) null, LineJoin.Miter, true);
  }
}
