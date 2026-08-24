// Decompiled with JetBrains decompiler
// Type: OxyPlot.Arrays
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public static class Arrays
{
  public static T[] CopyOfRange<T>(T[] source, int from, int to)
  {
    T[] objArray = new T[to - from];
    for (int index = from; index < Math.Min(source.Length, to); ++index)
      objArray[index - from] = source[index];
    return objArray;
  }

  public static T[] CopyOf<T>(T[] source, int newLength)
  {
    T[] objArray = new T[newLength];
    for (int index = 0; index < Math.Min(source.Length, newLength); ++index)
      objArray[index] = source[index];
    return objArray;
  }

  public static void Fill<T>(T[] source, int i0, int i1, T v)
  {
    for (int index = i0; index < i1; ++index)
      source[index] = v;
  }
}
