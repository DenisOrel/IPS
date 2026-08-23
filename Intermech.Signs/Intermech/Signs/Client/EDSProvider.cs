// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.EDSProvider
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Вьюшка для подписей</summary>
public class EDSProvider : IViewsProvider
{
  /// <summary>Вьюшка для подписей</summary>
  /// <param name="items">Выбранные объекты</param>
  /// <param name="services">сервисы</param>
  /// <returns>Информация о вьшках</returns>
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (items.Count != 1)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    views.Suppress("ObjectVisualizer", 0);
    views.Suppress("ObjectApplicability", 0);
    views.Suppress("ObjectFiles", 0);
    views.Suppress("ObjectSecurity", 0);
    return views;
  }
}
