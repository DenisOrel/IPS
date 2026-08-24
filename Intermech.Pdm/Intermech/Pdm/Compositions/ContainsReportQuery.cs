// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ContainsReportQuery
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using ImSSP;
using Intermech.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class ContainsReportQuery : ContainsQuery
{
  private NodeColumnCollection _columns;

  public ContainsReportQuery(
    INodeQuerySupport support,
    long objectID,
    SearchSchemeID scheme,
    BackgroundReader reader,
    bool inProducts,
    bool realQuery,
    NodeColumnCollection Columns,
    BackgroundReader oldReader)
    : base(support, objectID, scheme, reader, inProducts, realQuery)
  {
    this._columns = Columns;
    this.reader = oldReader;
  }

  protected override DataTable GetDataTable(RecordMapping mapping)
  {
    if (this.reader != null && this.reader.QueryResult != null)
    {
      if (this.reader.QueryResult.Rows.Count == 0)
        return (DataTable) null;
      bool flag = true;
      for (int index = 0; index < mapping.Count; ++index)
      {
        if (!this.InCollection((int) mapping[index].Column.ID, this._columns))
        {
          flag = false;
          break;
        }
      }
      if (flag)
        return this.FormingDataTable(mapping);
    }
    int count = mapping.Count;
    for (int index1 = 0; index1 < this.reader.Mapping.Count; ++index1)
    {
      RecordMappingItem recordMappingItem = this.reader.Mapping[index1];
      bool flag = false;
      for (int index2 = 0; index2 < mapping.Fields.Length; ++index2)
      {
        if (((NodeColumnID) mapping.Fields[index2]).AttributeID == ((NodeColumnID) recordMappingItem.Field).AttributeID && ((NodeColumnID) mapping.Fields[index2]).AttrSource == ((NodeColumnID) recordMappingItem.Field).AttrSource)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        mapping.RegisterColumn(recordMappingItem.Column, recordMappingItem.Field, recordMappingItem.Transform);
    }
    using (FixEditingContext fixEditingContext = new FixEditingContext())
    {
      this.reader = new BackgroundReader((IServiceProvider) this.services)
      {
        EditingContext = fixEditingContext.EditingContext
      };
      this.reader.Execute(mapping, this.objectID, this.scheme.SchemeID, this.inProducts, VersionsRuleSources.GetEditorRule().OwnerId);
    }
    using (ReportQueryForm reportQueryForm = new ReportQueryForm(this.reader))
    {
      if (reportQueryForm.ShowDialog() == DialogResult.OK)
        return this.reader.QueryResult;
      if (this.reader.State == BackgroundState.Error)
      {
        int num = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString(sc_16685.ssp_pdm_16686()), LocalizationHolder.rm.GetString("Pdm_57"), MessageBoxButtons.OK, IMMessageBoxImage.Error);
      }
      return (DataTable) null;
    }
  }

  private DataTable FormingDataTable(RecordMapping mapping)
  {
    for (int index = 0; index < mapping.Count; ++index)
      this.reader.QueryResult.Columns[index].ColumnName = Convert.ToString((int) mapping[index].Column.ID);
    DataTable toTable = new DataTable(this.reader.QueryResult.TableName);
    for (int index = 0; index < mapping.Count; ++index)
    {
      DataColumn column = new DataColumn(Convert.ToString((int) mapping[index].Column.ID));
      toTable.Columns.Add(column);
    }
    foreach (DataRow row in (InternalDataCollectionBase) this.reader.QueryResult.Rows)
      DataSetProcessor.AddRow(toTable, row, false);
    toTable.AcceptChanges();
    return toTable;
  }

  private bool InCollection(int attributeID, NodeColumnCollection collection)
  {
    foreach (NodeColumn nodeColumn in (List<NodeColumn>) collection)
    {
      if ((int) nodeColumn.ID == attributeID)
        return true;
    }
    return false;
  }
}
