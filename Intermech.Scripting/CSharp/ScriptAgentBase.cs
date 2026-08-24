// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ScriptAgentBase
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Diagnostics;
using System.Reflection;

#nullable disable
namespace Intermech.Scripting.CSharp;

internal abstract class ScriptAgentBase : MarshalByRefObject
{
  private ScriptExecutorServices executorServices;
  private CompiledCodeCache compiledCodeCacheService;

  public ScriptExecutorServices ExecutorServices
  {
    [DebuggerStepThrough] get => this.executorServices;
    [DebuggerStepThrough] set
    {
      this.executorServices = value;
      this.UpdateMostUsedExecutorServicesCache();
    }
  }

  protected virtual void UpdateMostUsedExecutorServicesCache()
  {
    if (this.ExecutorServices != null)
      this.compiledCodeCacheService = this.ExecutorServices.CompiledCodeCache;
    else
      this.compiledCodeCacheService = (CompiledCodeCache) null;
  }

  protected CompiledCodeCache CompiledCodeCacheService
  {
    [DebuggerStepThrough] get => this.compiledCodeCacheService;
  }

  protected Assembly LoadScriptAssembly(string scriptAssemblyPath)
  {
    return Assembly.LoadFrom(scriptAssemblyPath);
  }

  /// <summary>
  /// Возвращает сервис управления временем жизни текущего объекта
  /// </summary>
  /// <returns>Сервис управления временем жизни</returns>
  public override object InitializeLifetimeService() => (object) null;
}
