// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.ColumnSeriesExt
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Series;

public class ColumnSeriesExt : ColumnSeries
{
  public ColumnSeriesExt() => this.ColumnWidth = 1.0;

  protected override double GetActualBarWidth()
  {
    CategoryAxisExt category = this.GetCategory();
    return this.ColumnWidth / (1.0 + category.GapWidth) / category.GetMaxWidth();
  }

  private CategoryAxisExt GetCategory()
  {
    return this.XAxis is CategoryAxisExt ? this.XAxis as CategoryAxisExt : throw new Exception("Ошибка построения графика.");
  }

  protected internal override void UpdateValidData()
  {
    this.ValidItems = (IList<BarItemBase>) new List<BarItemBase>();
    this.ValidItemsIndexInversion = new Dictionary<int, int>();
    int count = this.GetCategory().ActualLabels.Count;
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

  protected internal override void UpdateMaxMin()
  {
    if (this.ValidItems == null || this.ValidItems.Count == 0)
      return;
    CategoryAxisExt category = this.GetCategory();
    double val1_1 = double.MaxValue;
    double val1_2 = double.MinValue;
    if (this.IsStacked)
    {
      List<string> actualLabels = this.GetCategory().ActualLabels;
      for (int i = 0; i < actualLabels.Count; i++)
      {
        int j = 0;
        List<double> list = this.ValidItems.Where<BarItemBase>((Func<BarItemBase, bool>) (item => item.GetCategoryIndex(j++) == i)).Select<BarItemBase, double>((Func<BarItemBase, double>) (item => item.Value)).Concat<double>((IEnumerable<double>) new double[1]).ToList<double>();
        double newValue1 = list.Where<double>((Func<double, bool>) (v => v <= 0.0)).Sum();
        double newValue2 = list.Where<double>((Func<double, bool>) (v => v >= 0.0)).Sum();
        int stackIndex = category.GetStackIndex(this.StackGroup);
        double currentMinValue = category.GetCurrentMinValue(stackIndex, i);
        if (!double.IsNaN(currentMinValue))
          newValue1 += currentMinValue;
        category.SetCurrentMinValue(stackIndex, i, newValue1);
        double currentMaxValue = category.GetCurrentMaxValue(stackIndex, i);
        if (!double.IsNaN(currentMaxValue))
          newValue2 += currentMaxValue;
        category.SetCurrentMaxValue(stackIndex, i, newValue2);
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

  public override void Render(IRenderContext rc)
  {
    this.ActualBarRectangles = (IList<OxyRect>) new List<OxyRect>();
    if (this.ValidItems == null || this.ValidItems.Count == 0)
      return;
    OxyRect clippingRect = this.GetClippingRect();
    CategoryAxisExt category = this.GetCategory();
    double actualBarWidth = this.GetActualBarWidth();
    int stackIndex = this.IsStacked ? category.GetStackIndex(this.StackGroup) : 0;
    for (int index = 0; index < this.ValidItems.Count; ++index)
    {
      BarItemBase validItem = this.ValidItems[index];
      int categoryIndex = this.ValidItems[index].GetCategoryIndex(index);
      double num1 = validItem.Value;
      double num2 = double.NaN;
      if (this.IsStacked)
        num2 = category.GetCurrentBaseValue(stackIndex, categoryIndex, num1 < 0.0);
      if (double.IsNaN(num2))
        num2 = this.BaseValue;
      double num3 = this.IsStacked ? num2 + num1 : num1;
      double num4 = !this.IsStacked ? (double) categoryIndex - 0.5 + category.GetCurrentBarOffset(categoryIndex) : category.GetCategoryValue(categoryIndex, stackIndex, actualBarWidth);
      if (this.IsStacked)
        category.SetCurrentBaseValue(stackIndex, categoryIndex, num1 < 0.0, num3);
      OxyRect rectangle = this.GetRectangle(num2, num3, num4, num4 + actualBarWidth);
      this.ActualBarRectangles.Add(rectangle);
      this.RenderItem(rc, clippingRect, num3, num4, actualBarWidth, validItem, rectangle);
      if (this.LabelFormatString != null)
        this.RenderLabel(rc, clippingRect, rectangle, num1, index);
      if (!this.IsStacked)
        category.IncreaseCurrentBarOffset(categoryIndex, actualBarWidth);
    }
  }

  protected override string GetTrackerText(BarItemBase barItem, object item, int categoryIndex)
  {
    CategoryAxisExt category = this.GetCategory();
    Axis valueAxis = this.GetValueAxis();
    return StringHelper.Format((IFormatProvider) this.ActualCulture, this.TrackerFormatString, item, (object) this.Title, (object) category.FormatValue((double) categoryIndex), valueAxis.GetValue(barItem.Value));
  }
}
