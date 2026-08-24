// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.SortingAttributesControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal class SortingAttributesControl : TabSettingsControl
{
  private IContainer components;
  private SplitContainer splitContainer3;
  private TreeView twParentTypes;
  private Panel panel3;
  private Button bDelete;
  private Button bAdd;
  private GroupBox groupBox1;
  private GroupBox groupBox2;
  private ListView lbAttributes;

  public SortingAttributesControl()
  {
    this.InitializeComponent();
    this.twParentTypes.ImageList = this.iconService.ImageList;
    this.lbAttributes.SmallImageList = this.iconService.ImageList;
  }

  public override void RefreshData()
  {
    this.twParentTypes.Nodes.Clear();
    this.lbAttributes.Items.Clear();
    foreach (Tuple<int, List<Tuple<int, AttributeSourceTypes>>> sortedAttribute in this.Settings.SortedAttributes)
      this.CreateObjectTypeNode(sortedAttribute.Item1, (object) sortedAttribute, this.twParentTypes.Nodes);
    if (this.twParentTypes.Nodes.Count > 0)
      this.twParentTypes.SelectedNode = this.twParentTypes.Nodes[0];
    this.RefreshButtons();
  }

  private void bAdd_Click(object sender, EventArgs e)
  {
    if (this.twParentTypes.SelectedNode == null || !(this.twParentTypes.SelectedNode.Tag is Tuple<int, List<Tuple<int, AttributeSourceTypes>>> tag))
      return;
    using (SelectAttributeAndSourceForm attributeAndSourceForm = new SelectAttributeAndSourceForm())
    {
      if (attributeAndSourceForm.ShowDialog() != DialogResult.OK)
        return;
      bool flag = false;
      foreach (Tuple<int, AttributeSourceTypes> selectedAttribute in attributeAndSourceForm.SelectedAttributes)
      {
        if (this.Settings.AddSortedAttribute(tag.Item1, selectedAttribute.Item1, selectedAttribute.Item2))
        {
          this.lbAttributes.Items.Add(this.CreateItem(selectedAttribute.Item1, selectedAttribute.Item2));
          flag = true;
        }
      }
      if (!flag)
        return;
      this.DataChanged();
      this.RefreshButtons();
    }
  }

  private ListViewItem CreateItem(int attributeID, AttributeSourceTypes sourceType)
  {
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attributeID);
    string attributeTypeName = MetaDataHelper.GetAttributeTypeName(attributeID);
    if (sourceType == AttributeSourceTypes.Relation)
      attributeTypeName += " (связь)";
    return new ListViewItem(attributeTypeName)
    {
      Tag = (object) new MetadataListNode(attributeID, string.Empty, (object) sourceType),
      ImageIndex = this.iconService.IndexOf(3, -1, (object) attributeType.FieldType)
    };
  }

  private void RefreshButtons()
  {
    this.bAdd.Enabled = this.twParentTypes.SelectedNode != null;
    this.bDelete.Enabled = this.lbAttributes.SelectedItems != null && this.lbAttributes.SelectedItems.Count > 0;
  }

  private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this.lbAttributes.Items.Clear();
    if (!(e.Node.Tag is Tuple<int, List<Tuple<int, AttributeSourceTypes>>> tag))
      return;
    foreach (Tuple<int, AttributeSourceTypes> tuple in tag.Item2)
      this.lbAttributes.Items.Add(this.CreateItem(tuple.Item1, tuple.Item2));
    this.RefreshButtons();
  }

  private void bDelete_Click(object sender, EventArgs e)
  {
    if (this.twParentTypes.SelectedNode == null || !(this.twParentTypes.SelectedNode.Tag is Tuple<int, List<Tuple<int, AttributeSourceTypes>>> tag1) || this.lbAttributes.SelectedItems == null)
      return;
    MetadataListNode tag2 = (MetadataListNode) this.lbAttributes.SelectedItems[0].Tag;
    this.Settings.RemoveSortedAttribute(tag1.Item1, tag2.ID, (AttributeSourceTypes) tag2.Tag);
    this.lbAttributes.Items.Remove(this.lbAttributes.SelectedItems[0]);
    this.DataChanged();
  }

  private void lbAttributes_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.RefreshButtons();
  }

  public override string Caption => "Атрибуты сортировки";

  public override string ToolTipText => "Атрибуты сортировки позиций на уровне состава";

  public override int Index => 20;

  public override Guid ID => new Guid("AEEAAA26-D7DA-4B60-AF96-B3B2542C2230");

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    TreeNode treeNode1 = new TreeNode("Детали");
    TreeNode treeNode2 = new TreeNode("Документы");
    TreeNode treeNode3 = new TreeNode("Сборочные единицы");
    this.splitContainer3 = new SplitContainer();
    this.groupBox1 = new GroupBox();
    this.twParentTypes = new TreeView();
    this.groupBox2 = new GroupBox();
    this.lbAttributes = new ListView();
    this.panel3 = new Panel();
    this.bDelete = new Button();
    this.bAdd = new Button();
    this.splitContainer3.BeginInit();
    this.splitContainer3.Panel1.SuspendLayout();
    this.splitContainer3.Panel2.SuspendLayout();
    this.splitContainer3.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.panel3.SuspendLayout();
    this.SuspendLayout();
    this.splitContainer3.Dock = DockStyle.Fill;
    this.splitContainer3.Location = new Point(0, 0);
    this.splitContainer3.Name = "splitContainer3";
    this.splitContainer3.Panel1.Controls.Add((Control) this.groupBox1);
    this.splitContainer3.Panel2.Controls.Add((Control) this.groupBox2);
    this.splitContainer3.Panel2.Controls.Add((Control) this.panel3);
    this.splitContainer3.Size = new Size(656, 277);
    this.splitContainer3.SplitterDistance = 323;
    this.splitContainer3.TabIndex = 2;
    this.groupBox1.Controls.Add((Control) this.twParentTypes);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(323, 277);
    this.groupBox1.TabIndex = 1;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Родительские типы объектов";
    this.twParentTypes.Dock = DockStyle.Fill;
    this.twParentTypes.HideSelection = false;
    this.twParentTypes.Location = new Point(3, 16 /*0x10*/);
    this.twParentTypes.Name = "twParentTypes";
    treeNode1.Name = "Node6";
    treeNode1.Text = "Детали";
    treeNode2.Name = "Node8";
    treeNode2.Text = "Документы";
    treeNode3.Name = "Node5";
    treeNode3.Text = "Сборочные единицы";
    this.twParentTypes.Nodes.AddRange(new TreeNode[3]
    {
      treeNode1,
      treeNode2,
      treeNode3
    });
    this.twParentTypes.Size = new Size(317, 258);
    this.twParentTypes.TabIndex = 0;
    this.twParentTypes.AfterSelect += new TreeViewEventHandler(this.treeView2_AfterSelect);
    this.groupBox2.Controls.Add((Control) this.lbAttributes);
    this.groupBox2.Dock = DockStyle.Fill;
    this.groupBox2.Location = new Point(0, 0);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(329, 239);
    this.groupBox2.TabIndex = 2;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Атрибуты для сортировки на уровне состава";
    this.lbAttributes.Activation = ItemActivation.OneClick;
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
    this.panel3.Controls.Add((Control) this.bDelete);
    this.panel3.Controls.Add((Control) this.bAdd);
    this.panel3.Dock = DockStyle.Bottom;
    this.panel3.Location = new Point(0, 239);
    this.panel3.Name = "panel3";
    this.panel3.Size = new Size(329, 38);
    this.panel3.TabIndex = 0;
    this.bDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
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
    this.Controls.Add((Control) this.splitContainer3);
    this.Name = nameof (SortingAttributesControl);
    this.Size = new Size(656, 277);
    this.splitContainer3.Panel1.ResumeLayout(false);
    this.splitContainer3.Panel2.ResumeLayout(false);
    this.splitContainer3.EndInit();
    this.splitContainer3.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
