// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.Dialogs.ReplaceVersionDialog
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Pdm.Compositions.CompareTree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2.Dialogs;

internal class ReplaceVersionDialog : Form
{
  private List<NodeColumnID> _leftColumns;
  private List<NodeColumnID> _rightColumns;
  private bool _selfScroll;
  private HybridDictionary _controlsSettings = new HybridDictionary(0, true);
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Button OkButton;
  private Button CancelButton;
  private TableLayoutPanel tableLayoutPanel1;
  private Panel panel1;
  private Button button4;
  private Button button3;
  private ToolTip toolTip1;
  private CompareTreeView leftTree;
  private CompareTreeView rightTree;

  private List<NodeColumnID> LeftColumns
  {
    get
    {
      if (this._leftColumns != null)
        return this._leftColumns;
      this._leftColumns = new List<NodeColumnID>()
      {
        new NodeColumnID((object) ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Object)
      };
      foreach (Tuple<int, int, AttributableElements> cfgCompareAttr in MRP2PropertyPage.cfg_compareAttrs)
      {
        AttributeSourceTypes AnAttrSource = cfgCompareAttr.Item3 == AttributableElements.Object ? AttributeSourceTypes.Object : AttributeSourceTypes.Relation;
        this._leftColumns.Add(new NodeColumnID((object) cfgCompareAttr.Item1, AnAttrSource));
      }
      return this._leftColumns;
    }
  }

  private List<NodeColumnID> RightColumns
  {
    get
    {
      if (this._rightColumns != null)
        return this._rightColumns;
      this._rightColumns = new List<NodeColumnID>()
      {
        new NodeColumnID((object) ObligatoryObjectAttributes.CAPTION, AttributeSourceTypes.Object)
      };
      foreach (Tuple<int, int, AttributableElements> cfgCompareAttr in MRP2PropertyPage.cfg_compareAttrs)
      {
        AttributeSourceTypes AnAttrSource = cfgCompareAttr.Item3 == AttributableElements.Object ? AttributeSourceTypes.Object : AttributeSourceTypes.Relation;
        this._rightColumns.Add(new NodeColumnID((object) cfgCompareAttr.Item2, AnAttrSource));
      }
      return this._rightColumns;
    }
  }

  private void AddTreeViewColumn(Intermech.VirtualTreeView.VirtualTreeView view, NodeColumnID columnID)
  {
    int attributeId = columnID.AttributeID;
    string name = MetaDataHelper.GetAttributeType(attributeId).Name;
    Column column = new Column()
    {
      Caption = name,
      Name = attributeId.ToString(),
      Sortable = false,
      Width = 150
    };
    column.CellStyle.HorzAlignment = StringAlignment.Near;
    column.Changed += new EventHandler(this.columnChangedEvent);
    view.Columns.Add(column);
  }

  private void columnChangedEvent(object sender, EventArgs e)
  {
    if (!(sender is Column column))
      return;
    int index1 = this.leftTree.Columns.IndexOf(column);
    if (index1 != -1)
    {
      this.rightTree.Columns[index1].Changed -= new EventHandler(this.columnChangedEvent);
      this.rightTree.Columns[index1].Width = column.Width;
      this.rightTree.Columns[index1].Changed += new EventHandler(this.columnChangedEvent);
    }
    else
    {
      int index2 = this.rightTree.Columns.IndexOf(column);
      if (index2 == -1)
        return;
      this.leftTree.Columns[index2].Changed -= new EventHandler(this.columnChangedEvent);
      this.leftTree.Columns[index2].Width = column.Width;
      this.leftTree.Columns[index2].Changed += new EventHandler(this.columnChangedEvent);
    }
  }

  private void CreateColumns()
  {
    this.leftTree.Columns.Clear();
    this.rightTree.Columns.Clear();
    foreach (NodeColumnID leftColumn in this.LeftColumns)
      this.AddTreeViewColumn((Intermech.VirtualTreeView.VirtualTreeView) this.leftTree, leftColumn);
    foreach (NodeColumnID rightColumn in this.RightColumns)
      this.AddTreeViewColumn((Intermech.VirtualTreeView.VirtualTreeView) this.rightTree, rightColumn);
  }

  public ReplaceVersionDialog(ProductionListComparer plc)
  {
    this.InitializeComponent();
    this.Plc = plc;
    this.CreateColumns();
    this.leftTree.AllowMultiSelect = true;
    this.leftTree.DisableExpandCollapse = true;
    this.leftTree.DataSource = (object) plc.leftItem;
    this.leftTree.VScrollBar.ValueChanged += new EventHandler(this.VScrollBar_ValueChanged);
    this.leftTree.HScrollBar.ValueChanged += new EventHandler(this.HScrollBar_ValueChanged);
    this.rightTree.AllowMultiSelect = true;
    this.rightTree.DisableExpandCollapse = true;
    this.rightTree.DataSource = (object) plc.rightItem;
    this.rightTree.VScrollBar.ValueChanged += new EventHandler(this.VScrollBar_ValueChanged);
    this.rightTree.HScrollBar.ValueChanged += new EventHandler(this.HScrollBar_ValueChanged);
  }

  private void HScrollBar_ValueChanged(object sender, EventArgs e)
  {
  }

  public ProductionListComparer Plc { get; }

  private void VScrollBar_ValueChanged(object sender, EventArgs e)
  {
    if (this._selfScroll)
      return;
    this._selfScroll = true;
    try
    {
      if ((VScrollBar) sender == this.leftTree.VScrollBar)
        this.rightTree.TopRowIndex = this.leftTree.TopRowIndex;
      else
        this.leftTree.TopRowIndex = this.rightTree.TopRowIndex;
    }
    finally
    {
      this._selfScroll = false;
    }
  }

  private void button3_Click(object sender, EventArgs e)
  {
    foreach (Row selectedRow in this.leftTree.SelectedRows)
    {
      if (selectedRow.Level > 0 && selectedRow.Item is CompositionItem leftItem)
        this.ApplyItem(leftItem, selectedRow);
    }
    this.rightTree.FocusRowChanged -= new EventHandler(this.rightTree_FocusRowChanged);
    try
    {
      this.rightTree.UpdateRows(false);
      this.SyncSelectedRows(this.leftTree, this.rightTree);
    }
    finally
    {
      this.rightTree.FocusRowChanged += new EventHandler(this.rightTree_FocusRowChanged);
    }
  }

  private void ApplyItem(CompositionItem leftItem, Row row)
  {
    if (!leftItem.Empty && !leftItem.CompositionItemFlag.HasFlag((Enum) CompositionItemFlags.Removed) && !leftItem.CompositionItemFlag.HasFlag((Enum) CompositionItemFlags.Added) && !leftItem.CompositionItemFlag.HasFlag((Enum) CompositionItemFlags.AttributesChanged))
      return;
    CompositionItem rItem = this.rightTree.GetRow(row.RowIndex).Item as CompositionItem;
    if (leftItem.CompositionItemFlag.HasFlag((Enum) CompositionItemFlags.AttributesChanged))
    {
      if (DialogResult.OK != AttributesCompareDialog.Execute(this.Plc, leftItem, rItem))
        return;
      CompositionItemFlags compositionItemFlags = CompositionItemFlags.CreateNewCopy;
      if (this.Plc.NewAttributes.Count > 0)
      {
        compositionItemFlags |= CompositionItemFlags.AttributesChangedInCompositionObject;
        foreach (CompositionItemAttribute newAttribute in this.Plc.NewAttributes)
        {
          CompositionItemAttribute attribute = newAttribute;
          if (attribute.SourceType == AttributeSourceTypes.Object)
            compositionItemFlags = CompositionItemFlags.AttributesChanged | CompositionItemFlags.CreateNewCopy;
          int index = rItem.Attributes.FindIndex((Predicate<CompositionItemAttribute>) (x => x.AttributeID == attribute.AttributeID));
          if (index != -1)
            rItem.Attributes[index] = attribute;
        }
      }
      if (leftItem.CompositionItemFlag.HasFlag((Enum) CompositionItemFlags.AnotherVersion))
      {
        rItem.UpdateNewVersion(leftItem);
        compositionItemFlags |= CompositionItemFlags.AnotherVersion;
      }
      rItem.CompositionItemFlag = compositionItemFlags;
    }
    else
    {
      CompositionItem compositionItem = new CompositionItem(rItem.Parent, leftItem.Empty, leftItem.LevelIndex);
      if (!leftItem.Empty)
      {
        foreach (CompositionItemAttribute attribute in leftItem.Attributes)
          compositionItem.Attributes.Add((CompositionItemAttribute) attribute.Clone());
      }
      compositionItem.CompositionItemFlag = CompositionItemFlags.CreateNewCopy;
      rItem.Parent[row.ChildIndex] = compositionItem;
    }
  }

  private void button4_Click(object sender, EventArgs e)
  {
    for (int childIndex = 0; childIndex < this.leftTree.RootRow.NumChildren; ++childIndex)
    {
      Row row = this.leftTree.RootRow.ChildRowByIndex(childIndex);
      if (row.Level > 0 && row.Item is CompositionItem leftItem)
        this.ApplyItem(leftItem, row);
    }
    this.rightTree.FocusRowChanged -= new EventHandler(this.rightTree_FocusRowChanged);
    try
    {
      this.rightTree.UpdateRows(false);
      this.SyncSelectedRows(this.leftTree, this.rightTree);
    }
    finally
    {
      this.rightTree.FocusRowChanged += new EventHandler(this.rightTree_FocusRowChanged);
    }
  }

  private void leftTree_FocusRowChanged(object sender, EventArgs e)
  {
    this.rightTree.FocusRowChanged -= new EventHandler(this.rightTree_FocusRowChanged);
    try
    {
      this.SyncSelectedRows(this.leftTree, this.rightTree);
    }
    finally
    {
      this.rightTree.FocusRowChanged += new EventHandler(this.rightTree_FocusRowChanged);
    }
  }

  private void SyncSelectedRows(CompareTreeView Source, CompareTreeView Destination)
  {
    Destination.SelectedRows.Clear();
    foreach (Row selectedRow in Source.SelectedRows)
    {
      int rowIndex = selectedRow.RowIndex;
      Row row = Destination.GetRow(rowIndex);
      if (row != null)
        row.Selected = true;
    }
    int rowIndex1 = Source.FocusRow.RowIndex;
    Row row1 = Destination.GetRow(rowIndex1);
    if (row1 == null)
      return;
    Destination.FocusRow = row1;
  }

  private void rightTree_FocusRowChanged(object sender, EventArgs e)
  {
    this.leftTree.FocusRowChanged -= new EventHandler(this.leftTree_FocusRowChanged);
    try
    {
      this.SyncSelectedRows(this.rightTree, this.leftTree);
    }
    finally
    {
      this.leftTree.FocusRowChanged += new EventHandler(this.leftTree_FocusRowChanged);
    }
  }

  private void rightTree_GetCellData(object sender, GetCellDataEventArgs e)
  {
    if (!(e.Row.Item is CompositionItem))
      return;
    CompositionItem compositionItem = (CompositionItem) e.Row.Item;
    if (compositionItem.Attributes == null)
      return;
    CompositionItemAttribute compositionItemAttribute = compositionItem.Attributes.Find((Predicate<CompositionItemAttribute>) (x => x.AttributeID == MRP2Consts.attrIdDeleteTag));
    if (compositionItemAttribute == null || DataSetProcessor.GetInt32Value(compositionItemAttribute.Value, 0) == 0)
      return;
    Font font = new Font(e.Column.CellStyle.Font, FontStyle.Strikeout);
    StyleDelta delta = new StyleDelta();
    delta.Font = font;
    e.CellData.OddStyle = new Style(e.CellData.OddStyle, delta);
    e.CellData.EvenStyle = new Style(e.CellData.EvenStyle, delta);
  }

  private void ReplaceVersionDialog_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this, (IDictionary) this._controlsSettings);
    this.SetControlsState(this._controlsSettings);
  }

  private void SetControlsState(HybridDictionary controlsSettings)
  {
    for (int index = 0; index < this.leftTree.Columns.Count; ++index)
    {
      Column column = this.leftTree.Columns[index];
      string key = "column" + column.Name;
      if (controlsSettings.Contains((object) key))
        column.Width = (int) controlsSettings[(object) key];
    }
  }

  private void ReplaceVersionDialog_FormClosed(object sender, FormClosedEventArgs e)
  {
    this.GetControlsState(this._controlsSettings);
    FormStorage.SaveLayout((Control) this, (IDictionary) this._controlsSettings);
  }

  private void GetControlsState(HybridDictionary controlsSettings)
  {
    for (int index = 0; index < this.leftTree.Columns.Count; ++index)
    {
      Column column = this.leftTree.Columns[index];
      string key = "column" + column.Name;
      controlsSettings[(object) key] = (object) column.Width;
    }
  }

  private void rightTree_RowCollapse(object sender, RowEventArgs e)
  {
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.OkButton = new Button();
    this.CancelButton = new Button();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.panel1 = new Panel();
    this.button4 = new Button();
    this.button3 = new Button();
    this.leftTree = new CompareTreeView();
    this.rightTree = new CompareTreeView();
    this.toolTip1 = new ToolTip(this.components);
    this.tableLayoutPanel1.SuspendLayout();
    this.panel1.SuspendLayout();
    this.leftTree.BeginInit();
    this.rightTree.BeginInit();
    this.SuspendLayout();
    this.OkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.OkButton.DialogResult = DialogResult.OK;
    this.OkButton.Location = new Point(630, 440);
    this.OkButton.Name = "OkButton";
    this.OkButton.Size = new Size(75, 23);
    this.OkButton.TabIndex = 0;
    this.OkButton.Text = "ОК";
    this.OkButton.UseVisualStyleBackColor = true;
    this.CancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.CancelButton.DialogResult = DialogResult.Cancel;
    this.CancelButton.Location = new Point(711, 440);
    this.CancelButton.Name = "CancelButton";
    this.CancelButton.Size = new Size(75, 23);
    this.CancelButton.TabIndex = 1;
    this.CancelButton.Text = "Отмена";
    this.CancelButton.UseVisualStyleBackColor = true;
    this.tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.tableLayoutPanel1.ColumnCount = 3;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel1.Controls.Add((Control) this.panel1, 1, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.leftTree, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.rightTree, 2, 0);
    this.tableLayoutPanel1.Location = new Point(12, 12);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 1;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 422f));
    this.tableLayoutPanel1.Size = new Size(774, 422);
    this.tableLayoutPanel1.TabIndex = 6;
    this.panel1.Controls.Add((Control) this.button4);
    this.panel1.Controls.Add((Control) this.button3);
    this.panel1.Dock = DockStyle.Fill;
    this.panel1.Location = new Point(365, 3);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(44, 416);
    this.panel1.TabIndex = 2;
    this.button4.Location = new Point(3, 162);
    this.button4.Name = "button4";
    this.button4.Size = new Size(38, 23);
    this.button4.TabIndex = 1;
    this.button4.Text = ">>";
    this.toolTip1.SetToolTip((Control) this.button4, "Перенести весь состав");
    this.button4.UseVisualStyleBackColor = true;
    this.button4.Click += new EventHandler(this.button4_Click);
    this.button3.Location = new Point(3, 133);
    this.button3.Name = "button3";
    this.button3.Size = new Size(38, 23);
    this.button3.TabIndex = 0;
    this.button3.Text = ">";
    this.toolTip1.SetToolTip((Control) this.button3, "Перенести отмеченные изменения");
    this.button3.UseVisualStyleBackColor = true;
    this.button3.Click += new EventHandler(this.button3_Click);
    this.leftTree.AllowDrop = true;
    this.leftTree.AllowMultiSelect = false;
    this.leftTree.Control = (object) null;
    this.leftTree.DisableHeaderContextMenu = false;
    this.leftTree.Dock = DockStyle.Fill;
    this.leftTree.ImageList = (ImageList) null;
    this.leftTree.Location = new Point(3, 3);
    this.leftTree.Name = "leftTree";
    this.leftTree.SelectedItems = (ISelectedItems) null;
    this.leftTree.Services = (IServiceProvider) null;
    this.leftTree.Size = new Size(356, 416);
    this.leftTree.TabIndex = 3;
    this.leftTree.FocusRowChanged += new EventHandler(this.leftTree_FocusRowChanged);
    this.rightTree.AllowDrop = true;
    this.rightTree.AllowMultiSelect = false;
    this.rightTree.Control = (object) null;
    this.rightTree.DisableHeaderContextMenu = false;
    this.rightTree.Dock = DockStyle.Fill;
    this.rightTree.ImageList = (ImageList) null;
    this.rightTree.Location = new Point(415, 3);
    this.rightTree.Name = "rightTree";
    this.rightTree.SelectedItems = (ISelectedItems) null;
    this.rightTree.Services = (IServiceProvider) null;
    this.rightTree.Size = new Size(356, 416);
    this.rightTree.TabIndex = 4;
    this.rightTree.FocusRowChanged += new EventHandler(this.rightTree_FocusRowChanged);
    this.rightTree.GetCellData += new GetCellDataHandler(this.rightTree_GetCellData);
    this.rightTree.RowCollapse += new RowEventHandler(this.rightTree_RowCollapse);
    this.AcceptButton = (IButtonControl) this.OkButton;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(798, 475);
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Controls.Add((Control) this.CancelButton);
    this.Controls.Add((Control) this.OkButton);
    this.Name = nameof (ReplaceVersionDialog);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Редактор замены версии объекта  в составе ПВ";
    this.FormClosed += new FormClosedEventHandler(this.ReplaceVersionDialog_FormClosed);
    this.Load += new EventHandler(this.ReplaceVersionDialog_Load);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.leftTree.EndInit();
    this.rightTree.EndInit();
    this.ResumeLayout(false);
  }
}
