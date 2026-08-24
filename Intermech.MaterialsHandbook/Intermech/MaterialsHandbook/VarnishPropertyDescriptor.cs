// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.VarnishPropertyDescriptor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class VarnishPropertyDescriptor : PropertyDescriptor
{
  private object _component;
  private PropertyDescriptor _propertyDescriptor;

  public VarnishPropertyDescriptor(
    object component,
    PropertyDescriptor propertyDescriptor,
    string name,
    Attribute[] attributes,
    TypeConverter converter)
    : base(name, attributes)
  {
    this._component = component;
    this._propertyDescriptor = propertyDescriptor;
    this.Converter = converter;
  }

  public override bool CanResetValue(object component) => true;

  public override Type ComponentType => this._component.GetType();

  public override object GetValue(object component)
  {
    return this._component == null ? this._propertyDescriptor.GetValue(component) : this._propertyDescriptor.GetValue(this._component);
  }

  public override bool IsReadOnly => false;

  public override Type PropertyType => typeof (string);

  public override void ResetValue(object component)
  {
    this._propertyDescriptor.ResetValue(component);
  }

  public override void SetValue(object component, object value)
  {
    this._propertyDescriptor.SetValue(this._component, value);
  }

  public override bool ShouldSerializeValue(object component) => false;

  public override TypeConverter Converter { get; }
}
