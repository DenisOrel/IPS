// Decompiled with JetBrains decompiler
// Type: OxyPlot.FractionHelper
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Globalization;

#nullable disable
namespace OxyPlot;

public static class FractionHelper
{
  public static string ConvertToFractionString(
    double value,
    double unit = 1.0,
    string unitSymbol = null,
    double eps = 1E-06,
    IFormatProvider formatProvider = null,
    string formatString = null)
  {
    if (Math.Abs(value) < eps)
      return "0";
    value /= unit;
    for (int index = 1; index <= 64 /*0x40*/; ++index)
    {
      double a = value * (double) index;
      int num = (int) Math.Round(a);
      if (Math.Abs(a - (double) num) < eps)
      {
        string str = unitSymbol == null || num != 1 ? num.ToString((IFormatProvider) CultureInfo.InvariantCulture) : string.Empty;
        return index == 1 ? $"{str}{unitSymbol}" : $"{str}{unitSymbol}/{index}";
      }
    }
    string format = string.IsNullOrEmpty(formatString) ? "{0}{1}" : $"{{0:{formatString}}}{{1}}";
    return string.Format(formatProvider ?? (IFormatProvider) CultureInfo.CurrentCulture, format, (object) value, (object) unitSymbol);
  }
}
