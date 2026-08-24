// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Addons.TechNumerationScriptProjectInitializer
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Services;

#nullable disable
namespace Intermech.Scripting.Addons;

internal sealed class TechNumerationScriptProjectInitializer : DBScriptProjectInitializer
{
  private const string csharpTemplate = "using System;\r\nusing System.ComponentModel.Design;\r\nusing Intermech.Interfaces;\r\nusing Intermech.Interfaces.TechCard;\r\nusing Intermech.Interfaces.TechCard.TechNumeration;\r\nusing Intermech.TechCard.Server;\r\nusing Intermech.TechCard.Server.TechNumeration;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public void Execute(TechNumerationTask task, TechNumerationTaskParams taskParams)\r\n    {\r\n        //Вставьте ваш код здесь\r\n    }\r\n}\r\n";

  public TechNumerationScriptProjectInitializer()
    : base("using System;\r\nusing System.ComponentModel.Design;\r\nusing Intermech.Interfaces;\r\nusing Intermech.Interfaces.TechCard;\r\nusing Intermech.Interfaces.TechCard.TechNumeration;\r\nusing Intermech.TechCard.Server;\r\nusing Intermech.TechCard.Server.TechNumeration;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public void Execute(TechNumerationTask task, TechNumerationTaskParams taskParams)\r\n    {\r\n        //Вставьте ваш код здесь\r\n    }\r\n}\r\n")
  {
  }
}
