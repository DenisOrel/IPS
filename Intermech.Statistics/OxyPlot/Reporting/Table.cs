// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.Table
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Reporting;

public class Table : ReportItem
{
  public Table()
  {
    this.Rows = (IList<TableRow>) new List<TableRow>();
    this.Columns = (IList<TableColumn>) new List<TableColumn>();
    this.Width = double.NaN;
  }

  public double ActualWidth { get; private set; }

  public string Caption { get; set; }

  public IList<TableColumn> Columns { get; private set; }

  public IList<TableRow> Rows { get; private set; }

  public int TableNumber { get; set; }

  public double Width { get; set; }

  public string GetFullCaption(ReportStyle style)
  {
    return string.Format(style.TableCaptionFormatString, (object) this.TableNumber, (object) this.Caption);
  }

  public override void Update()
  {
    base.Update();
    this.UpdateWidths();
  }

  public override void WriteContent(IReportWriter w)
  {
  }

  private void UpdateWidths()
  {
    this.ActualWidth = this.Width >= 0.0 ? this.Width : 150.0 * -this.Width;
    double num1 = 0.0;
    double num2 = 0.0;
    foreach (TableColumn column in (IEnumerable<TableColumn>) this.Columns)
    {
      if (double.IsNaN(column.Width))
      {
        column.ActualWidth = 40.0;
        num2 += column.ActualWidth;
      }
      if (column.Width < 0.0)
        num1 += -column.Width;
      if (column.Width >= 0.0)
      {
        num2 += column.Width;
        column.ActualWidth = column.Width;
      }
    }
    if (double.IsNaN(this.ActualWidth))
      this.ActualWidth = Math.Max(150.0, num2 + 100.0);
    double num3 = this.ActualWidth - num2;
    foreach (TableColumn column in (IEnumerable<TableColumn>) this.Columns)
    {
      if (column.Width < 0.0 && !num1.Equals(0.0))
      {
        double num4 = -column.Width;
        column.ActualWidth = num3 * (num4 / num1);
      }
    }
  }
}
