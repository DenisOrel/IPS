// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.ArchiveSignsProvider
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

/// <summary>Закладка для подписей архивов</summary>
public class ArchiveSignsProvider : IViewsProvider
{
  /// <summary>Зарегистрирована ли закладка</summary>
  private static bool _registeredView;

  /// <summary>Получить доступные вьюшки</summary>
  /// <param name="items">выбранные объекты</param>
  /// <param name="services">дополнительные сервисы</param>
  /// <returns>Информация о вьюшках</returns>
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (!ArchiveSignsProvider._registeredView)
    {
      AdjustableViewsHelper.RegisterView("ArchiveSigns", LocalizationHolder.rm.GetString("Signs_37"), "", LocalizationHolder.rm.GetString("Signs_54"), "", true, 0);
      ArchiveSignsProvider._registeredView = true;
    }
    if (items.Count != 1 || !SignsHolder.isArchivesLoaded)
      return ViewsInfo.Empty;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBTypedObjectID itemData = (IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID));
      IDBObject dbObject = sessionKeeper.Session.GetObject(itemData.ObjectID, false);
      if (dbObject == null)
        return ViewsInfo.Empty;
      if (dbObject is IDBSecurity dbSecurity)
      {
        if (!dbSecurity.CheckAccess(ActionType.Edit, false, false))
          return ViewsInfo.Empty;
      }
    }
    ViewsInfo views = new ViewsInfo();
    views.Add("SignsCheck", new ViewInfo(0, 1269, typeof (ArchiveSigns)));
    return views;
  }
}
