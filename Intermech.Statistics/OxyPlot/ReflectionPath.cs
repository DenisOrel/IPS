// Decompiled with JetBrains decompiler
// Type: OxyPlot.ReflectionPath
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Reflection;

#nullable disable
namespace OxyPlot;

public class ReflectionPath
{
  private readonly string[] items;
  private readonly PropertyInfo[] infos;
  private readonly Type[] reflectedTypes;

  public ReflectionPath(string path)
  {
    this.items = path.Split('.');
    this.infos = new PropertyInfo[this.items.Length];
    this.reflectedTypes = new Type[this.items.Length];
  }

  public object GetValue(object instance)
  {
    object obj = instance;
    for (int index = 0; index < this.items.Length; ++index)
    {
      if (obj == null)
        return (object) null;
      Type type = obj.GetType();
      PropertyInfo propertyInfo = this.infos[index];
      if (propertyInfo == (PropertyInfo) null || this.reflectedTypes[index] != type)
      {
        propertyInfo = this.infos[index] = type.GetRuntimeProperty(this.items[index]);
        this.reflectedTypes[index] = type;
      }
      obj = !(propertyInfo == (PropertyInfo) null) ? propertyInfo.GetValue(obj, (object[]) null) : throw new InvalidOperationException($"Could not find property {this.items[index]} in {obj}");
    }
    return obj;
  }
}
