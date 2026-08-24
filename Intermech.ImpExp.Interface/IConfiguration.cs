// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IConfiguration
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Интерфейс конфигурации</summary>
public interface IConfiguration : IEnumerable<IConfiguration>, IEnumerable
{
  /// <summary>Открыть конфигурацию</summary>
  /// <param name="name">имя конфигурации (возможно путь сразу)</param>
  /// <returns>интерфейс конфигурации</returns>
  IConfiguration Open(string name);

  /// <summary>Открыть конфигурацию</summary>
  /// <param name="name">имя конфигурации (возможно путь сразу)</param>
  /// <param name="viaCreate">создавать</param>
  /// <returns>интерфейс конфигурации</returns>
  IConfiguration Open(string name, bool viaCreate);

  /// <summary>Проверить наличие атрибута</summary>
  /// <param name="name">имя атрибута</param>
  /// <returns>true - если есть</returns>
  bool HasAttribute(string name);

  /// <summary>Получить значение атрибута</summary>
  /// <param name="name">имя атрибута</param>
  /// <returns>значение атрибута</returns>
  string GetAttribute(string name);

  /// <summary>Установить значение атрибута</summary>
  /// <param name="name">имя атрибута</param>
  /// <param name="value">значение</param>
  void SetAttribute(string name, string value);

  /// <summary>
  /// Проверить возможность наличие текста
  /// (только если нет вхождений)
  /// </summary>
  /// <returns>true - если конфигурация может иметь текст</returns>
  bool HasText();

  /// <summary>
  /// Получить текст
  /// (только если нет вхождений)
  /// </summary>
  /// <returns>текст</returns>
  string GetText();

  /// <summary>
  /// Установить текст
  /// (только если нет вхождений)
  /// </summary>
  /// <param name="value">текст</param>
  void SetText(string value);

  /// <summary>
  /// Нод конфигурации
  /// (прямой доступ)
  /// </summary>
  XmlNode Node { get; }
}
