// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PacketsListDescriptor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.VirtualNodes;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PacketsListDescriptor : HiveDescriptor
{
  private List<long> _objectIDs;
  private INode _node;

  public PacketsListDescriptor(List<long> objectIDs, int typeID)
    : base(SiteClientConsts.CategoryPublishPacket, typeID, "Список опубликованных пакетов")
  {
    this._objectIDs = objectIDs;
    this._node = (INode) new PacketsListNode(this._objectIDs, this._typeID);
  }

  public override INode GetChild(INodeID nodeID) => this._node;
}
