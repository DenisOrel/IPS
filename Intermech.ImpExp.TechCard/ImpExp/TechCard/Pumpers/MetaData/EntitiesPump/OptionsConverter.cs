// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.OptionsConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

internal class OptionsConverter : TypeConverter
{
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
    switch (value)
    {
      case int _:
        Convert.ToInt32(value);
        return (object) new StringBuilder().ToString();
      case AttributeOptions _:
        string str = string.Empty;
        int int32 = Convert.ToInt32(value);
        Array values = Enum.GetValues(typeof (AttributeOptions));
        for (int index = 0; index < values.Length; ++index)
        {
          AttributeOptions attributeOptions = (AttributeOptions) values.GetValue(index);
          if (attributeOptions != AttributeOptions.None && (int32 | Convert.ToInt32((object) attributeOptions)) == int32)
            str += $"{EnumDescConverter.GetEnumDescription((Enum) attributeOptions)}; ";
        }
        if (int32 == Convert.ToInt32((object) AttributeOptions.None))
          str = EnumDescConverter.GetEnumDescription((Enum) AttributeOptions.None);
        return (object) str;
      default:
        return base.ConvertTo(context, culture, value, destinationType);
    }
  }
}
