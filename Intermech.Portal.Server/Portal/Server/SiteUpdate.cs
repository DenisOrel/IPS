// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.SiteUpdate
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Portal.Server;

internal class SiteUpdate
{
  private List<TransferedObject> _data;
  private readonly TaskStatus _status;
  private readonly long[] _siteIDs;
  private readonly string _authorID;

  public SiteUpdate(List<TransferedObject> data, long[] siteIDs, string authorID)
  {
    this._status = TaskStatus.Waiting;
    this._data = data;
    this._siteIDs = siteIDs;
    this._authorID = authorID;
  }

  public long SaveIntoBase(IUserSession session, Guid updateGuid)
  {
    IDBObject dbObject = session.GetObjectCollection(PortalConsts.objtypeChanges).Create(updateGuid);
    dbObject.Attributes.AddAttribute(session.GetAttributeType(PortalServerConsts.attributeSiteId, true).AttributeID, false, this._siteIDs.Cast<object>().ToArray<object>());
    if (this._authorID != null && this._authorID != string.Empty)
      dbObject.Attributes.AddAttribute(session.GetAttributeType(PortalConsts.attributeFirstPublishSite, true).AttributeID, false, new object[1]
      {
        (object) this._authorID
      });
    dbObject.Attributes.AddAttribute(session.GetAttributeType(PortalConsts.attributeTaskStatus, true).AttributeID, false, new object[1]
    {
      (object) (int) this._status
    });
    IDBAttribute attrUnits = dbObject.Attributes.AddAttribute(session.GetAttributeType(PortalServerConsts.attributeUpdateData, true).AttributeID, false);
    UpdateDataAttributeHelper.Save(session, attrUnits, this._data);
    dbObject.CommitCreation(true);
    return dbObject.ObjectID;
  }

  public static void Delete(
    IUserSession session,
    string fileStorage,
    SiteInfo info,
    Guid updateGuid)
  {
    IDBObject dbObject = session.GetObject(updateGuid, false);
    if (dbObject == null)
      return;
    IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(PortalServerConsts.attributeSiteId);
    List<object> objectList = new List<object>(attributeByGuid.ValuesCount);
    for (int index = 0; index < attributeByGuid.ValuesCount; ++index)
    {
      attributeByGuid.Index = index;
      if (attributeByGuid.AsInteger != info.ID)
        objectList.Add((object) attributeByGuid.AsInteger);
    }
    if (objectList.Count > 0)
    {
      attributeByGuid.Values = objectList.ToArray();
      dbObject.GetAttributeByGuid(PortalConsts.attributeTaskStatus).AsInteger = 4L;
    }
    else
      dbObject.Delete(0L);
  }
}
