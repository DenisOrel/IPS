// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Projects.DiskScripts.DiskScriptDialogService
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Collections;
using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Intermech.Scripting.Projects.DiskScripts;

internal abstract class DiskScriptDialogService
{
  internal string CalculateDialogFilters(ICollection<LanguageInfo> languageFilter)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (languageFilter != null && languageFilter.Count != 0)
    {
      if (languageFilter.Count > 1)
        stringBuilder.AppendFormat("Все сценарии|{0}|", (object) string.Join(";", (IEnumerable<string>) CollectionUtils.ConvertAsList<LanguageInfo, string>(languageFilter, (Converter<LanguageInfo, string>) (item => "*" + item.SourceExtension))));
      foreach (LanguageInfo languageInfo in (IEnumerable<LanguageInfo>) languageFilter)
        stringBuilder.AppendFormat($"Сценарии {languageInfo.Name}|*{languageInfo.SourceExtension}|");
    }
    stringBuilder.Append("Все файлы|*.*");
    return stringBuilder.ToString();
  }
}
