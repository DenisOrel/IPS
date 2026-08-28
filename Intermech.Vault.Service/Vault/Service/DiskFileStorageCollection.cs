// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.DiskFileStorageCollection
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using Intermech.Vault.Interfaces;
using Intermech.Vault.Interfaces.Server;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Vault.Service;

public class DiskFileStorageCollection
{
  public static object SyncRoot = new object();
  private static int connectionID = 1;
  private static Dictionary<int, IDiskFileStorage> StoragesConnections = new Dictionary<int, IDiskFileStorage>();

  public static int AddStorageConnection(IDiskFileStorage addedFileStorage)
  {
    lock (DiskFileStorageCollection.SyncRoot)
    {
      DiskFileStorageCollection.StoragesConnections.Add(DiskFileStorageCollection.connectionID, addedFileStorage);
      return DiskFileStorageCollection.connectionID++;
    }
  }

  public static void DeleteStorageConnection(int connectID)
  {
    lock (DiskFileStorageCollection.SyncRoot)
    {
      if (!DiskFileStorageCollection.StoragesConnections.ContainsKey(connectID))
        return;
      IDiskFileStorage storagesConnection = DiskFileStorageCollection.StoragesConnections[connectID];
      DiskFileStorageCollection.StoragesConnections.Remove(connectID);
    }
  }

  public static bool CheckLogin(int connectID)
  {
    lock (DiskFileStorageCollection.SyncRoot)
      return DiskFileStorageCollection.StoragesConnections.ContainsKey(connectID);
  }

  public static DataTable GetConnections()
  {
    lock (DiskFileStorageCollection.SyncRoot)
    {
      DataTable connections = new DataTable();
      connections.Columns.AddRange(new DataColumn[4]
      {
        new DataColumn("StoragePath"),
        new DataColumn("StorageGuid"),
        new DataColumn("MachineName"),
        new DataColumn("UserName")
      });
      foreach (int key in DiskFileStorageCollection.StoragesConnections.Keys)
        connections.Rows.Add((object) DiskFileStorageCollection.StoragesConnections[key].StoragePath, (object) DiskFileStorageCollection.StoragesConnections[key].StorageGUID, (object) DiskFileStorageCollection.StoragesConnections[key].СomputerName, (object) DiskFileStorageCollection.StoragesConnections[key].UserName);
      return connections;
    }
  }

  public static void RemoveAllConnections()
  {
    lock (DiskFileStorageCollection.SyncRoot)
    {
      DiskFileStorageCollection.connectionID = 0;
      Dictionary<int, IDiskFileStorage> dictionary = new Dictionary<int, IDiskFileStorage>((IDictionary<int, IDiskFileStorage>) DiskFileStorageCollection.StoragesConnections);
      foreach (int key in dictionary.Keys)
        dictionary[key].Logout();
      DiskFileStorageCollection.StoragesConnections.Clear();
    }
  }

  public static bool IsConnectionExists(string storageGuid, string storageName)
  {
    lock (DiskFileStorageCollection.SyncRoot)
    {
      foreach (int key in DiskFileStorageCollection.StoragesConnections.Keys)
      {
        IDiskFileStorage storagesConnection = DiskFileStorageCollection.StoragesConnections[key];
        if (storagesConnection.StorageGUID == storageGuid && storagesConnection.StorageName == storageName)
          return true;
      }
      return false;
    }
  }

  public static void DisconnectByTimeOut()
  {
    lock (DiskFileStorageCollection.SyncRoot)
    {
      Dictionary<int, IDiskFileStorage> dictionary = new Dictionary<int, IDiskFileStorage>((IDictionary<int, IDiskFileStorage>) DiskFileStorageCollection.StoragesConnections);
      foreach (int key in dictionary.Keys)
      {
        IDiskFileStorage diskFileStorage = dictionary[key];
        if (diskFileStorage.InTransaction && DateTime.UtcNow - diskFileStorage.StartTransactionTime > CommonVariables.WAIT_SPAN)
        {
          ApplicationEventLog.Log.InfoFormat(EventStringMessage.TIME_OUT, (object) diskFileStorage.UserName, (object) diskFileStorage.StorageGUID);
          diskFileStorage.Logout();
        }
      }
    }
  }
}
