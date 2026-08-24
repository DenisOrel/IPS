// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.SubordinateDescriber
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.PropertyEditors;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.Office.Client;

internal class SubordinateDescriber : IAttributePropertyDescriber
{
  [NotNull]
  public Type GetPropDescriptorType(int attributeId, FieldTypes baseType)
  {
    return typeof (ObjectPropertyClass);
  }

  [NotNull]
  public object GetPropDescriptorEditor(int attributeId)
  {
    return (object) new SubordinateAttEditor(attributeId);
  }

  [CanBeNull]
  public TypeConverter GetPropDescriptorConverter(int attributeId) => (TypeConverter) null;

  public bool GetPropDescriptorReadonly(int attributeId, bool baseReadonly) => baseReadonly;

  public bool GetPropDescriptorReset(int attributeId, bool baseReset) => true;

  public string GetPropDescriptorMask(int attributeId, string baseMask) => baseMask;

  [CanBeNull]
  public object GetPropDescriptorValue(
    IElementInfo elementInfo,
    int attributeId,
    [CanBeNull] object actualValue)
  {
    return !(actualValue?.GetType() == typeof (long)) ? (object) null : (object) new ObjectPropertyClass(Convert.ToInt64(actualValue));
  }

  [CanBeNull]
  public object GetAttributeValue(IElementInfo elementInfo, int attributeId, [CanBeNull] object propertyValue)
  {
    if (propertyValue != null && propertyValue != DBNull.Value)
    {
      switch (propertyValue)
      {
        case ObjectPropertyClass objectPropertyClass:
          return (object) objectPropertyClass.ObjectID;
        case long _:
          return propertyValue;
      }
    }
    return (object) null;
  }

  [CanBeNull]
  public TypeConverter GetConverter(int attributeId, object attributeProcessor)
  {
    return (TypeConverter) null;
  }
}
