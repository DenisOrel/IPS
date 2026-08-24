// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.ItemView.GtcViewProvider
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.GTC.Client.ItemView;

internal class GtcViewProvider : IViewsProvider
{
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (items.Count != 1)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    views.Add(Const.GtcViewName, new ViewInfo(0, typeof (GtcProperitesView)));
    return views;
  }
}
