// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.PowerShell.ScriptExecutor`1
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Collections;
using Intermech.Scripting.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;

#nullable disable
namespace Intermech.Scripting.PowerShell;

/// <summary>Реализация является thread safe.</summary>
public class ScriptExecutor<TScriptContext> : IScriptExecutorEvents where TScriptContext : class
{
  private TScriptContext scriptContext;
  private ScriptCodeHashService scriptCodeHashService;
  private ScriptInstancePool scriptInstancePool;
  private ScriptDependencyInjectionService dependencyInjectionService;
  private object syncRoot;

  /// <summary>Создает объект.</summary>
  /// <param name="scriptContext">Контекст выполнения сценариев</param>
  /// <param name="runspaceUseCountLimit">Ограничение переиспользования изолированных сред, используемых для выполнения сценариев</param>
  public ScriptExecutor(TScriptContext scriptContext, int runspaceUseCountLimit)
  {
    if ((object) scriptContext == null)
      throw new ArgumentNullException(nameof (scriptContext));
    if (runspaceUseCountLimit <= 0)
      throw new ArgumentOutOfRangeException(nameof (runspaceUseCountLimit));
    this.scriptContext = scriptContext;
    this.scriptCodeHashService = new ScriptCodeHashService();
    this.scriptInstancePool = new ScriptInstancePool(runspaceUseCountLimit);
    this.dependencyInjectionService = (ScriptDependencyInjectionService) EmptyScriptDependencyInjectionService.Default;
    this.syncRoot = new object();
  }

  /// <summary>Возвращает контекст выполнения сценариев.</summary>
  public TScriptContext ScriptContext
  {
    [DebuggerStepThrough] get => this.scriptContext;
  }

  /// <summary>
  /// Возвращает или задает сервис для внедрения зависимостей в объекты сценариев.
  /// </summary>
  /// <exception cref="T:System.ArgumentNullException">Значение свойства не должно быть равно null</exception>
  public ScriptDependencyInjectionService DependencyInjectionService
  {
    [DebuggerStepThrough] get
    {
      lock (this.syncRoot)
        return this.dependencyInjectionService;
    }
    [DebuggerStepThrough] set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      lock (this.syncRoot)
        this.dependencyInjectionService = value;
    }
  }

  public object Execute(string scriptCode, params object[] arguments)
  {
    if (scriptCode == null)
      throw new ArgumentNullException(nameof (scriptCode));
    if (arguments == null)
      throw new ArgumentNullException(nameof (arguments));
    object obj;
    try
    {
      obj = this.ExecuteCore(scriptCode, arguments);
    }
    catch (ScriptInvocationException ex)
    {
      this.RaiseScriptInvocationFailed(scriptCode, arguments, ex);
      throw;
    }
    this.RaiseScriptInvocationCompleted(scriptCode, arguments);
    return obj;
  }

  private object ExecuteCore(string scriptCode, object[] arguments)
  {
    ScriptCodeKey scriptCodeKey = (ScriptCodeKey) null;
    ScriptInstanceData scriptInstanceData = (ScriptInstanceData) null;
    ScriptInvocationData invocationData = (ScriptInvocationData) null;
    try
    {
      scriptCodeKey = new ScriptCodeKey(this.scriptCodeHashService.ComputeHash(scriptCode), false);
      scriptInstanceData = this.scriptInstancePool.Allocate(scriptCode, scriptCodeKey);
      if (scriptInstanceData.IsEmpty)
      {
        this.PrepareScriptToExecute(scriptCode, scriptCodeKey, scriptInstanceData);
        scriptInstanceData.SharedData.SetServiceProperties(this.GetScriptServiceProperties(scriptInstanceData).ToArray());
      }
      invocationData = new ScriptInvocationData();
      return this.InvokeExecuteMethod(scriptCode, scriptCodeKey, invocationData, scriptInstanceData, arguments);
    }
    finally
    {
      invocationData?.Clear();
      if (scriptInstanceData != null)
      {
        ++scriptInstanceData.UseCount;
        this.scriptInstancePool.Release(scriptCodeKey, scriptInstanceData);
      }
    }
  }

  private void PrepareScriptToExecute(
    string scriptCode,
    ScriptCodeKey scriptCodeKey,
    ScriptInstanceData scriptInstanceData)
  {
    using (System.Management.Automation.PowerShell powerShell = System.Management.Automation.PowerShell.Create())
    {
      powerShell.Runspace = scriptInstanceData.Runspace;
      powerShell.Commands.AddScript(scriptCode);
      try
      {
        powerShell.Invoke();
      }
      catch (ParseException ex)
      {
        throw this.CreateScriptCompilationException(ex);
      }
      catch (RuntimeException ex)
      {
        throw this.CreateScriptInvocationException((Exception) ex);
      }
    }
    scriptInstanceData.IsEmpty = false;
  }

  private List<string> GetScriptServiceProperties(ScriptInstanceData scriptInstanceData)
  {
    using (System.Management.Automation.PowerShell powerShell = System.Management.Automation.PowerShell.Create())
    {
      powerShell.Runspace = scriptInstanceData.Runspace;
      powerShell.Commands.AddCommand("Get-Variable");
      Collection<PSObject> collection;
      try
      {
        collection = powerShell.Invoke();
      }
      catch (RuntimeException ex)
      {
        throw this.CreateScriptInvocationException((Exception) ex);
      }
      List<string> serviceProperties = new List<string>();
      foreach (PSObject psObject in collection)
      {
        PSVariable baseObject = (PSVariable) psObject.BaseObject;
        if (baseObject.Value == null)
        {
          string name = baseObject.Name;
          if (name.StartsWith("I") && name.EndsWith("Service"))
            serviceProperties.Add(name);
        }
      }
      return serviceProperties;
    }
  }

  private object InvokeExecuteMethod(
    string scriptCode,
    ScriptCodeKey scriptCodeKey,
    ScriptInvocationData invocationData,
    ScriptInstanceData scriptInstanceData,
    object[] arguments)
  {
    using (System.Management.Automation.PowerShell powerShell = System.Management.Automation.PowerShell.Create())
    {
      powerShell.Runspace = scriptInstanceData.Runspace;
      powerShell.AddCommand("execute");
      for (int index = 0; index < arguments.Length; ++index)
        powerShell.AddArgument(arguments[index]);
      Collection<PSObject> collection;
      try
      {
        this.SetScriptVariables(scriptCodeKey, invocationData, scriptInstanceData);
        collection = powerShell.Invoke();
      }
      catch (RuntimeException ex)
      {
        throw this.CreateScriptInvocationException((Exception) ex);
      }
      finally
      {
        this.ResetScriptVariables(scriptCodeKey, invocationData, scriptInstanceData);
      }
      if (powerShell.Streams.Error.Count != 0)
        throw this.CreateScriptInvocationException(powerShell.Streams.Error[0].Exception);
      return collection[collection.Count - 1].BaseObject;
    }
  }

  private void SetScriptVariables(
    ScriptCodeKey scriptCodeKey,
    ScriptInvocationData invocationData,
    ScriptInstanceData scriptInstanceData)
  {
    SessionStateProxy sessionStateProxy = scriptInstanceData.Runspace.SessionStateProxy;
    sessionStateProxy.SetVariable("ScriptContext", (object) this.scriptContext);
    string[] serviceProperties = scriptInstanceData.SharedData.ServiceProperties;
    object[] objArray = this.dependencyInjectionService.ResolveProperties(scriptCodeKey, invocationData, serviceProperties);
    for (int index = 0; index < serviceProperties.Length; ++index)
      sessionStateProxy.SetVariable(serviceProperties[index], objArray[index]);
  }

  private void ResetScriptVariables(
    ScriptCodeKey scriptCodeKey,
    ScriptInvocationData invocationData,
    ScriptInstanceData scriptInstanceData)
  {
    SessionStateProxy sessionStateProxy = scriptInstanceData.Runspace.SessionStateProxy;
    sessionStateProxy.SetVariable("ScriptContext", (object) null);
    foreach (string serviceProperty in scriptInstanceData.SharedData.ServiceProperties)
      sessionStateProxy.SetVariable(serviceProperty, (object) null);
  }

  private ScriptCompilationException CreateScriptCompilationException(ParseException parseException)
  {
    return parseException.Errors != null && parseException.Errors.Length != 0 ? ScriptCompilationException.FromErrors("PowerShell", (IList<ScriptCompilationError>) CollectionUtils.ConvertAsArray<ParseError, ScriptCompilationError>((ICollection<ParseError>) parseException.Errors, new Converter<ParseError, ScriptCompilationError>(this.ToScriptCompilationError))) : new ScriptCompilationException(parseException.Message);
  }

  private ScriptCompilationError ToScriptCompilationError(ParseError parseError)
  {
    return new ScriptCompilationError(parseError.ErrorId, parseError.Message, string.Empty, parseError.Extent.StartLineNumber, parseError.Extent.StartColumnNumber, false);
  }

  private ScriptInvocationException CreateScriptInvocationException(Exception exception)
  {
    return new ScriptInvocationException("Необработанное исключение в коде PowerShell-сценария.", exception);
  }

  /// <summary>
  /// Освобождает все ресурсы и удаляет все временные файлы, связанные с выполнением сценариев.
  /// </summary>
  public void Shutdown() => this.scriptInstancePool.Clear();

  private void RaiseScriptInvocationCompleted(string scriptCode, object[] arguments)
  {
    EventHandler<ScriptInvocationEventArgs> invocationCompleted = this.ScriptInvocationCompleted;
    if (invocationCompleted == null)
      return;
    invocationCompleted((object) this, new ScriptInvocationEventArgs(scriptCode, arguments));
  }

  private void RaiseScriptInvocationFailed(
    string scriptCode,
    object[] arguments,
    ScriptInvocationException exception)
  {
    EventHandler<ScriptInvocationFailedEventArgs> invocationFailed = this.ScriptInvocationFailed;
    if (invocationFailed == null)
      return;
    invocationFailed((object) this, new ScriptInvocationFailedEventArgs(scriptCode, arguments, exception));
  }

  /// <summary>Событие успешного выполнения сценария.</summary>
  public event EventHandler<ScriptInvocationEventArgs> ScriptInvocationCompleted;

  /// <summary>Событие неудачного выполнения сценария.</summary>
  public event EventHandler<ScriptInvocationFailedEventArgs> ScriptInvocationFailed;
}
