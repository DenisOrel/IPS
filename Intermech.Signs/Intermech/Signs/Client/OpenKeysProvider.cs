// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.OpenKeysProvider
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Signs.Client;

public class OpenKeysProvider : IViewsProvider
{
  private static bool _registeredView;

  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (!OpenKeysProvider._registeredView)
    {
      AdjustableViewsHelper.RegisterView("OpenKeysView", LocalizationHolder.rm.GetString("Signs_50"), "", LocalizationHolder.rm.GetString("Signs_54"), "", true, 0);
      OpenKeysProvider._registeredView = true;
    }
    if (items.Count != 1)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    views.Add("OpenKeysView", new ViewInfo(0, 1266, typeof (OpenKeysView)));
    return views;
  }
}
