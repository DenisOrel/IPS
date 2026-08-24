// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.OxyPlotColumnChartPresenter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Statistics.Interfaces;
using OxyPlot.Axes;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Statistics;

internal class OxyPlotColumnChartPresenter(CollectedStatistics collectedStatistics) : 
  OxyPlotBasePresenter(collectedStatistics)
{
  public override List<Period> GetPeriods()
  {
    List<Period> periods = new List<Period>();
    for (int index = 1; index < this._collectedStatisticsModel.Periods.Count; ++index)
      periods.Add(this._collectedStatisticsModel.Periods[index]);
    return periods;
  }

  protected override List<double> GetAllValues()
  {
    List<double> allValues = new List<double>();
    foreach (StatisticsResultValues statisticsResultValue in this._collectedStatisticsModel.StatisticsResultValues)
    {
      foreach (StatisticsPoint point in statisticsResultValue.Points)
      {
        if (point.PeriodsIndex != 0)
          allValues.Add(Axis.ToDouble(point.Value));
      }
    }
    return allValues;
  }

  protected override void DoSomethingWithAbnormalValues(
    List<StatisticsResultValues> resultValueses,
    Limits limits)
  {
    foreach (StatisticsResultValues resultValuese in resultValueses)
    {
      List<StatisticsPoint> statisticsPointList = new List<StatisticsPoint>();
      foreach (StatisticsPoint point in resultValuese.Points)
      {
        if (!limits.Fits(Axis.ToDouble(point.Value)))
          statisticsPointList.Add(new StatisticsPoint((object) 0, point.PeriodsStart, point.PeriodsEnd, point.PeriodsIndex));
        else
          statisticsPointList.Add(point);
      }
      resultValuese.Points = statisticsPointList;
    }
  }
}
