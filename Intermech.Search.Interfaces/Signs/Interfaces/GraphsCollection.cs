// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.GraphsCollection
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>Класс, содержащий набор граф</summary>
[Serializable]
public class GraphsCollection : IEnumerable
{
  private SortedList entry = new SortedList();

  /// <summary>Конструктор</summary>
  public GraphsCollection()
  {
  }

  /// <summary>Конструктор</summary>
  /// <param name="collection">Коллекция граф (типа GraphClass)</param>
  public GraphsCollection(GraphsCollection collection)
  {
    foreach (GraphClass graphClass in collection)
      this.entry.Add((object) graphClass.Value, (object) graphClass);
  }

  /// <summary>Удалить объект из списка</summary>
  /// <param name="value">Графа</param>
  public void Remove(GraphClass value) => this.entry.Remove((object) value.Value);

  /// <summary>Удалить объект из списка</summary>
  /// <param name="value">Строковое значение графы</param>
  public void Remove(string value) => this.entry.Remove((object) value);

  /// <summary>Проверить, есть ли объект в списке</summary>
  /// <param name="value">Объект для проверки (либо GraphClass, либо string)</param>
  /// <returns>true - если объект содержится</returns>
  public bool Contains(GraphClass value) => this.entry.ContainsKey((object) value.Value);

  /// <summary>Проверить, есть ли объект в списке</summary>
  /// <param name="value">Объект для проверки (либо GraphClass, либо string)</param>
  /// <returns>true - если объект содержится</returns>
  public bool Contains(string value) => this.entry.ContainsKey((object) value);

  /// <summary>Очистка содержимого</summary>
  public void Clear() => this.entry.Clear();

  /// <summary>Добавить объект к списку</summary>
  /// <param name="value">Объект для добавления</param>
  public void Add(GraphClass value) => this.entry.Add((object) value.Value, (object) value);

  /// <summary>Количество объектов в коллекции</summary>
  public int Count => this.entry.Keys.Count;

  /// <summary>Копировать данные в другой массив</summary>
  /// <param name="array">Массив для копирования (destination)</param>
  /// <param name="index">Помещать в массив начиная с индекса</param>
  public void CopyTo(Array array, int index)
  {
    int num = index;
    foreach (string key in (IEnumerable) this.entry.Keys)
    {
      GraphClass graphClass = this.entry[(object) key] as GraphClass;
      array.SetValue((object) graphClass, num++);
    }
  }

  /// <summary>Получения интерфейса IEnumerator</summary>
  /// <returns></returns>
  public IEnumerator GetEnumerator() => this.entry.Values.GetEnumerator();

  /// <summary>Сохранить информацию в Xml</summary>
  /// <param name="doc">Документ для сохранения</param>
  /// <returns>Сохраненный узел</returns>
  public XmlNode GetXmlNode(XmlDocument doc)
  {
    XmlNode node = doc.CreateNode(XmlNodeType.Element, "SignGraphsCollection", string.Empty);
    foreach (GraphClass graphClass in this)
    {
      XmlNode xmlNode = graphClass.GetXmlNode(doc);
      node.AppendChild(xmlNode);
    }
    return node;
  }

  /// <summary>Загрузить информацию из Xml</summary>
  /// <param name="node">Узел для загрузки</param>
  /// <returns>Загруженный объект - если ок, иначе - null</returns>
  public static GraphsCollection ParseXmlNode(XmlNode node)
  {
    if (!node.Name.Equals("SignGraphsCollection"))
      return (GraphsCollection) null;
    GraphsCollection xmlNode1 = new GraphsCollection();
    foreach (XmlNode childNode in node.ChildNodes)
    {
      GraphClass xmlNode2 = GraphClass.ParseXmlNode(childNode);
      if (xmlNode2 != null)
        xmlNode1.Add(xmlNode2);
    }
    return xmlNode1;
  }

  /// <summary>Получить точную копию коллекции</summary>
  /// <param name="vCollection">Исходная коллекция</param>
  /// <returns>Точная копия коллекции</returns>
  public static GraphsCollection Clone(GraphsCollection vCollection)
  {
    GraphsCollection graphsCollection = new GraphsCollection();
    foreach (GraphClass v in vCollection)
    {
      GraphClass graphClass = GraphClass.Clone(v);
      graphsCollection.entry.Add((object) graphClass.Value, (object) graphClass);
    }
    return graphsCollection;
  }

  /// <summary>Произвести замену значений граф подписей</summary>
  /// <param name="substitutes">Что на что меняем</param>
  /// <returns>Количество замен</returns>
  public int DoSubstitutes(Dictionary<string, string> substitutes)
  {
    int num = 0;
    SortedList sortedList = new SortedList();
    foreach (GraphClass graphClass1 in (IEnumerable) this.entry.Values)
    {
      string str;
      GraphClass graphClass2;
      if (substitutes.TryGetValue(graphClass1.Value, out str))
      {
        graphClass2 = new GraphClass(str, graphClass1.StrongCheck, graphClass1.II);
        ++num;
      }
      else
        graphClass2 = new GraphClass(graphClass1.Value, graphClass1.StrongCheck, graphClass1.II);
      if (!sortedList.ContainsKey((object) graphClass2.Value))
        sortedList.Add((object) graphClass2.Value, (object) graphClass2);
    }
    this.entry = sortedList;
    return num;
  }
}
