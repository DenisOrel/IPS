// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Advanced.DictionaryPropertyDescriptor
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.TechCard.Advanced;

internal class DictionaryPropertyDescriptor : PropertyDescriptor
{
  private readonly IDictionary _dictionary;
  private readonly object _key;

  internal DictionaryPropertyDescriptor(IDictionary d, object key)
    : base(key.ToString(), (Attribute[]) null)
  {
    this._dictionary = d;
    this._key = key;
  }

  public override Type PropertyType => this._dictionary[this._key].GetType();

  public override void SetValue(object component, object value)
  {
    this._dictionary[this._key] = value;
  }

  public override object GetValue(object component) => this._dictionary[this._key];

  public override bool IsReadOnly => false;

  public override Type ComponentType => (Type) null;

  public override bool CanResetValue(object component) => false;

  public override void ResetValue(object component)
  {
  }

  public override bool ShouldSerializeValue(object component) => false;
}
