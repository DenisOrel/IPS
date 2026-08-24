// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.PropertyGrid.GtcObjectPropertyGridTab
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.PropertyEditors;
using System;
using System.ComponentModel;
using System.Windows.Forms.Design;

#nullable disable
namespace Intermech.GTC.Client.PropertyGrid;

public abstract class GtcObjectPropertyGridTab : PropertyTab, IObjectPropertyGridTab
{
  public abstract GetAttributeValuesModes TabAttributeValuesModes { get; }

  public virtual void InitTab(GetAttributeValuesModes avm)
  {
  }

  public abstract Guid TabGuid { get; }

  public PropertyDescriptorCollection PropDescriptorCollection(object component)
  {
    return component is IGtcObjectPropDescriptorHolder ? this.GetProperties(component) : (PropertyDescriptorCollection) null;
  }

  public override PropertyDescriptorCollection GetProperties(
    object component,
    Attribute[] attributes)
  {
    return this.GetProperties((ITypeDescriptorContext) null, component, attributes);
  }

  public override PropertyDescriptorCollection GetProperties(object component)
  {
    return this.GetProperties(component, (Attribute[]) null);
  }

  public override PropertyDescriptorCollection GetProperties(
    ITypeDescriptorContext context,
    object component,
    Attribute[] attributes)
  {
    PropertyDescriptorCollection properties = (PropertyDescriptorCollection) null;
    if (component is IGtcObjectPropDescriptorHolder)
      properties = ((IGtcObjectPropDescriptorHolder) component).ExtendPropDescriptorCollectionbyMode((object) this, this.TabAttributeValuesModes | ClientConsts.GetAttributeValuesModesMinimum, true);
    else if (context != null && context.PropertyDescriptor != null && context.PropertyDescriptor.Converter != null)
      properties = context.PropertyDescriptor.Converter.GetProperties(context, component, attributes);
    return properties;
  }

  public override bool CanExtend(object extendee) => base.CanExtend(extendee);
}
