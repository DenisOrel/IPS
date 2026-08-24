// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.HtmlReportWriter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace OxyPlot.Reporting;

public class HtmlReportWriter : XmlWriterBase, IReportWriter
{
  private readonly IRenderContext textMeasurer;
  private int figureCounter;
  private ReportStyle style;

  public HtmlReportWriter(Stream stream, IRenderContext textMeasurer = null)
    : base(stream)
  {
    this.textMeasurer = textMeasurer;
    this.WriteHtmlElement();
    this.PlotElementType = HtmlPlotElementType.Svg;
  }

  public HtmlPlotElementType PlotElementType { get; set; }

  public override void Close()
  {
    this.WriteEndElement();
    this.WriteEndElement();
    base.Close();
  }

  public void WriteClassId(string className, string id = null)
  {
    if (className != null)
      this.WriteAttributeString("class", className);
    if (id == null)
      return;
    this.WriteAttributeString(nameof (id), id);
  }

  public void WriteDrawing(DrawingFigure d)
  {
    this.WriteStartFigure();
    this.WriteRaw(d.Content);
    this.WriteEndFigure(d.FigureText);
  }

  public void WriteEquation(Equation equation)
  {
  }

  public void WriteHeader(Header h)
  {
    if (h.Text == null)
      return;
    this.WriteStartElement(nameof (h) + (object) h.Level);
    this.WriteString(h.ToString());
    this.WriteEndElement();
  }

  public void WriteImage(Image i)
  {
    string source = i.Source;
    this.WriteStartFigure();
    this.WriteStartElement("img");
    this.WriteAttributeString("src", source);
    this.WriteAttributeString("alt", i.FigureText);
    this.WriteEndElement();
    this.WriteEndFigure(i.FigureText);
  }

  public void WriteParagraph(Paragraph p) => this.WriteElementString(nameof (p), p.Text);

  public void WritePlot(PlotFigure plot)
  {
    this.WriteStartFigure();
    switch (this.PlotElementType)
    {
      case HtmlPlotElementType.Svg:
        this.WriteRaw(SvgExporter.ExportToString((IPlotModel) plot.PlotModel, plot.Width, plot.Height, false, this.textMeasurer));
        break;
    }
    this.WriteEndFigure(plot.FigureText);
  }

  public void WriteReport(Report report, ReportStyle reportStyle)
  {
    this.style = reportStyle;
    this.WriteHtmlHeader(report.Title, (string) null, HtmlReportWriter.CreateCss(reportStyle));
    report.Write((IReportWriter) this);
  }

  public void WriteRows(Table t)
  {
    foreach (TableColumn column in (IEnumerable<TableColumn>) t.Columns)
    {
      this.WriteStartElement("col");
      this.WriteAttributeString("align", HtmlReportWriter.GetAlignmentString(column.Alignment));
      if (double.IsNaN(column.Width))
        this.WriteAttributeString("width", column.Width.ToString() + "pt");
      this.WriteEndElement();
    }
    foreach (TableRow row in (IEnumerable<TableRow>) t.Rows)
    {
      if (row.IsHeader)
        this.WriteStartElement("thead");
      this.WriteStartElement("tr");
      int num1 = 0;
      foreach (TableCell cell in (IEnumerable<TableCell>) row.Cells)
      {
        int num2 = row.IsHeader ? 1 : (t.Columns[num1++].IsHeader ? 1 : 0);
        this.WriteStartElement("td");
        if (num2 != 0)
          this.WriteAttributeString("class", "header");
        this.WriteString(cell.Content);
        this.WriteEndElement();
      }
      this.WriteEndElement();
      if (row.IsHeader)
        this.WriteEndElement();
    }
  }

  public void WriteTable(Table t)
  {
    if (t.Rows == null || t.Columns == null)
      return;
    this.WriteStartElement("table");
    if (t.Caption != null)
    {
      this.WriteStartElement("caption");
      this.WriteString(t.GetFullCaption(this.style));
      this.WriteEndElement();
    }
    this.WriteRows(t);
    this.WriteEndElement();
  }

  private static string CreateCss(ReportStyle style)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendLine($"body {{ {HtmlReportWriter.ParagraphStyleToCss(style.BodyTextStyle)} }}");
    for (int index = 0; index < style.HeaderStyles.Length; ++index)
      stringBuilder.AppendLine($"h{(object) (index + 1)} {{{HtmlReportWriter.ParagraphStyleToCss(style.HeaderStyles[index])} }}");
    stringBuilder.AppendLine($"table caption {{ {HtmlReportWriter.ParagraphStyleToCss(style.TableCaptionStyle)} }}");
    stringBuilder.AppendLine($"thead {{ {HtmlReportWriter.ParagraphStyleToCss(style.TableHeaderStyle)} }}");
    stringBuilder.AppendLine($"td {{ {HtmlReportWriter.ParagraphStyleToCss(style.TableTextStyle)} }}");
    stringBuilder.AppendLine($"td.header {{ {HtmlReportWriter.ParagraphStyleToCss(style.TableHeaderStyle)} }}");
    stringBuilder.AppendLine($"figuretext {{ {HtmlReportWriter.ParagraphStyleToCss(style.FigureTextStyle)} }}");
    stringBuilder.Append("body { margin:20pt; }\n            table { border: solid 1px black; margin: 8pt; border-collapse:collapse; }\n            td { padding: 0 2pt 0 2pt; border-left: solid 1px black; border-right: solid 1px black;}\n            thead { border:solid 1px black; }\n            .content, .content td { border: none; }\n            .figure { margin: 8pt;}\n            .table { margin: 8pt;}\n            .table caption { margin: 4pt;}\n            .table thead td { padding: 2pt;}");
    return stringBuilder.ToString();
  }

  private static string GetAlignmentString(Alignment a) => a.ToString().ToLower();

  private static string ParagraphStyleToCss(ParagraphStyle s)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (s.FontFamily != null)
      stringBuilder.Append($"font-family:{s.FontFamily};");
    stringBuilder.Append($"font-size:{s.FontSize}pt;");
    if (s.Bold)
      stringBuilder.Append(string.Format("font-weight:bold;"));
    return stringBuilder.ToString();
  }

  private void WriteHtmlElement() => this.WriteStartElement("html", "http://www.w3.org/1999/xhtml");

  private void WriteDiv(string divstyle, string content)
  {
    this.WriteStartElement("div");
    this.WriteAttributeString("class", divstyle);
    this.WriteString(content);
    this.WriteEndElement();
  }

  private void WriteEndFigure(string text)
  {
    if (text != null)
      this.WriteDiv("figuretext", $"Fig {this.figureCounter}. {text}");
    this.WriteEndElement();
  }

  private void WriteHtmlHeader(string title, string cssPath, string cssStyle)
  {
    this.WriteStartElement("head");
    if (title != null)
      this.WriteElementString(nameof (title), title);
    if (cssPath != null)
    {
      this.WriteStartElement("link");
      this.WriteAttributeString("href", cssPath);
      this.WriteAttributeString("rel", "stylesheet");
      this.WriteAttributeString("type", "text/css");
      this.WriteEndElement();
    }
    if (cssStyle != null)
    {
      this.WriteStartElement("style");
      this.WriteAttributeString("type", "text/css");
      this.WriteRaw(cssStyle);
      this.WriteEndElement();
    }
    this.WriteEndElement();
    this.WriteStartElement("body");
  }

  private void WriteStartFigure()
  {
    ++this.figureCounter;
    this.WriteStartElement("p");
    this.WriteClassId("figure");
  }
}
