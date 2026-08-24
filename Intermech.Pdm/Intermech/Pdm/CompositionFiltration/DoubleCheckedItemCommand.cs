// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.CompositionFiltration.DoubleCheckedItemCommand
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Interfaces.Client;
using Intermech.Search;
using System;

#nullable disable
namespace Intermech.Pdm.CompositionFiltration;

internal abstract class DoubleCheckedItemCommand : CompositionFiltrationCommand
{
  protected ButtonItem buttonItem;
  protected MenuButtonItem menuItem;

  public DoubleCheckedItemCommand(IFiltrationService filtration, IMainMenuService mainMenuService)
    : base(filtration, mainMenuService)
  {
  }

  public override object Value => (object) this.buttonItem.Checked;

  private void SetCheckedState(bool state)
  {
    this.menuItem.Checked = state;
    this.buttonItem.Checked = state;
  }

  protected void OnClick(object sender, EventArgs e)
  {
    this.SetCheckedState(((ButtonItemBase) sender).Checked);
    this.filtration.FiltrationApplyUpdates(true);
  }
}
