// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionCopying.CompositionCopyingTreeControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Queries;
using Intermech.Navigator.VirtualNodes;
using Intermech.Search.CompositionContexts;
using Intermech.Search.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.CompositionCopying;

public sealed class CompositionCopyingTreeControl : UserControl
{
  private static readonly Guid CopyTag = new Guid("4B93EDF2-4F00-4513-A360-F2AEE68007A5");
  private static readonly Guid ExcludeTag = new Guid("D8DC935D-DDC5-4E41-A393-3CC610B90628");
  private IFiltrationService _filtrationService;
  private INavigatorClientService _navigatorClientService;
  private int[] _allowableForCreateCopyObjectTypes = new int[0];
  private long _objectVersionID;
  private long[] _instances = new long[0];
  private bool _fullComposition;
  private bool _considerInstances;
  private int[] _relationTypes = new int[0];
  private bool _suppressRebuildTree;
  private int[] _forbiddenForCreateCopyObjectTypes;
  private IContainer components;
  private NavigatorTreeView _navigatorTreeView;
  private Column _copyColumn;
  private CellEditor _checkBoxCellEditor;
  private CheckBox _checkBox;
  private Column _removeColumn;

  public CompositionCopyingTreeControl()
  {
    this.InitializeComponent();
    if (!this.Controls.Contains((Control) this._navigatorTreeView))
      this.Controls.Add((Control) this._navigatorTreeView);
    this._navigatorTreeView.DisableJobs = true;
    this.Controls.Remove((Control) this._checkBox);
    this._navigatorTreeView.Columns.ListChanged += new ListChangedEventHandler(this.NavigatorTreeViewColumns_ListChanged);
    this._navigatorTreeView.AfterExpand += new EventHandler<NodeEventArgs>(this.NavigatorTreeView_AfterExpand);
    this._navigatorTreeView.GetCellData += new GetCellDataHandler(this.NavigatorTreeView_GetCellData);
    this._navigatorTreeView.SelectionChanged += new EventHandler(this.NavigatorTreeView_SelectionChanged);
    this._navigatorTreeView.SetCellValue += new SetCellValueHandler(this.NavigatorTreeView_SetCellValue);
    this._navigatorTreeView.CommandsProvider = (ICommandsProvider) new CompositionCopyingTreeControl.CommandsProvider(this);
  }

  public event EventHandler SelectionChanged;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int[] AllowableForCreateCopyObjectTypes
  {
    get => this._allowableForCreateCopyObjectTypes;
    set
    {
      if (value == null || value.Length == 0 || ObjectTypeHelper.IsAnyUnknownObjectTypeID((IEnumerable<int>) value))
        throw new ArgumentException();
      if (this.IsItemsEqual((Array) this._allowableForCreateCopyObjectTypes, (Array) value))
        return;
      this._allowableForCreateCopyObjectTypes = ((IEnumerable<int>) value).Distinct<int>().ToArray<int>();
      this.RebuildTree();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long ObjectVersionID
  {
    get => this._objectVersionID;
    set
    {
      if (ObjectHelper.IsUnknownObjectVersionID(value))
        throw new ArgumentException();
      if (this._objectVersionID == value)
        return;
      this._objectVersionID = value;
      this._instances = new long[0];
      this.RebuildTree();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long[] Instances
  {
    get => this._instances;
    set
    {
      if (value == null || value.Length == 0 || ObjectHelper.IsAnyUnknownObjectVersionID((IEnumerable<long>) value))
        throw new ArgumentException();
      if (this.IsItemsEqual((Array) this._instances, (Array) value))
        return;
      this._instances = value;
      this.RebuildTree();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool ConsiderInstances
  {
    get => this._considerInstances;
    set
    {
      if (this._considerInstances == value)
        return;
      this._considerInstances = value;
      this.RebuildTree();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int[] RelationTypes
  {
    get => this._relationTypes;
    set
    {
      if (value == null || value.Length == 0 || RelationTypeHelper.IsAnyUnknownRelationTypeID((IEnumerable<int>) value))
        throw new ArgumentException();
      if (this.IsItemsEqual((Array) this._relationTypes, (Array) value))
        return;
      this._relationTypes = ((IEnumerable<int>) value).Distinct<int>().ToArray<int>();
      this.RebuildTree();
    }
  }

  public bool HasSelectedItems => this._navigatorTreeView.SelectedRow != null;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool SuppressRebuildTree
  {
    get => this._suppressRebuildTree;
    set
    {
      if (this._suppressRebuildTree == value)
        return;
      this._suppressRebuildTree = value;
      if (!this._suppressRebuildTree)
        return;
      this.RebuildTree();
    }
  }

  public void Initialize(
    IFiltrationService filtrationService,
    INavigatorClientService navigatorClientService)
  {
    if (filtrationService == null)
      throw new ArgumentNullException(nameof (filtrationService));
    if (navigatorClientService == null)
      throw new ArgumentNullException(nameof (navigatorClientService));
    this._filtrationService = filtrationService;
    this._navigatorClientService = navigatorClientService;
    this.SetDefaultColumns();
    this._navigatorTreeView.SupportedColumns = Utils.NavigatorColumns(NodeColumnSortOrder.Ascending);
    this._forbiddenForCreateCopyObjectTypes = CompositionCopyingHelper.GetForbiddenForCreateCopyObjectTypes();
  }

  public void FindNext(Regex regex)
  {
    if (regex == null)
      throw new ArgumentNullException(nameof (regex));
    NavigatorTreeNode navigatorTreeNode1 = ((IEnumerable<NavigatorTreeNode>) this._navigatorTreeView.SelectedNodes).LastOrDefault<NavigatorTreeNode>() ?? this._navigatorTreeView.RootNode;
    if (navigatorTreeNode1 == null)
      return;
    foreach (NavigatorTreeNode navigatorTreeNode2 in navigatorTreeNode1.GetAllNextThenAllPreviousThenSelf(true))
    {
      if (navigatorTreeNode2.IsMatch(regex))
      {
        navigatorTreeNode2.Focus();
        return;
      }
    }
    CompositionCopyingTreeControl.ShowNothingFoundMessage();
  }

  public void FindAll(Regex regex)
  {
    if (regex == null)
      throw new ArgumentNullException(nameof (regex));
    if (this._navigatorTreeView.RootNode == null)
      return;
    List<NavigatorTreeNode> navigatorTreeNodeList = new List<NavigatorTreeNode>();
    foreach (NavigatorTreeNode navigatorTreeNode in this._navigatorTreeView.RootNode.GetDescendantsAndSelf(true))
    {
      if (navigatorTreeNode.IsMatch(regex))
        navigatorTreeNodeList.Add(navigatorTreeNode);
    }
    if (navigatorTreeNodeList.Count == 0)
      CompositionCopyingTreeControl.ShowNothingFoundMessage();
    else
      this.SelectNodes(navigatorTreeNodeList.ToArray());
  }

  public void InverseSelectedCopies()
  {
    List<long> longList = new List<long>();
    foreach (NavigatorTreeNode node in (IEnumerable<NavigatorTreeNode>) this._navigatorTreeView.SelectedRows.Select<Row, NavigatorTreeNode>((System.Func<Row, NavigatorTreeNode>) (o => (NavigatorTreeNode) o.Item)).OrderByDescending<NavigatorTreeNode, int>((System.Func<NavigatorTreeNode, int>) (o => o.Level)))
    {
      if (node.NodeID is NodeID nodeId && !longList.Contains(nodeId.ObjectID))
      {
        if (this.IsCopy(node))
          this.RemoveCopy(node);
        else
          this.AddCopy(node);
        longList.Add(nodeId.ObjectID);
      }
    }
  }

  public long[] GetCopies()
  {
    List<long> source = new List<long>();
    if (this._navigatorTreeView.RootNode != null)
    {
      foreach (NavigatorTreeNode node in this._navigatorTreeView.RootNode.GetDescendantsAndSelf())
      {
        if (this.IsCopy(node) && node.NodeID is NodeID)
          source.Add(((NodeID) node.NodeID).ObjectID);
      }
    }
    return source.Distinct<long>().ToArray<long>();
  }

  public long[] GetExcluded()
  {
    List<long> longList = new List<long>();
    if (this._navigatorTreeView.RootNode != null)
    {
      foreach (NavigatorTreeNode node in this._navigatorTreeView.RootNode.GetDescendantsAndSelf())
      {
        if (this.IsExcluded(node) && node.NodeID is NodeID)
          longList.Add(((NodeID) node.NodeID).PrjLinkID);
      }
    }
    return longList.ToArray();
  }

  public object CreateMemento()
  {
    return (object) new CompositionCopyingTreeControl.CompositionCopyingTreeMemento()
    {
      Columns = this._navigatorTreeView.ReflectTreeColumsChanges()
    };
  }

  public void SetMemento(object memento)
  {
    CompositionCopyingTreeControl.CompositionCopyingTreeMemento copyingTreeMemento = memento is CompositionCopyingTreeControl.CompositionCopyingTreeMemento ? (CompositionCopyingTreeControl.CompositionCopyingTreeMemento) memento : throw new ArgumentException();
    if (copyingTreeMemento.Columns == null)
      return;
    this.SetColumns(copyingTreeMemento.Columns);
  }

  public void ChangeColumns()
  {
    this.SetColumns(this._navigatorClientService.ChangeColumns(this._navigatorTreeView.ReflectTreeColumsChanges(), this._navigatorTreeView.SupportedColumns));
  }

  private void NavigatorTreeViewColumns_ListChanged(object sender, ListChangedEventArgs e)
  {
    this._navigatorTreeView.Columns.ListChanged -= new ListChangedEventHandler(this.NavigatorTreeViewColumns_ListChanged);
    try
    {
      if (this._navigatorTreeView.Columns.Contains(this._copyColumn))
        this._navigatorTreeView.Columns.Remove(this._copyColumn);
      this._navigatorTreeView.Columns.Add(this._copyColumn);
      if (this._navigatorTreeView.Columns.Contains(this._removeColumn))
        this._navigatorTreeView.Columns.Remove(this._removeColumn);
      this._navigatorTreeView.Columns.Add(this._removeColumn);
    }
    finally
    {
      this._navigatorTreeView.Columns.ListChanged += new ListChangedEventHandler(this.NavigatorTreeViewColumns_ListChanged);
    }
  }

  private void NavigatorTreeView_AfterExpand(object sender, NodeEventArgs e)
  {
    foreach (long copy in this.GetCopies())
    {
      foreach (NavigatorTreeNode child in (List<NavigatorTreeNode>) e.Node.Children)
      {
        if (this.GetObjectVersionID(child) == copy)
          this.AddCopy(child);
      }
    }
    if (!this.IsExcluded(e.Node))
      return;
    this.Exclude(e.Node);
  }

  private void NavigatorTreeView_GetCellData(object sender, GetCellDataEventArgs e)
  {
    if (!(e.Row.Item is NavigatorTreeNode))
      return;
    NavigatorTreeNode node = (NavigatorTreeNode) e.Row.Item;
    bool flag = this.IsExcluded(node);
    if (flag)
    {
      e.CellData.EvenStyle = new Style(e.CellData.EvenStyle, new StyleDelta()
      {
        ForeColor = Color.Gray,
        Font = new Font(e.CellData.EvenStyle.Font, FontStyle.Strikeout)
      });
      e.CellData.OddStyle = new Style(e.CellData.OddStyle, new StyleDelta()
      {
        ForeColor = Color.Gray,
        Font = new Font(e.CellData.OddStyle.Font, FontStyle.Strikeout)
      });
    }
    if (e.Column == this._copyColumn)
    {
      if (node.NodeID is NodeID)
        e.CellData.Value = (object) this.IsCopy(node);
      else
        e.CellData.Editor = (CellEditor) null;
    }
    else
    {
      if (e.Column != this._removeColumn)
        return;
      if (node.NodeID is NodeID && !this.IsRootNode(node))
        e.CellData.Value = (object) flag;
      else
        e.CellData.Editor = (CellEditor) null;
    }
  }

  private void NavigatorTreeView_SelectionChanged(object sender, EventArgs e)
  {
    EventHandler selectionChanged = this.SelectionChanged;
    if (selectionChanged == null)
      return;
    selectionChanged((object) this, EventArgs.Empty);
  }

  private void NavigatorTreeView_SetCellValue(object sender, SetCellValueEventArgs e)
  {
    if (!(e.Row.Item is NavigatorTreeNode))
      return;
    NavigatorTreeNode node = (NavigatorTreeNode) e.Row.Item;
    if (!this.IsExcluded(node.Parent))
    {
      if (e.Column == this._copyColumn)
      {
        if (object.Equals(e.NewValue, (object) true))
          this.AddCopy(node);
        else
          this.RemoveCopy(node);
      }
      else
      {
        if (e.Column != this._removeColumn)
          return;
        if (object.Equals(e.NewValue, (object) true))
          this.Exclude(node);
        else
          this.Include(node);
      }
    }
    else
      e.Cancel = true;
  }

  private bool IsRootNode(NavigatorTreeNode node) => node == node.Tree.RootNode;

  private bool IsExcluded(NavigatorTreeNode node)
  {
    return object.Equals(node.Tag, (object) CompositionCopyingTreeControl.ExcludeTag);
  }

  private void Exclude(NavigatorTreeNode node)
  {
    this.AddCopy(node.Parent);
    foreach (NavigatorTreeNode node1 in node.GetDescendantsAndSelf())
    {
      node1.Tag = (object) CompositionCopyingTreeControl.ExcludeTag;
      this.UpdateNode(node1);
    }
  }

  private void Include(NavigatorTreeNode node)
  {
    foreach (NavigatorTreeNode node1 in node.GetDescendantsAndSelf())
    {
      node1.Tag = (object) (this.IsCopy(node1) ? CompositionCopyingTreeControl.CopyTag : Guid.Empty);
      this.UpdateNode(node1);
    }
  }

  private void RebuildTree()
  {
    if (this._relationTypes.Length == 0 || !this._navigatorTreeView.Columns.Any<Column>((System.Func<Column, bool>) (o => o != this._copyColumn && o != this._removeColumn)) || this.SuppressRebuildTree)
      return;
    this.InvokeAndKeepState((Action) (() =>
    {
      if (this._considerInstances && this._instances.Length != 0)
        this._navigatorTreeView.Build((IDescriptor) new CompositionCopyingTreeControl.ObjectWithInstancesDescriptor(this._objectVersionID, this._instances, this));
      else
        this._navigatorTreeView.Build((IDescriptor) new CompositionCopyingTreeControl.ObjectDescriptor(this._objectVersionID, this));
    }));
  }

  private void InvokeAndKeepState(Action action)
  {
    INodeID[][] expandedPathes = this.GetExpandedPathes();
    INodeID[][] selectedPathes = this.GetSelectedPathes();
    INodeID[][] copiesPathes = this.GetCopiesPathes();
    try
    {
      action();
    }
    finally
    {
      this.ExpandRootNode();
      this.ExpandPathes(this.CorrectPathes(expandedPathes));
      this.SelectPathes(this.CorrectPathes(selectedPathes));
      this.SelectRootNode();
      this.SetObligatoryCopies();
      this.SetCopiesPathes(this.CorrectPathes(copiesPathes));
    }
  }

  private void SelectRootNode()
  {
    if (this._navigatorTreeView.RootNode == null || this._navigatorTreeView.SelectedRow != null)
      return;
    this._navigatorTreeView.RootNode.Focus();
  }

  private INodeID[][] GetExpandedPathes()
  {
    List<INodeID[]> pathes = new List<INodeID[]>();
    if (this._navigatorTreeView.RootNode != null)
    {
      if (this._navigatorTreeView.RootNode.Expanded && this._navigatorTreeView.RootNode.Children.Count > 0)
        pathes.Add(new INodeID[1]
        {
          this._navigatorTreeView.RootNode.NodeID
        });
      this.GatherExpandedPathes(this._navigatorTreeView.RootNode, pathes);
    }
    return pathes.ToArray();
  }

  private void GatherExpandedPathes(NavigatorTreeNode node, List<INodeID[]> pathes)
  {
    foreach (NavigatorTreeNode child in (List<NavigatorTreeNode>) node.Children)
    {
      if (child.Expanded && child.Children.Count > 0)
      {
        pathes.Add(child.GetPath());
        this.GatherExpandedPathes(child, pathes);
      }
    }
  }

  private INodeID[][] GetSelectedPathes()
  {
    return ((IEnumerable<NavigatorTreeNode>) this._navigatorTreeView.SelectedNodes).Select<NavigatorTreeNode, INodeID[]>((System.Func<NavigatorTreeNode, INodeID[]>) (o => o.GetPath())).ToArray<INodeID[]>();
  }

  private INodeID[][] GetCopiesPathes()
  {
    List<INodeID[]> nodeIdArrayList = new List<INodeID[]>();
    if (this._navigatorTreeView.RootNode != null)
    {
      foreach (NavigatorTreeNode node in this._navigatorTreeView.RootNode.GetDescendantsAndSelf())
      {
        if (this.IsCopy(node))
          nodeIdArrayList.Add(node.GetPath());
      }
    }
    return nodeIdArrayList.ToArray();
  }

  private INodeID[][] CorrectPathes(INodeID[][] pathes)
  {
    return ((IEnumerable<INodeID[]>) pathes).Select<INodeID[], INodeID[]>((System.Func<INodeID[], INodeID[]>) (o => this.CorrectPath(o))).Where<INodeID[]>((System.Func<INodeID[], bool>) (o => o.Length != 0)).ToArray<INodeID[]>();
  }

  private INodeID[] CorrectPath(INodeID[] path)
  {
    if (this._navigatorTreeView.RootNode != null)
    {
      if (this._navigatorTreeView.RootNode.NodeID is HiveNodeID && !(path[0] is HiveNodeID))
      {
        List<INodeID> nodeIdList = new List<INodeID>();
        nodeIdList.Add(this._navigatorTreeView.RootNode.NodeID);
        nodeIdList.AddRange((IEnumerable<INodeID>) path);
        return nodeIdList.ToArray();
      }
      if (!(this._navigatorTreeView.RootNode.NodeID is HiveNodeID) && path[0] is HiveNodeID)
      {
        List<INodeID> nodeIdList = new List<INodeID>();
        nodeIdList.AddRange(((IEnumerable<INodeID>) path).Skip<INodeID>(1));
        return nodeIdList.ToArray();
      }
    }
    return path;
  }

  private void ExpandRootNode()
  {
    if (this._navigatorTreeView.RootNode == null)
      return;
    this.ExpandNode(this._navigatorTreeView.RootNode);
  }

  private void ExpandPathes(INodeID[][] pathes)
  {
    foreach (INodeID[] pathe in pathes)
      this.ExpandPath(pathe);
  }

  private void ExpandPath(INodeID[] path)
  {
    if (this._navigatorTreeView.RootNode == null || !object.Equals((object) this._navigatorTreeView.RootNode.NodeID, (object) path[0]))
      return;
    NavigatorTreeNode node = this._navigatorTreeView.RootNode;
    this.ExpandNode(node);
    foreach (INodeID nodeId in ((IEnumerable<INodeID>) path).Skip<INodeID>(1))
    {
      INodeID nodeID = nodeId;
      node = node.Children.FirstOrDefault<NavigatorTreeNode>((System.Func<NavigatorTreeNode, bool>) (o => object.Equals((object) o.NodeID, (object) nodeID)));
      if (node == null)
        break;
      this.ExpandNode(node);
    }
  }

  private void ExpandNode(NavigatorTreeNode node)
  {
    if (node.Expanded)
      return;
    node.Fetch();
    if (node.Handle == null)
      return;
    node.Handle.EnsureVisible();
    node.Handle.Expand();
  }

  private void SelectPathes(INodeID[][] pathes)
  {
    this._navigatorTreeView.SelectedRows.Clear();
    foreach (INodeID[] pathe in pathes)
      this.SelectPath(pathe);
  }

  private void SelectPath(INodeID[] path)
  {
    if (this._navigatorTreeView.RootNode == null || !object.Equals((object) this._navigatorTreeView.RootNode.NodeID, (object) path[0]))
      return;
    NavigatorTreeNode navigatorTreeNode = this._navigatorTreeView.RootNode;
    foreach (INodeID nodeId in ((IEnumerable<INodeID>) path).Skip<INodeID>(1))
    {
      INodeID nodeID = nodeId;
      navigatorTreeNode = navigatorTreeNode.Children.FirstOrDefault<NavigatorTreeNode>((System.Func<NavigatorTreeNode, bool>) (o => object.Equals((object) o.NodeID, (object) nodeID)));
      if (navigatorTreeNode == null)
        break;
    }
    if (navigatorTreeNode == null || navigatorTreeNode.Handle == null)
      return;
    navigatorTreeNode.Handle.Selected = true;
  }

  private void SetCopiesPathes(INodeID[][] pathes)
  {
    foreach (INodeID[] pathe in pathes)
      this.SetCopyPath(pathe);
  }

  private void SetCopyPath(INodeID[] path)
  {
    if (this._navigatorTreeView.RootNode == null || !object.Equals((object) this._navigatorTreeView.RootNode.NodeID, (object) path[0]))
      return;
    NavigatorTreeNode node = this._navigatorTreeView.RootNode;
    for (int index = 1; index < path.Length; ++index)
    {
      INodeID nodeID = path[index];
      node.Fetch();
      node = node.Children.FirstOrDefault<NavigatorTreeNode>((System.Func<NavigatorTreeNode, bool>) (o => object.Equals((object) o.NodeID, (object) nodeID)));
      if (node == null)
        break;
      if (index == path.Length - 1)
        this.AddCopy(node);
    }
  }

  private void SetObligatoryCopies()
  {
    if (this._navigatorTreeView.RootNode == null)
      return;
    foreach (NavigatorTreeNode node in this._navigatorTreeView.RootNode.GetDescendantsAndSelf().Where<NavigatorTreeNode>((System.Func<NavigatorTreeNode, bool>) (o => this.IsObligatoryCopy(o))))
    {
      this.AddCopy(node);
      this.UpdateNode(node);
    }
  }

  private long GetObjectVersionID(NavigatorTreeNode node)
  {
    return node.NodeID is NodeID ? ((NodeID) node.NodeID).ObjectID : 0L;
  }

  private void SetDefaultColumns()
  {
    NodeColumnCollection columns = new NodeColumnCollection();
    NodeColumn nodeColumn1 = this._navigatorClientService.CreateNodeColumn(ObligatoryObjectAttributes.CAPTION);
    nodeColumn1.Width = 250;
    columns.Add(nodeColumn1);
    NodeColumn nodeColumn2 = this._navigatorClientService.CreateNodeColumn(ObligatoryObjectAttributes.F_VERSION_ID);
    nodeColumn2.Width = 100;
    columns.Add(nodeColumn2);
    this.SetColumns(columns);
  }

  private void SetColumns(NodeColumnCollection columns)
  {
    this.InvokeAndKeepState((Action) (() => this._navigatorTreeView.SetColumns(columns)));
  }

  private static void ShowNothingFoundMessage()
  {
    int num = (int) MessageBox.Show("Поиск завершен. Ничего не найдено.", "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
  }

  private void SelectNodes(NavigatorTreeNode[] nodes)
  {
    this._navigatorTreeView.SelectedRow = (Row) null;
    for (int index = 0; index < nodes.Length; ++index)
      nodes[index].Select();
  }

  public bool CanBeCopy(NavigatorTreeNode node)
  {
    return !((IEnumerable<int>) this._forbiddenForCreateCopyObjectTypes).Contains<int>(this.GetObjectType(node));
  }

  private int GetObjectType(NavigatorTreeNode node)
  {
    return !(node.NodeID is NodeID nodeId) ? -1 : nodeId.ObjectTypeID;
  }

  private bool IsCopy(NavigatorTreeNode node)
  {
    return object.Equals(node.Tag, (object) CompositionCopyingTreeControl.CopyTag);
  }

  private void AddCopy(NavigatorTreeNode node)
  {
    NavigatorTreeNode[] array = node.GetAncestorsAndSelf().ToArray<NavigatorTreeNode>();
    if (((IEnumerable<NavigatorTreeNode>) array).All<NavigatorTreeNode>((System.Func<NavigatorTreeNode, bool>) (o => this.CanBeCopy(o) && this.CheckObjectReferenceAssociatedWithDocumentElement(o))))
    {
      foreach (NavigatorTreeNode firstNode in array)
      {
        foreach (NavigatorTreeNode navigatorTreeNode in this._navigatorTreeView.RootNode.GetDescendantsAndSelf())
        {
          if (this.IsObjectVersionIdsEquals(firstNode, navigatorTreeNode) && !this.IsCopy(navigatorTreeNode))
          {
            navigatorTreeNode.Tag = (object) CompositionCopyingTreeControl.CopyTag;
            this.UpdateNode(navigatorTreeNode);
            this.AddCopy(navigatorTreeNode);
          }
        }
      }
    }
    else
      this.UpdateNode(node);
  }

  private bool CheckObjectReferenceAssociatedWithDocumentElement(NavigatorTreeNode node)
  {
    if (node.NodeID is NodeID)
    {
      NodeID nodeId = (NodeID) node.NodeID;
      if (CompositionCopyingHelper.IsDocument(nodeId.ObjectTypeID))
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
          return ((ICompositionCopyingServerService) sessionKeeper.Session.GetCustomService(typeof (ICompositionCopyingServerService))).CheckObjectReferenceAssociatedWithDocumentElement(sessionKeeper.Session.SessionGUID, nodeId.ObjectID);
      }
    }
    return true;
  }

  private bool IsObjectVersionIdsEquals(NavigatorTreeNode firstNode, NavigatorTreeNode secondNode)
  {
    NodeID nodeId1 = firstNode.NodeID as NodeID;
    NodeID nodeId2 = secondNode.NodeID as NodeID;
    return nodeId1 != null && nodeId2 != null && nodeId1.ObjectID == nodeId2.ObjectID;
  }

  private void UpdateNode(NavigatorTreeNode node)
  {
    if (node.Handle == null)
      return;
    this._navigatorTreeView.UpdateRowData(node.Handle);
  }

  private void RemoveCopy(NavigatorTreeNode node)
  {
    IEnumerable<NavigatorTreeNode> objectVersionIdSameAs = this.GetAllNodesWithObjectVersionIDSameAs(node);
    if (objectVersionIdSameAs.All<NavigatorTreeNode>((System.Func<NavigatorTreeNode, bool>) (o => !this.IsAnyChildCopy(o))) && !this.IsObligatoryCopy(node))
    {
      foreach (NavigatorTreeNode node1 in objectVersionIdSameAs)
      {
        if (this.IsCopy(node1))
        {
          node1.Tag = (object) null;
          this.UpdateNode(node1);
        }
      }
    }
    else
      this.UpdateNode(node);
  }

  private bool IsObligatoryCopy(NavigatorTreeNode node)
  {
    long objectVersionId = this.GetObjectVersionID(node);
    if (this._objectVersionID == objectVersionId)
      return true;
    return this._considerInstances && ((IEnumerable<long>) this._instances).Contains<long>(objectVersionId);
  }

  private IEnumerable<NavigatorTreeNode> GetAllNodesWithObjectVersionIDSameAs(NavigatorTreeNode node)
  {
    foreach (NavigatorTreeNode secondNode in this._navigatorTreeView.RootNode.GetDescendantsAndSelf())
    {
      if (this.IsObjectVersionIdsEquals(node, secondNode))
        yield return secondNode;
    }
  }

  private bool IsAnyChildCopy(NavigatorTreeNode node)
  {
    return node.Children.Any<NavigatorTreeNode>((System.Func<NavigatorTreeNode, bool>) (o => this.IsCopy(o)));
  }

  private bool IsItemsEqual(Array firstArray, Array secondArray)
  {
    if (!object.Equals((object) firstArray, (object) secondArray))
    {
      foreach (object first in firstArray)
      {
        if (Array.IndexOf(secondArray, first) < 0)
          return false;
      }
      foreach (object second in secondArray)
      {
        if (Array.IndexOf(firstArray, second) < 0)
          return false;
      }
    }
    return true;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this._copyColumn = new Column();
    this._checkBoxCellEditor = new CellEditor();
    this._checkBox = new CheckBox();
    this._navigatorTreeView = new NavigatorTreeView();
    this._removeColumn = new Column();
    this._navigatorTreeView.BeginInit();
    this.SuspendLayout();
    this._copyColumn.Caption = "Копия";
    this._copyColumn.CellEditor = this._checkBoxCellEditor;
    this._copyColumn.CellStyle.HorzAlignment = StringAlignment.Center;
    this._copyColumn.Movable = false;
    this._copyColumn.Name = "_copyColumn";
    this._copyColumn.Resizable = false;
    this._copyColumn.Sortable = false;
    this._checkBoxCellEditor.CellAlignment = ContentAlignment.MiddleCenter;
    this._checkBoxCellEditor.Control = (Control) this._checkBox;
    this._checkBoxCellEditor.DisplayMode = CellEditorDisplayMode.Always;
    this._checkBoxCellEditor.UseCellColors = false;
    this._checkBoxCellEditor.UseCellFont = false;
    this._checkBoxCellEditor.UseCellHeight = false;
    this._checkBoxCellEditor.UseCellWidth = false;
    this._checkBox.CheckAlign = ContentAlignment.MiddleCenter;
    this._checkBox.Cursor = Cursors.Default;
    this._checkBox.FlatAppearance.BorderSize = 0;
    this._checkBox.FlatStyle = FlatStyle.System;
    this._checkBox.Location = new Point(0, 0);
    this._checkBox.Margin = new Padding(0);
    this._checkBox.Name = "_checkBox";
    this._checkBox.Size = new Size(13, 13);
    this._checkBox.TabIndex = 0;
    this._checkBox.TextAlign = ContentAlignment.MiddleCenter;
    this._checkBox.UseVisualStyleBackColor = true;
    this._checkBox.Visible = false;
    this._navigatorTreeView.AllowDrop = true;
    this._navigatorTreeView.AllowUserPinnedColumns = false;
    this._navigatorTreeView.Columns.Add(this._copyColumn);
    this._navigatorTreeView.Columns.Add(this._removeColumn);
    this._navigatorTreeView.DisableCheckedOutColumn = true;
    this._navigatorTreeView.DisableDragAndDrop = true;
    this._navigatorTreeView.DisableIMContextMenu = true;
    this._navigatorTreeView.Dock = DockStyle.Fill;
    this._navigatorTreeView.Editors.Add(this._checkBoxCellEditor);
    this._navigatorTreeView.HeaderStyle.HorzAlignment = StringAlignment.Near;
    this._navigatorTreeView.ImageList = (ImageList) null;
    this._navigatorTreeView.LineStyle = LineStyle.Dot;
    this._navigatorTreeView.Location = new Point(0, 0);
    this._navigatorTreeView.MultiSelect = true;
    this._navigatorTreeView.Name = "_navigatorTreeView";
    this._navigatorTreeView.RowEvenStyle.WordWrap = false;
    this._navigatorTreeView.RowOddStyle.WordWrap = false;
    this._navigatorTreeView.RowSelectedStyle.WordWrap = false;
    this._navigatorTreeView.RowStyle.BorderColor = SystemColors.Control;
    this._navigatorTreeView.RowStyle.BorderStyle = Border3DStyle.Adjust;
    this._navigatorTreeView.RowStyle.BorderWidth = 1;
    this._navigatorTreeView.RowStyle.WordWrap = false;
    this._navigatorTreeView.SelectBeforeEdit = true;
    this._navigatorTreeView.ShowRootRow = false;
    this._navigatorTreeView.Size = new Size(0, 0);
    this._navigatorTreeView.SuppressErrorMessages = true;
    this._navigatorTreeView.TabIndex = 0;
    this._removeColumn.Caption = "Удалить";
    this._removeColumn.CellEditor = this._checkBoxCellEditor;
    this._removeColumn.CellStyle.HorzAlignment = StringAlignment.Center;
    this._removeColumn.Movable = false;
    this._removeColumn.Name = "_removeColumn";
    this._removeColumn.Resizable = false;
    this._removeColumn.Sortable = false;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._checkBox);
    this.Name = nameof (CompositionCopyingTreeControl);
    this.Size = new Size(592, 423);
    this._navigatorTreeView.EndInit();
    this.ResumeLayout(false);
  }

  [Serializable]
  private sealed class CompositionCopyingTreeMemento
  {
    public NodeColumnCollection Columns { get; set; }
  }

  private sealed class ObjectDescriptor : Intermech.Navigator.DBObjects.Descriptor
  {
    private CompositionCopyingTreeControl _tree;

    public ObjectDescriptor(long objectVersionID, CompositionCopyingTreeControl tree)
      : base(objectVersionID)
    {
      this._tree = tree != null ? tree : throw new ArgumentNullException(nameof (tree));
    }

    public override INode GetChild(INodeID nodeID)
    {
      return (INode) new CompositionCopyingTreeControl.ObjectNode(this._realObjID, this._tree);
    }
  }

  private sealed class ObjectNode : Intermech.Navigator.DBObjects.ObjectNode
  {
    private CompositionCopyingTreeControl _tree;

    public ObjectNode(long objectVersionID, CompositionCopyingTreeControl tree)
      : base(CompositionCopyingTreeControl.ObjectNode.GetObjectTypeID(objectVersionID), objectVersionID)
    {
      this._tree = tree != null ? tree : throw new ArgumentNullException(nameof (tree));
    }

    protected override List<PartSlot> CreateFolderSlots()
    {
      return new List<PartSlot>()
      {
        new PartSlot(Guid.NewGuid(), (INodePart) new CompositionCopyingTreeControl.ObjectNodePart(this._objID, this._objTypeID, this.Services, this._tree))
      };
    }

    protected override List<PartSlot> CreateNonFolderSlots() => new List<PartSlot>(0);

    private static int GetObjectTypeID(long objectVersionID)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        return sessionKeeper.Session.GetObject(objectVersionID).ObjectType;
    }
  }

  private sealed class ObjectNodePart : RelatedObjectsPart
  {
    private CompositionCopyingTreeControl _tree;

    public ObjectNodePart(
      long objectVersionID,
      int objectTypeID,
      IServiceProvider serviceProvider,
      CompositionCopyingTreeControl tree)
      : base(objectTypeID, objectVersionID, RelatedObjectsRole.Composition, serviceProvider)
    {
      this._tree = tree != null ? tree : throw new ArgumentNullException(nameof (tree));
    }

    public override INode GetChild(INodeID nodeID)
    {
      return nodeID is NodeID nodeId && ((IEnumerable<int>) this._tree.AllowableForCreateCopyObjectTypes).Contains<int>(nodeId.ObjectTypeID) ? (INode) new CompositionCopyingTreeControl.ObjectNode(((NodeID) nodeID).ObjectID, this._tree) : (INode) null;
    }

    protected override INodeQuery GetQuery(ConditionStructure[] conditions)
    {
      return (INodeQuery) new CompositionCopyingTreeControl.ObjectNodeQuery((INodeQuerySupport) this, this._objID, this._objTypeID, conditions, this._tree);
    }

    public override INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
    {
      return base.CreateNodeId(fieldValues, adapter);
    }
  }

  private sealed class ObjectNodeQuery : RelatedObjectsQuery
  {
    private CompositionCopyingTreeControl _tree;

    public ObjectNodeQuery(
      INodeQuerySupport nodeQuerySupport,
      long objectVersionID,
      int objectTypeID,
      ConditionStructure[] conditions,
      CompositionCopyingTreeControl tree)
      : base(nodeQuerySupport, objectVersionID, objectTypeID, RelatedObjectsRole.Composition, -1, conditions)
    {
      this._tree = tree;
    }

    protected override DataTable GetDataTable(DBRecordSetParams queryParams)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        ICompositionCopyingServerService customService = (ICompositionCopyingServerService) sessionKeeper.Session.GetCustomService(typeof (ICompositionCopyingServerService));
        FindCompositionParams compositionParams = new FindCompositionParams(this.objId)
        {
          CompositionContexts = this.GetCompositonContexts(),
          FiltrationOwnerID = this._tree._filtrationService.FiltrationServiceOwnerID,
          RecordSetParams = queryParams,
          RelationTypes = this._tree.RelationTypes
        };
        Guid sessionGuid = sessionKeeper.Session.SessionGUID;
        FindCompositionParams @params = compositionParams;
        return customService.FindComposition(sessionGuid, @params);
      }
    }

    private CompositionContext[] GetCompositonContexts()
    {
      return this._tree._filtrationService.Filtration.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] is IEnumerable tag ? CompositionContextClientHelper.BuildCompositionContextsBasedOnValues(tag.Cast<object>().Select<object, long>((System.Func<object, long>) (o => Convert.ToInt64(o)))) : (CompositionContext[]) null;
    }
  }

  private sealed class ObjectWithInstancesDescriptor(
    long objectVersionID,
    long[] instances,
    CompositionCopyingTreeControl tree) : Intermech.Navigator.CustomNode.Descriptor(Intermech.Navigator.Consts.CategoryCustomNode, 0, "Исполнения", new DescriptorCollection((IEnumerable<IDescriptor>) CompositionCopyingTreeControl.ObjectWithInstancesDescriptor.CreateObjectDescriptors(objectVersionID, instances, tree)))
  {
    private static CompositionCopyingTreeControl.ObjectDescriptor[] CreateObjectDescriptors(
      long objectVersionID,
      long[] objectVersionIds,
      CompositionCopyingTreeControl tree)
    {
      if (ObjectHelper.IsUnknownObjectVersionID(objectVersionID))
        throw new ArgumentException();
      if (objectVersionIds == null || objectVersionIds.Length == 0 || ObjectHelper.IsAnyUnknownObjectVersionID((IEnumerable<long>) objectVersionIds))
        throw new ArgumentException();
      if (tree == null)
        throw new ArgumentNullException(nameof (tree));
      List<CompositionCopyingTreeControl.ObjectDescriptor> objectDescriptorList = new List<CompositionCopyingTreeControl.ObjectDescriptor>();
      objectDescriptorList.Add(new CompositionCopyingTreeControl.ObjectDescriptor(objectVersionID, tree));
      objectDescriptorList.AddRange(((IEnumerable<long>) objectVersionIds).Select<long, CompositionCopyingTreeControl.ObjectDescriptor>((System.Func<long, CompositionCopyingTreeControl.ObjectDescriptor>) (o => new CompositionCopyingTreeControl.ObjectDescriptor(o, tree))));
      return objectDescriptorList.ToArray();
    }
  }

  public sealed class CommandsProvider : ICommandsProvider
  {
    private CompositionCopyingTreeControl _tree;

    public CommandsProvider(CompositionCopyingTreeControl tree)
    {
      this._tree = tree != null ? tree : throw new ArgumentNullException(nameof (tree));
    }

    public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
    {
      return CommandsInfo.Empty;
    }

    public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
    {
      CommandsInfo groupCommands = new CommandsInfo();
      groupCommands.Add("SetupColumns", new CommandInfo(64 /*0x40*/, new ClickEventHandler(this.SetupColumns)));
      return groupCommands;
    }

    private void SetupColumns(
      ISelectedItems items,
      IServiceProvider viewServices,
      object additionalInfo)
    {
      this._tree.ChangeColumns();
    }
  }
}
