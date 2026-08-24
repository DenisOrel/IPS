// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.TableOfContents
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Reporting;

public class TableOfContents : ItemsTable
{
  public TableOfContents(ReportItem b)
    : base()
  {
    this.Base = b;
    this.Contents = new List<TableOfContents.ContentItem>();
    this.Fields.Add(new ItemsTableField((string) null, "Chapter"));
    this.Fields.Add(new ItemsTableField((string) null, "Title"));
    this.Items = (IEnumerable) this.Contents;
  }

  public ReportItem Base { get; private set; }

  public List<TableOfContents.ContentItem> Contents { get; private set; }

  public override void Update()
  {
    this.Contents.Clear();
    this.AppendHeaders(this.Base, new HeaderHelper());
    base.Update();
  }

  private void AppendHeaders(ReportItem item, HeaderHelper hh)
  {
    if (item is Header header)
    {
      header.Chapter = hh.GetHeader(header.Level);
      this.Contents.Add(new TableOfContents.ContentItem()
      {
        Chapter = header.Chapter,
        Title = header.Text
      });
    }
    foreach (ReportItem child in item.Children)
      this.AppendHeaders(child, hh);
  }

  public class ContentItem
  {
    public string Chapter { get; set; }

    public string Title { get; set; }
  }
}
