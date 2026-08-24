// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.Measure2ProductionListConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

public class Measure2ProductionListConverter : TypeConverter
{
  public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
  {
    return destType == typeof (string);
  }

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
    Dictionary<long, List<int>> dictionary = value as Dictionary<long, List<int>>;
    lPropertyDescriptor propertyDescriptor = context.PropertyDescriptor as lPropertyDescriptor;
    if (dictionary == null || propertyDescriptor == null)
      return base.ConvertTo(context, culture, value, destinationType);
    if (!(propertyDescriptor.OldValue is MeasureDescriptor oldValue))
      return base.ConvertTo(context, culture, value, destinationType);
    List<int> source;
    return !dictionary.TryGetValue(oldValue.MeasureID, out source) || !source.Any<int>() ? (object) string.Empty : (object) string.Join(",", source.Select<int, string>((Func<int, string>) (item => Measure2ProductionListConverter.GetProductionNameById(item))).ToArray<string>());
  }

  internal static string GetProductionNameById(int productionId)
  {
    string productionNameById = string.Empty;
    if (productionId == 0)
      return "Все";
    IpsProductionObj ipsProductionObj;
    if (TechPumpData.Production.Productions.TryGetValue(productionId, out ipsProductionObj))
      productionNameById = ipsProductionObj?.ProdInfo != null ? ipsProductionObj.ProdInfo.Name : productionNameById;
    return productionNameById;
  }
}
