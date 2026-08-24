// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ScriptProject
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Базовый класс для сценарных проектов в IDE.
/// Реализация не является thread safe.
/// </summary>
/// <remarks>
/// Сценарные проекты являются аналогом .csproj-файлов в Visual Studio.
/// Каждый проект хранит код и свойства одного сценария, задающие способ выполнения и отладки этого сценария.
/// </remarks>
public class ScriptProject
{
  private LanguageInfo languageInfo;
  private string name;
  private object repositoryKey;
  private ScriptProjectFile file;
  private ScriptProjectBehaviors behaviors;

  /// <summary>Создает объект.</summary>
  /// <param name="languageInfo">Язык сценария</param>
  /// <exception cref="T:System.ArgumentNullException">параметр <paramref name="languageInfo" /> не должен быть равен null</exception>
  public ScriptProject(LanguageInfo languageInfo)
  {
    this.languageInfo = languageInfo;
    this.name = string.Empty;
    this.file = (ScriptProjectFile) new InMemoryScriptProjectFile();
    this.behaviors = new ScriptProjectBehaviors();
  }

  /// <summary>Возвращает задает язык сценария.</summary>
  public LanguageInfo LanguageInfo => this.languageInfo;

  /// <summary>Возвращает или задает имя сценария.</summary>
  public string Name
  {
    get => this.name;
    set => this.name = value != null ? value : throw new ArgumentNullException(nameof (value));
  }

  /// <summary>
  /// Возвращает или задает идентификатор сценария в хранилище сценариев.
  /// Значение свойства может быть не задано, если сценарий еще не был добавлен в хранилище.
  /// </summary>
  public object RepositoryKey
  {
    [DebuggerStepThrough] get => this.repositoryKey;
    [DebuggerStepThrough] set => this.repositoryKey = value;
  }

  /// <summary>
  /// Возвращает признак нового сценария, еще не добавленного в хранилище сценариев.
  /// </summary>
  public bool IsNew
  {
    [DebuggerStepThrough] get => this.repositoryKey == null;
  }

  /// <summary>Возвращает контейнер для кода сценария.</summary>
  public ScriptProjectFile File
  {
    [DebuggerStepThrough] get => this.file;
  }

  /// <summary>Возвращает "поведения" сценария для интеграции с IDE.</summary>
  public ScriptProjectBehaviors Behaviors
  {
    [DebuggerStepThrough] get => this.behaviors;
  }
}
