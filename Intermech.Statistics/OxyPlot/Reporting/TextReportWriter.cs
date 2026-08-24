// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.TextReportWriter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace OxyPlot.Reporting;

public class TextReportWriter : StreamWriter, IReportWriter
{
  private const string TableCellSeparator = " | ";
  private const string TableRowEnd = " |";
  private const string TableRowStart = "| ";
  private int tableCounter;

  public TextReportWriter(Stream stream)
    : base(stream)
  {
    this.MaxLineLength = 60;
  }

  public int MaxLineLength { get; set; }

  public void WriteDrawing(DrawingFigure d)
  {
  }

  public void WriteEquation(Equation equation)
  {
  }

  public void WriteHeader(Header h)
  {
    if (h.Text == null)
      return;
    this.WriteLine((object) h);
    if (h.Level == 1)
      this.WriteLine("=".Repeat(h.Text.Length));
    this.WriteLine();
  }

  public void WriteImage(Image i)
  {
  }

  public void WriteParagraph(Paragraph p)
  {
    foreach (string splitLine in p.Text.SplitLines(this.MaxLineLength))
      this.WriteLine(splitLine);
    this.WriteLine();
  }

  public void WritePlot(PlotFigure plot)
  {
  }

  public void WriteReport(Report report, ReportStyle reportStyle)
  {
    report.Write((IReportWriter) this);
  }

  public void WriteTable(Table t)
  {
    if (t.Caption != null)
    {
      ++this.tableCounter;
      this.WriteLine("Table {0}. {1}", (object) this.tableCounter, (object) t.Caption);
    }
    this.WriteLine();
    int count = t.Columns.Count;
    int[] numArray = new int[count];
    for (int index = 0; index < count; ++index)
    {
      numArray[index] = 0;
      foreach (TableRow row in (IEnumerable<TableRow>) t.Rows)
      {
        string content = row.Cells[index].Content;
        numArray[index] = Math.Max(numArray[index], content != null ? content.Length : 0);
      }
    }
    foreach (TableRow row in (IEnumerable<TableRow>) t.Rows)
    {
      for (int index = 0; index < count; ++index)
      {
        string content = row.Cells[index].Content;
        this.Write(TextReportWriter.GetCellText(index, count, TextReportWriter.PadString(content, t.Columns[index].Alignment, numArray[index])));
      }
      this.WriteLine();
    }
    this.WriteLine();
  }

  private static string GetCellText(int cellIndex, int columns, string content)
  {
    if (cellIndex == 0)
      content = "| " + content;
    if (cellIndex + 1 < columns)
      content += " | ";
    if (cellIndex == columns - 1)
      content += " |";
    return content;
  }

  private static string PadString(string text, Alignment alignment, int width)
  {
    if (text == null)
      return string.Empty.PadLeft(width);
    switch (alignment)
    {
      case Alignment.Left:
        return text.PadRight(width);
      case Alignment.Right:
        return text.PadLeft(width);
      case Alignment.Center:
        text = text.PadRight((text.Length + width) / 2);
        return text.PadLeft(width);
      default:
        return (string) null;
    }
  }
}
