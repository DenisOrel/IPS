// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.ExtConnections
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using Intermech.Vault.Interfaces;
using Intermech.Vault.Interfaces.Server;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

#nullable disable
namespace Intermech.Vault.Service;

internal class ExtConnections
{
  private string volumeDbName = "volume.db3";
  private Dictionary<string, SQLiteConnection> connections = new Dictionary<string, SQLiteConnection>();
  private Dictionary<string, SQLiteTransaction> transactions = new Dictionary<string, SQLiteTransaction>();

  public void CloseConnections()
  {
    foreach (SQLiteConnection sqLiteConnection in this.connections.Values)
      sqLiteConnection?.Close();
    this.transactions.Clear();
    this.connections.Clear();
  }

  public void CreateConnection(string dbPath)
  {
    string str1 = Path.Combine(dbPath, this.volumeDbName);
    bool flag = File.Exists(str1);
    if (this.connections.ContainsKey(dbPath))
      return;
    string str2 = "Data Source=" + str1;
    if (CommonVariables.SyncModeOff)
      str2 += ";synchronous=off";
    SQLiteConnection connection = (SQLiteConnection) DbProviderFactories.GetFactory("System.Data.SQLite").CreateConnection();
    connection.ConnectionString = str2;
    if (!flag)
    {
      SQLiteConnection.CreateFile(str1);
      StorageSecurity.RemoveFileDeleteRights(str1);
    }
    connection.Open();
    if (!flag)
    {
      using (SQLiteCommand command = connection.CreateCommand())
      {
        command.CommandText = SQLCommands.CreateFilesTable;
        command.ExecuteNonQuery();
      }
    }
    this.transactions.Add(dbPath, connection.BeginTransaction());
    this.connections.Add(dbPath, connection);
  }

  public SQLiteCommand CreateCommand(string dbPath, string commandText)
  {
    SQLiteCommand command = this.connections[dbPath].CreateCommand();
    command.CommandText = commandText;
    if (this.transactions[dbPath] != null)
      command.Transaction = this.transactions[dbPath];
    return command;
  }

  public SQLiteCommand CreateCommand(
    string dbPath,
    string commandText,
    SQLiteParameter[] parameters)
  {
    SQLiteCommand command = this.CreateCommand(dbPath, commandText);
    command.Parameters.Clear();
    if (parameters != null)
    {
      foreach (SQLiteParameter parameter in parameters)
        command.Parameters.Add(parameter);
    }
    ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_60"), (object) dbPath);
    return command;
  }

  public void CommitTransactions()
  {
    foreach (string key in this.transactions.Keys)
    {
      this.transactions[key].Commit();
      ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_61"), (object) key);
    }
  }

  public void RollbackTransactions()
  {
    foreach (string key in this.transactions.Keys)
    {
      this.transactions[key].Rollback();
      ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_62"), (object) key);
    }
  }
}
