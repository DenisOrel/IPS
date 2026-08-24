// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.CompositionFiltration.HiddenCompositionCommand
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Search;
using System;
using System.Collections.Specialized;

#nullable disable
namespace Intermech.Pdm.CompositionFiltration;

internal sealed class HiddenCompositionCommand : DoubleCheckedItemCommand
{
  private ICompositionFiltrationCommand _hiddenChildsCommand;
  private bool _subscribedToConfigationOptionChanged;

  public HiddenCompositionCommand(
    IFiltrationService filtration,
    IMainMenuService mainMenuService,
    HiddenChildsCommand hiddenChildsCommand)
    : base(filtration, mainMenuService)
  {
    this._hiddenChildsCommand = (ICompositionFiltrationCommand) hiddenChildsCommand;
  }

  public override void CreateCommand(INamedImageList namedImageList)
  {
    this.buttonItem = this.filtration.AddNewButton();
    this.buttonItem.BeginGroup = false;
    this.buttonItem.ShowText = false;
    this.buttonItem.ImageIndex = namedImageList.ImageIndex("imgHiddenComposition.PDM");
    this.buttonItem.AutoToggle = AutoToggleType.Single;
    this.buttonItem.Text = string.Empty;
    this.buttonItem.ToolTipText = PDMPluginConsts.buttonHiddenCompositionHint;
    this.buttonItem.Click += new EventHandler(((DoubleCheckedItemCommand) this).OnClick);
    MenuButtonItem menuButtonItem = new MenuButtonItem(PDMPluginConsts.menuHiddenComposition);
    menuButtonItem.AutoToggle = AutoToggleType.Single;
    menuButtonItem.CommandName = "PDM.HiddenComposition";
    menuButtonItem.BeginGroup = false;
    menuButtonItem.ImageIndex = namedImageList.ImageIndex("imgHiddenComposition.PDM");
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

  public override void OnGetPluginData(HybridDictionary tag)
  {
    HiddenCompositionFiltrationMode compositionFiltrationMode = (bool) this._hiddenChildsCommand.Value ? (!(bool) this.Value ? HiddenCompositionFiltrationMode.HideChilds : HiddenCompositionFiltrationMode.HideAll) : (!(bool) this.Value ? HiddenCompositionFiltrationMode.None : HiddenCompositionFiltrationMode.HideAll);
    tag[(object) "{54C2DCB9-63C7-4736-867B-1EA7539B7645}"] = (object) compositionFiltrationMode;
    tag[(object) "{4545B911-6878-4625-AA9E-33B6ACE8CDCF}"] = (object) (bool) this._hiddenChildsCommand.Value;
    tag[(object) "{86C8373B-7537-40E1-8F02-24444C4FED7A}"] = this.Value;
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
