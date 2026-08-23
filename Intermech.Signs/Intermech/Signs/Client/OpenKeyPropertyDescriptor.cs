// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.OpenKeyPropertyDescriptor
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Interfaces;
using Intermech.Localization;
using Intermech.Signs.Interfaces;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Класс представляющий собой одну строку в PropertyGrid</summary>
public class OpenKeyPropertyDescriptor : PropertyDescriptor
{
  private OpenKey _base;
  private PropertyDescriptor _prop;
  private string displayName = string.Empty;

  /// <summary>Конструктор</summary>
  /// <param name="baseClass">ключ</param>
  /// <param name="prop"></param>
  public OpenKeyPropertyDescriptor(OpenKey baseClass, PropertyDescriptor prop)
    : base((MemberDescriptor) prop)
  {
    this._base = baseClass;
    this._prop = prop;
  }

  /// <summary>
  /// 
  /// </summary>
  public override Type ComponentType => this._prop.ComponentType;

  /// <summary>
  /// 
  /// </summary>
  public override bool IsReadOnly => true;

  /// <summary>
  /// 
  /// </summary>
  public override Type PropertyType => this._prop.PropertyType;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="component"></param>
  /// <returns></returns>
  public override bool CanResetValue(object component) => false;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="component"></param>
  /// <returns></returns>
  public override object GetValue(object component) => (object) this._base.Key;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="component"></param>
  public override void ResetValue(object component)
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="component"></param>
  /// <param name="value"></param>
  public override void SetValue(object component, object value)
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="component"></param>
  /// <returns></returns>
  public override bool ShouldSerializeValue(object component) => false;

  /// <summary>
  /// 
  /// </summary>
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

  /// <summary>
  /// 
  /// </summary>
  public override string Category => LocalizationHolder.rm.GetString("Signs_2");

  /// <summary>Базовый открытый ключ</summary>
  public OpenKey Parent => this._base;
}
