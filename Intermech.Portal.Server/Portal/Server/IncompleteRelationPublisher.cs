// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.IncompleteRelationPublisher
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class IncompleteRelationPublisher(
  IUserSession session,
  TransferedObject unit,
  SiteInfo info) : UnitPublisher(session, unit, (XmlDocument) null, (XmlNode) null, info, 5)
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
    IncompleteRelationTag tag = this.unit.Tag as IncompleteRelationTag;
    Guid guid = new Guid(tag.Guid);
    AnalyzObjectInfo analyzObjectInfo;
    if (packAnalyzInfo.AnalyzObjectInfo.TryGetValue(new Guid(tag.ProjectGuid), out analyzObjectInfo))
      caches.Relations.Add(Tuple.Create<Guid, long>(guid, analyzObjectInfo.ID));
    return guid;
  }
}
