// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.TimeSpanAxis
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Axes;

public class TimeSpanAxis : LinearAxis
{
  public static double ToDouble(TimeSpan s) => s.TotalSeconds;

  public static TimeSpan ToTimeSpan(double value) => TimeSpan.FromSeconds(value);

  public override object GetValue(double x) => (object) TimeSpan.FromSeconds(x);

  protected override string FormatValueOverride(double x)
  {
    TimeSpan timeSpan = TimeSpanAxis.ToTimeSpan(x);
    return string.Format((IFormatProvider) this.ActualCulture, $"{{0:{(this.ActualStringFormat ?? this.StringFormat ?? string.Empty).Replace(":", "\\:")}}}", (object) timeSpan);
  }

  protected override double CalculateActualInterval(double availableSize, double maxIntervalSize)
  {
    double num1 = Math.Abs(this.ActualMinimum - this.ActualMaximum);
    double interval = 1.0;
    double[] source = new double[12]
    {
      1.0,
      5.0,
      10.0,
      30.0,
      60.0,
      120.0,
      300.0,
      600.0,
      900.0,
      1200.0,
      1800.0,
      3600.0
    };
    double num2;
    for (int index = Math.Max((int) (availableSize / maxIntervalSize), 2); num1 / interval >= (double) index; interval = num2)
    {
      num2 = ((IEnumerable<double>) source).FirstOrDefault<double>((Func<double, bool>) (i => i > interval));
      if (Math.Abs(num2) < double.Epsilon)
        num2 = interval * 2.0;
    }
    return interval;
  }
}
