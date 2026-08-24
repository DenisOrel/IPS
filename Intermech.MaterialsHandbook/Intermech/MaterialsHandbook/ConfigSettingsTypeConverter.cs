// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ConfigSettingsTypeConverter
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class ConfigSettingsTypeConverter : TypeConverter
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
    string str1 = string.Empty;
    string str2 = Convert.ToString(value);
    if (GuidHelper.IsGuid(str2))
    {
      Guid guid = new Guid(str2);
      if (guid != Guid.Empty)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(guid);
          if (!objectInfo.Empty)
          {
            str1 = objectInfo.Caption;
          }
          else
          {
            IDBAttributeType attributeType = sessionKeeper.Session.GetAttributeType(guid);
            if (attributeType != null)
              str1 = attributeType.Name;
          }
        }
      }
    }
    return (object) str1;
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
