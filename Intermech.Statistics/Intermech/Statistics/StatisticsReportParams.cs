// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.StatisticsReportParams
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Document.Client.Report;
using System;

#nullable disable
namespace Intermech.Statistics;

public class StatisticsReportParams
{
  public string ReportName;
  public string ReportCaption;
  public Guid TemplateGuid;
  public bool CanAddCreatingDate;
  public DatePrintFormats DateFormat;
  public DatePrintFormats ReportCreatingDateFormat;
  public MainData MainData;
  public int DataColumnsNumber;
  public bool ShowOnlyIntervalStartDate;
}
