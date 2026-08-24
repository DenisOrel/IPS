// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.ItemsToCreate.ItemToCreateSelectDialog
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData.ItemsToCreate;

public class ItemToCreateSelectDialog : Form, IItemToCreateSelectDialog
{
  private IAttributeImageList _attrImages;
  private IAttributeTypeToCreateList _attrService;
  private IAttributeGroupToCreateList _attrGrService;
  private IObjectTypeToCreateList _objService;
  private IDataWriter _idw;
  private IMetadataInfo _imdi;
  private Type itemsType;
  private static string msgCaption = "Ошибка изменения параметра \"{0}\"";
  private static string msgText = "с таким значением параметра \"{0}\" уже есть.";
  private static string msgTextAttr = "Тип атрибута " + ItemToCreateSelectDialog.msgText;
  private static string msgTextAttrGr = "Группа атрибута " + ItemToCreateSelectDialog.msgText;
  private static string msgTextObj = "Тип объекта " + ItemToCreateSelectDialog.msgText;
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private GroupBox groupBox1;
  private SplitContainer splitContainer1;
  private TreeView treeViewAT;
  private PropertyGrid propertyGrid;
  private Button buttonCancel;
  private Button buttonAccept;
  private Button buttonNew;

  public ItemToCreateSelectDialog() => this.InitializeComponent();

  private IAttributeImageList attrImages
  {
    get
    {
      if (this._attrImages == null)
        this._attrImages = ServicesManager.ServiceContainer.GetService(typeof (IAttributeImageList)) as IAttributeImageList;
      return this._attrImages;
    }
  }

  private IAttributeTypeToCreateList attrService
  {
    get
    {
      if (this._attrService == null)
        this._attrService = ServicesManager.ServiceContainer.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
      return this._attrService;
    }
  }

  private IAttributeGroupToCreateList attrGrService
  {
    get
    {
      if (this._attrGrService == null)
        this._attrGrService = ServicesManager.ServiceContainer.GetService(typeof (IAttributeGroupToCreateList)) as IAttributeGroupToCreateList;
      return this._attrGrService;
    }
  }

  private IObjectTypeToCreateList objService
  {
    get
    {
      if (this._objService == null)
        this._objService = ServicesManager.ServiceContainer.GetService(typeof (IObjectTypeToCreateList)) as IObjectTypeToCreateList;
      return this._objService;
    }
  }

  private IDataWriter idw
  {
    get
    {
      if (this._idw == null)
        this._idw = ServicesManager.ServiceContainer.GetService(typeof (IDataWriter)) as IDataWriter;
      return this._idw;
    }
  }

  private IMetadataInfo imdi
  {
    get
    {
      if (this._imdi == null)
        this._imdi = ServicesManager.ServiceContainer.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
      return this._imdi;
    }
  }

  public ItemToCreateSelectDialog(Type itemsType)
  {
    this.InitializeComponent();
    this.itemsType = itemsType;
    this.treeViewAT.Nodes.Clear();
    if (itemsType.Equals(typeof (IAttributeTypeToCreate)))
    {
      this.Text = "Выбор типа атрибута";
      this.buttonNew.Text = "Создать тип атрибута";
      this.treeViewAT.ImageList = this.attrImages.ImageList;
      foreach (IItemToCreate itemToCreate in (IEnumerable<IAttributeTypeToCreate>) this.attrService.Items)
        this.addNewNode(itemToCreate);
    }
    else if (itemsType.Equals(typeof (IAttributeGroupToCreate)))
    {
      this.Text = "Выбор группы атрибутов";
      this.buttonNew.Text = "Создать группу атрибутов";
      foreach (IItemToCreate itemToCreate in (IEnumerable<IAttributeGroupToCreate>) this.attrGrService.Items)
        this.addNewNode(itemToCreate);
    }
    else if (itemsType.Equals(typeof (IObjectTypeToCreate)))
    {
      this.Text = "Выбор типа объектов";
      this.buttonNew.Text = "Создать тип объекта";
      foreach (IItemToCreate itemToCreate in (IEnumerable<IObjectTypeToCreate>) this.objService.Items)
        this.addNewNode(itemToCreate);
    }
    this.treeViewAT.TreeViewNodeSorter = (IComparer) new ItemToCreateSelectDialog.NodeSorter();
  }

  private TreeNode addNewNode(IItemToCreate item)
  {
    TreeNode node = new TreeNode();
    node.Text = item.Name;
    node.Tag = (object) item;
    if (item is IAttributeTypeToCreate)
      node.ImageIndex = node.SelectedImageIndex = this.attrImages.ImageIndex((item as IAttributeTypeToCreate).FieldType);
    this.treeViewAT.Nodes.Add(node);
    return node;
  }

  private TreeNode getNodeForAttrValueType(FieldTypes attrValueType)
  {
    foreach (TreeNode node in this.treeViewAT.Nodes)
    {
      if ((FieldTypes) node.Tag == attrValueType)
        return node;
    }
    TreeNode node1 = new TreeNode();
    node1.Text = AttributesTypeHelper.GetCaption(attrValueType);
    node1.Tag = (object) attrValueType;
    this.treeViewAT.Nodes.Add(node1);
    return node1;
  }

  private TreeNode findNodeByGUID(TreeNodeCollection nodes, Guid guid)
  {
    foreach (TreeNode node in nodes)
    {
      if (node.Tag is IItemToCreate tag && tag.GUID.Equals(guid))
        return node;
      if (node.Nodes.Count > 0)
      {
        TreeNode nodeByGuid = this.findNodeByGUID(node.Nodes, guid);
        if (nodeByGuid != null)
          return nodeByGuid;
      }
    }
    return (TreeNode) null;
  }

  public void UpdateNodeInfo(Guid guid, PropertyValueChangedEventArgs e)
  {
    if (guid.Equals(Guid.Empty))
      return;
    this.updateNodeInfo(this.findNodeByGUID(this.treeViewAT.Nodes, guid), e);
  }

  protected void updateNodeInfo(TreeNode node, PropertyValueChangedEventArgs e)
  {
    if (node == null || node.Tag == null)
      return;
    string displayName = e.ChangedItem.PropertyDescriptor.DisplayName;
    if (node.Tag is IObjectTypeToCreate)
    {
      if (!(node.Tag is IObjectTypeToCreate tag))
        return;
      switch (e.ChangedItem.PropertyDescriptor.Name)
      {
        case "Name":
          string name = Convert.ToString(e.ChangedItem.Value);
          if (this.objService.ExistsByName(name))
          {
            int num = (int) MessageBox.Show(string.Format(ItemToCreateSelectDialog.msgTextObj, (object) displayName), string.Format(ItemToCreateSelectDialog.msgCaption, (object) displayName));
            tag.Name = Convert.ToString(e.OldValue);
            break;
          }
          node.Text = Convert.ToString(name);
          this.objService.UpdateCasheName(Convert.ToString(e.OldValue));
          break;
        case "ShortName":
          if (this.objService.ExistsByShortName(Convert.ToString(e.ChangedItem.Value)))
          {
            int num = (int) MessageBox.Show(string.Format(ItemToCreateSelectDialog.msgTextObj, (object) displayName), string.Format(ItemToCreateSelectDialog.msgCaption, (object) displayName));
            tag.ShortName = Convert.ToString(e.OldValue);
            break;
          }
          this.objService.UpdateCasheShortName(Convert.ToString(e.OldValue), tag);
          break;
      }
    }
    else if (node.Tag is IAttributeGroupToCreate)
    {
      if (!(node.Tag is IAttributeGroupToCreate tag) || !(e.ChangedItem.PropertyDescriptor.Name == "Name"))
        return;
      string name = Convert.ToString(e.ChangedItem.Value);
      if (this.attrGrService.ExistsByName(name))
      {
        int num = (int) MessageBox.Show(string.Format(ItemToCreateSelectDialog.msgTextAttrGr, (object) displayName), string.Format(ItemToCreateSelectDialog.msgCaption, (object) displayName));
        tag.Name = Convert.ToString(e.OldValue);
      }
      else
      {
        node.Text = Convert.ToString(name);
        this.attrGrService.UpdateCasheName(Convert.ToString(e.OldValue));
      }
    }
    else
    {
      if (!(node.Tag is IAttributeTypeToCreate) || !(node.Tag is IAttributeTypeToCreate tag))
        return;
      switch (e.ChangedItem.PropertyDescriptor.Name)
      {
        case "Name":
          string name = Convert.ToString(e.ChangedItem.Value);
          IAttributeTypeToCreate byName = this.attrService.GetByName(name);
          if (byName != null && byName.GUID != tag.GUID)
          {
            int num = (int) MessageBox.Show(string.Format(ItemToCreateSelectDialog.msgTextAttr, (object) displayName), string.Format(ItemToCreateSelectDialog.msgCaption, (object) displayName));
            tag.Name = Convert.ToString(e.OldValue);
            break;
          }
          node.Text = Convert.ToString(name);
          this.attrService.UpdateCasheName(Convert.ToString(e.OldValue));
          break;
        case "Alias":
          IAttributeTypeToCreate byAlias = this.attrService.GetByAlias(Convert.ToString(e.ChangedItem.Value));
          if (byAlias != null && byAlias.GUID != tag.GUID)
          {
            int num = (int) MessageBox.Show(string.Format(ItemToCreateSelectDialog.msgTextAttr, (object) displayName), string.Format(ItemToCreateSelectDialog.msgCaption, (object) displayName));
            tag.Alias = Convert.ToString(e.OldValue);
            break;
          }
          this.attrService.UpdateCasheAlias(Convert.ToString(e.OldValue), tag);
          break;
        case "FieldType":
          this.treeViewAT.BeginUpdate();
          try
          {
            TreeNode forAttrValueType = this.getNodeForAttrValueType((FieldTypes) e.ChangedItem.Value);
            node.Parent.Nodes.Remove(node);
            forAttrValueType.Nodes.Add(node);
          }
          finally
          {
            this.treeViewAT.EndUpdate();
          }
          if (!this.Visible)
            break;
          this.treeViewAT.SelectedNode = node;
          node.EnsureVisible();
          break;
      }
    }
  }

  public Guid SelectedItemGUID
  {
    get
    {
      TreeNode selectedNode = this.treeViewAT.SelectedNode;
      return selectedNode != null && selectedNode.Tag is IItemToCreate ? (selectedNode.Tag as IItemToCreate).GUID : Guid.Empty;
    }
    set
    {
      TreeNode nodeByGuid = this.findNodeByGUID(this.treeViewAT.Nodes, value);
      if (nodeByGuid == null)
        return;
      this.treeViewAT.SelectedNode = nodeByGuid;
      nodeByGuid.EnsureVisible();
    }
  }

  public string SelectedItemName
  {
    get
    {
      TreeNode selectedNode = this.treeViewAT.SelectedNode;
      return selectedNode != null && selectedNode.Tag is IItemToCreate ? (selectedNode.Tag as IItemToCreate).Name : string.Empty;
    }
  }

  private void treeViewAT_AfterSelect(object sender, TreeViewEventArgs e)
  {
    TreeNode selectedNode = this.treeViewAT.SelectedNode;
    IItemToCreate tag = selectedNode == null ? (IItemToCreate) null : selectedNode.Tag as IItemToCreate;
    this.propertyGrid.SelectedObject = (object) tag;
    this.buttonAccept.Enabled = tag != null;
  }

  private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
  {
    this.updateNodeInfo(this.treeViewAT.SelectedNode, e);
  }

  private void buttonNew_Click(object sender, EventArgs e)
  {
    TreeNode treeNode = (TreeNode) null;
    if (this.itemsType.Equals(typeof (IAttributeTypeToCreate)))
    {
      string name = "Новый тип атрибута ";
      int num = 1;
      while (this.attrService.ExistsByName(name + num.ToString()))
        ++num;
      NewAttributeTypeForm attributeTypeForm = new NewAttributeTypeForm(this.attrService, name);
      if (attributeTypeForm.ShowDialog() == DialogResult.OK)
        treeNode = this.addNewNode((IItemToCreate) this.attrService.AddItem(true, attributeTypeForm.LongName, attributeTypeForm.ShortName, string.Empty, attributeTypeForm.Type, (long) attributeTypeForm.Size, this.imdi.NewPumpGuid(), long.MaxValue, false, -1, attributeTypeForm.DefaultValue, attributeTypeForm.MultiValueMode));
    }
    else if (this.itemsType.Equals(typeof (IAttributeGroupToCreate)))
    {
      string str = "Новая группа атрибутов ";
      int num = 1;
      while (this.attrGrService.ExistsByName(str + num.ToString()))
        ++num;
      treeNode = this.addNewNode((IItemToCreate) this.attrGrService.AddItem(true, str + num.ToString(), this.imdi.NewPumpGuid(), long.MaxValue));
    }
    else if (this.itemsType.Equals(typeof (IObjectTypeToCreate)))
    {
      string str = "Новый тип объекта ";
      int num = 1;
      while (this.objService.ExistsByName(str + num.ToString()))
        ++num;
      treeNode = this.addNewNode((IItemToCreate) this.objService.AddItem(true, str + num.ToString(), string.Empty, this.imdi.NewPumpGuid(), long.MaxValue));
    }
    if (treeNode == null)
      return;
    this.treeViewAT.SelectedNode = treeNode;
    treeNode.EnsureVisible();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ItemToCreateSelectDialog));
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.groupBox1 = new GroupBox();
    this.splitContainer1 = new SplitContainer();
    this.treeViewAT = new TreeView();
    this.propertyGrid = new PropertyGrid();
    this.buttonCancel = new Button();
    this.buttonAccept = new Button();
    this.buttonNew = new Button();
    this.tableLayoutPanel1.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.AccessibleDescription = (string) null;
    this.tableLayoutPanel1.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
    this.tableLayoutPanel1.BackgroundImage = (Image) null;
    this.tableLayoutPanel1.Controls.Add((Control) this.groupBox1, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.buttonCancel, 2, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.buttonAccept, 1, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.buttonNew, 0, 1);
    this.tableLayoutPanel1.Font = (Font) null;
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.groupBox1.AccessibleDescription = (string) null;
    this.groupBox1.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
    this.groupBox1.BackgroundImage = (Image) null;
    this.tableLayoutPanel1.SetColumnSpan((Control) this.groupBox1, 3);
    this.groupBox1.Controls.Add((Control) this.splitContainer1);
    this.groupBox1.Font = (Font) null;
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.TabStop = false;
    this.splitContainer1.AccessibleDescription = (string) null;
    this.splitContainer1.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
    this.splitContainer1.BackgroundImage = (Image) null;
    this.splitContainer1.Font = (Font) null;
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.AccessibleDescription = (string) null;
    this.splitContainer1.Panel1.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.splitContainer1.Panel1, "splitContainer1.Panel1");
    this.splitContainer1.Panel1.BackgroundImage = (Image) null;
    this.splitContainer1.Panel1.Controls.Add((Control) this.treeViewAT);
    this.splitContainer1.Panel1.Font = (Font) null;
    this.splitContainer1.Panel2.AccessibleDescription = (string) null;
    this.splitContainer1.Panel2.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.splitContainer1.Panel2, "splitContainer1.Panel2");
    this.splitContainer1.Panel2.BackgroundImage = (Image) null;
    this.splitContainer1.Panel2.Controls.Add((Control) this.propertyGrid);
    this.splitContainer1.Panel2.Font = (Font) null;
    this.treeViewAT.AccessibleDescription = (string) null;
    this.treeViewAT.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.treeViewAT, "treeViewAT");
    this.treeViewAT.BackgroundImage = (Image) null;
    this.treeViewAT.Font = (Font) null;
    this.treeViewAT.FullRowSelect = true;
    this.treeViewAT.HideSelection = false;
    this.treeViewAT.Name = "treeViewAT";
    this.treeViewAT.AfterSelect += new TreeViewEventHandler(this.treeViewAT_AfterSelect);
    this.propertyGrid.AccessibleDescription = (string) null;
    this.propertyGrid.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.propertyGrid, "propertyGrid");
    this.propertyGrid.BackgroundImage = (Image) null;
    this.propertyGrid.Font = (Font) null;
    this.propertyGrid.Name = "propertyGrid";
    this.propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
    this.buttonCancel.AccessibleDescription = (string) null;
    this.buttonCancel.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.buttonCancel, "buttonCancel");
    this.buttonCancel.BackgroundImage = (Image) null;
    this.buttonCancel.DialogResult = DialogResult.Cancel;
    this.buttonCancel.Font = (Font) null;
    this.buttonCancel.Name = "buttonCancel";
    this.buttonCancel.UseVisualStyleBackColor = true;
    this.buttonAccept.AccessibleDescription = (string) null;
    this.buttonAccept.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.buttonAccept, "buttonAccept");
    this.buttonAccept.BackgroundImage = (Image) null;
    this.buttonAccept.DialogResult = DialogResult.OK;
    this.buttonAccept.Font = (Font) null;
    this.buttonAccept.Name = "buttonAccept";
    this.buttonAccept.UseVisualStyleBackColor = true;
    this.buttonNew.AccessibleDescription = (string) null;
    this.buttonNew.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.buttonNew, "buttonNew");
    this.buttonNew.BackgroundImage = (Image) null;
    this.buttonNew.Font = (Font) null;
    this.buttonNew.Name = "buttonNew";
    this.buttonNew.UseVisualStyleBackColor = true;
    this.buttonNew.Click += new EventHandler(this.buttonNew_Click);
    this.AccessibleDescription = (string) null;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackgroundImage = (Image) null;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Font = (Font) null;
    this.Icon = (Icon) null;
    this.Name = nameof (ItemToCreateSelectDialog);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  DialogResult IItemToCreateSelectDialog.ShowDialog() => this.ShowDialog();

  private class NodeSorter : IComparer
  {
    public int Compare(object x, object y)
    {
      return string.Compare((x as TreeNode).Text, (y as TreeNode).Text);
    }
  }
}
