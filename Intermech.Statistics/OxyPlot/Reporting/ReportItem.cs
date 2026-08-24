// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.ReportItem
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace OxyPlot.Reporting;

public abstract class ReportItem
{
  protected ReportItem() => this.Children = new Collection<ReportItem>();

  public Collection<ReportItem> Children { get; private set; }

  public Report Report { get; internal set; }

  public void Add(ReportItem child) => this.Children.Add(child);

  public void AddDrawing(string content, string text)
  {
    DrawingFigure child = new DrawingFigure();
    child.Content = content;
    child.FigureText = text;
    this.Add((ReportItem) child);
  }

  public void AddPlot(PlotModel plot, string text, double width, double height)
  {
    PlotFigure child = new PlotFigure();
    child.PlotModel = plot;
    child.Width = width;
    child.Height = height;
    child.FigureText = text;
    this.Add((ReportItem) child);
  }

  public void AddEquation(string equation, string caption = null)
  {
    this.Add((ReportItem) new Equation()
    {
      Content = equation,
      Caption = caption
    });
  }

  public void AddHeader(int level, string header)
  {
    this.Add((ReportItem) new Header()
    {
      Level = level,
      Text = header
    });
  }

  public void AddImage(string src, string text)
  {
    Image child = new Image();
    child.Source = src;
    child.FigureText = text;
    this.Add((ReportItem) child);
  }

  public void AddItemsTable(string title, IEnumerable items, IList<ItemsTableField> fields)
  {
    ItemsTable child = new ItemsTable();
    child.Caption = title;
    child.Items = items;
    child.Fields = fields;
    this.Add((ReportItem) child);
  }

  public void AddParagraph(string content)
  {
    this.Add((ReportItem) new Paragraph() { Text = content });
  }

  public PropertyTable AddPropertyTable(string title, object obj)
  {
    if (!(obj is IEnumerable items))
      items = (IEnumerable) new object[1]{ obj };
    PropertyTable propertyTable = new PropertyTable(items, false);
    propertyTable.Caption = title;
    PropertyTable child = propertyTable;
    this.Add((ReportItem) child);
    return child;
  }

  public void AddTableOfContents(ReportItem b) => this.Add((ReportItem) new TableOfContents(b));

  public virtual void Update()
  {
  }

  public virtual void Write(IReportWriter w)
  {
    this.Update();
    this.WriteContent(w);
    foreach (ReportItem child in this.Children)
      child.Write(w);
  }

  public virtual void WriteContent(IReportWriter w)
  {
  }

  protected void UpdateFigureNumbers() => this.UpdateFigureNumbers(new ReportItem.FigureCounter());

  protected void UpdateParent(Report report)
  {
    this.Report = report;
    foreach (ReportItem child in this.Children)
      child.UpdateParent(report);
  }

  private void UpdateFigureNumbers(ReportItem.FigureCounter fc)
  {
    if (this is Table table && table.Caption != null)
      table.TableNumber = fc.TableNumber++;
    if (this is Figure figure && figure.FigureText != null)
      figure.FigureNumber = fc.FigureNumber++;
    foreach (ReportItem child in this.Children)
      child.UpdateFigureNumbers(fc);
  }

  private class FigureCounter
  {
    public FigureCounter()
    {
      this.FigureNumber = 1;
      this.TableNumber = 1;
    }

    public int FigureNumber { get; set; }

    public int TableNumber { get; set; }
  }
}
