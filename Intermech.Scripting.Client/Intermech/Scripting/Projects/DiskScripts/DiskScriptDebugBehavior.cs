// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Projects.DiskScripts.DiskScriptDebugBehavior
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Interfaces;
using Intermech.Mvp;
using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.Utils;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Projects.DiskScripts;

internal sealed class DiskScriptDebugBehavior : IScriptDebugBehavior
{
  private ScriptProject scriptProject;
  private List<string> scriptArguments;

  public DiskScriptDebugBehavior(ScriptProject scriptProject)
  {
    this.scriptProject = scriptProject != null ? scriptProject : throw new ArgumentNullException(nameof (scriptProject));
    this.scriptArguments = new List<string>();
  }

  public void EditArguments()
  {
    DebugParametersPresenter parametersPresenter = new DebugParametersPresenter();
    parametersPresenter.ScriptArguments = this.scriptArguments;
    MvpContext.ViewService.ShowModal((IPresenter) parametersPresenter);
    this.scriptArguments = parametersPresenter.ScriptArguments;
  }

  public ScriptDebugInvocationResult Execute(ILanguageSession languageSession, string scriptCode)
  {
    if (languageSession == null)
      throw new ArgumentNullException(nameof (languageSession));
    if (scriptCode == null)
      throw new ArgumentNullException(nameof (scriptCode));
    LanguageInfo languageInfo = this.scriptProject.LanguageInfo;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession userSession = sessionKeeper.Session;
      if (languageInfo.IsDynamic)
        userSession = (IUserSession) TransparentProxyAdapter.CreateAdapter((object) userSession, typeof (IUserSession));
      return languageSession.Execute(scriptCode, new ScriptDebugInvocationParameters()
      {
        Arguments = {
          (object) userSession,
          (object) this.scriptArguments.ToArray()
        }
      });
    }
  }
}
