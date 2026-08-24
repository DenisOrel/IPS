// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareObjectsTreeView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls;
using Intermech.DataFormats;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class CompareObjectsTreeView : NavigatorTreeView
{
  public CompareObjectsTreeView()
  {
  }

  public CompareObjectsTreeView(IServiceProvider services)
    : base(services)
  {
  }

  protected override StyleDelta CheckedOutByCurrentUserStyleDelta(long currentID)
  {
    if (currentID > 0L && this.RootNode.Children != null && this.RootNode.Children.Count == 2)
    {
      long[] numArray = new long[2];
      for (int index = 0; index < this.RootNode.Children.Count; ++index)
      {
        NavigatorTreeNode child = this.RootNode.Children[index];
        INodeID nodeId = child.NodeID;
        IDBObjectID data = this.GetNodeHandler(child).GetData(nodeId, typeof (IDBObjectID)) as IDBObjectID;
        numArray[index] = data.Value;
      }
      if (Math.Abs(numArray[0]) == Math.Abs(numArray[1]))
        return (StyleDelta) null;
    }
    return base.CheckedOutByCurrentUserStyleDelta(currentID);
  }
}
