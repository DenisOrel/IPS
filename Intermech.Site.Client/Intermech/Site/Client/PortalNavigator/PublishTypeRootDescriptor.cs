// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishTypeRootDescriptor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.VirtualNodes;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PublishTypeRootDescriptor : HiveDescriptor
{
  protected Guid typeGuid;

  public PublishTypeRootDescriptor(int categoryID, Guid typeGuid)
    : base(categoryID, -1, string.Empty)
  {
    this.typeGuid = typeGuid;
    this.Initialize();
  }

  protected void Initialize()
  {
    IPortalMetadata service = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
    if (service != null)
    {
      PortalObjectType publishObjectType = service.GetPublishObjectType(this.typeGuid);
      if (publishObjectType != null)
      {
        this._caption = publishObjectType.Name;
        this._typeID = publishObjectType.ID;
      }
    }
    if (this._typeID != -1)
      return;
    this._caption = MetaDataHelper.GetObjectTypeName(this.typeGuid);
  }

  public override INode GetChild(INodeID nodeID) => (INode) new PublishTypeNode(this._typeID);

  public virtual Guid GUID => SiteClientConsts.CategoryRootPublishTypeGuid;

  public override INodeID GetRecordNodeID()
  {
    return (INodeID) new PublishTypeNodeID(this._typeID, this._caption);
  }

  public override bool Equals(object obj)
  {
    if (obj == null || obj.GetType() != typeof (PublishTypeRootDescriptor))
      return base.Equals(obj);
    PublishTypeRootDescriptor typeRootDescriptor = (PublishTypeRootDescriptor) obj;
    return this._categoryID == typeRootDescriptor._categoryID && this._typeID == typeRootDescriptor._typeID;
  }

  public override int GetHashCode() => base.GetHashCode();
}
