// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompositionsComparer
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class CompositionsComparer
{
  protected RecordMapping mapping;
  protected CompareObjectsInfo info;
  protected long curObjectID;
  protected CompareDifferences differences;

  public CompositionsComparer(
    RecordMapping mapping,
    CompareObjectsInfo info,
    CompareDifferences currentDifferences,
    long curObjectID)
  {
    this.mapping = mapping;
    this.info = info;
    this.curObjectID = curObjectID;
    this.differences = currentDifferences;
  }

  public DataTable Compare()
  {
    if (this.info.Result == null || !this.info.Result.ContainsKey(this.curObjectID))
      return (DataTable) null;
    DataTable dataTable = this.info.Result[this.curObjectID];
    if (dataTable == null)
      return (DataTable) null;
    int num = this.info.ColumnAttributes.IndexOf(new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object));
    string columnName = dataTable.Columns[num].ColumnName;
    DataTable toTable = dataTable.Clone();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        if (this.CompareRow(sessionKeeper.Session, dataTable.Rows[index], this.curObjectID, columnName, Convert.ToInt64(dataTable.Rows[index][num]), this.info.Result))
          DataSetProcessor.AddRow(toTable, dataTable.Rows[index], false);
      }
    }
    if (toTable.Rows.Count > 0)
      toTable.AcceptChanges();
    return toTable;
  }

  protected DataRow[] Select(DataTable table, string idColumnName, object id)
  {
    return table?.Select($"[{idColumnName}]={id}");
  }

  protected bool CompareAttribute(
    DataRow[] rows,
    DataRow compareRow,
    IDBAttributeType attr,
    int index)
  {
    bool flag = true;
    for (int index1 = 0; index1 < rows.Length; ++index1)
    {
      if (attr.AttributeType == FieldTypes.ftMeasured)
        flag = CompareValuesHelper.CompareMeasuredValues(compareRow[index], rows[index1][index]);
      else if (attr.AttributeType == FieldTypes.ftObjectLink)
      {
        index = this.info.AttrIDIndexes[attr.AttributeID];
        flag = CompareValuesHelper.CompareIntValues(compareRow[index], rows[index1][index]);
      }
      else
      {
        switch (attr.TextFieldName)
        {
          case "F_INTEGER_VALUE":
            flag = CompareValuesHelper.CompareIntValues(compareRow[index], rows[index1][index]);
            break;
          case "F_STRING_VALUE":
            flag = CompareValuesHelper.CompareStringValues(compareRow[index], rows[index1][index]);
            break;
          case "F_DOUBLE_VALUE":
            flag = CompareValuesHelper.CompareFloatValues(compareRow[index], rows[index1][index]);
            break;
          case "F_DATE_VALUE":
            flag = CompareValuesHelper.CompareDateTimeValues(compareRow[index], rows[index1][index]);
            break;
        }
      }
      if (!flag)
        break;
    }
    return flag;
  }

  protected virtual bool CompareRow(
    IUserSession session,
    DataRow compareRow,
    long curObjectID,
    string idColumnName,
    long id,
    Dictionary<long, DataTable> results)
  {
    foreach (KeyValuePair<long, DataTable> result in results)
    {
      if (result.Key != curObjectID)
      {
        DataRow[] rows = this.Select(result.Value, idColumnName, (object) id);
        if (rows != null && rows.Length != 0)
        {
          if (this.info.CompareAttributes != null && this.info.CompareAttributes.Count > 0)
          {
            for (int index1 = 0; index1 < this.info.CompareAttributes.Count; ++index1)
            {
              IDBAttributeType attributeType = session.GetAttributeType(this.info.CompareAttributes[index1]);
              int index2 = this.info.ColumnAttributes.IndexOf(new NodeColumnID((object) attributeType.AttributeID, AttributeSourceTypes.Relation));
              if (index2 != -1 && !this.CompareAttribute(rows, compareRow, attributeType, index2))
              {
                if (!this.differences.Differences.ContainsKey(id))
                  this.differences.Differences.Add(id, new List<int>());
                List<int> intList;
                if (!this.differences.Differences.TryGetValue(id, out intList))
                {
                  intList = new List<int>();
                  this.differences.Differences[id] = intList;
                }
                intList.Add(attributeType.AttributeID);
              }
            }
          }
        }
        else
        {
          if (!this.differences.Differences.ContainsKey(id))
          {
            this.differences.Differences.Add(id, (List<int>) null);
            break;
          }
          break;
        }
      }
    }
    return true;
  }
}
