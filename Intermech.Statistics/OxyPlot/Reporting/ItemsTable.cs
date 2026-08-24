// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.ItemsTable
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot.Reporting;

public class ItemsTable : Table
{
  public ItemsTable(bool itemsInRows = true)
  {
    this.Fields = (IList<ItemsTableField>) new List<ItemsTableField>();
    this.ItemsInRows = itemsInRows;
    this.Alignment = Alignment.Center;
  }

  public Alignment Alignment { get; set; }

  public IList<ItemsTableField> Fields { get; set; }

  public IEnumerable Items { get; set; }

  public bool ItemsInRows { get; private set; }

  public bool HasHeader()
  {
    return this.Fields.Any<ItemsTableField>((Func<ItemsTableField, bool>) (c => c.Header != null));
  }

  public string[,] ToArray()
  {
    List<object> list = this.Items.Cast<object>().ToList<object>();
    int count = list.Count;
    int num = this.HasHeader() ? 1 : 0;
    if (num != 0)
      ++count;
    string[,] input = new string[count, this.Fields.Count];
    int index1 = 0;
    if (num != 0)
    {
      for (int index2 = 0; index2 < this.Fields.Count; ++index2)
      {
        ItemsTableField field = this.Fields[index2];
        input[index1, index2] = field.Header;
      }
      ++index1;
    }
    foreach (object obj in list)
    {
      for (int index3 = 0; index3 < this.Fields.Count; ++index3)
      {
        string text = this.Fields[index3].GetText(obj, (IFormatProvider) this.Report.ActualCulture);
        input[index1, index3] = text;
      }
      ++index1;
    }
    if (!this.ItemsInRows)
      input = ItemsTable.Transpose(input);
    return input;
  }

  public override void Update()
  {
    base.Update();
    this.UpdateItems();
  }

  public void UpdateItems()
  {
    this.Rows.Clear();
    this.Columns.Clear();
    if (this.Fields == null || this.Fields.Count == 0)
      return;
    string[,] array = this.ToArray();
    int num1 = array.GetUpperBound(0) + 1;
    int num2 = array.GetUpperBound(1) + 1;
    for (int index1 = 0; index1 < num1; ++index1)
    {
      TableRow tableRow = new TableRow();
      if (this.ItemsInRows)
        tableRow.IsHeader = index1 == 0;
      this.Rows.Add(tableRow);
      for (int index2 = 0; index2 < num2; ++index2)
        tableRow.Cells.Add(new TableCell()
        {
          Content = array[index1, index2]
        });
    }
    for (int index = 0; index < num2; ++index)
    {
      TableColumn tableColumn = new TableColumn();
      if (this.ItemsInRows)
      {
        ItemsTableField field = this.Fields[index];
        tableColumn.Alignment = field.Alignment;
        tableColumn.Width = field.Width;
      }
      else
      {
        tableColumn.IsHeader = index == 0;
        tableColumn.Alignment = this.Alignment;
      }
      this.Columns.Add(tableColumn);
    }
  }

  public override void WriteContent(IReportWriter w) => w.WriteTable((Table) this);

  private static string[,] Transpose(string[,] input)
  {
    int length1 = input.GetUpperBound(0) + 1;
    int length2 = input.GetUpperBound(1) + 1;
    string[,] strArray = new string[length2, length1];
    for (int index1 = 0; index1 < length1; ++index1)
    {
      for (int index2 = 0; index2 < length2; ++index2)
        strArray[index2, index1] = input[index1, index2];
    }
    return strArray;
  }
}
