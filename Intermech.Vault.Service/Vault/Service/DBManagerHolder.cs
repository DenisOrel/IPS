// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.DBManagerHolder
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.Vault.Service;

public static class DBManagerHolder
{
  private static object SyncRoot = new object();
  private static Dictionary<string, DBManager> managerHashtable = new Dictionary<string, DBManager>();

  public static DBManager CreateConnection(string dbPath)
  {
    lock (DBManagerHolder.SyncRoot)
    {
      string key = Path.GetFileNameWithoutExtension(dbPath) + Directory.GetParent(dbPath).Name;
      if (DBManagerHolder.managerHashtable.ContainsKey(key))
      {
        ++DBManagerHolder.managerHashtable[key].СonnCounter;
        return DBManagerHolder.managerHashtable[key];
      }
      DBManager connection = new DBManager(dbPath);
      DBManagerHolder.managerHashtable.Add(key, connection);
      return connection;
    }
  }

  public static void RemoveConnection(string fileStoreGuid, string storageName)
  {
    lock (DBManagerHolder.SyncRoot)
    {
      string key = fileStoreGuid + storageName;
      if (!DBManagerHolder.managerHashtable.ContainsKey(key) || --DBManagerHolder.managerHashtable[key].СonnCounter != 0)
        return;
      DBManagerHolder.managerHashtable.Remove(key);
    }
  }
}
