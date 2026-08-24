// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.CategoryAxis
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Series;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace OxyPlot.Axes;

public class CategoryAxis : LinearAxis
{
  private readonly List<string> labels = new List<string>();
  private readonly List<string> itemsSourceLabels = new List<string>();
  private double[] currentBarOffset;
  private double[,] currentMaxValue;
  private double[,] currentMinValue;
  private double[,] currentPositiveBaseValues;
  private double[,] currentNegativeBaseValues;
  private int maxStackIndex;
  private double maxWidth;

  public CategoryAxis()
  {
    this.TickStyle = TickStyle.Outside;
    this.Position = AxisPosition.Bottom;
    this.MinimumPadding = 0.0;
    this.MaximumPadding = 0.0;
    this.MajorStep = 1.0;
    this.GapWidth = 1.0;
  }

  public double GapWidth { get; set; }

  public bool IsTickCentered { get; set; }

  public IEnumerable ItemsSource { get; set; }

  public string LabelField { get; set; }

  public List<string> Labels => this.labels;

  public List<string> ActualLabels
  {
    get => this.ItemsSource == null ? this.labels : this.itemsSourceLabels;
  }

  private double[] BarOffset { get; set; }

  private Dictionary<string, int> StackIndexMapping { get; set; }

  private double[,] StackedBarOffset { get; set; }

  private double[] TotalWidthPerCategory { get; set; }

  public double GetMaxWidth() => this.maxWidth;

  public double GetCategoryValue(int categoryIndex, int stackIndex, double actualBarWidth)
  {
    double num1 = this.StackedBarOffset[stackIndex, categoryIndex];
    double num2 = this.StackedBarOffset[stackIndex + 1, categoryIndex];
    return (double) categoryIndex - 0.5 + (num2 + num1 - actualBarWidth) * 0.5;
  }

  public double GetCategoryValue(int categoryIndex)
  {
    return (double) categoryIndex - 0.5 + this.BarOffset[categoryIndex];
  }

  public override void GetTickValues(
    out IList<double> majorLabelValues,
    out IList<double> majorTickValues,
    out IList<double> minorTickValues)
  {
    base.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);
    minorTickValues.Clear();
    if (this.IsTickCentered)
      return;
    List<double> doubleList = new List<double>(majorLabelValues.Count);
    doubleList.AddRange(majorLabelValues.Select<double, double>((Func<double, double>) (v => v - 0.5)));
    if (doubleList.Count > 0)
      doubleList.Add(doubleList[doubleList.Count - 1] + 1.0);
    majorTickValues = (IList<double>) doubleList;
  }

  public override object GetValue(double x) => (object) this.FormatValue(x);

  public double GetCurrentBarOffset(int categoryIndex) => this.currentBarOffset[categoryIndex];

  public void IncreaseCurrentBarOffset(int categoryIndex, double delta)
  {
    this.currentBarOffset[categoryIndex] += delta;
  }

  public double GetCurrentBaseValue(int stackIndex, int categoryIndex, bool negativeValue)
  {
    return !negativeValue ? this.currentPositiveBaseValues[stackIndex, categoryIndex] : this.currentNegativeBaseValues[stackIndex, categoryIndex];
  }

  public void SetCurrentBaseValue(
    int stackIndex,
    int categoryIndex,
    bool negativeValue,
    double newValue)
  {
    if (negativeValue)
      this.currentNegativeBaseValues[stackIndex, categoryIndex] = newValue;
    else
      this.currentPositiveBaseValues[stackIndex, categoryIndex] = newValue;
  }

  public double GetCurrentMaxValue(int stackIndex, int categoryIndex)
  {
    return this.currentMaxValue[stackIndex, categoryIndex];
  }

  public void SetCurrentMaxValue(int stackIndex, int categoryIndex, double newValue)
  {
    this.currentMaxValue[stackIndex, categoryIndex] = newValue;
  }

  public double GetCurrentMinValue(int stackIndex, int categoryIndex)
  {
    return this.currentMinValue[stackIndex, categoryIndex];
  }

  public void SetCurrentMinValue(int stackIndex, int categoryIndex, double newValue)
  {
    this.currentMinValue[stackIndex, categoryIndex] = newValue;
  }

  public int GetStackIndex(string stackGroup) => this.StackIndexMapping[stackGroup];

  internal override void UpdateActualMaxMin()
  {
    this.Include(-0.5);
    List<string> actualLabels = this.ActualLabels;
    if (actualLabels.Count > 0)
      this.Include((double) (actualLabels.Count - 1) + 0.5);
    else
      this.Include(0.5);
    base.UpdateActualMaxMin();
    this.MinorStep = 1.0;
  }

  internal override void UpdateFromSeries(OxyPlot.Series.Series[] series)
  {
    base.UpdateFromSeries(series);
    this.UpdateLabels((IEnumerable<OxyPlot.Series.Series>) series);
    List<string> actualLabels = this.ActualLabels;
    if (actualLabels.Count == 0)
    {
      this.TotalWidthPerCategory = (double[]) null;
      this.maxWidth = double.NaN;
      this.BarOffset = (double[]) null;
      this.StackedBarOffset = (double[,]) null;
      this.StackIndexMapping = (Dictionary<string, int>) null;
    }
    else
    {
      this.TotalWidthPerCategory = new double[actualLabels.Count];
      List<CategorizedSeries> list1 = ((IEnumerable<OxyPlot.Series.Series>) series).Where<OxyPlot.Series.Series>((Func<OxyPlot.Series.Series, bool>) (s => s.IsUsing((Axis) this))).ToList<OxyPlot.Series.Series>().OfType<CategorizedSeries>().ToList<CategorizedSeries>();
      List<IStackableSeries> list2 = list1.OfType<IStackableSeries>().Where<IStackableSeries>((Func<IStackableSeries, bool>) (s => s.IsStacked)).ToList<IStackableSeries>();
      List<string> stackIndices = list2.Select<IStackableSeries, string>((Func<IStackableSeries, string>) (s => s.StackGroup)).Distinct<string>().ToList<string>();
      Dictionary<int, double> dictionary = new Dictionary<int, double>();
      for (int j = 0; j < stackIndices.Count; j++)
      {
        double num = list2.Where<IStackableSeries>((Func<IStackableSeries, bool>) (s => s.StackGroup == stackIndices[j])).Select<IStackableSeries, double>((Func<IStackableSeries, double>) (s => ((CategorizedSeries) s).GetBarWidth())).Concat<double>((IEnumerable<double>) new double[1]).Max();
        for (int i = 0; i < actualLabels.Count; i++)
        {
          int k = 0;
          if (list2.SelectMany<IStackableSeries, CategorizedItem>((Func<IStackableSeries, IEnumerable<CategorizedItem>>) (s => (IEnumerable<CategorizedItem>) ((CategorizedSeries) s).GetItems())).Any<CategorizedItem>((Func<CategorizedItem, bool>) (item => item.GetCategoryIndex(k++) == i)))
            this.TotalWidthPerCategory[i] += num;
        }
        dictionary[j] = num;
      }
      foreach (CategorizedSeries categorizedSeries in list1.Where<CategorizedSeries>((Func<CategorizedSeries, bool>) (s => !(s is IStackableSeries) || !((IStackableSeries) s).IsStacked)).ToList<CategorizedSeries>())
      {
        for (int i = 0; i < actualLabels.Count; i++)
        {
          int j = 0;
          int num = categorizedSeries.GetItems().Count<CategorizedItem>((Func<CategorizedItem, bool>) (item => item.GetCategoryIndex(j++) == i));
          this.TotalWidthPerCategory[i] += categorizedSeries.GetBarWidth() * (double) num;
        }
      }
      this.maxWidth = ((IEnumerable<double>) this.TotalWidthPerCategory).Max();
      this.BarOffset = new double[actualLabels.Count];
      this.StackedBarOffset = new double[stackIndices.Count + 1, actualLabels.Count];
      double num1 = 0.5 / (1.0 + this.GapWidth) / this.maxWidth;
      for (int index = 0; index < actualLabels.Count; ++index)
        this.BarOffset[index] = 0.5 - this.TotalWidthPerCategory[index] * num1;
      for (int key = 0; key <= stackIndices.Count; ++key)
      {
        for (int i = 0; i < actualLabels.Count; i++)
        {
          int k = 0;
          if (!list2.SelectMany<IStackableSeries, CategorizedItem>((Func<IStackableSeries, IEnumerable<CategorizedItem>>) (s => (IEnumerable<CategorizedItem>) ((CategorizedSeries) s).GetItems())).All<CategorizedItem>((Func<CategorizedItem, bool>) (item => item.GetCategoryIndex(k++) != i)))
          {
            this.StackedBarOffset[key, i] = this.BarOffset[i];
            if (key < stackIndices.Count)
              this.BarOffset[i] += dictionary[key] / (1.0 + this.GapWidth) / this.maxWidth;
          }
        }
      }
      stackIndices.Sort();
      this.StackIndexMapping = new Dictionary<string, int>();
      for (int index = 0; index < stackIndices.Count; ++index)
        this.StackIndexMapping.Add(stackIndices[index], index);
      this.maxStackIndex = stackIndices.Count;
    }
  }

  protected internal override void ResetCurrentValues()
  {
    base.ResetCurrentValues();
    this.currentBarOffset = this.BarOffset != null ? ((IEnumerable<double>) this.BarOffset).ToArray<double>() : (double[]) null;
    List<string> actualLabels = this.ActualLabels;
    if (this.maxStackIndex > 0)
    {
      this.currentPositiveBaseValues = new double[this.maxStackIndex, actualLabels.Count];
      this.currentPositiveBaseValues.Fill2D(double.NaN);
      this.currentNegativeBaseValues = new double[this.maxStackIndex, actualLabels.Count];
      this.currentNegativeBaseValues.Fill2D(double.NaN);
      this.currentMaxValue = new double[this.maxStackIndex, actualLabels.Count];
      this.currentMaxValue.Fill2D(double.NaN);
      this.currentMinValue = new double[this.maxStackIndex, actualLabels.Count];
      this.currentMinValue.Fill2D(double.NaN);
    }
    else
    {
      this.currentPositiveBaseValues = (double[,]) null;
      this.currentNegativeBaseValues = (double[,]) null;
      this.currentMaxValue = (double[,]) null;
      this.currentMinValue = (double[,]) null;
    }
  }

  protected override string FormatValueOverride(double x)
  {
    int index = (int) x;
    List<string> actualLabels = this.ActualLabels;
    return index >= 0 && index < actualLabels.Count ? actualLabels[index] : (string) null;
  }

  private void UpdateLabels(IEnumerable<OxyPlot.Series.Series> series)
  {
    if (this.ItemsSource != null)
    {
      this.itemsSourceLabels.Clear();
      this.itemsSourceLabels.AddFormattedRange(this.ItemsSource, this.LabelField, this.StringFormat, (IFormatProvider) this.ActualCulture);
    }
    else
    {
      if (this.Labels.Count != 0)
        return;
      foreach (OxyPlot.Series.Series series1 in series)
      {
        if (series1.IsUsing((Axis) this) && series1 is CategorizedSeries categorizedSeries)
        {
          int count = categorizedSeries.GetItems().Count;
          while (this.Labels.Count < count)
            this.Labels.Add((this.Labels.Count + 1).ToString((IFormatProvider) CultureInfo.InvariantCulture));
        }
      }
    }
  }
}
