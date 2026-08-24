// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.Paragraph
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Reporting;

public class Paragraph : ReportItem
{
  public string Text { get; set; }

  public override void WriteContent(IReportWriter w) => w.WriteParagraph(this);
}
