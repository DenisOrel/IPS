// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.IDAttributesControl
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

internal class IDAttributesControl : TabSettingsControl
{
  private IDAttributesNodes _idAttributesNodes;
  private IContainer components;
  private SplitContainer splitContainer2;
  private TreeView twTypes;
  private Panel panel2;
  private Button bDelete;
  private Button bAdd;
  private GroupBox groupBox1;
  private GroupBox groupBox2;
  private ListView lbAttributes;

  public IDAttributesControl()
  {
    this.InitializeComponent();
    this._idAttributesNodes = new IDAttributesNodes(this.iconService);
    this.twTypes.ImageList = this.iconService.ImageList;
    this.lbAttributes.SmallImageList = this.iconService.ImageList;
  }

  public override void RefreshData()
  {
    this.lbAttributes.Items.Clear();
    foreach (TreeNode node in this.twTypes.Nodes)
      this._idAttributesNodes.RefreshNode(node, this.Settings);
    if (this.twTypes.Nodes.Count > 0)
      this.twTypes.SelectedNode = this.twTypes.Nodes[0];
    this.RefreshButtons();
  }

  private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this.lbAttributes.Items.Clear();
    List<int> attributesForNode = this._idAttributesNodes.GetAttributesForNode(e.Node);
    if (attributesForNode != null)
    {
      foreach (int attributeID in attributesForNode)
        this.lbAttributes.Items.Add(ControlsHelper.CreateAttributeListViewItem(attributeID, this.iconService));
    }
    this.RefreshButtons();
  }

  private void bAdd_Click(object sender, EventArgs e)
  {
    List<int> intList = this._idAttributesNodes.AddAttribute(this.twTypes.SelectedNode, this.Settings);
    if (intList == null)
      return;
    foreach (int attributeID in intList)
      this.lbAttributes.Items.Add(ControlsHelper.CreateAttributeListViewItem(attributeID, this.iconService));
    this.DataChanged();
    this.RefreshButtons();
  }

  private void bDelete_Click(object sender, EventArgs e)
  {
    if (this.lbAttributes.SelectedItems.Count <= 0 || !(this.lbAttributes.SelectedItems[0].Tag is MetadataListNode tag) || !this._idAttributesNodes.RemoveAttribute(this.twTypes.SelectedNode, tag.ID, this.Settings))
      return;
    this.lbAttributes.Items.Remove(this.lbAttributes.SelectedItems[0]);
    this.DataChanged();
    this.RefreshButtons();
  }

  private void RefreshButtons()
  {
    this.bAdd.Enabled = this.twTypes.SelectedNode != null && this._idAttributesNodes.IsSettingsNode(this.twTypes.SelectedNode);
    this.bDelete.Enabled = this.lbAttributes.SelectedItems != null && this.lbAttributes.SelectedItems.Count > 0;
  }

  private void lbAttributes_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.RefreshButtons();
  }

  public override string Caption => "Идентифицирующие атрибуты";

  public override string ToolTipText
  {
    get => "Атрибуты, для дополнительной идентификации версий в составе";
  }

  public override int Index => 30;

  public override Guid ID => new Guid("B9002DCC-4994-41DF-9AF6-BE42688C2BC9");

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
    TreeNode treeNode7 = new TreeNode("Сборочные единицы", new TreeNode[2]
    {
      treeNode5,
      treeNode6
    });
    TreeNode treeNode8 = new TreeNode("Типы связей в составе", new TreeNode[1]
    {
      treeNode7
    });
    this.splitContainer2 = new SplitContainer();
    this.groupBox1 = new GroupBox();
    this.twTypes = new TreeView();
    this.groupBox2 = new GroupBox();
    this.lbAttributes = new ListView();
    this.panel2 = new Panel();
    this.bDelete = new Button();
    this.bAdd = new Button();
    this.splitContainer2.BeginInit();
    this.splitContainer2.Panel1.SuspendLayout();
    this.splitContainer2.Panel2.SuspendLayout();
    this.splitContainer2.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.splitContainer2.Dock = DockStyle.Fill;
    this.splitContainer2.Location = new Point(0, 0);
    this.splitContainer2.Name = "splitContainer2";
    this.splitContainer2.Panel1.Controls.Add((Control) this.groupBox1);
    this.splitContainer2.Panel2.Controls.Add((Control) this.groupBox2);
    this.splitContainer2.Panel2.Controls.Add((Control) this.panel2);
    this.splitContainer2.Size = new Size(656, 277);
    this.splitContainer2.SplitterDistance = 323;
    this.splitContainer2.TabIndex = 1;
    this.groupBox1.Controls.Add((Control) this.twTypes);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(323, 277);
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
    treeNode4.Name = "nodeObjectTypes";
    treeNode4.Tag = (object) "0";
    treeNode4.Text = "Типы объектов";
    treeNode5.Name = "Node3";
    treeNode5.Text = "Состав изделия";
    treeNode6.Name = "Node4";
    treeNode6.Text = "Документация на изделие";
    treeNode7.Name = "Node2";
    treeNode7.Text = "Сборочные единицы";
    treeNode8.Name = "nodeRelationTypes";
    treeNode8.Tag = (object) "1";
    treeNode8.Text = "Типы связей в составе";
    this.twTypes.Nodes.AddRange(new TreeNode[2]
    {
      treeNode4,
      treeNode8
    });
    this.twTypes.Size = new Size(317, 258);
    this.twTypes.TabIndex = 0;
    this.twTypes.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
    this.groupBox2.Controls.Add((Control) this.lbAttributes);
    this.groupBox2.Dock = DockStyle.Fill;
    this.groupBox2.Location = new Point(0, 0);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(329, 239);
    this.groupBox2.TabIndex = 2;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Идентифицирующие атрибуты";
    this.lbAttributes.Dock = DockStyle.Fill;
    this.lbAttributes.HideSelection = false;
    this.lbAttributes.Location = new Point(3, 16 /*0x10*/);
    this.lbAttributes.MultiSelect = false;
    this.lbAttributes.Name = "lbAttributes";
    this.lbAttributes.Size = new Size(323, 220);
    this.lbAttributes.TabIndex = 2;
    this.lbAttributes.UseCompatibleStateImageBehavior = false;
    this.lbAttributes.View = View.List;
    this.lbAttributes.SelectedIndexChanged += new EventHandler(this.lbAttributes_SelectedIndexChanged);
    this.panel2.Controls.Add((Control) this.bDelete);
    this.panel2.Controls.Add((Control) this.bAdd);
    this.panel2.Dock = DockStyle.Bottom;
    this.panel2.Location = new Point(0, 239);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(329, 38);
    this.panel2.TabIndex = 0;
    this.bDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bDelete.Enabled = false;
    this.bDelete.Location = new Point(194, 5);
    this.bDelete.Name = "bDelete";
    this.bDelete.Size = new Size(121, 27);
    this.bDelete.TabIndex = 3;
    this.bDelete.Text = "Удалить";
    this.bDelete.UseVisualStyleBackColor = true;
    this.bDelete.Click += new EventHandler(this.bDelete_Click);
    this.bAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bAdd.Location = new Point(67, 5);
    this.bAdd.Name = "bAdd";
    this.bAdd.Size = new Size(121, 27);
    this.bAdd.TabIndex = 2;
    this.bAdd.Text = "Добавить";
    this.bAdd.UseVisualStyleBackColor = true;
    this.bAdd.Click += new EventHandler(this.bAdd_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.splitContainer2);
    this.Name = nameof (IDAttributesControl);
    this.Size = new Size(656, 277);
    this.splitContainer2.Panel1.ResumeLayout(false);
    this.splitContainer2.Panel2.ResumeLayout(false);
    this.splitContainer2.EndInit();
    this.splitContainer2.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
