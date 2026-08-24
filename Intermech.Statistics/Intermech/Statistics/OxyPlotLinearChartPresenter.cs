// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.OxyPlotLinearChartPresenter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Statistics.Interfaces;
using OxyPlot.Axes;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Statistics;

internal class OxyPlotLinearChartPresenter(CollectedStatistics collectedStatistics) : 
  OxyPlotBasePresenter(collectedStatistics)
{
  public override List<Period> GetPeriods() => this._collectedStatisticsModel.Periods;

  protected override List<double> GetAllValues()
  {
    List<double> allValues = new List<double>();
    foreach (StatisticsResultValues statisticsResultValue in this._collectedStatisticsModel.StatisticsResultValues)
    {
      foreach (StatisticsPoint point in statisticsResultValue.Points)
        allValues.Add(Axis.ToDouble(point.Value));
    }
    return allValues;
  }

  protected override void DoSomethingWithAbnormalValues(
    List<StatisticsResultValues> resultValueses,
    Limits limits)
  {
    foreach (StatisticsResultValues resultValuese in resultValueses)
    {
      List<StatisticsPoint> statisticsPointList = new List<StatisticsPoint>((IEnumerable<StatisticsPoint>) resultValuese.Points);
      foreach (StatisticsPoint point in resultValuese.Points)
      {
        if (Axis.ToDouble(point.Value) != 0.0 && !limits.Fits(Axis.ToDouble(point.Value)))
          statisticsPointList.Remove(point);
      }
      resultValuese.Points = statisticsPointList;
    }
  }
}
