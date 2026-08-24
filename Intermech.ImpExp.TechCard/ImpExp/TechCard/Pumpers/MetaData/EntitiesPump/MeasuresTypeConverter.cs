// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.MeasuresTypeConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Interfaces;
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

internal class MeasuresTypeConverter : TypeConverter
{
  public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
  {
    return destType == typeof (string) || base.CanConvertTo(context, destType);
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destType)
  {
    return base.ConvertTo(context, culture, value, destType);
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
    if (value == null)
      return base.ConvertFrom(context, culture, (object) null);
    if (!(value is string))
      return base.ConvertFrom(context, culture, value);
    long id = -1;
    if (context.Instance is EntityDescriptor instance)
      id = instance.Entity.Settings.MeasProdSettings.PhysicalValueId;
    return (object) EntityDescriptor.GetMeasureDescriptorsByPhisicalValueId(id).FirstOrDefault<MeasureDescriptor>((Func<MeasureDescriptor, bool>) (item => item.ToString() == value.ToString()));
  }

  public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;

  public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

  public override TypeConverter.StandardValuesCollection GetStandardValues(
    ITypeDescriptorContext context)
  {
    long id = -1;
    if (context.Instance is EntityDescriptor instance)
      id = instance.Entity.Settings.MeasProdSettings.PhysicalValueId;
    return new TypeConverter.StandardValuesCollection((ICollection) EntityDescriptor.GetMeasureDescriptorsByPhisicalValueId(id).ToArray());
  }
}
