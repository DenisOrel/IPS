// Decompiled with JetBrains decompiler
// Type: Intermech.ReportBuilder.Client.IMenuScript
// Assembly: Intermech.ReportBuilder.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 84A30C1D-3856-44D0-92A6-A87D49736592
// Assembly location: D:\IPS\Client\Intermech.ReportBuilder.Client.dll

using Intermech.Interfaces;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.ReportBuilder.Client;

internal interface IMenuScript
{
  string CommandName { get; }

  string CommandText { get; }

  ClickEventHandler Target { get; }

  bool Visible(IUserSession session, ISelectedItems items, IServiceProvider viewServices);
}
