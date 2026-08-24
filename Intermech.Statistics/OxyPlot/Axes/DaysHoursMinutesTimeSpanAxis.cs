// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.DaysHoursMinutesTimeSpanAxis
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot.Axes;

public class DaysHoursMinutesTimeSpanAxis : TimeSpanAxis
{
  protected override string FormatValueOverride(double x)
  {
    TimeSpan timeSpan = TimeSpanAxis.ToTimeSpan(x);
    return $"{(object) timeSpan.Days} дн. {(object) timeSpan.Hours} ч. {(object) timeSpan.Minutes} мин.";
  }
}
