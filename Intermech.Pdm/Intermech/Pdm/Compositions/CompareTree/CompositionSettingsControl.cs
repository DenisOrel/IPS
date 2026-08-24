// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompositionSettingsControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal class CompositionSettingsControl : TabSettingsControl
{
  public static Guid GUID = new Guid("056AA4A1-713C-4870-A0BB-2FC28BF88983");
  private bool _selfCheck;
  private bool _initialized;
  private bool _selfSelectAllCheck;
  private IContainer components;
  private SplitContainer splitContainer1;
  private GroupBox groupBox2;
  private TreeView twComposition;
  private Panel panel1;
  private Button bRemove;
  private Button bAdd;
  private GroupBox groupBox1;
  private CheckedListBox lbObjectTypes;
  private Panel panel3;
  private Panel panel2;
  private CheckBox cbSelectAll;

  public CompositionSettingsControl()
  {
    this.InitializeComponent();
    this.twComposition.ImageList = this.iconService.ImageList;
  }

  public override void RefreshData()
  {
    this._selfCheck = true;
    this.twComposition.Nodes.Clear();
    this.lbObjectTypes.Items.Clear();
    List<Tuple<int, TreeNode>> tupleList = new List<Tuple<int, TreeNode>>();
    TreeNode treeNode = (TreeNode) null;
    foreach (Tuple<int, int, List<int>> childType in this.Settings.ChildTypes)
    {
      Tuple<int, int, List<int>> pair = childType;
      Tuple<int, TreeNode> tuple = tupleList.Find((Predicate<Tuple<int, TreeNode>>) (x => x.Item1 == pair.Item1));
      TreeNode objectTypeNode;
      if (tuple == null)
      {
        objectTypeNode = this.CreateObjectTypeNode(pair.Item1, (object) pair.Item1, this.twComposition.Nodes);
        tupleList.Add(new Tuple<int, TreeNode>(pair.Item1, objectTypeNode));
      }
      else
        objectTypeNode = tuple.Item2;
      TreeNode relationTypeNode = this.CreateRelationTypeNode(pair.Item2, (object) pair.Item2, objectTypeNode.Nodes);
      if (treeNode == null)
        treeNode = relationTypeNode;
    }
    if (treeNode != null)
      this.twComposition.SelectedNode = treeNode;
    this.RefreshButtons();
    this._initialized = true;
  }

  private void RefreshButtons()
  {
    this.bRemove.Enabled = this.twComposition.SelectedNode != null && this.twComposition.SelectedNode.Parent == null;
  }

  private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
  {
    if (this._selfCheck)
      return;
    this._selfSelectAllCheck = true;
    try
    {
      if (this.lbObjectTypes.Items.Count <= 0)
        return;
      for (int index = 0; index < this.lbObjectTypes.Items.Count; ++index)
        this.lbObjectTypes.SetItemChecked(index, this.cbSelectAll.Checked);
      this.DataChanged();
    }
    finally
    {
      this._selfSelectAllCheck = false;
    }
  }

  private void twComposition_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this.lbObjectTypes.Items.Clear();
    this.RefreshButtons();
    if (e.Node.Parent == null || e.Node.Parent.Tag == null || e.Node.Tag == null)
      return;
    List<int> childObjectTypesId = MetaDataHelper.GetApplicabilityChildObjectTypesID((int) e.Node.Parent.Tag, (int) e.Node.Tag);
    if (childObjectTypesId == null || childObjectTypesId.Count == 0)
      return;
    Tuple<int, int, List<int>> tuple = this.Settings.ChildTypes.Find((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == (int) e.Node.Parent.Tag && x.Item2 == (int) e.Node.Tag));
    bool flag = false;
    this._selfCheck = true;
    try
    {
      foreach (int num in childObjectTypesId)
      {
        bool isChecked = tuple != null && tuple.Item3.Contains(num);
        this.lbObjectTypes.Items.Add((object) new MetadataListNode(num, MetaDataHelper.GetObjectTypeName(num)), isChecked);
        if (!isChecked)
          flag = true;
      }
      this.cbSelectAll.Checked = !flag;
    }
    finally
    {
      this._selfCheck = false;
    }
  }

  private void lbObjectTypes_ItemCheck(object sender, ItemCheckEventArgs e)
  {
    if (this._selfCheck)
      return;
    TreeNode node = this.twComposition.SelectedNode;
    if (node.Parent == null || node.Parent.Tag == null || node.Tag == null)
      return;
    Tuple<int, int, List<int>> tuple = this.Settings.ChildTypes.Find((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == (int) node.Parent.Tag && x.Item2 == (int) node.Tag));
    if (tuple == null)
    {
      tuple = new Tuple<int, int, List<int>>((int) node.Parent.Tag, (int) node.Tag, new List<int>());
      this.Settings.AddApplicability(tuple);
    }
    int id = ((MetadataListNode) this.lbObjectTypes.Items[e.Index]).ID;
    if (e.NewValue == CheckState.Checked)
      this.Settings.AddChildType(tuple.Item1, tuple.Item2, id);
    else
      this.Settings.RemoveChildType(tuple.Item1, tuple.Item2, id);
    if (!this._selfSelectAllCheck)
    {
      this._selfCheck = true;
      try
      {
        this.cbSelectAll.Checked = this.lbObjectTypes.CheckedItems.Count + (e.NewValue == CheckState.Checked ? 1 : -1) == this.lbObjectTypes.Items.Count;
      }
      finally
      {
        this._selfCheck = false;
      }
    }
    this.DataChanged();
  }

  protected override void DataChanged()
  {
    if (!this._initialized)
      return;
    base.DataChanged();
  }

  private void bAdd_Click(object sender, EventArgs e)
  {
    SelectorForm selectorForm = new SelectorForm(typeof (ObjectTypesFolder), "Выберите тип объекта", typeof (ObjectTypeFolder), false);
    if (selectorForm.ShowDialog() != DialogResult.OK || selectorForm.IDList.Count <= 0)
      return;
    bool flag = false;
    foreach (int id in selectorForm.IDList)
    {
      int typeID = id;
      if (!this.Settings.ChildTypes.Exists((Predicate<Tuple<int, int, List<int>>>) (x => x.Item1 == typeID)))
      {
        TreeNode objectTypeNode = this.CreateObjectTypeNode(typeID, (object) typeID, this.twComposition.Nodes);
        List<int> applicabilityRelationTypesId = MetaDataHelper.GetApplicabilityRelationTypesID(typeID);
        if (applicabilityRelationTypesId != null && applicabilityRelationTypesId.Count > 0)
        {
          TreeNode treeNode = (TreeNode) null;
          foreach (int num in applicabilityRelationTypesId)
          {
            TreeNode relationTypeNode = this.CreateRelationTypeNode(num, (object) num, objectTypeNode.Nodes);
            if (treeNode == null)
              treeNode = relationTypeNode;
            this.Settings.AddApplicability(new Tuple<int, int, List<int>>(typeID, num, new List<int>()));
          }
          flag = true;
          if (treeNode != null)
            this.twComposition.SelectedNode = treeNode;
        }
      }
    }
    if (!flag)
      return;
    this.DataChanged();
    this.RefreshButtons();
  }

  private void bRemove_Click(object sender, EventArgs e)
  {
    if (this.twComposition.SelectedNode.Parent != null)
      return;
    int tag = (int) this.twComposition.SelectedNode.Tag;
    if (MessageBox.Show($"Удалить настройки поиска состава для типа \"{MetaDataHelper.GetObjectTypeName(tag)}\"?", "Удаление настройки", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
      return;
    this.Settings.RemoveRootType(tag);
    this.twComposition.Nodes.Remove(this.twComposition.SelectedNode);
    bool flag = false;
    if (this.twComposition.Nodes.Count > 0)
    {
      foreach (TreeNode node in this.twComposition.Nodes)
      {
        if (node.Nodes.Count > 0)
        {
          this.twComposition.SelectedNode = node;
          flag = true;
          break;
        }
      }
      if (!flag)
        this.twComposition.SelectedNode = this.twComposition.Nodes[0];
    }
    this.DataChanged();
    this.RefreshButtons();
  }

  public override void AnotherTabDataChanged(TabDataChangedEventArgs e)
  {
  }

  public override string Caption => "Поиск состава";

  public override string ToolTipText => "Настройки поиска состава сравниваемых объектов";

  public override int Index => 10;

  public override Guid ID => CompositionSettingsControl.GUID;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    TreeNode treeNode1 = new TreeNode("Состав изделий");
    TreeNode treeNode2 = new TreeNode("Документация на изделие");
    TreeNode treeNode3 = new TreeNode("Сборочные единицы", new TreeNode[2]
    {
      treeNode1,
      treeNode2
    });
    this.splitContainer1 = new SplitContainer();
    this.groupBox2 = new GroupBox();
    this.twComposition = new TreeView();
    this.panel1 = new Panel();
    this.bRemove = new Button();
    this.bAdd = new Button();
    this.groupBox1 = new GroupBox();
    this.panel3 = new Panel();
    this.lbObjectTypes = new CheckedListBox();
    this.panel2 = new Panel();
    this.cbSelectAll = new CheckBox();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.panel1.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.panel3.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.Location = new Point(0, 0);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.groupBox2);
    this.splitContainer1.Panel1.Controls.Add((Control) this.panel1);
    this.splitContainer1.Panel2.Controls.Add((Control) this.groupBox1);
    this.splitContainer1.Size = new Size(597, 304);
    this.splitContainer1.SplitterDistance = 310;
    this.splitContainer1.TabIndex = 3;
    this.groupBox2.Controls.Add((Control) this.twComposition);
    this.groupBox2.Dock = DockStyle.Fill;
    this.groupBox2.Location = new Point(0, 0);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(310, 266);
    this.groupBox2.TabIndex = 2;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Родительские типы объектов и типы связей для поиска";
    this.twComposition.Dock = DockStyle.Fill;
    this.twComposition.HideSelection = false;
    this.twComposition.Location = new Point(3, 16 /*0x10*/);
    this.twComposition.Name = "twComposition";
    treeNode1.Name = "Node1";
    treeNode1.Text = "Состав изделий";
    treeNode2.Name = "Node3";
    treeNode2.Text = "Документация на изделие";
    treeNode3.Name = "Node0";
    treeNode3.Text = "Сборочные единицы";
    this.twComposition.Nodes.AddRange(new TreeNode[1]
    {
      treeNode3
    });
    this.twComposition.Size = new Size(304, 247);
    this.twComposition.TabIndex = 0;
    this.twComposition.AfterSelect += new TreeViewEventHandler(this.twComposition_AfterSelect);
    this.panel1.Controls.Add((Control) this.bRemove);
    this.panel1.Controls.Add((Control) this.bAdd);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 266);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(310, 38);
    this.panel1.TabIndex = 1;
    this.bRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bRemove.Enabled = false;
    this.bRemove.Location = new Point(173, 5);
    this.bRemove.Name = "bRemove";
    this.bRemove.Size = new Size(121, 27);
    this.bRemove.TabIndex = 1;
    this.bRemove.Text = "Удалить";
    this.bRemove.UseVisualStyleBackColor = true;
    this.bRemove.Click += new EventHandler(this.bRemove_Click);
    this.bAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bAdd.Location = new Point(46, 5);
    this.bAdd.Name = "bAdd";
    this.bAdd.Size = new Size(121, 27);
    this.bAdd.TabIndex = 0;
    this.bAdd.Text = "Добавить";
    this.bAdd.UseVisualStyleBackColor = true;
    this.bAdd.Click += new EventHandler(this.bAdd_Click);
    this.groupBox1.Controls.Add((Control) this.panel3);
    this.groupBox1.Controls.Add((Control) this.panel2);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(283, 304);
    this.groupBox1.TabIndex = 1;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Типы объектов в составе";
    this.panel3.Controls.Add((Control) this.lbObjectTypes);
    this.panel3.Dock = DockStyle.Fill;
    this.panel3.Location = new Point(3, 16 /*0x10*/);
    this.panel3.Name = "panel3";
    this.panel3.Size = new Size(277, 250);
    this.panel3.TabIndex = 2;
    this.lbObjectTypes.CheckOnClick = true;
    this.lbObjectTypes.Dock = DockStyle.Fill;
    this.lbObjectTypes.FormattingEnabled = true;
    this.lbObjectTypes.Items.AddRange(new object[3]
    {
      (object) "Детали",
      (object) "Прочие изделия",
      (object) "Стандартные изделия"
    });
    this.lbObjectTypes.Location = new Point(0, 0);
    this.lbObjectTypes.Name = "lbObjectTypes";
    this.lbObjectTypes.Size = new Size(277, 250);
    this.lbObjectTypes.TabIndex = 0;
    this.lbObjectTypes.ItemCheck += new ItemCheckEventHandler(this.lbObjectTypes_ItemCheck);
    this.panel2.Controls.Add((Control) this.cbSelectAll);
    this.panel2.Dock = DockStyle.Bottom;
    this.panel2.Location = new Point(3, 266);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(277, 35);
    this.panel2.TabIndex = 1;
    this.cbSelectAll.AutoSize = true;
    this.cbSelectAll.Location = new Point(3, 11);
    this.cbSelectAll.Name = "cbSelectAll";
    this.cbSelectAll.Size = new Size(96 /*0x60*/, 17);
    this.cbSelectAll.TabIndex = 0;
    this.cbSelectAll.Text = "Отметить все";
    this.cbSelectAll.UseVisualStyleBackColor = true;
    this.cbSelectAll.CheckedChanged += new EventHandler(this.cbSelectAll_CheckedChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.splitContainer1);
    this.Name = nameof (CompositionSettingsControl);
    this.Size = new Size(597, 304);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    this.ResumeLayout(false);
  }
}
