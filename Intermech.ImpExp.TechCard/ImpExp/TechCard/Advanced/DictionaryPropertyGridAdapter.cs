// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Advanced.DictionaryPropertyGridAdapter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.TechCard.Advanced;

internal class DictionaryPropertyGridAdapter : ICustomTypeDescriptor
{
  private IDictionary _dictionary;

  public DictionaryPropertyGridAdapter(IDictionary d) => this._dictionary = d;

  public string GetComponentName() => TypeDescriptor.GetComponentName((object) this, true);

  public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent((object) this, true);

  public string GetClassName() => TypeDescriptor.GetClassName((object) this, true);

  public EventDescriptorCollection GetEvents(Attribute[] attributes)
  {
    return TypeDescriptor.GetEvents((object) this, attributes, true);
  }

  EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
  {
    return TypeDescriptor.GetEvents((object) this, true);
  }

  public TypeConverter GetConverter() => TypeDescriptor.GetConverter((object) this, true);

  public object GetPropertyOwner(PropertyDescriptor pd) => (object) this._dictionary;

  public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes((object) this, true);

  public object GetEditor(Type editorBaseType)
  {
    return TypeDescriptor.GetEditor((object) this, editorBaseType, true);
  }

  public PropertyDescriptor GetDefaultProperty() => (PropertyDescriptor) null;

  PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
  {
    return this.GetProperties(new Attribute[0]);
  }

  public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
  {
    ArrayList arrayList = new ArrayList();
    foreach (DictionaryEntry dictionaryEntry in this._dictionary)
      arrayList.Add((object) new DictionaryPropertyDescriptor(this._dictionary, dictionaryEntry.Key));
    return new PropertyDescriptorCollection((PropertyDescriptor[]) arrayList.ToArray(typeof (PropertyDescriptor)));
  }
}
