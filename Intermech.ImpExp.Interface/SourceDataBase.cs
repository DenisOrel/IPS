// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SourceDataBase
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Data;

#nullable disable
namespace Intermech.ImpExp.Interface;

public abstract class SourceDataBase : IDataBase
{
  protected string dataBaseType;
  protected IDbConnection connection;
  protected TableListCache tableListCache;

  public SourceDataBase(IDbConnection connection, string dataBaseType)
  {
    this.connection = connection;
    this.dataBaseType = dataBaseType;
  }

  public string DataBaseType => this.dataBaseType;

  public IDbConnection DbConnection => this.connection;

  public IDbCommand CreateCommand() => this.PrepareCommand(this.connection.CreateCommand());

  protected virtual IDbCommand PrepareCommand(IDbCommand command) => command;

  public IDataReader GetDataReader(string sqlText)
  {
    IDbCommand command = this.CreateCommand();
    command.CommandText = sqlText;
    return command.ExecuteReader();
  }

  public bool TableExists(string tableName)
  {
    if (this.tableListCache == null)
      this.tableListCache = this.GetTableListCache();
    return this.tableListCache.TableExists(tableName);
  }

  protected abstract TableListCache GetTableListCache();

  public abstract IDbDataAdapter GetDataAdapter(string sqlText);

  public abstract string GetIntField(string fieldName, string asFieldName);

  public virtual void OnAfterConnect()
  {
  }

  public abstract int MaxInOperator { get; }
}
