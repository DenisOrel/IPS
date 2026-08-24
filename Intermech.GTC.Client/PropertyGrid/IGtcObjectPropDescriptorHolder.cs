// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.PropertyGrid.IGtcObjectPropDescriptorHolder
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Interfaces;
using System.ComponentModel;

#nullable disable
namespace Intermech.GTC.Client.PropertyGrid;

public interface IGtcObjectPropDescriptorHolder
{
  PropertyDescriptorCollection ExtendPropDescriptorCollectionbyMode(
    object component,
    GetAttributeValuesModes avm,
    bool hideIfNotInMode);

  GetAttributeValuesModes AttributeValuesModes { get; }

  PropertyDescriptorCollection PropDescriptorCollection { get; }

  GtcPropertyGrid PropertyGrid { get; }
}
