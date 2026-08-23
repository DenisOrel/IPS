// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.GraphsProvider
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Вьюшека настройки "Граф для подписей"</summary>
public class GraphsProvider : IViewsProvider
{
  /// <summary>Зарегистрирована ли закладка</summary>
  private static bool _registeredView;

  /// <summary>Получении информации о вьюшке</summary>
  /// <param name="items">Выбранные объекты</param>
  /// <param name="services">сервисы</param>
  /// <returns>Информация о вьшках</returns>
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (!GraphsProvider._registeredView)
    {
      AdjustableViewsHelper.RegisterView("Graphs", LocalizationHolder.rm.GetString("Signs_45"), "", LocalizationHolder.rm.GetString("Signs_54"), "", true, 0);
      GraphsProvider._registeredView = true;
    }
    if (items.Count != 1)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    views.Add("Graphs", new ViewInfo(0, 1267, typeof (Graphs)));
    return views;
  }
}
