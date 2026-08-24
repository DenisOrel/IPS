// Decompiled with JetBrains decompiler
// Type: OxyPlot.ListFiller`1
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace OxyPlot;

public class ListFiller<T> where T : class, new()
{
  private readonly Dictionary<string, Action<T, object>> properties;

  public ListFiller() => this.properties = new Dictionary<string, Action<T, object>>();

  public void Add(string propertyName, Action<T, object> setter)
  {
    if (string.IsNullOrEmpty(propertyName))
      return;
    this.properties.Add(propertyName, setter);
  }

  public void FillT(IList<T> target, IEnumerable source) => this.Fill((IList) target, source);

  public void FillT(IList<T> target, IEnumerable source, Random rnd)
  {
    this.Fill((IList) target, source, rnd);
  }

  public void Fill(IList target, IEnumerable source, Random rand = null)
  {
    PropertyInfo[] propertyInfoArray = (PropertyInfo[]) null;
    Type type = (Type) null;
    foreach (object obj1 in source)
    {
      if (propertyInfoArray == null || obj1.GetType() != type)
      {
        type = obj1.GetType();
        propertyInfoArray = new PropertyInfo[this.properties.Count];
        int index = 0;
        foreach (KeyValuePair<string, Action<T, object>> property in this.properties)
        {
          if (string.IsNullOrEmpty(property.Key))
          {
            ++index;
          }
          else
          {
            propertyInfoArray[index] = type.GetRuntimeProperty(property.Key);
            if (propertyInfoArray[index] == (PropertyInfo) null)
              throw new InvalidOperationException($"Could not find field {property.Key} on type {type}");
            ++index;
          }
        }
      }
      T obj2 = new T();
      int index1 = 0;
      foreach (KeyValuePair<string, Action<T, object>> property in this.properties)
      {
        if (propertyInfoArray[index1] != (PropertyInfo) null)
        {
          object obj3 = propertyInfoArray[index1].GetValue(obj1, (object[]) null);
          property.Value(obj2, obj3);
        }
        ++index1;
      }
      target.Add((object) obj2);
    }
  }
}
