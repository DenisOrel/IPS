// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavWindow
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Site.Client;

public class PortalNavWindow : NavWindow
{
  public PortalNavWindow()
  {
    this.Guid = new Guid("87E86071-E881-4105-814A-DABB869F3CE4");
    this.TreeView.OnGetSupportedColumnsEventHandler += new GetSupportedColumnsEventHandler(PortalNavWindow.GetSupportedColumns);
    this.PersistState = false;
  }

  public static NodeColumnCollection GetSupportedColumns(object sender = null)
  {
    return Intermech.Site.Client.PortalNavigator.Helper.GetPublishedObjectColumns();
  }

  public override string HelpID => this.TreeView.Focused ? "1655" : base.HelpID;
}
