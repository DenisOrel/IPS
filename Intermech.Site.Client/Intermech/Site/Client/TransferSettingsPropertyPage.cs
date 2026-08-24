// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.TransferSettingsPropertyPage
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.Client;
using System;

#nullable disable
namespace Intermech.Site.Client;

internal abstract class TransferSettingsPropertyPage : IPropertyPage
{
  private ITransferSettings _settings;
  private ClassWrapperForPropertyGrid _object;

  public PropertyPageType Type => PropertyPageType.Object;

  public object Control
  {
    get
    {
      if (this._object == null)
      {
        this._settings = this.Settings;
        this._object = new ClassWrapperForPropertyGrid((object) this._settings);
      }
      return (object) this._object;
    }
  }

  protected abstract ITransferSettings Settings { get; }

  public abstract string PageName { get; }

  public abstract string HelpTopicID { get; }

  public abstract string HeaderText { get; }

  public event EventHandler Changed;

  public void Apply()
  {
    if (this._settings == null)
      return;
    this._settings.Apply();
    this._object.ResetOldValues();
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, new EventArgs());
  }

  public void Cancel()
  {
    if (this._settings == null)
      return;
    this._settings.OnCancel();
  }
}
