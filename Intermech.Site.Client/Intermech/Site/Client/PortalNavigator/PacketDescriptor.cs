// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PacketDescriptor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.VirtualNodes;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal sealed class PacketDescriptor : HiveDescriptor
{
  private int _objectType;
  private long _objectID;
  private char _creatorID;
  private new string _caption;
  private DateTime _createDate;

  public PacketDescriptor(
    int objectType,
    long objectID,
    char creatorID,
    string caption,
    DateTime createDate)
    : base(SiteClientConsts.CategoryPublishPacket, objectType, caption)
  {
    this._objectType = objectType;
    this._objectID = objectID;
    this._creatorID = creatorID;
    this._caption = caption;
    this._createDate = createDate;
  }

  public override INodeID GetRecordNodeID()
  {
    return (INodeID) new PacketNodeID(this._objectType, this._objectID, this._creatorID, this._caption, this._createDate);
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    return dataFormat == typeof (IPacketNodeID) && nodeID is PacketNodeID packetNodeId ? (object) packetNodeId : base.GetData(nodeID, dataFormat);
  }
}
