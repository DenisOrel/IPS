// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.DesignTime.CSharpSession
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Interfaces;
using Intermech.Scripting.Common;
using Intermech.Scripting.Common.Debugging;
using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.CSharp.Debugging;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.CSharp.DesignTime;

internal sealed class CSharpSession : LanguageSession
{
  private ICSharpScriptExecutor clientExecutorService;
  private CSharpDebugOperations clientDebugOperations;

  public CSharpSession(ICSharpScriptExecutor clientExecutorService)
  {
    this.clientExecutorService = clientExecutorService != null ? clientExecutorService : throw new ArgumentNullException(nameof (clientExecutorService));
    this.clientDebugOperations = new CSharpDebugOperations();
  }

  public IScriptOutputStream DebugStream { get; set; }

  protected override Dictionary<string, string> DoGetRuntimeOptions(
    Dictionary<string, string> scriptProjectOptions)
  {
    CSharpScriptRuntimeInfo runtimeInfo;
    if (CSharpScriptProjectOptions.FromDictionary(scriptProjectOptions).RunAtClientSide)
    {
      runtimeInfo = this.clientExecutorService.GetRuntimeInfo();
    }
    else
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        runtimeInfo = ((ICSharpScriptExecutor) sessionKeeper.Session.GetCustomService(typeof (ICSharpScriptExecutor))).GetRuntimeInfo();
    }
    return CSharpScriptDebugRuntimeOptions.ToDictionary(new CSharpScriptDebugRuntimeOptions()
    {
      AutoReferencedAssemblies = runtimeInfo.AutoReferencesAssemblies,
      SearchPathList = runtimeInfo.SearchPathList
    });
  }

  protected override ScriptDebugInvocationResult DoExecute(
    string scriptCode,
    ScriptDebugInvocationParameters invocationParameters)
  {
    DebugExecuteResult debugExecuteResult = !CSharpScriptProjectOptions.FromDictionary(invocationParameters.ProjectOptions).RunAtClientSide ? this.DoExecuteAtServerSide(scriptCode, invocationParameters) : this.DoExecuteAtClientSide(scriptCode, invocationParameters);
    if (debugExecuteResult.DebugOutput != null && debugExecuteResult.DebugOutput.Length != 0)
    {
      foreach (string line in debugExecuteResult.DebugOutput)
        this.DebugStream.WriteLine(line);
    }
    return debugExecuteResult.Exception == null ? new ScriptDebugInvocationResult(debugExecuteResult.ReturnValue, false) : throw debugExecuteResult.Exception;
  }

  private DebugExecuteResult DoExecuteAtClientSide(
    string scriptCode,
    ScriptDebugInvocationParameters invocationParameters)
  {
    return this.clientDebugOperations.DebugExecute(this.clientExecutorService, scriptCode, invocationParameters.Arguments.ToArray());
  }

  private DebugExecuteResult DoExecuteAtServerSide(
    string scriptCode,
    ScriptDebugInvocationParameters invocationParameters)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return ((IDebugExecutor) sessionKeeper.Session.GetCustomService(typeof (ICSharpScriptExecutor))).DebugExecute(ClientTokenProvider.Default.GetClientToken(), scriptCode, (object) null, invocationParameters.Arguments.ToArray());
  }
}
