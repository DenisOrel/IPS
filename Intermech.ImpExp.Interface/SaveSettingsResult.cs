// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SaveSettingsResult
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Результат сохранения данных на шаге настройки</summary>
public enum SaveSettingsResult
{
  /// <summary>Критический сбой при сохранении</summary>
  [Description("Критический сбой при сохранении")] ssrError = -1, // 0xFFFFFFFF
  /// <summary>Не сохранено</summary>
  [Description("Не сохранено")] ssrRetry = 0,
  /// <summary>Успешно сохранено</summary>
  [Description("Успешно сохранено")] ssrOk = 1,
  /// <summary>
  /// Для сообщения главной формы при установленном флажке не качать данные
  /// </summary>
  [Description("Прервано после закачки памперов с метаданными")] ssrMetadataTerminate = 2,
}
