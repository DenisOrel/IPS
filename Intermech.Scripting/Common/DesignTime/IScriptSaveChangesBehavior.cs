// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.IScriptSaveChangesBehavior
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс поведения сценариев во время загрузки/сохранения в IDE.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface IScriptSaveChangesBehavior
{
  /// <summary>
  /// Обработчик сохранения новых сценариев, а также существующих сценариев с новым именем.
  /// Метод должен запросить у пользователя необходимые параметры и вернуть их в виде контейнера.
  /// Пользователь может отказаться от сохранения сценария, в этом случае метод должен вернуть null.
  /// </summary>
  /// <returns>Параметры сохранения сценария или null</returns>
  ScriptSaveAsParameters TrySaveAs();

  /// <summary>
  /// Обработчик события, вызывающегося перед сохранением изменений.
  /// Метод вызывается и для новых, и для измененных существующих сценариев.
  /// </summary>
  /// <param name="e">Аргументы события</param>
  void BeforeSave(ScriptBeforeSaveEventArgs e);

  /// <summary>
  /// Обработчик события, вызывающегося после успешного сохранения изменений.
  /// Метод вызывается и для новых, и для измененных существующих сценариев.
  /// </summary>
  /// <param name="e">Аргументы события</param>
  void AfterSave(ScriptAfterSaveEventArgs e);
}
