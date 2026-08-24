// Decompiled with JetBrains decompiler
// Type: Intermech.Sales.KeyComposition4RequestViewProvider
// Assembly: Intermech.Sales, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D9A9043-6548-439B-99F7-AF22F44A5D2B
// Assembly location: D:\IPS\Client\Intermech.Sales.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Sales;

internal class KeyComposition4RequestViewProvider : IViewsProvider
{
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    ViewsInfo views = new ViewsInfo();
    views.Add("Sales.KeyComposition4Request", new ViewInfo(0, 0, typeof (KeyComposition4RequestView)));
    return views;
  }
}
