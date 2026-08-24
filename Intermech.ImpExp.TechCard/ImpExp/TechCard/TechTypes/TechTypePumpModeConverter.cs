// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechTypes.TechTypePumpModeConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechTypes;

internal class TechTypePumpModeConverter : TypeConverter
{
  public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;

  public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

  public override TypeConverter.StandardValuesCollection GetStandardValues(
    ITypeDescriptorContext context)
  {
    ArrayList values = new ArrayList((ICollection) Enum.GetValues(typeof (TechTypePumpMode)));
    for (int index = values.Count - 1; index >= 0; --index)
    {
      if ((TechTypePumpMode) values[index] == TechTypePumpMode.LockedType)
        values.RemoveAt(index);
    }
    return new TypeConverter.StandardValuesCollection((ICollection) values);
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
    return value is string s ? (object) (TechTypePumpMode) EnumTypeHelper.GetEnumValue(typeof (TechTypePumpMode), s) : base.ConvertFrom(context, culture, value);
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    return destinationType == typeof (string) && value is TechTypePumpMode techTypePumpMode ? (object) EnumTypeHelper.GetCaption((Enum) techTypePumpMode) : base.ConvertTo(context, culture, value, destinationType);
  }
}
