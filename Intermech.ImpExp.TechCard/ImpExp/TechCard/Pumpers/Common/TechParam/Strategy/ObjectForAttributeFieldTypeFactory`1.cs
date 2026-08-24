// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.ObjectForAttributeFieldTypeFactory`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy;

internal class ObjectForAttributeFieldTypeFactory<T> where T : class
{
  private readonly IDictionary<FieldTypes, Type> _typeCache = (IDictionary<FieldTypes, Type>) new ConcurrentDictionary<FieldTypes, Type>();

  private Type GetTypeForFieldType(FieldTypes attributeFieldType)
  {
    Type typeForFieldType;
    if (this._typeCache.TryGetValue(attributeFieldType, out typeForFieldType))
      return typeForFieldType;
    foreach (Type element in ((IEnumerable<Type>) Assembly.GetAssembly(typeof (T)).GetTypes()).Where<Type>((Func<Type, bool>) (myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof (T)))))
    {
      if (element.GetCustomAttributes(typeof (AttributeFieldTypeAttribute)).FirstOrDefault<Attribute>() is AttributeFieldTypeAttribute fieldTypeAttribute && ((IEnumerable<FieldTypes>) fieldTypeAttribute.AttributeFieldTypes).Contains<FieldTypes>(attributeFieldType))
      {
        typeForFieldType = element;
        break;
      }
    }
    return this._typeCache[attributeFieldType] = typeForFieldType;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public T Create(FieldTypes attributeFieldType)
  {
    Type typeForFieldType = this.GetTypeForFieldType(attributeFieldType);
    return typeForFieldType != (Type) null ? (T) Activator.CreateInstance(typeForFieldType) : default (T);
  }
}
