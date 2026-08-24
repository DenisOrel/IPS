// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.XmlUtils
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Xml;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class XmlUtils
{
  /// <summary>
  /// Добавление атрибута (и его значения) к указанному узлу
  /// </summary>
  /// <param name="xmlNode">Узел XML-документа</param>
  /// <param name="attrName">Название атрибута</param>
  /// <param name="attrValue">Значение атрибута</param>
  public static void AddXmlAtrubute(XmlNode xmlNode, string attrName, string attrValue)
  {
    XmlAttribute attribute = xmlNode.OwnerDocument.CreateAttribute(attrName);
    attribute.Value = attrValue;
    xmlNode.Attributes.Append(attribute);
  }

  /// <summary>Получение значения атрибута у указанного узла</summary>
  /// <param name="xmlNode">Узел XML-документа</param>
  /// <param name="attributeName">Название атрибута</param>
  /// <returns>Значение атрибута</returns>
  public static string GetXmlAttributeValue(XmlNode xmlNode, string attributeName)
  {
    XmlAttribute attribute = xmlNode.Attributes[attributeName];
    return attribute != null ? attribute.Value : "";
  }
}
