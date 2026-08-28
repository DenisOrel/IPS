// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.OwnCompleteAction
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

internal sealed class OwnCompleteAction : PortalAction
{
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
    IUserSession userSession = this.GetUserSession(sessionGuid);
    this.GetSiteInfo(userSession);
    List<object> objectList = new List<object>(objectGuids.Length);
    for (int index = 0; index < objectGuids.Length; ++index)
      objectList.Add((object) new Guid(objectGuids[index]));
    DataTable dataTable = userSession.GetObjectCollection(PortalConsts.objtypePublishObjects).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(PortalConsts.attributePublishObjectGUID, RelationalOperators.In, (object) objectList.ToArray(), LogicalOperators.AND, 0)
    }, new object[1]{ (object) -2 }));
    if (dataTable.Rows.Count != objectGuids.Length)
      throw new Exception(LocalizationHolder.rm.GetString("PortalServer_61"));
    List<long> longList = new List<long>(objectGuids.Length);
    for (int index = 0; index < dataTable.Rows.Count; ++index)
      longList.Add(Convert.ToInt64(dataTable.Rows[index][0]));
    return this.OwnCompleteEx(sessionGuid, longList.ToArray(), ownerSites, relationTypes, recursiveRelationTypes, recursive, skipNotOwned, autoUpdate);
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
    return this.OwnComplete(sessionGuid, objectIDs, ownerSites, recursive, skipNotOwned, autoUpdate);
  }

  public string[] OwnComplete(
    Guid sessionGuid,
    long[] objectIDs,
    string ownerSites,
    bool withComposition,
    bool skipNotOwned,
    bool autoUpdate)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start OwnComplete sessionGuid={sessionGuid}");
    if (objectIDs == null || objectIDs.Length == 0)
      throw new Exception(LocalizationHolder.rm.GetString("PortalServer_62"));
    if (ownerSites == null || ownerSites == string.Empty)
      throw new Exception(LocalizationHolder.rm.GetString("PortalServer_63"));
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    List<string> stringList = new List<string>();
    List<Tuple<long, bool>> objects = new List<Tuple<long, bool>>();
    List<Tuple<Guid, long>> relations = new List<Tuple<Guid, long>>();
    if (withComposition)
    {
      CompositionHelper.GetComposition(userSession, objectIDs, (string[]) null, objects, relations, -1);
    }
    else
    {
      objects = new List<Tuple<long, bool>>();
      foreach (long linkedObjects in CompositionHelper.GetLinkedObjectsArray(userSession, objectIDs))
        objects.Add(new Tuple<long, bool>(linkedObjects, false));
    }
    for (int index = 0; index < objects.Count; ++index)
    {
      IDBObject dbObject = userSession.GetObject(objects[index].Item1);
      if (autoUpdate)
        ActionsHelper.AddSiteCode(siteInfo, dbObject);
      IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(PortalConsts.attributeOwner);
      if (attributeByGuid.AsString != siteInfo.Code.ToString())
      {
        if (!skipNotOwned)
          throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_64"), (object) siteInfo.Caption, (object) dbObject.NameInMessages));
      }
      else
      {
        this.SetOwnerAttributes(dbObject, attributeByGuid, ownerSites);
        stringList.Add(dbObject.GetAttributeByGuid(PortalConsts.attributePublishObjectGUID).AsString);
      }
    }
    if (TraceLog.Enabled)
      TraceLog.Write($"End OwnComplete site={siteInfo.Code}");
    return stringList.ToArray();
  }

  private void SetOwnerAttributes(IDBObject obj, IDBAttribute attrOwner, string ownerSites)
  {
    attrOwner.AsString = string.Empty;
    obj.GetAttributeByGuid(PortalConsts.attributeParentSites).AsString = ownerSites;
    obj.GetAttributeByGuid(PortalConsts.attributeCompositionOwner).AsString = string.Empty;
    obj.GetAttributeByGuid(PortalConsts.attributeCompositionParentSites).AsString = ownerSites;
    string asString = obj.GetAttributeByGuid(PortalConsts.attributeEnabledSites).AsString;
    foreach (char ownerSite in ownerSites)
    {
      if (asString.IndexOf(ownerSite) < 0)
        asString += ownerSite.ToString();
    }
    obj.GetAttributeByGuid(PortalConsts.attributeEnabledSites).AsString = asString;
  }
}
