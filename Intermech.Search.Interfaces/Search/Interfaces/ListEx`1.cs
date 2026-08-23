// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Interfaces.ListEx`1
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Search.Interfaces;

/// <summary>Класс оболочка над листом</summary>
/// <typeparam name="T"></typeparam>
public class ListEx<T> : List<T>
{
  /// <summary>Простой конструктор</summary>
  public ListEx()
  {
  }

  /// <summary>Конструктор</summary>
  /// <param name="collection">коллекция</param>
  public ListEx(IEnumerable<T> collection)
    : this(collection, false)
  {
  }

  /// <summary>Конструктор</summary>
  /// <param name="collection">коллекция</param>
  /// <param name="removeDublicate">убирать дублирующиеся элементы</param>
  public ListEx(IEnumerable<T> collection, bool removeDublicate)
  {
    if (removeDublicate)
    {
      foreach (T obj in collection)
      {
        if (!this.Contains(obj))
          this.Add(obj);
      }
    }
    else
      this.AddRange(collection);
  }

  /// <summary>Удалить дублирующиеся элементы</summary>
  public void RemoveDublicateItems()
  {
    List<T> objList = new List<T>((IEnumerable<T>) this.BaseList);
    this.Clear();
    foreach (T obj in objList)
    {
      if (!this.Contains(obj))
        this.Add(obj);
    }
  }

  /// <summary>Доступ к обычному списку</summary>
  public List<T> BaseList => (List<T>) this;

  /// <summary>Убрать дублирующиеся элементы из списка или массим</summary>
  /// <param name="items">список элементов</param>
  /// <returns>список</returns>
  public static List<T> RemoveDublicateItems(IEnumerable<T> items)
  {
    return new ListEx<T>(items, true).BaseList;
  }
}
