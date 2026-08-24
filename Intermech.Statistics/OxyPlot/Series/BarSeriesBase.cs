// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.BarSeriesBase
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public abstract class BarSeriesBase : CategorizedSeries, IStackableSeries
{
  public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}";
  private OxyColor defaultFillColor;

  protected BarSeriesBase()
  {
    this.FillColor = OxyColors.Automatic;
    this.NegativeFillColor = OxyColors.Undefined;
    this.StrokeColor = OxyColors.Black;
    this.StrokeThickness = 0.0;
    this.TrackerFormatString = "{0}\n{1}: {2}";
    this.LabelMargin = 2.0;
    this.StackGroup = string.Empty;
  }

  public double BaseValue { get; set; }

  public string ColorField { get; set; }

  public OxyColor FillColor { get; set; }

  public OxyColor ActualFillColor => this.FillColor.GetActualColor(this.defaultFillColor);

  public bool IsStacked { get; set; }

  public string LabelFormatString { get; set; }

  public double LabelMargin { get; set; }

  public LabelPlacement LabelPlacement { get; set; }

  public OxyColor NegativeFillColor { get; set; }

  public string StackGroup { get; set; }

  public OxyColor StrokeColor { get; set; }

  public double StrokeThickness { get; set; }

  public string ValueField { get; set; }

  protected IList<BarItemBase> ValidItems { get; set; }

  protected Dictionary<int, int> ValidItemsIndexInversion { get; set; }

  protected IList<OxyRect> ActualBarRectangles { get; set; }

  public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
  {
    if (this.ActualBarRectangles == null || this.ValidItems == null)
      return (TrackerHitResult) null;
    int num = 0;
    foreach (OxyRect actualBarRectangle in (IEnumerable<OxyRect>) this.ActualBarRectangles)
    {
      if (actualBarRectangle.Contains(point))
      {
        BarItemBase validItem = this.ValidItems[num];
        int categoryIndex = validItem.GetCategoryIndex(num);
        DataPoint dataPoint = new DataPoint((double) categoryIndex, this.ValidItems[num].Value);
        object obj = this.GetItem(this.ValidItemsIndexInversion[num]);
        return new TrackerHitResult()
        {
          Series = (OxyPlot.Series.Series) this,
          DataPoint = dataPoint,
          Position = point,
          Item = obj,
          Index = (double) num,
          Text = this.GetTrackerText(validItem, obj, categoryIndex)
        };
      }
      ++num;
    }
    return (TrackerHitResult) null;
  }

  public override void Render(IRenderContext rc)
  {
    this.ActualBarRectangles = (IList<OxyRect>) new List<OxyRect>();
    if (this.ValidItems == null || this.ValidItems.Count == 0)
      return;
    OxyRect clippingRect = this.GetClippingRect();
    CategoryAxis categoryAxis = this.GetCategoryAxis();
    double actualBarWidth = this.GetActualBarWidth();
    int stackIndex = this.IsStacked ? categoryAxis.GetStackIndex(this.StackGroup) : 0;
    for (int index = 0; index < this.ValidItems.Count; ++index)
    {
      BarItemBase validItem = this.ValidItems[index];
      int categoryIndex = this.ValidItems[index].GetCategoryIndex(index);
      double num1 = validItem.Value;
      double num2 = double.NaN;
      if (this.IsStacked)
        num2 = categoryAxis.GetCurrentBaseValue(stackIndex, categoryIndex, num1 < 0.0);
      if (double.IsNaN(num2))
        num2 = this.BaseValue;
      double num3 = this.IsStacked ? num2 + num1 : num1;
      double num4 = !this.IsStacked ? (double) categoryIndex - 0.5 + categoryAxis.GetCurrentBarOffset(categoryIndex) : categoryAxis.GetCategoryValue(categoryIndex, stackIndex, actualBarWidth);
      if (this.IsStacked)
        categoryAxis.SetCurrentBaseValue(stackIndex, categoryIndex, num1 < 0.0, num3);
      OxyRect rectangle = this.GetRectangle(num2, num3, num4, num4 + actualBarWidth);
      this.ActualBarRectangles.Add(rectangle);
      this.RenderItem(rc, clippingRect, num3, num4, actualBarWidth, validItem, rectangle);
      if (this.LabelFormatString != null)
        this.RenderLabel(rc, clippingRect, rectangle, num1, index);
      if (!this.IsStacked)
        categoryAxis.IncreaseCurrentBarOffset(categoryIndex, actualBarWidth);
    }
  }

  public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
  {
    double num1 = (legendBox.Left + legendBox.Right) / 2.0;
    double num2 = (legendBox.Top + legendBox.Bottom) / 2.0;
    double height = (legendBox.Bottom - legendBox.Top) * 0.8;
    double width = height;
    rc.DrawRectangleAsPolygon(new OxyRect(num1 - 0.5 * width, num2 - 0.5 * height, width, height), this.GetSelectableColor(this.ActualFillColor), this.StrokeColor, this.StrokeThickness);
  }

  protected internal override bool IsUsing(Axis axis) => this.XAxis == axis || this.YAxis == axis;

  protected internal override void SetDefaultValues()
  {
    if (!this.FillColor.IsAutomatic())
      return;
    this.defaultFillColor = this.PlotModel.GetDefaultColor();
  }

  protected internal override void UpdateAxisMaxMin()
  {
    Axis valueAxis = this.GetValueAxis();
    if (valueAxis.IsVertical())
    {
      valueAxis.Include(this.MinY);
      valueAxis.Include(this.MaxY);
    }
    else
    {
      valueAxis.Include(this.MinX);
      valueAxis.Include(this.MaxX);
    }
  }

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
        List<double> list = this.ValidItems.Where<BarItemBase>((Func<BarItemBase, bool>) (item => item.GetCategoryIndex(j++) == i)).Select<BarItemBase, double>((Func<BarItemBase, double>) (item => item.Value)).Concat<double>((IEnumerable<double>) new double[1]).ToList<double>();
        double newValue1 = list.Where<double>((Func<double, bool>) (v => v <= 0.0)).Sum();
        double newValue2 = list.Where<double>((Func<double, bool>) (v => v >= 0.0)).Sum();
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
      List<double> list = this.ValidItems.Select<BarItemBase, double>((Func<BarItemBase, double>) (item => item.Value)).Concat<double>((IEnumerable<double>) new double[1]).ToList<double>();
      val1_1 = list.Min();
      val1_2 = list.Max();
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

  protected internal override void UpdateValidData()
  {
    this.ValidItems = (IList<BarItemBase>) new List<BarItemBase>();
    this.ValidItemsIndexInversion = new Dictionary<int, int>();
    int count = this.GetCategoryAxis().ActualLabels.Count;
    Axis valueAxis = this.GetValueAxis();
    int defaultIndex = 0;
    foreach (CategorizedItem categorizedItem in (IEnumerable<CategorizedItem>) this.GetItems())
    {
      if (categorizedItem is BarItemBase barItemBase && categorizedItem.GetCategoryIndex(defaultIndex) < count && valueAxis.IsValidValue(barItemBase.Value))
      {
        this.ValidItemsIndexInversion.Add(this.ValidItems.Count, defaultIndex);
        this.ValidItems.Add(barItemBase);
      }
      ++defaultIndex;
    }
  }

  protected abstract OxyRect GetRectangle(
    double baseValue,
    double topValue,
    double beginValue,
    double endValue);

  protected virtual string GetTrackerText(BarItemBase barItem, object item, int categoryIndex)
  {
    CategoryAxis categoryAxis = this.GetCategoryAxis();
    Axis valueAxis = this.GetValueAxis();
    return StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, item, (object) this.Title, (object) categoryAxis.FormatValue((double) categoryIndex), valueAxis.GetValue(barItem.Value));
  }

  protected abstract Axis GetValueAxis();

  protected virtual bool IsValidPoint(double v, Axis yaxis)
  {
    return !double.IsNaN(v) && !double.IsInfinity(v);
  }

  protected virtual void RenderItem(
    IRenderContext rc,
    OxyRect clippingRect,
    double topValue,
    double categoryValue,
    double actualBarWidth,
    BarItemBase item,
    OxyRect rect)
  {
    OxyColor originalColor = item.Color;
    if (originalColor.IsAutomatic())
    {
      originalColor = this.ActualFillColor;
      if (item.Value < 0.0 && !this.NegativeFillColor.IsUndefined())
        originalColor = this.NegativeFillColor;
    }
    rc.DrawClippedRectangleAsPolygon(clippingRect, rect, this.GetSelectableFillColor(originalColor), this.StrokeColor, this.StrokeThickness);
  }

  protected abstract void RenderLabel(
    IRenderContext rc,
    OxyRect clippingRect,
    OxyRect rect,
    double value,
    int index);
}
