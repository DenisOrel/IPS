// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ArticlesInProductListProvider
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.MRP2;

internal class ArticlesInProductListProvider : IViewsProvider
{
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (items.Count != 1)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    views.Add("MRP2.ProductionCopyEntersInView", new ViewInfo(0, typeof (ProductionCopyEntersInView)));
    return views;
  }
}
