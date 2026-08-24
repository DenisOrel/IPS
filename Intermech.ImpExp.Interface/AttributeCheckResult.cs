// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.AttributeCheckResult
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Результат проверки соответствия типа данных поля таблицы типу данных атрибута, в который предполагается перекачка
/// </summary>
public enum AttributeCheckResult
{
  /// <summary>Перенос информации доступен</summary>
  [Description("Перенос информации доступен")] cresOk,
  /// <summary>Перенос информации может потребовать усечения данных</summary>
  [Description("Перенос информации может потребовать усечения данных")] cresCut,
  /// <summary>
  /// При переносе инвормации нужно произести конвертацию данных
  /// </summary>
  [Description("При переносе информации нужно произести конвертацию данных")] cresConvert,
  /// <summary>
  /// Выбранный атрибут не допускает перенос информации текущего типа
  /// </summary>
  [Description("Выбранный атрибут не допускает перенос информации текущего типа")] cresError,
  /// <summary>Выбранный атрибут может привести к потере данных</summary>
  [Description("Выбранный атрибут может привести к потере данных")] cresLost,
  /// <summary>Выбранный атрибут может привести к потере данных</summary>
  [Description("Выбранный атрибут не допускает перенос значений в текущей физической величине")] cresPhysVal,
}
