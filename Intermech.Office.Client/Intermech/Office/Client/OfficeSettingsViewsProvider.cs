// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeSettingsViewsProvider
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Office.Interfaces;
using System;

#nullable disable
namespace Intermech.Office.Client;

internal class OfficeSettingsViewsProvider : IViewsProvider
{
  public ViewsInfo GetViews([CanBeNull] ISelectedItems items, IServiceProvider services)
  {
    if (items == null || items.Count != 1)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetCustomService<IOfficeGeneralSettingsService>().Settings.PrivateOffice)
        views.Add("Office.OfficeSettingsView", new ViewInfo(0, typeof (OfficeSettingsView)));
    }
    return views;
  }
}
