// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.RelationAnalyzer
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server.WebPortal;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class RelationAnalyzer(
  IUserSession session,
  ISitesCacheService cacheService,
  SiteInfo info,
  TransferedObject unit,
  XmlNode rootNode) : UnitAnalyzer(session, cacheService, info, unit, rootNode)
{
  public override void Analysis(
    IDBObjectCollection publishObjects,
    List<Guid> importedObjects,
    PackAnalyzInfo packAnalyzInfo,
    Dictionary<Guid, int> partCounter)
  {
    RelationInfo relationAttributes = AttributesFile.GetRelationAttributes(this.rootNode);
    if (relationAttributes.Guid == Guid.Empty || relationAttributes.ProjectGuid == Guid.Empty || relationAttributes.PartGuid == Guid.Empty)
      throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_28"), (object) relationAttributes.Guid));
    if (!packAnalyzInfo.AnalyzObjectInfo.ContainsKey(relationAttributes.ProjectGuid))
    {
      Tuple<long, Guid> publishObjectId = UnitAnalyzer.GetPublishObjectID(publishObjects, relationAttributes.ProjectGuid);
      if (publishObjectId == null)
        throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_30"), (object) relationAttributes.Guid, (object) relationAttributes.ProjectGuid));
      packAnalyzInfo.AnalyzObjectInfo.Add(relationAttributes.ProjectGuid, new AnalyzObjectInfo(publishObjectId.Item1, publishObjectId.Item2, false));
    }
    if (partCounter.ContainsKey(relationAttributes.PartGuid))
      partCounter[relationAttributes.PartGuid]++;
    else
      partCounter.Add(relationAttributes.PartGuid, 1);
    packAnalyzInfo.AnalyzRelationInfo.Add(new Tuple<Guid, Guid, Guid, AnalyzRelationInfo>(relationAttributes.Guid, relationAttributes.ProjectGuid, relationAttributes.PartGuid, new AnalyzRelationInfo(relationAttributes.Guid, true)));
  }
}
