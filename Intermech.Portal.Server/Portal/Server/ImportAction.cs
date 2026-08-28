// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.ImportAction
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class ImportAction : PortalAction
{
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
    if (TraceLog.Enabled)
    {
      TraceLog.Write($"ImportObjectsEx: updateGuid={updateGuid} sessionGuid={sessionGuid}");
      StringBuilder stringBuilder1 = new StringBuilder();
      if (objectsIDs != null)
      {
        foreach (long objectsId in objectsIDs)
          stringBuilder1.Append(objectsId.ToString() + ";");
      }
      TraceLog.Write($"...objectsIDs: {stringBuilder1.ToString()}");
      StringBuilder stringBuilder2 = new StringBuilder();
      if (relationTypes != null)
      {
        foreach (string relationType in relationTypes)
          stringBuilder2.Append(relationType + ";");
      }
      TraceLog.Write($"...relationTypes: {stringBuilder2.ToString()}");
      StringBuilder stringBuilder3 = new StringBuilder();
      if (recursiveRelationTypes != null)
      {
        foreach (string recursiveRelationType in recursiveRelationTypes)
          stringBuilder3.Append(recursiveRelationType + ";");
      }
      TraceLog.Write($"...recursiveRelationTypes: {stringBuilder3.ToString()}");
      TraceLog.Write($"...ownBegin={ownBegin} autoUpdate={autoUpdate} withVersions={withVersions} recursive={recursive}");
    }
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    this.Import(userSession, siteInfo, updateGuid, objectsIDs, (string[]) null, ownBegin, autoUpdate, recursive ? -1 : 1, false, true);
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
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    this.Import(userSession, siteInfo, updateGuid, objIDs, filteredTypes, ownBegin, autoUpdate, countLevels, false, true);
  }

  public void CreateImportTask(
    ImportTasksDictionary importTasksDictionary,
    Guid sessionGuid,
    Guid updateGuid,
    long[] objIDs,
    string[] filteredTypes,
    bool ownBegin,
    bool autoUpdate,
    int countLevels)
  {
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    this.CreateImportTask(importTasksDictionary, userSession, siteInfo, updateGuid, objIDs, filteredTypes, ownBegin, autoUpdate, countLevels, false, true);
  }

  public void CreateImportTask(
    ImportTasksDictionary importTasksDictionary,
    IUserSession session,
    SiteInfo info,
    Guid updateGuid,
    long[] rootObjectIDs,
    string[] filteredTypes,
    bool ownBegin,
    bool autoUpdate,
    int countLevels,
    bool forUpdate,
    bool skipNotOwned)
  {
    importTasksDictionary.AddTask(updateGuid, new ImportInfo());
    new Thread(new ThreadStart(new ImportTask(importTasksDictionary, session.Clone($"ImportTask_{updateGuid}"), info, updateGuid, rootObjectIDs, filteredTypes, ownBegin, autoUpdate, countLevels, forUpdate, skipNotOwned, true).Import))
    {
      Name = $"ImportTask_{session.SessionGUID}",
      IsBackground = true
    }.Start();
  }

  public void Import(
    IUserSession session,
    SiteInfo info,
    Guid updateGuid,
    long[] rootObjectIDs,
    string[] filteredTypes,
    bool ownBegin,
    bool autoUpdate,
    int countLevels,
    bool forUpdate,
    bool skipNotOwned)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start Import from site={info.Code} sessionGuid={session.SessionGUID}");
    new ImportTask((ImportTasksDictionary) null, session, info, updateGuid, rootObjectIDs, filteredTypes, ownBegin, autoUpdate, countLevels, forUpdate, skipNotOwned, false).Import();
  }

  public string[] AutoImportComplete(Guid sessionGuid, long[] objectIDs, bool withComposition)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start AutoImportComplete sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    List<Tuple<long, bool>> objects;
    if (withComposition)
    {
      List<Tuple<Guid, long>> relations = new List<Tuple<Guid, long>>();
      objects = new List<Tuple<long, bool>>();
      CompositionHelper.GetComposition(userSession, objectIDs, (string[]) null, objects, relations, -1);
    }
    else
    {
      objects = new List<Tuple<long, bool>>();
      foreach (long objectId in objectIDs)
        objects.Add(new Tuple<long, bool>(objectId, false));
    }
    List<string> stringList = new List<string>();
    for (int index = 0; index < objects.Count; ++index)
    {
      IDBObject dbObject = userSession.GetObject(objects[index].Item1);
      IDBAttribute attributeById = dbObject.GetAttributeByID(IDHelper.AttributeCopyKeepersID);
      string str = attributeById != null ? attributeById.AsString : string.Empty;
      if (!string.IsNullOrEmpty(str) && str.Contains(siteInfo.Code.ToString()))
      {
        attributeById.AsString = str.Replace(siteInfo.Code.ToString(), string.Empty);
        stringList.Add(dbObject.GetAttributeByGuid(PortalConsts.attributePublishObjectGUID).AsString);
      }
    }
    if (TraceLog.Enabled)
      TraceLog.Write($"End AutoImportComplete site={siteInfo.Code}");
    return stringList.ToArray();
  }
}
