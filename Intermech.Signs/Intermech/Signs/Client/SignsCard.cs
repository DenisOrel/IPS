// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignsCard
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Signs.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>
/// Класс для хранения "Карточки подписей пользователя"
/// (Куприянчик по анализу кода) Хранит соответствие, в какой должности какие типы объектов можно подписывать
/// </summary>
public class SignsCard
{
  private SortedList _RanksToGraph4Type = new SortedList();
  private SortedList _ObjectTypeToGraphs = new SortedList();
  private SortedList _ObjectTypeToRanks = new SortedList();

  /// <summary>Добавление записи в карточку подписей</summary>
  /// <param name="rankID">Должность</param>
  /// <param name="info">информация на графы из объекта "Должность"</param>
  public void Add(long rankID, Graphs4Type info)
  {
    this._RanksToGraph4Type[(object) rankID] = (object) info;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (int num in info)
      {
        Graphs4TypeStruct graphs4ObjectType = info.GetGraphs4ObjectType(sessionKeeper.Session, num, true);
        if (this._ObjectTypeToGraphs.ContainsKey((object) num))
        {
          ArrayList objectTypeToGraph = this._ObjectTypeToGraphs[(object) num] as ArrayList;
          foreach (string graph in graphs4ObjectType.Graphs)
          {
            if (!objectTypeToGraph.Contains((object) graph))
              objectTypeToGraph.Add((object) graph);
          }
        }
        else
          this._ObjectTypeToGraphs[(object) num] = (object) new ArrayList((ICollection) graphs4ObjectType.Graphs);
        if (this._ObjectTypeToRanks.ContainsKey((object) num))
        {
          ArrayList objectTypeToRank = this._ObjectTypeToRanks[(object) num] as ArrayList;
          if (!objectTypeToRank.Contains((object) rankID))
            objectTypeToRank.Add((object) rankID);
        }
        else
          this._ObjectTypeToRanks[(object) num] = (object) new ArrayList((ICollection) new long[1]
          {
            rankID
          });
      }
    }
  }

  private void GetParentGraphs(int objectType, bool parently, List<string> list)
  {
    if (this._ObjectTypeToGraphs.ContainsKey((object) objectType))
    {
      foreach (object obj in this._ObjectTypeToGraphs[(object) objectType] as ArrayList)
      {
        if (!list.Contains(Convert.ToString(obj)) && SignsCache.PossibleGraphs.ContainsKey(Convert.ToString(obj)))
          list.Add(Convert.ToString(obj));
      }
    }
    if (!parently)
      return;
    int num = objectType;
    while (num >= 0)
    {
      num = MetaDataHelper.GetObjectTypeParentID(num);
      this.GetParentGraphs(num, false, list);
    }
  }

  private void GetParentRanks(int objectType, bool parently, List<long> list)
  {
    if (this._ObjectTypeToRanks.ContainsKey((object) objectType))
    {
      foreach (object obj in this._ObjectTypeToRanks[(object) objectType] as ArrayList)
      {
        if (!list.Contains((long) Convert.ToInt32(obj)))
          list.Add((long) Convert.ToInt32(obj));
      }
    }
    if (!parently)
      return;
    int num = objectType;
    while (num >= 0)
    {
      num = MetaDataHelper.GetObjectTypeParentID(num);
      this.GetParentRanks(num, false, list);
    }
  }

  /// <summary>
  /// Получение полного неповторяющегося списка граф для данного пользователя
  /// </summary>
  /// <returns>Список граф для подписи.</returns>
  public List<string> GetGraphs()
  {
    List<string> graphs = new List<string>();
    IList valueList = this._ObjectTypeToGraphs.GetValueList();
    for (int index = 0; index < valueList.Count; ++index)
    {
      foreach (object obj in valueList[index] as ArrayList)
      {
        if (!graphs.Contains(Convert.ToString(obj)) && SignsCache.PossibleGraphs.ContainsKey(Convert.ToString(obj)))
          graphs.Add(Convert.ToString(obj));
      }
    }
    return graphs;
  }

  /// <summary>Получить графы для подписей для объекта типа</summary>
  /// <param name="rankID">Идентификатор должности пользователя</param>
  /// <param name="objectType">Тип объекта</param>
  /// <returns>Графы для подписей</returns>
  public List<string> GetGraphs(long rankID, int objectType)
  {
    if (!(this._RanksToGraph4Type[(object) rankID] is Graphs4Type graphs4Type))
      return new List<string>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return graphs4Type.GetGraphs4ObjectType(sessionKeeper.Session, objectType, true).Graphs;
  }

  /// <summary>Получить графы для подписей для объекта типа</summary>
  /// <param name="rankID">Идентификатор должности пользователя</param>
  /// <param name="typedObjectID">Тип объекта</param>
  /// <returns>Графы для подписей</returns>
  public List<string> GetGraphs(long rankID, IDBTypedObjectID typedObjectID)
  {
    return this.GetGraphs(rankID, typedObjectID.ObjectType);
  }

  /// <summary>
  /// Получение списка граф для "Должности и "Типов объектов"
  /// </summary>
  /// <param name="rankID">Должность</param>
  /// <param name="objectTypes">Типы объектов</param>
  /// <returns>Графы для подписей</returns>
  public List<string> GetGraphs(long rankID, List<int> objectTypes)
  {
    List<string> graphs1 = new List<string>();
    foreach (int objectType in objectTypes)
    {
      List<string> graphs2 = this.GetGraphs(rankID, objectType);
      if (graphs1.Count.Equals(0))
      {
        graphs1.AddRange((IEnumerable<string>) graphs2);
      }
      else
      {
        List<string> range = graphs1.GetRange(0, graphs1.Count);
        List<string> stringList = new List<string>((IEnumerable<string>) graphs2);
        foreach (string str in range)
        {
          if (!stringList.Contains(str))
            graphs1.Remove(str);
        }
      }
    }
    return graphs1;
  }

  /// <summary>
  /// Получение списка граф для "Должности и "Типов объектов"
  /// </summary>
  /// <param name="rankID">Должность</param>
  /// <param name="typedObjectIDs">Типы объектов</param>
  /// <returns>Графы для подписей</returns>
  public List<string> GetGraphs(long rankID, List<IDBTypedObjectID> typedObjectIDs)
  {
    List<string> graphs1 = new List<string>();
    foreach (IDBTypedObjectID typedObjectId in typedObjectIDs)
    {
      List<string> graphs2 = this.GetGraphs(rankID, typedObjectId);
      if (graphs1.Count.Equals(0))
      {
        graphs1.AddRange((IEnumerable<string>) graphs2);
      }
      else
      {
        List<string> range = graphs1.GetRange(0, graphs1.Count);
        List<string> stringList = new List<string>((IEnumerable<string>) graphs2);
        foreach (string str in range)
        {
          if (!stringList.Contains(str))
            graphs1.Remove(str);
        }
      }
    }
    return graphs1;
  }

  /// <summary>Получение списка граф для "Типа объекта"</summary>
  /// <param name="objectType">Тип объекта</param>
  /// <returns>Список граф для подписи</returns>
  public List<string> GetGraphs(int objectType)
  {
    List<string> list = new List<string>();
    this.GetParentGraphs(objectType, true, list);
    return list;
  }

  /// <summary>Получение списка граф для "Типа объекта"</summary>
  /// <param name="typedObjectID">Тип объекта</param>
  /// <returns>Список граф для подписи</returns>
  public List<string> GetGraphs(IDBTypedObjectID typedObjectID)
  {
    return this.GetGraphs(typedObjectID.ObjectType);
  }

  /// <summary>Получение списка граф для "Типов объектов"</summary>
  /// <param name="objectTypes">Типы объектов</param>
  /// <returns>Список граф для подписи</returns>
  public List<string> GetGraphs(List<int> objectTypes)
  {
    List<string> graphs1 = new List<string>();
    foreach (int objectType in objectTypes)
    {
      List<string> graphs2 = this.GetGraphs(objectType);
      if (graphs1.Count.Equals(0))
      {
        graphs1.AddRange((IEnumerable<string>) graphs2);
      }
      else
      {
        foreach (string str in graphs1.GetRange(0, graphs1.Count))
        {
          if (!graphs2.Contains(str))
            graphs1.Remove(str);
        }
      }
    }
    return graphs1;
  }

  /// <summary>Получение списка граф для "Типов объектов"</summary>
  /// <param name="typedObjectIDs">Типы объектов</param>
  /// <returns>Список граф для подписи</returns>
  public List<string> GetGraphs(List<IDBTypedObjectID> typedObjectIDs)
  {
    List<string> graphs1 = new List<string>();
    foreach (IDBTypedObjectID typedObjectId in typedObjectIDs)
    {
      List<string> graphs2 = this.GetGraphs(typedObjectId);
      if (graphs1.Count.Equals(0))
      {
        graphs1.AddRange((IEnumerable<string>) graphs2);
      }
      else
      {
        foreach (string str in graphs1.GetRange(0, graphs1.Count))
        {
          if (!graphs2.Contains(str))
            graphs1.Remove(str);
        }
      }
    }
    return graphs1;
  }

  /// <summary>Получение "Должностей" по "Типу объекта"</summary>
  /// <param name="objectType">Тип объекта</param>
  /// <returns>Должности</returns>
  public List<long> GetRanks(int objectType)
  {
    List<long> list = new List<long>();
    this.GetParentRanks(objectType, true, list);
    return list;
  }

  /// <summary>Получение "Должностей" по "Типу объекта"</summary>
  /// <param name="typedObjectID">Тип объекта</param>
  /// <returns>Должности</returns>
  public List<long> GetRanks(IDBTypedObjectID typedObjectID)
  {
    return this.GetRanks(typedObjectID.ObjectType);
  }

  /// <summary>Получение "Должностей" по "Типам объектов"</summary>
  /// <param name="objectTypes">Типы объектов</param>
  /// <returns>Должности</returns>
  public List<long> GetRanks(List<int> objectTypes)
  {
    List<long> ranks = new List<long>();
    foreach (int objectType in objectTypes)
    {
      foreach (long rank in this.GetRanks(objectType))
      {
        if (!ranks.Contains(rank))
          ranks.Add(rank);
      }
    }
    return ranks;
  }

  /// <summary>Получение "Должностей" по "Типам объектов"</summary>
  /// <param name="typedObjectIDs">Типы объектов</param>
  /// <returns>Должности</returns>
  public List<long> GetRanks(List<IDBTypedObjectID> typedObjectIDs)
  {
    List<long> ranks = new List<long>();
    foreach (IDBTypedObjectID typedObjectId in typedObjectIDs)
    {
      foreach (long rank in this.GetRanks(typedObjectId))
      {
        if (!ranks.Contains(rank))
          ranks.Add(rank);
      }
    }
    return ranks;
  }

  /// <summary>Получение всех должностей</summary>
  /// <returns>Список ID Должностей</returns>
  public IList<long> GetRanks() => this._RanksToGraph4Type.Keys as IList<long>;

  /// <summary>
  /// может ли пользователь подписывать
  /// выбранные типы объектов
  /// </summary>
  /// <param name="typedObjectIDs"></param>
  /// <param name="rankIDs"> должности в которых пользователь может подписать
  /// выбранные типы объектов</param>
  /// <returns></returns>
  public bool IsUserCanSign(List<IDBTypedObjectID> typedObjectIDs, out List<long> rankIDs)
  {
    rankIDs = this.GetRanks(typedObjectIDs);
    foreach (long rankID in rankIDs.GetRange(0, rankIDs.Count))
    {
      if (this.GetGraphs(rankID, typedObjectIDs).Count == 0)
        rankIDs.Remove(rankID);
    }
    return rankIDs.Count != 0;
  }
}
