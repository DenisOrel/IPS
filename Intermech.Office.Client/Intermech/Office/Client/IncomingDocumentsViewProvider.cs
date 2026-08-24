// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.IncomingDocumentsViewProvider
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Office.Client;

internal sealed class IncomingDocumentsViewProvider : IViewsProvider
{
  [NotNull]
  public ViewsInfo GetViews(ISelectedItems items, IServiceProvider services)
  {
    ViewsInfo views = new ViewsInfo();
    views.Add("ChildrenView", new ViewInfo(0, -1, typeof (ChildrenView)));
    return views;
  }
}
