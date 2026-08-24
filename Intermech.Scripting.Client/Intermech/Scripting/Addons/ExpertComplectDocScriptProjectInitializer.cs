// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Addons.ExpertComplectDocScriptProjectInitializer
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Services;

#nullable disable
namespace Intermech.Scripting.Addons;

internal sealed class ExpertComplectDocScriptProjectInitializer : DBScriptProjectInitializer
{
  private const string csharpTemplate = "using System;\r\nusing Intermech.Interfaces;\r\nusing Intermech.Expert.Scenarios;\r\nusing Intermech.Interfaces.Document;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public bool Execute(IUserSession session, Int64 complectTemplateID, Int64[] objectIDs)\r\n    {\r\n        //Вставьте ваш код здесь\r\n\r\n        return true;\r\n    }\r\n}\r\n";

  public ExpertComplectDocScriptProjectInitializer()
    : base("using System;\r\nusing Intermech.Interfaces;\r\nusing Intermech.Expert.Scenarios;\r\nusing Intermech.Interfaces.Document;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public bool Execute(IUserSession session, Int64 complectTemplateID, Int64[] objectIDs)\r\n    {\r\n        //Вставьте ваш код здесь\r\n\r\n        return true;\r\n    }\r\n}\r\n")
  {
  }
}
