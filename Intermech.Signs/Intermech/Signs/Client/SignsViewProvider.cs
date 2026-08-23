// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignsViewProvider
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Signs.Interfaces;
using System;

#nullable disable
namespace Intermech.Signs.Client;

internal class SignsViewProvider : IViewsProvider
{
  /// <summary>Зарегистрирована ли закладка</summary>
  private static bool _registeredView;

  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (!SignsViewProvider._registeredView)
    {
      AdjustableViewsHelper.RegisterView("SignsView", LocalizationHolder.rm.GetString("Signs_54"), "", LocalizationHolder.rm.GetString("Signs_54"), "imgSign", true, 0);
      SignsViewProvider._registeredView = true;
    }
    if (items.Count != 1 || !(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || !MetaDataHelper.HasApplicability(itemData.ObjectType, SignsHolder.SignObjectTypeID, SignsHolder.SignRelationTypeID))
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    views.Add("SignsView", new ViewInfo(4, 1597, typeof (SignsView)));
    return views;
  }
}
