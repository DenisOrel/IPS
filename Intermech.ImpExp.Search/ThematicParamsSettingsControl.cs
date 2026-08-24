// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ThematicParamsSettingsControl
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.ImpExp.Search.ItemFactories;
using Intermech.ImpExp.Search.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Search;

public class ThematicParamsSettingsControl : StepControl
{
  private Image _image;
  private IContainer components;
  private SplitContainer splitContainer1;
  private TableLayoutPanel tableLayoutPanel1;
  private Panel panelOptions;
  private TreeView treeViewSrc;
  private TableLayoutPanel tableLayoutPanel2;
  private Button buttonApply;
  private Button buttonSelectAT;
  private Button buttonCancel;
  private PropertyGrid propertyGridAT;
  private TextBox textBoxAT;
  private Label label1;
  private CheckBox checkBoxShowOnlyError;
  private CheckBox checkBoxShowCuted;
  private CheckBox checkBoxShowConvert;

  public ThematicParamsSettingsControl() => this.InitializeComponent();

  protected override string getCaption() => "Настройка тематических параметров";

  protected override Image getImage()
  {
    if (this._image == null)
      this._image = (Image) Resources.ArchiveSettings;
    return this._image;
  }

  public override SaveSettingsResult SaveSettings() => SaveSettingsResult.ssrOk;

  private void AddNode(TreeNode node, TreeNode parentNode)
  {
    if (parentNode == null)
      this.treeViewSrc.Nodes.Add(node);
    else
      parentNode.Nodes.Add(node);
  }

  internal void AddThematicParamsGroup(IThematicParamsGroupItem newItem)
  {
    this.treeViewSrc.Invoke((Delegate) new ThematicParamsSettingsControl.AddNodeHandler(this.AddNode), (object) new TreeNode(newItem.Label)
    {
      Name = newItem.GroupId.ToString(),
      Tag = (object) newItem
    }, null);
  }

  internal void AddThematicParams(IThematicParamsItem newItem, int groupID)
  {
    TreeNode treeNode = new TreeNode(newItem.Label);
    treeNode.Name = newItem.ParamId.ToString();
    treeNode.Tag = (object) newItem;
    TreeNode node = this.treeViewSrc.Nodes[groupID.ToString()];
    if (node == null)
      return;
    this.treeViewSrc.Invoke((Delegate) new ThematicParamsSettingsControl.AddNodeHandler(this.AddNode), (object) treeNode, (object) node);
  }

  private void treeViewSrc_AfterSelect(object sender, TreeViewEventArgs e)
  {
    if (e.Node == null)
      return;
    this.updatePropertyGrid(e.Node.Tag);
  }

  private void updatePropertyGrid(object nodeTag)
  {
    switch (nodeTag)
    {
      case null:
        this.propertyGridAT.SelectedObject = (object) null;
        this.textBoxAT.Text = string.Empty;
        break;
      case IThematicParamsGroupItem _:
        IThematicParamsGroupItem thematicParamsGroupItem = nodeTag as IThematicParamsGroupItem;
        this.propertyGridAT.SelectedObject = (object) thematicParamsGroupItem;
        this.textBoxAT.Text = thematicParamsGroupItem.Label;
        break;
      case IThematicParamsItem _:
        IThematicParamsItem thematicParamsItem = nodeTag as IThematicParamsItem;
        this.propertyGridAT.SelectedObject = (object) thematicParamsItem;
        this.textBoxAT.Text = thematicParamsItem.Label;
        break;
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.splitContainer1 = new SplitContainer();
    this.treeViewSrc = new TreeView();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.panelOptions = new Panel();
    this.buttonCancel = new Button();
    this.buttonSelectAT = new Button();
    this.buttonApply = new Button();
    this.tableLayoutPanel2 = new TableLayoutPanel();
    this.textBoxAT = new TextBox();
    this.propertyGridAT = new PropertyGrid();
    this.label1 = new Label();
    this.checkBoxShowCuted = new CheckBox();
    this.checkBoxShowOnlyError = new CheckBox();
    this.checkBoxShowConvert = new CheckBox();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.tableLayoutPanel1.SuspendLayout();
    this.panelOptions.SuspendLayout();
    this.tableLayoutPanel2.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.SetColumnSpan((Control) this.splitContainer1, 2);
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.Location = new Point(3, 66);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.treeViewSrc);
    this.splitContainer1.Panel2.Controls.Add((Control) this.tableLayoutPanel2);
    this.splitContainer1.Size = new Size(567, 399);
    this.splitContainer1.SplitterDistance = 211;
    this.splitContainer1.TabIndex = 0;
    this.treeViewSrc.Dock = DockStyle.Fill;
    this.treeViewSrc.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.treeViewSrc.FullRowSelect = true;
    this.treeViewSrc.HideSelection = false;
    this.treeViewSrc.Location = new Point(0, 0);
    this.treeViewSrc.Name = "treeViewSrc";
    this.treeViewSrc.Size = new Size(211, 399);
    this.treeViewSrc.TabIndex = 11;
    this.treeViewSrc.AfterSelect += new TreeViewEventHandler(this.treeViewSrc_AfterSelect);
    this.tableLayoutPanel1.ColumnCount = 2;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52.31214f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 47.68786f));
    this.tableLayoutPanel1.Controls.Add((Control) this.panelOptions, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.splitContainer1, 0, 1);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 2;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 63f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 72.70341f));
    this.tableLayoutPanel1.Size = new Size(573, 468);
    this.tableLayoutPanel1.TabIndex = 12;
    this.tableLayoutPanel1.SetColumnSpan((Control) this.panelOptions, 2);
    this.panelOptions.Controls.Add((Control) this.label1);
    this.panelOptions.Controls.Add((Control) this.checkBoxShowOnlyError);
    this.panelOptions.Controls.Add((Control) this.checkBoxShowCuted);
    this.panelOptions.Controls.Add((Control) this.checkBoxShowConvert);
    this.panelOptions.Dock = DockStyle.Fill;
    this.panelOptions.Location = new Point(3, 3);
    this.panelOptions.Name = "panelOptions";
    this.panelOptions.Size = new Size(567, 57);
    this.panelOptions.TabIndex = 14;
    this.panelOptions.Visible = false;
    this.tableLayoutPanel2.SetColumnSpan((Control) this.buttonCancel, 2);
    this.buttonCancel.Enabled = false;
    this.buttonCancel.Location = new Point(275, 373);
    this.buttonCancel.Name = "buttonCancel";
    this.buttonCancel.Size = new Size(74, 23);
    this.buttonCancel.TabIndex = 0;
    this.buttonCancel.Text = "Отменить";
    this.buttonSelectAT.Enabled = false;
    this.buttonSelectAT.Location = new Point(325, 3);
    this.buttonSelectAT.Name = "buttonSelectAT";
    this.buttonSelectAT.Size = new Size(24, 20);
    this.buttonSelectAT.TabIndex = 3;
    this.buttonSelectAT.Text = "...";
    this.buttonSelectAT.UseVisualStyleBackColor = true;
    this.buttonApply.Enabled = false;
    this.buttonApply.Location = new Point(195, 373);
    this.buttonApply.Name = "buttonApply";
    this.buttonApply.Size = new Size(74, 23);
    this.buttonApply.TabIndex = 1;
    this.buttonApply.Text = "Применить";
    this.tableLayoutPanel2.ColumnCount = 4;
    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel2.Controls.Add((Control) this.buttonApply, 1, 2);
    this.tableLayoutPanel2.Controls.Add((Control) this.buttonSelectAT, 3, 0);
    this.tableLayoutPanel2.Controls.Add((Control) this.buttonCancel, 2, 2);
    this.tableLayoutPanel2.Controls.Add((Control) this.propertyGridAT, 0, 1);
    this.tableLayoutPanel2.Controls.Add((Control) this.textBoxAT, 0, 0);
    this.tableLayoutPanel2.Dock = DockStyle.Fill;
    this.tableLayoutPanel2.Location = new Point(0, 0);
    this.tableLayoutPanel2.Name = "tableLayoutPanel2";
    this.tableLayoutPanel2.RowCount = 3;
    this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel2.Size = new Size(352, 399);
    this.tableLayoutPanel2.TabIndex = 3;
    this.textBoxAT.BackColor = SystemColors.Window;
    this.tableLayoutPanel2.SetColumnSpan((Control) this.textBoxAT, 3);
    this.textBoxAT.Dock = DockStyle.Fill;
    this.textBoxAT.Location = new Point(3, 3);
    this.textBoxAT.Name = "textBoxAT";
    this.textBoxAT.ReadOnly = true;
    this.textBoxAT.Size = new Size(316, 20);
    this.textBoxAT.TabIndex = 3;
    this.tableLayoutPanel2.SetColumnSpan((Control) this.propertyGridAT, 4);
    this.propertyGridAT.Dock = DockStyle.Fill;
    this.propertyGridAT.Location = new Point(3, 29);
    this.propertyGridAT.Name = "propertyGridAT";
    this.propertyGridAT.Size = new Size(346, 338);
    this.propertyGridAT.TabIndex = 2;
    this.label1.Location = new Point(239, 7);
    this.label1.Name = "label1";
    this.label1.Size = new Size(83, 20);
    this.label1.TabIndex = 2;
    this.label1.Text = "label1";
    this.checkBoxShowCuted.Checked = true;
    this.checkBoxShowCuted.CheckState = CheckState.Checked;
    this.checkBoxShowCuted.Location = new Point(20, 40);
    this.checkBoxShowCuted.Name = "checkBoxShowCuted";
    this.checkBoxShowCuted.Size = new Size(196, 18);
    this.checkBoxShowCuted.TabIndex = 12;
    this.checkBoxShowCuted.Text = "Показать усеченные";
    this.checkBoxShowOnlyError.Checked = true;
    this.checkBoxShowOnlyError.CheckState = CheckState.Checked;
    this.checkBoxShowOnlyError.Location = new Point(8, 4);
    this.checkBoxShowOnlyError.Name = "checkBoxShowOnlyError";
    this.checkBoxShowOnlyError.Size = new Size(208 /*0xD0*/, 20);
    this.checkBoxShowOnlyError.TabIndex = 10;
    this.checkBoxShowOnlyError.Text = "Показать только проблемные";
    this.checkBoxShowConvert.Checked = true;
    this.checkBoxShowConvert.CheckState = CheckState.Checked;
    this.checkBoxShowConvert.Location = new Point(20, 20);
    this.checkBoxShowConvert.Name = "checkBoxShowConvert";
    this.checkBoxShowConvert.Size = new Size(196, 23);
    this.checkBoxShowConvert.TabIndex = 13;
    this.checkBoxShowConvert.Text = "Показать конвертируемые";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (ThematicParamsSettingsControl);
    this.Size = new Size(573, 468);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.ResumeLayout(false);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.panelOptions.ResumeLayout(false);
    this.tableLayoutPanel2.ResumeLayout(false);
    this.tableLayoutPanel2.PerformLayout();
    this.ResumeLayout(false);
  }

  internal delegate void AddNodeHandler(TreeNode node, TreeNode parentNode);
}
