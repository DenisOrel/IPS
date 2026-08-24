// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ServiceProcess.IScriptCompiler
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.CSharp.ServiceProcess;

/// <summary>
/// Базовый интерфейс компилятора C#-сценариев.
/// Реализация должна быть thread safe.
/// </summary>
public interface IScriptCompiler
{
  /// <summary>
  /// Возвращает список путей к сборкам, требуемым для компиляции и выполнения сценария.
  /// </summary>
  /// <param name="scriptCode">Код сценария</param>
  /// <param name="options">Опции компиляции сценария</param>
  /// <returns>Список путей к сборкам</returns>
  /// <exception cref="T:System.ArgumentNullException">параметр <paramref name="scriptCode" /> содержит null; параметр <paramref name="options" /> содержит null</exception>
  List<string> GetReferences(string scriptCode, ScriptCompilerOptions options);

  /// <summary>Компилирует сценарий в файл.</summary>
  /// <param name="scriptFilePath">Путь к файлу с исходным кодом сценария.</param>
  /// <param name="options">Опции компиляции сценария</param>
  /// <returns>Путь к файлу с кодом сценария</returns>
  /// <exception cref="T:System.ArgumentNullException">параметр <paramref name="scriptFilePath" /> содержит null; параметр <paramref name="options" /> содержит null</exception>
  /// <exception cref="T:Intermech.Scripting.ScriptCompilationException">Код сценария содержит ошибки</exception>
  string CompileToFile(string scriptFilePath, ScriptCompilerOptions options);
}
