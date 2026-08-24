// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.ConfiguratorCommand
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Bars;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using System;
using System.Collections.Specialized;

#nullable disable
namespace Intermech.PdmConfigurator;

internal sealed class ConfiguratorCommand : ICompositionFiltrationCommand
{
  private ButtonItem _buttonConfigurator;
  private bool _configuratorEnabled = true;
  private bool _inEvent;
  private IFiltrationService _filtration;

  public ConfiguratorCommand(IFiltrationService filtration) => this._filtration = filtration;

  public object Value => (object) this._configuratorEnabled;

  public void CreateCommand(INamedImageList namedImageList)
  {
    this._buttonConfigurator = this._filtration.AddNewButton();
    this._buttonConfigurator.BeginGroup = true;
    this._buttonConfigurator.ShowText = false;
    this._buttonConfigurator.ImageIndex = namedImageList.ImageIndex("imgPdmConfigurator.Configurator");
    this._buttonConfigurator.AutoToggle = AutoToggleType.Single;
    this._buttonConfigurator.Text = "";
    this._buttonConfigurator.ToolTipText = LocalizationHolder.rm.GetString("PdmConfigurator_100");
    this._buttonConfigurator.Checked = true;
    this._buttonConfigurator.Enabled = (ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole).EnabledPdmConfigurator;
    this._buttonConfigurator.Click += new EventHandler(this.ButtonConfiguratorClick);
    this._configuratorEnabled = this._buttonConfigurator.Checked && this._buttonConfigurator.Enabled;
    this._filtration.OnFiltrationChanged += new FiltrationChanged(this.OnFiltrationChanged);
  }

  private void OnFiltrationChanged(IFiltrationSettings NewFiltration, bool FiltrationValid)
  {
    if (this._inEvent)
      return;
    bool inEvent = this._inEvent;
    this._buttonConfigurator.Enabled = (ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole).EnabledPdmConfigurator;
    if (this._filtration == null)
      return;
    try
    {
      this._inEvent = true;
      bool result = false;
      if (this._filtration.Filtration.Tags[(object) "{0422E069-0A1D-4235-85E8-C52C3516CFC1}"] != null)
        bool.TryParse(this._filtration.Filtration.Tags[(object) "{0422E069-0A1D-4235-85E8-C52C3516CFC1}"].ToString(), out result);
      this._buttonConfigurator.Checked = !result;
      this._configuratorEnabled = this._buttonConfigurator.Checked && this._buttonConfigurator.Enabled;
    }
    finally
    {
      this._inEvent = inEvent;
    }
  }

  private void ButtonConfiguratorClick(object sender, EventArgs e)
  {
    if (this._inEvent)
      return;
    bool inEvent = this._inEvent;
    this._buttonConfigurator.Enabled = (ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole).EnabledPdmConfigurator;
    this._configuratorEnabled = (sender as ButtonItemBase).Checked && (sender as ButtonItemBase).Enabled;
    if (this._filtration == null)
      return;
    try
    {
      this._inEvent = true;
      this._buttonConfigurator.Checked = this._configuratorEnabled;
      this._filtration.Filtration.Tags[(object) "{0422E069-0A1D-4235-85E8-C52C3516CFC1}"] = (object) !(sender as ButtonItemBase).Checked;
      this._filtration.FiltrationApplyUpdates(true);
    }
    finally
    {
      this._inEvent = inEvent;
    }
  }

  public void OnGetPluginData(HybridDictionary tag)
  {
    if (this._configuratorEnabled)
      return;
    tag[(object) "{0422E069-0A1D-4235-85E8-C52C3516CFC1}"] = (object) true;
  }

  public void OnPutPluginData(HybridDictionary tag)
  {
    bool result = false;
    if (tag[(object) "{0422E069-0A1D-4235-85E8-C52C3516CFC1}"] != null && !bool.TryParse(tag[(object) "{0422E069-0A1D-4235-85E8-C52C3516CFC1}"].ToString(), out result) || this._buttonConfigurator.Checked == !result)
      return;
    this._buttonConfigurator.Checked = !result;
    this.ButtonConfiguratorClick((object) this, (EventArgs) null);
  }
}
