// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ConfigSettingsPropertyDescriptor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Imbase;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class ConfigSettingsPropertyDescriptor : PropertyDescriptor
{
  private object _component;
  private object _value;
  private PropertyDescriptorCollection _children = new PropertyDescriptorCollection((PropertyDescriptor[]) null);

  internal PropertyDescriptorCollection ChildProperties
  {
    get => this._children;
    set
    {
      this._children = value;
      this._children = this._children ?? new PropertyDescriptorCollection((PropertyDescriptor[]) null);
    }
  }

  internal DisableImbaseCategory DescriptorCategory { get; private set; }

  internal bool PropertiesSupported => this._children.Count > 0;

  public ConfigSettingsPropertyDescriptor(
    object component,
    string name,
    Attribute[] attributes,
    DisableImbaseCategory category,
    object value)
    : base(name, attributes)
  {
    this._component = component;
    this.DescriptorCategory = category;
    this._value = value;
  }

  public override bool CanResetValue(object component) => true;

  public override Type ComponentType => this._component.GetType();

  public override object GetValue(object component) => this._value;

  public override bool IsReadOnly => false;

  public override Type PropertyType => typeof (string);

  public override void ResetValue(object component) => this._value = (object) null;

  public override void SetValue(object component, object value) => this._value = value;

  public override bool ShouldSerializeValue(object component) => false;
}
