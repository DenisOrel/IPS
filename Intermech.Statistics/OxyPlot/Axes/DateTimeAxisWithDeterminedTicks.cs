// Decompiled with JetBrains decompiler
// Type: OxyPlot.Axes.DateTimeAxisWithDeterminedTicks
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Axes;

public class DateTimeAxisWithDeterminedTicks : DateTimeAxis
{
  public IList<double> TickValues { get; set; }

  public override void GetTickValues(
    out IList<double> majorLabelValues,
    out IList<double> majorTickValues,
    out IList<double> minorTickValues)
  {
    minorTickValues = this.TickValues;
    majorTickValues = this.TickValues;
    majorLabelValues = majorTickValues;
  }
}
