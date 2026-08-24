// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.NavigatorTreeNodeHelper
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Interfaces.Client;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;

#nullable disable
namespace Intermech.MRP.Orders;

public static class NavigatorTreeNodeHelper
{
  public static void Fetch(this NavigatorTreeNode navigatorTreeNode)
  {
    INode handler = navigatorTreeNode.Handler;
    INodeQuery query = handler.GetQuery(ContentType.Folders);
    if (query == null)
      return;
    NavigatorTreeNodeHelper.SetQueryColumns(query, navigatorTreeNode.Tree.GetColumns());
    query.Execute(navigatorTreeNode.Bookmark, int.MaxValue);
    navigatorTreeNode.Children.Clear();
    int index = 0;
    for (int recordCount = query.RecordCount; index < recordCount; ++index)
    {
      INodeID recordNodeId = query.GetRecordNodeID(index);
      NavigatorTreeNode navigatorTreeNode1 = new NavigatorTreeNode(navigatorTreeNode.Tree, navigatorTreeNode, recordNodeId, query.GetRecordValues(index), query.GetRawRecordValues(index), handler.GetChild(recordNodeId), TreeNodeFlags.None, (object) null, false, navigatorTreeNode.Tree.ShareValidColumns(navigatorTreeNode.Tree.GetColumns()));
    }
  }

  private static void SetQueryColumns(
    INodeQuery nodeQuery,
    NodeColumnCollection nodeColumnCollection)
  {
    IColumnSchemes service = ServicesManager.GetService(typeof (IColumnSchemes)) as IColumnSchemes;
    int index = 0;
    for (int count = nodeColumnCollection.Count; index < count; ++index)
      nodeQuery.AddColumn(nodeColumnCollection[index], service.GetDefaultTransform(nodeColumnCollection[index].SchemeGuid, nodeColumnCollection[index].ID));
  }
}
