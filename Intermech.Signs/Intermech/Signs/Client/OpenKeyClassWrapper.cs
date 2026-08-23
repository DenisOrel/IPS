// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.OpenKeyClassWrapper
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Signs.Interfaces;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Класс для отображения ключа в PropertyGrid</summary>
public class OpenKeyClassWrapper : ICustomTypeDescriptor
{
  private OpenKey[] _base;

  /// <summary>Конструктор</summary>
  /// <param name="baseClass">ключ в байтовом виде</param>
  public OpenKeyClassWrapper(OpenKey[] baseClass) => this._base = baseClass;

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public TypeConverter GetConverter() => TypeDescriptor.GetConverter(typeof (OpenKey));

  /// <summary>
  /// 
  /// </summary>
  /// <param name="attributes"></param>
  /// <returns></returns>
  public EventDescriptorCollection GetEvents(Attribute[] attributes)
  {
    return TypeDescriptor.GetEvents(typeof (OpenKey), attributes);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents(typeof (OpenKey));

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public string GetComponentName() => TypeDescriptor.GetComponentName((object) typeof (OpenKey));

  /// <summary>
  /// 
  /// </summary>
  /// <param name="pd"></param>
  /// <returns></returns>
  public object GetPropertyOwner(PropertyDescriptor pd) => (object) null;

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(typeof (OpenKey));

  /// <summary>
  /// 
  /// </summary>
  /// <param name="attributes"></param>
  /// <returns></returns>
  public PropertyDescriptorCollection GetProperties(Attribute[] attributes) => this.GetProperties();

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
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

  /// <summary>
  /// 
  /// </summary>
  /// <param name="editorBaseType"></param>
  /// <returns></returns>
  public object GetEditor(Type editorBaseType)
  {
    return TypeDescriptor.GetEditor(typeof (OpenKey), editorBaseType);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public PropertyDescriptor GetDefaultProperty()
  {
    return TypeDescriptor.GetDefaultProperty(typeof (OpenKey));
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(typeof (OpenKey));

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public string GetClassName() => TypeDescriptor.GetClassName(typeof (OpenKey));
}
