// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Client.MainMenuModule
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.ApplicationModel;
using Intermech.Bars;
using Intermech.Interfaces.Client;
using Intermech.Scripting.ScriptPad;
using Intermech.Scripting.Services;
using Intermech.Search;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Scripting.Client;

internal sealed class MainMenuModule : InitializerModule
{
  private const string ShowScriptPadCommand = "ShowScriptPad";
  private IMainMenuService mainMenuService;
  private ScriptPadService ideService;
  private MenuButtonItem scriptPadButton;
  private ICurrentUserAndRole currentUserAndRole;

  public MainMenuModule(
    IMainMenuService mainMenuService,
    ScriptPadService ideService,
    ICurrentUserAndRole currentUserAndRole)
  {
    if (mainMenuService == null)
      throw new ArgumentNullException(nameof (mainMenuService));
    if (ideService == null)
      throw new ArgumentNullException(nameof (ideService));
    if (currentUserAndRole == null)
      throw new ArgumentNullException(nameof (currentUserAndRole));
    this.mainMenuService = mainMenuService;
    this.ideService = ideService;
    this.currentUserAndRole = currentUserAndRole;
  }

  protected override void DoInitialize()
  {
    base.DoInitialize();
    if (!this.currentUserAndRole.IsAdmin)
      return;
    this.RegisterScriptPadButton();
  }

  protected override void DoShutdown()
  {
    this.UnregisterScriptPadButton();
    base.DoShutdown();
  }

  private void RegisterScriptPadButton()
  {
    this.scriptPadButton = new MenuButtonItem();
    this.scriptPadButton.Text = "Script pad";
    this.scriptPadButton.Icon = IDEResources.IDEWindowIcon;
    this.scriptPadButton.CommandName = "ShowScriptPad";
    this.scriptPadButton.Click += new EventHandler(this.ShowScriptPadHandler);
    this.scriptPadButton.ShortcutActive = true;
    this.scriptPadButton.Shortcut = Shortcut.AltF11;
    this.mainMenuService.RegisterMenuItems(MainMenuItemSite.Applications, MainMenuItemPosition.Default, this.scriptPadButton);
  }

  private void UnregisterScriptPadButton()
  {
    if (this.scriptPadButton == null)
      return;
    this.mainMenuService.UnregiterMenuItems(this.scriptPadButton);
    this.scriptPadButton.Dispose();
    this.scriptPadButton = (MenuButtonItem) null;
  }

  private void ShowScriptPadHandler(object sender, EventArgs e)
  {
    IDEPresenter idePresenter = this.ideService.OpenIDEWindow();
    if (idePresenter.HasOpenScripts())
      return;
    idePresenter.CreateScriptProject();
  }
}
