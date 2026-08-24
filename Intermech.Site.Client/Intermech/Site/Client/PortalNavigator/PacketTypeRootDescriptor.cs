// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PacketTypeRootDescriptor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PacketTypeRootDescriptor : PublishTypeRootDescriptor
{
  public PacketTypeRootDescriptor()
    : base(SiteClientConsts.CategoryRootPacketType, PortalConsts.objtypePacket)
  {
  }

  public override Guid GUID => SiteClientConsts.CategoryRootPacketTypeGuid;

  public override INode GetChild(INodeID nodeID) => (INode) new PacketTypeNode(this._typeID);

  public override INodeID GetRecordNodeID()
  {
    return (INodeID) new PacketTypeNodeID(this._typeID, this._caption);
  }

  public override bool Equals(object obj)
  {
    if (obj == null || obj.GetType() != typeof (PacketTypeRootDescriptor))
      return base.Equals(obj);
    PacketTypeRootDescriptor typeRootDescriptor = (PacketTypeRootDescriptor) obj;
    return this._categoryID == typeRootDescriptor._categoryID && this._typeID == typeRootDescriptor._typeID;
  }
}
