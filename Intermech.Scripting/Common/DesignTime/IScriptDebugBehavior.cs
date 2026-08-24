// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.IScriptDebugBehavior
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс поведения сценариев во время отладки в IDE.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface IScriptDebugBehavior
{
  /// <summary>
  /// Позволяет изменить аргументы сценария, передаваемые ему во время выполнения.
  /// </summary>
  void EditArguments();

  /// <summary>Выполняет сценарий.</summary>
  /// <param name="languageSession">Языковая сессия исполнителя</param>
  /// <param name="scriptCode">Код сценария</param>
  /// <returns>Результат выполнения сценария</returns>
  /// <exception cref="T:System.ArgumentNullException">Параметр <paramref name="languageSession" /> не должен быть равен null; параметр <paramref name="scriptCode" /> не должен быть равен null</exception>
  ScriptDebugInvocationResult Execute(ILanguageSession languageSession, string scriptCode);
}
