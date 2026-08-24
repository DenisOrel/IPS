// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechConnectionsManager
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.SafeDataProxy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal class TechConnectionsManager
{
  private bool _isConnected;
  private ConnectionSetting _connSett;
  private readonly IMConnection _connectionOpt;
  private readonly PluginClass _plugin;
  private readonly List<IDataBase> _connections = new List<IDataBase>();
  private readonly IDictionary<string, bool> _tableNameCache = (IDictionary<string, bool>) new ConcurrentDictionary<string, bool>();

  private IDataBase GetExistFreeConnection()
  {
    if (this._connectionOpt.DataBaseType != "IntermechConnection.MsSQL")
      return this.CountConnections != 0 ? this._connections[0] : (IDataBase) null;
    foreach (IDataBase connection in this._connections)
    {
      if (connection != null && connection.DbConnection.State != ConnectionState.Open)
      {
        int state = (int) connection.DbConnection.State;
        if (connection.DbConnection.State == ConnectionState.Closed)
        {
          connection.DbConnection.Open();
          if (connection.DbConnection.State != ConnectionState.Open)
            continue;
        }
        return connection;
      }
    }
    return (IDataBase) null;
  }

  private IDataBase CreateNewConnection()
  {
    if (this._plugin == null)
      return (IDataBase) null;
    if (!this._isConnected)
    {
      if (!this._plugin.BaseConnect())
        return (IDataBase) null;
      this._isConnected = true;
      this._connSett = SavedConnectionStrings.Items["IMBASE"];
    }
    try
    {
      SafeDataBaseProxy newConnection = new SafeDataBaseProxy(this._plugin.appManager.DBManager.GetDbType(this._connectionOpt.DataBaseType).GetNewDataBase(), (ISafeProxyErrorHandler) new ImpExpErrorHandler(TechcardConsts.Plugin.appManager));
      IDbConnection dbConnection = newConnection.DbConnection;
      dbConnection.ConnectionString = this._connSett.ConnectionString;
      dbConnection.Open();
      return (IDataBase) newConnection;
    }
    catch (Exception ex)
    {
      this._plugin.appManager.AddErrorMessage($"Невозможно подключиться к базе: {ex.Message}");
      return (IDataBase) null;
    }
  }

  public TechConnectionsManager(PluginClass thisplugin)
  {
    this._plugin = thisplugin;
    this._connectionOpt = new IMConnection(ConnStrType.Imbase);
  }

  public IDbCommand CreateCommand()
  {
    IDbCommand command = this.GetConnection().CreateCommand();
    command.CommandTimeout = 0;
    return command;
  }

  public bool IsTableExists(string tableName)
  {
    bool flag;
    if (this._tableNameCache.TryGetValue(tableName, out flag))
      return flag;
    try
    {
      flag = this._plugin.idb.TableExists(tableName);
      if (!this._tableNameCache.ContainsKey(tableName))
        this._tableNameCache.Add(tableName, flag);
    }
    catch (Exception ex)
    {
      this._plugin.appManager.AddErrorMessage($"Невозможно получить сведения о существовании таблицы \"{tableName}\": {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    return flag;
  }

  public string DataBaseType => this._connectionOpt.DataBaseType;

  public int CountConnections => this._connections.Count;

  public IDataBase GetConnection()
  {
    IDataBase connection = this.GetExistFreeConnection();
    if (connection == null)
    {
      connection = this.CreateNewConnection();
      this._connections.Add(connection);
    }
    return connection;
  }

  public CommandBehavior CommandBehavior
  {
    get
    {
      CommandBehavior commandBehavior = CommandBehavior.Default;
      switch (this.DataBaseType)
      {
        case "IntermechConnection.MsSQL":
          commandBehavior = CommandBehavior.CloseConnection;
          break;
      }
      return commandBehavior;
    }
  }
}
