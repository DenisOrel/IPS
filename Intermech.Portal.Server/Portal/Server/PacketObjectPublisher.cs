// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.PacketObjectPublisher
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal class PacketObjectPublisher(
  IUserSession session,
  TransferedObject unit,
  XmlDocument xmlDocument,
  XmlNode rootNode,
  SiteInfo info,
  IDBImporter importer) : ObjectPublisher(session, unit, xmlDocument, rootNode, info, importer)
{
  public override Guid Publish(
    IDBObjectCollection publishObjects,
    string enabledSites,
    GroupPublishItem item,
    PackAnalyzInfo packAnalyzInfo,
    PublishCaches caches,
    IDBRelationCollection relCollection,
    IDBRelationType relTypePublish)
  {
    Guid guid = base.Publish(publishObjects, enabledSites, (GroupPublishItem) null, packAnalyzInfo, caches, relCollection, relTypePublish);
    PublishHelper.AddUnitFilesToPacket(item as PublishPacket, this.unit, this.UnitTempDirectory, guid.ToString());
    return guid;
  }
}
