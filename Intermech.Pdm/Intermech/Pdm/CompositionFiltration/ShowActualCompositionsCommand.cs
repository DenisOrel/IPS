// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.CompositionFiltration.ShowActualCompositionsCommand
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

internal sealed class ShowActualCompositionsCommand(IFiltrationService filtration) : 
  CompositionFiltrationCommand(filtration, (IMainMenuService) null)
{
  private ButtonItem _showActualCompositionsButton;

  public override object Value => (object) this._showActualCompositionsButton.Checked;

  public override void CreateCommand(INamedImageList namedImageList)
  {
    this._showActualCompositionsButton = this.filtration.AddNewButton();
    this._showActualCompositionsButton.BeginGroup = true;
    this._showActualCompositionsButton.ShowText = false;
    this._showActualCompositionsButton.ImageIndex = namedImageList.ImageIndex("imgSubstitutes.PDM");
    this._showActualCompositionsButton.AutoToggle = AutoToggleType.Single;
    this._showActualCompositionsButton.Text = PDMPluginConsts.buttonSubstitutesText;
    this._showActualCompositionsButton.ToolTipText = PDMPluginConsts.buttonSubstitutesHint;
    this._showActualCompositionsButton.Click += new EventHandler(this.ShowActualComposition);
  }

  private void ShowActualComposition(object sender, EventArgs e)
  {
    this.filtration.Filtration.Tags[(object) "{82E381A1-8952-416A-B303-F81BA2945F8F}"] = this.Value;
    this.filtration.FiltrationApplyUpdates(true);
  }

  public override void OnPutPluginData(HybridDictionary tag)
  {
    if (tag[(object) "{82E381A1-8952-416A-B303-F81BA2945F8F}"] == null)
      return;
    this._showActualCompositionsButton.Checked = Convert.ToBoolean(tag[(object) "{2FACA180-73B8-4F24-9928-5623661BBBE6}"]);
  }

  public override void OnGetPluginData(HybridDictionary tag)
  {
    tag[(object) "{82E381A1-8952-416A-B303-F81BA2945F8F}"] = this.Value;
  }
}
