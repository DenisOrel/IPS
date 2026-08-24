// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.MultiValuesAttributeConverter
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class MultiValuesAttributeConverter : DropDownTypeConverter
{
  private Dictionary<object, object> _attrValuesDict;

  public MultiValuesAttributeConverter(Guid attributeTypeGuid)
  {
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attributeTypeGuid);
    if (attributeType == null)
      return;
    this._attrValuesDict = new Dictionary<object, object>();
    List<object> possibleValues = attributeType.PossibleValues;
    List<object> valuesDescriptions = attributeType.PossibleValuesDescriptions;
    for (int index = 0; index < possibleValues.Count; ++index)
      this._attrValuesDict.Add(possibleValues[index], valuesDescriptions[index]);
  }

  public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;

  public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

  public override TypeConverter.StandardValuesCollection GetStandardValues(
    ITypeDescriptorContext context)
  {
    return new TypeConverter.StandardValuesCollection((ICollection) this._attrValuesDict?.Values ?? this.GetStandardValuesCustomList(context, Array.Empty<object>()));
  }

  public virtual ICollection GetStandardValuesCustomList(
    ITypeDescriptorContext context,
    params object[] args)
  {
    return (ICollection) null;
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    if (this._attrValuesDict == null || value == null)
      return base.ConvertTo(context, culture, value, destinationType);
    object obj;
    return !this._attrValuesDict.TryGetValue(value, out obj) ? value : obj;
  }

  public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
  {
    return destinationType == typeof (string) || base.CanConvertTo(context, destinationType);
  }

  public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
  {
    return sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);
  }

  public override object ConvertFrom(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value)
  {
    return !(value is string) ? base.ConvertFrom(context, culture, value) : this._attrValuesDict.FirstOrDefault<KeyValuePair<object, object>>((Func<KeyValuePair<object, object>, bool>) (x => x.Value == value)).Key;
  }
}
