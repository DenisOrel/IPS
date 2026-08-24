// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Advanced.ObjectTypeIntConverter
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

internal class ObjectTypeIntConverter : Int64Converter
{
  protected string _emptyStrValue = "Любой тип объекта";

  public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
  {
    return destType == typeof (string) || base.CanConvertTo(context, destType);
  }

  public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
  {
    return !sourceType.Equals(typeof (string)) && base.CanConvertFrom(context, sourceType);
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    if (destinationType.Equals(typeof (string)))
    {
      switch (value)
      {
        case long _:
        case int _:
          int int32 = Convert.ToInt32(value);
          string str = this._emptyStrValue;
          if (int32 >= 0)
          {
            IMetadataInfo service = (IMetadataInfo) ServicesManager.GetService(typeof (IMetadataInfo));
            if (service != null)
            {
              IObjectTypeItem byId = service.ObjectTypes.GetByID(int32);
              str = byId != null ? byId.Name : $"ObjTypeID = {int32}";
            }
            else
            {
              IMSObjectType objectType = MetaDataHelper.GetObjectType(int32);
              str = objectType != null ? objectType.ObjectTypeName : $"ObjTypeID = {int32}";
            }
          }
          return (object) str;
      }
    }
    return base.ConvertTo(context, culture, value, destinationType);
  }
}
