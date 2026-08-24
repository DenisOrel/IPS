// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.TablePresenter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Statistics;

public class TablePresenter
{
  private readonly CollectedStatistics _collectedStatisticsModel;
  private List<Period> _periods;
  private List<Intermech.Statistics.Interfaces.StatisticsResultValues> _sortedByCaptionStatisticsResultValueses;
  private string _mainColumnCaption = string.Empty;

  public string MainColumnCaption => this._mainColumnCaption;

  public string MainColumnData => "Дата";

  public List<Period> Periods
  {
    get
    {
      if (this._periods == null)
      {
        this._periods = new List<Period>((IEnumerable<Period>) this._collectedStatisticsModel.Periods);
        this._periods.RemoveAt(0);
      }
      return this._periods;
    }
  }

  public List<Intermech.Statistics.Interfaces.StatisticsResultValues> StatisticsResultValues
  {
    get
    {
      if (this._sortedByCaptionStatisticsResultValueses == null)
        this._sortedByCaptionStatisticsResultValueses = this._collectedStatisticsModel.StatisticsResultValues.OrderBy<Intermech.Statistics.Interfaces.StatisticsResultValues, string>((Func<Intermech.Statistics.Interfaces.StatisticsResultValues, string>) (x => x.Caption)).ToList<Intermech.Statistics.Interfaces.StatisticsResultValues>();
      return this._sortedByCaptionStatisticsResultValueses;
    }
  }

  public TablePresenter(CollectedStatistics collectedStatistics)
  {
    this._collectedStatisticsModel = collectedStatistics;
    this.BuildData();
  }

  private void BuildData() => this.SetMainColumnCaption();

  private void SetMainColumnCaption()
  {
    switch (this._collectedStatisticsModel.StatisticsType)
    {
      case CommandStatisticsTypesEnum.CreatedDate:
      case CommandStatisticsTypesEnum.SignDate:
      case CommandStatisticsTypesEnum.TimeOneTaskFormUsers:
        this._mainColumnCaption = "Пользователь, группа и пр.";
        break;
      case CommandStatisticsTypesEnum.LCStepDate:
      case CommandStatisticsTypesEnum.LCLevelDate:
      case CommandStatisticsTypesEnum.DateAttrValue:
        this._mainColumnCaption = "Наименование";
        break;
      case CommandStatisticsTypesEnum.ProcessTemplate:
        this._mainColumnCaption = "Шаблон";
        break;
      case CommandStatisticsTypesEnum.TimeInTask:
      case CommandStatisticsTypesEnum.RevertCountTask:
        this._mainColumnCaption = "Задача";
        break;
      default:
        this._mainColumnCaption = string.Empty;
        break;
    }
  }
}
