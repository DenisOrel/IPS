// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.Report
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Globalization;

#nullable disable
namespace OxyPlot.Reporting;

public class Report : ReportItem
{
  public CultureInfo ActualCulture => this.Culture ?? CultureInfo.CurrentCulture;

  public string Author { get; set; }

  public CultureInfo Culture { get; set; }

  public string SubTitle { get; set; }

  public string Title { get; set; }

  public override void Write(IReportWriter w)
  {
    this.UpdateParent(this);
    this.UpdateFigureNumbers();
    base.Write(w);
  }
}
