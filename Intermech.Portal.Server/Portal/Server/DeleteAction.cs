// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.DeleteAction
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class DeleteAction : PortalAction
{
  private bool CheckAccessForDeleteComposition(
    IUserSession session,
    SiteInfo info,
    Guid rootObjectGuid)
  {
    IDBObject dbObject = session.GetObject(rootObjectGuid);
    IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(PortalConsts.attributeCompositionOwner);
    return attributeByGuid != null && !string.IsNullOrEmpty(attributeByGuid.AsString) ? attributeByGuid.AsString[0].Equals(info.Code) : dbObject.GetAttributeByID(IDHelper.AttributeOwner).AsString[0].Equals(info.Code);
  }

  public void ClearComposition(Guid sessionGuid, string objectGuid, string[] relationTypes)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start ClearComposition sessionGuid={sessionGuid}");
    if (objectGuid == string.Empty || !GuidHelper.IsGuid(objectGuid))
      throw new ArgumentException(LocalizationHolder.rm.GetString("PortalServer_56"));
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    if (siteInfo.SystemType != SystemTypes.Search && this.CheckAccessForDeleteComposition(userSession, siteInfo, new Guid(objectGuid)))
      throw new Exception($"У узла {siteInfo.Caption} нет прав на удаление состава опубликованного объекта {objectGuid}");
    if (relationTypes == null || relationTypes.Length == 0)
      return;
    List<string> stringList = new List<string>(relationTypes.Length);
    for (int index = 0; index < relationTypes.Length; ++index)
      stringList.Add(relationTypes[index].ToLower());
    IDBObjectCollection objectCollection = userSession.GetObjectCollection(PortalConsts.objtypePublishObjects);
    DataTable dataTable1 = objectCollection.Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(PortalConsts.attributePublishObjectGUID, RelationalOperators.Equal, (object) new Guid(objectGuid), LogicalOperators.AND, 0)
    }, new object[1]{ (object) -2 }));
    if (dataTable1.Rows.Count == 0)
      return;
    long int64 = Convert.ToInt64(dataTable1.Rows[0][0]);
    IDBObject dBObject = userSession.GetObject(int64);
    bool flag = ActionsHelper.IsObjectOwner(siteInfo, dBObject);
    IDBRelationCollection relationCollection = userSession.GetRelationCollection(MetaDataHelper.GetRelationTypeID(PortalConsts.reltypePublish));
    DataTable dataTable2 = relationCollection.ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[4]
    {
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Default, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -20, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.Default, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ActionsHelper.GetAttributeTypeID(userSession, PortalConsts.attributeRelTypeName), AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.Default, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ActionsHelper.GetAttributeTypeID(userSession, PortalConsts.attributeOwner), AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Default, SortOrders.NONE, 0)
    }), int64, false);
    for (int index = 0; index < dataTable2.Rows.Count; ++index)
    {
      if (stringList.Contains(Convert.ToString(dataTable2.Rows[index][2]).ToLower()) && (siteInfo.SystemType != SystemTypes.Search || flag || !(Convert.ToString(dataTable2.Rows[index][3]) != siteInfo.Code.ToString())))
      {
        IDBRelation relation = userSession.GetRelation(Convert.ToInt64(dataTable2.Rows[index][1]), false);
        if (relation != null)
          this.CheckAndDeleteRelation(userSession, siteInfo, objectCollection, relationCollection, relation);
      }
    }
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write($"End ClearComposition site={siteInfo.Code}");
  }

  private bool EntersInPresent(IDBRelationCollection rellColl, long objectID)
  {
    return rellColl.EntersInVersion(new DBRecordSetParams((ConditionStructure[]) null, new object[1]
    {
      (object) -20
    }), objectID).Rows.Count > 0;
  }

  public void DeleteObjects(Guid sessionGuid, long[] objectIDs)
  {
    this.DeleteObjectsEx(sessionGuid, objectIDs);
  }

  public string[] DeleteObjectsEx(Guid sessionGuid, long[] objectIDs)
  {
    if (TraceLog.Enabled)
      TraceLog.Write("Start DeleteObjectsEx");
    if (objectIDs == null || objectIDs.Length == 0)
      return (string[]) null;
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    if (TraceLog.Enabled)
      TraceLog.Write($"..site={siteInfo.Code} sessionGuid={sessionGuid}");
    List<string> deletedObjectGuids = new List<string>();
    IDBRelationType relationType = userSession.GetRelationType(PortalConsts.reltypePublish);
    IDBRelationCollection relationCollection = userSession.GetRelationCollection(relationType.RelationType);
    List<long> longList = new List<long>();
    for (int index1 = 0; index1 < objectIDs.Length; ++index1)
    {
      if (!longList.Contains(objectIDs[index1]))
      {
        DataTable dataTable = relationCollection.ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new object[1]
        {
          (object) -2
        }), objectIDs[index1], true);
        for (int index2 = 0; index2 < dataTable.Rows.Count; ++index2)
        {
          long int64 = Convert.ToInt64(dataTable.Rows[index2][0]);
          if (!longList.Contains(int64))
          {
            this.DeleteObject(userSession, deletedObjectGuids, siteInfo, int64, relationCollection, false, true);
            longList.Add(int64);
          }
        }
        this.DeleteObject(userSession, deletedObjectGuids, siteInfo, objectIDs[index1], relationCollection, true, true);
        longList.Add(objectIDs[index1]);
      }
    }
    if (TraceLog.Enabled)
      TraceLog.Write($"End DeleteObjectsEx site={siteInfo.Code}");
    return deletedObjectGuids.ToArray();
  }

  public void CheckAndDeleteRelation(
    IUserSession session,
    SiteInfo info,
    IDBObjectCollection objColl,
    IDBRelationCollection relCollection,
    IDBRelation relation)
  {
    IDBAttribute attributeById = relation.GetAttributeByID(IDHelper.AttributePublishLinksID);
    Guid guid = relation.GUID;
    long projId = relation.ProjID;
    if (attributeById != null)
    {
      for (int index = 0; index < attributeById.ValuesCount; ++index)
      {
        attributeById.Index = index;
        if (!attributeById.IsNull && attributeById is IDBObjectLinkAttribute && ActionsHelper.CountLinks(session, objColl, relCollection, attributeById.AsInteger, 0L) == 0)
          this.DeleteObject(session, (List<string>) null, info, (attributeById as IDBObjectLinkAttribute).DBObject, relCollection, false, false);
      }
    }
    relation = session.GetRelation(guid, projId, false);
    relation?.Delete(0L);
  }

  public bool CheckAndDeleteVersion(
    IUserSession session,
    SiteInfo info,
    IDBRelationCollection relCollection,
    long objectID)
  {
    IDBObject objVer = session.GetObject(objectID, false);
    return objVer != null && this.CheckAndDeleteVersion(session, info, relCollection, objVer);
  }

  public bool CheckAndDeleteVersion(
    IUserSession session,
    SiteInfo info,
    IDBRelationCollection relCollection,
    IDBObject objVer)
  {
    if (objVer == null || objVer.IsBaseVersion)
      return false;
    IDBAttribute attributeByGuid = objVer.GetAttributeByGuid(PortalConsts.attributePublishInComposition);
    if (attributeByGuid != null && attributeByGuid.AsBoolean)
    {
      if (relCollection.Select(new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(IDHelper.AttributeVersionInRelationID, RelationalOperators.Equal, (object) Math.Abs(objVer.ObjectID), LogicalOperators.AND, 0, false)
      }, new object[1]{ (object) -20 })).Rows.Count == 0)
      {
        List<long> deletedObjects = new List<long>();
        try
        {
          this.DeleteObject(session, (List<string>) null, deletedObjects, info, objVer, relCollection, false, false);
        }
        finally
        {
        }
      }
    }
    return true;
  }

  private bool DeleteObject(
    IUserSession session,
    List<string> deletedObjectGuids,
    SiteInfo info,
    long objectID,
    IDBRelationCollection relCollection,
    bool rootObject,
    bool throwException)
  {
    IDBObject dbObject = session.GetObject(objectID, false);
    return dbObject != null && this.DeleteObject(session, deletedObjectGuids, new List<long>(), info, dbObject, relCollection, rootObject, throwException);
  }

  private bool DeleteObject(
    IUserSession session,
    List<string> deletedObjectGuids,
    SiteInfo info,
    IDBObject obj,
    IDBRelationCollection relCollection,
    bool rootObject,
    bool throwException)
  {
    return this.DeleteObject(session, deletedObjectGuids, new List<long>(), info, obj, relCollection, rootObject, throwException);
  }

  private bool DeleteObject(
    IUserSession session,
    List<string> deletedObjectGuids,
    List<long> deletedObjects,
    SiteInfo info,
    long objectID,
    IDBRelationCollection relCollection,
    bool rootObject,
    bool throwException)
  {
    IDBObject dbObject = session.GetObject(objectID, false);
    return dbObject != null && this.DeleteObject(session, deletedObjectGuids, deletedObjects, info, dbObject, relCollection, rootObject, throwException);
  }

  private bool DeleteObject(
    IUserSession session,
    List<string> deletedObjectGuids,
    List<long> deletedObjects,
    SiteInfo info,
    IDBObject obj,
    IDBRelationCollection relCollection,
    bool rootObject,
    bool throwException)
  {
    return this.DeleteObject(session, deletedObjectGuids, deletedObjects, info, obj, relCollection, rootObject, throwException, false);
  }

  private bool DeleteObject(
    IUserSession session,
    List<string> deletedObjectGuids,
    List<long> deletedObjects,
    SiteInfo info,
    IDBObject obj,
    IDBRelationCollection relCollection,
    bool rootObject,
    bool throwException,
    bool fromClearComposition)
  {
    if (deletedObjects.Contains(obj.ObjectID) || !OwnChecks.DeleteCheckCreator(info, obj, throwException) || !OwnChecks.DeleteCheckOwner(info, obj, rootObject, throwException) || !OwnChecks.DeleteCheckCompositionOwner(info, obj, throwException))
      return false;
    IDBObjectCollection objectCollection = session.GetObjectCollection(PortalConsts.objtypePublishObjects);
    IDBAttribute attributeById = obj.GetAttributeByID(IDHelper.AttributePublishLinksID);
    if (attributeById != null)
    {
      for (int index = 0; index < attributeById.ValuesCount; ++index)
      {
        attributeById.Index = index;
        if (!attributeById.IsNull && attributeById is IDBObjectLinkAttribute && ActionsHelper.CountLinks(session, objectCollection, relCollection, attributeById.AsInteger, obj.ObjectID) == 0)
        {
          IDBObject dbObject = (attributeById as IDBObjectLinkAttribute).DBObject;
          if (!deletedObjects.Contains(dbObject.ObjectID))
            this.DeleteObject(session, deletedObjectGuids, deletedObjects, info, dbObject, relCollection, false, throwException, fromClearComposition);
        }
      }
    }
    List<long> longList = (List<long>) null;
    IDBAttribute attributeByGuid = obj.GetAttributeByGuid(PortalConsts.attributeLinkedGuid, false);
    if (attributeByGuid != null && GuidHelper.IsGuid(attributeByGuid.AsString))
    {
      DataTable dataTable = objectCollection.SelectWithLocalObjects(new DBRecordSetParams(new ConditionStructure[2]
      {
        new ConditionStructure(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeLinkedGuid), RelationalOperators.Equal, (object) attributeByGuid.AsString, LogicalOperators.AND, 0, false),
        new ConditionStructure(-2, RelationalOperators.NotEqual, (object) obj.ObjectID, LogicalOperators.AND, 0, false)
      }, new object[1]{ (object) -2 }));
      if (dataTable.Rows.Count > 0)
      {
        longList = new List<long>(dataTable.Rows.Count);
        for (int index = 0; index < dataTable.Rows.Count; ++index)
        {
          long int64 = Convert.ToInt64(dataTable.Rows[index][0]);
          if (!deletedObjects.Contains(int64))
            longList.Add(int64);
        }
      }
    }
    deletedObjectGuids?.Add(obj.GetAttributeByGuid(PortalConsts.attributePublishObjectGUID).AsString);
    deletedObjects.Add(obj.ObjectID);
    if (TraceLog.Enabled)
      TraceLog.Write($"...object deleted ObjectID={obj.ObjectID} ObjectGuid={obj.ObjectGUID} Caption={obj.Caption}");
    try
    {
      obj.Delete(0L);
    }
    catch
    {
      if (throwException)
        throw;
    }
    if (longList != null && longList.Count > 0)
    {
      for (int index = 0; index < longList.Count; ++index)
      {
        bool flag = true;
        if (fromClearComposition && this.EntersInPresent(relCollection, longList[index]))
          flag = false;
        if (flag)
          this.DeleteObject(session, deletedObjectGuids, deletedObjects, info, session.GetObject(longList[index]), relCollection, false, throwException, fromClearComposition);
      }
    }
    return true;
  }
}
