// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxyColor
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Globalization;

#nullable disable
namespace OxyPlot;

public struct OxyColor : ICodeGenerating, IEquatable<OxyColor>
{
  private readonly byte r;
  private readonly byte g;
  private readonly byte b;
  private readonly byte a;

  private OxyColor(byte a, byte r, byte g, byte b)
  {
    this.a = a;
    this.r = r;
    this.g = g;
    this.b = b;
  }

  public byte A => this.a;

  public byte B => this.b;

  public byte G => this.g;

  public byte R => this.r;

  public static OxyColor Parse(string value)
  {
    value = value.Trim();
    if (value.StartsWith("#"))
    {
      value = value.Trim('#');
      uint color = uint.Parse(value, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
      if (value.Length < 8)
        color += 4278190080U /*0xFF000000*/;
      return OxyColor.FromUInt32(color);
    }
    string[] strArray1 = value.Split(',');
    if (strArray1.Length < 3 || strArray1.Length > 4)
      throw new FormatException("Invalid format.");
    int num1 = 0;
    byte maxValue = byte.MaxValue;
    if (strArray1.Length > 3)
      maxValue = byte.Parse(strArray1[num1++], (IFormatProvider) CultureInfo.InvariantCulture);
    string[] strArray2 = strArray1;
    int index1 = num1;
    int num2 = index1 + 1;
    byte r = byte.Parse(strArray2[index1], (IFormatProvider) CultureInfo.InvariantCulture);
    string[] strArray3 = strArray1;
    int index2 = num2;
    int index3 = index2 + 1;
    byte g = byte.Parse(strArray3[index2], (IFormatProvider) CultureInfo.InvariantCulture);
    byte b = byte.Parse(strArray1[index3], (IFormatProvider) CultureInfo.InvariantCulture);
    return OxyColor.FromArgb(maxValue, r, g, b);
  }

  public static double ColorDifference(OxyColor c1, OxyColor c2)
  {
    double num1 = (double) ((int) c1.R - (int) c2.R) / (double) byte.MaxValue;
    double num2 = (double) ((int) c1.G - (int) c2.G) / (double) byte.MaxValue;
    double num3 = (double) ((int) c1.B - (int) c2.B) / (double) byte.MaxValue;
    double num4 = (double) ((int) c1.A - (int) c2.A) / (double) byte.MaxValue;
    return Math.Sqrt(num1 * num1 + num2 * num2 + num3 * num3 + num4 * num4);
  }

  [CLSCompliant(false)]
  public static OxyColor FromUInt32(uint color)
  {
    int a = (int) (byte) (color >> 24);
    byte num1 = (byte) (color >> 16 /*0x10*/);
    byte num2 = (byte) (color >> 8);
    byte num3 = (byte) color;
    int r = (int) num1;
    int g = (int) num2;
    int b = (int) num3;
    return OxyColor.FromArgb((byte) a, (byte) r, (byte) g, (byte) b);
  }

  public static OxyColor FromHsv(double[] hsv)
  {
    return hsv.Length == 3 ? OxyColor.FromHsv(hsv[0], hsv[1], hsv[2]) : throw new InvalidOperationException("Wrong length of hsv array.");
  }

  public static OxyColor FromHsv(double hue, double sat, double val)
  {
    double num1;
    double num2 = num1 = 0.0;
    double num3 = num1;
    double num4 = num1;
    if (sat.Equals(0.0))
    {
      double num5;
      num2 = num5 = val;
      num3 = num5;
      num4 = num5;
    }
    else
    {
      if (hue.Equals(1.0))
        hue = 0.0;
      hue *= 6.0;
      int num6 = (int) Math.Floor(hue);
      double num7 = hue - (double) num6;
      double num8 = val * (1.0 - sat);
      double num9 = val * (1.0 - sat * num7);
      double num10 = val * (1.0 - sat * (1.0 - num7));
      switch (num6)
      {
        case 0:
          num4 = val;
          num3 = num10;
          num2 = num8;
          break;
        case 1:
          num4 = num9;
          num3 = val;
          num2 = num8;
          break;
        case 2:
          num4 = num8;
          num3 = val;
          num2 = num10;
          break;
        case 3:
          num4 = num8;
          num3 = num9;
          num2 = val;
          break;
        case 4:
          num4 = num10;
          num3 = num8;
          num2 = val;
          break;
        case 5:
          num4 = val;
          num3 = num8;
          num2 = num9;
          break;
      }
    }
    return OxyColor.FromRgb((byte) (num4 * (double) byte.MaxValue), (byte) (num3 * (double) byte.MaxValue), (byte) (num2 * (double) byte.MaxValue));
  }

  public static double HueDifference(OxyColor c1, OxyColor c2)
  {
    double num = c1.ToHsv()[0] - c2.ToHsv()[0];
    if (num > 0.5)
      --num;
    if (num < -0.5)
      ++num;
    return Math.Sqrt(num * num);
  }

  public static OxyColor FromAColor(byte a, OxyColor color)
  {
    return OxyColor.FromArgb(a, color.R, color.G, color.B);
  }

  public static OxyColor FromArgb(byte a, byte r, byte g, byte b) => new OxyColor(a, r, g, b);

  public static OxyColor FromRgb(byte r, byte g, byte b) => new OxyColor(byte.MaxValue, r, g, b);

  public static OxyColor Interpolate(OxyColor color1, OxyColor color2, double t)
  {
    return OxyColor.FromArgb((byte) ((double) color1.A * (1.0 - t) + (double) color2.A * t), (byte) ((double) color1.R * (1.0 - t) + (double) color2.R * t), (byte) ((double) color1.G * (1.0 - t) + (double) color2.G * t), (byte) ((double) color1.B * (1.0 - t) + (double) color2.B * t));
  }

  public static bool operator ==(OxyColor first, OxyColor second) => first.Equals(second);

  public static bool operator !=(OxyColor first, OxyColor second) => !first.Equals(second);

  public override bool Equals(object obj)
  {
    return obj != null && !(obj.GetType() != typeof (OxyColor)) && this.Equals((OxyColor) obj);
  }

  public bool Equals(OxyColor other)
  {
    return (int) other.A == (int) this.A && (int) other.R == (int) this.R && (int) other.G == (int) this.G && (int) other.B == (int) this.B;
  }

  public override int GetHashCode()
  {
    byte num1 = this.A;
    int num2 = num1.GetHashCode() * 397;
    num1 = this.R;
    int hashCode1 = num1.GetHashCode();
    int num3 = (num2 ^ hashCode1) * 397;
    num1 = this.G;
    int hashCode2 = num1.GetHashCode();
    int num4 = (num3 ^ hashCode2) * 397;
    num1 = this.B;
    int hashCode3 = num1.GetHashCode();
    return num4 ^ hashCode3;
  }

  public override string ToString()
  {
    return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "#{0:x2}{1:x2}{2:x2}{3:x2}", (object) this.A, (object) this.R, (object) this.G, (object) this.B);
  }

  public bool IsInvisible() => this.A == (byte) 0;

  public bool IsVisible() => this.A > (byte) 0;

  public bool IsUndefined() => this.Equals(OxyColors.Undefined);

  public bool IsAutomatic() => this.Equals(OxyColors.Automatic);

  public OxyColor GetActualColor(OxyColor defaultColor)
  {
    return !this.IsAutomatic() ? this : defaultColor;
  }

  string ICodeGenerating.ToCode() => OxyColorExtensions.ToCode(this);
}
