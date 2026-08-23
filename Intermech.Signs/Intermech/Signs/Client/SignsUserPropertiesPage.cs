// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignsUserPropertiesPage
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Interfaces.Client;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace Intermech.Signs.Client;

public class SignsUserPropertiesPage : IPropertyPage, IPropertyPageSearchOptionEvents
{
  private IServiceProvider _provider;
  private SignsUserProperties _props;
  private ClassWrapperForPropertyGrid _object;

  public PropertyPageType Type => PropertyPageType.Object;

  public object Control
  {
    get
    {
      if (this._object == null)
      {
        this._props = new SignsUserProperties();
        this._object = new ClassWrapperForPropertyGrid((object) this._props);
      }
      return (object) this._object;
    }
  }

  public string PageName => LocalizationHolder.rm.GetString("SignsUserPageName");

  public string HelpTopicID => string.Empty;

  public string HeaderText
  {
    [DebuggerStepThrough] get => this.PageName;
  }

  public event EventHandler Changed;

  public void Apply()
  {
    if (this._props == null)
      return;
    this._props.ApplyUpdates();
    this._object.ResetOldValues();
    if (this.Changed == null)
      return;
    this.Changed((object) this, new EventArgs());
  }

  public void Cancel()
  {
    if (this._props == null)
      return;
    this._props._inited = false;
  }

  public SignsUserPropertiesPage(IServiceProvider provider)
  {
    this._provider = provider;
    ((IPropertyPagesService) this._provider.GetService(typeof (IPropertyPagesService)))?.AddPage(LocalizationHolder.rm.GetString("SignsUserSettings"), (IPropertyPage) this);
  }

  public List<string> GetOptionNames()
  {
    return !(this.Control is ClassWrapperForPropertyGrid control) ? new List<string>() : IPropertyPageHelper.GetOptionNames((ICustomTypeDescriptor) control);
  }
}
