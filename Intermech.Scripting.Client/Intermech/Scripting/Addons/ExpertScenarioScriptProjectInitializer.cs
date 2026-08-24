// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Addons.ExpertScenarioScriptProjectInitializer
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Services;

#nullable disable
namespace Intermech.Scripting.Addons;

internal sealed class ExpertScenarioScriptProjectInitializer : DBScriptProjectInitializer
{
  private const string csharpTemplate = "using System;\r\nusing System.Collections.Generic;\r\nusing Intermech.Interfaces;\r\nusing Intermech.Interfaces.Expert;\r\nusing Intermech.Interfaces.Server;\r\nusing Intermech.Expert;\r\nusing Intermech.Expert.Server;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    /* Используются только первые два параметра - ti и context, и из контекста берется только первый объект */\r\n    public void Execute(ExpertServer.ExpServTask ti, long[] context, Intermech.Interfaces.Expert.HybridTableExp dTable, Int32 parm1, Int32 parm2, List<object> parmList)\r\n    {\r\n        //Вставьте ваш код здесь\r\n    }\r\n}\r\n";

  public ExpertScenarioScriptProjectInitializer()
    : base("using System;\r\nusing System.Collections.Generic;\r\nusing Intermech.Interfaces;\r\nusing Intermech.Interfaces.Expert;\r\nusing Intermech.Interfaces.Server;\r\nusing Intermech.Expert;\r\nusing Intermech.Expert.Server;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    /* Используются только первые два параметра - ti и context, и из контекста берется только первый объект */\r\n    public void Execute(ExpertServer.ExpServTask ti, long[] context, Intermech.Interfaces.Expert.HybridTableExp dTable, Int32 parm1, Int32 parm2, List<object> parmList)\r\n    {\r\n        //Вставьте ваш код здесь\r\n    }\r\n}\r\n")
  {
  }
}
