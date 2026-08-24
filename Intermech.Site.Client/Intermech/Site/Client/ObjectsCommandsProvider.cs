// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ObjectsCommandsProvider
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using ImSSP;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class ObjectsCommandsProvider : ICommandsProvider
{
  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo mergedCommands = new CommandsInfo();
    IMServerService service1 = ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService;
    ISitesCacheService customService1 = (ISitesCacheService) service1.GetCustomService(typeof (ISitesCacheService));
    IPublishTypesConfiguration customService2 = service1.GetCustomService(typeof (IPublishTypesConfiguration)) as IPublishTypesConfiguration;
    if (items.Count > 0)
    {
      int code = (int) customService1.Info.Code;
      bool flag = true;
      for (int index = 0; index < items.Count; ++index)
      {
        if (items.GetItemData(index, typeof (IDBCheckedOutByID)) is IDBCheckedOutByID itemData1 && itemData1.CheckedOutBy != 0L)
        {
          flag = false;
          break;
        }
        if (items.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData2 && !customService2.IsPublishObjectType(itemData2.ObjectType))
        {
          flag = false;
          break;
        }
      }
      if (flag)
        mergedCommands.Add(SiteClientConsts.CommandToPublishName, new CommandInfo(0, new ClickEventHandler(this.ToPublishCommand)));
      if (viewServices.GetService(typeof (NavigatorTreeView)) is NavigatorTreeView service2 && service2.RootDescriptor is AutoPublishListDescriptor)
        mergedCommands.Add(SiteClientConsts.CommandEndAutoPublish, new CommandInfo(0, new ClickEventHandler(this.EndAutoPublish)));
    }
    return mergedCommands;
  }

  private void EndAutoPublish(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    List<long> longList = new List<long>(items.Count);
    for (int index = 0; index < items.Count; ++index)
    {
      IDBObjectID itemData = items.GetItemData(index, typeof (IDBObjectID)) as IDBObjectID;
      longList.Add(itemData.Value);
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(PortalConsts.selectionAutoPublish);
      (sessionKeeper.Session.GetCustomService(typeof (ISelectionsService)) as ISelectionsService).ExcludeObjects((object) sessionKeeper.Session.SessionGUID, objectInfo.ObjectID, longList.ToArray());
      ((INotificationService) ServicesManager.GetService(typeof (INotificationService))).FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", objectInfo.ObjectID));
    }
  }

  private void ToPublishCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    Helper.CheckAccess(ActionType.Export);
    if (!Helper.Initialized)
      throw new Exception(string.Format(LocalizationHolder.rm.GetString(sc_18513.ssp_webportal_18514()), (object) SiteClientConsts.ErrorInitializeHelper));
    int num = (int) UnitedPublishForm.ShowForm(items);
  }

  private int GetOwnParent(List<int> childTypes, List<int> allParentTypes)
  {
    List<int> childTypes1 = new List<int>(childTypes.Count);
    for (int index = 0; index < childTypes.Count; ++index)
    {
      int objectTypeParentId = MetaDataHelper.GetObjectTypeParentID(childTypes[index]);
      if (!allParentTypes.Contains(objectTypeParentId) && !childTypes1.Contains(objectTypeParentId))
      {
        allParentTypes.Add(objectTypeParentId);
        childTypes1.Add(objectTypeParentId);
      }
    }
    if (childTypes1.Count == 0)
      throw new Exception();
    return childTypes1.Count == 1 ? childTypes1[0] : this.GetOwnParent(childTypes1, allParentTypes);
  }
}
