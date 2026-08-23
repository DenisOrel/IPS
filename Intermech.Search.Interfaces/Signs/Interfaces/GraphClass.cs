// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.GraphClass
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System;
using System.Xml;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>Класс описывающий графу для подписи</summary>
[Serializable]
public class GraphClass
{
  private string _value = string.Empty;
  private bool _strongCheck;
  private bool _II;

  /// <summary>Конструктор</summary>
  /// <param name="value">Значение графы</param>
  public GraphClass(string value) => this._value = value;

  /// <summary>Конструктор</summary>
  /// <param name="value">значение графы</param>
  /// <param name="strongCheck">строгая проверка графы</param>
  /// <param name="II">извещение</param>
  public GraphClass(string value, bool strongCheck, bool II)
    : this(value)
  {
    this._strongCheck = strongCheck;
    this._II = II;
  }

  /// <summary>Значение графы</summary>
  public string Value => this._value;

  /// <summary>
  /// Использует ли графа строгий контроль (контроль даты изменения содержимого и даты подписи)
  /// </summary>
  public bool StrongCheck
  {
    get => this._strongCheck;
    set => this._strongCheck = value;
  }

  /// <summary>Выпускать извещение</summary>
  public bool II
  {
    get => this._II;
    set => this._II = value;
  }

  /// <summary>Проверка на равенство объектов</summary>
  /// <param name="obj">объект для сравнения</param>
  /// <returns>True если равны, False - иначе</returns>
  public override bool Equals(object obj)
  {
    switch (obj)
    {
      case GraphClass _:
        return base.Equals(obj);
      case string _:
        return (obj as string).Equals(this._value);
      default:
        return false;
    }
  }

  /// <summary>Получение хэш-кода объекта</summary>
  /// <returns>хэш-код объекта</returns>
  public override int GetHashCode() => base.GetHashCode();

  /// <summary>Сохранение в XmlNode</summary>
  /// <param name="doc">Xml документ</param>
  /// <returns>созданный XmlNode</returns>
  public XmlNode GetXmlNode(XmlDocument doc)
  {
    XmlNode node = doc.CreateNode(XmlNodeType.Element, "SignGraph", string.Empty);
    XmlAttribute attribute1 = doc.CreateAttribute("Value");
    XmlAttribute attribute2 = doc.CreateAttribute("StrongCheck");
    XmlAttribute attribute3 = doc.CreateAttribute("II");
    attribute1.Value = this._value;
    attribute2.Value = this._strongCheck.ToString();
    attribute3.Value = this._II.ToString();
    node.Attributes.Append(attribute1);
    node.Attributes.Append(attribute2);
    node.Attributes.Append(attribute3);
    return node;
  }

  /// <summary>Загрузка из XmlNode'а</summary>
  /// <param name="node">Сам XmlNode</param>
  /// <returns>Если все ок - то класс, иначе - null</returns>
  public static GraphClass ParseXmlNode(XmlNode node)
  {
    if (node.Name.Equals("SignGraph"))
    {
      XmlAttribute attribute1 = node.Attributes["Value"];
      XmlAttribute attribute2 = node.Attributes["StrongCheck"];
      XmlAttribute attribute3 = node.Attributes["II"];
      if (attribute1 != null && attribute2 != null && attribute3 != null)
        return new GraphClass(attribute1.Value, Convert.ToBoolean(attribute2.Value), Convert.ToBoolean(attribute3.Value));
    }
    return (GraphClass) null;
  }

  /// <summary>Клонирование класса</summary>
  /// <param name="vClass">Исходный класс</param>
  /// <returns>Копированный класс</returns>
  public static GraphClass Clone(GraphClass vClass)
  {
    return new GraphClass(vClass.Value)
    {
      _value = vClass._value,
      _II = vClass._II,
      _strongCheck = vClass._strongCheck
    };
  }
}
