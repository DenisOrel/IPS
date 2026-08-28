// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.IncompleteRelationAnalyzer
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class IncompleteRelationAnalyzer(
  IUserSession session,
  ISitesCacheService cacheService,
  SiteInfo info,
  TransferedObject unit) : UnitAnalyzer(session, cacheService, info, unit, (XmlNode) null)
{
  public override void Analysis(
    IDBObjectCollection publishObjects,
    List<Guid> importedObjects,
    PackAnalyzInfo packAnalyzInfo,
    Dictionary<Guid, int> partCounter)
  {
    IncompleteRelationTag tag = this.unit.Tag as IncompleteRelationTag;
    Guid guid = new Guid(tag.Guid);
    packAnalyzInfo.AnalyzRelationInfo.Add(new Tuple<Guid, Guid, Guid, AnalyzRelationInfo>(guid, new Guid(tag.ProjectGuid), new Guid(tag.PartGuid), new AnalyzRelationInfo(guid, false)));
  }
}
