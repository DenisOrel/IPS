// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.PublishAction
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class PublishAction : PortalAction
{
  private List<long> _working = new List<long>();

  public void TransferPublishUnitFileEx(
    Guid sessionGuid,
    string unitGuid,
    string fileName,
    string bytes,
    bool continuation)
  {
    if (bytes == null || bytes.Length <= 0)
      return;
    byte[] bytes1 = Convert.FromBase64String(bytes);
    this.TransferPublishUnitFile(sessionGuid, unitGuid, fileName, bytes1, continuation);
  }

  public long StartPublishingTask(
    Guid sessionGuid,
    string taskName,
    string enabledSites,
    long packetID)
  {
    try
    {
      if (TraceLog.Enabled)
        TraceLog.Write($"Start StartPublishingTask sessionGuid={sessionGuid} taskName={taskName} packetID={packetID}");
      IDBObject dbTask;
      PublishTask publishTask = PublishTask.NewTask(this.GetUserSession(sessionGuid), taskName, TempStorage.RootFolder, enabledSites, out dbTask);
      if (packetID != 0L)
        dbTask.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalServerConsts.attributePacketNumber), false, new object[1]
        {
          (object) packetID
        });
      if (TraceLog.Enabled)
        TraceLog.Write($"End StartPublishingTask taskName={taskName}");
      return publishTask.DBTask.ObjectID;
    }
    catch (Exception ex)
    {
      throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_19"), (object) taskName, (object) LogException.Create(ex)));
    }
  }

  public long StartPublishingTask(Guid sessionGuid, string taskName, string enabledSites)
  {
    return this.StartPublishingTask(sessionGuid, taskName, enabledSites, 0L);
  }

  public void DeleteGroup(Guid sessionGuid, long packetID, bool withObjects)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start DeletePacket sessionGuid={sessionGuid} packetID={packetID}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    IDBObject dbObject = userSession.GetObject(packetID);
    (userSession as UserSession).StartTransaction();
    try
    {
      if (withObjects)
      {
        DataTable dataTable = userSession.GetRelationCollection(userSession.IdentHelper.SimpleRelationTypeID).ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new object[1]
        {
          (object) -2
        }), packetID);
        if (dataTable.Rows.Count > 0)
        {
          List<long> longList = new List<long>(dataTable.Rows.Count);
          for (int index = 0; index < dataTable.Rows.Count; ++index)
          {
            long int64 = Convert.ToInt64(dataTable.Rows[index][0]);
            if (!longList.Contains(int64))
              longList.Add(int64);
          }
          if (TraceLog.Enabled)
            TraceLog.Write($"...deleting {longList.Count} objects from packet");
          new DeleteAction().DeleteObjects(sessionGuid, longList.ToArray());
        }
      }
      dbObject.Delete(0L);
      (userSession as UserSession).Commit();
      if (!TraceLog.Enabled)
        return;
      TraceLog.Write($"End DeletePacket sessionGuid={sessionGuid} packetID={packetID}");
    }
    catch
    {
      (userSession as UserSession).Rollback();
      throw;
    }
  }

  public long CreateGroup(Guid sessionGuid, long taskID)
  {
    try
    {
      if (TraceLog.Enabled)
        TraceLog.Write($"Start CreatePacket sessionGuid={sessionGuid} taskID={taskID}");
      IDBObject publishUnit = this.CreatePublishUnit(this.GetUserSession(sessionGuid), taskID, PortalServerConsts.objecttypePublishObjectsGroups, new Guid?());
      if (TraceLog.Enabled)
        TraceLog.Write($"End CreatePacket taskID={taskID}");
      return publishUnit.ObjectID;
    }
    catch (Exception ex)
    {
      throw new Exception($"Ошибка при создании пакета для задачи {taskID}: {LogException.Create(ex)}");
    }
  }

  public string[][] UseGroup(Guid sessionGuid, long taskID, long groupID, string ownerCode)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start UsePacket taskID={taskID} packetID={groupID} sessionGuid={sessionGuid} ");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    this.GetSiteInfo(userSession);
    PublishTask publishTask = PublishTask.GetPublishTask(userSession, taskID);
    publishTask.Status = TaskStatus.Transmitting;
    try
    {
      List<Tuple<Guid, Guid, PublishObjectRootType>> objects;
      List<Guid> relations;
      this.ReadPacket(userSession, groupID, out objects, out relations);
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      List<string> stringList3 = new List<string>();
      for (int index = 0; index < objects.Count; ++index)
      {
        ObjectTag tag = new ObjectTag();
        if (!string.IsNullOrEmpty(ownerCode))
        {
          tag.OwnerCode = new char?(ownerCode[0]);
          if (ownerCode.Length >= 2)
            tag.CompositionOwnerCode = new char?(ownerCode[1]);
        }
        TransferedObject unit = new TransferedObject(objects[index].Item1, TransferedObjectCategory.GroupObject, (TransferedObjectTag) tag);
        publishTask.AddUnitData(unit);
        switch (objects[index].Item3)
        {
          case PublishObjectRootType.rtUnknown:
            stringList3.Add(objects[index].Item2.ToString());
            break;
          case PublishObjectRootType.rtArticle:
            stringList1.Add(objects[index].Item2.ToString());
            break;
          case PublishObjectRootType.rtDocument:
            stringList2.Add(objects[index].Item2.ToString());
            break;
        }
      }
      for (int index = 0; index < relations.Count; ++index)
      {
        TransferedObject unit = new TransferedObject(relations[index], TransferedObjectCategory.GroupRelation);
        publishTask.AddUnitData(unit);
      }
      if (TraceLog.Enabled)
        TraceLog.Write($"End UsePacket sessionGuid={sessionGuid} objects={objects.Count} relations={relations.Count}");
      return new string[3][]
      {
        stringList1.ToArray(),
        stringList2.ToArray(),
        stringList3.ToArray()
      };
    }
    catch (Exception ex)
    {
      this.SetErrorForTask(userSession, publishTask.DBTask, ex);
      throw;
    }
  }

  private void ReadPacket(
    IUserSession session,
    long packetID,
    out List<Tuple<Guid, Guid, PublishObjectRootType>> objects,
    out List<Guid> relations)
  {
    DataTable dataTable = session.GetRelationCollection(session.IdentHelper.SimpleRelationTypeID).ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[3]
    {
      new ColumnDescriptor((object) -12, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Default, SortOrders.NONE, 0),
      new ColumnDescriptor((object) PortalConsts.attributePublishObjectGUID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Default, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid("cad01544-306c-11d8-b4e9-00304f19f545")), AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Default, SortOrders.NONE, 0)
    }), packetID);
    objects = new List<Tuple<Guid, Guid, PublishObjectRootType>>(dataTable.Rows.Count);
    for (int index = 0; index < dataTable.Rows.Count; ++index)
      objects.Add(new Tuple<Guid, Guid, PublishObjectRootType>(new Guid(Convert.ToString(dataTable.Rows[index][0])), new Guid(Convert.ToString(dataTable.Rows[index][1])), (PublishObjectRootType) Convert.ToInt32(dataTable.Rows[index][2])));
    IDBAttribute attributeByGuid = session.GetObject(packetID).GetAttributeByGuid(PortalServerConsts.attributeRelationsList);
    relations = new List<Guid>(attributeByGuid.ValuesCount);
    for (int index = 0; index < attributeByGuid.ValuesCount; ++index)
    {
      if (index > 0)
        attributeByGuid.Index = index;
      if (GuidHelper.IsGuid(attributeByGuid.AsString))
        relations.Add(new Guid(attributeByGuid.AsString));
    }
  }

  public void PublishUnit(Guid sessionGuid, long taskID, TransferedObject unit)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start PublishUnit unit={unit.GUID} sessionGuid={sessionGuid} ");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    PublishTask publishTask = PublishTask.GetPublishTask(userSession, taskID);
    publishTask.Status = TaskStatus.Transmitting;
    try
    {
      publishTask.AddUnitData(unit);
      if (!TraceLog.Enabled)
        return;
      TraceLog.Write($"End PublishUnit unit={unit.GUID}");
    }
    catch (Exception ex)
    {
      this.SetErrorForTask(userSession, publishTask.DBTask, ex);
      throw;
    }
  }

  public void TransferPublishUnitFile(
    Guid sessionGuid,
    string unitGuid,
    string fileName,
    byte[] bytes,
    bool continuation)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start TransferPublishUnitFile unitGuid={unitGuid} fileName={fileName} continuation={continuation} sessionGuid={sessionGuid} ");
    this.GetUserSession(sessionGuid);
    string publishUnitPath = TempStorage.GetPublishUnitPath(unitGuid);
    TempStorage.CheckDirectory(publishUnitPath);
    FileInfo fileInfo = new FileInfo(Path.Combine(publishUnitPath, fileName));
    if (!fileInfo.Exists & continuation)
      throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_20"), (object) fileName));
    TempStorage.CheckAndCreateLocDirectory(publishUnitPath, fileName);
    FileMode mode = continuation ? FileMode.Append : FileMode.Create;
    using (FileStream fileStream = new FileStream(fileInfo.FullName, mode, FileAccess.Write))
    {
      if (bytes != null && bytes.Length != 0)
        fileStream.Write(bytes, 0, bytes.Length);
      if (!TraceLog.Enabled)
        return;
      TraceLog.Write($"End TransferPublishUnitFile unitGuid={unitGuid} fileName={fileName}");
    }
  }

  public void PublishObject(
    Guid sessionGuid,
    long taskID,
    string unitGuid,
    int changesType,
    int category,
    string[] dataFiles,
    bool inComposition,
    bool withComposition,
    string creatorCode,
    string ownerCode,
    string compositionOwnerCode,
    int rootType)
  {
    if (unitGuid == string.Empty || !GuidHelper.IsGuid(unitGuid))
      throw new ArgumentException(LocalizationHolder.rm.GetString("PortalServer_21"));
    if (string.IsNullOrEmpty(creatorCode))
      throw new ArgumentException("Отсутствует значение параметра creatorCode");
    ActionsHelper.ValuePresentInEnum(typeof (ChangeType), changesType, nameof (changesType));
    ActionsHelper.ValuePresentInEnum(typeof (TransferedObjectCategory), category, nameof (category));
    ActionsHelper.ValuePresentInEnum(typeof (PublishObjectRootType), rootType, nameof (rootType));
    TransferedObject unit = new TransferedObject((ChangeType) changesType, (TransferedObjectCategory) category, dataFiles)
    {
      GUID = unitGuid,
      Tag = (TransferedObjectTag) new ObjectTag(inComposition, withComposition, creatorCode[0], (PublishObjectRootType) rootType)
    };
    if (!string.IsNullOrEmpty(ownerCode))
      (unit.Tag as ObjectTag).OwnerCode = new char?(ownerCode[0]);
    if (!string.IsNullOrEmpty(compositionOwnerCode))
      (unit.Tag as ObjectTag).CompositionOwnerCode = new char?(compositionOwnerCode[0]);
    this.PublishUnit(sessionGuid, taskID, unit);
  }

  public void PublishRelation(
    Guid sessionGuid,
    long taskID,
    string unitGuid,
    int changesType,
    int category,
    string[] dataFiles)
  {
    if (unitGuid == string.Empty || !GuidHelper.IsGuid(unitGuid))
      throw new ArgumentException(LocalizationHolder.rm.GetString("PortalServer_21"));
    ActionsHelper.ValuePresentInEnum(typeof (ChangeType), changesType, nameof (changesType));
    ActionsHelper.ValuePresentInEnum(typeof (TransferedObjectCategory), category, nameof (category));
    TransferedObject unit = new TransferedObject((ChangeType) changesType, (TransferedObjectCategory) category, dataFiles)
    {
      GUID = unitGuid
    };
    this.PublishUnit(sessionGuid, taskID, unit);
  }

  public void DeletePublishTask(Guid sessionGuid, long taskID)
  {
    this.DeletePublishTask(sessionGuid, taskID, PortalConsts.DeleteWithoutFiles);
  }

  public void DeletePublishTask(Guid sessionGuid, long taskID, int deleteMode)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start DeletePublishTask taskID={taskID} sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    long objectID = !this._working.Contains(taskID) ? taskID : throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_22"), (object) taskID));
    userSession.GetObject(objectID, false)?.Delete((long) deleteMode);
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write($"End DeletePublishTask taskID={taskID}");
  }

  public int GetTaskStatus(Guid sessionGuid, long taskID)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetTaskStatus taskID={taskID} sessionGuid={sessionGuid}");
    IDBObject dbObject = this.GetUserSession(sessionGuid).GetObject(taskID, true);
    if (TraceLog.Enabled)
      TraceLog.Write($"End GetTaskStatus taskID={taskID}");
    Guid attributeTaskStatus = PortalConsts.attributeTaskStatus;
    return Convert.ToInt32(dbObject.GetAttributeByGuid(attributeTaskStatus).AsInteger);
  }

  public void CompletePublish(Guid sessionGuid, long taskID, bool deleteTask)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start CompletePublish from taskID={taskID} sessionGuid={sessionGuid}");
    if (this._working.Contains(taskID))
      throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_26"), (object) taskID));
    this._working.Add(taskID);
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    PublishTask publishTask = (PublishTask) null;
    List<string> stringList = new List<string>();
    PublishCaches caches = new PublishCaches();
    PackAnalyzInfo packAnalyzInfo = new PackAnalyzInfo();
    List<TransferedObject> trObjects = new List<TransferedObject>();
    try
    {
      publishTask = PublishTask.GetPublishTask(userSession, taskID);
      IDBAttribute attributeByGuid1 = publishTask.DBTask.GetAttributeByGuid(PortalConsts.attributePercent);
      this.SetTaskStatusTransmiting(publishTask.DBTask);
      attributeByGuid1.AsInteger = 1L;
      IDBAttribute attributeByGuid2 = publishTask.DBTask.GetAttributeByGuid(PortalServerConsts.attributePacketNumber);
      long asInteger = attributeByGuid2 == null || attributeByGuid2.AsInteger == 0L ? 0L : attributeByGuid2.AsInteger;
      string enabledSites = publishTask.EnabledSites;
      List<FileInfo> files = BackupTaskUnitFiles.FindFiles(publishTask.TaskFolder);
      ISitesCacheService customService = (ISitesCacheService) userSession.GetCustomService(typeof (ISitesCacheService));
      if (files.Count > 0)
      {
        IDBRelationType relationType = userSession.GetRelationType(PortalConsts.reltypePublish);
        IDBRelationCollection relationCollection = userSession.GetRelationCollection(relationType.RelationType);
        IDBObjectCollection objectCollection = userSession.GetObjectCollection(PortalConsts.objtypePublishObjects);
        Dictionary<Guid, int> partCounter = new Dictionary<Guid, int>(files.Count);
        List<Guid> importedObjects = new List<Guid>(files.Count);
        try
        {
          if (TraceLog.Enabled)
            TraceLog.Write($"Start analyz. Count of units={files.Count}");
          for (int index = 0; index < files.Count; ++index)
          {
            if (TraceLog.Enabled)
              TraceLog.Write($"Start analyz unit={files[index].Name}");
            IUnitAnalyzer analyzer = UnitAnalyzer.GetAnalyzer(userSession, files[index].FullName, enabledSites, customService, siteInfo);
            if (analyzer != null)
            {
              try
              {
                analyzer.Analysis(objectCollection, importedObjects, packAnalyzInfo, partCounter);
                if (analyzer.SiteForUpdate != string.Empty)
                  packAnalyzInfo.SiteForUpdate = analyzer.SiteForUpdate;
                if (analyzer.AutoTransfer)
                  packAnalyzInfo.IsAutoTransfer = true;
                int percent = ActionsHelper.CalculatePercent(files.Count, index, 2, 10);
                if (attributeByGuid1.AsInteger != (long) percent)
                  attributeByGuid1.AsInteger = (long) percent;
              }
              catch
              {
                if (TraceLog.Enabled && analyzer != null && analyzer.RootNode != null)
                {
                  TraceLog.Write("Unit rootNode:");
                  TraceLog.Write(analyzer.RootNode.InnerXml);
                }
                throw;
              }
              if (TraceLog.Enabled)
                TraceLog.Write("End analyz unit");
            }
          }
        }
        finally
        {
          if (TraceLog.Enabled)
            TraceLog.Write("End analyz");
        }
        attributeByGuid1.AsInteger = 11L;
        IDBImporter importer = userSession.GetImporter(string.Empty);
        if (TraceLog.Enabled)
          TraceLog.Write("Start publish units");
        GroupPublishItem packet = (GroupPublishItem) null;
        if (asInteger != 0L)
          packet = GroupPublishItem.GetPacket(userSession, asInteger);
        for (int index = 0; index < files.Count; ++index)
        {
          TransferedObject unit;
          IUnitPublisher publisher = UnitPublisher.GetPublisher(userSession, out unit, files[index].FullName, siteInfo, importer, packet);
          try
          {
            if (publisher != null)
            {
              if (!string.IsNullOrEmpty(publisher.UnitTempDirectory))
                stringList.Add(publisher.UnitTempDirectory);
              publisher.Publish(objectCollection, enabledSites, packet, packAnalyzInfo, caches, relationCollection, relationType);
            }
            else
              stringList.Add(TempStorage.GetPublishUnitPath(unit.GUID));
          }
          finally
          {
            if (packAnalyzInfo.IsAutoTransfer && unit.Category != TransferedObjectCategory.IncompleteRelation)
              trObjects.Add(unit);
            int percent = ActionsHelper.CalculatePercent(files.Count, index, 12, 67);
            if (attributeByGuid1.AsInteger != (long) percent)
              attributeByGuid1.AsInteger = (long) percent;
          }
        }
        packet?.CommitCreate();
        if (TraceLog.Enabled)
          TraceLog.Write("End publish");
        if (TraceLog.Enabled)
          TraceLog.Write($"Start correct relations object links {caches.RelationsWithLinks.Count}");
        attributeByGuid1.AsInteger = 68L;
        for (int index = 0; index < caches.RelationsWithLinks.Count; ++index)
        {
          Tuple<Guid, long> relationsWithLink = caches.RelationsWithLinks[index];
          IDBRelation relation = userSession.GetRelation(relationsWithLink.Item1, relationsWithLink.Item2, false);
          if (relation != null)
            PublishHelper.CorrectLinks(userSession.GetObjectCollection(PortalConsts.objtypePublishObjects), (IDBAttributable) relation, caches.ImportedObjectsIDs);
        }
        if (TraceLog.Enabled)
          TraceLog.Write("End correct relations object links");
        attributeByGuid1.AsInteger = 71L;
        if (TraceLog.Enabled)
          TraceLog.Write($"Start objects correct {caches.Objects.Count}");
        foreach (KeyValuePair<Guid, long> importedObjectsId in caches.ImportedObjectsIDs)
        {
          int index = 0;
          long objectID = importedObjectsId.Value;
          if (TraceLog.Enabled)
            TraceLog.Write($"...get object {caches.Objects[index]}");
          if (siteInfo.SystemType == SystemTypes.IPS && caches.Objects.Contains(objectID))
            this.CorrectComposition(userSession, siteInfo, relationCollection, objectID, importedObjectsId.Key, packAnalyzInfo, caches);
          if (caches.ObjectsWithLinks.Contains(objectID))
          {
            IDBObject dbObject = userSession.GetObject(objectID, false);
            if (dbObject != null)
              PublishHelper.CorrectLinks(userSession.GetObjectCollection(PortalConsts.objtypePublishObjects), (IDBAttributable) dbObject, caches.ImportedObjectsIDs);
          }
          int percent = ActionsHelper.CalculatePercent(caches.ImportedObjectsIDs.Count, index, 72, 86);
          if (attributeByGuid1.AsInteger != (long) percent)
            attributeByGuid1.AsInteger = (long) percent;
          int num = index + 1;
        }
        attributeByGuid1.AsInteger = 87L;
        if (packAnalyzInfo.IsAutoTransfer)
          CreateAutoTransferBase.Create(userSession, siteInfo, packAnalyzInfo, packet, trObjects);
        attributeByGuid1.AsInteger = 99L;
      }
      if (TraceLog.Enabled)
        TraceLog.Write("Delete temp files");
      if (stringList.Count > 0)
      {
        foreach (string path in stringList)
        {
          try
          {
            if (Directory.Exists(path))
              Directory.Delete(path, true);
          }
          catch (Exception ex)
          {
            if (TraceLog.Enabled)
              TraceLog.Write($"...delete folder {path} error: {ex.Message}");
          }
        }
      }
      publishTask.DBTask.GetAttributeByGuid(PortalConsts.attributeTaskStatus).AsInteger = 0L;
      attributeByGuid1.AsInteger = 100L;
      if (!deleteTask)
        return;
      publishTask.DBTask.Delete((long) PortalConsts.DeleteWithoutFiles);
      publishTask.ClearTempData();
    }
    catch (Exception ex)
    {
      if (TraceLog.Enabled)
        TraceLog.Write("CompletePublish Exception", ex);
      if (publishTask != null)
        this.SetErrorForTask(userSession, publishTask.DBTask, ex);
      throw;
    }
    finally
    {
      caches.Destroy();
      packAnalyzInfo.Destroy();
      this._working.Remove(taskID);
      if (TraceLog.Enabled)
        TraceLog.Write("End CompletePublish");
    }
  }

  private void SetErrorForTask(IUserSession session, IDBObject dbTask, Exception ex)
  {
    dbTask.GetAttributeByGuid(PortalConsts.attributeTaskStatus, false).AsInteger = 2L;
    (dbTask.GetAttributeByGuid(PortalConsts.attributeError) ?? dbTask.Attributes.AddAttribute(session.GetAttributeType(PortalConsts.attributeError).AttributeID, false)).AsString = LogException.Create(ex);
  }

  private void SetTaskStatusTransmiting(IDBObject dbTask)
  {
    IDBAttribute attributeByGuid1 = dbTask.GetAttributeByGuid(PortalConsts.attributeTaskStatus);
    if (attributeByGuid1.AsInteger != 3L)
      attributeByGuid1.AsInteger = 3L;
    IDBAttribute attributeByGuid2 = dbTask.GetAttributeByGuid(PortalConsts.attributeError);
    if (attributeByGuid2 == null)
      return;
    attributeByGuid2.AsString = string.Empty;
  }

  private void CorrectComposition(
    IUserSession session,
    SiteInfo info,
    IDBRelationCollection relCollection,
    long objectID,
    Guid publishObjGuid,
    PackAnalyzInfo packAnalyzInfo,
    PublishCaches caches)
  {
    if (!packAnalyzInfo.AnalyzObjectInfo[publishObjGuid].WithComposition)
      return;
    if (TraceLog.Enabled)
      TraceLog.Write("...correct composition");
    DataTable dataTable = relCollection.ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new object[2]
    {
      (object) -26,
      (object) -2
    }), objectID);
    IDBObjectCollection objectCollection = session.GetObjectCollection(PortalConsts.objtypePublishObjects);
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      Guid relationGuid = new Guid(Convert.ToString(dataTable.Rows[index][0]));
      if (!caches.Relations.Exists((Predicate<Tuple<Guid, long>>) (x => x.Item1.Equals(relationGuid) && x.Item2.Equals(objectID))))
      {
        DeleteAction deleteAction = new DeleteAction();
        IDBRelation relation = session.GetRelation(relationGuid, objectID, false);
        if (relation != null)
        {
          if (TraceLog.Enabled)
            TraceLog.Write($"...delete relation {relationGuid}");
          deleteAction.CheckAndDeleteRelation(session, info, objectCollection, relCollection, relation);
        }
        long int64 = Convert.ToInt64(dataTable.Rows[index][1]);
        if (!caches.Objects.Contains(int64))
          deleteAction.CheckAndDeleteVersion(session, info, relCollection, int64);
      }
    }
  }

  private IDBObject CreatePublishUnit(
    IUserSession session,
    long taskID,
    Guid objectType,
    Guid? guid)
  {
    IDBObject dbObject = session.GetObject(taskID);
    SiteInfo siteInfo = this.GetSiteInfo(session);
    IDBObject publishUnit = (IDBObject) null;
    if (guid.HasValue && !guid.Value.Equals(Guid.Empty))
      publishUnit = session.GetObject(guid.Value, false);
    bool flag = false;
    if (publishUnit == null)
    {
      IDBObjectCollection objectCollection = session.GetObjectCollection(objectType);
      publishUnit = !guid.HasValue || guid.Value.Equals(Guid.Empty) ? objectCollection.Create() : objectCollection.Create(guid.Value);
      flag = true;
    }
    publishUnit.GetAttributeByGuid(PortalConsts.attributeFirstPublishSite).Value = (object) siteInfo.Code;
    if (flag)
      publishUnit.CommitCreation(true);
    dbObject.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalServerConsts.attributePacketNumber), false).Value = (object) publishUnit.ObjectID;
    return publishUnit;
  }

  public long CreatePacket(
    Guid sessionGuid,
    long taskID,
    Guid guid,
    string name,
    string designation,
    string note,
    string enableSites)
  {
    try
    {
      if (TraceLog.Enabled)
        TraceLog.Write(string.Format("Start CreatePacket  \"{2}\" sessionGuid={0} taskID={1}", (object) sessionGuid, (object) taskID, (object) name));
      IUserSession userSession = this.GetUserSession(sessionGuid);
      IDBObject publishUnit = this.CreatePublishUnit(userSession, taskID, PortalServerConsts.objecttypePublishObjectsPacket, new Guid?(guid));
      IDBAttribute dbAttribute = publishUnit.GetAttributeByGuid(PortalConsts.attributeEnabledSites, false) ?? publishUnit.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeEnabledSites), false);
      char code = this.GetSiteInfo(userSession).Code;
      if (string.IsNullOrEmpty(enableSites))
        enableSites = code.ToString();
      else if (!enableSites.Contains(code.ToString()))
        enableSites += code.ToString();
      dbAttribute.AsString = enableSites;
      this.SetStringAttributeValue(publishUnit, new Guid("cad00020-306c-11d8-b4e9-00304f19f545"), name, true);
      this.SetStringAttributeValue(publishUnit, new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"), designation, true);
      this.SetStringAttributeValue(publishUnit, PortalConsts.attributePacketNote, note, true);
      if (TraceLog.Enabled)
        TraceLog.Write($"End CreatePacket taskID={taskID}");
      return publishUnit.ObjectID;
    }
    catch (Exception ex)
    {
      throw new Exception($"Ошибка при создании пакета для задачи {taskID}: {LogException.Create(ex)}");
    }
  }

  public void SetStringAttributeValue(IDBObject obj, Guid attributeGuid, string val, bool create)
  {
    if (string.IsNullOrEmpty(val))
      return;
    IDBAttribute dbAttribute = obj.GetAttributeByGuid(attributeGuid, false);
    if (dbAttribute == null)
    {
      if (!create)
        return;
      dbAttribute = obj.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(attributeGuid), false);
    }
    dbAttribute.AsString = val;
  }
}
