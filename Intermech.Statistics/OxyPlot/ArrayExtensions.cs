// Decompiled with JetBrains decompiler
// Type: OxyPlot.ArrayExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public static class ArrayExtensions
{
  public static double Max2D(this double[,] array)
  {
    double minValue = double.MinValue;
    for (int index1 = 0; index1 < array.GetLength(0); ++index1)
    {
      for (int index2 = 0; index2 < array.GetLength(1); ++index2)
      {
        if (array[index1, index2].CompareTo(minValue) > 0)
          minValue = array[index1, index2];
      }
    }
    return minValue;
  }

  public static double Min2D(this double[,] array, bool excludeNaN = false)
  {
    double maxValue = double.MaxValue;
    for (int index1 = 0; index1 < array.GetLength(0); ++index1)
    {
      for (int index2 = 0; index2 < array.GetLength(1); ++index2)
      {
        if ((!excludeNaN || !double.IsNaN(array[index1, index2])) && array[index1, index2].CompareTo(maxValue) < 0)
          maxValue = array[index1, index2];
      }
    }
    return maxValue;
  }
}
