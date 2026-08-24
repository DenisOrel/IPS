// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Adapter.Oracle.OracleDataBase
// Assembly: Intermech.ImpExp.Adapter.Oracle, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D553EB52-5206-4E60-A4A5-05A894FA883B
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Adapter.Oracle.dll

using Intermech.ImpExp.Interface;
using Oracle.ManagedDataAccess.Client;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Adapter.Oracle;

internal sealed class OracleDataBase : SourceDataBase
{
  public override int MaxInOperator => 1000;

  public OracleDataBase()
    : base((IDbConnection) new OracleConnection(), OracleDataBaseType.DBType)
  {
  }

  protected override TableListCache GetTableListCache()
  {
    return (TableListCache) new OracleTableListCache(this.connection);
  }

  public override IDbDataAdapter GetDataAdapter(string sqlText)
  {
    OracleDataAdapter dataAdapter = new OracleDataAdapter(sqlText, (OracleConnection) this.connection);
    OracleCommandBuilder oracleCommandBuilder = new OracleCommandBuilder(dataAdapter);
    return (IDbDataAdapter) dataAdapter;
  }

  protected override IDbCommand PrepareCommand(IDbCommand command)
  {
    (command as OracleCommand).BindByName = true;
    (command as OracleCommand).FetchSize = 262144L /*0x040000*/;
    (command as OracleCommand).InitialLOBFetchSize = 262144 /*0x040000*/;
    (command as OracleCommand).InitialLONGFetchSize = -1;
    return command;
  }

  public override string GetIntField(string fieldName, string asFieldName)
  {
    return $"NVL({fieldName},0) as {asFieldName}";
  }

  public override void OnAfterConnect()
  {
    using (IDbCommand command = this.connection.CreateCommand())
    {
      command.CommandText = "ALTER SESSION SET SKIP_UNUSABLE_INDEXES=true";
      command.ExecuteNonQuery();
    }
  }
}
