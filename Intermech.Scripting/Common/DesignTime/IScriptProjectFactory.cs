// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.IScriptProjectFactory
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс сервиса создания новых сценариев.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface IScriptProjectFactory
{
  /// <summary>Создает новый пустой сценарий на указанном языке.</summary>
  /// <param name="languageInfo">Язык сценария</param>
  /// <returns>Объект сценария</returns>
  /// <exception cref="T:System.ArgumentNullException">Параметр <paramref name="languageInfo" /> не должен быть равен null</exception>
  ScriptProject CreateEmptyProject(LanguageInfo languageInfo);

  /// <summary>Создает новый пустой сценарий на указанном языке.</summary>
  /// <param name="fileExtension">Расширение файла сценария</param>
  /// <returns>Объект сценария</returns>
  /// <exception cref="T:System.ArgumentNullException">Параметр <paramref name="fileExtension" /> не должен быть равен null</exception>
  ScriptProject CreateEmptyProject(string fileExtension);
}
