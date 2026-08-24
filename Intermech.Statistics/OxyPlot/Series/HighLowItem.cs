// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.HighLowItem
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class HighLowItem : ICodeGenerating
{
  public static readonly HighLowItem Undefined = new HighLowItem(double.NaN, double.NaN, double.NaN);

  public HighLowItem()
  {
  }

  public HighLowItem(double x, double high, double low, double open = double.NaN, double close = double.NaN)
  {
    this.X = x;
    this.High = high;
    this.Low = low;
    this.Open = open;
    this.Close = close;
  }

  public double Close { get; set; }

  public double High { get; set; }

  public double Low { get; set; }

  public double Open { get; set; }

  public double X { get; set; }

  public static int FindIndex(List<HighLowItem> items, double targetX, int guess)
  {
    int index1 = 0;
    int index2 = 0;
    double x1;
    double num;
    for (int index3 = items.Count - 1; index2 <= index3 && guess >= index2; guess = index2 + (int) ((targetX - x1) * num))
    {
      if (guess > index3)
        return index3;
      double x2 = items[guess].X;
      if (x2.Equals(targetX))
        return guess;
      if (x2 > targetX)
      {
        index3 = guess - 1;
        if (index3 < index2)
          return index1;
        if (index3 == index2)
          return index3;
      }
      else
      {
        index2 = guess + 1;
        index1 = guess;
      }
      if (index2 >= index3)
        return index1;
      double x3 = items[index3].X;
      x1 = items[index2].X;
      num = (double) (index3 - index2 + 1) / (x3 - x1);
    }
    return index1;
  }

  public string ToCode()
  {
    return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2},{3},{4}", (object) this.X, (object) this.High, (object) this.Low, (object) this.Open, (object) this.Close);
  }
}
