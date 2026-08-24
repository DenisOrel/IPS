// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.ITypeItemList`1
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

/// <summary>Интерфейс для списка типов</summary>
public interface ITypeItemList<T> : 
  IList<T>,
  ICollection<T>,
  IEnumerable<T>,
  IEnumerable,
  IList,
  ICollection
{
  /// <summary>Получение элемента типа по идентификатору типа</summary>
  /// <param name="id">Идентификатор типа</param>
  /// <returns>Элемент типа</returns>
  T GetByID(int id);

  /// <summary>
  /// Получение элемента типа по глобальному идентификатору типа
  /// </summary>
  /// <param name="guid">Глобальный идентификатор типа</param>
  /// <returns>Элемент типа</returns>
  T GetByGuid(Guid guid);

  /// <summary>Получение элемента типа по наименованию типа</summary>
  /// <param name="name">Наименование типа</param>
  /// <returns>Элемент типа</returns>
  T GetByName(string name);

  /// <summary>Получение массива всех элементов типа</summary>
  /// <returns>Массив всех элементов типа</returns>
  T[] GetItems();

  /// <summary>
  /// Генерация следующего идентификатора для экземпляра типа (нужна при создании нового типа)
  /// </summary>
  /// <returns>Новый идентификатор</returns>
  int GenNextID();

  /// <summary>Проверка наличия элемента типа по идентификатору</summary>
  /// <param name="id">Идентификатор типа</param>
  /// <returns>Если элемент типа с заданным идентификатором существует - true, иначе - false</returns>
  bool ExistsById(int id);

  /// <summary>
  /// Проверка наличия элемента типа по глобальному идентификатору
  /// </summary>
  /// <param name="guid">Глобальный идентификатор типа</param>
  /// <returns>Если элемент типа с заданным глобальным идентификатором существует - true, иначе - false</returns>
  bool ExistsByGuid(Guid guid);

  /// <summary>Проверка наличия элемента типа по наименованию</summary>
  /// <param name="name">Наименование типа</param>
  /// <returns>Если элемент типа с заданным наименованием существует - true, иначе - false</returns>
  bool ExistsByName(string name);
}
