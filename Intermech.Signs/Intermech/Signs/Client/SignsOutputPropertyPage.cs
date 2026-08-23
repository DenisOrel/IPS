// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignsOutputPropertyPage
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Interfaces.Client;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

#nullable disable
namespace Intermech.Signs.Client;

internal class SignsOutputPropertyPage : IPropertyPage, IPropertyPageSearchOptionEvents
{
  /// <summary>Контейнер сервисов</summary>
  private IServiceProvider _provider;
  private SignsOutputProperties _props;
  private ClassWrapperForPropertyGrid _object;

  public SignsOutputPropertyPage(IServiceProvider provider)
  {
    if (!(ServicesManager.GetService(typeof (ICurrentUserAndRole)) is ICurrentUserAndRole service) || !service.IsAdmin)
      return;
    this._provider = provider;
    ((IPropertyPagesService) this._provider.GetService(typeof (IPropertyPagesService)))?.AddPage(LocalizationHolder.rm.GetString("SignsSettingsOutput"), (IPropertyPage) this);
  }

  public event EventHandler Changed;

  public PropertyPageType Type => PropertyPageType.Object;

  public object Control
  {
    get
    {
      if (this._object == null)
      {
        this._props = new SignsOutputProperties();
        this._object = new ClassWrapperForPropertyGrid((object) this._props);
      }
      return (object) this._object;
    }
  }

  public string PageName => LocalizationHolder.rm.GetString("SignsOutputPropertiesPageName");

  public void Apply()
  {
    if (this._props == null)
      return;
    if (!DateTime.TryParseExact(DateTime.Now.ToString(this._props.SignDateOutputFormat), this._props.SignDateOutputFormat, (IFormatProvider) null, DateTimeStyles.None, out DateTime _))
      throw new KernelException(LocalizationHolder.rm.GetString("SignDateFormatError"));
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

  public string HelpTopicID => string.Empty;

  public string HeaderText
  {
    [DebuggerStepThrough] get => this.PageName;
  }

  /// <summary>
  /// Возвращает список имен настроек, содержащихся в контроле
  /// </summary>
  public List<string> GetOptionNames()
  {
    return !(this.Control is ClassWrapperForPropertyGrid control) ? new List<string>() : IPropertyPageHelper.GetOptionNames((ICustomTypeDescriptor) control);
  }
}
