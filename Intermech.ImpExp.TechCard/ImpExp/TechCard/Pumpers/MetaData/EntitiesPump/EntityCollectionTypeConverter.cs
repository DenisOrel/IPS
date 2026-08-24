// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntityCollectionTypeConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

internal class EntityCollectionTypeConverter : TypeConverter
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
    if (!(destType == typeof (string)))
      return base.ConvertTo(context, culture, value, destType);
    string str = "Не назначено";
    if (value is Entity entity)
      str = entity.Name;
    return (object) str;
  }
}
