// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Advanced.RelationTypeConverter
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

internal class RelationTypeConverter : GuidConverter
{
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
    if (!destinationType.Equals(typeof (string)) || !(value is Guid guid))
      return base.ConvertTo(context, culture, value, destinationType);
    string empty = string.Empty;
    if (guid == Guid.Empty)
      return (object) empty;
    IMetadataInfo service = (IMetadataInfo) ServicesManager.GetService(typeof (IMetadataInfo));
    if (service != null)
    {
      IRelationTypeItem byGuid = service.RelationTypes.GetByGuid(guid);
      return byGuid == null ? (object) value.ToString() : (object) byGuid.Name;
    }
    IMSRelationType relationType = MetaDataHelper.GetRelationType((Guid) value);
    return relationType == null ? (object) value.ToString() : (object) relationType.ShortName;
  }
}
