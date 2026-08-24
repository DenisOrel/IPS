// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishedObjectViewProvider
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PublishedObjectViewProvider : IViewsProvider
{
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (items.Count != 1)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    INodeID itemId = items.GetItemID(0);
    if (itemId is IPublishObjectID)
    {
      views.Add("Intermech.Site.Client.AttributesView", new ViewInfo(3, 1737, typeof (ObjectAttributesView)));
      views.Add("Intermech.Site.Client.CompositionView", new ViewInfo(3, 1737, typeof (CompositionView)));
    }
    if (itemId is IPublishRelationID)
      views.Add("Intermech.Site.Client.RelationAttributesView", new ViewInfo(3, 1737, typeof (RelationAttributesView)));
    return views;
  }
}
