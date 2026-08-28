// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.Portal
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Portal;
using Intermech.Interfaces.WebPortal;
using Intermech.Protection;
using System;

#nullable disable
namespace Intermech.Portal.Server;

public class Portal : LongLifeObject, IPortal
{
  private readonly ImportTasksDictionary _importTasksDictionary = new ImportTasksDictionary();

  public PortalObjectType[] GetObjectTypesTree(Guid sessionGuid)
  {
    return new MetadataInfoAction().GetObjectTypesTree(sessionGuid);
  }

  public string[][] GetPublishObjectTypes(Guid sessionGuid)
  {
    return new MetadataInfoAction().GetPublishObjectTypes(sessionGuid);
  }

  public string[][] GetAttributesForPublishObjectType(Guid sessionGuid, int objectTypeID)
  {
    return new MetadataInfoAction().GetAttributesForPublishObjectType(sessionGuid, objectTypeID);
  }

  public PortalAttributeType[] GetPublishRelationAttributes(Guid sessionGuid)
  {
    return new MetadataInfoAction().GetPublishRelationAttributes(sessionGuid);
  }

  public AttributePossibleValues[] GetAttributePossibleValues(Guid sessionGuid)
  {
    return new MetadataInfoAction().GetAttributePossibleValues(sessionGuid);
  }

  public DateTime GetLasModifyMetadata(Guid sessionGuid)
  {
    return new MetadataInfoAction().GetLasModifyMetadata(sessionGuid);
  }

  public byte[] GetPacketContent(Guid sessionGuid, long packetID)
  {
    return new PacketAction().GetPacketContent(sessionGuid, packetID);
  }

  public void PacketImportComplete(Guid sessionGuid, long packetID)
  {
    new PacketAction().ImportComplete(sessionGuid, packetID);
  }

  public void ImportPackets(Guid sessionGuid, Guid updateGuid, long[] packetIDs)
  {
    new PacketAction().ImportPackets(sessionGuid, updateGuid, packetIDs);
  }

  public void DeletePackets(Guid sessionGuid, long[] packetIDs)
  {
    new PacketAction().DeletePackets(sessionGuid, packetIDs);
  }

  public PublicationReceipt[] GetImportReceipts(Guid sessionGuid, long packetID)
  {
    return new PacketAction().GetImportReceipts(sessionGuid, packetID);
  }

  public byte[] GetReceiptContent(Guid sessionGuid, long receiptID)
  {
    return new PacketAction().GetReceiptContent(sessionGuid, receiptID);
  }

  public long CreatePacket(
    Guid sessionGuid,
    long taskID,
    string guid,
    string name,
    string designation,
    string note,
    string enableSites)
  {
    return new PublishAction().CreatePacket(sessionGuid, taskID, new Guid(guid), name, designation, note, enableSites);
  }

  public long CreateGroup(Guid sessionGuid, long taskID)
  {
    return new PublishAction().CreateGroup(sessionGuid, taskID);
  }

  public long StartPublishingTask(Guid sessionGuid, string taskName)
  {
    return new PublishAction().StartPublishingTask(sessionGuid, taskName, (string) null);
  }

  public void TransferPublishUnitFileEx(
    Guid sessionGuid,
    string unitGuid,
    string fileName,
    string bytes,
    bool continuation)
  {
    new PublishAction().TransferPublishUnitFileEx(sessionGuid, unitGuid, fileName, bytes, continuation);
  }

  public long StartPublishingTask(Guid sessionGuid, string taskName, string enabledSites)
  {
    return new PublishAction().StartPublishingTask(sessionGuid, taskName, enabledSites);
  }

  public long StartPublishingTask(
    Guid sessionGuid,
    string taskName,
    string enabledSites,
    long packetID)
  {
    return new PublishAction().StartPublishingTask(sessionGuid, taskName, enabledSites, packetID);
  }

  public void PublishUnit(Guid sessionGuid, long taskID, TransferedObject unit)
  {
    new PublishAction().PublishUnit(sessionGuid, taskID, unit);
  }

  public void TransferPublishUnitFile(
    Guid sessionGuid,
    string unitGuid,
    string fileName,
    byte[] bytes,
    bool continuation)
  {
    new PublishAction().TransferPublishUnitFile(sessionGuid, unitGuid, fileName, bytes, continuation);
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
    int rootType)
  {
    new PublishAction().PublishObject(sessionGuid, taskID, unitGuid, changesType, category, dataFiles, inComposition, withComposition, creatorCode, ownerCode, (string) null, rootType);
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
    new PublishAction().PublishObject(sessionGuid, taskID, unitGuid, changesType, category, dataFiles, inComposition, withComposition, creatorCode, ownerCode, compositionOwnerCode, rootType);
  }

  public void PublishRelation(
    Guid sessionGuid,
    long taskID,
    string unitGuid,
    int changesType,
    int category,
    string[] dataFiles)
  {
    new PublishAction().PublishRelation(sessionGuid, taskID, unitGuid, changesType, category, dataFiles);
  }

  public void DeletePublishTask(Guid sessionGuid, long taskID)
  {
    new PublishAction().DeletePublishTask(sessionGuid, taskID);
  }

  public void DeletePublishTask(Guid sessionGuid, long taskID, int deleteMode)
  {
    new PublishAction().DeletePublishTask(sessionGuid, taskID, deleteMode);
  }

  public int GetTaskStatus(Guid sessionGuid, long taskID)
  {
    return new PublishAction().GetTaskStatus(sessionGuid, taskID);
  }

  public void CompletePublish(Guid sessionGuid, long taskID, bool deleteTask)
  {
    new PublishAction().CompletePublish(sessionGuid, taskID, deleteTask);
  }

  public string[][] UseGroup(Guid sessionGuid, long taskID, long groupID, string ownerCode)
  {
    return new PublishAction().UseGroup(sessionGuid, taskID, groupID, ownerCode);
  }

  public void CancelPublish(Guid sessionGuid, long taskID)
  {
  }

  public void DeleteGroup(Guid sessionGuid, long packetID, bool withObjects)
  {
    new PublishAction().DeleteGroup(sessionGuid, packetID, withObjects);
  }

  public string[] GetUpdatesEx(Guid sessionGuid, string[] relationTypes)
  {
    return new UpdateAction().GetUpdatesEx(sessionGuid, relationTypes);
  }

  public long CheckUpdate(Guid sessionGuid, Guid objectGuid)
  {
    return new UpdateAction().CheckUpdate(sessionGuid, objectGuid);
  }

  public string[] GetUpdates(Guid sessionGuid, CompositionApplicabilities applic)
  {
    return new UpdateAction().GetUpdates(sessionGuid, applic);
  }

  public string GetUpdateAuthor(Guid sessionGuid, string updateGUID)
  {
    return new UpdateAction().GetUpdateAuthor(sessionGuid, updateGUID);
  }

  public TransferedObject[] GetUpdateUnit(Guid sessionGuid, string updateGUID)
  {
    return new UpdateAction().GetUpdateUnit(sessionGuid, updateGUID);
  }

  public byte[] GetUpdateAttributesFile(
    Guid sessionGuid,
    Guid transferedGuid,
    string fileName,
    long startPosition)
  {
    return new UpdateAction().GetUpdateAttributesFile(sessionGuid, transferedGuid, fileName, startPosition);
  }

  public long GetUpdateAttributesFileLength(Guid sessionGuid, Guid transferedGuid, string fileName)
  {
    return new UpdateAction().GetUpdateAttributesFileLength(sessionGuid, transferedGuid, fileName);
  }

  public void EndUpdateUnit(Guid sessionGuid, string updateGUID)
  {
    new UpdateAction().EndUpdateUnit(sessionGuid, updateGUID);
  }

  public void EndUpdateUnit(Guid sessionGuid, string updateGUID, string[] guids)
  {
    new UpdateAction().EndUpdateUnit(sessionGuid, updateGUID, guids);
  }

  public void StartUpdateUnit(Guid sessionGuid, string updateGUID)
  {
    new UpdateAction().StartUpdateUnit(sessionGuid, updateGUID);
  }

  public string[][] GetUpdateUnitEx(Guid sessionGuid, string updateGuid)
  {
    return new UpdateAction().GetUpdateUnitEx(sessionGuid, updateGuid);
  }

  public string GetUpdateAttributesFileEx(
    Guid sessionGuid,
    Guid transferedGuid,
    string fileName,
    long startPosition)
  {
    return new UpdateAction().GetUpdateAttributesFileEx(sessionGuid, transferedGuid, fileName, startPosition);
  }

  public void SetUpdateUnitStatus(Guid sessionGuid, string updateGuid, int statusID)
  {
    new UpdateAction().SetUpdateUnitStatus(sessionGuid, updateGuid, statusID);
  }

  public void SetUpdateUnitError(Guid sessionGuid, string updateGuid, string errorText)
  {
    new UpdateAction().SetUpdateUnitError(sessionGuid, updateGuid, errorText);
  }

  public long[] GetImportComposition(
    Guid sessionGuid,
    long[] objectIDs,
    string[] filteredTypes,
    int countLevels)
  {
    return new ObjectCollectionAction().GetImportComposition(sessionGuid, objectIDs, filteredTypes, countLevels);
  }

  public PublishObjectsTable SelectPublishObjects(
    Guid sessionGuid,
    int objectType,
    DBQueryParams dbParams)
  {
    return new ObjectCollectionAction().SelectPublishObjects(sessionGuid, objectType, dbParams);
  }

  public PublishObjectsTable SelectComposition(
    Guid sessionGuid,
    long objectID,
    DBQueryParams dbParams,
    int countLevels)
  {
    return new ObjectCollectionAction().SelectComposition(sessionGuid, objectID, dbParams, countLevels);
  }

  public string[][] SelectPublishObjectsEx(
    Guid sessionGuid,
    int objectType,
    string[] columns,
    int recordCount)
  {
    return new ObjectCollectionAction().SelectPublishObjectsEx(sessionGuid, objectType, columns, recordCount);
  }

  public string[][] SelectPublishObjectsEx(
    Guid sessionGuid,
    int objectType,
    string[] columns,
    int recordCount,
    string[] attributes,
    int[] relationalOperators,
    string[] values,
    string[] values2,
    int[] logicalOperators,
    int[] groupIDs,
    bool[] caseSensitives)
  {
    return new ObjectCollectionAction().SelectPublishObjectsEx(sessionGuid, objectType, columns, recordCount, attributes, relationalOperators, values, values2, logicalOperators, groupIDs, caseSensitives);
  }

  public string GetObjectAttributesEx(Guid sessionGuid, long objectID, params string[] attrIDs)
  {
    return new ObjectCollectionAction().GetObjectAttributesEx(sessionGuid, objectID, attrIDs);
  }

  public string GetRelationAttributesEx(Guid sessionGuid, long relationID, params string[] attrIDs)
  {
    return new ObjectCollectionAction().GetRelationAttributesEx(sessionGuid, relationID, attrIDs);
  }

  public PublishAttribute[] GetObjectAttributes(Guid sessionGuid, long objectID, string[] attrIDs)
  {
    return new ObjectCollectionAction().GetObjectAttributes(sessionGuid, objectID, attrIDs);
  }

  public PublishAttribute[] GetRelationAttributes(
    Guid sessionGuid,
    long relationID,
    params string[] attrIDs)
  {
    return new ObjectCollectionAction().GetRelationAttributes(sessionGuid, relationID, attrIDs);
  }

  public void ClearComposition(Guid sessionGuid, string objectGuid, string[] relationTypes)
  {
    new DeleteAction().ClearComposition(sessionGuid, objectGuid, relationTypes);
  }

  public void DeleteObjects(Guid sessionGuid, long[] objectIDs)
  {
    new DeleteAction().DeleteObjects(sessionGuid, objectIDs);
  }

  public string[] DeleteObjectsEx(Guid sessionGuid, long[] objectIDs)
  {
    return new DeleteAction().DeleteObjectsEx(sessionGuid, objectIDs);
  }

  public ImportInfo GetImportInfo(Guid updateGuid)
  {
    return this._importTasksDictionary.GetInfo(updateGuid);
  }

  public string[] AutoImportComplete(Guid sessionGuid, long[] objectIDs, bool withComposition)
  {
    return new ImportAction().AutoImportComplete(sessionGuid, objectIDs, withComposition);
  }

  public void ImportObjectsEx(
    Guid sessionGuid,
    Guid updateGuid,
    long[] objectsIDs,
    string[] relationTypes,
    string[] recursiveRelationTypes,
    bool ownBegin,
    bool autoUpdate,
    bool withVersions,
    bool recursive)
  {
    new ImportAction().ImportObjectsEx(sessionGuid, updateGuid, objectsIDs, relationTypes, recursiveRelationTypes, ownBegin, autoUpdate, withVersions, recursive);
  }

  public void ImportObjects(
    Guid sessionGuid,
    Guid updateGuid,
    long[] objIDs,
    string[] filteredTypes,
    bool ownBegin,
    bool autoUpdate,
    int countLevels)
  {
    new ImportAction().ImportObjects(sessionGuid, updateGuid, objIDs, filteredTypes, ownBegin, autoUpdate, countLevels);
  }

  public void CreateImportTask(
    Guid sessionGuid,
    Guid updateGuid,
    long[] objIDs,
    string[] filteredTypes,
    bool ownBegin,
    bool autoUpdate,
    int countLevels)
  {
    new ImportAction().CreateImportTask(this._importTasksDictionary, sessionGuid, updateGuid, objIDs, filteredTypes, ownBegin, autoUpdate, countLevels);
  }

  public string[] OwnCompleteEx(
    Guid sessionGuid,
    string[] objectGuids,
    string ownerSites,
    string[] relationTypes,
    string[] recursiveRelationTypes,
    bool recursive,
    bool skipNotOwned,
    bool autoUpdate)
  {
    return new OwnCompleteAction().OwnCompleteEx(sessionGuid, objectGuids, ownerSites, relationTypes, recursiveRelationTypes, recursive, skipNotOwned, autoUpdate);
  }

  public string[] OwnCompleteEx(
    Guid sessionGuid,
    long[] objectIDs,
    string ownerSites,
    string[] relationTypes,
    string[] recursiveRelationTypes,
    bool recursive,
    bool skipNotOwned,
    bool autoUpdate)
  {
    return new OwnCompleteAction().OwnCompleteEx(sessionGuid, objectIDs, ownerSites, relationTypes, recursiveRelationTypes, recursive, skipNotOwned, autoUpdate);
  }

  public string[] OwnComplete(
    Guid sessionGuid,
    long[] objectIDs,
    string ownerSites,
    bool withComposition,
    bool skipNotOwned,
    bool autoUpdate)
  {
    return new OwnCompleteAction().OwnComplete(sessionGuid, objectIDs, ownerSites, withComposition, skipNotOwned, autoUpdate);
  }

  public ProcessTemplateInfo[] GetProcessTemplates(Guid sessionGuid, Guid siteGuid)
  {
    return new WorkflowAction().GetProcessTemplates(sessionGuid, siteGuid);
  }

  public void ChangeUserPassword(Guid sessionGuid, string login, string newPassword)
  {
    new UserAction().ChangeUserPassword(sessionGuid, login, newPassword);
  }

  public void AddUser(
    Guid sessionGuid,
    string userName,
    string login,
    string password,
    Guid userGuid)
  {
    new UserAction().AddUser(sessionGuid, userName, login, password, userGuid);
  }

  public void ChangeUserPassword(Guid sessionGuid, string login, PswPackage newPassword)
  {
    new UserAction().ChangeUserPassword(sessionGuid, login, newPassword);
  }

  public void AddUser(
    Guid sessionGuid,
    string userName,
    string login,
    PswPackage password,
    Guid userGuid)
  {
    new UserAction().AddUser(sessionGuid, userName, login, password, userGuid);
  }

  public void DeleteUser(Guid sessionGuid, string login)
  {
    new UserAction().DeleteUser(sessionGuid, login);
  }

  public PublishObjectsTable GetSiteUsers(
    Guid sessionGuid,
    string siteGuid,
    DBQueryParams dbParams)
  {
    return new UserAction().GetSiteUsers(sessionGuid, siteGuid, dbParams);
  }

  public void ImportUsers(Guid sessionGuid, Guid updateGuid, long[] userIDs)
  {
    new UserAction().ImportUsers(sessionGuid, updateGuid, userIDs);
  }

  public bool IsAdmin(Guid sessionGuid) => new UserAction().IsAdmin(sessionGuid);

  public char GetSiteCode(Guid sessionGuid, string siteGuid)
  {
    return new InfoAction().GetSiteCode(sessionGuid, siteGuid);
  }

  public DateTime GetLastSitesInfoUpdate() => new InfoAction().GetLastSitesInfoUpdate();

  public SiteInfo[] GetSitesInfo(Guid sessionGuid) => new InfoAction().GetSitesInfo(sessionGuid);

  public string Login(
    string login,
    string password,
    string siteGUID,
    string computerName,
    int timeZone)
  {
    return new SecurityAction().Login(login, password, siteGUID, computerName, timeZone);
  }

  public void Logout(Guid sessionGuid) => new SecurityAction().Logout(sessionGuid);

  public IUserSession GetSession(Guid sessionGuid) => new SecurityAction().GetSession(sessionGuid);

  public string Login(
    string login,
    PswPackage password,
    string siteGUID,
    string computerName,
    int timeZone)
  {
    return new SecurityAction().Login(login, password, siteGUID, computerName, timeZone);
  }

  public string Version => this.GetType().Assembly.GetName().Version.ToString();
}
