// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PublishObjectsCommandsProvider
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.DataFormats;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Site.Client.PortalNavigator;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class PublishObjectsCommandsProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (!Helper.Initialized || items.Count <= 0)
      return CommandsInfo.Empty;
    CommandsInfo groupCommands = new CommandsInfo();
    bool flag1 = true;
    bool flag2 = true;
    bool flag3 = true;
    bool flag4 = true;
    for (int index = 0; index < items.Count; ++index)
    {
      if (!(items.GetItemData(index, typeof (IPublishObjectID)) is IPublishObjectID itemData))
      {
        flag3 = false;
        flag1 = false;
        flag2 = false;
        flag4 = false;
        break;
      }
      if (itemData.OwnerID != 0L && itemData.OwnerID != Helper.SiteID)
      {
        flag2 = false;
        flag3 = false;
      }
      if (string.IsNullOrEmpty(itemData.CopyKeepers))
        flag4 = false;
    }
    if (flag1)
      groupCommands.Add(SiteClientConsts.CommandImport, new CommandInfo(0, new ClickEventHandler(PublishObjectsCommandsProvider.ImportCommand)));
    if (flag2)
      groupCommands.Add(SiteClientConsts.CommandDelete, new CommandInfo(0, new ClickEventHandler(PublishObjectsCommandsProvider.DeleteCommand)));
    if (flag3)
      groupCommands.Add(SiteClientConsts.CommandOwnComplete, new CommandInfo(0, new ClickEventHandler(PublishObjectsCommandsProvider.OwnCompleteCommand)));
    if (flag4)
      groupCommands.Add(SiteClientConsts.CommandAutoImportComplete, new CommandInfo(0, new ClickEventHandler(this.AutoImportComplete)));
    groupCommands.Add("OpenInNewWindow", new CommandInfo(4, new ClickEventHandler(PublishObjectsCommandsProvider.OpenInNewWindow)));
    return groupCommands;
  }

  private void AutoImportComplete(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    List<long> objectIDs = new List<long>();
    for (int index = 0; index < items.Count; ++index)
    {
      IPublishObjectID itemData = items.GetItemData(index, typeof (IPublishObjectID)) as IPublishObjectID;
      objectIDs.Add(itemData.ObjectID);
    }
    DialogResult dialogResult = MessageBox.Show("Завершить автоимпорт и для объектов состава?", SiteClientConsts.CommandAutoImportCompleteCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
    if (dialogResult == DialogResult.Cancel)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      int num = (int) MessageBox.Show($"Автоимпорт завершен для {((IPortalConnector) sessionKeeper.Session.GetCustomService(typeof (IPortalConnector))).AutoImportComplete(sessionKeeper.Session.SessionGUID, objectIDs.ToArray(), dialogResult == DialogResult.Yes).Length} объектов.", SiteClientConsts.CommandAutoImportCompleteCaption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      if (!(items.GetParentData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID parentData))
        return;
      INotificationService service = (INotificationService) ServicesManager.GetService(typeof (INotificationService));
      if (parentData.ObjectType == MetaDataHelper.GetObjectTypeID(PortalConsts.objtypePortalSelections))
        service.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", parentData.ObjectID));
      else
        service.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", (IList<long>) objectIDs));
    }
  }

  private static void OwnCompleteCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    Helper.CheckAccess(ActionType.Export);
    if (!Helper.Initialized)
      throw new Exception(SiteClientConsts.ErrorInitializeHelper);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      List<IPublishObjectID> objs = new List<IPublishObjectID>(items.Count);
      for (int index = 0; index < items.Count; ++index)
      {
        if (items.GetItemData(index, typeof (IPublishObjectID)) is IPublishObjectID itemData)
          objs.Add(itemData);
      }
      using (OwnCompleteForm ownCompleteForm = new OwnCompleteForm())
      {
        ownCompleteForm.Init(sessionKeeper.Session, objs);
        if (ownCompleteForm.ShowDialog() != DialogResult.OK)
          return;
        ((IPortalTasksQueue) sessionKeeper.Session.GetCustomService(typeof (IPortalTasksQueue))).OwnComplete(sessionKeeper.Session.SessionGUID, ownCompleteForm.Objects, ownCompleteForm.ObjectGuids, ownCompleteForm.ParentSites, ownCompleteForm.CompositionType == SelectCompositionType.RecursiveComposition, ownCompleteForm.AutoUpdate);
        ((INotificationService) viewServices.GetService(typeof (INotificationService))).FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("OwnComplete", (IList<long>) ownCompleteForm.Objects));
      }
    }
  }

  private static void OpenInNewWindow(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (!Helper.Initialized)
      throw new Exception(SiteClientConsts.ErrorInitializeHelper);
    if (!(items.GetItemData(0, typeof (IPublishObjectID)) is IPublishObjectID itemData))
      throw new ArgumentException();
    DockManager service = (DockManager) ServicesManager.GetService(typeof (DockManager));
    PortalNavWindow portalNavWindow = new PortalNavWindow();
    portalNavWindow.TreeView.SetColumns(Intermech.Site.Client.PortalNavigator.Helper.GetPublicObjectCaptionOnlyColumns());
    portalNavWindow.TreeView.Build((IDescriptor) new PublishedObjectDescriptor(itemData.ObjectID, itemData.ObjectGuid, itemData.TypeID, itemData.CopyKeepers, itemData.OwnerID, itemData.Caption));
    portalNavWindow.Show(service);
    portalNavWindow.Activate();
  }

  private static void DeleteCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    Helper.CheckAccess(ActionType.Export);
    List<long> longList = new List<long>();
    string str = (string) null;
    for (int index = 0; index < items.Count; ++index)
    {
      IPublishObjectID itemData = items.GetItemData(index, typeof (IPublishObjectID)) as IPublishObjectID;
      longList.Add(itemData.ObjectID);
      if (items.Count == 1)
        str = itemData.Caption;
    }
    if (longList.Count <= 0)
      return;
    if (longList.Count > 1)
      str = $"{items.Count} оъекта(ов)";
    if (MessageBox.Show($"Вы действительно хотите удалить {str}?", "Удаление опубликованных объектов", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IPortalConnector customService = (IPortalConnector) sessionKeeper.Session.GetCustomService(typeof (IPortalConnector));
      if (customService == null)
        return;
      long[] objectIDs = customService.DeleteObjects(sessionKeeper.Session.SessionGUID, longList.ToArray());
      if (objectIDs == null)
        return;
      ((INotificationService) viewServices.GetService(typeof (INotificationService))).FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("PublishObjectsRemoved", (IList<long>) objectIDs));
    }
  }

  private static void ImportCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    Helper.CheckAccess(ActionType.Import);
    using (ImportForm importForm = new ImportForm())
    {
      importForm.Initialize(items, viewServices);
      if (importForm.ShowDialog() != DialogResult.OK)
        return;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IPortalConnector customService = (IPortalConnector) sessionKeeper.Session.GetCustomService(typeof (IPortalConnector));
        ImportOptions options = importForm.Options as ImportOptions;
        int[] numArray = (int[]) null;
        if (options.FilteredTypes != null && options.FilteredTypes.Count > 0)
          numArray = options.FilteredTypes.ToArray();
        Guid sessionGuid = sessionKeeper.Session.SessionGUID;
        long[] array = importForm.ImportedObjectIDs.ToArray();
        int[] filteredTypes = numArray;
        int num1 = options.SetOwner ? 1 : 0;
        int num2 = options.AutoUpdate ? 1 : 0;
        int compositionType = (int) options.CompositionType;
        int num3 = options.StartImmediately ? 1 : 0;
        customService.ImportObjects(sessionGuid, TaskPriority.Normal, array, filteredTypes, num1 != 0, num2 != 0, (SelectCompositionType) compositionType, num3 != 0);
      }
    }
  }
}
