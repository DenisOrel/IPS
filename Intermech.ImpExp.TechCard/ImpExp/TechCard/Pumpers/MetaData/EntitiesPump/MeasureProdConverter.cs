// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.MeasureProdConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

internal class MeasureProdConverter : TypeConverter
{
  public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
  {
    return destType == typeof (string);
  }

  public override object ConvertFrom(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value)
  {
    return value is string ? value : base.ConvertFrom(context, culture, value);
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    if (!(value is EntMeasureProdSetting measureProdSetting))
      return base.ConvertTo(context, culture, value, destinationType);
    string str = "Не настроено..";
    if (measureProdSetting.Measure2ProdList.Count > 0)
      str = "Список..";
    return (object) str;
  }
}
