// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.CategoryAxisExt
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Microsoft.CSharp.RuntimeBinder;
using OxyPlot.Series;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace OxyPlot.Axes;

public class CategoryAxisExt : DateTimeAxis
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
  private DateTimeIntervalType actualIntervalType;
  private DateTimeIntervalType actualMinorIntervalType;

  public CategoryAxisExt()
  {
    this.TickStyle = TickStyle.Outside;
    this.Position = AxisPosition.Bottom;
    this.MinimumPadding = 0.0;
    this.MaximumPadding = 0.0;
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
    minorTickValues = this.CreateDateValues(this.AbsoluteMinimum, this.AbsoluteMaximum, this.ActualMinorStep < 1.0 ? 1.0 : this.ActualMinorStep);
    majorTickValues = this.CreateDateValues(this.AbsoluteMinimum, this.AbsoluteMaximum, this.ActualMajorStep < 1.0 ? 1.0 : this.ActualMajorStep);
    majorLabelValues = majorTickValues;
  }

  private IList<double> CreateDateValues(double min, double max, double interval)
  {
    return this.CreateTickVal(min, max, interval);
  }

  private IList<double> CreateTickVal(double from, double to, double step, int maxTicks = 1000)
  {
    if (step <= 0.0)
      throw new ArgumentException("Шаг не может быть отрицательным.", nameof (step));
    if (to <= from && step > 0.0)
      step *= -1.0;
    double num1 = Math.Round(from / step) * step;
    int capacity = Math.Max((int) ((to - from) / step), 1);
    double num2 = step * 0.001 * (double) Math.Sign(step);
    List<double> tickVal = new List<double>(capacity);
    for (int index = 0; index < maxTicks; ++index)
    {
      double num3 = num1 + step * (double) index;
      if (num3 <= to + num2)
      {
        double num4 = Math.Round(num3 / step, 14) * step;
        tickVal.Add(num4);
      }
      else
        break;
    }
    return (IList<double>) tickVal;
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
      this.itemsSourceLabels.AddFormattedRange(this.ItemsSource, this.LabelField, this.ActualStringFormat, (IFormatProvider) this.ActualCulture);
    }
    else if (this.Labels.Count == 0)
    {
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
    else
    {
      if (string.IsNullOrEmpty(this.ActualStringFormat))
        return;
      this.Labels.Clear();
      foreach (OxyPlot.Series.Series series2 in series)
      {
        if (series2 is ColumnSeriesExt columnSeriesExt)
        {
          foreach (object obj1 in columnSeriesExt.ItemsSource)
          {
            // ISSUE: reference to a compiler-generated field
            if (CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__1 == null)
            {
              // ISSUE: reference to a compiler-generated field
              CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (CategoryAxisExt)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, DateTime> target1 = CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__1.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, DateTime>> p1 = CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__1;
            // ISSUE: reference to a compiler-generated field
            if (CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PeriodsEnd", typeof (CategoryAxisExt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj2 = CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__0.Target((CallSite) CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__0, obj1);
            DateTime dateTime = target1((CallSite) p1, obj2);
            switch (this.actualIntervalType)
            {
              case DateTimeIntervalType.Seconds:
                this.ActualStringFormat = "dd-MM-yyyy";
                break;
              case DateTimeIntervalType.Minutes:
                if (dateTime.Minute == 0 && dateTime.Hour == 0)
                {
                  this.ActualStringFormat = "dd-MM-yyyy";
                  break;
                }
                break;
              case DateTimeIntervalType.Hours:
                if (dateTime.Hour == 0)
                {
                  this.ActualStringFormat = "dd-MM-yyyy";
                  break;
                }
                break;
            }
            // ISSUE: reference to a compiler-generated field
            if (CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__4 = CallSite<Action<CallSite, List<string>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (CategoryAxisExt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, List<string>, object> target2 = CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__4.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, List<string>, object>> p4 = CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__4;
            List<string> labels = this.Labels;
            // ISSUE: reference to a compiler-generated field
            if (CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (CategoryAxisExt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, string, object> target3 = CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__3.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, string, object>> p3 = CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__3;
            // ISSUE: reference to a compiler-generated field
            if (CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__2 == null)
            {
              // ISSUE: reference to a compiler-generated field
              CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PeriodsEnd", typeof (CategoryAxisExt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj3 = CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__2.Target((CallSite) CategoryAxisExt.\u003C\u003Eo__66.\u003C\u003Ep__2, obj1);
            string actualStringFormat = this.ActualStringFormat;
            object obj4 = target3((CallSite) p3, obj3, actualStringFormat);
            target2((CallSite) p4, labels, obj4);
          }
        }
      }
    }
  }

  internal override void UpdateIntervals(OxyRect plotArea)
  {
    base.UpdateIntervals(plotArea);
    switch (this.actualIntervalType)
    {
      case DateTimeIntervalType.Seconds:
        this.ActualMinorStep = this.ActualMajorStep;
        if (this.ActualStringFormat != null)
          break;
        this.ActualStringFormat = "HH:mm:ss";
        break;
      case DateTimeIntervalType.Minutes:
        this.ActualMinorStep = this.ActualMajorStep;
        if (this.ActualStringFormat != null)
          break;
        this.ActualStringFormat = "HH:mm";
        break;
      case DateTimeIntervalType.Hours:
        this.ActualMinorStep = this.ActualMajorStep;
        if (this.ActualStringFormat != null)
          break;
        this.ActualStringFormat = "HH";
        break;
      case DateTimeIntervalType.Days:
        this.ActualMinorStep = this.ActualMajorStep;
        if (this.ActualStringFormat != null)
          break;
        this.ActualStringFormat = "dd-MM-yyyy";
        break;
      case DateTimeIntervalType.Weeks:
        this.actualMinorIntervalType = DateTimeIntervalType.Days;
        this.ActualMajorStep = 7.0;
        this.ActualMinorStep = 1.0;
        if (this.ActualStringFormat != null)
          break;
        this.ActualStringFormat = "ww/yyyy";
        break;
      case DateTimeIntervalType.Months:
        this.actualMinorIntervalType = DateTimeIntervalType.Months;
        if (this.ActualStringFormat != null)
          break;
        this.ActualStringFormat = "MM-yyyy";
        break;
      case DateTimeIntervalType.Years:
        this.ActualMinorStep = 31.0;
        this.actualMinorIntervalType = DateTimeIntervalType.Years;
        if (this.ActualStringFormat != null)
          break;
        this.ActualStringFormat = "yyyy";
        break;
    }
  }

  protected override double CalculateActualInterval(double availableSize, double maxIntervalSize)
  {
    double num1 = Math.Abs(this.ActualMinimum - this.ActualMaximum);
    double[] source = new double[25]
    {
      1.1574074074074073E-05,
      2.3148148148148147E-05,
      5.7870370370370366E-05,
      0.00011574074074074073,
      0.00034722222222222218,
      0.00069444444444444436,
      1.0 / 720.0,
      1.0 / 288.0,
      1.0 / 144.0,
      1.0 / 48.0,
      1.0 / 24.0,
      1.0 / 6.0,
      1.0 / 3.0,
      0.5,
      1.0,
      2.0,
      5.0,
      7.0,
      14.0,
      30.5,
      61.0,
      91.5,
      122.0,
      183.0,
      365.25
    };
    double interval = source[0];
    double num2;
    for (int index = Math.Max((int) (availableSize / maxIntervalSize), 2); num1 / interval >= (double) index; interval = num2)
    {
      num2 = ((IEnumerable<double>) source).FirstOrDefault<double>((Func<double, bool>) (i => i > interval));
      if (Math.Abs(num2) <= double.Epsilon)
        num2 = interval * 2.0;
    }
    this.actualIntervalType = this.IntervalType;
    this.actualMinorIntervalType = this.MinorIntervalType;
    if (this.IntervalType == DateTimeIntervalType.Auto)
    {
      this.actualIntervalType = DateTimeIntervalType.Seconds;
      if (interval >= 0.00069444444444444436)
        this.actualIntervalType = DateTimeIntervalType.Minutes;
      if (interval >= 1.0 / 24.0)
        this.actualIntervalType = DateTimeIntervalType.Hours;
      if (interval >= 1.0)
        this.actualIntervalType = DateTimeIntervalType.Days;
      if (interval >= 30.0)
        this.actualIntervalType = DateTimeIntervalType.Months;
      if (num1 >= 365.25)
        this.actualIntervalType = DateTimeIntervalType.Years;
    }
    if (this.actualIntervalType == DateTimeIntervalType.Months)
    {
      double range = num1 / 30.5;
      interval = this.CalculateActualInterval(availableSize, maxIntervalSize, range);
    }
    if (this.actualIntervalType == DateTimeIntervalType.Years)
    {
      double range = num1 / 365.25;
      interval = this.CalculateActualInterval(availableSize, maxIntervalSize, range);
    }
    if (this.actualMinorIntervalType == DateTimeIntervalType.Auto)
    {
      switch (this.actualIntervalType)
      {
        case DateTimeIntervalType.Hours:
          this.actualMinorIntervalType = DateTimeIntervalType.Minutes;
          break;
        case DateTimeIntervalType.Days:
          this.actualMinorIntervalType = DateTimeIntervalType.Hours;
          break;
        case DateTimeIntervalType.Weeks:
          this.actualMinorIntervalType = DateTimeIntervalType.Days;
          break;
        case DateTimeIntervalType.Months:
          this.actualMinorIntervalType = DateTimeIntervalType.Days;
          break;
        case DateTimeIntervalType.Years:
          this.actualMinorIntervalType = DateTimeIntervalType.Months;
          break;
        default:
          this.actualMinorIntervalType = DateTimeIntervalType.Days;
          break;
      }
    }
    return interval;
  }
}
