// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.ReportGeneration.ReportGeneratorHelper
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Document.Client.Reports;
using Intermech.Interfaces.Document;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Statistics.ReportGeneration;

internal class ReportGeneratorHelper : DocumentGeneratorHelper
{
  private const int CaptionColumnsCount = 1;

  public ReportGeneratorHelper(Guid documentTemplateGuid, IList<TableColumnSettings> columns)
    : base(documentTemplateGuid)
  {
    TextData node = (TextData) this.DocumentTemplate.FindNode("Заголовок строки");
    columns.First<TableColumnSettings>().Width = node.Bounds.Width;
    float num = ((RectangleElement) this.DocumentTemplate.FindNode("Заголовки столбцов")).Bounds.Width / (float) (columns.Count - 1);
    foreach (TableColumnSettings tableColumnSettings in columns.Skip<TableColumnSettings>(1))
      tableColumnSettings.Width = num;
    this.SetupHeaderColumns(columns);
    this.SetupMainTable(columns);
    this.DocumentTemplate.UpdateLayout(false);
  }

  private void SetupMainTable(IList<TableColumnSettings> columns)
  {
    DocumentGeneratorHelper.SetupOrCreateColumnsInDataTable(this.DocumentTemplate.GetFirstPageTemplate().FindFirstMainTable(), columns, new int?(1));
    foreach (PageData pageData in this.DocumentTemplate.Nodes.OfType<PageData>())
    {
      TableData firstMainTable = pageData.FindFirstMainTable();
      if (firstMainTable != null && firstMainTable.UsePreviousTableTemplates)
        firstMainTable.ApplyPreviousTableTemplate(false, false);
    }
  }

  private void SetupHeaderColumns(IList<TableColumnSettings> columns)
  {
    TableColumnSettings tableColumnSettings = columns.First<TableColumnSettings>();
    ((TextData) this.DocumentTemplate.FindNode("Заголовок строки")).AssignText(tableColumnSettings.Caption, false, false, false, false, false);
    List<TableColumnSettings> list = columns.Skip<TableColumnSettings>(1).ToList<TableColumnSettings>();
    DocumentGeneratorHelper.CreateColumnsInHeaderRow((TableData) this.DocumentTemplate.FindNode("Заголовки столбцов"), (IList<TableColumnSettings>) list, new int?(0));
  }
}
