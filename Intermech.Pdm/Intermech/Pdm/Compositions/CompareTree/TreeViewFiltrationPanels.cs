// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.TreeViewFiltrationPanels
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class TreeViewFiltrationPanels
{
  private FiltrationPanel _filtrationPanel;
  private ContextDropDownControl _contextPanel;
  private ComparePanelSeriesToolbar _comparePanelSeriesToolbar;
  private readonly IAdditionalCompositionFiltrationService _addFiltrationService;

  public TreeViewFiltrationPanels()
  {
    this._addFiltrationService = ServicesManager.GetService(typeof (IAdditionalCompositionFiltrationService)) as IAdditionalCompositionFiltrationService;
  }

  public void Create(
    ToolBar filterToolbar,
    ComboBoxItem cbFiltrationRule,
    ButtonItem btRuleBrowser,
    ButtonItem btRuleVariant,
    ButtonItem btRuleHint,
    ButtonItem btUseStoredExplicitPartVersionID,
    ButtonItem buttonEditingContextsBrowse,
    DropDownMenuItem contextsList,
    ToolBarContainer toolBarContainer)
  {
    this._filtrationPanel = new FiltrationPanel(filterToolbar, cbFiltrationRule, btRuleBrowser, btRuleVariant, btRuleHint, btUseStoredExplicitPartVersionID, false);
    this.AddCommandGuid = this.InitFiltrationPanel(this._filtrationPanel);
    this._contextPanel = new ContextDropDownControl(buttonEditingContextsBrowse, contextsList, (Image) null, (IList<long>) null, 0L);
    this._contextPanel.Load((IList<long>) new List<long>(), 0L);
    this._comparePanelSeriesToolbar = new ComparePanelSeriesToolbar((IFiltrationService) this._filtrationPanel, toolBarContainer);
    this._comparePanelSeriesToolbar.Initialize(false);
  }

  public CompositionFiltrationSettings CompositionFiltrationSettings
  {
    get
    {
      HybridDictionary tags = this._filtrationPanel.Filtration.Tags;
      this._addFiltrationService.GetToolBar(this.AddCommandGuid).GetPluginData(tags);
      this._comparePanelSeriesToolbar.GetPluginData(tags);
      return new CompositionFiltrationSettings(this._filtrationPanel.RuleClass, tags, this._contextPanel.SelectedItem);
    }
  }

  private Guid InitFiltrationPanel(FiltrationPanel filtrationPanel)
  {
    filtrationPanel.Initialize();
    filtrationPanel.FiltrationReload("cad00601-306c-11d8-b4e9-00304f19f545");
    Guid commands = this._addFiltrationService.CreateCommands((IFiltrationService) filtrationPanel);
    filtrationPanel.FiltrationApplyUpdates(false);
    return commands;
  }

  public Guid AddCommandGuid { get; private set; }
}
