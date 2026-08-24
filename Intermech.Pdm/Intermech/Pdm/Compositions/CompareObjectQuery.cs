// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareObjectQuery
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class CompareObjectQuery : DelayedQuery
{
  private ICompareBackgroundReader _reader;
  private List<Tuple<long, int>> _compareObjects;
  private long _objectId;
  private CompareObjectsInfo _info;
  private CompareDifferences _currentDifferences;

  public CompareObjectQuery(
    INodeQuerySupport support,
    CompareObjectsInfo info,
    List<Tuple<long, int>> compareObjects,
    long objectId,
    ICompareBackgroundReader reader,
    CompareDifferences currentDifferences)
    : base(support, info.Result != null && info.Result.Count == 0)
  {
    this._reader = reader;
    this._info = info;
    this._compareObjects = compareObjects;
    this._objectId = objectId;
    this._currentDifferences = currentDifferences;
  }

  protected override DataTable GetDataTable(RecordMapping mapping)
  {
    this._currentDifferences.Differences = new Dictionary<long, List<int>>();
    this._currentDifferences.Grouping = CompareHelper.Grouping(this._info.EnabledRelationTypes, this._info.CompareAttributes, this._info.Recursive);
    if (this._reader == null)
      return this._info.Result == null || !this._info.Result.ContainsKey(this._objectId) ? (DataTable) null : this.GetResultTable(mapping, this._info.Result[this._objectId]);
    switch (this._reader.State)
    {
      case BackgroundState.Empty:
        if (!this.realQuery)
          return this.GetResultTable(mapping, this._reader.QueryResult);
        RuntimeSearchScheme scheme = new RuntimeSearchScheme(this._info.Recursive ? SearchDirection.RecursiveContains : SearchDirection.Contains, 0L, (int[]) null, this._info.EnabledRelationTypes.ToArray(), (AttributeSource[]) null);
        if (this._currentDifferences.Grouping)
          scheme.Options |= SearchOptions.ObjectGrouping;
        this._reader.Execute((object) mapping, this._info, this._compareObjects, scheme);
        return (DataTable) null;
      case BackgroundState.Error:
      case BackgroundState.Reading:
        return (DataTable) null;
      case BackgroundState.Fill:
        this._info.ColumnAttributes = new List<NodeColumnID>(mapping.Fields.Length);
        for (int index = 0; index < mapping.Fields.Length; ++index)
          this._info.ColumnAttributes.Add((NodeColumnID) mapping.Fields[index]);
        return this.GetResultTable(mapping, this._reader.QueryResult);
      default:
        return (DataTable) null;
    }
  }

  private DataTable RebuildColumns(DataTable source)
  {
    if (source == null)
      return source;
    DataTable dataTable = new DataTable(source.TableName);
    for (int index1 = 0; index1 < this.mapping.Fields.Length; ++index1)
    {
      int index2 = this._info.ColumnAttributes.IndexOf((NodeColumnID) this.mapping.Fields[index1]);
      dataTable.Columns.Add(index2 >= 0 ? new DataColumn(source.Columns[index2].ColumnName, source.Columns[index2].DataType) : new DataColumn());
    }
    for (int index3 = 0; index3 < source.Rows.Count; ++index3)
    {
      DataRow row = dataTable.NewRow();
      for (int index4 = 0; index4 < source.Columns.Count; ++index4)
      {
        if (dataTable.Columns.Contains(source.Columns[index4].ColumnName))
          row[source.Columns[index4].ColumnName] = source.Rows[index3][index4];
      }
      dataTable.Rows.Add(row);
    }
    dataTable.AcceptChanges();
    return dataTable;
  }

  private DataTable GetResultTable(RecordMapping mapping, DataTable source)
  {
    CompositionsComparer compositionsComparer = (CompositionsComparer) null;
    switch (this._info.CompositionMode)
    {
      case CompositionModes.Differences:
        compositionsComparer = (CompositionsComparer) new DifferencesComparer(mapping, this._info, this._currentDifferences, this._objectId);
        break;
      case CompositionModes.Compatibility:
        compositionsComparer = (CompositionsComparer) new CompatibilityComparer(mapping, this._info, this._currentDifferences, this._objectId);
        break;
      case CompositionModes.Composition:
        compositionsComparer = new CompositionsComparer(mapping, this._info, this._currentDifferences, this._objectId);
        break;
    }
    return compositionsComparer != null ? this.FilterTable(mapping, this.RebuildColumns(compositionsComparer.Compare())) : this.FilterTable(mapping, this.RebuildColumns(source));
  }
}
