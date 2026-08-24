// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.PropertyGrid.AttrsRelationshipDescriber
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.PropertyEditors;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.GTC.Client.PropertyGrid;

public class AttrsRelationshipDescriber : IAttributePropertyDescriber
{
  public Type GetPropDescriptorType(int attributeId, FieldTypes baseType)
  {
    return typeof (AttrsRelationshipPropertyClass);
  }

  public object GetPropDescriptorEditor(int attributeId) => (object) new AttrsRelationshipEditor();

  public TypeConverter GetPropDescriptorConverter(int attributeId)
  {
    return (TypeConverter) new AttrsRelationshipTypeConverter();
  }

  public TypeConverter GetConverter(int attributeId, object attributeProcessor)
  {
    return (TypeConverter) null;
  }

  public bool GetPropDescriptorReadonly(int attributeId, bool baseReadonly) => baseReadonly;

  public bool GetPropDescriptorReset(int attributeId, bool baseReset) => true;

  public string GetPropDescriptorMask(int attributeId, string baseMask) => baseMask;

  public object GetPropDescriptorValue(
    IElementInfo elementInfo,
    int attributeId,
    object actualValue)
  {
    if (elementInfo.ElementKind != AttributableElements.Object)
      return (object) null;
    if (!(actualValue is string stringValue))
      return (object) new AttrsRelationshipPropertyClass(elementInfo.ElementIdentifier);
    AttrsRelationshipPropertyClass propDescriptorValue = new AttrsRelationshipPropertyClass(stringValue);
    if (propDescriptorValue.ObjectId.Equals(0L))
      propDescriptorValue.ObjectId = elementInfo.ElementIdentifier;
    return (object) propDescriptorValue;
  }

  public object GetAttributeValue(IElementInfo elementInfo, int attributeId, object propertyValue)
  {
    return propertyValue is AttrsRelationshipPropertyClass relationshipPropertyClass ? (object) relationshipPropertyClass.ToString() : (object) new AttrsRelationshipPropertyClass().ToString();
  }
}
