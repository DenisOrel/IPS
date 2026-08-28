// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.UpdateAction
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
using System.IO;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class UpdateAction : PortalAction
{
  public string[] GetUpdatesEx(Guid sessionGuid, string[] relationTypes)
  {
    return this.GetUpdates(sessionGuid, new CompositionApplicabilities((string[]) null, relationTypes));
  }

  public long CheckUpdate(Guid sessionGuid, Guid objectGuid)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start CheckUpdate objectGuid={objectGuid} sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    DataTable dataTable = userSession.GetObjectCollection(PortalConsts.objtypePublishObjects).Select(new DBRecordSetParams(new ConditionStructure[2]
    {
      new ConditionStructure(PortalConsts.attributeSitesForUpdate, RelationalOperators.Substring, (object) siteInfo.Code.ToString(), LogicalOperators.AND, 0),
      new ConditionStructure(PortalConsts.attributePublishObjectGUID, RelationalOperators.Equal, (object) objectGuid, LogicalOperators.AND, 0)
    }, new object[1]{ (object) -2 }));
    if (TraceLog.Enabled)
      TraceLog.Write($"End CheckUpdate site={siteInfo.Code} objectGuid={objectGuid}");
    return dataTable.Rows.Count <= 0 ? -1L : Convert.ToInt64(dataTable.Rows[0][0]);
  }

  public string[] GetUpdates(Guid sessionGuid, CompositionApplicabilities applic)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetUpdates sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    DataTable dataTable1 = userSession.GetObjectCollection(PortalConsts.objtypePublishObjects).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(PortalConsts.attributeSitesForUpdate, RelationalOperators.Substring, (object) siteInfo.Code.ToString(), LogicalOperators.AND, 0)
    }, new object[1]{ (object) -2 }));
    if (dataTable1.Rows.Count > 0)
    {
      List<long> longList = new List<long>(dataTable1.Rows.Count);
      for (int index = 0; index < dataTable1.Rows.Count; ++index)
      {
        long int64 = Convert.ToInt64(dataTable1.Rows[index][0]);
        IDBAttribute attributeByGuid = userSession.GetObject(int64).GetAttributeByGuid(PortalConsts.attributeSitesForUpdate);
        attributeByGuid.AsString = attributeByGuid.AsString.Replace(siteInfo.Code.ToString(), string.Empty);
        longList.Add(int64);
      }
      new ImportAction().Import(userSession, siteInfo, Guid.NewGuid(), longList.ToArray(), (string[]) null, false, false, -1, true, true);
    }
    DataTable dataTable2 = userSession.GetObjectCollection(PortalConsts.objtypeChanges).Select(new DBRecordSetParams(new ConditionStructure[2]
    {
      new ConditionStructure(PortalConsts.attributeTaskStatus, RelationalOperators.Equal, (object) 4, LogicalOperators.AND, 0),
      new ConditionStructure(PortalServerConsts.attributeSiteId, RelationalOperators.Equal, (object) siteInfo.ID, LogicalOperators.AND, 0)
    }, new object[2]{ (object) -12, (object) -13 }, new object[1]
    {
      (object) -13
    }, new SortOrders[1]{ SortOrders.ASC }));
    if (dataTable2.Rows.Count > 0)
    {
      string[] updates = new string[dataTable2.Rows.Count];
      for (int index = 0; index < dataTable2.Rows.Count; ++index)
        updates[index] = Convert.ToString(dataTable2.Rows[index][0]);
      if (TraceLog.Enabled)
        TraceLog.Write($"End GetUpdates site={siteInfo.Code} updates={dataTable2.Rows.Count}");
      return updates;
    }
    if (TraceLog.Enabled)
      TraceLog.Write($"End GetUpdates site={siteInfo.Code}");
    return (string[]) null;
  }

  public string GetUpdateAuthor(Guid sessionGuid, string updateGUID)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetUpdateAuthor updateGUID={updateGUID} sessionGuid={sessionGuid}");
    IDBAttribute attributeByGuid = ((updateGUID != null && GuidHelper.IsGuid(updateGUID) ? this.GetUserSession(sessionGuid).GetObject(new Guid(updateGUID), false) : throw new ArgumentException(LocalizationHolder.rm.GetString("PortalServer_39"))) ?? throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_40"), (object) updateGUID))).GetAttributeByGuid(PortalConsts.attributeFirstPublishSite, false);
    return attributeByGuid == null ? string.Empty : attributeByGuid.AsString;
  }

  public TransferedObject[] GetUpdateUnit(Guid sessionGuid, string updateGUID)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetUpdateUnit updateGUID={updateGUID} sessionGuid={sessionGuid}");
    List<TransferedObject> transferedObjectList = UpdateDataAttributeHelper.Load(((updateGUID != null && GuidHelper.IsGuid(updateGUID) ? this.GetUserSession(sessionGuid).GetObject(new Guid(updateGUID), false) : throw new ArgumentException(LocalizationHolder.rm.GetString("PortalServer_39"))) ?? throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_40"), (object) updateGUID))).GetAttributeByGuid(PortalServerConsts.attributeUpdateData, false) ?? throw new Exception(LocalizationHolder.rm.GetString("PortalServer_41")), false);
    if (TraceLog.Enabled)
      TraceLog.Write($"End GetUpdateUnit updateGUID={updateGUID}");
    return transferedObjectList.ToArray();
  }

  public byte[] GetUpdateAttributesFile(
    Guid sessionGuid,
    Guid transferedGuid,
    string fileName,
    long startPosition)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetUpdateAttributesFile transferedGuid={transferedGuid} fileName={fileName} sessionGuid={sessionGuid}");
    this.GetUserSession(sessionGuid);
    FileInfo fileInfo = new FileInfo(Path.Combine(TempStorage.GetUpdateUnitPath(transferedGuid.ToString()), fileName));
    if (!fileInfo.Exists)
      throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_45"), (object) fileName));
    using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
    {
      if (fileStream.Length == 0L || startPosition == fileStream.Length)
        return new byte[0];
      if (startPosition < 0L || startPosition > fileStream.Length)
        throw new Exception(LocalizationHolder.rm.GetString("PortalServer_46"));
      fileStream.Position = startPosition;
      byte[] updateAttributesFile = new byte[PortalConsts.DefaultFileTransferBufferLength];
      int length = fileStream.Read(updateAttributesFile, 0, PortalConsts.DefaultFileTransferBufferLength);
      if (TraceLog.Enabled)
        TraceLog.Write($"End GetUpdateAttributesFile transferedGuid={transferedGuid} fileName={fileName}");
      if (length >= PortalConsts.DefaultFileTransferBufferLength)
        return updateAttributesFile;
      byte[] destinationArray = new byte[length];
      Array.Copy((Array) updateAttributesFile, (Array) destinationArray, length);
      return destinationArray;
    }
  }

  public long GetUpdateAttributesFileLength(Guid sessionGuid, Guid transferedGuid, string fileName)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetUpdateAttributesFileLength transferedGuid={transferedGuid} fileName={fileName} sessionGuid={sessionGuid}");
    FileInfo fileInfo = new FileInfo(Path.Combine(TempStorage.GetUpdateUnitPath(transferedGuid.ToString()), fileName));
    if (!fileInfo.Exists)
      throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_45"), (object) fileName));
    if (TraceLog.Enabled)
      TraceLog.Write($"End GetUpdateAttributesFileLength transferedGuid={transferedGuid} fileName={fileName}");
    return fileInfo.Length;
  }

  public void EndUpdateUnit(Guid sessionGuid, string updateGUID)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start EndUpdateUnit updateGUID={updateGUID} sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    try
    {
      if (updateGUID == null || !GuidHelper.IsGuid(updateGUID))
        throw new ArgumentException(LocalizationHolder.rm.GetString("PortalServer_39"));
      SiteUpdate.Delete(userSession, TempStorage.RootFolder, siteInfo, new Guid(updateGUID));
    }
    catch (ArgumentException ex)
    {
      userSession.EventLog.AddToTrace(string.Format(LocalizationHolder.rm.GetString("PortalServer_47"), (object) updateGUID, (object) ex.Message), Consts.traceAlways, string.Empty);
    }
    catch (Exception ex)
    {
      string TraceFileName = $"error_{updateGUID}_{DateTime.Now.Ticks}.xml";
      userSession.EventLog.AddToTrace(LogException.Create(ex), Consts.traceAlways, TraceFileName);
      userSession.EventLog.AddToTrace(string.Format(LocalizationHolder.rm.GetString("PortalServer_48"), (object) updateGUID, (object) ex.Message, (object) TraceFileName), Consts.traceAlways, string.Empty);
    }
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write($"End EndUpdateUnit site={siteInfo.Code} updateGUID={updateGUID}");
  }

  public void EndUpdateUnit(Guid sessionGuid, string updateGUID, string[] guids)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start EndUpdateUnit with guids  updateGUID={updateGUID} sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    if (guids != null && guids.Length != 0)
    {
      DataTable dataTable = userSession.GetObjectCollection(PortalConsts.objtypePublishObjects).Select(new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(PortalConsts.attributePublishObjectGUID, RelationalOperators.In, (object) guids, LogicalOperators.AND, 0)
      }, new object[1]{ (object) -2 }));
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        IDBObject dbObject = userSession.GetObject(Convert.ToInt64(dataTable.Rows[index][0]));
        ActionsHelper.SetOwner(siteInfo, userSession, dbObject, true);
      }
    }
    this.EndUpdateUnit(sessionGuid, updateGUID);
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write($"End EndUpdateUnit with guids site={siteInfo.Code} updateGUID={updateGUID}");
  }

  public void StartUpdateUnit(Guid sessionGuid, string updateGUID)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start StartUpdateUnit updateGUID={updateGUID} sessionGuid={sessionGuid}");
    if (updateGUID == null || !GuidHelper.IsGuid(updateGUID))
      throw new ArgumentException(LocalizationHolder.rm.GetString("PortalServer_39"));
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    userSession.GetObject(new Guid(updateGUID), true).GetAttributeByGuid(PortalConsts.attributeTaskStatus).AsInteger = 3L;
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write($"End StartUpdateUnit site={siteInfo.Code} updateGUID={updateGUID}");
  }

  public string[][] GetUpdateUnitEx(Guid sessionGuid, string updateGuid)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetUpdateUnitEx updateGuid={updateGuid} sessionGuid={sessionGuid}");
    TransferedObject[] updateUnit = this.GetUpdateUnit(sessionGuid, updateGuid);
    if (updateUnit != null && updateUnit.Length != 0)
    {
      List<string[]> strArrayList = new List<string[]>(updateUnit.Length);
      for (int index1 = 0; index1 < updateUnit.Length; ++index1)
      {
        string empty1 = string.Empty;
        if (updateUnit[index1].DataFiles != null)
        {
          bool flag = true;
          for (int index2 = 0; index2 < updateUnit[index1].DataFiles.Length; ++index2)
          {
            if (flag)
              flag = false;
            else
              empty1 += ";";
            empty1 += updateUnit[index1].DataFiles[index2];
          }
        }
        List<string> stringList1 = new List<string>((IEnumerable<string>) new string[4]
        {
          updateUnit[index1].GUID,
          Convert.ToString((object) updateUnit[index1].ChangesType),
          Convert.ToString((object) updateUnit[index1].Category),
          empty1
        });
        if (updateUnit[index1].Tag != null && updateUnit[index1].Tag is ObjectTag)
        {
          ObjectTag tag = updateUnit[index1].Tag as ObjectTag;
          stringList1.Add(tag.CreatorCode.ToString());
          List<string> stringList2 = stringList1;
          char ch;
          string empty2;
          if (!tag.OwnerCode.HasValue)
          {
            empty2 = string.Empty;
          }
          else
          {
            ch = tag.OwnerCode.Value;
            empty2 = ch.ToString();
          }
          stringList2.Add(empty2);
          List<string> stringList3 = stringList1;
          string empty3;
          if (!tag.CompositionOwnerCode.HasValue)
          {
            empty3 = string.Empty;
          }
          else
          {
            ch = tag.CompositionOwnerCode.Value;
            empty3 = ch.ToString();
          }
          stringList3.Add(empty3);
        }
        strArrayList.Add(stringList1.ToArray());
      }
      if (TraceLog.Enabled)
        TraceLog.Write($"End GetUpdateUnitEx updateGuid={updateGuid} result={strArrayList.Count}");
      return strArrayList.ToArray();
    }
    if (TraceLog.Enabled)
      TraceLog.Write($"End GetUpdateUnitEx updateGuid={updateGuid}");
    return (string[][]) null;
  }

  public string GetUpdateAttributesFileEx(
    Guid sessionGuid,
    Guid transferedGuid,
    string fileName,
    long startPosition)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetUpdateAttributesFileEx transferedGuid={transferedGuid} fileName={fileName} startPosition={startPosition} sessionGuid={sessionGuid}");
    byte[] updateAttributesFile = this.GetUpdateAttributesFile(sessionGuid, transferedGuid, fileName, startPosition);
    if (TraceLog.Enabled)
      TraceLog.Write($"End GetUpdateAttributesFileEx transferedGuid={transferedGuid} fileName={fileName} startPosition={startPosition}");
    return updateAttributesFile != null && updateAttributesFile.Length != 0 ? Convert.ToBase64String(updateAttributesFile) : string.Empty;
  }

  public void SetUpdateUnitError(Guid sessionGuid, string updateGuid, string errorText)
  {
    IDBObject dbObject = this.GetUserSession(sessionGuid).GetObject(new Guid(updateGuid), true);
    dbObject.GetAttributeByGuid(PortalConsts.attributeTaskStatus).AsInteger = 2L;
    (dbObject.GetAttributeByID(IDHelper.AttributeErrorTextID) ?? dbObject.Attributes.AddAttribute(IDHelper.AttributeErrorTextID, false)).Value = (object) errorText;
  }

  public void SetUpdateUnitStatus(Guid sessionGuid, string updateGuid, int statusID)
  {
    this.GetUserSession(sessionGuid).GetObject(new Guid(updateGuid), true).GetAttributeByGuid(PortalConsts.attributeTaskStatus).AsInteger = Enum.IsDefined(typeof (TaskStatus), (object) statusID) ? (long) statusID : throw new Exception($"Не найден статус со значением {statusID}");
  }
}
