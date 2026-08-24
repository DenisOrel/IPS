// Decompiled with JetBrains decompiler
// Type: OxyPlot.CodeGeneratorStringExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Globalization;

#nullable disable
namespace OxyPlot;

public static class CodeGeneratorStringExtensions
{
  public static string ToCode(this string value)
  {
    value = value.Replace("\"", "\\\"");
    value = value.Replace("\r\n", "\\n");
    value = value.Replace("\n", "\\n");
    value = value.Replace("\t", "\\t");
    return $"\"{value}\"";
  }

  public static string ToCode(this bool value) => value.ToString().ToLower();

  public static string ToCode(this int value)
  {
    return value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  public static string ToCode(this Enum value) => $"{value.GetType().Name}.{value}";

  public static string ToCode(this double value)
  {
    if (double.IsNaN(value))
      return "double.NaN";
    if (double.IsPositiveInfinity(value))
      return "double.PositiveInfinity";
    if (double.IsNegativeInfinity(value))
      return "double.NegativeInfinity";
    if (value.Equals(double.MinValue))
      return "double.MinValue";
    return value.Equals(double.MaxValue) ? "double.MaxValue" : value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  public static string ToCode(this object value)
  {
    switch (value)
    {
      case null:
        return "null";
      case int num1:
        return num1.ToCode();
      case double num2:
        return num2.ToCode();
      case string _:
        return CodeGeneratorStringExtensions.ToCode((string) value);
      case bool flag:
        return flag.ToCode();
      case Enum _:
        return CodeGeneratorStringExtensions.ToCode((Enum) value);
      case ICodeGenerating _:
        return ((ICodeGenerating) value).ToCode();
      default:
        return (string) null;
    }
  }
}
