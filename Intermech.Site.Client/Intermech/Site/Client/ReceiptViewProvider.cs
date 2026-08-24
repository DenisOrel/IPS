// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ReceiptViewProvider
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Site.Client;

internal class ReceiptViewProvider : IViewsProvider
{
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (items.Count != 1)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    views.Suppress("ObjectVisualizer", 3);
    views.Add("Intermech.Site.Client.ReceiptContentView", new ViewInfo(3, typeof (ReceiptContentView)));
    return views;
  }
}
