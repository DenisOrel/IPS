// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ILanguageSessionService
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс сервиса языковых сессий. Он отвечает за создание новых языковых сессий.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface ILanguageSessionService
{
  /// <summary>
  /// Возвращает параметры по умолчанию для создания языковых сессий.
  /// </summary>
  ILanguageSessionParameters CreateSessionParameters();

  /// <summary>
  /// Загружает параметры создания языковой сессии из контейнера настроек.
  /// </summary>
  /// <param name="container">Контейнер настроек</param>
  /// <returns>Параметры создания языковой сессии</returns>
  /// <exception cref="T:System.ArgumentNullException">параметр <paramref name="container" /> не должен быть равен null</exception>
  ILanguageSessionParameters LoadSessionParameters(ISettingsContainer container);

  /// <summary>
  /// Загружает параметры создания языковой сессии из контейнера настроек.
  /// </summary>
  /// <param name="container">Контейнер настроек</param>
  /// <param name="parameters">Параметры создания языковой сессии</param>
  /// <exception cref="T:System.ArgumentNullException">параметр <paramref name="container" /> не должен быть равен null; параметр <paramref name="parameters" /> не должен быть равен null</exception>
  void SaveSessionParameters(ISettingsContainer container, ILanguageSessionParameters parameters);

  /// <summary>
  /// Позволяет изменить параметры по умолчанию для создания языковых сессий.
  /// </summary>
  /// <param name="parameters">Параметры сессии</param>
  /// <returns>Признак, что параметры были изменены, и языковая сессия должна быть пересоздана</returns>
  /// <exception cref="T:System.ArgumentNullException">Параметр <paramref name="parameters" /> не должен быть равен null</exception>
  bool EditSessionParameters(ILanguageSessionParameters parameters);

  /// <summary>Создает новую языковую сессию.</summary>
  /// <param name="parameters">Параметры сессии</param>
  /// <returns>Объект сессии</returns>
  /// <exception cref="T:System.ArgumentNullException">Параметр <paramref name="parameters" /> не должен быть равен null</exception>
  ILanguageSession CreateSession(ILanguageSessionParameters parameters);
}
