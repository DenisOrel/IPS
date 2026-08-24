// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Analogs.AnalogDropDownMenuItem
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Extensions;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Drawing;

#nullable disable
namespace Intermech.Search.Pdm.Analogs;

internal sealed class AnalogDropDownMenuItem : DropDownMenuItem
{
  private INotificationService _notificationService;
  private MenuButtonItem _doNotChooseAnalogsMenuButtonItem;
  private MenuButtonItem _chooseActingAnalogMenuButtonItem;
  private MenuButtonItem _chooseOneAnalogMenuButtonItem;
  private MenuButtonItem _showAllAnalogsMenuButtonItem;

  public AnalogDropDownMenuItem()
  {
    this.Tag = (object) AnalogSelectionMode.None;
    this.Text = "Подбор аналогов";
    MenuButtonItem menuButtonItem1 = new MenuButtonItem(AnalogSelectionMode.None.GetDescription<AnalogSelectionMode>(), new EventHandler(this.DoNotChooseAnalogsMenuButtonItem_Click));
    menuButtonItem1.Image = (Image) AnalogsResource.DoNotChooseAnalogs;
    menuButtonItem1.Tag = (object) AnalogSelectionMode.None;
    this._doNotChooseAnalogsMenuButtonItem = menuButtonItem1;
    this.Items.Add((ToolbarItemBase) this._doNotChooseAnalogsMenuButtonItem);
    MenuButtonItem menuButtonItem2 = new MenuButtonItem(AnalogSelectionMode.ActualAnalog.GetDescription<AnalogSelectionMode>(), new EventHandler(this.ChooseActingAnalogMenuButtonItem_Click));
    menuButtonItem2.Image = (Image) AnalogsResource.ChooseActingAnalog;
    menuButtonItem2.Tag = (object) AnalogSelectionMode.ActualAnalog;
    this._chooseActingAnalogMenuButtonItem = menuButtonItem2;
    this.Items.Add((ToolbarItemBase) this._chooseActingAnalogMenuButtonItem);
    MenuButtonItem menuButtonItem3 = new MenuButtonItem(AnalogSelectionMode.OneAnalog.GetDescription<AnalogSelectionMode>(), new EventHandler(this.ChooseOneAnalogMenuButtonItem_Click));
    menuButtonItem3.Image = (Image) AnalogsResource.ChooseOneAnalog;
    menuButtonItem3.Tag = (object) AnalogSelectionMode.OneAnalog;
    this._chooseOneAnalogMenuButtonItem = menuButtonItem3;
    this.Items.Add((ToolbarItemBase) this._chooseOneAnalogMenuButtonItem);
    MenuButtonItem menuButtonItem4 = new MenuButtonItem(AnalogSelectionMode.AllAnalogs.GetDescription<AnalogSelectionMode>(), new EventHandler(this.ShowAllAnalogsMenuButtonItem_Click));
    menuButtonItem4.Image = (Image) AnalogsResource.ShowAllAnalogs;
    menuButtonItem4.Tag = (object) AnalogSelectionMode.AllAnalogs;
    this._showAllAnalogsMenuButtonItem = menuButtonItem4;
    this.Items.Add((ToolbarItemBase) this._showAllAnalogsMenuButtonItem);
    this.ChooseAnalogsMenuItem(this._doNotChooseAnalogsMenuButtonItem);
  }

  public event EventHandler AnalogSelectionModeChanged;

  public AnalogSelectionMode GetCurrentAnalogSelectionMode() => (AnalogSelectionMode) this.Tag;

  public void SetCurrentAnalogSelectionMode(AnalogSelectionMode mode)
  {
    switch (mode)
    {
      case AnalogSelectionMode.None:
        this.ChooseAnalogsMenuItem(this._doNotChooseAnalogsMenuButtonItem);
        break;
      case AnalogSelectionMode.ActualAnalog:
        this.ChooseAnalogsMenuItem(this._chooseActingAnalogMenuButtonItem);
        break;
      case AnalogSelectionMode.OneAnalog:
        this.ChooseAnalogsMenuItem(this._chooseOneAnalogMenuButtonItem);
        break;
      case AnalogSelectionMode.AllAnalogs:
        this.ChooseAnalogsMenuItem(this._showAllAnalogsMenuButtonItem);
        break;
      default:
        throw new NotSupportedEnumException((Enum) mode);
    }
  }

  private void DoNotChooseAnalogsMenuButtonItem_Click(object sender, EventArgs e)
  {
    this.SetCurrentAnalogSelectionMode(AnalogSelectionMode.None);
  }

  private void ChooseActingAnalogMenuButtonItem_Click(object sender, EventArgs e)
  {
    this.SetCurrentAnalogSelectionMode(AnalogSelectionMode.ActualAnalog);
  }

  private void ChooseOneAnalogMenuButtonItem_Click(object sender, EventArgs e)
  {
    this.SetCurrentAnalogSelectionMode(AnalogSelectionMode.OneAnalog);
  }

  private void ShowAllAnalogsMenuButtonItem_Click(object sender, EventArgs e)
  {
    this.SetCurrentAnalogSelectionMode(AnalogSelectionMode.AllAnalogs);
  }

  private void ChooseAnalogsMenuItem(MenuButtonItem selectedMenuButtonItem)
  {
    AnalogSelectionMode tag = (AnalogSelectionMode) this.Tag;
    foreach (ButtonItemBase buttonItemBase in (CollectionBase) this.Items)
      buttonItemBase.Checked = false;
    selectedMenuButtonItem.Checked = true;
    this.Image = selectedMenuButtonItem.Image;
    this.Tag = selectedMenuButtonItem.Tag;
    this.Text = selectedMenuButtonItem.Text;
    this.ToolTipText = selectedMenuButtonItem.ToolTipText;
    if (tag == (AnalogSelectionMode) this.Tag)
      return;
    EventHandler selectionModeChanged = this.AnalogSelectionModeChanged;
    if (selectionModeChanged == null)
      return;
    selectionModeChanged((object) this, EventArgs.Empty);
  }
}
