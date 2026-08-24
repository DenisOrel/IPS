// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.SiteViewProvider
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class SiteViewProvider : IViewsProvider
{
  private static bool _registeredView;

  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (!SiteViewProvider._registeredView)
    {
      AdjustableViewsHelper.RegisterView("SiteUsersView", LocalizationHolder.rm.GetString("Site.Client_9"), "", "Intermech Portal", "", true, 0);
      SiteViewProvider._registeredView = true;
    }
    ViewsInfo views = new ViewsInfo();
    views.Add("SiteUsersView", new ViewInfo(1, typeof (SiteUsersView)));
    return views;
  }
}
