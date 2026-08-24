// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TableListCache
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Кэш таблиц БД</summary>
public abstract class TableListCache
{
  protected IDbConnection connection;
  protected string alltTablesSelectSQL;
  /// <summary>Список таблиц имеющихся в базе</summary>
  protected List<string> tablesList;

  public TableListCache(IDbConnection connection, string alltTablesSelectSQL)
  {
    this.connection = connection;
    this.alltTablesSelectSQL = alltTablesSelectSQL;
  }

  public bool TableExists(string tableName)
  {
    if (this.tablesList == null)
    {
      this.tablesList = new List<string>();
      this.LoadTablesList(this.connection.CreateCommand());
    }
    return this.tablesList.Exists((Predicate<string>) (x => x.Equals(tableName.ToUpper())));
  }

  private void LoadTablesList(IDbCommand command)
  {
    command.CommandText = this.alltTablesSelectSQL;
    IDataReader dataReader = command.ExecuteReader();
    if (dataReader == null)
      return;
    try
    {
      while (dataReader.Read())
        this.tablesList.Add(dataReader.GetString(0).ToUpper());
    }
    finally
    {
      dataReader.Close();
    }
  }
}
