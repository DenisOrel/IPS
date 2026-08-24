// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.Dialogs.AttributesCompareDialog
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Client.Core;
using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.Controls.Grid;
using Intermech.Interfaces.Pdm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2.Dialogs;

internal class AttributesCompareDialog : Form
{
  private List<(CompositionItemAttribute attrCopy, CompositionItemAttribute attrArticle)> _diffAttrs;
  private CompositionItem _lItem;
  private CompositionItem _rItem;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Button cancelBtn;
  private Button okBtn;
  private TableLayoutPanel tableLayoutPanel1;
  private Panel panel1;
  private Button applyAll;
  private Button applySelected;
  private Panel panel2;
  private Panel panel3;
  private Panel infoRpanel;
  private ListGrid rGrid;
  private Panel infoLpanel;
  private ListGrid lGrid;
  private IMLabel rLabel;
  private IMLabel lLabel;
  private Button RedoBtn;

  public AttributesCompareDialog(ProductionListComparer plc)
    : this(plc, plc.leftItem, plc.rightItem)
  {
  }

  public AttributesCompareDialog(
    ProductionListComparer plc,
    CompositionItem lItem,
    CompositionItem rItem)
  {
    this.InitializeComponent();
    this.Plc = plc;
    this._diffAttrs = this.Plc.CompareAttributes(lItem, rItem);
    this._lItem = lItem;
    this._rItem = rItem;
    this.FillAttrGrids(this._lItem, this._rItem);
    this.rLabel.Text = this._rItem.Caption;
    this.lLabel.Text = this._lItem.Caption;
    CompositionItemAttribute compositionItemAttribute = lItem.Attributes.Find((Predicate<CompositionItemAttribute>) (x => x.AttributeID == -5));
    if (compositionItemAttribute == null || !(compositionItemAttribute.AttributeValueText != "0"))
      return;
    this.lLabel.Text += $" [{compositionItemAttribute.Value}]";
  }

  private void FillAttrGrids(CompositionItem lItem, CompositionItem rItem)
  {
    this.Plc.NewAttributes.Clear();
    this.lGrid.BeginUpdate();
    this.rGrid.BeginUpdate();
    try
    {
      this.lGrid.Items.Clear();
      this.rGrid.Items.Clear();
      foreach ((CompositionItemAttribute attrCopy, CompositionItemAttribute attrArticle) in this._diffAttrs)
      {
        ListItem listItem1 = new ListItem(this.lGrid);
        if (attrArticle != null)
        {
          listItem1.Text = attrArticle.AttributeName;
          listItem1.SubItems.Add(attrArticle.AttributeValueText);
        }
        else
        {
          listItem1.Text = "";
          listItem1.SubItems.Add("");
        }
        this.lGrid.Items.Add(listItem1);
        ListItem listItem2 = new ListItem(this.rGrid);
        if (attrCopy != null)
        {
          listItem2.Text = attrCopy.AttributeName;
          listItem2.SubItems.Add(attrCopy.AttributeValueText);
        }
        else
        {
          listItem2.Text = "";
          listItem2.SubItems.Add("");
        }
        this.rGrid.Items.Add(listItem2);
        this.Plc.NewAttributes.Add(attrCopy);
      }
    }
    finally
    {
      this.lGrid.EndUpdate();
      this.rGrid.EndUpdate();
    }
  }

  public ProductionListComparer Plc { get; }

  public static DialogResult Execute(ProductionListComparer plc)
  {
    AttributesCompareDialog attributesCompareDialog = new AttributesCompareDialog(plc);
    return attributesCompareDialog._diffAttrs.Count == 0 ? DialogResult.OK : attributesCompareDialog.ShowDialog();
  }

  public static DialogResult Execute(
    ProductionListComparer plc,
    CompositionItem lItem,
    CompositionItem rItem)
  {
    AttributesCompareDialog attributesCompareDialog = new AttributesCompareDialog(plc, lItem, rItem);
    return attributesCompareDialog._diffAttrs.Count == 0 ? DialogResult.OK : attributesCompareDialog.ShowDialog();
  }

  private void applyBtn_Click(object sender, EventArgs e) => this.ApplyAttributes(true);

  private void ApplyAttributes(bool OnlySelected)
  {
    this.rGrid.BeginUpdate();
    try
    {
      for (int index = 0; index < this.lGrid.Items.Count; ++index)
      {
        if (OnlySelected && this.lGrid.Items[index].Selected || !OnlySelected)
          this.ApplyAttributeItem(index);
      }
    }
    finally
    {
      this.rGrid.EndUpdate();
    }
  }

  private void ApplyAttributeItem(int Index)
  {
    ListItem listItem1 = this.lGrid.Items[Index];
    if (listItem1.Tag != null || this._diffAttrs[Index].attrArticle == null)
      return;
    this.Plc.NewAttributes[Index] = new CompositionItemAttribute(this._diffAttrs[Index].attrCopy.AttributeID, this._diffAttrs[Index].attrCopy.SourceType, this._diffAttrs[Index].attrArticle.Value, this._diffAttrs[Index].attrArticle.Description);
    ListItem listItem2 = this.rGrid.Items[Index];
    if (listItem2.Text == "")
      listItem2.Text = listItem1.Text;
    listItem2.SubItems[1].Text = listItem1.SubItems[1].Text;
    listItem1.Tag = (object) 1;
  }

  private void applyAll_Click(object sender, EventArgs e) => this.ApplyAttributes(false);

  private void RedoBtn_Click(object sender, EventArgs e)
  {
    this.FillAttrGrids(this._lItem, this._rItem);
  }

  private void lGrid_DoubleClick(object sender, EventArgs e)
  {
    ArrayList selectedIndicies = this.lGrid.Items.SelectedIndicies;
    if (selectedIndicies.Count <= 0)
      return;
    this.ApplyAttributeItem((int) selectedIndicies[0]);
  }

  private void AttributesCompareDialog_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void AttributesCompareDialog_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
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
    ListColumn listColumn1 = new ListColumn();
    ListColumn listColumn2 = new ListColumn();
    ListColumn listColumn3 = new ListColumn();
    ListColumn listColumn4 = new ListColumn();
    this.cancelBtn = new Button();
    this.okBtn = new Button();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.panel3 = new Panel();
    this.infoRpanel = new Panel();
    this.rLabel = new IMLabel();
    this.rGrid = new ListGrid();
    this.panel1 = new Panel();
    this.applyAll = new Button();
    this.applySelected = new Button();
    this.panel2 = new Panel();
    this.infoLpanel = new Panel();
    this.lLabel = new IMLabel();
    this.lGrid = new ListGrid();
    this.RedoBtn = new Button();
    this.tableLayoutPanel1.SuspendLayout();
    this.panel3.SuspendLayout();
    this.infoRpanel.SuspendLayout();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.infoLpanel.SuspendLayout();
    this.SuspendLayout();
    this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.cancelBtn.DialogResult = DialogResult.Cancel;
    this.cancelBtn.Location = new Point(732, 424);
    this.cancelBtn.Name = "cancelBtn";
    this.cancelBtn.Size = new Size(75, 23);
    this.cancelBtn.TabIndex = 4;
    this.cancelBtn.Text = "Отмена";
    this.cancelBtn.UseVisualStyleBackColor = true;
    this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.okBtn.DialogResult = DialogResult.OK;
    this.okBtn.Location = new Point(651, 424);
    this.okBtn.Name = "okBtn";
    this.okBtn.Size = new Size(75, 23);
    this.okBtn.TabIndex = 3;
    this.okBtn.Text = "ОК";
    this.okBtn.UseVisualStyleBackColor = true;
    this.tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.tableLayoutPanel1.ColumnCount = 3;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel1.Controls.Add((Control) this.panel3, 2, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.panel1, 1, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.panel2, 0, 0);
    this.tableLayoutPanel1.Location = new Point(12, 12);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 1;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 406f));
    this.tableLayoutPanel1.Size = new Size(795, 406);
    this.tableLayoutPanel1.TabIndex = 5;
    this.panel3.Controls.Add((Control) this.infoRpanel);
    this.panel3.Controls.Add((Control) this.rGrid);
    this.panel3.Dock = DockStyle.Fill;
    this.panel3.Location = new Point(425, 3);
    this.panel3.Name = "panel3";
    this.panel3.Size = new Size(367, 400);
    this.panel3.TabIndex = 4;
    this.infoRpanel.Controls.Add((Control) this.rLabel);
    this.infoRpanel.Dock = DockStyle.Top;
    this.infoRpanel.Location = new Point(0, 0);
    this.infoRpanel.Name = "infoRpanel";
    this.infoRpanel.Size = new Size(367, 76);
    this.infoRpanel.TabIndex = 4;
    this.rLabel.Dock = DockStyle.Fill;
    this.rLabel.Image = (Image) null;
    this.rLabel.Location = new Point(0, 0);
    this.rLabel.Name = "rLabel";
    this.rLabel.Size = new Size(367, 76);
    this.rLabel.Text = "imLabel1";
    this.rLabel.TextAlign = ContentAlignment.MiddleCenter;
    this.rGrid.AlternateBackground = Color.DarkGreen;
    this.rGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.rGrid.BackColor = SystemColors.ControlLightLight;
    listColumn1.Name = "Column1";
    listColumn1.Text = "Имя";
    listColumn1.Width = 150;
    listColumn2.Name = "Column2";
    listColumn2.Text = "Значение";
    listColumn2.Width = 200;
    this.rGrid.Columns.AddRange(new ListColumn[2]
    {
      listColumn1,
      listColumn2
    });
    this.rGrid.GridColor = Color.LightGray;
    this.rGrid.HeaderHeight = 22;
    this.rGrid.HotTrackingColor = Color.LightGray;
    this.rGrid.ImageList = (ImageList) null;
    this.rGrid.ItemHeight = 17;
    this.rGrid.Location = new Point(3, 82);
    this.rGrid.Name = "rGrid";
    this.rGrid.SelectedTextColor = Color.White;
    this.rGrid.SelectionColor = Color.DarkBlue;
    this.rGrid.Size = new Size(360, 315);
    this.rGrid.SortType = SortType.None;
    this.rGrid.SuperFlatHeaderColor = Color.White;
    this.rGrid.TabIndex = 0;
    this.rGrid.Text = "listGrid2";
    this.panel1.Controls.Add((Control) this.applyAll);
    this.panel1.Controls.Add((Control) this.applySelected);
    this.panel1.Dock = DockStyle.Fill;
    this.panel1.Location = new Point(375, 3);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(44, 400);
    this.panel1.TabIndex = 2;
    this.applyAll.Location = new Point(3, 162);
    this.applyAll.Name = "applyAll";
    this.applyAll.Size = new Size(38, 23);
    this.applyAll.TabIndex = 1;
    this.applyAll.Text = ">>";
    this.applyAll.UseVisualStyleBackColor = true;
    this.applyAll.Click += new EventHandler(this.applyAll_Click);
    this.applySelected.Location = new Point(3, 133);
    this.applySelected.Name = "applySelected";
    this.applySelected.Size = new Size(38, 23);
    this.applySelected.TabIndex = 0;
    this.applySelected.Text = ">";
    this.applySelected.UseVisualStyleBackColor = true;
    this.applySelected.Click += new EventHandler(this.applyBtn_Click);
    this.panel2.Controls.Add((Control) this.infoLpanel);
    this.panel2.Controls.Add((Control) this.lGrid);
    this.panel2.Dock = DockStyle.Fill;
    this.panel2.Location = new Point(3, 3);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(366, 400);
    this.panel2.TabIndex = 3;
    this.infoLpanel.Controls.Add((Control) this.lLabel);
    this.infoLpanel.Dock = DockStyle.Top;
    this.infoLpanel.Location = new Point(0, 0);
    this.infoLpanel.Name = "infoLpanel";
    this.infoLpanel.Size = new Size(366, 76);
    this.infoLpanel.TabIndex = 4;
    this.lLabel.Dock = DockStyle.Fill;
    this.lLabel.Image = (Image) null;
    this.lLabel.Location = new Point(0, 0);
    this.lLabel.Name = "lLabel";
    this.lLabel.Size = new Size(366, 76);
    this.lLabel.Text = "imLabel1";
    this.lLabel.TextAlign = ContentAlignment.MiddleCenter;
    this.lGrid.AllowMultiselect = true;
    this.lGrid.AlternateBackground = Color.DarkGreen;
    this.lGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.lGrid.BackColor = SystemColors.ControlLightLight;
    listColumn3.Name = "Column1";
    listColumn3.Text = "Имя";
    listColumn3.Width = 150;
    listColumn4.Name = "Column2";
    listColumn4.Text = "Значение";
    listColumn4.Width = 200;
    this.lGrid.Columns.AddRange(new ListColumn[2]
    {
      listColumn3,
      listColumn4
    });
    this.lGrid.GridColor = Color.LightGray;
    this.lGrid.HeaderHeight = 22;
    this.lGrid.HotTrackingColor = Color.LightGray;
    this.lGrid.ImageList = (ImageList) null;
    this.lGrid.ItemHeight = 17;
    this.lGrid.Location = new Point(3, 82);
    this.lGrid.Name = "lGrid";
    this.lGrid.SelectedTextColor = Color.White;
    this.lGrid.SelectionColor = Color.DarkBlue;
    this.lGrid.Size = new Size(360, 315);
    this.lGrid.SortType = SortType.None;
    this.lGrid.SuperFlatHeaderColor = Color.White;
    this.lGrid.TabIndex = 0;
    this.lGrid.Text = "listGrid1";
    this.lGrid.DoubleClick += new EventHandler(this.lGrid_DoubleClick);
    this.RedoBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.RedoBtn.Location = new Point(12, 424);
    this.RedoBtn.Name = "RedoBtn";
    this.RedoBtn.Size = new Size(75, 23);
    this.RedoBtn.TabIndex = 6;
    this.RedoBtn.Text = "Заново";
    this.RedoBtn.UseVisualStyleBackColor = true;
    this.RedoBtn.Click += new EventHandler(this.RedoBtn_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.cancelBtn;
    this.ClientSize = new Size(819, 459);
    this.Controls.Add((Control) this.RedoBtn);
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Controls.Add((Control) this.cancelBtn);
    this.Controls.Add((Control) this.okBtn);
    this.Name = nameof (AttributesCompareDialog);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Сравнение атрибутов изделия и производственной копии";
    this.FormClosed += new FormClosedEventHandler(this.AttributesCompareDialog_FormClosed);
    this.Load += new EventHandler(this.AttributesCompareDialog_Load);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.infoRpanel.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.infoLpanel.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
