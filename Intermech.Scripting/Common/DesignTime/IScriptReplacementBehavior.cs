// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.IScriptReplacementBehavior
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс поведения сценариев во время выполнения команды "Заменить" другим сценарием.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface IScriptReplacementBehavior
{
  /// <summary>
  /// Выбирает из хранилища другой сценарий для замены текущего сценария в IDE.
  /// </summary>
  /// <returns>Сценарный проект или null</returns>
  ScriptProject TryGetAnotherScriptProject();

  /// <summary>
  /// Обработчик события, вызывающийся после успешной замены текущего сценария в IDE.
  /// </summary>
  /// <param name="anotherScriptProject">Сценарный проект, на который была выполнена замена</param>
  /// <exception cref="T:System.ArgumentNullException">Параметр <paramref name="anotherScriptProject" /> не должен быть равен null</exception>
  void AfterReplace(ScriptProject anotherScriptProject);
}
