// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.GroupRelationPublisher
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal class GroupRelationPublisher : UnitPublisher
{
  public GroupRelationPublisher(IUserSession session, TransferedObject unit, SiteInfo info)
    : base(session, unit, (XmlDocument) null, (XmlNode) null, info, 5)
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
    IDBRelation relation = this.session.GetRelation(new Guid(this.unit.GUID), false);
    if (relation == null)
    {
      if (TraceLog.Enabled)
        TraceLog.Write($"...relation {this.unit.GUID} from packet not found!");
      return Guid.Empty;
    }
    TransferedObject transferedObject = ImportTask.SaveObjectInfoDisk(this.info, (IDBAttributable) relation, ChangeType.ctUpdate, TransferedObjectCategory.Relation, new Guid(this.unit.GUID), false, out string _);
    this.unit.DataFiles = transferedObject.DataFiles;
    this.unit.Tag = transferedObject.Tag;
    return relation.GUID;
  }
}
