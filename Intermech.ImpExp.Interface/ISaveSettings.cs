// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ISaveSettings
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Интерфейс на сервис сохранения настроек для классов-памперов
/// </summary>
public interface ISaveSettings
{
  /// <summary>Получить ранее сохраненную конфигурацию</summary>
  /// <param name="settingsName">Имя конфигурации( уникальное в пределах всех классов-памперов)</param>
  /// <returns></returns>
  Dictionary<string, SaveSettingsAttribute[]> GetSettings(string settingsName);

  /// <summary>Сохранить конфигурацию</summary>
  /// <param name="settingsName">Имя конфигурации( уникальное в пределах всех классов-памперов)</param>
  /// <param name="settings">Конфигурация</param>
  void SetSettings(
    string settingsName,
    Dictionary<string, SaveSettingsAttribute[]> settings);

  /// <summary>Очистить конфигурацию</summary>
  /// <param name="settingsName"></param>
  void ClearSettings(string settingsName);

  /// <summary>Время последней записи конфигурации</summary>
  DateTime SettingsDateTime { get; }
}
