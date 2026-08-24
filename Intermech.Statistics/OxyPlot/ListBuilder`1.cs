// Decompiled with JetBrains decompiler
// Type: OxyPlot.ListBuilder`1
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace OxyPlot;

public class ListBuilder<T>
{
  private readonly List<string> properties;
  private readonly List<object> defaultValues;

  public ListBuilder()
  {
    this.properties = new List<string>();
    this.defaultValues = new List<object>();
  }

  public void Add<TProperty>(string propertyName, TProperty defaultValue)
  {
    this.properties.Add(propertyName);
    this.defaultValues.Add((object) defaultValue);
  }

  public void FillT(IList<T> target, IEnumerable source, Func<IList<object>, T> instanceCreator)
  {
    this.Fill((IList) target, source, (Func<IList<object>, object>) (args => (object) instanceCreator(args)));
  }

  public void Fill(IList target, IEnumerable source, Func<IList<object>, object> instanceCreator)
  {
    PropertyInfo[] propertyInfoArray = (PropertyInfo[]) null;
    Type type = (Type) null;
    foreach (object obj1 in source)
    {
      if (propertyInfoArray == null || obj1.GetType() != type)
      {
        type = obj1.GetType();
        propertyInfoArray = new PropertyInfo[this.properties.Count];
        for (int index = 0; index < this.properties.Count; ++index)
        {
          string property = this.properties[index];
          if (property == null)
          {
            propertyInfoArray[index] = (PropertyInfo) null;
          }
          else
          {
            propertyInfoArray[index] = type.GetRuntimeProperty(property);
            if (propertyInfoArray[index] == (PropertyInfo) null)
              throw new InvalidOperationException($"Could not find field {property} on type {type}");
          }
        }
      }
      List<object> objectList = new List<object>();
      for (int index = 0; index < propertyInfoArray.Length; ++index)
      {
        if (propertyInfoArray[index] != (PropertyInfo) null)
          objectList.Add(propertyInfoArray[index].GetValue(obj1, (object[]) null));
        else
          objectList.Add(this.defaultValues[index]);
      }
      object obj2 = instanceCreator((IList<object>) objectList);
      target.Add(obj2);
    }
  }
}
