// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Addons.TechAutoselectionScriptProjectInitializer
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Services;

#nullable disable
namespace Intermech.Scripting.Addons;

internal sealed class TechAutoselectionScriptProjectInitializer : DBScriptProjectInitializer
{
  private const string csharpTemplate = "using System;\r\nusing System.ComponentModel.Design;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing System.Windows.Forms;\r\nusing Intermech;\r\nusing Intermech.Interfaces;\r\nusing Intermech.Interfaces.AutoSelection;\r\nusing Intermech.AutoSelection.Client;\r\nusing Intermech.AutoSelection.Client.AutoSelectionService;\r\nusing Intermech.AutoSelection.Client.AutoSelectionNode;\r\nusing Intermech.AutoSelection.Client.AutoSelectionNodeSupport;\r\nusing Intermech.AutoSelection.Client.AutoSelectionLog;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public AutoSelExecuteStatus Execute(AutoSelectionSession asSession, IServiceContainer services)\r\n    {\r\n        var logRecord = services.GetService<AutoSelectionLogRec>(false);  // AutoSelection log record\r\n        var asNode = services.GetService<AutoSelectionNodeScript>(false);  // AutoSelection current nodes\r\n        var ownerNodeObjects = services.GetService<IEnumerable<AutoSelectionObject>>(false);  // Объекты, созданные родительским узлом дерева автоподбора\r\n\r\n        //Вставьте ваш код здесь\r\n\r\n        return AutoSelExecuteStatus.Applied;\r\n    }\r\n}\r\n";

  public TechAutoselectionScriptProjectInitializer()
    : base("using System;\r\nusing System.ComponentModel.Design;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing System.Windows.Forms;\r\nusing Intermech;\r\nusing Intermech.Interfaces;\r\nusing Intermech.Interfaces.AutoSelection;\r\nusing Intermech.AutoSelection.Client;\r\nusing Intermech.AutoSelection.Client.AutoSelectionService;\r\nusing Intermech.AutoSelection.Client.AutoSelectionNode;\r\nusing Intermech.AutoSelection.Client.AutoSelectionNodeSupport;\r\nusing Intermech.AutoSelection.Client.AutoSelectionLog;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public AutoSelExecuteStatus Execute(AutoSelectionSession asSession, IServiceContainer services)\r\n    {\r\n        var logRecord = services.GetService<AutoSelectionLogRec>(false);  // AutoSelection log record\r\n        var asNode = services.GetService<AutoSelectionNodeScript>(false);  // AutoSelection current nodes\r\n        var ownerNodeObjects = services.GetService<IEnumerable<AutoSelectionObject>>(false);  // Объекты, созданные родительским узлом дерева автоподбора\r\n\r\n        //Вставьте ваш код здесь\r\n\r\n        return AutoSelExecuteStatus.Applied;\r\n    }\r\n}\r\n")
  {
  }
}
