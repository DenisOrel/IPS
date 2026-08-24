// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ScriptExecutor`1
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Pools;
using Intermech.Scripting.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace Intermech.Scripting.CSharp;

public sealed class ScriptExecutor<TScriptContext> : IScriptExecutorEvents where TScriptContext : class
{
  private TScriptContext scriptContext;
  private object syncRoot;
  private ScriptCodeHashService scriptCodeHashService;
  private CurrentProcessTempDirectoryService tempDirectoryService;
  private ScriptExecutorServices executorServices;
  private IObjectPool<IScriptCompilerAgent> compilerPool;
  private IObjectPool<IScriptExecutorAgent> executorPool;
  private ScriptArgumentsConverter argumentsConverter;

  /// <summary>Создает объект.</summary>
  /// <param name="scriptContext">Контекст выполнения сценариев</param>
  public ScriptExecutor(TScriptContext scriptContext)
  {
    this.scriptContext = (object) scriptContext != null ? scriptContext : throw new ArgumentNullException(nameof (scriptContext));
    this.syncRoot = new object();
    this.scriptCodeHashService = new ScriptCodeHashService();
    this.tempDirectoryService = new CurrentProcessTempDirectoryService(Path.Combine(Path.GetTempPath(), "CSharpScripts"));
    this.executorServices = new ScriptExecutorServices(new ScriptFileNameGenerator(this.tempDirectoryService.DirectoryPath, "Script", ".cs"));
    this.compilerPool = (IObjectPool<IScriptCompilerAgent>) new ConcurrentBagPool<IScriptCompilerAgent>(0, new Func<IScriptCompilerAgent>(this.CreateCompilerAgent));
    this.executorPool = (IObjectPool<IScriptExecutorAgent>) new ConcurrentBagPool<IScriptExecutorAgent>(0, new Func<IScriptExecutorAgent>(this.CreateExecutorAgent));
    this.tempDirectoryService.CreateDirectory();
  }

  /// <summary>Возвращает контекст выполнения сценариев.</summary>
  public TScriptContext ScriptContext
  {
    [DebuggerStepThrough] get => this.scriptContext;
  }

  /// <summary>
  /// Возвращает или задает объект для предварительной валидации и преобразования аргументов вызова сценария.
  /// Значение свойства может быть не задано и равно null.
  /// </summary>
  public ScriptArgumentsConverter ArgumentsConverter
  {
    [DebuggerStepThrough] get
    {
      lock (this.syncRoot)
        return this.argumentsConverter;
    }
    [DebuggerStepThrough] set
    {
      lock (this.syncRoot)
        this.argumentsConverter = value;
    }
  }

  /// <summary>
  /// Возвращает или задает провайдер для путей поиска сборок, на которые имеются ссылки из сценариев.
  /// </summary>
  /// <exception cref="T:System.ArgumentNullException">Значение свойства не должно быть равно null</exception>
  public SearchPathListProvider SearchPathListProvider
  {
    [DebuggerStepThrough] get => this.executorServices.SearchPathListProvider;
    [DebuggerStepThrough] set
    {
      this.executorServices.SearchPathListProvider = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  /// <summary>
  /// Возвращает или задает коллекцию имен файлов сборок, которые всегда передаются компилятору сценариев, даже если они не указаны в самом сценарии.
  /// </summary>
  /// <exception cref="T:System.ArgumentNullException">Значение свойства не должно быть равно null</exception>
  public ICollection<string> AutoReferencedAssemblies
  {
    [DebuggerStepThrough] get => this.executorServices.AutoReferencedAssemblies;
    [DebuggerStepThrough] set
    {
      this.executorServices.AutoReferencedAssemblies = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  /// <summary>
  /// Возвращает или задает сервис для внедрения зависимостей в объекты сценариев.
  /// </summary>
  /// <exception cref="T:System.ArgumentNullException">Значение свойства не должно быть равно null</exception>
  public ScriptDependencyInjectionService DependencyInjectionService
  {
    [DebuggerStepThrough] get => this.executorServices.DependencyInjectionService;
    [DebuggerStepThrough] set => this.executorServices.DependencyInjectionService = value;
  }

  private IScriptExecutorAgent CreateExecutorAgent()
  {
    MainDomainScriptExecutorAgent executorAgent = new MainDomainScriptExecutorAgent();
    executorAgent.ExecutorServices = this.executorServices;
    return (IScriptExecutorAgent) executorAgent;
  }

  private IScriptCompilerAgent CreateCompilerAgent()
  {
    RoslynCompilerAgent compilerAgent = new RoslynCompilerAgent();
    compilerAgent.ExecutorServices = this.executorServices;
    return (IScriptCompilerAgent) compilerAgent;
  }

  public object Execute(
    string scriptCode,
    IScriptInvocationOptions options,
    params object[] arguments)
  {
    if (scriptCode == null)
      throw new ArgumentNullException(nameof (scriptCode));
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    if (arguments == null)
      throw new ArgumentNullException(nameof (arguments));
    ScriptCodeKey scriptCodeKey = new ScriptCodeKey(this.scriptCodeHashService.ComputeHash(scriptCode), options.EnableDebugInfo);
    if (!this.executorServices.CompiledCodeCache.ContainsKey(scriptCodeKey))
      this.CompileCore(scriptCode, scriptCodeKey, options);
    this.ArgumentsConverter?.Convert((IList<object>) arguments);
    object obj;
    try
    {
      obj = this.ExecuteCore(scriptCode, scriptCodeKey, options, arguments);
    }
    catch (ScriptInvocationException ex)
    {
      this.RaiseScriptInvocationFailed(scriptCode, arguments, ex);
      throw;
    }
    this.RaiseScriptInvocationCompleted(scriptCode, arguments);
    return obj;
  }

  public IScriptObjectKeeper CreateScriptObject(string scriptCode, IScriptInvocationOptions options)
  {
    if (scriptCode == null)
      throw new ArgumentNullException(nameof (scriptCode));
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    if (options.DebugStream != null)
      throw new ScriptExecutorException("В этом режиме выполнения C#-сценариев использование DebugStream не поддерживается.");
    ScriptCodeKey scriptCodeKey = new ScriptCodeKey(this.scriptCodeHashService.ComputeHash(scriptCode), options.EnableDebugInfo);
    if (!this.executorServices.CompiledCodeCache.ContainsKey(scriptCodeKey))
      this.CompileCore(scriptCode, scriptCodeKey, options);
    IScriptExecutorAgent scriptExecutorAgent = (IScriptExecutorAgent) null;
    ScriptInvocationData invocationData = (ScriptInvocationData) null;
    try
    {
      scriptExecutorAgent = this.executorPool.Allocate();
      invocationData = new ScriptInvocationData();
      CreateScriptObjectParams createParams = new CreateScriptObjectParams((object) this.scriptContext, typeof (TScriptContext), invocationData, options.EnableDebugInfo);
      return scriptExecutorAgent.CreateScriptObject(scriptCode, scriptCodeKey, createParams);
    }
    finally
    {
      invocationData?.Clear();
      if (scriptExecutorAgent != null)
        this.executorPool.Release(scriptExecutorAgent);
    }
  }

  private void CompileCore(
    string scriptCode,
    ScriptCodeKey scriptCodeKey,
    IScriptInvocationOptions options)
  {
    IScriptCompilerAgent scriptCompilerAgent = (IScriptCompilerAgent) null;
    try
    {
      scriptCompilerAgent = this.compilerPool.Allocate();
      CompileParams compileParams = new CompileParams(typeof (TScriptContext), options.EnableDebugInfo);
      scriptCompilerAgent.Compile(scriptCode, scriptCodeKey, compileParams);
    }
    finally
    {
      if (scriptCompilerAgent != null)
        this.compilerPool.Release(scriptCompilerAgent);
    }
  }

  private object ExecuteCore(
    string scriptCode,
    ScriptCodeKey scriptCodeKey,
    IScriptInvocationOptions options,
    object[] arguments)
  {
    IScriptExecutorAgent scriptExecutorAgent = (IScriptExecutorAgent) null;
    ScriptInvocationData invocationData = (ScriptInvocationData) null;
    try
    {
      scriptExecutorAgent = this.executorPool.Allocate();
      invocationData = new ScriptInvocationData();
      ExecuteParams executeParams = new ExecuteParams((object) this.scriptContext, typeof (TScriptContext), arguments, invocationData, options.EnableDebugInfo, options.DebugStream);
      return scriptExecutorAgent.Execute(scriptCode, scriptCodeKey, executeParams);
    }
    finally
    {
      invocationData?.Clear();
      if (scriptExecutorAgent != null)
        this.executorPool.Release(scriptExecutorAgent);
    }
  }

  /// <summary>
  /// Освобождает все ресурсы и удаляет все временные файлы, связанные с выполнением сценариев.
  /// </summary>
  public void Shutdown()
  {
  }

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
