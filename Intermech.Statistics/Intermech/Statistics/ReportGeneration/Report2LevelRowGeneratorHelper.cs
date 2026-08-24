// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.ReportGeneration.Report2LevelRowGeneratorHelper
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Document.Client.Reports;
using Intermech.Document.Model;
using Intermech.Interfaces.Document;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Statistics.ReportGeneration;

internal class Report2LevelRowGeneratorHelper : DocumentGeneratorHelper
{
  private readonly IList<TableColumnSettings> _columns;

  public Report2LevelRowGeneratorHelper(
    Guid documentTemplateGuid,
    IList<TableColumnSettings> columns,
    int? columnsOnPage = null)
    : base(documentTemplateGuid)
  {
    this._columns = columns;
    this.SetupMainTable(this._columns, columnsOnPage);
    this.DocumentTemplate.UpdateLayout(false);
  }

  private void SetupMainTable(IList<TableColumnSettings> columns, int? columnsOnPage)
  {
    TableData defaultDataRowTemplate = DocumentGeneratorHelper.FindDefaultDataRowTemplate(this.DocumentTemplate);
    TableData dataSubTable = this.FindDataSubTable(defaultDataRowTemplate);
    if (!columnsOnPage.HasValue)
      columnsOnPage = new int?(this.FindDataCaptionsRowInSubTable(this.FindDataSubTable(defaultDataRowTemplate)).NodesCount);
    float width = defaultDataRowTemplate.Bounds.Width / (float) columnsOnPage.Value;
    List<TableColumnSettings> columns1 = new List<TableColumnSettings>(columnsOnPage.Value);
    HorzAlignment textAlignment = columns.Count > 1 ? columns[1].TextAlignment : (columns.Count > 0 ? columns[0].TextAlignment : HorzAlignment.Center);
    for (int index = 0; index < columnsOnPage.Value; ++index)
      columns1.Add(new TableColumnSettings($"Столбец {index}", width, textAlignment));
    DocumentGeneratorHelper.SetupOrCreateColumnsInDataTable(dataSubTable, (IList<TableColumnSettings>) columns1, new int?(0));
  }

  private TableData FindDataSubTable(TableData dataRow) => (TableData) dataRow.Nodes[1];

  private TableData FindDataCaptionsRowInSubTable(TableData dataSubTable)
  {
    return (TableData) dataSubTable.Nodes[0];
  }

  private TableData FindDataRowInSubTable(TableData dataSubTable)
  {
    return (TableData) dataSubTable.Nodes[1];
  }

  public override ImDocument GenerateDocument(DataTable sourceDataTable, string tableCaption)
  {
    ImDocument doc = new ImDocument(this.DocumentTemplate, true, true);
    this.SetTableCaption(doc, tableCaption);
    TableData defaultDataRowTemplate = DocumentGeneratorHelper.FindDefaultDataRowTemplate(doc);
    int nodesCount = this.FindDataCaptionsRowInSubTable(this.FindDataSubTable(defaultDataRowTemplate)).NodesCount;
    TableData nodeFromTemplate = (TableData) doc.FindFirstNodeFromTemplate((DocumentTreeNode) defaultDataRowTemplate.TopLevelTable);
    foreach (DataRow row in (InternalDataCollectionBase) sourceDataTable.Rows)
    {
      TableData dataRow = (TableData) nodeFromTemplate.AddRowByTemplate((RectangleElement) defaultDataRowTemplate);
      ((TextData) dataRow.Nodes[0]).AssignText(row[0].ToString(), false, false, false, false, false);
      TableData dataSubTable1 = this.FindDataSubTable(dataRow);
      TableData captionsRowInSubTable = this.FindDataCaptionsRowInSubTable(dataSubTable1);
      TableData dataRowInSubTable = this.FindDataRowInSubTable(dataSubTable1);
      int index1 = 0;
      for (int index2 = 1; index2 < sourceDataTable.Columns.Count; ++index2)
      {
        if (index1 == nodesCount)
        {
          TableData dataSubTable2 = (TableData) dataRow.AddRowByTemplate((RectangleElement) dataSubTable1.Template);
          captionsRowInSubTable = this.FindDataCaptionsRowInSubTable(dataSubTable2);
          dataRowInSubTable = this.FindDataRowInSubTable(dataSubTable2);
          index1 = 0;
        }
        string caption = this._columns[index2].Caption;
        ((TextData) captionsRowInSubTable.Nodes[index1]).AssignText(caption, false, false, false, false, false);
        string str = row[index2].ToString();
        ((TextData) dataRowInSubTable.Nodes[index1]).AssignText(str, false, false, false, false, false);
        ++index1;
      }
    }
    doc.UpdateLayout(false);
    return doc;
  }
}
