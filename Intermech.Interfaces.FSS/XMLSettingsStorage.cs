// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.FSS.XMLSettingsStorage
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
public class XMLSettingsStorage : IXMLSettingsStorage
{
  /// <summary>Документ XML, в котором хранятся настройки</summary>
  private XmlDocument _document = new XmlDocument();

  /// <summary>Создать пустой экземпляр настроек</summary>
  public XMLSettingsStorage()
  {
    this._document.LoadXml("<?xml version='1.0' encoding='utf-8' ?>\n<IPS.FSS.V1 />\n");
  }

  /// <summary>Создать экземпляр настроек на основе указанного файла</summary>
  /// <param name="FileName">Имя файла, из которого будут загружены настройки</param>
  public XMLSettingsStorage(string FileName)
  {
    this._document.LoadXml("<?xml version='1.0' encoding='utf-8' ?>\n<IPS.FSS.V1 />\n");
    this.Load(FileName);
  }

  /// <summary>
  /// Создать экземпляр настроек на основе указанного потока
  /// </summary>
  /// <param name="stream">Поток, из которого будут загружены настройки</param>
  public XMLSettingsStorage(Stream stream)
  {
    this._document.LoadXml("<?xml version='1.0' encoding='utf-8' ?>\n<IPS.FSS.V1 />\n");
    this.Load(stream);
  }

  /// <summary>Документ XML, в котором хранятся настройки</summary>
  public XmlDocument document
  {
    get => this._document;
    set
    {
      if (value == null || this._document == value || value.DocumentElement == null || value.DocumentElement.Name != "IPS.FSS.V1")
        return;
      this._document = value;
    }
  }

  /// <summary>Сохранить все настройки в указанный файл</summary>
  /// <param name="FileName">Имя файла, в который требуется сохранять настройки</param>
  /// <returns>true, если сохранение прошло успешно</returns>
  public bool Save(string FileName)
  {
    try
    {
      FileStream fileStream = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite);
      try
      {
        return this.Save((Stream) fileStream);
      }
      finally
      {
        fileStream.Close();
        fileStream.Dispose();
      }
    }
    catch
    {
      return false;
    }
  }

  /// <summary>Сохранить все настройки в указанный поток</summary>
  /// <param name="stream">Поток, в которой будут сохранены настройки</param>
  /// <returns>true, если сохранение прошло успешно</returns>
  public bool Save(Stream stream)
  {
    if (stream == null)
      return false;
    try
    {
      this._document.Save(stream);
    }
    catch
    {
      return false;
    }
    return true;
  }

  /// <summary>Загрузить все настройки из указанного файла</summary>
  /// <param name="FileName">Имя файла, из которого будут загружены настройки</param>
  /// <returns>true, если загрузка прошла успешно</returns>
  public bool Load(string FileName)
  {
    if (!new FileInfo(FileName).Exists)
      return false;
    try
    {
      FileStream fileStream = new FileStream(FileName, FileMode.Open);
      try
      {
        return this.Load((Stream) fileStream);
      }
      finally
      {
        fileStream.Close();
        fileStream.Dispose();
      }
    }
    catch
    {
      return false;
    }
  }

  /// <summary>Загрузить все настройки из указанного потока</summary>
  /// <param name="stream">Поток, из которого будут загружены настройки</param>
  /// <returns>true, если загрузка прошла успешно</returns>
  public bool Load(Stream stream)
  {
    if (stream != null && stream.Length != 0L)
    {
      if (stream.Position != stream.Length - 1L)
      {
        try
        {
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.LoadXml("<?xml version='1.0' encoding='utf-8' ?>\n<IPS.FSS.V1 />\n");
          xmlDocument.Load(stream);
          if (xmlDocument.DocumentElement == null || xmlDocument.DocumentElement.Name != "IPS.FSS.V1")
            return false;
          this._document = xmlDocument;
        }
        catch
        {
          return false;
        }
        return true;
      }
    }
    return false;
  }

  /// <summary>Создать дочерний узел в составе родительского</summary>
  /// <param name="parentNode">Родительский узел</param>
  /// <param name="childName">Имя нового дочернего узла</param>
  /// <returns>Дочерний узел или null</returns>
  public XmlNode AddNode(XmlNode parentNode, string childName)
  {
    if (parentNode == null || childName == string.Empty)
      return (XmlNode) null;
    XmlElement element = parentNode.OwnerDocument.CreateElement(childName);
    return parentNode.AppendChild((XmlNode) element);
  }

  /// <summary>
  /// Отыскать дочерний узел в составе родительского. При необходимости создать его.
  /// </summary>
  /// <param name="parentNode">Родительский узел</param>
  /// <param name="childName">Имя разыскиваемого дочернего узла</param>
  /// <param name="autoCreate">Если true, то создать дочерний узел, если он не был найден</param>
  /// <returns>Дочерний узел или null</returns>
  public XmlNode FindNode(XmlNode parentNode, string childName, bool autoCreate)
  {
    if (parentNode == null || childName == string.Empty)
      return (XmlNode) null;
    for (int i = 0; i < parentNode.ChildNodes.Count; ++i)
    {
      if (!(parentNode.ChildNodes[i].Name != childName))
        return parentNode.ChildNodes[i];
    }
    if (!autoCreate)
      return (XmlNode) null;
    XmlElement element = parentNode.OwnerDocument.CreateElement(childName);
    return parentNode.AppendChild((XmlNode) element);
  }

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
  public XmlNode FindNodeWithAttr(
    XmlNode parentNode,
    string childName,
    string attrName,
    string attrValue,
    bool autoCreate)
  {
    if (parentNode == null || childName == string.Empty)
      return (XmlNode) null;
    for (int i = 0; i < parentNode.ChildNodes.Count; ++i)
    {
      XmlNode childNode = parentNode.ChildNodes[i];
      if (!(childNode.Name != childName))
      {
        XmlNode namedItem = childNode.Attributes.GetNamedItem(attrName);
        if (namedItem != null && namedItem.InnerText == attrValue)
          return childNode;
      }
    }
    if (!autoCreate)
      return (XmlNode) null;
    XmlElement element = parentNode.OwnerDocument.CreateElement(childName);
    XmlNode node = parentNode.AppendChild((XmlNode) element);
    this.SetAttributeValue(node, attrName, attrValue);
    return node;
  }

  /// <summary>
  /// Получить текущее значение атрибута. Если атрибута нет - вернётся значение по умолчанию
  /// </summary>
  /// <param name="node">Узел - владелец атрибута</param>
  /// <param name="attrName">Имя атрибута</param>
  /// <param name="defValue">Значение атрибута по умолчанию (
  /// будет возвращено, если атрибут не найден у указанного узла)</param>
  /// <returns>Ссылка на атрибут</returns>
  public string GetAttributeValue(XmlNode node, string attrName, string defValue)
  {
    if (node == null || attrName == string.Empty || node.Attributes.Count == 0)
      return defValue;
    XmlNode namedItem = node.Attributes.GetNamedItem(attrName);
    return namedItem == null ? defValue : namedItem.InnerText;
  }

  /// <summary>
  /// Получить текущее значение атрибута в виде Guid. Если атрибута нет - вернётся значение по умолчанию
  /// </summary>
  /// <param name="node">Узел - владелец атрибута</param>
  /// <param name="attrName">Имя атрибута</param>
  /// <param name="defValue">Значение атрибута по умолчанию
  /// (будет возвращено, если атрибут не найден у указанного узла, либо если в атрибуте хранится не Guid)</param>
  /// <returns>Ссылка на атрибут</returns>
  public Guid GetAttributeAsGuid(XmlNode node, string attrName, Guid defValue)
  {
    if (node == null || attrName == string.Empty || node.Attributes.Count == 0)
      return defValue;
    XmlNode namedItem = node.Attributes.GetNamedItem(attrName);
    if (namedItem == null)
      return defValue;
    try
    {
      return new Guid(namedItem.InnerText);
    }
    catch
    {
      return defValue;
    }
  }

  /// <summary>
  /// Получить текущее значение атрибута в виде Int32. Если атрибута нет - вернётся значение по умолчанию
  /// </summary>
  /// <param name="node">Узел - владелец атрибута</param>
  /// <param name="attrName">Имя атрибута</param>
  /// <param name="defValue">Значение атрибута по умолчанию
  /// (будет возвращено, если атрибут не найден у указанного узла, либо если в атрибуте хранится не Int32)</param>
  /// <returns>Ссылка на атрибут</returns>
  public int GetAttributeAsInt32(XmlNode node, string attrName, int defValue)
  {
    if (node == null || attrName == string.Empty || node.Attributes.Count == 0)
      return defValue;
    XmlNode namedItem = node.Attributes.GetNamedItem(attrName);
    if (namedItem == null)
      return defValue;
    int result = defValue;
    return !int.TryParse(namedItem.InnerText, out result) ? defValue : result;
  }

  /// <summary>
  /// Получить текущее значение атрибута в виде Int64. Если атрибута нет - вернётся значение по умолчанию
  /// </summary>
  /// <param name="node">Узел - владелец атрибута</param>
  /// <param name="attrName">Имя атрибута</param>
  /// <param name="defValue">Значение атрибута по умолчанию
  /// (будет возвращено, если атрибут не найден у указанного узла, либо если в атрибуте хранится не Int64)</param>
  /// <returns>Ссылка на атрибут</returns>
  public long GetAttributeAsInt64(XmlNode node, string attrName, long defValue)
  {
    if (node == null || attrName == string.Empty || node.Attributes.Count == 0)
      return defValue;
    XmlNode namedItem = node.Attributes.GetNamedItem(attrName);
    if (namedItem == null)
      return defValue;
    long result = defValue;
    return !long.TryParse(namedItem.InnerText, out result) ? defValue : result;
  }

  /// <summary>
  /// Установить новое значение для атрибута. Атрибут создаётся, если его нет в коллекции
  /// </summary>
  /// <param name="node">Узел - владелец атрибута</param>
  /// <param name="attrName">Имя атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <returns>Ссылка на атрибут</returns>
  public void SetAttributeValue(XmlNode node, string attrName, string value)
  {
    if (node == null || attrName == string.Empty)
      return;
    XmlNode namedItem = node.Attributes.GetNamedItem(attrName);
    if (namedItem != null)
    {
      namedItem.InnerText = value;
    }
    else
    {
      XmlAttribute attribute = node.OwnerDocument.CreateAttribute(attrName);
      attribute.InnerText = value;
      node.Attributes.Append(attribute);
    }
  }
}
