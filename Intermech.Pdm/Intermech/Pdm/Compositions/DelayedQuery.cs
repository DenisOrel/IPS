// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.DelayedQuery
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Expert;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal abstract class DelayedQuery : BaseNodeQuery, IContextAware
{
  protected bool realQuery;
  protected INodeQuerySupport support;
  protected List<DataRow> rows;
  private const string _asc = " ASC";
  private const string _desc = " DESC";
  protected AdvancedServiceContainer services = new AdvancedServiceContainer();

  public DelayedQuery(INodeQuerySupport support, bool realQuery)
  {
    this.support = support;
    this.realQuery = realQuery;
  }

  protected override INodeQuerySupport Support => this.support;

  IServiceProvider IContextAware.Services
  {
    [DebuggerStepThrough] get => (IServiceProvider) this.services;
    set => this.services.AdvancedProvider = value;
  }

  private int GetColumnIndex(RecordMapping mapping, int attrID)
  {
    int columnIndex = -1;
    for (int index = 0; index < mapping.Fields.Length; ++index)
    {
      if (((NodeColumnID) mapping.Fields[index]).AttributeID == attrID)
      {
        columnIndex = index;
        break;
      }
    }
    return columnIndex;
  }

  protected override NodeQueryResult Execute(object[] recordIds, RecordMapping mapping)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      List<ColumnDescriptor> columns = new List<ColumnDescriptor>();
      List<int> intList = new List<int>();
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      foreach (NodeColumnID nodeColumnId in ((IEnumerable<object>) mapping.Fields).Select<object, NodeColumnID>((System.Func<object, NodeColumnID>) (field => field as NodeColumnID)).Where<NodeColumnID>((System.Func<NodeColumnID, bool>) (field => field != null)))
      {
        if (!(nodeColumnId.ID is ObligatoryObjectAttributes) || ObligatoryObjectAttributesHelper.GetAttributeSourceType((ObligatoryObjectAttributes) nodeColumnId.ID) == AttributeSourceTypes.Object)
        {
          int num = intList.IndexOf(nodeColumnId.AttributeID);
          if (num == -1)
          {
            num = intList.Count;
            intList.Add(nodeColumnId.AttributeID);
            columns.Add(new ColumnDescriptor(nodeColumnId.ID, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
          }
          dictionary[this.GetColumnIndex(mapping, nodeColumnId.AttributeID)] = num;
        }
      }
      if (columns.Count == 0)
        return NodeQueryResult.Empty;
      int num1 = -2;
      int columnIndex1 = this.GetColumnIndex(mapping, num1);
      if (columnIndex1 == -1)
        return NodeQueryResult.Empty;
      DataTable dataTable = this.GetDataTable(mapping);
      if (dataTable == null)
        return NodeQueryResult.Empty;
      int columnIndex2 = intList.IndexOf(num1);
      if (columnIndex2 == -1)
      {
        columnIndex2 = intList.Count;
        intList.Add(num1);
        columns.Add(new ColumnDescriptor((object) num1, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0));
      }
      DataTable objectData = DataHelper.GetObjectData(-1, sessionKeeper.Session, (IEnumerable<ConditionStructure>) null, (IEnumerable<ColumnDescriptor>) columns, recordIds.Cast<long>());
      if (objectData == null)
        return NodeQueryResult.Empty;
      this.rows = new List<DataRow>();
      List<long> longList = new List<long>((IEnumerable<long>) Array.ConvertAll<object, long>(recordIds, (Converter<object, long>) (x => Math.Abs((long) x))));
      foreach (DataRow row1 in (InternalDataCollectionBase) objectData.Rows)
      {
        long num2 = Math.Abs(Convert.ToInt64(row1[columnIndex2]));
        for (int index = 0; index < dataTable.Rows.Count; ++index)
        {
          DataRow row2 = dataTable.Rows[index];
          long num3 = Math.Abs(Convert.ToInt64(row2[columnIndex1]));
          if (num2 == num3)
          {
            this.rows.Add(row2);
            foreach (KeyValuePair<int, int> keyValuePair in dictionary)
              row2[keyValuePair.Key] = row1[keyValuePair.Value];
          }
        }
      }
      return new NodeQueryResult(this.rows.Count, this.TotalRecordCount, mapping.Fields);
    }
  }

  protected override NodeQueryResult Execute(object bookmark, int count, RecordMapping mapping)
  {
    DataTable dataTable = this.GetDataTable(mapping);
    if (dataTable == null || dataTable.Rows.Count <= 0)
      return NodeQueryResult.Empty;
    this.rows = new List<DataRow>();
    for (int index = 0; index < dataTable.Rows.Count; ++index)
      this.rows.Add(dataTable.Rows[index]);
    return new NodeQueryResult(this.rows.Count, this.TotalRecordCount, mapping.Fields);
  }

  protected override object[] GetFieldValues(int index) => this.rows[index].ItemArray;

  private string GetSortOrder(RecordMapping mapping)
  {
    if (mapping.SortFields == null)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder(mapping.SortFields.Length * 32 /*0x20*/);
    stringBuilder.Append((string) mapping.SortFields[0]);
    stringBuilder.Append(mapping.SortOrders[0] == NodeColumnSortOrder.Ascending ? " ASC" : " DESC");
    for (int index = 1; index < mapping.SortFields.Length; ++index)
    {
      stringBuilder.Append(',');
      stringBuilder.Append((string) mapping.SortFields[index]);
      stringBuilder.Append(mapping.SortOrders[index] == NodeColumnSortOrder.Ascending ? " ASC" : " DESC");
    }
    return stringBuilder.ToString();
  }

  private object[] GetFieldsOrder(DataTable dataTable)
  {
    object[] fieldsOrder = new object[dataTable.Columns.Count];
    for (int index = 0; index < fieldsOrder.Length; ++index)
      fieldsOrder[index] = (object) dataTable.Columns[index].ColumnName;
    return fieldsOrder;
  }

  protected virtual DataTable GetDataTable(RecordMapping mapping) => (DataTable) null;
}
