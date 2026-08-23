// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.GraphsSet
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>
/// Класс, содержащий набор коллекций граф. Настраивается на "Типы объектов + шаг жизненного цикла",
/// или на "Уровни продвижения", или на "Архивы"
/// </summary>
[Serializable]
public class GraphsSet
{
  private string stepID;
  private List<string> keys = new List<string>();
  private Hashtable values = new Hashtable();

  /// <summary>Guid шага жизненного цикла</summary>
  public string StepID
  {
    get => this.stepID;
    set => this.stepID = value;
  }

  /// <summary>Получение значения по ключу</summary>
  public GraphsCollection this[string key]
  {
    get => this.values[(object) key] as GraphsCollection;
    set => this.values[(object) key] = (object) value;
  }

  /// <summary>Удалить объект с ключом</summary>
  /// <param name="key">Заголовок группы</param>
  public void Remove(string key)
  {
    this.keys.Remove(key);
    this.values.Remove((object) key);
  }

  /// <summary>Проверка, содержится ли объект с ключом</summary>
  /// <param name="key">Заголовок группы</param>
  /// <returns>true, если содержиться, false - иначе</returns>
  public bool Contains(string key) => this.keys.Contains(key);

  /// <summary>Очистить все</summary>
  public void Clear()
  {
    this.keys.Clear();
    this.values.Clear();
  }

  /// <summary>Получить коллекцию значений</summary>
  public ICollection Values => this.values.Values;

  /// <summary>Добавить значение по ключу</summary>
  /// <param name="key">Объект-ключ</param>
  /// <param name="value">Значение</param>
  public void Add(string key, GraphsCollection value)
  {
    this.values.Add((object) key, (object) value);
    this.keys.Add(key);
  }

  /// <summary>Добавить значение</summary>
  /// <param name="gSet">Набор значений</param>
  public void Add(GraphsSet gSet)
  {
    foreach (string g in gSet)
    {
      if (this.keys.Contains(g))
        this.values[(object) g] = (object) gSet[g];
      else
        this.Add(g, gSet[g]);
    }
  }

  /// <summary>Получить коллекцию объектов-ключей</summary>
  public ICollection Keys => (ICollection) this.keys;

  /// <summary>Количество объектов-ключей</summary>
  public int Count => this.keys.Count;

  /// <summary>Копирование объектов-значений в массив</summary>
  /// <param name="array">Массив для копирования (destination)</param>
  /// <param name="index">Индекс, с которого начинается вставка в массив</param>
  public void CopyTo(Array array, int index)
  {
    int num = index;
    foreach (object key in this.keys)
    {
      object obj = this.values[key];
      array.SetValue(obj, num++);
    }
  }

  /// <summary>Интерфейс IEnumerator</summary>
  /// <returns></returns>
  public IEnumerator GetEnumerator() => (IEnumerator) this.keys.GetEnumerator();

  /// <summary>Сохранение в поток</summary>
  /// <param name="destination">Поток-назначение</param>
  public void Save(Stream destination)
  {
    XmlDocument doc = new XmlDocument();
    XmlNode node1 = doc.CreateNode(XmlNodeType.XmlDeclaration, "xml", string.Empty);
    doc.AppendChild(node1);
    XmlNode node2 = doc.CreateNode(XmlNodeType.Element, "Intermech.Signs", string.Empty);
    doc.AppendChild(node2);
    foreach (object key in this.keys)
    {
      object obj = this.values[key];
      if (obj is GraphsCollection && key is string)
      {
        XmlNode xmlNode = (obj as GraphsCollection).GetXmlNode(doc);
        XmlAttribute attribute = doc.CreateAttribute("Caption");
        attribute.Value = key.ToString();
        xmlNode.Attributes.Append(attribute);
        node2.AppendChild(xmlNode);
      }
    }
    doc.Save(destination);
  }

  /// <summary>Загрузка из потока</summary>
  /// <param name="source">Поток с исходными данными</param>
  /// <returns>Класс GraphSet</returns>
  public static GraphsSet Load(Stream source)
  {
    source.Position = 0L;
    GraphsSet graphsSet = new GraphsSet();
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.Load(source);
    foreach (XmlNode childNode1 in xmlDocument.ChildNodes)
    {
      if (childNode1.Name.Equals("Intermech.Signs"))
      {
        foreach (XmlNode childNode2 in childNode1.ChildNodes)
        {
          GraphsCollection xmlNode = GraphsCollection.ParseXmlNode(childNode2);
          if (xmlNode != null)
          {
            XmlAttribute attribute = childNode2.Attributes["Caption"];
            graphsSet.Add(attribute.Value, xmlNode);
          }
        }
      }
    }
    return graphsSet;
  }

  /// <summary>Получение точной копии класса</summary>
  /// <param name="vSet">Исходные данные</param>
  /// <returns>Точная копия исходных данных</returns>
  public static GraphsSet Clone(GraphsSet vSet)
  {
    GraphsSet graphsSet = new GraphsSet();
    foreach (string key in vSet.keys)
    {
      GraphsCollection graphsCollection = GraphsCollection.Clone(vSet[key]);
      graphsSet.Add(key, graphsCollection);
    }
    return graphsSet;
  }

  /// <summary>Заменяет значения граф подписей</summary>
  /// <param name="substitutes">Что на что меняяем</param>
  /// <returns>Количество замен</returns>
  public int DoSubstitutes(Dictionary<string, string> substitutes)
  {
    int num = 0;
    foreach (object key in this.keys)
    {
      if (this.values[key] is GraphsCollection graphsCollection)
        num += graphsCollection.DoSubstitutes(substitutes);
    }
    return num;
  }
}
