// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.TreeListNode
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class TreeListNode : ITreeListNode
{
  private TreeListView _ctrl;
  private ColumnHeader _column;
  private object _value;
  private bool _selected;

  [Browsable(false)]
  public Dictionary<string, object> AdditionalValues { get; set; }

  [Category("Appearance")]
  [DefaultValue(typeof (Color), "Window")]
  public Color BackColor { get; set; }

  [Browsable(false)]
  public TreeListView Control
  {
    get => this._ctrl;
    set
    {
      this.Nodes.Control = this._ctrl = value;
      if (this._ctrl != null && this._ctrl.Columns.Count > 0)
        this._column = this._ctrl.Columns[0];
      this.SubNodes.Control = value;
      this.AdjustText();
    }
  }

  [Category("Behavior")]
  [DefaultValue(false)]
  public bool Expanded { get; set; }

  [Category("Appearance")]
  [DefaultValue(typeof (Color), "WindowText")]
  public Color ForeColor { get; set; }

  [Browsable(false)]
  public int Index
  {
    get
    {
      TreeListNode parent = this.Parent;
      return parent == null ? -1 : parent.Nodes.IndexOf(this);
    }
  }

  [Browsable(false)]
  public string Name { get; }

  [Category("Data")]
  [Description("The collection of root nodes in the treelist.")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  [Editor(typeof (CollectionEditor), typeof (UITypeEditor))]
  public TreeListNodeCollection Nodes { get; }

  [Browsable(false)]
  public FieldTypes NodeType { get; set; }

  [Browsable(false)]
  public TreeListNode Parent { get; set; }

  [Browsable(false)]
  public DataRow Row { get; set; }

  [Browsable(false)]
  [DefaultValue(false)]
  public bool Selected
  {
    get => this._selected;
    set
    {
      int num = this._selected != value ? 1 : 0;
      this._selected = value;
      if (num == 0)
        return;
      this.OnSelectedChanged((object) this, new EventArgs());
    }
  }

  [Category("Behavior")]
  [Description("The items collection of sub controls.")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  [Editor(typeof (CollectionEditor), typeof (UITypeEditor))]
  public TreeListSubNodeCollection SubNodes { get; }

  [Category("Appearance")]
  public string Text { get; private set; }

  [Category("Appearance")]
  public object Value
  {
    get => this._value;
    set
    {
      this._value = value;
      this.AdjustText();
    }
  }

  [Category("Behavior")]
  [DefaultValue(true)]
  public bool Visible { get; set; }

  public TreeListNode(FieldTypes type = FieldTypes.ftString, string name = "", string value = "")
  {
    this.Nodes = new TreeListNodeCollection()
    {
      Owner = this
    };
    this.Nodes.SelectedChanged += new EventHandler(this.OnSelectedChanged);
    this.SubNodes = new TreeListSubNodeCollection();
    this.AdditionalValues = new Dictionary<string, object>();
    this.BackColor = SystemColors.Window;
    this.ForeColor = SystemColors.WindowText;
    this.Visible = true;
    this.NodeType = type;
    this.Name = name;
    this._value = (object) value;
  }

  public event EventHandler SelectedChanged;

  private void OnSelectedChanged(object sender, EventArgs e)
  {
    EventHandler selectedChanged = this.SelectedChanged;
    if (selectedChanged == null)
      return;
    selectedChanged(sender, e);
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

  public void CollapseAll()
  {
    foreach (TreeListNode node in (List<TreeListNode>) this.Nodes)
      node.CollapseAll();
    this.Expanded = false;
  }

  public void ExpandAll()
  {
    foreach (TreeListNode node in (List<TreeListNode>) this.Nodes)
      node.ExpandAll();
    this.Expanded = true;
  }

  public int GetNodeCount(bool includeSubTrees)
  {
    int num = 0;
    if (includeSubTrees)
    {
      foreach (TreeListNode node in (List<TreeListNode>) this.Nodes)
        num += node.GetNodeCount(true);
    }
    return num + this.Nodes.Count;
  }

  public int GetVisibleNodeCount(bool includeSubTrees)
  {
    int visibleNodeCount = 0;
    if (this.Expanded)
    {
      if (includeSubTrees)
      {
        foreach (TreeListNode node in (List<TreeListNode>) this.Nodes)
        {
          if (node.Expanded)
            visibleNodeCount += node.GetVisibleNodeCount(true);
        }
      }
      foreach (TreeListNode node in (List<TreeListNode>) this.Nodes)
      {
        if (node.Visible)
          ++visibleNodeCount;
      }
    }
    return visibleNodeCount;
  }

  public void SetValue(string name, object value)
  {
    if (this.Name == name)
    {
      this.Value = value;
    }
    else
    {
      foreach (TreeListSubNode subNode in (CollectionBase) this.SubNodes)
      {
        if (!(subNode.Name != name))
        {
          subNode.Value = value;
          break;
        }
      }
    }
  }
}
