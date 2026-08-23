// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.OpenKeyPropertyDescriptor
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Interfaces;
using Intermech.Localization;
using Intermech.Signs.Interfaces;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.Signs.Client;

public class OpenKeyPropertyDescriptor : PropertyDescriptor
{
  private OpenKey _base;
  private PropertyDescriptor _prop;
  private string displayName = string.Empty;

  public OpenKeyPropertyDescriptor(OpenKey baseClass, PropertyDescriptor prop)
    : base((MemberDescriptor) prop)
  {
    this._base = baseClass;
    this._prop = prop;
  }

  public override Type ComponentType => this._prop.ComponentType;

  public override bool IsReadOnly => true;

  public override Type PropertyType => this._prop.PropertyType;

  public override bool CanResetValue(object component) => false;

  public override object GetValue(object component) => (object) this._base.Key;

  public override void ResetValue(object component)
  {
  }

  public override void SetValue(object component, object value)
  {
  }

  public override bool ShouldSerializeValue(object component) => false;

  public override string DisplayName
  {
    get
    {
      if (string.IsNullOrEmpty(this.displayName))
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
          this.displayName = sessionKeeper.Session.GetObject(this._base.ProviderGuid).Caption;
      }
      return this.displayName;
    }
  }

  public override string Category => LocalizationHolder.rm.GetString("Signs_2");

  public OpenKey Parent => this._base;
}
