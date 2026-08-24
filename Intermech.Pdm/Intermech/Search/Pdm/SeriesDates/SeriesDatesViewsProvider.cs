// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.SeriesDates.SeriesDatesViewsProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Search.Pdm.SeriesDates;

public sealed class SeriesDatesViewsProvider : IViewsProvider
{
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    ViewsInfo views = new ViewsInfo();
    if (SeriesDatesView.CheckViewParams(items, services))
      views.Add("VersionsApplicabilitiesView", new ViewInfo(-1, 2769, typeof (SeriesDatesView)));
    return views;
  }
}
