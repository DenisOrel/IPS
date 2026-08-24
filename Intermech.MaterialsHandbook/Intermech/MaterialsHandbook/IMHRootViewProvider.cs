// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHRootViewProvider
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHRootViewProvider : IViewsProvider
{
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider provider)
  {
    ViewsInfo views = new ViewsInfo();
    if (items.Count == 1 && items.GetItemData(0, typeof (IIMHNode)) is IIMHNode itemData)
    {
      views.Suppress("ChildrenView", 3);
      views.Suppress("Thumbnails", 7);
      views.Suppress("PDM.ContainsView", 3);
      views.Suppress("PDM.ApplicabilityView", 3);
      views.Suppress("ContextsSearchView", 3);
      views.Suppress("ObjectVisualizer", 3);
      views.Suppress("ObjectFiles", 3);
      if (itemData.CategoryID != Intermech.Imbase.Consts.ImbaseFolderTypeID)
        views.Add("MaterialsChildrenView", new ViewInfo(1, 876, typeof (MaterialsChildrenView)));
      else if (provider.GetService(typeof (NavigatorTreeView)) is NavigatorTreeView service)
      {
        service.PopulateNode(service.FocusedNode);
        if (itemData.ParentCategoryID == Consts.IMHProfilesNodeCategoryID)
        {
          views.Add("IMHView", new ViewInfo(0, typeof (IMHView)));
          if (service.FocusedNode.Children.Count > 0)
            views.Add("MaterialsChildrenView", new ViewInfo(1, 876, typeof (MaterialsChildrenView)));
        }
        else if (service.FocusedNode.Children.Count == 0)
          views.Add("IMHView", new ViewInfo(0, typeof (IMHView)));
        else
          views.Add("MaterialsChildrenView", new ViewInfo(1, 876, typeof (MaterialsChildrenView)));
      }
    }
    return views;
  }
}
