// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.YesNoConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Intermech.ImpExp.TechCard;

internal class YesNoConverter : BooleanConverter
{
  private static string Yes = "Да";
  private static string No = "Нет";

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    if (!(value is bool) || !destinationType.Equals(typeof (string)))
      return base.ConvertTo(context, culture, value, destinationType);
    return !(bool) value ? (object) YesNoConverter.No : (object) YesNoConverter.Yes;
  }

  public override object ConvertFrom(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value)
  {
    if (value is string str)
    {
      if (str.Equals(YesNoConverter.Yes))
        return (object) true;
      if (str.Equals(YesNoConverter.No))
        return (object) false;
    }
    return base.ConvertFrom(context, culture, value);
  }
}
