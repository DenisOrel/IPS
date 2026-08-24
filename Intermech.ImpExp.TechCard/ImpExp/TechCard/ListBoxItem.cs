// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.ListBoxItem
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard;

internal class ListBoxItem : IComparable
{
  private readonly object _item;
  private readonly string _text;
  private readonly FieldTypes _fieldType;

  public ListBoxItem(object item, string text, FieldTypes fieldType)
  {
    this._item = item;
    this._text = text;
    this._fieldType = fieldType;
  }

  public object Item => this._item;

  public FieldTypes FieldType => this._fieldType;

  public override string ToString() => this._text;

  public int CompareTo(object obj)
  {
    return this._item != null && obj is ListBoxItem ? string.CompareOrdinal(this._text, obj.ToString()) : throw new ArgumentException("Нельзя сравнивать объекты (null)");
  }
}
