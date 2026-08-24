// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishSelectionViewProvider
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.DataFormats;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PublishSelectionViewProvider : IViewsProvider
{
  private static bool _registeredView;

  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (!PublishSelectionViewProvider._registeredView)
    {
      AdjustableViewsHelper.RegisterView("PortalSelectionView", LocalizationHolder.rm.GetString("Site.Client_35"), "", "Intermech Portal", "", true, 0);
      PublishSelectionViewProvider._registeredView = true;
    }
    if (items.Count != 1 || (IDBSelectionID) items.GetItemData(0, typeof (IDBSelectionID)) == null)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    views.Add("PortalSelectionView", new ViewInfo(0, 1736, typeof (PortalSelectionView)));
    views.Add("SelectionViewObject", new ViewInfo(0, 785, typeof (Intermech.Navigator.SelectionView.SelectionView)));
    views.Add("ChildrenView", new ViewInfo(1, typeof (PublishedObjectsView)));
    return views;
  }
}
