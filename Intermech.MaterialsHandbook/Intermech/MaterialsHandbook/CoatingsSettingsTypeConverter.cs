// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.CoatingsSettingsTypeConverter
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Interfaces.MaterialsHandbook;
using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class CoatingsSettingsTypeConverter : TypeConverter
{
  public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
  {
    return base.CanConvertTo(context, destinationType);
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    return !(value is IMHCoatingsSystemSettings coatingsSystemSettings) ? (object) string.Empty : (object) coatingsSystemSettings.Formula;
  }

  public override PropertyDescriptorCollection GetProperties(
    ITypeDescriptorContext context,
    object value,
    Attribute[] attributes)
  {
    return !(context.PropertyDescriptor is ConfigSettingsPropertyDescriptor propertyDescriptor) ? (PropertyDescriptorCollection) null : propertyDescriptor.ChildProperties;
  }

  public override bool GetPropertiesSupported(ITypeDescriptorContext context)
  {
    return !(context.PropertyDescriptor is ConfigSettingsPropertyDescriptor propertyDescriptor) ? base.GetPropertiesSupported(context) : propertyDescriptor.PropertiesSupported;
  }
}
