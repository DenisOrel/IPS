// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.ObjectForAttributeFieldTypeService`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy;

internal class ObjectForAttributeFieldTypeService<T> where T : class
{
  private T[] _items;
  private readonly ObjectForAttributeFieldTypeFactory<T> _factory;

  private void InitializeItems()
  {
    int val1 = 0;
    foreach (int val2 in Enum.GetValues(typeof (FieldTypes)))
      val1 = Math.Max(val1, val2);
    this._items = new T[val1 + 1];
    foreach (FieldTypes attributeFieldType in Enum.GetValues(typeof (FieldTypes)))
    {
      try
      {
        this._items[(int) attributeFieldType] = this._factory.Create(attributeFieldType);
      }
      catch
      {
      }
    }
  }

  public ObjectForAttributeFieldTypeService(ObjectForAttributeFieldTypeFactory<T> factory)
  {
    this._factory = factory;
    this.InitializeItems();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public T GetObject(FieldTypes attributeFieldType, bool useUnknownInNotFound = false)
  {
    T obj = this._items[(int) attributeFieldType];
    return (object) obj != null || !useUnknownInNotFound ? obj : this._items[0];
  }
}
