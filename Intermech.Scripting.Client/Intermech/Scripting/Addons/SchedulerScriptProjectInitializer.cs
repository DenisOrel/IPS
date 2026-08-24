// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Addons.SchedulerScriptProjectInitializer
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.Projects.DBScripts;
using Intermech.Scripting.Services;

#nullable disable
namespace Intermech.Scripting.Addons;

internal sealed class SchedulerScriptProjectInitializer : DBScriptProjectInitializer
{
  private const string csharpTemplate = "using System;\r\nusing System.ComponentModel.Design;\r\nusing Intermech.Interfaces;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public void Execute(IUserSession session, IServiceContainer services)\r\n    {\r\n        //Вставьте ваш код здесь\r\n    }\r\n}\r\n";

  public SchedulerScriptProjectInitializer()
    : base("using System;\r\nusing System.ComponentModel.Design;\r\nusing Intermech.Interfaces;\r\n\r\npublic class Script\r\n{\r\n    public ICSharpScriptContext ScriptContext { get; set; }\r\n\r\n    public void Execute(IUserSession session, IServiceContainer services)\r\n    {\r\n        //Вставьте ваш код здесь\r\n    }\r\n}\r\n")
  {
  }

  protected override void DoInitialize(DBScriptProject scriptProject)
  {
    base.DoInitialize(scriptProject);
    scriptProject.RunAtClientSide = false;
    scriptProject.Behaviors.AddDebugBehavior((IScriptDebugBehavior) new SchedulerScriptDebugBehavior(scriptProject));
  }
}
