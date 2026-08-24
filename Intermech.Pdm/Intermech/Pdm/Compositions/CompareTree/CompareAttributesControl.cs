// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareAttributesControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal class CompareAttributesControl : TabSettingsControl
{
  private bool _selfChecked;
  private IContainer components;
  private SplitContainer splitContainer4;
  private TreeView twTypes;
  private Panel panel4;
  private Button bDelete;
  private Button bAdd;
  private GroupBox groupBox1;
  private GroupBox groupBox2;
  private ListView lbAttributes;
  private CheckBox cbCheckAttributes;

  public CompareAttributesControl()
  {
    this.InitializeComponent();
    this.twTypes.ImageList = this.iconService.ImageList;
    this.lbAttributes.SmallImageList = this.iconService.ImageList;
  }

  public override void RefreshData()
  {
    this.lbAttributes.Items.Clear();
    foreach (TreeNode node in this.twTypes.Nodes)
      CompareAttributesNode.RefreshNode(node, this.Settings, this.iconService);
    if (this.twTypes.Nodes.Count > 0)
      this.twTypes.SelectedNode = this.twTypes.Nodes[0];
    this._selfChecked = true;
    try
    {
      this.cbCheckAttributes.Checked = this.Settings.CheckExistsAttributes;
    }
    finally
    {
      this._selfChecked = false;
    }
    this.RefreshButtons();
  }

  private void RefreshButtons()
  {
    this.bAdd.Enabled = this.twTypes.SelectedNode != null && this.twTypes.SelectedNode.Tag is Tuple<int, List<int>>;
    this.bDelete.Enabled = this.lbAttributes.SelectedItems != null && this.lbAttributes.SelectedItems.Count > 0;
  }

  private void treeView3_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this.lbAttributes.Items.Clear();
    if (e.Node.Tag != null && e.Node.Tag is Tuple<int, List<int>>)
    {
      foreach (int attributeID in ((Tuple<int, List<int>>) e.Node.Tag).Item2)
        this.lbAttributes.Items.Add(ControlsHelper.CreateAttributeListViewItem(attributeID, this.iconService));
    }
    this.RefreshButtons();
  }

  private void lbAttributes_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.RefreshButtons();
  }

  private void bAdd_Click(object sender, EventArgs e)
  {
    if (this.twTypes.SelectedNode == null || !(this.twTypes.SelectedNode.Tag is Tuple<int, List<int>> tag))
      return;
    RootNodeTypes int32 = (RootNodeTypes) Convert.ToInt32(this.twTypes.SelectedNode.Parent.Tag);
    List<int> intList = CompareAttributesNode.OpenAttributeDialog(int32, tag.Item1, this.iconService);
    if (intList == null)
      return;
    bool flag = false;
    foreach (int attributeID in intList)
    {
      if (!tag.Item2.Contains(attributeID))
      {
        switch (int32)
        {
          case RootNodeTypes.ObjectTypesList:
            this.Settings.AddObjectCompareAttribute(tag.Item1, attributeID);
            break;
          case RootNodeTypes.RelationTypesList:
            this.Settings.AddRelationCompareAttribute(tag.Item1, attributeID);
            break;
        }
        flag = true;
        this.lbAttributes.Items.Add(ControlsHelper.CreateAttributeListViewItem(attributeID, this.iconService));
      }
    }
    if (!flag)
      return;
    this.DataChanged();
    this.RefreshButtons();
  }

  private void bDelete_Click(object sender, EventArgs e)
  {
    if (this.lbAttributes.SelectedItems == null || this.lbAttributes.SelectedItems.Count <= 0 || !(this.lbAttributes.SelectedItems[0].Tag is MetadataListNode tag1) || this.twTypes.SelectedNode == null || !(this.twTypes.SelectedNode.Tag is Tuple<int, List<int>> tag2))
      return;
    switch ((RootNodeTypes) Convert.ToInt32(this.twTypes.SelectedNode.Parent.Tag))
    {
      case RootNodeTypes.ObjectTypesList:
        this.Settings.RemoveObjectCompareAttribute(tag2.Item1, tag1.ID);
        break;
      case RootNodeTypes.RelationTypesList:
        this.Settings.RemoveRelationCompareAttribute(tag2.Item1, tag1.ID);
        break;
    }
    this.lbAttributes.Items.Remove(this.lbAttributes.SelectedItems[0]);
    this.DataChanged();
    this.RefreshButtons();
  }

  private void cbCheckAttributes_CheckedChanged(object sender, EventArgs e)
  {
    if (this._selfChecked)
      return;
    this.Settings.CheckExistsAttributes = this.cbCheckAttributes.Checked;
    this.DataChanged();
    this.RefreshButtons();
  }

  public override string Caption => "Сравниваемые атрибуты";

  public override string ToolTipText => "Сравниваемые атрибуты сравниваемых объектов";

  public override int Index => 40;

  public override Guid ID => new Guid("A3AAAD56-6303-419B-8333-DFF25E6E1750");

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    TreeNode treeNode1 = new TreeNode("Сборочные единицы");
    TreeNode treeNode2 = new TreeNode("Детали");
    TreeNode treeNode3 = new TreeNode("Документы");
    TreeNode treeNode4 = new TreeNode("Типы объектов", new TreeNode[3]
    {
      treeNode1,
      treeNode2,
      treeNode3
    });
    TreeNode treeNode5 = new TreeNode("Состав изделия");
    TreeNode treeNode6 = new TreeNode("Документация на изделие");
    TreeNode treeNode7 = new TreeNode("Типы связей", new TreeNode[2]
    {
      treeNode5,
      treeNode6
    });
    this.splitContainer4 = new SplitContainer();
    this.groupBox1 = new GroupBox();
    this.twTypes = new TreeView();
    this.groupBox2 = new GroupBox();
    this.lbAttributes = new ListView();
    this.panel4 = new Panel();
    this.bDelete = new Button();
    this.bAdd = new Button();
    this.cbCheckAttributes = new CheckBox();
    this.splitContainer4.BeginInit();
    this.splitContainer4.Panel1.SuspendLayout();
    this.splitContainer4.Panel2.SuspendLayout();
    this.splitContainer4.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.panel4.SuspendLayout();
    this.SuspendLayout();
    this.splitContainer4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.splitContainer4.Location = new Point(0, 0);
    this.splitContainer4.Name = "splitContainer4";
    this.splitContainer4.Panel1.Controls.Add((Control) this.groupBox1);
    this.splitContainer4.Panel2.Controls.Add((Control) this.groupBox2);
    this.splitContainer4.Panel2.Controls.Add((Control) this.panel4);
    this.splitContainer4.Size = new Size(598, 268);
    this.splitContainer4.SplitterDistance = 293;
    this.splitContainer4.TabIndex = 2;
    this.groupBox1.Controls.Add((Control) this.twTypes);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(293, 268);
    this.groupBox1.TabIndex = 1;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Типы объектов и связей";
    this.twTypes.Dock = DockStyle.Fill;
    this.twTypes.HideSelection = false;
    this.twTypes.Location = new Point(3, 16 /*0x10*/);
    this.twTypes.Name = "twTypes";
    treeNode1.Name = "Node5";
    treeNode1.Text = "Сборочные единицы";
    treeNode2.Name = "Node6";
    treeNode2.Text = "Детали";
    treeNode3.Name = "Node8";
    treeNode3.Text = "Документы";
    treeNode4.Name = "nodeObjectAttributes";
    treeNode4.Tag = (object) "0";
    treeNode4.Text = "Типы объектов";
    treeNode5.Name = "Node3";
    treeNode5.Text = "Состав изделия";
    treeNode6.Name = "Node4";
    treeNode6.Text = "Документация на изделие";
    treeNode7.Name = "nodeRelationAttributes";
    treeNode7.Tag = (object) "1";
    treeNode7.Text = "Типы связей";
    this.twTypes.Nodes.AddRange(new TreeNode[2]
    {
      treeNode4,
      treeNode7
    });
    this.twTypes.Size = new Size(287, 249);
    this.twTypes.TabIndex = 0;
    this.twTypes.AfterSelect += new TreeViewEventHandler(this.treeView3_AfterSelect);
    this.groupBox2.Controls.Add((Control) this.lbAttributes);
    this.groupBox2.Dock = DockStyle.Fill;
    this.groupBox2.Location = new Point(0, 0);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(301, 230);
    this.groupBox2.TabIndex = 2;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Атрибуты для сравнения";
    this.lbAttributes.Dock = DockStyle.Fill;
    this.lbAttributes.HideSelection = false;
    this.lbAttributes.Location = new Point(3, 16 /*0x10*/);
    this.lbAttributes.MultiSelect = false;
    this.lbAttributes.Name = "lbAttributes";
    this.lbAttributes.Size = new Size(295, 211);
    this.lbAttributes.TabIndex = 2;
    this.lbAttributes.UseCompatibleStateImageBehavior = false;
    this.lbAttributes.View = View.List;
    this.lbAttributes.SelectedIndexChanged += new EventHandler(this.lbAttributes_SelectedIndexChanged);
    this.panel4.Controls.Add((Control) this.bDelete);
    this.panel4.Controls.Add((Control) this.bAdd);
    this.panel4.Dock = DockStyle.Bottom;
    this.panel4.Location = new Point(0, 230);
    this.panel4.Name = "panel4";
    this.panel4.Size = new Size(301, 38);
    this.panel4.TabIndex = 0;
    this.bDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bDelete.Location = new Point(166, 5);
    this.bDelete.Name = "bDelete";
    this.bDelete.Size = new Size(121, 27);
    this.bDelete.TabIndex = 3;
    this.bDelete.Text = "Удалить";
    this.bDelete.UseVisualStyleBackColor = true;
    this.bDelete.Click += new EventHandler(this.bDelete_Click);
    this.bAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bAdd.Location = new Point(39, 5);
    this.bAdd.Name = "bAdd";
    this.bAdd.Size = new Size(121, 27);
    this.bAdd.TabIndex = 2;
    this.bAdd.Text = "Добавить";
    this.bAdd.UseVisualStyleBackColor = true;
    this.bAdd.Click += new EventHandler(this.bAdd_Click);
    this.cbCheckAttributes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.cbCheckAttributes.Location = new Point(6, 268);
    this.cbCheckAttributes.Name = "cbCheckAttributes";
    this.cbCheckAttributes.Size = new Size(444, 33);
    this.cbCheckAttributes.TabIndex = 3;
    this.cbCheckAttributes.Text = "При сравнении атрибутов определять их наличие у объекта/связи. \r\nВнимание! Включение этой опции значительно замедляет процесс сравнения. ";
    this.cbCheckAttributes.UseVisualStyleBackColor = true;
    this.cbCheckAttributes.CheckedChanged += new EventHandler(this.cbCheckAttributes_CheckedChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.cbCheckAttributes);
    this.Controls.Add((Control) this.splitContainer4);
    this.Name = nameof (CompareAttributesControl);
    this.Size = new Size(598, 307);
    this.splitContainer4.Panel1.ResumeLayout(false);
    this.splitContainer4.Panel2.ResumeLayout(false);
    this.splitContainer4.EndInit();
    this.splitContainer4.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.panel4.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
