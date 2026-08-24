// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.OxyPlotBasePresenter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Statistics.Interfaces;
using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Statistics;

public abstract class OxyPlotBasePresenter
{
  protected readonly CollectedStatistics _collectedStatisticsModel;

  public double MaxValue { get; private set; }

  public double MinValue { get; private set; }

  public List<Graph> Graphs { get; }

  public OxyPlotBasePresenter(CollectedStatistics collectedStatistics)
  {
    this._collectedStatisticsModel = collectedStatistics;
    this.Graphs = new List<Graph>();
    this.MaxValue = double.MaxValue;
    this.MinValue = 0.0;
    this.BuildData();
  }

  private void BuildData()
  {
    this.SetGraphsData();
    this.SetMinMaxValue();
  }

  private void SetMinMaxValue()
  {
    foreach (Graph graph in this.Graphs)
    {
      foreach (DataPoint point in graph.Points)
      {
        if (this.MaxValue < point.Y)
          this.MaxValue = point.Y;
        if (this.MinValue > point.y)
          this.MinValue = point.Y;
      }
    }
  }

  private void SetGraphsData()
  {
    ExcludeAbnormalValuesSettings abnormalValuesSettings = this._collectedStatisticsModel.ExcludeAbnormalValuesSettings;
    List<StatisticsResultValues> statisticsResultValuesList = new List<StatisticsResultValues>(this._collectedStatisticsModel.StatisticsResultValues.Select<StatisticsResultValues, StatisticsResultValues>((Func<StatisticsResultValues, StatisticsResultValues>) (x => x.Clone() as StatisticsResultValues)));
    if (abnormalValuesSettings.NeedExcludeAbnormalValues && abnormalValuesSettings.Percentage != 0U)
      this.DoSomethingWithAbnormalValues(abnormalValuesSettings, statisticsResultValuesList);
    this.ConvertStatisticsResultValuesToGraphs(statisticsResultValuesList);
  }

  private void ConvertStatisticsResultValuesToGraphs(
    List<StatisticsResultValues> statisticsResultValueses)
  {
    foreach (StatisticsResultValues statisticsResultValuese in statisticsResultValueses)
    {
      List<DataPoint> points = new List<DataPoint>();
      foreach (StatisticsPoint point in statisticsResultValuese.Points)
        points.Add(new DataPoint(Axis.ToDouble((object) point.PeriodsEnd), Axis.ToDouble(point.Value)));
      this.Graphs.Add(new Graph(statisticsResultValuese.Caption, points));
    }
  }

  private void DoSomethingWithAbnormalValues(
    ExcludeAbnormalValuesSettings excludeAbnormalValuesSettings,
    List<StatisticsResultValues> resultValueses)
  {
    List<double> allValues = this.GetAllValues();
    if (allValues.Count == 0)
      return;
    Limits limits = Limits.CountLimits(allValues.Where<double>((Func<double, bool>) (x => x != 0.0)).ToList<double>(), excludeAbnormalValuesSettings.Percentage);
    this.DoSomethingWithAbnormalValues(resultValueses, limits);
  }

  protected abstract void DoSomethingWithAbnormalValues(
    List<StatisticsResultValues> resultValueses,
    Limits limits);

  protected abstract List<double> GetAllValues();

  public abstract List<Period> GetPeriods();
}
