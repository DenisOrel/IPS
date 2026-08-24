// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.PrivateRegNumberAttDescriber
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.PropertyEditors;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.Office.Client;

internal sealed class PrivateRegNumberAttDescriber : IAttributePropertyDescriber
{
  [NotNull]
  public Type GetPropDescriptorType(int attributeId, FieldTypes baseType)
  {
    return typeof (PrivateRegNumberValueAttrProxy);
  }

  [NotNull]
  public object GetPropDescriptorEditor(int attributeId) => (object) new PrivateRegNumberEditor();

  [CanBeNull]
  public TypeConverter GetPropDescriptorConverter(int attributeId) => (TypeConverter) null;

  public bool GetPropDescriptorReadonly(int attributeId, bool baseReadonly) => true;

  public bool GetPropDescriptorReset(int attributeId, bool baseReset) => baseReset;

  public string GetPropDescriptorMask(int attributeId, string baseMask) => baseMask;

  [NotNull]
  public object GetPropDescriptorValue(
    [NotNull] IElementInfo elementInfo,
    int attributeId,
    [CanBeNull] object actualValue)
  {
    return actualValue == DBNull.Value || actualValue == null ? (object) new PrivateRegNumberValueAttrProxy(string.Empty, elementInfo.ElementIdentifier) : (object) new PrivateRegNumberValueAttrProxy(Convert.ToString(actualValue), elementInfo.ElementIdentifier);
  }

  [CanBeNull]
  public object GetAttributeValue(IElementInfo elementInfo, int attributeId, [CanBeNull] object propertyValue)
  {
    PrivateRegNumberValueAttrProxy numberValueAttrProxy = (PrivateRegNumberValueAttrProxy) propertyValue;
    return numberValueAttrProxy == null ? (object) null : (object) numberValueAttrProxy.Value;
  }

  [CanBeNull]
  public TypeConverter GetConverter(int attributeId, object attributeProcessor)
  {
    return (TypeConverter) null;
  }
}
