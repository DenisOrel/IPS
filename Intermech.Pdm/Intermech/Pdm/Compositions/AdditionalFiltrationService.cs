// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.AdditionalFiltrationService
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Pdm.CompositionFiltration;
using Intermech.Search;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class AdditionalFiltrationService : IAdditionalCompositionFiltrationService
{
  private List<AdditionalFiltrationToolBar> _toolBars;
  private INamedImageList _namedImageList;
  private IMainMenuService _mainMenuService;
  private INotificationService _notificationService;

  public AdditionalFiltrationService(
    INamedImageList namedImageList,
    IMainMenuService mainMenuService,
    INotificationService notificationService)
  {
    this._mainMenuService = mainMenuService;
    this._namedImageList = namedImageList;
    this._notificationService = notificationService;
    this._toolBars = new List<AdditionalFiltrationToolBar>();
  }

  public Guid CreateCommands(IFiltrationService filtration)
  {
    return this.CreateCommands(filtration, AdditionalFiltrationToolBarOptions.None, Guid.Empty);
  }

  public Guid CreateCommands(
    IFiltrationService filtration,
    AdditionalFiltrationToolBarOptions options,
    Guid registerGuid)
  {
    IMainMenuService mainMenuService = (options & AdditionalFiltrationToolBarOptions.WithMainMenu) == AdditionalFiltrationToolBarOptions.WithMainMenu ? this._mainMenuService : (IMainMenuService) null;
    INotificationService notificationService = (options & AdditionalFiltrationToolBarOptions.WithNotificationServiceUsing) == AdditionalFiltrationToolBarOptions.WithNotificationServiceUsing ? this._notificationService : (INotificationService) null;
    HiddenChildsCommand hiddenChildsCommand = new HiddenChildsCommand(filtration, mainMenuService);
    List<ICompositionFiltrationCommand> filtrationCommand1 = new List<ICompositionFiltrationCommand>()
    {
      (ICompositionFiltrationCommand) hiddenChildsCommand,
      (ICompositionFiltrationCommand) new HiddenCompositionCommand(filtration, mainMenuService, hiddenChildsCommand),
      (ICompositionFiltrationCommand) new ContextCommand(filtration),
      (ICompositionFiltrationCommand) new ShowActualCompositionsCommand(filtration),
      (ICompositionFiltrationCommand) new AnalogCommand(filtration, notificationService)
    };
    if (this.GetCompositionFiltrationCommand != null)
      filtrationCommand1.Add(this.GetCompositionFiltrationCommand((object) this, new GetCompositionFiltrationCommandEventArgs(filtration, mainMenuService, notificationService)));
    foreach (ICompositionFiltrationCommand filtrationCommand2 in filtrationCommand1)
      filtrationCommand2.CreateCommand(this._namedImageList);
    AdditionalFiltrationToolBar ClientPluginsDataTransfer = new AdditionalFiltrationToolBar(filtrationCommand1, registerGuid);
    if (registerGuid != Guid.Empty)
      (ServicesManager.GetService(typeof (IClientPluginsService)) as IClientPluginsService).RegisterClientPlugin(registerGuid, (IClientPluginsDataTransfer) ClientPluginsDataTransfer);
    this._toolBars.Add(ClientPluginsDataTransfer);
    return ClientPluginsDataTransfer.PluginGuid;
  }

  public void OnToolBarClosed(Guid guid)
  {
    AdditionalFiltrationToolBar filtrationToolBar = this._toolBars.Find((Predicate<AdditionalFiltrationToolBar>) (x => x.PluginGuid.Equals(guid)));
    if (filtrationToolBar.Registered)
      (ServicesManager.GetService(typeof (IClientPluginsService)) as IClientPluginsService).UnregisterClientPlugin(guid);
    this._toolBars.Remove(filtrationToolBar);
  }

  public IClientPluginsDataTransfer GetToolBar(Guid guid)
  {
    return (IClientPluginsDataTransfer) this._toolBars.Find((Predicate<AdditionalFiltrationToolBar>) (x => x.PluginGuid.Equals(guid)));
  }

  public event GetCompositionFiltrationCommandEventHandler GetCompositionFiltrationCommand;
}
