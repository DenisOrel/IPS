// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ContainsViewProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class ContainsViewProvider : IViewsProvider
{
  private static bool _registeredView;

  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (!ContainsViewProvider._registeredView)
    {
      AdjustableViewsHelper.RegisterView("PDM.ContainsView", LocalizationHolder.rm.GetString("Pdm_47"), "", "Intermech.Pdm", "imgContains", true, 0);
      AdjustableViewsHelper.RegisterView("PDM.ApplicabilityView", LocalizationHolder.rm.GetString("Pdm_46"), "", "Intermech.Pdm", "imgEntersTo", true, 0);
      ContainsViewProvider._registeredView = true;
    }
    ViewsInfo views = new ViewsInfo();
    if (items.Count == 1)
    {
      if (!(services.GetService(typeof (IViewState)) is IViewState service) || (service.ViewState & ViewStateFlags.NoContainsInView) == ViewStateFlags.NoContainsInView)
        return ViewsInfo.Empty;
      views.Add("PDM.ContainsView", new ViewInfo(0, 812, typeof (ContainsView)));
      views.Add("PDM.ApplicabilityView", new ViewInfo(0, 812, typeof (ApplicabilityView)));
    }
    return views;
  }
}
