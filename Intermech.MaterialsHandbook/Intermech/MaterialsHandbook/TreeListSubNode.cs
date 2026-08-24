// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.TreeListSubNode
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.ComponentModel;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class TreeListSubNode : ITreeListNode
{
  private ColumnHeader _column;
  private object _value;

  [Browsable(false)]
  public ColumnHeader Column
  {
    get => this._column;
    set
    {
      this._column = value;
      this.AdjustText();
    }
  }

  public string Name { get; }

  [Browsable(false)]
  public FieldTypes NodeType { get; set; }

  public string Text { get; private set; }

  [Category("Data")]
  public object Value
  {
    get => this._value;
    set
    {
      this._value = value == null ? (object) null : (!string.IsNullOrEmpty(Convert.ToString(value)) ? value : (object) null);
      this.AdjustText();
    }
  }

  public TreeListSubNode(FieldTypes type, string name = "", string value = "")
  {
    this.NodeType = type;
    this.Name = name;
    this._value = !string.IsNullOrEmpty(value) ? (object) value : (object) (string) null;
  }

  public override string ToString() => this.Text;

  private void AdjustText()
  {
    this.Text = Convert.ToString(this._value);
    if (this._column?.DataSource == null || string.IsNullOrEmpty(this._column.DisplayMember) || string.IsNullOrEmpty(this._column.ValueMember) || string.IsNullOrEmpty(this.Text))
      return;
    DataTable dataSource = this._column.DataSource;
    string columnName = dataSource.Columns.Contains("NUM_VALUE") ? "NUM_VALUE" : this._column.ValueMember;
    DataRow dataRow = dataSource.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (x => x[columnName] == this._value));
    if (dataRow == null)
      return;
    this.Text = Convert.ToString(dataRow[this._column.DisplayMember]);
  }
}
