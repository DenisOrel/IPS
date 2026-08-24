// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.SiteUsersView
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Bars;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class SiteUsersView : ChildrenView
{
  private int _imageIndex = -1;

  public SiteUsersView() => this.DisableFiltration = true;

  public override string Caption => LocalizationHolder.rm.GetString("Site.Client_9");

  public override int ImageIndex => this._imageIndex;

  public override ContentType ViewContentType => ContentType.NonFolders;

  public override bool QueryStatus(ICommandState commandState)
  {
    int num = base.QueryStatus(commandState) ? 1 : 0;
    if (!(commandState.CommandName == "ParametersCard"))
      return num != 0;
    commandState.Enabled = false;
    return num != 0;
  }
}
