// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IImportedUsers
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Интерфейс на импортированных пользователей</summary>
public interface IImportedUsers
{
  /// <summary>
  /// Получить глобальный идентификатор импортированного пользователя
  /// </summary>
  /// <param name="oldID"></param>
  /// <returns></returns>
  Guid GetGUID(int oldID);

  /// <summary>Получить идентификатор версии объекта в новой системе</summary>
  /// <param name="oldID"></param>
  /// <returns></returns>
  long GetNewKey(int oldID);

  /// <summary>Получить имя пользователя</summary>
  /// <param name="oldID"></param>
  /// <returns></returns>
  string GetUserName(int oldID);

  /// <summary>Добавить пользователя</summary>
  /// <param name="oldID"></param>
  /// <param name="objectID"></param>
  /// <param name="objectTypeID"></param>
  void AddValue(int oldID, long objectID, string caption, Guid objectGuid);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="oldID"></param>
  /// <returns></returns>
  DictionaryValue GetValue(int oldID);

  /// <summary>
  /// 
  /// </summary>
  Dictionary<object, DictionaryValue> Category { get; }
}
