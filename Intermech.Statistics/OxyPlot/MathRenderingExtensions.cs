// Decompiled with JetBrains decompiler
// Type: OxyPlot.MathRenderingExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public static class MathRenderingExtensions
{
  static MathRenderingExtensions()
  {
    MathRenderingExtensions.SubAlignment = 0.6;
    MathRenderingExtensions.SuperAlignment = 0.0;
    MathRenderingExtensions.SubSize = 0.62;
    MathRenderingExtensions.SuperSize = 0.62;
  }

  private static double SubAlignment { get; set; }

  private static double SubSize { get; set; }

  private static double SuperAlignment { get; set; }

  private static double SuperSize { get; set; }

  public static OxySize DrawMathText(
    this IRenderContext rc,
    ScreenPoint pt,
    string text,
    OxyColor textColor,
    string fontFamily,
    double fontSize,
    double fontWeight,
    double angle,
    HorizontalAlignment ha,
    VerticalAlignment va,
    OxySize? maxSize,
    bool measure)
  {
    if (string.IsNullOrEmpty(text))
      return OxySize.Empty;
    if (text.Contains("^{") || text.Contains("_{"))
    {
      double x = pt.X;
      double y = pt.Y;
      OxySize oxySize = MathRenderingExtensions.InternalDrawMathText(rc, x, y, 0.0, 0.0, text, textColor, fontFamily, fontSize, fontWeight, true, angle);
      double dx = 0.0;
      double dy = 0.0;
      switch (ha)
      {
        case HorizontalAlignment.Center:
          dx = -oxySize.Width * 0.5;
          break;
        case HorizontalAlignment.Right:
          dx = -oxySize.Width;
          break;
      }
      switch (va)
      {
        case VerticalAlignment.Middle:
          dy = -oxySize.Height * 0.5;
          break;
        case VerticalAlignment.Bottom:
          dy = -oxySize.Height;
          break;
      }
      MathRenderingExtensions.InternalDrawMathText(rc, x, y, dx, dy, text, textColor, fontFamily, fontSize, fontWeight, false, angle);
      return !measure ? OxySize.Empty : oxySize;
    }
    rc.DrawText(pt, text, textColor, fontFamily, fontSize, fontWeight, angle, ha, va, maxSize);
    return measure ? rc.MeasureText(text, fontFamily, fontSize, fontWeight) : OxySize.Empty;
  }

  public static void DrawMathText(
    this IRenderContext rc,
    ScreenPoint pt,
    string text,
    OxyColor textColor,
    string fontFamily,
    double fontSize,
    double fontWeight,
    double angle,
    HorizontalAlignment ha,
    VerticalAlignment va,
    OxySize? maxSize = null)
  {
    rc.DrawMathText(pt, text, textColor, fontFamily, fontSize, fontWeight, angle, ha, va, maxSize, false);
  }

  public static OxySize MeasureMathText(
    this IRenderContext rc,
    string text,
    string fontFamily,
    double fontSize,
    double fontWeight)
  {
    return text.Contains("^{") || text.Contains("_{") ? MathRenderingExtensions.InternalDrawMathText(rc, 0.0, 0.0, 0.0, 0.0, text, OxyColors.Black, fontFamily, fontSize, fontWeight, true, 0.0) : rc.MeasureText(text, fontFamily, fontSize, fontWeight);
  }

  private static OxySize InternalDrawMathText(
    IRenderContext rc,
    double x,
    double y,
    double dx,
    double dy,
    string s,
    OxyColor textColor,
    string fontFamily,
    double fontSize,
    double fontWeight,
    bool measureOnly,
    double angle)
  {
    int num1 = 0;
    double num2 = angle * Math.PI / 180.0;
    double cosAngle = Math.Round(Math.Cos(num2), 5);
    double sinAngle = Math.Round(Math.Sin(num2), 5);
    double num3 = x;
    double val2_1 = x;
    double val2_2 = x;
    double val1_1 = y;
    double val2_3 = y;
    double val2_4 = y;
    double num4 = fontSize * MathRenderingExtensions.SuperAlignment;
    double num5 = fontSize * MathRenderingExtensions.SubAlignment;
    double num6 = fontSize * MathRenderingExtensions.SuperSize;
    double num7 = fontSize * MathRenderingExtensions.SubSize;
    Func<double, double, string, double, OxySize> func = (Func<double, double, string, double, OxySize>) ((xb, yb, text, fSize) =>
    {
      if (!measureOnly)
        rc.DrawText(new ScreenPoint(x + (xb - x + dx) * cosAngle - (yb - y + dy) * sinAngle, y + (xb - x + dx) * sinAngle + (yb - y + dy) * cosAngle), text, textColor, fontFamily, fSize, fontWeight, angle);
      OxySize oxySize = rc.MeasureText(text, fontFamily, fSize, fontWeight);
      return new OxySize(oxySize.Width, oxySize.Height);
    });
    while (num1 < s.Length)
    {
      if (num1 + 1 < s.Length && s[num1] == '^' && s[num1 + 1] == '{')
      {
        int num8 = s.IndexOf('}', num1);
        if (num8 != -1)
        {
          string str = s.Substring(num1 + 2, num8 - num1 - 2);
          num1 = num8 + 1;
          double val1_2 = num3;
          double val1_3 = val1_1 + num4;
          OxySize oxySize = func(val1_2, val1_3, str, num6);
          val2_1 = Math.Max(val1_2 + oxySize.Width, val2_1);
          val2_3 = Math.Max(val1_3 + oxySize.Height, val2_3);
          val2_2 = Math.Min(val1_2, val2_2);
          val2_4 = Math.Min(val1_3, val2_4);
          continue;
        }
      }
      if (num1 + 1 < s.Length && s[num1] == '_' && s[num1 + 1] == '{')
      {
        int num9 = s.IndexOf('}', num1);
        if (num9 != -1)
        {
          string str = s.Substring(num1 + 2, num9 - num1 - 2);
          num1 = num9 + 1;
          double val1_4 = num3;
          double val1_5 = val1_1 + num5;
          OxySize oxySize = func(val1_4, val1_5, str, num7);
          val2_1 = Math.Max(val1_4 + oxySize.Width, val2_1);
          val2_3 = Math.Max(val1_5 + oxySize.Height, val2_3);
          val2_2 = Math.Min(val1_4, val2_2);
          val2_4 = Math.Min(val1_5, val2_4);
          continue;
        }
      }
      int num10 = s.IndexOfAny("^_".ToCharArray(), num1 + 1);
      string str1;
      if (num10 == -1)
      {
        str1 = s.Substring(num1);
        num1 = s.Length;
      }
      else
      {
        str1 = s.Substring(num1, num10 - num1);
        num1 = num10;
      }
      double val1_6 = val2_1 + 2.0;
      OxySize oxySize1 = func(val1_6, val1_1, str1, fontSize);
      val2_1 = Math.Max(val1_6 + oxySize1.Width, val2_1);
      val2_3 = Math.Max(val1_1 + oxySize1.Height, val2_3);
      val2_2 = Math.Min(val1_6, val2_2);
      val2_4 = Math.Min(val1_1, val2_4);
      num3 = val2_1;
    }
    return new OxySize(val2_1 - val2_2, val2_3 - val2_4);
  }
}
