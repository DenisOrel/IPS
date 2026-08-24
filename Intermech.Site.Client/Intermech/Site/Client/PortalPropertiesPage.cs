// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalPropertiesPage
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Site.Client.Settings;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class PortalPropertiesPage : IPropertyPage
{
  private readonly IServiceProvider _provider;
  private PortalProperties _props;
  private ClassWrapperForPropertyGrid _object;

  public PortalPropertiesPage(IServiceProvider provider) => this._provider = provider;

  public string HelpTopicID => "1669";

  public event EventHandler Changed;

  public PropertyPageType Type => PropertyPageType.Object;

  public object Control
  {
    get
    {
      if (this._object == null)
      {
        this._props = new PortalProperties();
        this._object = new ClassWrapperForPropertyGrid((object) this._props);
      }
      return (object) this._object;
    }
  }

  public string PageName => LocalizationHolder.rm.GetString("Site.Client_89");

  public string HeaderText
  {
    [DebuggerStepThrough] get => this.PageName;
  }

  public void Apply()
  {
    if (this._props == null)
      return;
    this._props.ApplyUpdates();
    this._object.ResetOldValues();
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, new EventArgs());
  }

  public void Cancel()
  {
    if (this._props == null)
      return;
    this._props.Inited = false;
  }
}
