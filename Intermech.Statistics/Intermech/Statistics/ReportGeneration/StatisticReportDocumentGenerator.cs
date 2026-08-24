// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.ReportGeneration.StatisticReportDocumentGenerator
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Document.Client.Report;
using Intermech.Document.Client.Reports;
using Intermech.Document.Model;
using Intermech.Extensions;
using Intermech.Interfaces.Document;
using Intermech.Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.Statistics.ReportGeneration;

public class StatisticReportDocumentGenerator
{
  public static ImDocument GenerateStatisticReport(
    TablePresenter presenter,
    StatisticsReportParams reportParams)
  {
    return StatisticReportDocumentGenerator.GenerateStatisticReport(StatisticReportDocumentGenerator.GetTableColumnsSettings(presenter, reportParams), StatisticReportDocumentGenerator.GetDataSource(presenter, reportParams), reportParams);
  }

  private static List<TableColumnSettings> GetTableColumnsSettings(
    TablePresenter presenter,
    StatisticsReportParams reportParams)
  {
    List<TableColumnSettings> tableColumnsSettings = new List<TableColumnSettings>();
    tableColumnsSettings.Add(new TableColumnSettings(presenter.MainColumnCaption, 45f, HorzAlignment.Left));
    string formatString = reportParams.DateFormat.GetAttribute<DatePrintFormatValue>().FormatString;
    foreach (Period period in presenter.Periods)
    {
      string caption = !reportParams.ShowOnlyIntervalStartDate ? period.ToString(formatString) : period.StartDateTime.ToString(formatString);
      tableColumnsSettings.Add(new TableColumnSettings(caption, 20f, HorzAlignment.Center));
    }
    return tableColumnsSettings;
  }

  private static DataTable GetDataSource(
    TablePresenter presenter,
    StatisticsReportParams reportParams)
  {
    DataTable dataSource = new DataTable("TestTable");
    int length = presenter.Periods.Count + 1;
    for (int index = 0; index < length; ++index)
      dataSource.Columns.Add(index.ToString(), typeof (string));
    for (int index = 0; index < presenter.StatisticsResultValues.Count; ++index)
    {
      object[] objArray = new object[length];
      string caption = presenter.StatisticsResultValues[index].Caption;
      objArray[0] = (object) caption;
      for (int j = 1; j <= presenter.Periods.Count; j++)
      {
        StatisticsPoint statisticsPoint = presenter.StatisticsResultValues[index].Points.First<StatisticsPoint>((System.Func<StatisticsPoint, bool>) (x => x.PeriodsIndex == j));
        objArray[j] = statisticsPoint != null ? (object) statisticsPoint.ValueAsString() : (object) 0;
      }
      dataSource.Rows.Add(objArray);
    }
    return dataSource;
  }

  public static ImDocument GenerateStatisticReport(
    List<TableColumnSettings> columns,
    DataTable dataSource,
    StatisticsReportParams reportParams)
  {
    DocumentGeneratorHelper reportGenerator = StatisticReportDocumentGenerator.CreateReportGenerator(columns, reportParams);
    string tableCaption1 = StatisticReportDocumentGenerator.GetTableCaption(reportParams);
    DataTable sourceDataTable = dataSource;
    string tableCaption2 = tableCaption1;
    ImDocument document = reportGenerator.GenerateDocument(sourceDataTable, tableCaption2);
    document.DocumentName = reportParams.ReportName;
    return document;
  }

  private static string GetTableCaption(StatisticsReportParams reportParams)
  {
    if (reportParams.ReportCreatingDateFormat == DatePrintFormats.None)
      return reportParams.ReportCaption;
    string formatString = reportParams.ReportCreatingDateFormat.GetAttribute<DatePrintFormatValue>().FormatString;
    return $"{reportParams.ReportCaption}{Environment.NewLine}на {DateTime.Now.ToString(formatString)}";
  }

  private static DocumentGeneratorHelper CreateReportGenerator(
    List<TableColumnSettings> columns,
    StatisticsReportParams reportParams)
  {
    if (reportParams.TemplateGuid == StatisticsConst.MultilevelHorizontalTemplateGuid)
      return (DocumentGeneratorHelper) new Report2LevelRowGeneratorHelper(reportParams.TemplateGuid, (IList<TableColumnSettings>) columns, new int?(reportParams.DataColumnsNumber));
    if (reportParams.TemplateGuid == StatisticsConst.VerticalA4TemplateGuid)
      return (DocumentGeneratorHelper) new ReportGeneratorHelper(reportParams.TemplateGuid, (IList<TableColumnSettings>) columns);
    if (reportParams.TemplateGuid == StatisticsConst.HorizontalByDateTemplateGuid)
      return (DocumentGeneratorHelper) new ReportGeneratorHelper(reportParams.TemplateGuid, (IList<TableColumnSettings>) columns);
    if (reportParams.TemplateGuid == StatisticsConst.HorizontalA4TemplateGuid)
      return (DocumentGeneratorHelper) new ReportGeneratorHelper(reportParams.TemplateGuid, (IList<TableColumnSettings>) columns);
    if (reportParams.TemplateGuid == StatisticsConst.HorizontalA3TemplateGuid)
      return (DocumentGeneratorHelper) new ReportGeneratorHelper(reportParams.TemplateGuid, (IList<TableColumnSettings>) columns);
    if (reportParams.TemplateGuid == StatisticsConst.VerticalA3TemplateGuid)
      return (DocumentGeneratorHelper) new ReportGeneratorHelper(reportParams.TemplateGuid, (IList<TableColumnSettings>) columns);
    throw new Exception("Неопознанный шаблон для построения отчета.");
  }
}
