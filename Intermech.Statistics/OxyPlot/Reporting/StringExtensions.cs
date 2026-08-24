// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.StringExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace OxyPlot.Reporting;

public static class StringExtensions
{
  public static string Repeat(this string source, int n)
  {
    StringBuilder stringBuilder = new StringBuilder(n * source.Length);
    for (int index = 0; index < n; ++index)
      stringBuilder.Append(source);
    return stringBuilder.ToString();
  }

  public static string[] SplitLines(this string s, int lineLength = 80 /*0x50*/)
  {
    List<string> stringList = new List<string>();
    int num = 0;
    while (num < s.Length)
    {
      int lineLength1 = StringExtensions.FindLineLength(s, num, lineLength);
      stringList.Add(lineLength1 == 0 ? s.Substring(num).Trim() : s.Substring(num, lineLength1).Trim());
      num += lineLength1;
      if (lineLength1 == 0)
        break;
    }
    return stringList.ToArray();
  }

  private static int FindLineLength(string text, int i, int maxLineLength)
  {
    int num = i + 1;
    int lineLength = 0;
    while (num < i + maxLineLength && num < text.Length)
    {
      num = text.IndexOfAny(" \n\r".ToCharArray(), num + 1);
      if (num == -1)
        num = text.Length;
      if (num - i < maxLineLength)
        lineLength = num - i;
    }
    return lineLength;
  }
}
