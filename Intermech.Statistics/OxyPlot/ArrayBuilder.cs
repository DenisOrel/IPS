// Decompiled with JetBrains decompiler
// Type: OxyPlot.ArrayBuilder
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public static class ArrayBuilder
{
  public static double[] CreateVector(double x0, double x1, int n)
  {
    double[] vector = new double[n];
    for (int index = 0; index < n; ++index)
      vector[index] = Math.Round(x0 + (x1 - x0) * (double) index / (double) (n - 1), 8);
    return vector;
  }

  public static double[] CreateVector(double x0, double x1, double dx)
  {
    int num = (int) Math.Round((x1 - x0) / dx);
    double[] vector = new double[num + 1];
    for (int index = 0; index <= num; ++index)
      vector[index] = Math.Round(x0 + (double) index * dx, 8);
    return vector;
  }

  public static double[,] Evaluate(Func<double, double, double> f, double[] x, double[] y)
  {
    int length1 = x.Length;
    int length2 = y.Length;
    double[,] numArray = new double[length1, length2];
    for (int index1 = 0; index1 < length1; ++index1)
    {
      for (int index2 = 0; index2 < length2; ++index2)
        numArray[index1, index2] = f(x[index1], y[index2]);
    }
    return numArray;
  }

  public static void Fill(this double[] array, double value)
  {
    for (int index = 0; index < array.Length; ++index)
      array[index] = value;
  }

  public static void Fill2D(this double[,] array, double value)
  {
    for (int index1 = 0; index1 < array.GetLength(0); ++index1)
    {
      for (int index2 = 0; index2 < array.GetLength(1); ++index2)
        array[index1, index2] = value;
    }
  }
}
