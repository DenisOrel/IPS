// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.ManufactOrdersEditorNavigatorTreeView
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Infralution.Controls.VirtualTree;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP.Orders;

internal sealed class ManufactOrdersEditorNavigatorTreeView : NavigatorTreeView
{
  private bool _isFetch;

  protected override void TreeGetChildren(object sender, GetChildrenEventArgs e)
  {
    NavigatorTreeNode navigatorTreeNode = e.Row.Item as NavigatorTreeNode;
    if (!navigatorTreeNode.Full)
    {
      if (!this._isFetch)
      {
        try
        {
          this._isFetch = true;
          navigatorTreeNode.ClearChildren();
          navigatorTreeNode.Fetch();
          navigatorTreeNode.Full = true;
        }
        finally
        {
          this._isFetch = false;
        }
      }
      NodeColumnCollection columns = navigatorTreeNode.Tree.GetColumns();
      foreach (NavigatorTreeNode child in (List<NavigatorTreeNode>) navigatorTreeNode.Children)
        this.InitNodeData(child, child.NodeID, columns);
    }
    e.Children = (IList) navigatorTreeNode.Children;
  }

  protected override NavigatorTreeNode CreateRootNode()
  {
    this._rootNode.Handler = this.RootHandler;
    this._rootNode.Handle = this.RootRow;
    return base.CreateRootNode();
  }

  protected override void TreeRowExpand(object sender, RowEventArgs e)
  {
    (e.Row.Item as NavigatorTreeNode).Handle = e.Row;
  }

  protected override void TreeGetChildPolicy(object sender, GetChildPolicyEventArgs e)
  {
    e.ChildPolicy = RowChildPolicy.LoadOnExpand;
  }
}
