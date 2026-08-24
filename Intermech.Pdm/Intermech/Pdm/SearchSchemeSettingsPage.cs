// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SearchSchemeSettingsPage
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Pdm;

public class SearchSchemeSettingsPage : IPropertyPage
{
  private ClassWrapperForPropertyGrid _wrapper;
  private SearchSchemeSettings _settings;

  public string HelpTopicID => "-1";

  public event EventHandler Changed;

  public PropertyPageType Type => PropertyPageType.Object;

  public object Control
  {
    get
    {
      if (this._wrapper == null)
      {
        this._settings = new SearchSchemeSettings();
        this._wrapper = new ClassWrapperForPropertyGrid((object) this._settings);
      }
      return (object) this._wrapper;
    }
  }

  public string PageName => "Настройки работы схем поиска объектов";

  public string HeaderText
  {
    [DebuggerStepThrough] get => this.PageName;
  }

  public void Apply()
  {
    if (this._settings == null)
      return;
    this._settings.Save();
    this._wrapper.ResetOldValues();
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, new EventArgs());
  }

  public void Cancel()
  {
    if (this._settings == null)
      return;
    this._settings.Reset();
  }
}
