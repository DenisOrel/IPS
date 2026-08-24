// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mbom.MbomEditorControl
// Assembly: Intermech.Mbom, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 13559C9A-4DBC-479B-BA71-AFEA0247DEC7
// Assembly location: D:\IPS\Client\Intermech.Mbom.dll
// XML documentation location: D:\IPS\Client\Intermech.Mbom.xml

using Infralution.Controls.VirtualTree;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Mbom.Properties;
using Intermech.Navigator;
using Intermech.Navigator.ContextCommands;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Queries;
using Intermech.Search.UI;
using Intermech.Search.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Mbom;

public sealed class MbomEditorControl : UserControl, ISupportInitialize
{
  private const string TotalCountColumnKey = "TotalCount";
  private const string RemainingCountColumnKey = "RemainingCount";
  private const string CheckOutCommandName = "CheckOut";
  private const string CheckInCommandName = "CheckIn";
  private const string RemoveCommandName = "Exclude";
  private long _ebomVersionID;
  private long _mbomVersionID;
  private AddingToMbomInfo _addingToMbomInfo;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private SplitContainer splitContainer1;
  private ToolStrip toolStrip1;
  private ToolStripButton _addToMbomToolStripButton;
  private ToolStripButton _removeFromMbomToolStripButton;
  private ToolStrip _ebomToolStrip;
  private ToolStripButton _calculateQuantityToolStripButton;
  private ToolStrip _mbomToolStrip;
  private ToolStripButton _checkOutToolStripButton;
  private ToolStripButton _checkInToolStripButton;
  private ContextMenuStrip _ebomContextMenuStrip;
  private ContextMenuStrip _mbomContextMenuStrip;
  private ToolStripMenuItem _calculateQuantityToolStripMenuItem;
  private ToolStripMenuItem _addToMbomToolStripMenuItem;
  private ToolStripMenuItem _checkOutToolStripMenuItem;
  private ToolStripMenuItem _checkInToolStripMenuItem;
  private ToolStripMenuItem _removeFromMbomToolStripMenuItem;
  private Column _totalCountColumn;
  private Column _remainingCountColumn;
  private MbomEditorTreeControl _ebomTree;
  private MbomEditorTreeControl _mbomTree;
  private ToolStripButton _createTechnologicalAssemblyUnitToolStripButton;
  private ToolStripMenuItem _createTechnologicalAssemblyUnitToolStripMenuItem;

  public MbomEditorControl()
  {
    this.InitializeComponent();
    this._totalCountColumn.Sortable = false;
    this._totalCountColumn.DataField = "TotalCount";
    this._remainingCountColumn.Sortable = false;
    this._remainingCountColumn.DataField = "RemainingCount";
    this._ebomTree.AddColumn(this._totalCountColumn);
    this._ebomTree.AddColumn(this._remainingCountColumn);
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long EbomVersionID
  {
    get => this._ebomVersionID;
    set
    {
      if (ObjectHelper.IsUnknownObjectVersionID(value))
        throw new ArgumentException();
      if (this._ebomVersionID == value)
        return;
      this._ebomVersionID = value;
      this.CalculateNumber();
      this._ebomTree.Descriptor = (IDescriptor) new MbomEditorControl.EbomDescriptor(this._ebomVersionID, this);
      this.UpdateControl();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long MbomVersionID
  {
    get => this._mbomVersionID;
    set
    {
      if (ObjectHelper.IsUnknownObjectVersionID(value))
        throw new ArgumentException();
      if (this._mbomVersionID == value)
        return;
      this._mbomVersionID = value;
      this._mbomTree.Descriptor = (IDescriptor) new MbomEditorControl.MbomDescriptor(this._mbomVersionID);
      this.UpdateControl();
    }
  }

  public MbomEditorControl.MbomEditorControlMemento GetMemento()
  {
    return new MbomEditorControl.MbomEditorControlMemento()
    {
      EbomNodeColumns = this._ebomTree.GetNodeColumns(),
      MbomNodeColumns = this._mbomTree.GetNodeColumns()
    };
  }

  public void SetMemento(MbomEditorControl.MbomEditorControlMemento memento)
  {
    if (memento == null)
      throw new ArgumentNullException(nameof (memento));
    if (memento.EbomNodeColumns != null)
      this._ebomTree.SetNodeColumns(memento.EbomNodeColumns);
    if (memento.MbomNodeColumns == null)
      return;
    this._mbomTree.SetNodeColumns(memento.MbomNodeColumns);
  }

  public void BeginInit()
  {
  }

  public void EndInit()
  {
    if (this.DesignMode)
      return;
    INamedImageList namedImageList = ServiceLocator.Get<INamedImageList>();
    this._checkOutToolStripButton.Image = this._checkOutToolStripMenuItem.Image = namedImageList.ImageList.Images[namedImageList.ImageIndex("imgCheckOut")];
    this._checkInToolStripButton.Image = this._checkInToolStripMenuItem.Image = namedImageList.ImageList.Images[namedImageList.ImageIndex("imgCheckIn")];
    ServiceLocator.Get<ICategoryTypeIconService>();
    this._createTechnologicalAssemblyUnitToolStripButton.Image = this._createTechnologicalAssemblyUnitToolStripMenuItem.Image = namedImageList.ImageList.Images[namedImageList.ImageIndex("imgContextComposition.PDM")];
    NodeColumnCollection nodeColumns1 = Utils.CaptionAndStatesesColumns(NodeColumnSortOrder.Ascending);
    IColumnSchemes columnSchemes = ServiceLocator.Get<IColumnSchemes>();
    NodeColumn column1 = columnSchemes.CreateColumn(Intermech.Navigator.Consts.RelationColumnSchemeGuid, (object) Constants.CountAttributeTypeID);
    nodeColumns1.Add(column1);
    this._ebomTree.SetNodeColumns(nodeColumns1);
    NodeColumnCollection nodeColumns2 = Utils.CaptionAndStatesesColumns(NodeColumnSortOrder.Ascending);
    NodeColumn column2 = columnSchemes.CreateColumn(Intermech.Navigator.Consts.RelationColumnSchemeGuid, (object) Constants.CountAttributeTypeID);
    nodeColumns2.Add(column2);
    this._mbomTree.SetNodeColumns(nodeColumns2);
    this._ebomTree.SetSupportedColumns(Utils.GetNavigatorColumns());
    this._mbomTree.SetSupportedColumns(Utils.GetNavigatorColumns());
  }

  private void CalculateNumberToolStripButton_Click(object sender, EventArgs e)
  {
    this.CalculateNumber();
  }

  private void CheckOutToolStripButton_Click(object sender, EventArgs e) => this.CheckOut();

  private void CheckInToolStripButton_Click(object sender, EventArgs e) => this.CheckIn();

  private void AddToMbomToolStripButton_Click(object sender, EventArgs e) => this.AddToMbom();

  private void RemoveFromMbomToolStripButton_Click(object sender, EventArgs e)
  {
    this.RemoveFromMbom();
  }

  private void CalculateNumberToolStripMenuItem_Click(object sender, EventArgs e)
  {
    this.CalculateNumber();
  }

  private void AddToMbomToolStripMenuItem_Click(object sender, EventArgs e) => this.AddToMbom();

  private void CheckOutToolStripMenuItem_Click(object sender, EventArgs e) => this.CheckOut();

  private void CheckInToolStripMenuItem_Click(object sender, EventArgs e) => this.CheckIn();

  private void RemoveFromMbomToolStripMenuItem_Click(object sender, EventArgs e)
  {
    this.RemoveFromMbom();
  }

  private void СreateTechnologicalAssemblyUnitToolStripButton_Click(object sender, EventArgs e)
  {
    this.CreateTechnologicalAssemblyUnit();
  }

  private void СreateTechnologicalAssemblyUnitToolStripMenuItem_Click(object sender, EventArgs e)
  {
    this.CreateTechnologicalAssemblyUnit();
  }

  private void EbomMbomEditorTreeControl_SelectionChanged(object sender, EventArgs e)
  {
    this.UpdateControl();
  }

  private void MbomMbomEditorTreeControl_SelectionChanged(object sender, EventArgs e)
  {
    this.UpdateControl();
  }

  private void EbomMbomEditorTreeControl_GetCellValue(
    object sender,
    MbomEditorTreeControl.GetCellValueEventArgs e)
  {
    if (e.Column == this._remainingCountColumn)
    {
      e.CellValue = this.GetRemainingCountForEbomNavigatorTreeNode(e.NavigatorTreeNode);
    }
    else
    {
      if (e.Column != this._totalCountColumn)
        return;
      e.CellValue = this.GetTotalCountForEbomNavigatorTreeNode(e.NavigatorTreeNode);
    }
  }

  private void EbomMbomEditorTreeControl_CommandsTableUpdated(object sender, EventArgs e)
  {
    this.UpdateControl();
  }

  private void MbomMbomEditorTreeControl_CommandsTableUpdated(object sender, EventArgs e)
  {
    this.UpdateControl();
  }

  private void EbomMbomEditorTreeControl_GetNodeDragDropEffects(
    object sender,
    MbomNavigatorTreeView.GetNodeDragDropEffectsEventArgs e)
  {
    if ((e.DataObject.GetData(typeof (IOSource)) as IOSource).SelectedItems.GetItemData(0, typeof (NavigatorTreeNode)) is NavigatorTreeNode itemData && e.Node.Tree != itemData.Tree && itemData.Parent != null && itemData.Parent.NodeID is NodeID nodeId && MbomHelper.IsMbomObjectTypeID(nodeId.ObjectTypeID))
      e.DragDropEffects = DragDropEffects.Move;
    else
      e.DragDropEffects = DragDropEffects.None;
  }

  private void EbomMbomEditorTreeControl_NodeDrop(
    object sender,
    MbomNavigatorTreeView.NodeDropEventArgs e)
  {
    this.RemoveFromMbom((e.DataObject.GetData(typeof (IOSource)) as IOSource).SelectedItems.GetItemData(0, typeof (NavigatorTreeNode)) as NavigatorTreeNode);
  }

  private void MbomMbomEditorTreeControl_GetNodeDragDropEffects(
    object sender,
    MbomNavigatorTreeView.GetNodeDragDropEffectsEventArgs e)
  {
    NavigatorTreeNode itemData = (e.DataObject.GetData(typeof (IOSource)) as IOSource).SelectedItems.GetItemData(0, typeof (NavigatorTreeNode)) as NavigatorTreeNode;
    if (itemData.Tree != e.Node.Tree && this.CanAddToMbom(itemData, e.Node))
      e.DragDropEffects = DragDropEffects.Copy;
    else if (itemData.Tree == e.Node.Tree && this.CanTransferToMbom(itemData, e.Node))
      e.DragDropEffects = DragDropEffects.Move;
    else
      e.DragDropEffects = DragDropEffects.None;
  }

  private void MbomMbomEditorTreeControl_NodeDrop(
    object sender,
    MbomNavigatorTreeView.NodeDropEventArgs e)
  {
    NavigatorTreeNode itemData = (e.DataObject.GetData(typeof (IOSource)) as IOSource).SelectedItems.GetItemData(0, typeof (NavigatorTreeNode)) as NavigatorTreeNode;
    if (itemData.Tree != e.Node.Tree)
      this.AddToMbom(itemData, e.Node);
    else
      this.TransferToMbom(itemData, e.Node);
  }

  private void UpdateControl()
  {
    this._addToMbomToolStripButton.Enabled = this._addToMbomToolStripMenuItem.Enabled = this.CanAddToMbom();
    this._removeFromMbomToolStripButton.Enabled = this._removeFromMbomToolStripMenuItem.Enabled = this.CanRemoveFromMbom();
    this._checkOutToolStripButton.Enabled = this._checkOutToolStripMenuItem.Enabled = this.CanCheckOut();
    this._checkInToolStripButton.Enabled = this._checkInToolStripMenuItem.Enabled = this.CanCheckIn();
    this._createTechnologicalAssemblyUnitToolStripButton.Enabled = this._createTechnologicalAssemblyUnitToolStripMenuItem.Enabled = this.CanCreateTechnologicalAssemblyUnit();
  }

  private bool CanAddToMbom()
  {
    NavigatorTreeNode[] selectedNodes = this._ebomTree.GetSelectedNodes();
    NavigatorTreeNode[] selectedMbomNodes = this._mbomTree.GetSelectedNodes();
    return selectedNodes.Length != 0 && selectedMbomNodes.Length == 1 && ((IEnumerable<NavigatorTreeNode>) selectedNodes).All<NavigatorTreeNode>((System.Func<NavigatorTreeNode, bool>) (ebomNode => ((IEnumerable<NavigatorTreeNode>) selectedMbomNodes).All<NavigatorTreeNode>((System.Func<NavigatorTreeNode, bool>) (mbomNode => this.CanAddToMbom(ebomNode, mbomNode)))));
  }

  private bool CanAddToMbom(
    NavigatorTreeNode ebomNavigatorTreeNode,
    NavigatorTreeNode mbomNavigatorTreeNode)
  {
    AddingToMbomInfo navigatorTreeNode = this.GetAddingToMbomInfoForNavigatorTreeNode(ebomNavigatorTreeNode);
    return navigatorTreeNode != null && string.IsNullOrEmpty(navigatorTreeNode.ErrorMessage) && navigatorTreeNode.RemainingCount.Value > 0.0 && this.IsMbomOrSimilarNavigatorTreeNode(mbomNavigatorTreeNode);
  }

  private bool IsMbomOrSimilarNavigatorTreeNode(NavigatorTreeNode navigatorTreeNode)
  {
    return MbomHelper.IsMbomOrSimilarObjectType(MbomClientHelper.GetObjectNodeID(navigatorTreeNode).ObjectTypeID);
  }

  private bool CanRemoveFromMbom() => this._mbomTree.CanExecuteCommand("Exclude");

  private bool CanCheckOut() => this._mbomTree.CanExecuteCommand("CheckOut");

  private bool CanCheckIn() => this._mbomTree.CanExecuteCommand("CheckIn");

  private void CalculateNumber()
  {
    this.FindAddingToMbomInfo();
    this._ebomTree.Update();
  }

  private void FindAddingToMbomInfo()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._addingToMbomInfo = ((IMbomServerService) sessionKeeper.Session.GetCustomService(typeof (IMbomServerService))).FindAddingToMbomInfo(sessionKeeper.Session.SessionGUID, this._ebomVersionID);
  }

  private void AddToMbom()
  {
    foreach (NavigatorTreeNode selectedNode1 in this._ebomTree.GetSelectedNodes())
    {
      foreach (NavigatorTreeNode selectedNode2 in this._mbomTree.GetSelectedNodes())
        this.AddToMbom(selectedNode1, selectedNode2);
    }
  }

  private void AddToMbom(NavigatorTreeNode ebomNode, NavigatorTreeNode mbomNode)
  {
    AddingToMbomInfo navigatorTreeNode = this.GetAddingToMbomInfoForNavigatorTreeNode(ebomNode);
    using (AddingToMbomForm addingToMbomForm = new AddingToMbomForm())
    {
      NodeID objectNodeId1 = MbomClientHelper.GetObjectNodeID(ebomNode);
      addingToMbomForm.Text = $"Задание количества для {objectNodeId1.Caption}";
      addingToMbomForm.RemainingCount = navigatorTreeNode.RemainingCount;
      addingToMbomForm.TotalCount = navigatorTreeNode.TotalCount;
      if (addingToMbomForm.ShowDialog() != DialogResult.OK)
        return;
      NavigatorTreeNode[] selectedNodes = this._mbomTree.GetSelectedNodes();
      try
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          using (NotificationContext.Create(sessionKeeper.Session))
          {
            NodeID objectNodeId2 = MbomClientHelper.GetObjectNodeID(ebomNode);
            NodeID objectNodeId3 = MbomClientHelper.GetObjectNodeID(mbomNode);
            try
            {
              IMbomServerService customService = (IMbomServerService) sessionKeeper.Session.GetCustomService(typeof (IMbomServerService));
              AddingToMbomParams addingToMbomParams1 = new AddingToMbomParams(objectNodeId3.ObjectID, objectNodeId2.ObjectID)
              {
                Count = addingToMbomForm.Count
              };
              Guid sessionGuid = sessionKeeper.Session.SessionGUID;
              AddingToMbomParams addingToMbomParams2 = addingToMbomParams1;
              customService.AddToMbom(sessionGuid, addingToMbomParams2);
              navigatorTreeNode.RemainingCount.Substract(addingToMbomParams1.Count);
            }
            finally
            {
              this.FindAddingToMbomInfo();
              ServiceLocator.Get<INotificationService>().FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", objectNodeId2.ObjectID));
              this._ebomTree.Update();
            }
          }
        }
      }
      finally
      {
        this._mbomTree.SetSelectedNodes(selectedNodes);
      }
    }
  }

  private void RemoveFromMbom()
  {
    List<long> list = ((IEnumerable<NavigatorTreeNode>) this._mbomTree.GetSelectedNodes()).Select<NavigatorTreeNode, NodeID>((System.Func<NavigatorTreeNode, NodeID>) (node => MbomClientHelper.GetObjectNodeID(node))).Where<NodeID>((System.Func<NodeID, bool>) (node => node != null)).Select<NodeID, long>((System.Func<NodeID, long>) (node => node.ObjectID)).ToList<long>();
    try
    {
      this._mbomTree.ExecuteCommand("Exclude");
      this.FindAddingToMbomInfo();
    }
    finally
    {
      this.FindAddingToMbomInfo();
      ServiceLocator.Get<INotificationService>().FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", (IList<long>) list));
      this._ebomTree.Update();
    }
  }

  private void RemoveFromMbom(NavigatorTreeNode node)
  {
    try
    {
      ObjectCommands.ExcludeCommand((ISelectedItems) new NavigatorTreeViewSelectedItem(node.Tree, node), node.Tree.Services, (object) null);
    }
    finally
    {
      this.FindAddingToMbomInfo();
      NodeID objectNodeId = MbomClientHelper.GetObjectNodeID(node);
      ServiceLocator.Get<INotificationService>().FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", objectNodeId.ObjectID));
      this._ebomTree.Update();
    }
  }

  private void CheckOut() => this._mbomTree.ExecuteCommand(nameof (CheckOut));

  private void CheckIn() => this._mbomTree.ExecuteCommand(nameof (CheckIn));

  private bool CanCreateTechnologicalAssemblyUnit()
  {
    NavigatorTreeNode[] selectedNodes = this._mbomTree.GetSelectedNodes();
    if (selectedNodes.Length != 1)
      return false;
    NodeID objectNodeId = MbomClientHelper.GetObjectNodeID(selectedNodes[0]);
    return objectNodeId != null && MbomHelper.IsMbomOrSimilarObjectType(objectNodeId.ObjectTypeID);
  }

  private void CreateTechnologicalAssemblyUnit()
  {
    if (!(ServicesManager.GetService(typeof (IObjectCreatorService)) is IObjectCreatorService service))
      return;
    long objectByTypeDialog = service.CreateObjectByTypeDialog(MbomConstants.TechnologicalAssemblyUnitObjectTypeID);
    if (ObjectHelper.IsUnknownObjectVersionID(objectByTypeDialog))
      return;
    NodeID objectNodeId = MbomClientHelper.GetObjectNodeID(this._mbomTree.GetSelectedNodes()[0]);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session, (object) this))
        (sessionKeeper.Session.GetCustomService(typeof (IMbomServerService)) as IMbomServerService).AddTauToMbom(sessionKeeper.Session.SessionGUID, objectNodeId.ObjectID, objectByTypeDialog);
    }
  }

  private object GetRemainingCountForEbomNavigatorTreeNode(NavigatorTreeNode navigatorTreeNode)
  {
    AddingToMbomInfo navigatorTreeNode1 = this.GetAddingToMbomInfoForNavigatorTreeNode(navigatorTreeNode);
    return navigatorTreeNode1 == null ? (object) null : (object) navigatorTreeNode1.RemainingCount;
  }

  private object GetTotalCountForEbomNavigatorTreeNode(NavigatorTreeNode navigatorTreeNode)
  {
    AddingToMbomInfo navigatorTreeNode1 = this.GetAddingToMbomInfoForNavigatorTreeNode(navigatorTreeNode);
    return navigatorTreeNode1 == null ? (object) null : (object) navigatorTreeNode1.TotalCount;
  }

  private AddingToMbomInfo GetAddingToMbomInfoForNavigatorTreeNode(
    NavigatorTreeNode navigatorTreeNode)
  {
    if (this._addingToMbomInfo == null)
      return (AddingToMbomInfo) null;
    AddingToMbomInfo navigatorTreeNode1 = this._addingToMbomInfo;
    foreach (NavigatorTreeNode navigatorTreeNode2 in ((IEnumerable<NavigatorTreeNode>) this.GetNavigatorTreeNodePath(navigatorTreeNode)).Skip<NavigatorTreeNode>(1))
    {
      NodeID objectNodeId = MbomClientHelper.GetObjectNodeID(navigatorTreeNode2);
      if (objectNodeId != null)
      {
        if (!navigatorTreeNode1.Children.ContainsKey(objectNodeId.ObjectID))
          return (AddingToMbomInfo) null;
        navigatorTreeNode1 = navigatorTreeNode1.Children[objectNodeId.ObjectID];
      }
    }
    return navigatorTreeNode1;
  }

  private NavigatorTreeNode[] GetNavigatorTreeNodePath(NavigatorTreeNode navigatorTreeNode)
  {
    List<NavigatorTreeNode> source = new List<NavigatorTreeNode>();
    source.Add(navigatorTreeNode);
    for (; navigatorTreeNode.Parent != null && navigatorTreeNode.Parent.NodeID != null; navigatorTreeNode = navigatorTreeNode.Parent)
      source.Add(navigatorTreeNode.Parent);
    return source.Reverse<NavigatorTreeNode>().ToArray<NavigatorTreeNode>();
  }

  private bool CanTransferToMbom(NavigatorTreeNode node, NavigatorTreeNode destinationNode)
  {
    NodeID objectNodeId1 = MbomClientHelper.GetObjectNodeID(node);
    NodeID objectNodeId2 = MbomClientHelper.GetObjectNodeID(destinationNode);
    return objectNodeId1 != null && MbomHelper.IsTechnologicalAssemblyUnitObjectType(objectNodeId1.ObjectTypeID) && objectNodeId2 != null && MbomHelper.IsMbomOrSimilarObjectType(objectNodeId2.ObjectTypeID) && objectNodeId1.ObjectID != objectNodeId2.ObjectID;
  }

  private void TransferToMbom(NavigatorTreeNode node, NavigatorTreeNode destinationNode)
  {
    NodeID objectNodeId1 = MbomClientHelper.GetObjectNodeID(node);
    NodeID objectNodeId2 = MbomClientHelper.GetObjectNodeID(destinationNode);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session))
        (sessionKeeper.Session.GetCustomService(typeof (IMbomServerService)) as IMbomServerService).TransferTauToMbom(sessionKeeper.Session.SessionGUID, objectNodeId2.ObjectID, objectNodeId1.ObjectID, objectNodeId1.PrjLinkID);
    }
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MbomEditorControl));
    this.splitContainer1 = new SplitContainer();
    this._ebomTree = new MbomEditorTreeControl();
    this._ebomContextMenuStrip = new ContextMenuStrip(this.components);
    this._calculateQuantityToolStripMenuItem = new ToolStripMenuItem();
    this._addToMbomToolStripMenuItem = new ToolStripMenuItem();
    this.toolStrip1 = new ToolStrip();
    this._addToMbomToolStripButton = new ToolStripButton();
    this._removeFromMbomToolStripButton = new ToolStripButton();
    this._ebomToolStrip = new ToolStrip();
    this._calculateQuantityToolStripButton = new ToolStripButton();
    this._mbomTree = new MbomEditorTreeControl();
    this._mbomContextMenuStrip = new ContextMenuStrip(this.components);
    this._createTechnologicalAssemblyUnitToolStripMenuItem = new ToolStripMenuItem();
    this._checkOutToolStripMenuItem = new ToolStripMenuItem();
    this._checkInToolStripMenuItem = new ToolStripMenuItem();
    this._removeFromMbomToolStripMenuItem = new ToolStripMenuItem();
    this._mbomToolStrip = new ToolStrip();
    this._createTechnologicalAssemblyUnitToolStripButton = new ToolStripButton();
    this._checkOutToolStripButton = new ToolStripButton();
    this._checkInToolStripButton = new ToolStripButton();
    this._totalCountColumn = new Column();
    this._remainingCountColumn = new Column();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this._ebomContextMenuStrip.SuspendLayout();
    this.toolStrip1.SuspendLayout();
    this._ebomToolStrip.SuspendLayout();
    this._mbomContextMenuStrip.SuspendLayout();
    this._mbomToolStrip.SuspendLayout();
    this.SuspendLayout();
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.Location = new Point(0, 0);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this._ebomTree);
    this.splitContainer1.Panel1.Controls.Add((Control) this.toolStrip1);
    this.splitContainer1.Panel1.Controls.Add((Control) this._ebomToolStrip);
    this.splitContainer1.Panel2.Controls.Add((Control) this._mbomTree);
    this.splitContainer1.Panel2.Controls.Add((Control) this._mbomToolStrip);
    this.splitContainer1.Size = new Size(529, 380);
    this.splitContainer1.SplitterDistance = 253;
    this.splitContainer1.TabIndex = 0;
    this._ebomTree.ContextMenuStrip = this._ebomContextMenuStrip;
    this._ebomTree.Dock = DockStyle.Fill;
    this._ebomTree.Location = new Point(0, 25);
    this._ebomTree.Name = "_ebomMbomEditorTreeControl";
    this._ebomTree.Size = new Size(229, 355);
    this._ebomTree.TabIndex = 2;
    this._ebomTree.SelectionChanged += new EventHandler(this.EbomMbomEditorTreeControl_SelectionChanged);
    this._ebomTree.GetCellValue += new EventHandler<MbomEditorTreeControl.GetCellValueEventArgs>(this.EbomMbomEditorTreeControl_GetCellValue);
    this._ebomTree.CommandsTableUpdated += new EventHandler(this.EbomMbomEditorTreeControl_CommandsTableUpdated);
    this._ebomTree.GetNodeDragDropEffects += new EventHandler<MbomNavigatorTreeView.GetNodeDragDropEffectsEventArgs>(this.EbomMbomEditorTreeControl_GetNodeDragDropEffects);
    this._ebomTree.NodeDrop += new EventHandler<MbomNavigatorTreeView.NodeDropEventArgs>(this.EbomMbomEditorTreeControl_NodeDrop);
    this._ebomContextMenuStrip.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this._calculateQuantityToolStripMenuItem,
      (ToolStripItem) this._addToMbomToolStripMenuItem
    });
    this._ebomContextMenuStrip.Name = "_ebomContextMenuStrip";
    this._ebomContextMenuStrip.Size = new Size(203, 48 /*0x30*/);
    this._calculateQuantityToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("_calculateQuantityToolStripMenuItem.Image");
    this._calculateQuantityToolStripMenuItem.Name = "_calculateQuantityToolStripMenuItem";
    this._calculateQuantityToolStripMenuItem.Size = new Size(202, 22);
    this._calculateQuantityToolStripMenuItem.Text = "Рассчитать количество";
    this._calculateQuantityToolStripMenuItem.Click += new EventHandler(this.CalculateNumberToolStripMenuItem_Click);
    this._addToMbomToolStripMenuItem.Image = (Image) Resources.Actions_go_next_view_icon;
    this._addToMbomToolStripMenuItem.Name = "_addToMbomToolStripMenuItem";
    this._addToMbomToolStripMenuItem.Size = new Size(202, 22);
    this._addToMbomToolStripMenuItem.Text = "Добавить в ТЭСИ";
    this._addToMbomToolStripMenuItem.Click += new EventHandler(this.AddToMbomToolStripMenuItem_Click);
    this.toolStrip1.Dock = DockStyle.Right;
    this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
    this.toolStrip1.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this._addToMbomToolStripButton,
      (ToolStripItem) this._removeFromMbomToolStripButton
    });
    this.toolStrip1.Location = new Point(229, 25);
    this.toolStrip1.Name = "toolStrip1";
    this.toolStrip1.Size = new Size(24, 355);
    this.toolStrip1.TabIndex = 1;
    this.toolStrip1.Text = "toolStrip1";
    this._addToMbomToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._addToMbomToolStripButton.Image = (Image) componentResourceManager.GetObject("_addToMbomToolStripButton.Image");
    this._addToMbomToolStripButton.ImageTransparentColor = Color.Magenta;
    this._addToMbomToolStripButton.Name = "_addToMbomToolStripButton";
    this._addToMbomToolStripButton.Size = new Size(21, 20);
    this._addToMbomToolStripButton.Text = "Добавить в ТЭСИ";
    this._addToMbomToolStripButton.Click += new EventHandler(this.AddToMbomToolStripButton_Click);
    this._removeFromMbomToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._removeFromMbomToolStripButton.Image = (Image) componentResourceManager.GetObject("_removeFromMbomToolStripButton.Image");
    this._removeFromMbomToolStripButton.ImageTransparentColor = Color.Magenta;
    this._removeFromMbomToolStripButton.Name = "_removeFromMbomToolStripButton";
    this._removeFromMbomToolStripButton.Size = new Size(21, 20);
    this._removeFromMbomToolStripButton.Text = "Удалить из ТЭСИ";
    this._removeFromMbomToolStripButton.Click += new EventHandler(this.RemoveFromMbomToolStripButton_Click);
    this._ebomToolStrip.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this._calculateQuantityToolStripButton
    });
    this._ebomToolStrip.Location = new Point(0, 0);
    this._ebomToolStrip.Name = "_ebomToolStrip";
    this._ebomToolStrip.Size = new Size(253, 25);
    this._ebomToolStrip.TabIndex = 0;
    this._ebomToolStrip.Text = "toolStrip2";
    this._calculateQuantityToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._calculateQuantityToolStripButton.Image = (Image) componentResourceManager.GetObject("_calculateQuantityToolStripButton.Image");
    this._calculateQuantityToolStripButton.ImageTransparentColor = Color.Magenta;
    this._calculateQuantityToolStripButton.Name = "_calculateQuantityToolStripButton";
    this._calculateQuantityToolStripButton.Size = new Size(23, 22);
    this._calculateQuantityToolStripButton.Text = "Рассчитать количество";
    this._calculateQuantityToolStripButton.Click += new EventHandler(this.CalculateNumberToolStripButton_Click);
    this._mbomTree.ContextMenuStrip = this._mbomContextMenuStrip;
    this._mbomTree.Dock = DockStyle.Fill;
    this._mbomTree.Location = new Point(0, 25);
    this._mbomTree.Name = "_mbomMbomEditorTreeControl";
    this._mbomTree.Size = new Size(272, 355);
    this._mbomTree.TabIndex = 1;
    this._mbomTree.SelectionChanged += new EventHandler(this.MbomMbomEditorTreeControl_SelectionChanged);
    this._mbomTree.CommandsTableUpdated += new EventHandler(this.MbomMbomEditorTreeControl_CommandsTableUpdated);
    this._mbomTree.GetNodeDragDropEffects += new EventHandler<MbomNavigatorTreeView.GetNodeDragDropEffectsEventArgs>(this.MbomMbomEditorTreeControl_GetNodeDragDropEffects);
    this._mbomTree.NodeDrop += new EventHandler<MbomNavigatorTreeView.NodeDropEventArgs>(this.MbomMbomEditorTreeControl_NodeDrop);
    this._mbomContextMenuStrip.Items.AddRange(new ToolStripItem[4]
    {
      (ToolStripItem) this._createTechnologicalAssemblyUnitToolStripMenuItem,
      (ToolStripItem) this._checkOutToolStripMenuItem,
      (ToolStripItem) this._checkInToolStripMenuItem,
      (ToolStripItem) this._removeFromMbomToolStripMenuItem
    });
    this._mbomContextMenuStrip.Name = "_mbomContextMenuStrip";
    this._mbomContextMenuStrip.Size = new Size(228, 92);
    this._createTechnologicalAssemblyUnitToolStripMenuItem.Name = "_createTechnologicalAssemblyUnitToolStripMenuItem";
    this._createTechnologicalAssemblyUnitToolStripMenuItem.Size = new Size(227, 22);
    this._createTechnologicalAssemblyUnitToolStripMenuItem.Text = "Создать ТСЕ";
    this._createTechnologicalAssemblyUnitToolStripMenuItem.ToolTipText = "Создать ТСЕ";
    this._createTechnologicalAssemblyUnitToolStripMenuItem.Click += new EventHandler(this.СreateTechnologicalAssemblyUnitToolStripMenuItem_Click);
    this._checkOutToolStripMenuItem.Name = "_checkOutToolStripMenuItem";
    this._checkOutToolStripMenuItem.Size = new Size(227, 22);
    this._checkOutToolStripMenuItem.Text = "Взять на редактирование";
    this._checkOutToolStripMenuItem.Click += new EventHandler(this.CheckOutToolStripMenuItem_Click);
    this._checkInToolStripMenuItem.Name = "_checkInToolStripMenuItem";
    this._checkInToolStripMenuItem.Size = new Size(227, 22);
    this._checkInToolStripMenuItem.Text = "Завершить редактирование";
    this._checkInToolStripMenuItem.Click += new EventHandler(this.CheckInToolStripMenuItem_Click);
    this._removeFromMbomToolStripMenuItem.Image = (Image) Resources.Actions_go_previous_view_icon;
    this._removeFromMbomToolStripMenuItem.Name = "_removeFromMbomToolStripMenuItem";
    this._removeFromMbomToolStripMenuItem.Size = new Size(227, 22);
    this._removeFromMbomToolStripMenuItem.Text = "Удалить из ТЭСИ";
    this._removeFromMbomToolStripMenuItem.Click += new EventHandler(this.RemoveFromMbomToolStripMenuItem_Click);
    this._mbomToolStrip.Items.AddRange(new ToolStripItem[3]
    {
      (ToolStripItem) this._createTechnologicalAssemblyUnitToolStripButton,
      (ToolStripItem) this._checkOutToolStripButton,
      (ToolStripItem) this._checkInToolStripButton
    });
    this._mbomToolStrip.Location = new Point(0, 0);
    this._mbomToolStrip.Name = "_mbomToolStrip";
    this._mbomToolStrip.Size = new Size(272, 25);
    this._mbomToolStrip.TabIndex = 0;
    this._mbomToolStrip.Text = "toolStrip1";
    this._createTechnologicalAssemblyUnitToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._createTechnologicalAssemblyUnitToolStripButton.Image = (Image) componentResourceManager.GetObject("_createTechnologicalAssemblyUnitToolStripButton.Image");
    this._createTechnologicalAssemblyUnitToolStripButton.ImageTransparentColor = Color.Magenta;
    this._createTechnologicalAssemblyUnitToolStripButton.Name = "_createTechnologicalAssemblyUnitToolStripButton";
    this._createTechnologicalAssemblyUnitToolStripButton.Size = new Size(23, 22);
    this._createTechnologicalAssemblyUnitToolStripButton.Text = "Создать ТСЕ";
    this._createTechnologicalAssemblyUnitToolStripButton.Click += new EventHandler(this.СreateTechnologicalAssemblyUnitToolStripButton_Click);
    this._checkOutToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._checkOutToolStripButton.Image = (Image) componentResourceManager.GetObject("_checkOutToolStripButton.Image");
    this._checkOutToolStripButton.ImageTransparentColor = Color.Magenta;
    this._checkOutToolStripButton.Name = "_checkOutToolStripButton";
    this._checkOutToolStripButton.Size = new Size(23, 22);
    this._checkOutToolStripButton.Text = "Взять на редактирование";
    this._checkOutToolStripButton.Click += new EventHandler(this.CheckOutToolStripButton_Click);
    this._checkInToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._checkInToolStripButton.Image = (Image) componentResourceManager.GetObject("_checkInToolStripButton.Image");
    this._checkInToolStripButton.ImageTransparentColor = Color.Magenta;
    this._checkInToolStripButton.Name = "_checkInToolStripButton";
    this._checkInToolStripButton.Size = new Size(23, 22);
    this._checkInToolStripButton.Text = "Завершить редактирование";
    this._checkInToolStripButton.Click += new EventHandler(this.CheckInToolStripButton_Click);
    this._totalCountColumn.Caption = "Общ. кол.";
    this._totalCountColumn.Name = "_totalCountColumn";
    this._totalCountColumn.ToolTip = "Общее количество";
    this._remainingCountColumn.Caption = "Ост. кол.";
    this._remainingCountColumn.Name = "_remainingCountColumn";
    this._remainingCountColumn.ToolTip = "Оставшееся количество";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.splitContainer1);
    this.Name = nameof (MbomEditorControl);
    this.Size = new Size(529, 380);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel1.PerformLayout();
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.Panel2.PerformLayout();
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this._ebomContextMenuStrip.ResumeLayout(false);
    this.toolStrip1.ResumeLayout(false);
    this.toolStrip1.PerformLayout();
    this._ebomToolStrip.ResumeLayout(false);
    this._ebomToolStrip.PerformLayout();
    this._mbomContextMenuStrip.ResumeLayout(false);
    this._mbomToolStrip.ResumeLayout(false);
    this._mbomToolStrip.PerformLayout();
    this.ResumeLayout(false);
  }

  [Serializable]
  public sealed class MbomEditorControlMemento
  {
    public NodeColumnCollection MbomNodeColumns { get; set; }

    public NodeColumnCollection EbomNodeColumns { get; set; }
  }

  public sealed class EbomDescriptor : Intermech.Navigator.DBObjects.Descriptor
  {
    private MbomEditorControl _mbomEditorControl;

    public EbomDescriptor(long ebomVersionID, MbomEditorControl mbomEditorControl)
      : base(ebomVersionID)
    {
      this._mbomEditorControl = mbomEditorControl != null ? mbomEditorControl : throw new ArgumentNullException(nameof (mbomEditorControl));
    }

    public override INode GetChild(INodeID nodeID)
    {
      if (!(nodeID is NodeID))
        return (INode) null;
      NodeID nodeId = (NodeID) nodeID;
      return (INode) new MbomEditorControl.EbomNode(nodeId.ObjectTypeID, nodeId.ObjectID, this._mbomEditorControl, this.Services);
    }
  }

  public sealed class EbomNode : ObjectNode
  {
    private MbomEditorControl _mbomEditorControl;

    public EbomNode(
      int ebomTypeID,
      long ebomVersionID,
      MbomEditorControl mbomEditorControl,
      IServiceProvider serviceProvider)
      : base(ebomTypeID, ebomVersionID)
    {
      this._mbomEditorControl = mbomEditorControl != null ? mbomEditorControl : throw new ArgumentNullException(nameof (mbomEditorControl));
      this.Services = serviceProvider;
    }

    protected override List<PartSlot> CreateFolderSlots()
    {
      List<PartSlot> folderSlots = new List<PartSlot>();
      AddingToMbomInfo addingToMbomInfo = this._mbomEditorControl._addingToMbomInfo.GetDescendants().FirstOrDefault<AddingToMbomInfo>((System.Func<AddingToMbomInfo, bool>) (o => o.ObjectVersionID == this._objID));
      if (MbomHelper.IsEbomObjectType(this._objTypeID) && addingToMbomInfo == null || addingToMbomInfo != null && addingToMbomInfo.Statuses.HasFlag((Enum) AddingToMbomStatuses.NotBindedEbom))
        folderSlots.Add(new PartSlot(Guid.NewGuid(), (INodePart) new MbomEditorControl.EbomNodePart(this._objTypeID, this._objID, this._mbomEditorControl, this.Services)));
      else
        folderSlots.Add(new PartSlot(Guid.NewGuid(), (INodePart) new MbomEditorControl.MbomForEbomNodePart(this._objTypeID, this._objID, this.Services)));
      return folderSlots;
    }
  }

  public sealed class MbomForEbomNodePart(
    int ebomTypeID,
    long ebomVersionID,
    IServiceProvider serviceProvider) : RelatedObjectsPart(ebomTypeID, ebomVersionID, RelatedObjectsRole.Composition, MbomConstants.MbomBindingRelationTypeID, -1, (ConditionStructure[]) null, serviceProvider)
  {
    public override INode GetChild(INodeID nodeID) => (INode) null;
  }

  public sealed class EbomNodePart : RelatedObjectsPart
  {
    private MbomEditorControl _mbomEditorControl;

    public EbomNodePart(
      int ebomTypeID,
      long ebomVersionID,
      MbomEditorControl mbomEditorControl,
      IServiceProvider serviceProvider)
      : base(ebomTypeID, ebomVersionID, RelatedObjectsRole.Composition, MbomConstants.EbomCompositionRelationTypeID, -1, (ConditionStructure[]) null, serviceProvider)
    {
      this._mbomEditorControl = mbomEditorControl != null ? mbomEditorControl : throw new ArgumentNullException(nameof (mbomEditorControl));
    }

    public override INode GetChild(INodeID nodeID)
    {
      if (!(nodeID is NodeID))
        return (INode) null;
      NodeID nodeId = (NodeID) nodeID;
      return (INode) new MbomEditorControl.EbomNode(nodeId.ObjectTypeID, nodeId.ObjectID, this._mbomEditorControl, this.Services);
    }

    protected override INodeQuery GetQuery(ConditionStructure[] conditions)
    {
      return (INodeQuery) new MbomEditorControl.EbomNodeQuery(this._objID, this._objTypeID, conditions, (INodeQuerySupport) this, this._mbomEditorControl);
    }
  }

  public sealed class EbomNodeQuery : RelatedObjectsQuery
  {
    private MbomEditorControl _mbomEditorControl;

    public EbomNodeQuery(
      long ebomVersionID,
      int ebomTypeID,
      ConditionStructure[] conditionStructures,
      INodeQuerySupport nodeQuerySupport,
      MbomEditorControl mbomEditorControl)
      : base(nodeQuerySupport, ebomVersionID, ebomTypeID, RelatedObjectsRole.Composition, MbomConstants.EbomCompositionRelationTypeID, conditionStructures)
    {
      this._mbomEditorControl = mbomEditorControl != null ? mbomEditorControl : throw new ArgumentNullException(nameof (mbomEditorControl));
    }

    protected override void GetClientPluginsData(ref HybridDictionary hybridDictionary)
    {
      base.GetClientPluginsData(ref hybridDictionary);
      if (hybridDictionary == null)
        return;
      hybridDictionary[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) new long[2]
      {
        0L,
        1L
      };
    }

    protected override DataTable GetDataTable(DBRecordSetParams queryParams)
    {
      return MbomHelper.IsEbomObjectType(this.objTypeID) ? base.GetDataTable(queryParams) : (DataTable) null;
    }
  }

  public sealed class MbomDescriptor(long mbomVersionID) : Intermech.Navigator.DBObjects.Descriptor(mbomVersionID)
  {
    public override INode GetChild(INodeID nodeID)
    {
      if (!(nodeID is NodeID))
        return (INode) null;
      NodeID nodeId = (NodeID) nodeID;
      return (INode) new MbomEditorControl.MbomNode(nodeId.ObjectTypeID, nodeId.ObjectID, this.Services);
    }
  }

  public sealed class MbomNode : ObjectNode
  {
    public MbomNode(int mbomTypeID, long mbomVersionID, IServiceProvider serviceProvider)
      : base(mbomTypeID, mbomVersionID)
    {
      this.Services = serviceProvider;
    }

    protected override List<PartSlot> CreateFolderSlots()
    {
      return new List<PartSlot>()
      {
        new PartSlot(Guid.NewGuid(), (INodePart) new MbomEditorControl.MbomNodePart(this._objTypeID, this._objID, this.Services))
      };
    }
  }

  public sealed class MbomNodePart(
    int mbomTypeID,
    long mbomVersionID,
    IServiceProvider serviceProvider) : RelatedObjectsPart(mbomTypeID, mbomVersionID, RelatedObjectsRole.Composition, MbomHelper.GetRelationTypeIDForMbomOrSimilarObjectType(mbomTypeID), -1, (ConditionStructure[]) null, serviceProvider)
  {
    public override INode GetChild(INodeID nodeID)
    {
      if (nodeID is NodeID)
      {
        NodeID nodeId = (NodeID) nodeID;
        if (MbomHelper.IsMbomOrSimilarObjectType(nodeId.ObjectTypeID))
          return (INode) new MbomEditorControl.MbomNode(nodeId.ObjectTypeID, nodeId.ObjectID, this.Services);
        if (MbomHelper.IsEbomObjectType(nodeId.ObjectTypeID))
          return (INode) new ObjectNode(nodeId.ObjectTypeID, nodeId.ObjectID)
          {
            Services = this.Services
          };
      }
      return (INode) null;
    }

    protected override INodeQuery GetQuery(ConditionStructure[] conditions)
    {
      return (INodeQuery) new MbomEditorControl.MbomNodeQuery(this._objID, this._objTypeID, conditions, (INodeQuerySupport) this);
    }
  }

  public sealed class MbomNodeQuery(
    long mbomVersionID,
    int mbomTypeID,
    ConditionStructure[] conditionStructures,
    INodeQuerySupport nodeQuerySupport) : RelatedObjectsQuery(nodeQuerySupport, mbomVersionID, mbomTypeID, RelatedObjectsRole.Composition, MbomHelper.GetRelationTypeIDForMbomOrSimilarObjectType(mbomTypeID), conditionStructures)
  {
    protected override void GetClientPluginsData(ref HybridDictionary hybridDictionary)
    {
      base.GetClientPluginsData(ref hybridDictionary);
      if (hybridDictionary == null)
        return;
      hybridDictionary[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) new long[2]
      {
        0L,
        2L
      };
    }
  }
}
