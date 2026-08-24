// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PacketCommandsProvider
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Site.Client.PortalNavigator;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class PacketCommandsProvider : ICommandsProvider
{
  private static void ImportCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    Helper.CheckAccess(ActionType.Import);
    using (ImportPacketForm importPacketForm = new ImportPacketForm())
    {
      importPacketForm.Initialize(items, viewServices);
      if (importPacketForm.ShowDialog() != DialogResult.OK)
        return;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IPortalConnector customService = (IPortalConnector) sessionKeeper.Session.GetCustomService(typeof (IPortalConnector));
        ImportPacketOptions options = importPacketForm.Options as ImportPacketOptions;
        Guid sessionGuid = sessionKeeper.Session.SessionGUID;
        long[] array = importPacketForm.ImportedObjectIDs.ToArray();
        int importVersionsMode = (int) options.ImportVersionsMode;
        int num = options.StartImmediately ? 1 : 0;
        customService.ImportPackets(sessionGuid, TaskPriority.Normal, array, (ImportVersionsModes) importVersionsMode, num != 0);
      }
    }
  }

  private static void DeleteCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
  }

  private static void OpenInNewWindow(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (!Helper.Initialized)
      throw new Exception(SiteClientConsts.ErrorInitializeHelper);
    if (!(items.GetItemData(0, typeof (IPacketNodeID)) is IPacketNodeID itemData))
      throw new ArgumentException();
    DockManager service = (DockManager) ServicesManager.GetService(typeof (DockManager));
    PortalNavWindow portalNavWindow = new PortalNavWindow();
    portalNavWindow.TreeView.SetColumns(Intermech.Site.Client.PortalNavigator.Helper.GetPublicObjectCaptionOnlyColumns());
    portalNavWindow.TreeView.Build((IDescriptor) new PacketDescriptor(itemData.TypeID, itemData.ObjectID, itemData.CreatorID, itemData.Caption, itemData.CreateDate));
    portalNavWindow.Show(service);
    portalNavWindow.Activate();
  }

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
    char code = ((ISitesCacheService) (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (ISitesCacheService))).Info.Code;
    for (int index = 0; index < items.Count; ++index)
    {
      if (!(items.GetItemData(index, typeof (IPacketNodeID)) is IPacketNodeID itemData))
      {
        flag1 = false;
        flag2 = false;
        break;
      }
      if ((int) itemData.CreatorID != (int) code)
        flag2 = false;
    }
    if (flag1)
      groupCommands.Add(SiteClientConsts.CommandImport, new CommandInfo(0, new ClickEventHandler(PacketCommandsProvider.ImportCommand)));
    if (flag2)
      groupCommands.Add(SiteClientConsts.CommandDelete, new CommandInfo(0, new ClickEventHandler(PacketCommandsProvider.DeleteCommand)));
    groupCommands.Add("OpenInNewWindow", new CommandInfo(4, new ClickEventHandler(PacketCommandsProvider.OpenInNewWindow)));
    return groupCommands;
  }
}
