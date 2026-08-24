// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.DesignTime.CSharpLanguageExtension
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.CSharp.DesignTime;

internal sealed class CSharpLanguageExtension : ILanguageExtension
{
  private const string administrativeScriptTemplate = "using System;\r\nusing Intermech.Interfaces;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public void Execute(IUserSession scriptSession, string[] scriptArgs)\r\n    {\r\n        //Вставьте ваш код здесь\r\n    }\r\n}\r\n";
  private LanguageInfo languageInfo;
  private Func<CSharpSessionService> sessionServiceFactory;

  public CSharpLanguageExtension(Func<CSharpSessionService> sessionServiceFactory)
  {
    if (sessionServiceFactory == null)
      throw new ArgumentNullException(nameof (sessionServiceFactory));
    this.languageInfo = new LanguageInfo("C#", ".cs", false);
    this.sessionServiceFactory = sessionServiceFactory;
  }

  public LanguageInfo LanguageInfo
  {
    [DebuggerStepThrough] get => this.languageInfo;
  }

  public ILanguageSessionService CreateLanguageSessionService()
  {
    return (ILanguageSessionService) this.sessionServiceFactory();
  }

  public string CreateAdministrativeScriptTemplate()
  {
    return "using System;\r\nusing Intermech.Interfaces;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public void Execute(IUserSession scriptSession, string[] scriptArgs)\r\n    {\r\n        //Вставьте ваш код здесь\r\n    }\r\n}\r\n";
  }
}
