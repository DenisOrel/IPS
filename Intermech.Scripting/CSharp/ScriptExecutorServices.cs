// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ScriptExecutorServices
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>Реализация класса является thread safe.</summary>
internal sealed class ScriptExecutorServices : ScriptExecutorServiceBase
{
  private ScriptFileNameGenerator tempFileNameGenerator;
  private CompiledCodeCache compiledCodeCache;
  private SearchPathListProvider searchPathListProvider;
  private ICollection<string> autoReferencedAssemblies;
  private ScriptDependencyInjectionService dependencyInjectionService;
  private object syncRoot;

  public ScriptExecutorServices(ScriptFileNameGenerator tempFileNameGenerator)
  {
    this.tempFileNameGenerator = tempFileNameGenerator;
    this.compiledCodeCache = new CompiledCodeCache();
    this.searchPathListProvider = (SearchPathListProvider) EmptySearchPathListProvider.Default;
    this.autoReferencedAssemblies = (ICollection<string>) new string[0];
    this.dependencyInjectionService = (ScriptDependencyInjectionService) EmptyScriptDependencyInjectionService.Default;
    this.syncRoot = new object();
  }

  public ScriptFileNameGenerator TempFileNameGenerator
  {
    [DebuggerStepThrough] get => this.tempFileNameGenerator;
  }

  public CompiledCodeCache CompiledCodeCache
  {
    [DebuggerStepThrough] get => this.compiledCodeCache;
  }

  /// <summary>
  /// Возвращает или задает провайдер для путей поиска сборок, на которые имеются ссылки из сценариев.
  /// </summary>
  /// <exception cref="T:System.ArgumentNullException">Значение свойства не должно быть равно null</exception>
  public SearchPathListProvider SearchPathListProvider
  {
    [DebuggerStepThrough] get
    {
      lock (this.syncRoot)
        return this.searchPathListProvider;
    }
    [DebuggerStepThrough] set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      lock (this.syncRoot)
        this.searchPathListProvider = value;
    }
  }

  /// <summary>
  /// Возвращает или задает коллекцию имен файлов сборок, которые всегда передаются компилятору сценариев, даже если они не указаны в самом сценарии.
  /// </summary>
  /// <exception cref="T:System.ArgumentNullException">Значение свойства не должно быть равно null</exception>
  public ICollection<string> AutoReferencedAssemblies
  {
    [DebuggerStepThrough] get
    {
      lock (this.syncRoot)
        return this.autoReferencedAssemblies;
    }
    [DebuggerStepThrough] internal set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      lock (this.syncRoot)
        this.autoReferencedAssemblies = value;
    }
  }

  /// <summary>
  /// Возвращает сервис для внедрения зависимостей в объекты сценариев.
  /// </summary>
  /// <exception cref="T:System.ArgumentNullException">Значение свойства не должно быть равно null</exception>
  public ScriptDependencyInjectionService DependencyInjectionService
  {
    [DebuggerStepThrough] get
    {
      lock (this.syncRoot)
        return this.dependencyInjectionService;
    }
    [DebuggerStepThrough] internal set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      lock (this.syncRoot)
        this.dependencyInjectionService = value;
    }
  }
}
