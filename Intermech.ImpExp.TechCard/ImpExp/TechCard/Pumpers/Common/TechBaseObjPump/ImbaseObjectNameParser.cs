// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ImbaseObjectNameParser
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

public static class ImbaseObjectNameParser
{
  public const string ImbaseObjNameSeparator = "&^";
  public const string ImbaseKeyPrefix = "i6";
  public const int ImbaseKeyLength = 20;

  public static (string ObjectName, string ImbaseKey) ParseCompositeObjName(
    string sourceImbaseObjName)
  {
    string str1 = string.Empty;
    string text = string.Empty;
    int length = sourceImbaseObjName.IndexOf("&^", StringComparison.Ordinal);
    if (length >= 0)
    {
      str1 = sourceImbaseObjName.Substring(0, length);
      text = sourceImbaseObjName.Substring(length + "&^".Length, sourceImbaseObjName.Length - (length + "&^".Length));
    }
    string str2 = !string.IsNullOrEmpty(str1) ? str1 : sourceImbaseObjName;
    if (text == string.Empty)
      return (str2, text);
    if ((text.IndexOf("i6", StringComparison.Ordinal) != 0 || text.Length != 20) && !GuidHelper.IsGuid(text))
      text = string.Empty;
    return (str2, text);
  }
}
