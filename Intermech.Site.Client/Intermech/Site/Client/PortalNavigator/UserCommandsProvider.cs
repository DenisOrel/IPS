// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.UserCommandsProvider
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class UserCommandsProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo mergedCommands = new CommandsInfo();
    mergedCommands.Add(SiteClientConsts.CommandImport, new CommandInfo(0, new ClickEventHandler(this.ImportCommand)));
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  private void ImportCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    Intermech.Site.Client.Helper.CheckAccess(ActionType.Import);
    List<long> longList = new List<long>(items.Count);
    string str = string.Empty;
    Guid guid = Guid.Empty;
    for (int index = 0; index < items.Count; ++index)
    {
      if (items.GetItemData(index, typeof (IUserNodeID)) is IUserNodeID itemData)
      {
        longList.Add(itemData.UserID);
        if (longList.Count == 1)
        {
          str = itemData.UserName;
          guid = itemData.SiteGuid;
        }
      }
    }
    if (longList.Count <= 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ISitesCacheService customService = (ISitesCacheService) sessionKeeper.Session.GetCustomService(typeof (ISitesCacheService));
      if (guid.Equals(customService.Info.GUID))
      {
        int num1 = (int) MessageBox.Show(LocalizationHolder.rm.GetString("Site.Client_43"), LocalizationHolder.rm.GetString("Site.Client_44"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        ((IPortalConnector) sessionKeeper.Session.GetCustomService(typeof (IPortalConnector)))?.ImportUsers(sessionKeeper.Session.SessionGUID, longList.ToArray());
        int num2 = (int) MessageBox.Show(string.Format(LocalizationHolder.rm.GetString("Site.Client_45"), longList.Count > 1 ? (object) string.Format(LocalizationHolder.rm.GetString("Site.Client_46"), (object) str) : (object) string.Format(LocalizationHolder.rm.GetString("Site.Client_47"), (object) str)), LocalizationHolder.rm.GetString("Site.Client_44"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }
  }
}
