// Decompiled with JetBrains decompiler
// Type: OxyPlot.LineStyleHelper
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public static class LineStyleHelper
{
  public static double[] GetDashArray(this LineStyle style)
  {
    switch (style)
    {
      case LineStyle.Solid:
        return (double[]) null;
      case LineStyle.Dash:
        return new double[2]{ 4.0, 1.0 };
      case LineStyle.Dot:
        return new double[2]{ 1.0, 1.0 };
      case LineStyle.DashDot:
        return new double[4]{ 4.0, 1.0, 1.0, 1.0 };
      case LineStyle.DashDashDot:
        return new double[6]{ 4.0, 1.0, 4.0, 1.0, 1.0, 1.0 };
      case LineStyle.DashDotDot:
        return new double[6]{ 4.0, 1.0, 1.0, 1.0, 1.0, 1.0 };
      case LineStyle.DashDashDotDot:
        return new double[8]
        {
          4.0,
          1.0,
          4.0,
          1.0,
          1.0,
          1.0,
          1.0,
          1.0
        };
      case LineStyle.LongDash:
        return new double[2]{ 10.0, 1.0 };
      case LineStyle.LongDashDot:
        return new double[4]{ 10.0, 1.0, 1.0, 1.0 };
      case LineStyle.LongDashDotDot:
        return new double[6]
        {
          10.0,
          1.0,
          1.0,
          1.0,
          1.0,
          1.0
        };
      default:
        return (double[]) null;
    }
  }
}
