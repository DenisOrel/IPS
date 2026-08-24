// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.SitesListConverter
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Intermech.Site.Client;

public class SitesListConverter : TypeConverter
{
  public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
  {
    return destinationType == typeof (string) || base.CanConvertTo(context, destinationType);
  }

  public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
  {
    return sourceType == typeof (List<long>) || base.CanConvertFrom(context, sourceType);
  }

  public override object ConvertFrom(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value)
  {
    return value.GetType() == typeof (List<long>) ? (object) new SitesListPropertyClass((List<long>) value) : base.ConvertFrom(context, culture, value);
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    if (!(value is SitesListPropertyClass))
      throw new ArgumentException();
    return destinationType == typeof (string) ? (object) ((SitesListPropertyClass) value).ToString() : base.ConvertTo(context, culture, value, destinationType);
  }
}
