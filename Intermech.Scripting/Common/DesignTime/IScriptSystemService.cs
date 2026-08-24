// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.IScriptSystemService
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс сервиса, позволяющего IDE самостоятельно создавать, загружать и сохранять сценарии.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface IScriptSystemService
{
  /// <summary>Создает новый пустой сценарий на указанном языке.</summary>
  /// <param name="languageInfo">Язык сценария</param>
  /// <returns>Сценарный проект</returns>
  /// <exception cref="T:System.ArgumentNullException">Параметр <paramref name="languageInfo" /> не должен быть равен null</exception>
  ScriptProject CreateEmptyProject(LanguageInfo languageInfo);

  /// <summary>
  /// Открывает сценарий из хранилища, предлагая пользователю выбрать сценарий с помощью диалога.
  /// </summary>
  /// <returns>Сценарный проект или null</returns>
  ScriptProject TryOpenScript(ICollection<LanguageInfo> languageFilter);
}
