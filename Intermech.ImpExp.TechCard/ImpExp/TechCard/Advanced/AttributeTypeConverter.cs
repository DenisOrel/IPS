// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Advanced.AttributeTypeConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Intermech.ImpExp.TechCard.Advanced;

internal class AttributeTypeConverter : TypeConverter
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
    if (!(destType == typeof (string)) || !(value is Guid guid1))
      return base.ConvertTo(context, culture, value, destType);
    string str1 = "Не назначен";
    if (guid1 == Guid.Empty)
      return (object) str1;
    IMetadataInfo service = (IMetadataInfo) ServicesManager.GetService(typeof (IMetadataInfo));
    Guid guid2 = (Guid) value;
    string str2;
    if (service != null)
    {
      IAttributeTypeItem byGuid = service.AttributeTypes.GetByGuid(guid2);
      str2 = byGuid != null ? byGuid.Name : value.ToString();
    }
    else
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType((Guid) value);
      str2 = attributeType != null ? attributeType.Name : value.ToString();
    }
    return (object) str2;
  }
}
