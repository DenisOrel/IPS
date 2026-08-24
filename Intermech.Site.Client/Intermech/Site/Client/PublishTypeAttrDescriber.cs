// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PublishTypeAttrDescriber
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.PropertyEditors;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.Site.Client;

internal class PublishTypeAttrDescriber : IAttributePropertyDescriber
{
  public TypeConverter GetPropDescriptorConverter(int attributeId) => (TypeConverter) null;

  public bool GetPropDescriptorReadonly(int attributeId, bool baseReadonly) => baseReadonly;

  public string GetPropDescriptorMask(int attributeId, string baseMask) => baseMask;

  public object GetAttributeValue(IElementInfo iElementInfo, int attributeId, object propertyValue)
  {
    return propertyValue == null ? (object) null : (object) ((PublishTypeAttProxy) propertyValue).Guid;
  }

  public object GetPropDescriptorEditor(int attributeId) => (object) new PublishTypeAttEditor();

  public Type GetPropDescriptorType(int attributeId, FieldTypes baseType)
  {
    return typeof (PublishTypeAttProxy);
  }

  public object GetPropDescriptorValue(
    IElementInfo iElementInfo,
    int attributeId,
    object actualValue)
  {
    if (!GuidHelper.IsGuid(Convert.ToString(actualValue)))
      return (object) new PublishTypeAttProxy(-1, Guid.Empty, string.Empty);
    try
    {
      PortalObjectType publishObjectType = ((IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata))).GetPublishObjectType(new Guid(Convert.ToString(actualValue)));
      if (publishObjectType != null)
        return (object) new PublishTypeAttProxy(publishObjectType.ID, new Guid(publishObjectType.GUID), publishObjectType.Name);
    }
    catch
    {
    }
    string str = Convert.ToString(actualValue);
    return (object) new PublishTypeAttProxy(-1, new Guid(str), str);
  }

  public bool GetPropDescriptorReset(int attributeId, bool baseReset) => true;

  public TypeConverter GetConverter(int attributeId, object attributeProcessor)
  {
    return (TypeConverter) null;
  }
}
