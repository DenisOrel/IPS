// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IPlugin
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.Controls;
using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Интерфейс модулей расширения программы перекачки данных
/// </summary>
public interface IPlugin
{
  /// <summary>Описание модуля расширения</summary>
  string Description { get; }

  /// <summary>Название модуля расширения</summary>
  string Name { get; }

  /// <summary>Идентификатор плагина</summary>
  Guid GUID { get; }

  /// <summary>
  /// Получение элемента управления с настройками параметров для перекачки
  /// </summary>
  /// <returns>Элемент управления с настройками параметров для перекачки</returns>
  StepControl[] GetSettingsControls();

  /// <summary>Получение списка задач верификации перекачки</summary>
  /// <returns>Список задач веривикации (инициализации) данных</returns>
  IPumpTask[] GetVerifications();

  /// <summary>Получение списка задач перекачки данных</summary>
  /// <returns>Список задач перекачки данных</returns>
  IPumpTask[] GetPumps();

  /// <summary>Получение списка задач перекачки данных</summary>
  /// <returns>Список задач перекачки данных</returns>
  IPumpTask[] GetFinalPumps();

  /// <summary>Инициализация переменных для перекачки</summary>
  /// <returns>Если инициализация прошла успешно - true, иначе - false</returns>
  bool InitSettings();

  /// <summary>Перекачка данных</summary>
  /// <returns>Если перекачка прошла успешно - true, иначе - false</returns>
  bool Execute();

  /// <summary>Проерка подключен ли модуль расширения к базе</summary>
  /// <returns>Результат проверки</returns>
  bool IsConnected();

  /// <summary>Подключение к базе</summary>
  /// <returns></returns>
  bool BaseConnect();

  /// <summary>Отключение от базы</summary>
  /// <returns></returns>
  bool BaseDisconnect();

  /// <summary>Информация о подключении к БД</summary>
  string[] ConnectInfo { get; }

  /// <summary>Путь к плагину</summary>
  string Location { get; }
}
