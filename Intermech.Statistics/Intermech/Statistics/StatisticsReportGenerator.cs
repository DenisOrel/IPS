// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.StatisticsReportGenerator
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Document.Client;
using Intermech.Interfaces;
using Intermech.Statistics.Interfaces;
using Intermech.Statistics.ReportGeneration;

#nullable disable
namespace Intermech.Statistics;

internal class StatisticsReportGenerator
{
  private readonly StatisticsReportParams _reportParams;
  private readonly StatisticNodeItem _statisticNodeItem;
  private readonly IStatisticsService _statisticsService;

  public StatisticsReportGenerator(StatisticsReportParams reportParams, StatisticNodeItem nodeItem)
  {
    this._reportParams = reportParams;
    this._statisticNodeItem = nodeItem;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._statisticsService = (IStatisticsService) sessionKeeper.Session.GetCustomService(typeof (IStatisticsService));
  }

  public void Generate()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      DocumentEditorPlugin.Instance.OpenImDocument(StatisticReportDocumentGenerator.GenerateStatisticReport(new TablePresenter(this._statisticsService.CollectStatistics(sessionKeeper.Session.SessionGUID, this._statisticsService.ReadStatisticObjectsCommandSettings(sessionKeeper.Session.SessionGUID, this._statisticNodeItem.ObjectID) ?? throw new KernelException("Команда статистики не сконфигурирована. Продолжение невозможно."))), this._reportParams), callDialogWithObjectParamsBeforeSave: false, defaultDocumentDbObjectType: MetaDataHelper.GetObjectTypeID(StatisticsConst.ReportObjectsTypeGuid));
  }
}
