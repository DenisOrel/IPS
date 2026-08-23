// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.OpenKeyClassWrapper
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Signs.Interfaces;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.Signs.Client;

public class OpenKeyClassWrapper : ICustomTypeDescriptor
{
  private OpenKey[] _base;

  public OpenKeyClassWrapper(OpenKey[] baseClass) => this._base = baseClass;

  public TypeConverter GetConverter() => TypeDescriptor.GetConverter(typeof (OpenKey));

  public EventDescriptorCollection GetEvents(Attribute[] attributes)
  {
    return TypeDescriptor.GetEvents(typeof (OpenKey), attributes);
  }

  public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents(typeof (OpenKey));

  public string GetComponentName() => TypeDescriptor.GetComponentName((object) typeof (OpenKey));

  public object GetPropertyOwner(PropertyDescriptor pd) => (object) null;

  public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(typeof (OpenKey));

  public PropertyDescriptorCollection GetProperties(Attribute[] attributes) => this.GetProperties();

  public PropertyDescriptorCollection GetProperties()
  {
    PropertyDescriptorCollection properties = new PropertyDescriptorCollection((PropertyDescriptor[]) null);
    foreach (OpenKey openKey in this._base)
    {
      OpenKeyPropertyDescriptor propertyDescriptor = new OpenKeyPropertyDescriptor(openKey, TypeDescriptor.GetDefaultProperty((object) openKey));
      properties.Add((PropertyDescriptor) propertyDescriptor);
    }
    return properties;
  }

  public object GetEditor(Type editorBaseType)
  {
    return TypeDescriptor.GetEditor(typeof (OpenKey), editorBaseType);
  }

  public PropertyDescriptor GetDefaultProperty()
  {
    return TypeDescriptor.GetDefaultProperty(typeof (OpenKey));
  }

  public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(typeof (OpenKey));

  public string GetClassName() => TypeDescriptor.GetClassName(typeof (OpenKey));
}
