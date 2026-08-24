// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.InstanceViewProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class InstanceViewProvider : IViewsProvider
{
  private static bool _registeredView;

  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (!InstanceViewProvider._registeredView)
    {
      AdjustableViewsHelper.RegisterView("ChildrenView", PDMPluginConsts.ListInstancesWindow, "", "Intermech.Pdm", "imgObjects.PDM", true, 0);
      InstanceViewProvider._registeredView = true;
    }
    if (items.Count != 1)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    views.Add("ChildrenView", new ViewInfo(0, 691, typeof (InstanceObjectsView)));
    return views;
  }
}
