// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.TasksCommandsProvider
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using ImSSP;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class TasksCommandsProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items.Count > 1)
      return CommandsInfo.Empty;
    IDBTaskID itemData = (IDBTaskID) items.GetItemData(0, typeof (IDBTaskID));
    if (itemData == null)
      return CommandsInfo.Empty;
    CommandsInfo groupCommands = new CommandsInfo();
    if (itemData.Enabled)
    {
      switch (itemData.Status)
      {
        case TaskStatus.Aborted:
        case TaskStatus.Erroneous:
        case TaskStatus.Waiting:
          groupCommands.Add(SiteClientConsts.CommandStartTask, new CommandInfo(0, new ClickEventHandler(this.StartTask), (object) itemData));
          break;
      }
    }
    groupCommands.Add(SiteClientConsts.CommandTaskIncludes, new CommandInfo(0, new ClickEventHandler(this.TaskIncludes), (object) itemData));
    return groupCommands;
  }

  private void TaskIncludes(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (IncludesForm includesForm = new IncludesForm((sessionKeeper.Session.GetObject(itemData.ObjectID) as IDBTask).GetIncludesInfo(sessionKeeper.Session.SessionGUID)))
      {
        int num = (int) includesForm.ShowDialog();
      }
    }
  }

  private void StartTask(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    IDBTaskID dbTaskId = (IDBTaskID) additionalInfo;
    IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IPortalTasksQueue customService = (IPortalTasksQueue) sessionKeeper.Session.GetCustomService(typeof (IPortalTasksQueue));
      if (itemData == null || customService == null)
        return;
      if (dbTaskId.Type == TaskType.Publish)
        Helper.CheckAccess(ActionType.Export);
      else if (dbTaskId.Type == TaskType.ImportUpdates || dbTaskId.Type == TaskType.ImportObjects)
        Helper.CheckAccess(ActionType.Import);
      if (dbTaskId.Type == TaskType.ImportObjects)
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(itemData.ObjectID);
        IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(PortalConsts.attributeUpdateGuid);
        if (attributeByGuid == null || !GuidHelper.IsGuid(attributeByGuid.AsString))
          throw new Exception(string.Format(LocalizationHolder.rm.GetString(sc_18518.ssp_webportal_18519()), (object) dbObject.NameInMessages));
        customService.StartUpdate(sessionKeeper.Session.SessionGUID, attributeByGuid.AsString, (object) null);
      }
      else
        customService.StartTask(itemData.ObjectID);
    }
  }
}
