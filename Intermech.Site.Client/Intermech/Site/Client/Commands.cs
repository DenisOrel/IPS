// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Commands
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Site.Client.PortalNavigator;
using System;

#nullable disable
namespace Intermech.Site.Client;

internal static class Commands
{
  public static readonly string AutoPublishList = "autoPublishList";
  public static readonly Guid AutoPublishListGuid = new Guid("855AE3FF-DCCA-426A-ADDB-2F7F8B41D8FA");
  private static readonly Guid _portalWindowGuid = new Guid("24645263-1AB6-4401-A440-2C84C41E7F2D");

  public static void ShowPortal(IServiceProvider serviceProvider)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      bool isAdmin = ((IPortalConnector) sessionKeeper.Session.GetCustomService(typeof (IPortalConnector))).IsAdmin(sessionKeeper.Session.SessionGUID);
      ISiteServerService customService = (ISiteServerService) sessionKeeper.Session.GetCustomService(typeof (ISiteServerService));
      if (!Helper.Initialized)
        throw new Exception(SiteClientConsts.ErrorInitializeHelper);
      WellKnownNavWindow wellKnownNavWindow1 = (WellKnownNavWindow) ((IWellKnownNavigators) ServicesManager.GetService(typeof (IWellKnownNavigators))).Get("portalWindow");
      if (wellKnownNavWindow1 == null)
      {
        WellKnownNavWindow wellKnownNavWindow2 = new WellKnownNavWindow();
        wellKnownNavWindow2.WellKnownName = "portalWindow";
        wellKnownNavWindow2.Guid = Commands._portalWindowGuid;
        wellKnownNavWindow2.Text = customService.Settings.Name;
        wellKnownNavWindow1 = wellKnownNavWindow2;
        wellKnownNavWindow1.TreeView.SetColumns(Utils.CaptionColumnOnly(NodeColumnSortOrder.Ascending));
        wellKnownNavWindow1.TreeView.Build((IDescriptor) new PortalDescriptor(customService.Settings.Name, isAdmin));
        ICategoryTypeIconService service = (ICategoryTypeIconService) serviceProvider.GetService(typeof (ICategoryTypeIconService));
        if (service != null)
        {
          int index = service.IndexOf(SiteClientConsts.CategoryPortal, 0);
          if (index >= 0)
            wellKnownNavWindow1.TabImage = service.ImageList.Images[index];
        }
      }
      wellKnownNavWindow1.Show((DockManager) serviceProvider.GetService(typeof (DockManager)));
      wellKnownNavWindow1.Activate();
    }
  }

  public static void ShowAutoPublishOblectsList(IServiceProvider serviceProvider)
  {
    WellKnownNavWindow wellKnownNavWindow = new WellKnownNavWindow();
    wellKnownNavWindow.WellKnownName = Commands.AutoPublishList;
    wellKnownNavWindow.Guid = Commands.AutoPublishListGuid;
    wellKnownNavWindow.Text = SiteClientConsts.AutoPublishOblectsListCaption;
    wellKnownNavWindow.TreeView.OnGetSupportedColumnsEventHandler += new GetSupportedColumnsEventHandler(Utils.GetNavigatorColumns);
    wellKnownNavWindow.TreeView.SetColumns(Utils.CaptionColumnOnly(NodeColumnSortOrder.Ascending));
    wellKnownNavWindow.TreeView.Build((IDescriptor) new AutoPublishListDescriptor());
    wellKnownNavWindow.Show((DockManager) serviceProvider.GetService(typeof (DockManager)));
    wellKnownNavWindow.Activate();
  }
}
