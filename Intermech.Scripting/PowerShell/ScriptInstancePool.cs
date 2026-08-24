// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.PowerShell.ScriptInstancePool
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Pools;
using Intermech.Scripting.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;

#nullable disable
namespace Intermech.Scripting.PowerShell;

internal sealed class ScriptInstancePool
{
  private int runspaceUseCountLimit;
  private InitialSessionState initialSessionState;
  private ConcurrentBagPool<Runspace> runspacePool;
  private ConcurrentDictionary<ScriptCodeKey, ScriptInstanceBucket> scriptInstanceCache;
  private ConcurrentLRUList<ScriptInstanceBucket> scriptInstanceLRU;
  private int scriptInstanceSaveCount;

  public ScriptInstancePool(int runspaceUseCountLimit)
  {
    this.runspaceUseCountLimit = runspaceUseCountLimit > 0 ? runspaceUseCountLimit : throw new ArgumentOutOfRangeException(nameof (runspaceUseCountLimit));
    this.initialSessionState = this.CreateInitialSessionState();
    this.runspacePool = new ConcurrentBagPool<Runspace>(0, new Func<Runspace>(this.CreateRunspace));
    this.scriptInstanceCache = new ConcurrentDictionary<ScriptCodeKey, ScriptInstanceBucket>();
    this.scriptInstanceLRU = new ConcurrentLRUList<ScriptInstanceBucket>();
  }

  public void Clear()
  {
  }

  public ScriptInstanceData Allocate(string scriptCode, ScriptCodeKey scriptCodeKey)
  {
    ScriptInstanceBucket orAdd = this.scriptInstanceCache.GetOrAdd(scriptCodeKey, new Func<ScriptCodeKey, ScriptInstanceBucket>(this.CreateScriptInstanceBucket));
    this.scriptInstanceLRU.AddOrUpdate(orAdd);
    ScriptInstanceData result;
    if (orAdd.CachedRunspaces.TryTake(out result))
      return result;
    ScriptInstanceData scriptInstanceData = new ScriptInstanceData(orAdd);
    scriptInstanceData.IsEmpty = true;
    scriptInstanceData.Runspace = this.AllocateRunspace();
    try
    {
      scriptInstanceData.InitialFunctions = this.GetRunspaceFunctionList(scriptInstanceData.Runspace);
    }
    catch
    {
      this.ReleaseRunspace(scriptInstanceData.Runspace, true);
      throw;
    }
    return scriptInstanceData;
  }

  public void Release(ScriptCodeKey scriptCodeKey, ScriptInstanceData scriptInstanceData)
  {
    if (scriptInstanceData.UseCount >= this.runspaceUseCountLimit)
    {
      this.ReleaseRunspace(scriptInstanceData.Runspace, false);
      Interlocked.Decrement(ref this.scriptInstanceSaveCount);
    }
    else
    {
      this.scriptInstanceCache.GetOrAdd(scriptCodeKey, new Func<ScriptCodeKey, ScriptInstanceBucket>(this.CreateScriptInstanceBucket)).CachedRunspaces.Add(scriptInstanceData);
      if (Interlocked.Increment(ref this.scriptInstanceSaveCount) <= 100)
        return;
      ScriptInstanceBucket last = this.scriptInstanceLRU.TryGetLast();
      ScriptInstanceData result;
      if (last == null || !last.CachedRunspaces.TryTake(out result))
        return;
      this.ResetRunspaceState(result.Runspace, result.InitialFunctions);
      this.ReleaseRunspace(result.Runspace, true);
      Interlocked.Decrement(ref this.scriptInstanceSaveCount);
    }
  }

  private ScriptInstanceBucket CreateScriptInstanceBucket(ScriptCodeKey scriptCodeKey)
  {
    return new ScriptInstanceBucket(scriptCodeKey);
  }

  private ICollection<string> GetRunspaceFunctionList(Runspace runspace)
  {
    using (System.Management.Automation.PowerShell powerShell = System.Management.Automation.PowerShell.Create())
    {
      powerShell.Runspace = runspace;
      powerShell.AddCommand("Get-ChildItem").AddArgument((object) "function:");
      Collection<PSObject> collection;
      try
      {
        collection = powerShell.Invoke();
      }
      catch (RuntimeException ex)
      {
        throw new ScriptInvocationException("Произошла неожиданная ошибка при получении списка функций в Powershell-сценарии.", (Exception) ex);
      }
      HashSet<string> runspaceFunctionList = new HashSet<string>(collection.Count);
      foreach (PSObject psObject in collection)
      {
        FunctionInfo baseObject = (FunctionInfo) psObject.BaseObject;
        runspaceFunctionList.Add(baseObject.Name);
      }
      return (ICollection<string>) runspaceFunctionList;
    }
  }

  private void ResetRunspaceState(Runspace runspace, ICollection<string> initialFunctions)
  {
    if (initialFunctions != null)
      this.ResetRunspaceFunctions(runspace, initialFunctions);
    runspace.ResetRunspaceState();
  }

  private void ResetRunspaceFunctions(Runspace runspace, ICollection<string> initialFunctions)
  {
    foreach (string runspaceFunction in (IEnumerable<string>) this.GetRunspaceFunctionList(runspace))
    {
      if (!initialFunctions.Contains(runspaceFunction))
        this.RemoveRunspaceFunction(runspace, runspaceFunction);
    }
  }

  private void RemoveRunspaceFunction(Runspace runspace, string functionName)
  {
    using (System.Management.Automation.PowerShell powerShell = System.Management.Automation.PowerShell.Create())
    {
      powerShell.Runspace = runspace;
      powerShell.AddCommand("Remove-Item").AddArgument((object) ("function:" + functionName));
      try
      {
        powerShell.Invoke();
      }
      catch (RuntimeException ex)
      {
        throw new ScriptInvocationException("Произошла неожиданная ошибка при удалении функции из среды выполнения Powershell-сценария.", (Exception) ex);
      }
    }
  }

  private InitialSessionState CreateInitialSessionState()
  {
    InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
    initialSessionState.ThreadOptions = PSThreadOptions.UseCurrentThread;
    return initialSessionState;
  }

  private Runspace CreateRunspace()
  {
    Runspace runspace = RunspaceFactory.CreateRunspace(this.initialSessionState);
    runspace.Open();
    return runspace;
  }

  public Runspace AllocateRunspace() => this.runspacePool.Allocate();

  public void ReleaseRunspace(Runspace runspace, bool allowReuse)
  {
    if (allowReuse)
    {
      this.runspacePool.Release(runspace);
    }
    else
    {
      runspace.Close();
      runspace.Dispose();
    }
  }
}
