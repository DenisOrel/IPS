// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Addons.SchedulerScriptDebugBehavior
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Interfaces;
using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.Projects.DBScripts;

#nullable disable
namespace Intermech.Scripting.Addons;

internal sealed class SchedulerScriptDebugBehavior(DBScriptProject scriptProject) : 
  DBScriptDebugBehavior(scriptProject)
{
  protected override ScriptDebugInvocationResult DoExecute(
    ILanguageSession languageSession,
    string scriptCode,
    ScriptDebugInvocationParameters invocationParameters)
  {
    (IUserSession userSession, string sessionName) = this.CreateDebugSystemSession(invocationParameters);
    try
    {
      invocationParameters.Arguments.Add((object) userSession);
      invocationParameters.Arguments.Add((object) null);
      return languageSession.Execute(scriptCode, invocationParameters);
    }
    finally
    {
      userSession.Logout(sessionName);
    }
  }
}
