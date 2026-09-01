// Decompiled with JetBrains decompiler
// Type: Intermech.Text.MaskPatterns
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System.Text.RegularExpressions;

#nullable disable
namespace Intermech.Text;

internal static class MaskPatterns
{
  private static readonly string[] regexSpecialChars = new string[12]
  {
    "\\",
    ".",
    "$",
    "^",
    "{",
    "[",
    "(",
    "|",
    ")",
    "]",
    "}",
    "+"
  };
  private static readonly string[] regexSpecialEscapes = new string[12]
  {
    "\\\\",
    "\\.",
    "\\$",
    "\\^",
    "\\{",
    "\\[",
    "\\(",
    "\\|",
    "\\)",
    "\\]",
    "\\}",
    "\\+"
  };

  public static Regex ToRegex(string maskPattern)
  {
    if (maskPattern != string.Empty)
    {
      for (int index = 0; index < MaskPatterns.regexSpecialChars.Length; ++index)
        maskPattern = maskPattern.Replace(MaskPatterns.regexSpecialChars[index], MaskPatterns.regexSpecialEscapes[index]);
      maskPattern = maskPattern.Replace("*", ".*");
      maskPattern = maskPattern.Replace('?', '.');
    }
    return new Regex($"^{maskPattern}$", RegexOptions.Compiled | RegexOptions.Singleline);
  }
}
