// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.IReportWriter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Reporting;

public interface IReportWriter
{
  void WriteDrawing(DrawingFigure drawing);

  void WriteEquation(Equation equation);

  void WriteHeader(Header header);

  void WriteImage(Image image);

  void WriteParagraph(Paragraph paragraph);

  void WritePlot(PlotFigure plot);

  void WriteReport(Report report, ReportStyle reportStyle);

  void WriteTable(Table table);
}
