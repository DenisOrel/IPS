// Decompiled with JetBrains decompiler
// Type: OxyPlot.StringHelper
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Reflection;
using System.Text.RegularExpressions;

#nullable disable
namespace OxyPlot;

public static class StringHelper
{
  private static readonly Regex FormattingExpression = new Regex("{(?<Property>.+?)(?<Format>\\:.*?)?}");

  public static string Format(
    IFormatProvider provider,
    string formatString,
    object item,
    params object[] values)
  {
    string format = StringHelper.FormattingExpression.Replace(formatString, (MatchEvaluator) (match =>
    {
      string name = match.Groups["Property"].Value;
      if (name.Length > 0 && char.IsDigit(name[0]))
        return match.Value;
      PropertyInfo runtimeProperty = item.GetType().GetRuntimeProperty(name);
      if (runtimeProperty == (PropertyInfo) null)
        return string.Empty;
      object obj = runtimeProperty.GetValue(item, (object[]) null);
      return string.Format(provider, $"{{0{match.Groups[nameof (Format)].Value}}}", obj);
    }));
    return string.Format(provider, format, values);
  }
}
