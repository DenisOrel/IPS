// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VisDialogs.VisSchemeProvider
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Pdm.VisDialogs;

internal class VisSchemeProvider : IViewsProvider
{
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    ViewsInfo views = new ViewsInfo();
    views.Add("PDM.VisSchemeView", new ViewInfo(0, 1087, typeof (VisSchemeView)));
    return views;
  }
}
