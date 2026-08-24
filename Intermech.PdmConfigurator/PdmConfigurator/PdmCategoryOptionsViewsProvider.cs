// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.PdmCategoryOptionsViewsProvider
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.PdmConfigurator;

public class PdmCategoryOptionsViewsProvider : IViewsProvider
{
  private static volatile bool _registeredView;

  public PdmCategoryOptionsViewsProvider()
  {
    if (PdmCategoryOptionsViewsProvider._registeredView)
      return;
    AdjustableViewsHelper.RegisterView("PdmCategoryOptionsView", PdmCategoryObjectNode.NodeName, LocalizationHolder.rm.GetString("PdmConfigurator_20"), "Intermech.Pdm", "imgPdmConfigurator.Options", true, 0);
    PdmCategoryOptionsViewsProvider._registeredView = true;
  }

  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (items.Count != 1)
      return ViewsInfo.Empty;
    IViewState service = services != null ? services.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    if (((service != null ? (long) service.ViewState : 2L) & 256L /*0x0100*/) == 0L)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    views.Add("PdmCategoryOptionsView", new ViewInfo(0, 1823, typeof (PdmCategoryOptionsView)));
    return views;
  }
}
