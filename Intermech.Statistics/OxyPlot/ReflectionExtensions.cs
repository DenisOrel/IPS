// Decompiled with JetBrains decompiler
// Type: OxyPlot.ReflectionExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Axes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace OxyPlot;

public static class ReflectionExtensions
{
  public static PropertyInfo GetRuntimeProperty(this Type type, string name)
  {
    return type.GetProperty(name);
  }

  public static void AddRange<T>(this List<T> target, IEnumerable source, string propertyName)
  {
    ReflectionPath reflectionPath = new ReflectionPath(propertyName);
    foreach (object instance in source)
    {
      T obj = (T) Convert.ChangeType(reflectionPath.GetValue(instance), typeof (T), (IFormatProvider) CultureInfo.InvariantCulture);
      target.Add(obj);
    }
  }

  public static void AddFormattedRange(
    this List<string> target,
    IEnumerable source,
    string propertyName,
    string formatString,
    IFormatProvider provider)
  {
    ReflectionPath reflectionPath = new ReflectionPath(propertyName);
    string format = $"{{0:{formatString}}}";
    foreach (object instance in source)
    {
      object obj = reflectionPath.GetValue(instance);
      string str = string.Format(provider, format, obj);
      target.Add(str);
    }
  }

  public static void AddRange(
    this List<DataPoint> target,
    IEnumerable itemsSource,
    string dataFieldX,
    string dataFieldY)
  {
    ReflectionPath reflectionPath1 = new ReflectionPath(dataFieldX);
    ReflectionPath reflectionPath2 = new ReflectionPath(dataFieldY);
    foreach (object instance in itemsSource)
      target.Add(new DataPoint(Axis.ToDouble(reflectionPath1.GetValue(instance)), Axis.ToDouble(reflectionPath2.GetValue(instance))));
  }
}
