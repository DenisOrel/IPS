// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.LanguageSessionData
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal sealed class LanguageSessionData
{
  private readonly string languageName;
  private readonly LanguageDescriptor languageDescriptor;
  private readonly ILanguageSessionParameters sessionParameters;
  private ILanguageSession session;

  public LanguageSessionData(
    LanguageDescriptor languageDescriptor,
    ILanguageSessionParameters languageSessionParameters)
  {
    this.languageName = languageDescriptor.LanguageInfo.Name;
    this.languageDescriptor = languageDescriptor;
    this.sessionParameters = languageSessionParameters;
  }

  public string LanguageName
  {
    [DebuggerStepThrough] get => this.languageName;
  }

  public LanguageDescriptor LanguageDescriptor
  {
    [DebuggerStepThrough] get => this.languageDescriptor;
  }

  public ILanguageSessionParameters SessionParameters
  {
    [DebuggerStepThrough] get => this.sessionParameters;
  }

  public ILanguageSession Session
  {
    [DebuggerStepThrough] get => this.session;
    [DebuggerStepThrough] set => this.session = value;
  }

  public void ShutdownSession()
  {
    if (this.session == null)
      return;
    this.session.Dispose();
    this.session = (ILanguageSession) null;
  }
}
