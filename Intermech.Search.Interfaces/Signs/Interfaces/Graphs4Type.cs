// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.Graphs4Type
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Interfaces;
using Intermech.Search.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>
/// Настройка граф для типов объектов. Настраивается на "Должности".
/// </summary>
public class Graphs4Type : IEnumerable
{
  private SortedList _types = new SortedList();
  private SortedList _cache = new SortedList();
  private Dictionary<string, string> _possibleGraphs;

  /// <summary>Конструктор</summary>
  /// <param name="possibleGraphs">Доступные графы для подписи</param>
  public Graphs4Type(Dictionary<string, string> possibleGraphs)
  {
    if (possibleGraphs == null)
      return;
    this._possibleGraphs = new Dictionary<string, string>((IDictionary<string, string>) possibleGraphs);
  }

  /// <summary>Конструктор</summary>
  /// <param name="sourceStream">Поток для загрузки из XML</param>
  /// <param name="possibleGraphs">Доступные графы для подписи</param>
  public Graphs4Type(Stream sourceStream, Dictionary<string, string> possibleGraphs)
  {
    if (possibleGraphs != null)
      this._possibleGraphs = new Dictionary<string, string>((IDictionary<string, string>) possibleGraphs);
    sourceStream.Position = 0L;
    XmlDocument parent = new XmlDocument();
    parent.Load(sourceStream);
    this.ParseXml((XmlNode) parent);
  }

  private int[] GetObjectTypeChildrens(IUserSession session, int objectType)
  {
    List<int> intList = new List<int>();
    DataTable dataTable = session.GetObjectTypeCollection(objectType).Select(string.Empty, (object[]) null);
    if (dataTable != null)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        int int32 = Convert.ToInt32(row["F_OBJECT_TYPE"]);
        if (!intList.Contains(int32))
          intList.Add(int32);
      }
    }
    return intList.ToArray();
  }

  /// <summary>Разбор xml в структуры</summary>
  /// <param name="parent">XmlNode для разбора</param>
  private void ParseXml(XmlNode parent)
  {
    foreach (XmlNode childNode in parent.ChildNodes)
    {
      if (childNode.Name.Equals("Intermech.Signs"))
        this.ParseXml(childNode);
      if (childNode.Name.Equals("ObjectType"))
      {
        XmlAttribute attribute = childNode.Attributes["ID"];
        Guid empty = Guid.Empty;
        int key = -1;
        Guid objTypeGuid;
        try
        {
          objTypeGuid = new Guid(attribute.Value);
        }
        catch
        {
          objTypeGuid = Guid.Empty;
        }
        if (!objTypeGuid.Equals(Guid.Empty))
          key = MetaDataHelper.GetObjectTypeID(objTypeGuid);
        if (key != -1)
        {
          List<string> stringList = ListEx<string>.RemoveDublicateItems((IEnumerable<string>) new List<string>((IEnumerable<string>) childNode.Attributes["Graphs"].Value.Split(';')));
          this._types.Add((object) key, (object) stringList);
        }
      }
    }
  }

  /// <summary>Сохранение данных в поток</summary>
  /// <param name="destStream">Поток для сохранения</param>
  /// <param name="session">юзерская сессия</param>
  public void Save(Stream destStream, IUserSession session)
  {
    XmlDocument xmlDocument = new XmlDocument();
    XmlNode element1 = (XmlNode) xmlDocument.CreateElement("Intermech.Signs");
    xmlDocument.AppendChild(element1);
    foreach (int key in (IEnumerable) this._types.Keys)
    {
      List<string> type = this._types[(object) key] as List<string>;
      XmlNode element2 = (XmlNode) xmlDocument.CreateElement("ObjectType");
      XmlAttribute attribute1 = xmlDocument.CreateAttribute("ID");
      IDBObjectType objectType = session.GetObjectType(key);
      attribute1.Value = (objectType as IDBGuid).GUID.ToString();
      element2.Attributes.Append(attribute1);
      XmlAttribute attribute2 = xmlDocument.CreateAttribute("Graphs");
      attribute2.Value = string.Join(";", type.ToArray());
      element2.Attributes.Append(attribute2);
      element1.AppendChild(element2);
    }
    xmlDocument.Save(destStream);
  }

  /// <summary>Добавление данных в класс</summary>
  /// <param name="objectType">Тип объекта</param>
  /// <param name="graph">Графа для подписи</param>
  public void Add(int objectType, string graph)
  {
    if (this._possibleGraphs != null && !this._possibleGraphs.ContainsKey(graph))
      return;
    List<string> stringList = new List<string>();
    if (this._types.ContainsKey((object) objectType))
      stringList = this._types[(object) objectType] as List<string>;
    if (!stringList.Contains(graph))
      stringList.Add(graph);
    if (!this._types.ContainsKey((object) objectType))
      this._types.Add((object) objectType, (object) stringList);
    this._cache.Clear();
  }

  /// <summary>Добавление данных в класс</summary>
  /// <param name="objectType">Тип объекта</param>
  /// <param name="graphCollection">Коллекция граф для подписи</param>
  public void Add(int objectType, ICollection graphCollection)
  {
    foreach (string graph in (IEnumerable) graphCollection)
      this.Add(objectType, graph);
  }

  /// <summary>Убрать данные из класса</summary>
  /// <param name="objectType">Тип объекта</param>
  /// <param name="graph">Графа для подписи</param>
  public void Remove(int objectType, string graph)
  {
    if (!this._types.ContainsKey((object) objectType))
      return;
    List<string> type = this._types[(object) objectType] as List<string>;
    type.Remove(graph);
    if (type.Count.Equals(0))
      this._types.Remove((object) objectType);
    this._cache.Clear();
  }

  /// <summary>Убрать данные из класса</summary>
  /// <param name="objectType">Тип объекта</param>
  public void Remove(int objectType)
  {
    this._types.Remove((object) objectType);
    this._cache.Clear();
  }

  /// <summary>Очистить класс</summary>
  public void Clear()
  {
    this._types.Clear();
    this._cache.Clear();
  }

  /// <summary>Получить класс связки для типа объектов</summary>
  /// <param name="session">Сессия</param>
  /// <param name="objectType">Тип объекта</param>
  /// <param name="parently">Просматривать родительские типы объектов</param>
  /// <returns>Класс, содержащий связку [тип объекта] - [графы для подписи]</returns>
  public Graphs4TypeStruct GetGraphs4ObjectType(
    IUserSession session,
    int objectType,
    bool parently)
  {
    ListEx<string> graphs = new ListEx<string>();
    if (this._cache.ContainsKey((object) objectType))
      return new Graphs4TypeStruct(objectType, this._cache[(object) objectType] as List<string>);
    if (this._types.ContainsKey((object) objectType))
      graphs.AddRange((IEnumerable<string>) (this._types[(object) objectType] as List<string>));
    if (parently)
    {
      int num = objectType;
      while (num >= 0)
      {
        num = MetaDataHelper.GetObjectTypeParentID(num);
        Graphs4TypeStruct graphs4ObjectType = this.GetGraphs4ObjectType(session, num, true);
        graphs.AddRange((IEnumerable<string>) graphs4ObjectType.Graphs);
      }
    }
    graphs.RemoveDublicateItems();
    this._cache[(object) objectType] = (object) graphs.BaseList;
    return new Graphs4TypeStruct(objectType, (List<string>) graphs);
  }

  /// <summary>Возвращает интерфейс IEnumerator для класса</summary>
  /// <returns></returns>
  public IEnumerator GetEnumerator() => this._types.Keys.GetEnumerator();

  public int DoSubstitutes(Dictionary<string, string> substitutes)
  {
    int num = 0;
    SortedList sortedList = new SortedList();
    foreach (int key1 in (IEnumerable) this._types.Keys)
    {
      if (this._types[(object) key1] is List<string> type)
      {
        List<string> stringList = new List<string>();
        foreach (string key2 in type)
        {
          string str;
          if (substitutes.TryGetValue(key2, out str))
          {
            stringList.Add(str);
            ++num;
          }
          else
            stringList.Add(key2);
        }
        sortedList.Add((object) key1, (object) stringList);
      }
    }
    this._types = sortedList;
    return num;
  }
}
