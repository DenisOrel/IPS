// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxyColorExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#nullable disable
namespace OxyPlot;

public static class OxyColorExtensions
{
  public static OxyColor ChangeIntensity(this OxyColor color, double factor)
  {
    double[] hsv = color.ToHsv();
    hsv[2] *= factor;
    if (hsv[2] > 1.0)
      hsv[2] = 1.0;
    return OxyColor.FromHsv(hsv);
  }

  public static OxyColor ChangeSaturation(this OxyColor color, double factor)
  {
    double[] hsv = color.ToHsv();
    hsv[1] *= factor;
    if (hsv[1] > 1.0)
      hsv[1] = 1.0;
    return OxyColor.FromHsv(hsv);
  }

  public static OxyColor Complementary(this OxyColor color)
  {
    double[] hsv = color.ToHsv();
    double hue = hsv[0] - 0.5;
    if (hue < 0.0)
      ++hue;
    return OxyColor.FromHsv(hue, hsv[1], hsv[2]);
  }

  public static double[] ToHsv(this OxyColor color)
  {
    byte r = color.R;
    byte g = color.G;
    byte b = color.B;
    byte num1 = Math.Min(Math.Min(r, g), b);
    byte num2 = Math.Max(Math.Max(r, g), b);
    double num3 = (double) ((int) num2 - (int) num1);
    double num4 = num2.Equals((byte) 0) ? 0.0 : num3 / (double) num2;
    double num5 = 0.0;
    double num6;
    if (num4.Equals(0.0))
    {
      num6 = 0.0;
    }
    else
    {
      if ((int) r == (int) num2)
        num5 = (double) ((int) g - (int) b) / num3;
      else if ((int) g == (int) num2)
        num5 = 2.0 + (double) ((int) b - (int) r) / num3;
      else if ((int) b == (int) num2)
        num5 = 4.0 + (double) ((int) r - (int) g) / num3;
      num6 = num5 * 60.0;
      if (num6 < 0.0)
        num6 += 360.0;
    }
    return new double[3]
    {
      num6 / 360.0,
      num4,
      (double) num2 / (double) byte.MaxValue
    };
  }

  [CLSCompliant(false)]
  public static uint ToUint(this OxyColor color)
  {
    return (uint) (((int) color.A << 24) + ((int) color.R << 16 /*0x10*/) + ((int) color.G << 8)) + (uint) color.B;
  }

  public static string ToByteString(this OxyColor color)
  {
    return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0},{1},{2},{3}", (object) color.A, (object) color.R, (object) color.G, (object) color.B);
  }

  public static string ToCode(this OxyColor color)
  {
    string colorName = color.GetColorName();
    if (colorName != null)
      return $"OxyColors.{colorName}";
    return $"OxyColor.FromArgb({color.A}, {color.R}, {color.G}, {color.B})";
  }

  public static string GetColorName(this OxyColor color)
  {
    FieldInfo fieldInfo = ((IEnumerable<FieldInfo>) typeof (OxyColors).GetFields(BindingFlags.Static | BindingFlags.Public)).FirstOrDefault<FieldInfo>((Func<FieldInfo, bool>) (field => color.Equals(field.GetValue((object) null))));
    return !(fieldInfo != (FieldInfo) null) ? (string) null : fieldInfo.Name;
  }
}
