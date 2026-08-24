// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareObjectsRootViewProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class CompareObjectsRootViewProvider : IViewsProvider
{
  private static bool _registeredView;

  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (!CompareObjectsRootViewProvider._registeredView)
    {
      AdjustableViewsHelper.RegisterView("ChildrenView", PDMPluginConsts.ListCompareObjects, "", "Intermech.PDM", "imgCompCompare", true, 0);
      CompareObjectsRootViewProvider._registeredView = true;
    }
    if (items.Count != 1)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    views.Add("ChildrenView", new ViewInfo(0, 691, typeof (ListCompareObjectsView)));
    return views;
  }
}
