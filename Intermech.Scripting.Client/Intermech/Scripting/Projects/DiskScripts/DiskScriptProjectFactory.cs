// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Projects.DiskScripts.DiskScriptProjectFactory
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Collections;
using Intermech.IO;
using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Projects.DiskScripts;

internal sealed class DiskScriptProjectFactory : IScriptProjectFactory
{
  private ICollection<LanguageInfo> supportedLanguages;

  public DiskScriptProjectFactory(ICollection<LanguageInfo> supportedLanguages)
  {
    this.supportedLanguages = supportedLanguages != null ? supportedLanguages : throw new ArgumentNullException(nameof (supportedLanguages));
  }

  public ScriptProject CreateEmptyProject(LanguageInfo languageInfo)
  {
    if (languageInfo == null)
      throw new ArgumentNullException(nameof (languageInfo));
    return CollectionUtils.Contains<LanguageInfo>((IEnumerable<LanguageInfo>) this.supportedLanguages, languageInfo) ? this.CreateEmptyProjectInternal(languageInfo) : throw new ScriptDesignTimeException($"Язык {languageInfo.Name} не поддерживается.");
  }

  public ScriptProject CreateEmptyProject(string fileExtension)
  {
    if (fileExtension == null)
      throw new ArgumentNullException(nameof (fileExtension));
    return this.CreateEmptyProjectInternal(CollectionUtils.Find<LanguageInfo>((IEnumerable<LanguageInfo>) this.supportedLanguages, (Predicate<LanguageInfo>) (x => PathUtils.IsSamePath(x.SourceExtension, fileExtension))) ?? throw new ScriptDesignTimeException($"Файлы сценариев {fileExtension} не поддерживаются."));
  }

  private ScriptProject CreateEmptyProjectInternal(LanguageInfo languageInfo)
  {
    return new ScriptProject(languageInfo);
  }
}
