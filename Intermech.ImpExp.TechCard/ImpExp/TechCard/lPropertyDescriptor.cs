// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.lPropertyDescriptor
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.ImpExp.TechCard;

public class lPropertyDescriptor : PropertyDescriptor
{
  private readonly PropertyDescriptor _descr;
  private readonly object _oldValue;
  private string _displayName;
  private bool _isReadOnly;

  public lPropertyDescriptor(PropertyDescriptor descr, object oldValue)
    : base((MemberDescriptor) descr)
  {
    this._descr = descr;
    this._oldValue = oldValue;
  }

  public lPropertyDescriptor(PropertyDescriptor descr)
    : this(descr, (object) null)
  {
  }

  public object OldValue => this._oldValue;

  public override Type ComponentType => this._descr.ComponentType;

  public override bool IsReadOnly => this._descr.IsReadOnly || this._isReadOnly;

  public void SetReadOnly(bool isReadOnly) => this._isReadOnly = isReadOnly;

  public override Type PropertyType => this._descr.PropertyType;

  public override bool CanResetValue(object component) => this._descr.CanResetValue(component);

  public override void ResetValue(object component) => this._descr.ResetValue(component);

  public override object GetValue(object component) => this._descr.GetValue(component);

  public override void SetValue(object component, object value)
  {
    this._descr.SetValue(component, value);
  }

  public override bool ShouldSerializeValue(object component)
  {
    return !object.Equals(this._oldValue, this.GetValue(component));
  }

  public override string DisplayName
  {
    get => this._displayName != null ? this._displayName : base.DisplayName;
  }

  public void SetDisplayName(string displayName) => this._displayName = displayName;

  public void SetEditor(Type editorBaseType)
  {
    this.AttributeArray = new List<Attribute>((IEnumerable<Attribute>) this.AttributeArray)
    {
      (Attribute) new EditorAttribute(editorBaseType, typeof (UITypeEditor))
    }.ToArray();
  }

  public void SetConverter(Type converterType)
  {
    this.AttributeArray = new List<Attribute>((IEnumerable<Attribute>) this.AttributeArray)
    {
      (Attribute) new TypeConverterAttribute(converterType)
    }.ToArray();
  }
}
