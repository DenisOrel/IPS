// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareObjectViewProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class CompareObjectViewProvider : IViewsProvider
{
  private static bool _registeredView;

  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (!CompareObjectViewProvider._registeredView)
    {
      AdjustableViewsHelper.RegisterView("PDM.CompareCompositionView", PDMPluginConsts.CompareObjectComposition, "", "Intermech.PDM", "imgCompCompare", true, 0);
      CompareObjectViewProvider._registeredView = true;
    }
    if (items == null || items.Count != 1 || items.GetItemData(0, typeof (IPDMCompareObject)) == null)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    views.Add("PDM.CompareCompositionView", new ViewInfo(0, 0, typeof (CompareCompositionView)));
    return views;
  }
}
