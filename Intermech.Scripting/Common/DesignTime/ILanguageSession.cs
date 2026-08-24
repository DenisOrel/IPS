// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ILanguageSession
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс сессии для взаимодействия с языковой средой.
/// Через такую сессию IDE взаимодействует с исполнителем сценариев.
/// Реализация не должна быть thread safe.
/// </summary>
public interface ILanguageSession : IDisposable
{
  /// <summary>Читает текст сценария из указанного массива байт.</summary>
  /// <param name="content">Массив байт с кодом сценария</param>
  /// <returns>Текст сценария и его кодировка</returns>
  /// <exception cref="T:System.ArgumentNullException">Параметр <paramref name="content" /> не должен быть равен null</exception>
  /// <exception cref="T:Intermech.Scripting.ScriptExecutorException">Не удалось найти указанный файл</exception>
  Tuple<string, Encoding> LoadScriptCode(byte[] content);

  /// <summary>
  /// Возвращает опции среды, необходимые для выполнения сценария.
  /// </summary>
  /// <param name="scriptProjectOptions">Опции сценария</param>
  /// <returns>Опции среды выполнения</returns>
  Dictionary<string, string> GetRuntimeOptions(Dictionary<string, string> scriptProjectOptions);

  /// <summary>Выполняет код сценария.</summary>
  /// <param name="scriptCode">Код сценария</param>
  /// <param name="invocationParameters">Параметры вызова сценария</param>
  /// <returns>Результат выполнения</returns>
  /// <exception cref="T:System.ArgumentException">Не задан код сценария</exception>
  /// <exception cref="T:Intermech.Scripting.ScriptStructureException">Сценарий не имеет точки входа</exception>
  /// <exception cref="T:Intermech.Scripting.ScriptCompilationException">Синтаксическая ошибка в коде сценария</exception>
  /// <exception cref="T:Intermech.Scripting.ScriptExecutorException">Другие ошибки загрузки или компиляции сценария</exception>
  ScriptDebugInvocationResult Execute(
    string scriptCode,
    ScriptDebugInvocationParameters invocationParameters);
}
