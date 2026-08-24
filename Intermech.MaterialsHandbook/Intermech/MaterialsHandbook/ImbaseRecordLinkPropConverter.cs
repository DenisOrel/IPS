// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ImbaseRecordLinkPropConverter
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Imbase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class ImbaseRecordLinkPropConverter : TypeConverter
{
  private Dictionary<string, string> _imKeysNamesDict;

  public ImbaseRecordLinkPropConverter(IEnumerable<string> imbaseKeys)
  {
    if (imbaseKeys == null)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService))
        return;
      this._imKeysNamesDict = customService.NameRecordReferences(sessionKeeper.Session.SessionGUID, imbaseKeys.ToList<string>());
    }
  }

  public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;

  public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

  public override TypeConverter.StandardValuesCollection GetStandardValues(
    ITypeDescriptorContext context)
  {
    return new TypeConverter.StandardValuesCollection((ICollection) this._imKeysNamesDict?.Values ?? this.GetStandardValuesCustomList(context));
  }

  public virtual ICollection GetStandardValuesCustomList(
    ITypeDescriptorContext context,
    params object[] args)
  {
    return (ICollection) null;
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    if (value == null || !(destinationType == typeof (string)))
      return base.ConvertTo(context, culture, value, destinationType);
    string str;
    return !this._imKeysNamesDict.TryGetValue(value.ToString(), out str) ? (object) value.ToString() : (object) str;
  }

  public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
  {
    return destinationType == typeof (string) || base.CanConvertTo(context, destinationType);
  }

  public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
  {
    return sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);
  }

  public override object ConvertFrom(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value)
  {
    return !(value is string) ? (object) null : (object) this._imKeysNamesDict.FirstOrDefault<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (x => x.Value == value.ToString())).Key;
  }
}
