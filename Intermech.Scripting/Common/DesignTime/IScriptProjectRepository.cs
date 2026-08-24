// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.IScriptProjectRepository
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс хранилища сценариев, где единицей хранения является проект, а не отдельный файл.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface IScriptProjectRepository
{
  /// <summary>Возвращает сценарий из хранилища.</summary>
  /// <param name="key">Идентификатор сценария</param>
  /// <returns>Объект сценария</returns>
  /// <exception cref="T:System.ArgumentNullException">Параметр <paramref name="key" /> не должен быть равен null</exception>
  ScriptProject Get(object key);

  /// <summary>Добавляет новый сценарий в хранилище.</summary>
  /// <param name="scriptProject">Объект сценария</param>
  /// <param name="parameters">Параметры добавления сценария</param>
  /// <exception cref="T:System.ArgumentNullException">параметр <paramref name="scriptProject" /> не должен быть равен null; параметр <paramref name="parameters" /> не должен быть равен null</exception>
  void Add(ScriptProject scriptProject, ScriptSaveAsParameters parameters);

  /// <summary>Обновляет сценарий в хранилище.</summary>
  /// <param name="scriptProject">Объект сценария</param>
  /// <returns>Объект сценария</returns>
  /// <exception cref="T:System.ArgumentNullException">Параметр <paramref name="scriptProject" /> не должен быть равен null</exception>
  void Update(ScriptProject scriptProject);
}
