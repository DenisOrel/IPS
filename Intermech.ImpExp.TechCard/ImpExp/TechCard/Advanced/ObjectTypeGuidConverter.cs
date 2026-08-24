// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Advanced.ObjectTypeGuidConverter
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

internal class ObjectTypeGuidConverter : GuidConverter
{
  public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
  {
    return destType == typeof (string) || base.CanConvertTo(context, destType);
  }

  public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
  {
    return !(sourceType == typeof (string)) && base.CanConvertFrom(context, sourceType);
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    if (!(destinationType == typeof (string)) || !(value is Guid guid))
      return base.ConvertTo(context, culture, value, destinationType);
    string str1 = "Не назначен";
    if (guid == Guid.Empty)
      return (object) str1;
    IMetadataInfo service = (IMetadataInfo) ServicesManager.GetService(typeof (IMetadataInfo));
    string str2;
    if (service != null)
    {
      IObjectTypeItem byGuid = service.ObjectTypes.GetByGuid(guid);
      str2 = byGuid != null ? byGuid.Name : guid.ToString();
    }
    else
    {
      IMSObjectType objectType = MetaDataHelper.GetObjectType(guid);
      str2 = objectType != null ? objectType.ObjectTypeName : guid.ToString();
    }
    return (object) str2;
  }
}
