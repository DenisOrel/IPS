// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mbom.MbomNavigatorTreeView
// Assembly: Intermech.Mbom, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 13559C9A-4DBC-479B-BA71-AFEA0247DEC7
// Assembly location: D:\IPS\Client\Intermech.Mbom.dll
// XML documentation location: D:\IPS\Client\Intermech.Mbom.xml

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Mbom;

public sealed class MbomNavigatorTreeView : NavigatorTreeView
{
  public event EventHandler<MbomNavigatorTreeView.GetNodeDragDropEffectsEventArgs> GetNodeDragDropEffects;

  public event EventHandler<MbomNavigatorTreeView.NodeDropEventArgs> NodeDrop;

  protected override DragDropEffects RowDropEffect(
    Row row,
    RowDropLocation dropLocation,
    IDataObject data)
  {
    return row.Item is NavigatorTreeNode ? this.GetDragDropEffectsForNode((NavigatorTreeNode) row.Item, data) : DragDropEffects.None;
  }

  public override void SetColumns(
    NodeColumnCollection nodeColumnCollection,
    bool equalsWithExistingColumns = true)
  {
    Column[] array = this.Columns.Where<Column>((Func<Column, bool>) (o => !(o is NavigatorTreeColumn))).ToArray<Column>();
    try
    {
      base.SetColumns(nodeColumnCollection, equalsWithExistingColumns);
    }
    finally
    {
      foreach (Column column in array)
        this.Columns.Add(column);
    }
  }

  protected override void TreeDragDrop(object sender, DragEventArgs e)
  {
    NavigatorTreeNode node = this.GetNode(e.X, e.Y);
    if (node == null)
      return;
    this.OnNodeDrop(node, e.Data);
  }

  protected override void TreeDragEnter(object sender, DragEventArgs e)
  {
  }

  protected override void TreeDragOver(object sender, DragEventArgs e)
  {
  }

  private NavigatorTreeNode GetNode(int x, int y)
  {
    Point client = this.PointToClient(new Point(x, y));
    Widget widget = this.GetWidget(client.X, client.Y);
    Row row = (Row) null;
    if (widget is Infralution.Controls.VirtualTree.RowWidget)
      row = ((Infralution.Controls.VirtualTree.RowWidget) widget).Row;
    else if (widget is CellWidget)
      row = ((CellWidget) widget).Row;
    return row == null ? (NavigatorTreeNode) null : row.Item as NavigatorTreeNode;
  }

  private DragDropEffects GetDragDropEffectsForNode(NavigatorTreeNode node, IDataObject dataObject)
  {
    EventHandler<MbomNavigatorTreeView.GetNodeDragDropEffectsEventArgs> nodeDragDropEffects = this.GetNodeDragDropEffects;
    if (nodeDragDropEffects == null)
      return DragDropEffects.None;
    MbomNavigatorTreeView.GetNodeDragDropEffectsEventArgs e = new MbomNavigatorTreeView.GetNodeDragDropEffectsEventArgs(node, dataObject);
    nodeDragDropEffects((object) this, e);
    return e.DragDropEffects;
  }

  private void OnNodeDrop(NavigatorTreeNode node, IDataObject dataObject)
  {
    EventHandler<MbomNavigatorTreeView.NodeDropEventArgs> nodeDrop = this.NodeDrop;
    if (nodeDrop == null)
      return;
    nodeDrop((object) this, new MbomNavigatorTreeView.NodeDropEventArgs(node, dataObject));
  }

  public sealed class GetNodeDragDropEffectsEventArgs
  {
    public GetNodeDragDropEffectsEventArgs(NavigatorTreeNode node, IDataObject dataObject)
    {
      if (node == null)
        throw new ArgumentNullException(nameof (node));
      if (dataObject == null)
        throw new ArgumentNullException(nameof (dataObject));
      this.Node = node;
      this.DataObject = dataObject;
    }

    public NavigatorTreeNode Node { get; private set; }

    public IDataObject DataObject { get; private set; }

    public DragDropEffects DragDropEffects { get; set; }
  }

  public sealed class NodeDropEventArgs
  {
    public NodeDropEventArgs(NavigatorTreeNode node, IDataObject dataObject)
    {
      if (node == null)
        throw new ArgumentNullException(nameof (node));
      if (dataObject == null)
        throw new ArgumentNullException(nameof (dataObject));
      this.Node = node;
      this.DataObject = dataObject;
    }

    public NavigatorTreeNode Node { get; private set; }

    public IDataObject DataObject { get; private set; }
  }
}
