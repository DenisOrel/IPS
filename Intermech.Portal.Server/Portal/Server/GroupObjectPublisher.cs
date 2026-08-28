// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.GroupObjectPublisher
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class GroupObjectPublisher : UnitPublisher
{
  public GroupObjectPublisher(IUserSession session, TransferedObject unit, SiteInfo info)
    : base(session, unit, (XmlDocument) null, (XmlNode) null, info, 1)
  {
    this.directoryName = (string) null;
  }

  public override Guid Publish(
    IDBObjectCollection publishObjects,
    string enabledSites,
    GroupPublishItem item,
    PackAnalyzInfo packAnalyzInfo,
    PublishCaches caches,
    IDBRelationCollection relCollection,
    IDBRelationType relTypePublish)
  {
    IDBObject attributable = this.session.GetObject(new Guid(this.unit.GUID));
    ObjectTag tag = (ObjectTag) this.unit.Tag;
    if (tag != null)
    {
      if (tag.OwnerCode.HasValue)
      {
        IDBAttribute attributeByGuid1 = attributable.GetAttributeByGuid(PortalConsts.attributeOwner);
        IDBAttribute attributeByGuid2 = attributable.GetAttributeByGuid(PortalConsts.attributeParentSites);
        if (PublishHelper.SetAutoTransferOwnAttribute(attributeByGuid1, attributeByGuid2, this.info.Code.ToString(), tag.OwnerCode) && TraceLog.Enabled)
          TraceLog.Write($"...set dbAttrParentSites={attributeByGuid2.AsString} dbAttrOwner={attributeByGuid1.AsString}");
      }
      if (tag.CompositionOwnerCode.HasValue)
      {
        IDBAttribute attributeByGuid3 = attributable.GetAttributeByGuid(PortalConsts.attributeCompositionOwner);
        IDBAttribute attributeByGuid4 = attributable.GetAttributeByGuid(PortalConsts.attributeCompositionParentSites);
        if (PublishHelper.SetAutoTransferOwnAttribute(attributeByGuid3, attributeByGuid4, this.info.Code.ToString(), tag.CompositionOwnerCode) && TraceLog.Enabled)
          TraceLog.Write($"...set dbAttrCompositionParentSites={attributeByGuid4.AsString} dbAttrCompositionOwner={attributeByGuid3.AsString}");
      }
    }
    TransferedObject transferedObject = ImportTask.SaveObjectInfoDisk(this.info, (IDBAttributable) attributable, ChangeType.ctUpdate, TransferedObjectCategory.Object, new Guid(this.unit.GUID), false, out string _);
    this.unit.DataFiles = transferedObject.DataFiles;
    this.unit.Tag = transferedObject.Tag;
    return attributable.ObjectGUID;
  }
}
