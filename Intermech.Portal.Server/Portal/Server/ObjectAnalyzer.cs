// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.ObjectAnalyzer
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

internal sealed class ObjectAnalyzer(
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
    ObjectInfo oi;
    AnalyzInfo aInfo;
    this.GetObjectInfo(this.rootNode, this.info, publishObjects, importedObjects, out oi, out aInfo);
    ((AnalyzObjectInfo) aInfo).InComposition = this.unit.Tag != null && (this.unit.Tag as ObjectTag).InComposition;
    ((AnalyzObjectInfo) aInfo).WithComposition = this.unit.Tag != null && (this.unit.Tag as ObjectTag).WithComposition;
    if (packAnalyzInfo.AnalyzObjectInfo.ContainsKey(oi.ObjectGuid))
      return;
    packAnalyzInfo.AnalyzObjectInfo.Add(oi.ObjectGuid, (AnalyzObjectInfo) aInfo);
  }
}
