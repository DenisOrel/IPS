// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PortalDescriptor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.VirtualNodes;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class PortalDescriptor : HiveDescriptor
{
  private bool _isAdmin;

  public PortalDescriptor(string portalName, bool isAdmin)
    : base(SiteClientConsts.CategoryPortal, 0, portalName)
  {
    this._isAdmin = isAdmin;
  }

  public Guid GUID => new Guid("{D705BE41-4E62-44c9-8F4F-314E9CFAA0E8}");

  public override INode GetChild(INodeID nodeID) => (INode) new PortalRootNode(this._isAdmin);

  public override bool Equals(object obj)
  {
    if (obj == null || obj.GetType() != typeof (PortalDescriptor))
      return base.Equals(obj);
    PortalDescriptor portalDescriptor = (PortalDescriptor) obj;
    return this._categoryID == portalDescriptor._categoryID && this._typeID == portalDescriptor._typeID;
  }

  public override int GetHashCode() => base.GetHashCode();
}
