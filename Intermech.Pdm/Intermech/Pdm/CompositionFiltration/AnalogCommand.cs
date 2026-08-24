// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.CompositionFiltration.AnalogCommand
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Search;
using Intermech.Search.Pdm.Analogs;
using System;
using System.Collections.Specialized;

#nullable disable
namespace Intermech.Pdm.CompositionFiltration;

internal sealed class AnalogCommand : CompositionFiltrationCommand
{
  private AnalogDropDownMenuItem _analogsDropDownMenuItem;
  private INotificationService _notificationService;

  public AnalogCommand(IFiltrationService filtration, INotificationService notificationService)
    : base(filtration, (IMainMenuService) null)
  {
    this._notificationService = notificationService;
  }

  public override object Value
  {
    get => (object) this._analogsDropDownMenuItem.GetCurrentAnalogSelectionMode();
  }

  public override void CreateCommand(INamedImageList namedImageList)
  {
    this._analogsDropDownMenuItem = new AnalogDropDownMenuItem();
    this.filtration.ToolBar.Items.Add((ToolbarItemBase) this._analogsDropDownMenuItem);
    this._analogsDropDownMenuItem.AnalogSelectionModeChanged += new EventHandler(this.AnalogsDropDownMenuItem_AnalogSelectionModeChanged);
  }

  private void AnalogsDropDownMenuItem_AnalogSelectionModeChanged(object sender, EventArgs e)
  {
    if (this._notificationService == null)
      return;
    this._notificationService.FireEvent((object) this, new NotificationEventArgs("ProjectChanged"));
  }

  public override void OnPutPluginData(HybridDictionary tag)
  {
    if (!tag.Contains((object) "B6002FDD-2998-4EE8-986C-66728CBBFBD7"))
      return;
    this._analogsDropDownMenuItem.SetCurrentAnalogSelectionMode(AnalogsHelper.GetAnalogSelectionModeFromRecordSetParamsTags(tag));
  }

  public override void OnGetPluginData(HybridDictionary tag)
  {
    tag[(object) "B6002FDD-2998-4EE8-986C-66728CBBFBD7"] = this.Value;
  }
}
