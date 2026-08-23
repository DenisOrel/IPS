// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignControlPropertyTypeConverter
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Localization;
using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Intermech.Signs.Client;

internal class SignControlPropertyTypeConverter : TypeConverter
{
  public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
  {
    return destinationType.Equals(typeof (string)) || base.CanConvertTo(context, destinationType);
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    if (!destinationType.Equals(typeof (string)) || !(value is SignControlPropertyClass))
      return base.ConvertTo(context, culture, value, destinationType);
    return (value as SignControlPropertyClass).isFilledOk ? (object) LocalizationHolder.rm.GetString("Signs_23") : (object) LocalizationHolder.rm.GetString("Signs_24");
  }
}
