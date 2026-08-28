// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.RelationPublisher
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

internal class RelationPublisher : UnitPublisher
{
  public RelationPublisher(
    IUserSession session,
    TransferedObject unit,
    XmlDocument xmlDocument,
    XmlNode rootNode,
    SiteInfo info)
    : base(session, unit, xmlDocument, rootNode, info, 5)
  {
    if (rootNode == null)
      throw new ArgumentNullException(nameof (rootNode));
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
    RelationInfo ri = AttributesFile.GetRelationAttributes(this.rootNode);
    AnalyzRelationInfo analyzRelationInfo = packAnalyzInfo.AnalyzRelationInfo.Find((Predicate<Tuple<Guid, Guid, Guid, AnalyzRelationInfo>>) (x => x.Item1.Equals(ri.Guid) && x.Item2.Equals(ri.ProjectGuid) && x.Item3.Equals(ri.PartGuid))).Item4;
    if (TraceLog.Enabled)
    {
      TraceLog.Write($"RelationPublisher: Guid={ri.Guid} ProjectGuid={ri.ProjectGuid} PartGuid={ri.PartGuid}");
      TraceLog.Write($"AnalyzRelationInfo: ID={analyzRelationInfo.ID} PublishEnable={analyzRelationInfo.PublishEnable}");
    }
    try
    {
      if (!analyzRelationInfo.PublishEnable)
        return ri.Guid;
      AnalyzObjectInfo analyzObjectInfo1 = packAnalyzInfo.AnalyzObjectInfo[ri.ProjectGuid];
      IDBObject dBObject = this.session.GetObject(analyzObjectInfo1.ID);
      IDBRelation relation = this.session.GetRelation(ri.Guid, dBObject.ObjectID, false);
      bool flag = false;
      AnalyzObjectInfo analyzObjectInfo2;
      if (!packAnalyzInfo.AnalyzObjectInfo.TryGetValue(ri.PartGuid, out analyzObjectInfo2))
      {
        Tuple<long, Guid> publishObjectId = UnitAnalyzer.GetPublishObjectID(publishObjects, ri.PartGuid);
        if (publishObjectId == null)
          throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_38"), (object) ri.Guid, (object) ri.PartGuid));
        analyzObjectInfo2 = new AnalyzObjectInfo(publishObjectId.Item1, publishObjectId.Item2, false);
        packAnalyzInfo.AnalyzObjectInfo.Add(ri.PartGuid, analyzObjectInfo2);
      }
      if (relation == null)
      {
        if (analyzObjectInfo2.ID == 0L)
          throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_70"), (object) ri.PartGuid));
        if (this.info.SystemType == SystemTypes.Search && !ActionsHelper.IsObjectOwner(this.info, dBObject) && !ActionsHelper.IsObjectOwner(this.info, this.session.GetObject(analyzObjectInfo2.ID)))
          return ri.Guid;
        relation = relCollection.Create(dBObject.ObjectID, analyzObjectInfo2.ID);
        relation.GUID = ri.Guid;
        flag = true;
        if (ri.RelationTypeGuid != Guid.Empty)
          PublishHelper.AddAtribute((IDBAttributable) relation, IDHelper.AttributeRelationTypeGuidID, (object) ri.RelationTypeGuid);
        else
          PublishHelper.AddAtribute((IDBAttributable) relation, IDHelper.AttributeRelationTypeGuidID);
        PublishHelper.AddAtribute((IDBAttributable) relation, IDHelper.AttributeRelTypeNameID, (object) ri.RelationTypeName);
        if (TraceLog.Enabled)
          TraceLog.Write($"...new relation {(ri.RelationTypeGuid != Guid.Empty ? (object) ri.RelationTypeGuid.ToString() : (object) ri.RelationTypeName)} created. {relation.ProjID}->{relation.PartID}");
      }
      PublishHelper.AddAtribute((IDBAttributable) relation, IDHelper.AttributeVersionInRelationID, (object) Math.Abs(analyzObjectInfo2.ID));
      List<Guid> currentLinks = new List<Guid>();
      this.ParceXMLIntoAttributable(this.session, (IDBAttributable) relation, (IDBAttributableType) relTypePublish, this.xmlDocument, this.rootNode, this.directoryName, caches.ImportedObjectsIDs, ref currentLinks);
      if (currentLinks.Count > 0)
        caches.RelationsWithLinks.Add(Tuple.Create<Guid, long>(relation.GUID, analyzObjectInfo1.ID));
      this.SetLinksAttribute(this.session, (IDBAttributable) relation, currentLinks);
      caches.Relations.Add(Tuple.Create<Guid, long>(relation.GUID, analyzObjectInfo1.ID));
      string objectName = $"Связь {relation.GUID} между {relation.ProjID} и {relation.PartID}";
      if (flag)
        this.AddEvent(Math.Abs(relation.RelationID), objectName, "Создана связь");
      else
        this.AddEvent(Math.Abs(relation.RelationID), objectName, "Обновлена связь");
      return relation.GUID;
    }
    finally
    {
      if (item != null && item is PublishGroup && ri.Guid != Guid.Empty)
        ((PublishGroup) item).AddItemToRelationList(ri.Guid);
    }
  }
}
