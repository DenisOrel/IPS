// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Addons.FormButtonScriptProjectInitializer
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Services;

#nullable disable
namespace Intermech.Scripting.Addons;

internal sealed class FormButtonScriptProjectInitializer : DBScriptProjectInitializer
{
  private const string csharpTemplate = "using System;\r\nusing System.ComponentModel.Design;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing System.Windows.Forms;\r\nusing Intermech.Interfaces;\r\nusing Intermech.Interfaces.Client;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public AttributeValidationScriptParameters Execute(AttributeValidationScriptParameters parameters)\r\n    {\r\n        //Вставьте ваш код здесь\r\n\r\n        return parameters;\r\n    }\r\n}\r\n";

  public FormButtonScriptProjectInitializer()
    : base("using System;\r\nusing System.ComponentModel.Design;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing System.Windows.Forms;\r\nusing Intermech.Interfaces;\r\nusing Intermech.Interfaces.Client;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public AttributeValidationScriptParameters Execute(AttributeValidationScriptParameters parameters)\r\n    {\r\n        //Вставьте ваш код здесь\r\n\r\n        return parameters;\r\n    }\r\n}\r\n")
  {
  }
}
