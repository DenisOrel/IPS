// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.SignsServerUsersCache
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>кэш для получения guid пользователя по его id</summary>
public static class SignsServerUsersCache
{
  /// <summary>для синхронизации</summary>
  private static object SyncObject = new object();
  /// <summary>словарик guid пользователя - его id</summary>
  private static Dictionary<long, string> UsersCache = new Dictionary<long, string>();

  /// <summary>Загружаем всю информацию обо всех пользователях</summary>
  /// <param name="session">сессия</param>
  public static void LoadUsersInfo(IUserSession session)
  {
    DataTable dataTable = session.GetObjectCollection(new Guid("cad00002-306c-11d8-b4e9-00304f19f545")).Select(new DBRecordSetParams(new ConditionStructure[0], new object[2]
    {
      (object) -2,
      (object) -12
    }));
    lock (SignsServerUsersCache.SyncObject)
    {
      SignsServerUsersCache.UsersCache.Clear();
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        long int64 = Convert.ToInt64(row[0]);
        string lower = row[1].ToString().ToLower();
        if (!SignsServerUsersCache.UsersCache.ContainsKey(int64))
          SignsServerUsersCache.UsersCache.Add(int64, lower);
      }
    }
  }

  /// <summary>получить guid пользователя по его id-ку</summary>
  /// <param name="userID"></param>
  /// <returns></returns>
  public static string GetUserGuid(long userID)
  {
    lock (SignsServerUsersCache.SyncObject)
    {
      if (SignsServerUsersCache.UsersCache.ContainsKey(userID))
        return SignsServerUsersCache.UsersCache[userID];
    }
    return string.Empty;
  }

  /// <summary>добавить пользвоателя в кэш</summary>
  /// <param name="userId"> id пользователя</param>
  /// <param name="userGuid">guid пользователя </param>
  public static void AddUser(long userId, Guid userGuid)
  {
    lock (SignsServerUsersCache.SyncObject)
    {
      if (SignsServerUsersCache.UsersCache.ContainsKey(userId))
        return;
      SignsServerUsersCache.UsersCache.Add(userId, userGuid.ToString());
    }
  }

  /// <summary>
  /// удалить информацию о пользователе из кэша
  /// (пользователи всё равно не удаляются...)
  /// </summary>
  /// <param name="userId"> id пользователя</param>
  public static void RemoveUser(long userId)
  {
    lock (SignsServerUsersCache.SyncObject)
    {
      if (SignsServerUsersCache.UsersCache.ContainsKey(userId))
        return;
      SignsServerUsersCache.UsersCache.Remove(userId);
    }
  }
}
