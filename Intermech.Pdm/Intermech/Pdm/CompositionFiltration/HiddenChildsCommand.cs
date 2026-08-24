// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.CompositionFiltration.HiddenChildsCommand
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Search;
using System;

#nullable disable
namespace Intermech.Pdm.CompositionFiltration;

internal sealed class HiddenChildsCommand(
  IFiltrationService filtration,
  IMainMenuService mainMenuService) : DoubleCheckedItemCommand(filtration, mainMenuService)
{
  private bool _subscribedToConfigationOptionChanged;

  public override void CreateCommand(INamedImageList namedImageList)
  {
    this.buttonItem = this.filtration.AddNewButton();
    this.buttonItem.BeginGroup = true;
    this.buttonItem.ShowText = false;
    this.buttonItem.ImageIndex = namedImageList.ImageIndex("imgHiddenChilds.PDM");
    this.buttonItem.AutoToggle = AutoToggleType.Single;
    this.buttonItem.Text = string.Empty;
    this.buttonItem.ToolTipText = PDMPluginConsts.buttonHiddenChildsHint;
    this.buttonItem.Click += new EventHandler(((DoubleCheckedItemCommand) this).OnClick);
    MenuButtonItem menuButtonItem = new MenuButtonItem(PDMPluginConsts.menuHiddenChilds);
    menuButtonItem.AutoToggle = AutoToggleType.Single;
    menuButtonItem.CommandName = "PDM.HiddenChilds";
    menuButtonItem.BeginGroup = true;
    menuButtonItem.ImageIndex = namedImageList.ImageIndex("imgHiddenChilds.PDM");
    this.menuItem = menuButtonItem;
    this.menuItem.Click += new EventHandler(((DoubleCheckedItemCommand) this).OnClick);
    if (this.mainMenuService != null)
      this.mainMenuService.RegisterMenuItemsGroup(MainMenuItemSite.Composition, MainMenuItemPosition.Default, false, this.menuItem);
    if (!this._subscribedToConfigationOptionChanged)
    {
      if (ServicesManager.GetService(typeof (INotificationService)) is INotificationService service)
        service.Subscribe("ConfigurationOptionChanged", new NotificationEventHandler(this.NotificationService_ConfigurationOptionChanged));
      this._subscribedToConfigationOptionChanged = true;
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      bool flag = sessionKeeper.Session.Configurations.ReadBool("KERNEL", "PERFORMANCE", "UseHiddenComposition", true, DBConfigMode.GlobalOnly);
      this.buttonItem.Enabled = flag;
      this.menuItem.Enabled = flag;
    }
  }

  private void NotificationService_ConfigurationOptionChanged(
    object sender,
    NotificationEventArgs e)
  {
    if (!(e is ConfigurationOptionChangedEventArgs changedEventArgs) || !(changedEventArgs.ModuleName == "KERNEL") || !(changedEventArgs.SectionId == "PERFORMANCE") || !(changedEventArgs.ParamName == "UseHiddenComposition"))
      return;
    bool newValue = (bool) changedEventArgs.NewValue;
    this.buttonItem.Enabled = newValue;
    this.menuItem.Enabled = newValue;
  }
}
