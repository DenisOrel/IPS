// Decompiled with JetBrains decompiler
// Type: OxyPlot.Series.OhlcvItem
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Series;

public class OhlcvItem
{
  public static readonly OhlcvItem Undefined = new OhlcvItem(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

  public OhlcvItem()
  {
  }

  public OhlcvItem(
    double x,
    double open,
    double high,
    double low,
    double close,
    double buyvolume = 0.0,
    double sellvolume = 0.0)
  {
    this.X = x;
    this.Open = open;
    this.High = high;
    this.Low = low;
    this.Close = close;
    this.BuyVolume = buyvolume;
    this.SellVolume = sellvolume;
  }

  public double X { get; set; }

  public double Open { get; set; }

  public double High { get; set; }

  public double Low { get; set; }

  public double Close { get; set; }

  public double BuyVolume { get; set; }

  public double SellVolume { get; set; }

  public static int FindIndex(List<OhlcvItem> items, double targetX, int guessIdx)
  {
    int index1 = 0;
    int index2 = 0;
    double x1;
    double num;
    for (int index3 = items.Count - 1; index2 <= index3 && guessIdx >= index2; guessIdx = index2 + (int) ((targetX - x1) * num))
    {
      if (guessIdx > index3)
        return index3;
      double x2 = items[guessIdx].X;
      if (x2.Equals(targetX))
        return guessIdx;
      if (x2 > targetX)
      {
        index3 = guessIdx - 1;
        if (index3 < index2)
          return index1;
        if (index3 == index2)
          return index3;
      }
      else
      {
        index2 = guessIdx + 1;
        index1 = guessIdx;
      }
      if (index2 >= index3)
        return index1;
      double x3 = items[index3].X;
      x1 = items[index2].X;
      num = (double) (index3 - index2 + 1) / (x3 - x1);
    }
    return index1;
  }

  public bool IsValid()
  {
    return !double.IsNaN(this.X) && !double.IsNaN(this.Open) && !double.IsNaN(this.High) && !double.IsNaN(this.Low) && !double.IsNaN(this.Close);
  }
}
