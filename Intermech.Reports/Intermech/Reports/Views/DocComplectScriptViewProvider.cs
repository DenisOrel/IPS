// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Views.DocComplectScriptViewProvider
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.DataFormats;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Reports.Views;

/// <summary>Провайдер для закладки списка форм редактирования</summary>
public class DocComplectScriptViewProvider : IViewsProvider
{
  /// <summary>Конструктор</summary>
  static DocComplectScriptViewProvider()
  {
    AdjustableViewsHelper.RegisterView("ComplDocFormListView", LocalizationHolder.rm.GetString("Reports_52"), LocalizationHolder.rm.GetString("Reports_52"), "Intermech.Reports", "imgProp", true, 10);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="provider"></param>
  /// <returns></returns>
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider provider)
  {
    if (items == null || items.Count != 1 || !(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID))
      return ViewsInfo.Empty;
    IViewState service = provider != null ? provider.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    int num = service == null ? 0 : ((service.ViewState & ViewStateFlags.InParametersCard) > ViewStateFlags.None ? 1 : 0);
    ViewsInfo views = new ViewsInfo();
    if (num == 0)
      views.Add("ComplDocFormListView", new ViewInfo(3, 697, typeof (DocComplectScriptFormsView)));
    return views;
  }
}
