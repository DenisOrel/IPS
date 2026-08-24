// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeSettingsView
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

[ViewDescriptionProvider(typeof (OfficeSettingsView.OfficeSettingsViewDescriptionProvider))]
internal class OfficeSettingsView : UserControl, IView
{
  private long _objectID;
  [CanBeNull]
  private IViewState _viewState;
  [CanBeNull]
  private OfficeSettingsForm _form;
  private bool _loaded;

  public void Initialize([NotNull] ISelectedItems items, [NotNull] IServiceProvider provider)
  {
    this._objectID = items.GetItemData<IDBTypedObjectID>(0).ObjectID;
    this._viewState = provider.GetService<IViewState>(false);
    this._loaded = false;
    this.ImageIndex = Holder.NamedList.ImageIndex("Office.Office");
  }

  public void Activate(IView previousView)
  {
    if (this._form == null)
    {
      this._form = new OfficeSettingsForm(this._objectID, this._viewState);
      this._form.SetParent((Control) this);
    }
    else
      this._form._Unit = this._objectID;
    if (this._loaded)
      return;
    this._form.Reload();
    this._loaded = true;
  }

  public void Deactivate(IView nextView)
  {
    OfficeSettingsForm form = this._form;
    if ((form != null ? (form.IsModified ? 1 : 0) : 0) == 0 || MessageBox.Show("На закладке остались несохраненные данные. Сохранить?", this.Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    this._form.Save();
  }

  [NotNull]
  public string Caption => "Настройки канцелярии";

  public int ImageIndex { get; private set; } = -1;

  public int OrderID => 59;

  private sealed class OfficeSettingsViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      return new ViewDescription()
      {
        Caption = "Настройки канцелярии",
        ImageIndex = Holder.NamedList.ImageIndex("Office.Office"),
        OrderID = 59
      };
    }
  }
}
