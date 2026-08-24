// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ServiceProcess.ScriptCompilerOptions
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.CSharp.ServiceProcess;

/// <summary>
/// Опции компиляции C#-сценария.
/// Реализация не является thread safe.
/// </summary>
[Serializable]
public class ScriptCompilerOptions
{
  /// <summary>Создает объект.</summary>
  public ScriptCompilerOptions()
  {
    this.AutoReferencedAssemblies = new List<string>();
    this.SearchPathList = new List<string>();
  }

  /// <summary>
  /// Возвращает коллекцию имен файлов сборок, которые всегда передаются компилятору сценариев,
  /// даже если они не указаны в самом сценарии.
  /// </summary>
  public List<string> AutoReferencedAssemblies { get; }

  /// <summary>
  /// Возвращает список путей для поиска сборок, на которые имеются ссылки из сценариев.
  /// </summary>
  public List<string> SearchPathList { get; }

  /// <summary>
  /// Включает и выключает добавление отладочной информации в код сценария.
  /// </summary>
  public bool EnableDebugInfo { get; set; }
}
