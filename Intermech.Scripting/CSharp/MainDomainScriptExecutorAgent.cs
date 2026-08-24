// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.MainDomainScriptExecutorAgent
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Агент выполнения C#-сценариев в основном AppDomain приложения.
/// Реализация класса не является thread safe.
/// </summary>
/// <remarks>
/// <para>
/// Агент работает в основном AppDomain приложения, в его задачи входит выполнение
/// экземплярных C#-сценариев без статических полей данных.
/// Взаимодействие с родительским объектом выполняется через
/// свойство <see cref="P:Intermech.Scripting.CSharp.ScriptAgentBase.ExecutorServices" />.</para>
/// <para>
/// Экземпляры агентов этого типа собраны в пул, а в каждый момент времени
/// выполняется только одно обращение к каждому из агентов. Поэтому от них
/// не требуется быть thread safe.</para>
/// <para>
/// Для отладки сценариев в Visual Studio требуется в настройках отладчика Visual Studio
/// отключить опцию "Enable Just My Code" и включить опцию "Use Managed Compatibility Mode".
/// </para>
/// </remarks>
internal sealed class MainDomainScriptExecutorAgent : ScriptAgentBase, IScriptExecutorAgent
{
  private ScriptDebugTraceListener scriptDebugTraceListener;
  private static readonly ConcurrentDictionary<ScriptCodeKey, MainDomainScriptRuntimeHelper> executeRuntimeHelpers = new ConcurrentDictionary<ScriptCodeKey, MainDomainScriptRuntimeHelper>();
  private static readonly ConcurrentDictionary<ScriptCodeKey, MainDomainScriptRuntimeHelper> createScriptObjectRuntimeHelpers = new ConcurrentDictionary<ScriptCodeKey, MainDomainScriptRuntimeHelper>();

  public MainDomainScriptExecutorAgent()
  {
    this.scriptDebugTraceListener = new ScriptDebugTraceListener();
  }

  public object Execute(
    string scriptCode,
    ScriptCodeKey scriptCodeKey,
    ExecuteParams executeParams)
  {
    CompiledCodeInfo compiledCode = this.GetCompiledCode(scriptCodeKey);
    MainDomainScriptRuntimeHelper scriptRuntimeHelper = this.GetOrCreateScriptRuntimeHelper(scriptCodeKey, executeParams.ScriptContextType, compiledCode, true);
    MainDomainScriptInstanceKeeper scriptInstanceKeeper = (MainDomainScriptInstanceKeeper) null;
    bool flag = false;
    try
    {
      object scriptInstance = scriptRuntimeHelper.CtorMethod.Invoke((object[]) null);
      scriptInstanceKeeper = new MainDomainScriptInstanceKeeper(Thread.CurrentThread.ManagedThreadId, scriptInstance, scriptRuntimeHelper);
      this.SetContextAndServiceProperties(scriptCodeKey, (ExecuteAgentParams) executeParams, scriptRuntimeHelper, scriptInstance);
      flag = this.TrySetOutputStream(executeParams);
      return scriptRuntimeHelper.ExecuteMethod.Invoke(scriptInstance, executeParams.Arguments);
    }
    catch (TargetInvocationException ex)
    {
      if (ex.InnerException != null)
        throw this.CreateScriptInvocationException(ex);
      throw;
    }
    finally
    {
      if (flag)
        this.ResetOutputStream(executeParams);
      scriptInstanceKeeper?.Dispose();
    }
  }

  public IScriptObjectKeeper CreateScriptObject(
    string scriptCode,
    ScriptCodeKey scriptCodeKey,
    CreateScriptObjectParams createParams)
  {
    CompiledCodeInfo compiledCode = this.GetCompiledCode(scriptCodeKey);
    MainDomainScriptRuntimeHelper scriptRuntimeHelper = this.GetOrCreateScriptRuntimeHelper(scriptCodeKey, createParams.ScriptContextType, compiledCode, false);
    MainDomainScriptInstanceKeeper scriptObject = (MainDomainScriptInstanceKeeper) null;
    try
    {
      object scriptInstance = scriptRuntimeHelper.CtorMethod.Invoke((object[]) null);
      scriptObject = new MainDomainScriptInstanceKeeper(Thread.CurrentThread.ManagedThreadId, scriptInstance, scriptRuntimeHelper);
      this.SetContextAndServiceProperties(scriptCodeKey, (ExecuteAgentParams) createParams, scriptRuntimeHelper, scriptInstance);
      return (IScriptObjectKeeper) scriptObject;
    }
    catch (Exception ex)
    {
      scriptObject?.Dispose();
      if (ex is TargetInvocationException && ex.InnerException != null)
        throw this.CreateScriptInvocationException((TargetInvocationException) ex);
      throw;
    }
  }

  private CompiledCodeInfo GetCompiledCode(ScriptCodeKey scriptCodeKey)
  {
    return this.CompiledCodeCacheService.TryGet(scriptCodeKey) ?? throw new InvalidOperationException("Код C#-сценария должен быть предварительно скомпилирован.");
  }

  private MainDomainScriptRuntimeHelper GetOrCreateScriptRuntimeHelper(
    ScriptCodeKey scriptCodeKey,
    Type scriptContextType,
    CompiledCodeInfo compiledCodeInfo,
    bool executeMode)
  {
    ConcurrentDictionary<ScriptCodeKey, MainDomainScriptRuntimeHelper> concurrentDictionary = executeMode ? MainDomainScriptExecutorAgent.executeRuntimeHelpers : MainDomainScriptExecutorAgent.createScriptObjectRuntimeHelpers;
    MainDomainScriptRuntimeHelper scriptRuntimeHelper;
    if (!concurrentDictionary.TryGetValue(scriptCodeKey, out scriptRuntimeHelper))
    {
      scriptRuntimeHelper = this.CreateScriptRuntimeHelper(this.LoadScriptAssembly(compiledCodeInfo.AssemblyFilePath), scriptContextType, executeMode);
      scriptRuntimeHelper = concurrentDictionary.GetOrAdd(scriptCodeKey, scriptRuntimeHelper);
    }
    return scriptRuntimeHelper;
  }

  private void SetContextAndServiceProperties(
    ScriptCodeKey scriptCodeKey,
    ExecuteAgentParams initializationParams,
    MainDomainScriptRuntimeHelper scriptRuntimeHelper,
    object scriptInstance)
  {
    scriptRuntimeHelper.ScriptContextProperty.SetValue(scriptInstance, initializationParams.ScriptContext);
    if (!scriptRuntimeHelper.HasServiceProperties)
      return;
    ScriptInvocationData invocationData = initializationParams.InvocationData;
    object[] objArray = this.ExecutorServices.DependencyInjectionService.ResolveProperties(scriptCodeKey, invocationData, scriptRuntimeHelper.ServicePropertyTypes);
    for (int index = 0; index < scriptRuntimeHelper.ServiceProperties.Length; ++index)
      scriptRuntimeHelper.ServiceProperties[index].SetValue(scriptInstance, objArray[index]);
  }

  private bool TrySetOutputStream(ExecuteParams executeParams)
  {
    if (executeParams.DebugStream == null)
      return false;
    Trace.Listeners.Add((TraceListener) this.scriptDebugTraceListener);
    this.scriptDebugTraceListener.ThreadId = Thread.CurrentThread.ManagedThreadId;
    return true;
  }

  private void ResetOutputStream(ExecuteParams executeParams)
  {
    Trace.Listeners.Remove((TraceListener) this.scriptDebugTraceListener);
    this.scriptDebugTraceListener.ThreadId = 0;
    if (this.scriptDebugTraceListener.IsEmpty)
      return;
    IScriptOutputStream debugStream = executeParams.DebugStream;
    foreach (string line in this.scriptDebugTraceListener.ToList())
      debugStream.WriteLine(line);
    this.scriptDebugTraceListener.Clear();
  }

  private MainDomainScriptRuntimeHelper CreateScriptRuntimeHelper(
    Assembly scriptAssembly,
    Type scriptContextType,
    bool requireExecuteMethod)
  {
    List<Type> typeList = new List<Type>((IEnumerable<Type>) scriptAssembly.GetTypes());
    typeList.RemoveAll((Predicate<Type>) (item => item.Name != "Script"));
    if (typeList.Count == 0)
      throw new ScriptStructureException("Код C#-сценария должен содержать класс Script.");
    Type scriptType = typeList.Count == 1 ? typeList[0] : throw new ScriptStructureException("Код C#-сценария должен содержать только один класс Script.");
    ConstructorInfo constructor = scriptType.GetConstructor(Type.EmptyTypes);
    if (constructor == (ConstructorInfo) null)
      throw new ScriptStructureException("Код C#-сценария должен содержать конструктор по умолчанию Script().");
    MethodInfo executeMethod = (MethodInfo) null;
    if (requireExecuteMethod)
    {
      executeMethod = scriptType.GetMethod("Execute", BindingFlags.Instance | BindingFlags.Public);
      if (executeMethod == (MethodInfo) null)
        throw new ScriptStructureException("Код C#-сценария должен содержать экземплярный метод Execute.");
    }
    PropertyInfo property = scriptType.GetProperty("ScriptContext", BindingFlags.Instance | BindingFlags.Public);
    if (property == (PropertyInfo) null || !property.CanRead || !property.CanWrite)
      throw new ScriptStructureException($"Код C#-сценария должен содержать экземплярное свойство ScriptContext типа \"{scriptContextType}\", доступное для чтения и записи.");
    if (!property.PropertyType.IsAssignableFrom(scriptContextType))
      throw new ScriptStructureException($"В коде C#-сценария тип свойства ScriptContext должен быть \"{scriptContextType}\".");
    PropertyInfo[] serviceProperties = this.CollectServiceProperties(scriptType, property);
    return new MainDomainScriptRuntimeHelper(scriptType, constructor, executeMethod, property, serviceProperties);
  }

  private PropertyInfo[] CollectServiceProperties(
    Type scriptType,
    PropertyInfo scriptContextProperty)
  {
    PropertyInfo[] properties = scriptType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
    List<PropertyInfo> propertyInfoList = new List<PropertyInfo>(properties.Length);
    foreach (PropertyInfo propertyInfo in properties)
    {
      if (!(propertyInfo == scriptContextProperty) && propertyInfo.PropertyType.IsInterface && propertyInfo.CanRead && propertyInfo.CanWrite)
        propertyInfoList.Add(propertyInfo);
    }
    return propertyInfoList.ToArray();
  }

  private ScriptInvocationException CreateScriptInvocationException(
    TargetInvocationException exception)
  {
    return new ScriptInvocationException("Необработанное исключение в коде C#-сценария.", exception.InnerException);
  }
}
