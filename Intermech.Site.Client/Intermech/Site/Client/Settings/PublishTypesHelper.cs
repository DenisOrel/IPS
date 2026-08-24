// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.PublishTypesHelper
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client.Settings;

internal static class PublishTypesHelper
{
  private static List<PublishTypeAttProxy> _publishTypesCache;

  private static Guid GetPublishTypeGuid(
    IUserSession session,
    IContainerService containerService,
    IDBObjectType objType)
  {
    if (((IPortalConnector) session.GetCustomService(typeof (IPortalConnector))).IsOffline)
      return PortalConsts.objtypePublishObjects;
    Guid publishTypeGuid = Guid.Empty;
    IDBObject containerForObjectType = containerService.GetContainerForObjectType((object) session.SessionGUID, (objType as IDBGuid).GUID);
    if (containerForObjectType != null)
    {
      IDBAttribute attributeByGuid = containerForObjectType.GetAttributeByGuid(PortalConsts.attributePublishObjTypeGuid);
      if (attributeByGuid != null && GuidHelper.IsGuid(attributeByGuid.AsString))
        publishTypeGuid = new Guid(attributeByGuid.AsString);
    }
    if (publishTypeGuid == Guid.Empty && objType.ParentTypeID != -1)
      publishTypeGuid = PublishTypesHelper.GetPublishTypeGuid(session, containerService, session.GetObjectType(objType.ParentTypeID));
    return publishTypeGuid;
  }

  public static PublishTypeAttProxy GetPublishType(IUserSession session, IDBObjectType objType)
  {
    if (((IPortalConnector) session.GetCustomService(typeof (IPortalConnector))).IsOffline)
      return (PublishTypeAttProxy) null;
    IContainerService customService = session.GetCustomService(typeof (IContainerService)) as IContainerService;
    Guid publishObjectType = PublishTypesHelper.GetPublishTypeGuid(session, customService, objType);
    if (publishObjectType == Guid.Empty)
      publishObjectType = PortalConsts.objtypePublishObjects;
    return PublishTypesHelper.GetAttProxy(publishObjectType) ?? PublishTypesHelper.GetAttProxy(publishObjectType);
  }

  public static void SetPublishType(
    IUserSession session,
    Guid objectTypeGuid,
    ObjectType4PublicationProrerties properties)
  {
    if (!(properties is ObjectType4PublicationProrertiesEx publicationProrertiesEx))
      return;
    IContainerService customService = session.GetCustomService(typeof (IContainerService)) as IContainerService;
    IDBObject containerForObjectType = customService.GetContainerForObjectType((object) session.SessionGUID, objectTypeGuid);
    bool flag = true;
    if (containerForObjectType == null)
    {
      IDBObjectType objectType = session.GetObjectType(objectTypeGuid);
      if (objectType.ParentTypeID != -1)
      {
        Guid publishTypeGuid = PublishTypesHelper.GetPublishTypeGuid(session, customService, session.GetObjectType(objectType.ParentTypeID));
        flag = publicationProrertiesEx.PublishType.Guid != publishTypeGuid;
      }
    }
    if (!flag)
      return;
    if (containerForObjectType == null)
      containerForObjectType = customService.GetContainerForObjectType((object) session.SessionGUID, objectTypeGuid, true);
    IDBAttribute dbAttribute = containerForObjectType.GetAttributeByGuid(PortalConsts.attributePublishObjTypeGuid);
    if (publicationProrertiesEx.PublishType.Guid == Guid.Empty)
    {
      dbAttribute?.Delete(0L);
    }
    else
    {
      if (dbAttribute == null)
        dbAttribute = containerForObjectType.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributePublishObjTypeGuid), false);
      dbAttribute.Value = (object) publicationProrertiesEx.PublishType.Guid;
    }
  }

  private static PublishTypeAttProxy GetAttProxy(Guid publishObjectType)
  {
    if (PublishTypesHelper._publishTypesCache == null)
      PublishTypesHelper._publishTypesCache = new List<PublishTypeAttProxy>();
    PublishTypeAttProxy attProxy = PublishTypesHelper._publishTypesCache.Find((Predicate<PublishTypeAttProxy>) (x => x.Guid.Equals(publishObjectType)));
    if (attProxy == null)
    {
      PortalObjectType publishObjectType1 = ((IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata))).GetPublishObjectType(publishObjectType);
      if (publishObjectType1 == null)
        return (PublishTypeAttProxy) null;
      attProxy = new PublishTypeAttProxy(publishObjectType1.ID, publishObjectType, publishObjectType1.Name);
      PublishTypesHelper._publishTypesCache.Add(attProxy);
    }
    return attProxy;
  }

  public static void ClearCache()
  {
    if (PublishTypesHelper._publishTypesCache == null)
      return;
    PublishTypesHelper._publishTypesCache = (List<PublishTypeAttProxy>) null;
  }
}
