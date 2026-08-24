// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.DateTimeAxis
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

#nullable disable
namespace OxyPlot.Axes;

public class DateTimeAxis : LinearAxis
{
  private static readonly DateTime TimeOrigin = new DateTime(1899, 12, 31 /*0x1F*/, 0, 0, 0, DateTimeKind.Utc);
  private static readonly double MaxDayValue = (DateTime.MaxValue - DateTimeAxis.TimeOrigin).TotalDays;
  private static readonly double MinDayValue = (DateTime.MinValue - DateTimeAxis.TimeOrigin).TotalDays;
  private DateTimeIntervalType actualIntervalType;
  private DateTimeIntervalType actualMinorIntervalType;

  public DateTimeAxis()
  {
    this.Position = AxisPosition.Bottom;
    this.IntervalType = DateTimeIntervalType.Auto;
    this.FirstDayOfWeek = DayOfWeek.Monday;
    this.CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek;
  }

  public CalendarWeekRule CalendarWeekRule { get; set; }

  public DayOfWeek FirstDayOfWeek { get; set; }

  public DateTimeIntervalType IntervalType { get; set; }

  public DateTimeIntervalType MinorIntervalType { get; set; }

  public TimeZoneInfo TimeZone { get; set; }

  public static DataPoint CreateDataPoint(DateTime x, double y)
  {
    return new DataPoint(DateTimeAxis.ToDouble(x), y);
  }

  public static DataPoint CreateDataPoint(DateTime x, DateTime y)
  {
    return new DataPoint(DateTimeAxis.ToDouble(x), DateTimeAxis.ToDouble(y));
  }

  public static DataPoint CreateDataPoint(double x, DateTime y)
  {
    return new DataPoint(x, DateTimeAxis.ToDouble(y));
  }

  public static DateTime ToDateTime(double value)
  {
    return double.IsNaN(value) || value < DateTimeAxis.MinDayValue || value > DateTimeAxis.MaxDayValue ? new DateTime() : DateTimeAxis.TimeOrigin.AddDays(value - 1.0);
  }

  public static double ToDouble(DateTime value)
  {
    return (value - DateTimeAxis.TimeOrigin).TotalDays + 1.0;
  }

  public override void GetTickValues(
    out IList<double> majorLabelValues,
    out IList<double> majorTickValues,
    out IList<double> minorTickValues)
  {
    minorTickValues = this.CreateDateTimeTickValues(this.ActualMinimum, this.ActualMaximum, this.ActualMinorStep, this.actualMinorIntervalType);
    majorTickValues = this.CreateDateTimeTickValues(this.ActualMinimum, this.ActualMaximum, this.ActualMajorStep, this.actualIntervalType);
    majorLabelValues = majorTickValues;
  }

  public override object GetValue(double x)
  {
    DateTime dateTime = DateTimeAxis.ToDateTime(x);
    if (this.TimeZone != null)
      dateTime = TimeZoneInfo.ConvertTime(dateTime, this.TimeZone);
    return (object) dateTime;
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
        this.ActualStringFormat = "HH:mm";
        break;
      case DateTimeIntervalType.Days:
        this.ActualMinorStep = this.ActualMajorStep;
        if (this.ActualStringFormat != null)
          break;
        this.ActualStringFormat = "yyyy-MM-dd";
        break;
      case DateTimeIntervalType.Weeks:
        this.actualMinorIntervalType = DateTimeIntervalType.Days;
        this.ActualMajorStep = 7.0;
        this.ActualMinorStep = 1.0;
        if (this.ActualStringFormat != null)
          break;
        this.ActualStringFormat = "yyyy/ww";
        break;
      case DateTimeIntervalType.Months:
        this.actualMinorIntervalType = DateTimeIntervalType.Months;
        if (this.ActualStringFormat != null)
          break;
        this.ActualStringFormat = "yyyy-MM-dd";
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

  protected override string FormatValueOverride(double x)
  {
    DateTime dateTime = DateTimeAxis.ToDateTime(x);
    if (this.TimeZone != null)
      dateTime = TimeZoneInfo.ConvertTime(dateTime, this.TimeZone);
    string actualStringFormat = this.ActualStringFormat;
    if (actualStringFormat == null)
      return dateTime.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
    int week = this.GetWeek(dateTime);
    return string.Format((IFormatProvider) this.ActualCulture, $"{{0:{actualStringFormat.Replace("ww", week.ToString("00")).Replace("w", week.ToString((IFormatProvider) CultureInfo.InvariantCulture))}}}", (object) dateTime);
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

  private IList<double> CreateDateTickValues(
    double min,
    double max,
    double step,
    DateTimeIntervalType intervalType)
  {
    Collection<double> dateTickValues = new Collection<double>();
    DateTime dateTime1 = DateTimeAxis.ToDateTime(min);
    if (dateTime1.Ticks == 0L)
      return (IList<double>) dateTickValues;
    switch (intervalType)
    {
      case DateTimeIntervalType.Weeks:
        dateTime1 = dateTime1.AddDays((double) (-(int) dateTime1.DayOfWeek + this.FirstDayOfWeek));
        break;
      case DateTimeIntervalType.Months:
        dateTime1 = new DateTime(dateTime1.Year, dateTime1.Month, 1);
        break;
      case DateTimeIntervalType.Years:
        dateTime1 = new DateTime(dateTime1.Year, 1, 1);
        break;
    }
    DateTime dateTime2 = DateTimeAxis.ToDateTime(max).AddTicks(1L);
    if (dateTime2.Ticks == 0L)
      return (IList<double>) dateTickValues;
    DateTime dateTime3 = dateTime1;
    double num = step * 0.001;
    DateTime dateTime4 = DateTimeAxis.ToDateTime(min - num);
    DateTime dateTime5 = DateTimeAxis.ToDateTime(max + num);
    if (dateTime4.Ticks == 0L || dateTime5.Ticks == 0L)
      return (IList<double>) dateTickValues;
    while (dateTime3 < dateTime2)
    {
      if (dateTime3 > dateTime4 && dateTime3 < dateTime5)
        dateTickValues.Add(DateTimeAxis.ToDouble(dateTime3));
      try
      {
        switch (intervalType)
        {
          case DateTimeIntervalType.Months:
            dateTime3 = dateTime3.AddMonths((int) Math.Ceiling(step));
            continue;
          case DateTimeIntervalType.Years:
            dateTime3 = dateTime3.AddYears((int) Math.Ceiling(step));
            continue;
          default:
            dateTime3 = dateTime3.AddDays(step);
            continue;
        }
      }
      catch (ArgumentOutOfRangeException ex)
      {
        break;
      }
    }
    return (IList<double>) dateTickValues;
  }

  private IList<double> CreateDateTimeTickValues(
    double min,
    double max,
    double interval,
    DateTimeIntervalType intervalType)
  {
    return intervalType > DateTimeIntervalType.Days ? this.CreateDateTickValues(min, max, interval, intervalType) : Axis.CreateTickValues(min, max, interval);
  }

  private int GetWeek(DateTime date)
  {
    return this.ActualCulture.Calendar.GetWeekOfYear(date, this.CalendarWeekRule, this.FirstDayOfWeek);
  }
}
