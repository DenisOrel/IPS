// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.LanguageDescriptor
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Базовый класс для описателей языков сценариев для IDE.
/// Реализация не является thread safe.
/// </summary>
public class LanguageDescriptor
{
  private LanguageInfo languageInfo;
  private LanguageServices services;

  public LanguageDescriptor(LanguageInfo languageInfo)
  {
    this.languageInfo = languageInfo != null ? languageInfo : throw new ArgumentNullException(nameof (languageInfo));
    this.services = new LanguageServices();
  }

  public LanguageInfo LanguageInfo
  {
    [DebuggerStepThrough] get => this.languageInfo;
  }

  public LanguageServices Services
  {
    [DebuggerStepThrough] get => this.services;
  }
}
