// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.SearchScheme.RolePropertyDescriber
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.PropertyEditors;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.Pdm.Compositions.SearchScheme;

internal class RolePropertyDescriber : IAttributePropertyDescriber
{
  public TypeConverter GetPropDescriptorConverter(int attributeId) => (TypeConverter) null;

  public bool GetPropDescriptorReadonly(int attributeId, bool baseReadonly) => baseReadonly;

  public string GetPropDescriptorMask(int attributeId, string baseMask) => baseMask;

  public object GetAttributeValue(IElementInfo iElementInfo, int attributeId, object propertyValue)
  {
    return propertyValue == null ? (object) null : (object) ((RoleAttProxy) propertyValue).Guid;
  }

  public object GetPropDescriptorEditor(int attributeId) => (object) new RolePropertyEditor();

  public Type GetPropDescriptorType(int attributeId, FieldTypes baseType) => typeof (RoleAttProxy);

  public object GetPropDescriptorValue(
    IElementInfo iElementInfo,
    int attributeId,
    object actualValue)
  {
    return (object) new RoleAttProxy(actualValue == DBNull.Value || actualValue == null || Convert.ToString(actualValue) == string.Empty ? Guid.Empty : new Guid(Convert.ToString(actualValue)));
  }

  public bool GetPropDescriptorReset(int attributeId, bool baseReset) => true;

  public TypeConverter GetConverter(int attributeId, object attributeProcessor)
  {
    return (TypeConverter) null;
  }
}
