// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Addons.ReportCompareObjectsScriptProjectInitializer
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Services;

#nullable disable
namespace Intermech.Scripting.Addons;

internal sealed class ReportCompareObjectsScriptProjectInitializer : DBScriptProjectInitializer
{
  private const string csharpTemplate = "using System;\r\nusing Intermech.Interfaces;\r\nusing Intermech.Interfaces.Pdm;\r\nusing Intermech.Interfaces.Document;\r\n\r\nclass Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public void Execute(IUserSession session, ImDocumentData document, CompositionItem leftCompositionTree, CompositionItem rightCompositionTree)\r\n    {\r\n        // TODO\r\n    }\r\n}\r\n";

  public ReportCompareObjectsScriptProjectInitializer()
    : base("using System;\r\nusing Intermech.Interfaces;\r\nusing Intermech.Interfaces.Pdm;\r\nusing Intermech.Interfaces.Document;\r\n\r\nclass Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public void Execute(IUserSession session, ImDocumentData document, CompositionItem leftCompositionTree, CompositionItem rightCompositionTree)\r\n    {\r\n        // TODO\r\n    }\r\n}\r\n")
  {
  }
}
