// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.ObjectTypeSettingViewProvider
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

internal class ObjectTypeSettingViewProvider : IViewsProvider
{
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    if (items.Count != 1)
      return ViewsInfo.Empty;
    ViewsInfo views = new ViewsInfo();
    views.Add("ObjectTypeSettingViewPage", new ViewInfo(0, typeof (ObjectTypeSettingView)));
    return views;
  }
}
