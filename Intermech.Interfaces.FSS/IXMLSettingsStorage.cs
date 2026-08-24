// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.FSS.IXMLSettingsStorage
// Assembly: Intermech.Interfaces.FSS, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 89C13FA3-8295-4BAF-985C-14C35172BA6B
// Assembly location: D:\IPS\Client\Intermech.Interfaces.FSS.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.FSS.xml

using System;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.Interfaces.FSS;

/// <summary>XML-хранилище настроек</summary>
public interface IXMLSettingsStorage
{
  /// <summary>Документ XML, в котором хранятся настройки</summary>
  XmlDocument document { get; set; }

  /// <summary>Сохранить все настройки в указанный файл</summary>
  /// <param name="FileName">Имя файла, в который требуется сохранять настройки</param>
  /// <returns>true, если сохранение прошло успешно</returns>
  bool Save(string FileName);

  /// <summary>Сохранить все настройки в указанный поток</summary>
  /// <param name="stream">Поток, в которой будут сохранены настройки</param>
  /// <returns>true, если сохранение прошло успешно</returns>
  bool Save(Stream stream);

  /// <summary>Загрузить все настройки из указанного файла</summary>
  /// <param name="FileName">Имя файла, из которого будут загружены настройки</param>
  /// <returns>true, если загрузка прошла успешно</returns>
  bool Load(string FileName);

  /// <summary>Загрузить все настройки из указанного потока</summary>
  /// <param name="stream">Поток, из которого будут загружены настройки</param>
  /// <returns>true, если загрузка прошла успешно</returns>
  bool Load(Stream stream);

  /// <summary>Создать дочерний узел в составе родительского</summary>
  /// <param name="parentNode">Родительский узел</param>
  /// <param name="childName">Имя нового дочернего узла</param>
  /// <returns>Дочерний узел или null</returns>
  XmlNode AddNode(XmlNode parentNode, string childName);

  /// <summary>
  /// Отыскать дочерний узел в составе родительского. При необходимости создать его.
  /// </summary>
  /// <param name="parentNode">Родительский узел</param>
  /// <param name="childName">Имя разыскиваемого дочернего узла</param>
  /// <param name="autoCreate">Если true, то создать дочерний узел, если он не был найден</param>
  /// <returns>Дочерний узел или null</returns>
  XmlNode FindNode(XmlNode parentNode, string childName, bool autoCreate);

  /// <summary>
  /// Отыскать дочерний узел в составе родительского. Для поиска требуется название дочернего узла,
  /// название его атрибута и значение этого атрибута (для поиска среди множества одноименных узлов с общим по
  /// имени атрибутом)
  /// </summary>
  /// <param name="parentNode">Родительский узел</param>
  /// <param name="childName">Имя разыскиваемого дочернего узла</param>
  /// <param name="attrName">Название ключевого атрибута</param>
  /// <param name="attrValue">Значение ключевого атрибута</param>
  /// <param name="autoCreate">Если true, то создать дочерний узел, если он не был найден</param>
  /// <returns>Дочерний узел или null</returns>
  XmlNode FindNodeWithAttr(
    XmlNode parentNode,
    string childName,
    string attrName,
    string attrValue,
    bool autoCreate);

  /// <summary>
  /// Получить текущее значение атрибута. Если атрибута нет - вернётся значение по умолчанию
  /// </summary>
  /// <param name="node">Узел - владелец атрибута</param>
  /// <param name="attrName">Имя атрибута</param>
  /// <param name="defValue">Значение атрибута по умолчанию (
  /// будет возвращено, если атрибут не найден у указанного узла)</param>
  /// <returns>Ссылка на атрибут</returns>
  string GetAttributeValue(XmlNode node, string attrName, string defValue);

  /// <summary>
  /// Получить текущее значение атрибута в виде Guid. Если атрибута нет - вернётся значение по умолчанию
  /// </summary>
  /// <param name="node">Узел - владелец атрибута</param>
  /// <param name="attrName">Имя атрибута</param>
  /// <param name="defValue">Значение атрибута по умолчанию
  /// (будет возвращено, если атрибут не найден у указанного узла, либо если в атрибуте хранится не Guid)</param>
  /// <returns>Ссылка на атрибут</returns>
  Guid GetAttributeAsGuid(XmlNode node, string attrName, Guid defValue);

  /// <summary>
  /// Получить текущее значение атрибута в виде Int32. Если атрибута нет - вернётся значение по умолчанию
  /// </summary>
  /// <param name="node">Узел - владелец атрибута</param>
  /// <param name="attrName">Имя атрибута</param>
  /// <param name="defValue">Значение атрибута по умолчанию
  /// (будет возвращено, если атрибут не найден у указанного узла, либо если в атрибуте хранится не Int32)</param>
  /// <returns>Ссылка на атрибут</returns>
  int GetAttributeAsInt32(XmlNode node, string attrName, int defValue);

  /// <summary>
  /// Получить текущее значение атрибута в виде Int64. Если атрибута нет - вернётся значение по умолчанию
  /// </summary>
  /// <param name="node">Узел - владелец атрибута</param>
  /// <param name="attrName">Имя атрибута</param>
  /// <param name="defValue">Значение атрибута по умолчанию
  /// (будет возвращено, если атрибут не найден у указанного узла, либо если в атрибуте хранится не Int64)</param>
  /// <returns>Ссылка на атрибут</returns>
  long GetAttributeAsInt64(XmlNode node, string attrName, long defValue);

  /// <summary>
  /// Установить новое значение для атрибута. Атрибут создаётся, если его нет в коллекции
  /// </summary>
  /// <param name="node">Узел - владелец атрибута</param>
  /// <param name="attrName">Имя атрибута</param>
  /// <param name="Value">Значение атрибута</param>
  /// <returns>Ссылка на атрибут</returns>
  void SetAttributeValue(XmlNode node, string attrName, string Value);
}
