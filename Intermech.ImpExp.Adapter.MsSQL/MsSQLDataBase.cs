// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Adapter.MsSQL.MsSQLDataBase
// Assembly: Intermech.ImpExp.Adapter.MsSQL, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: AC488FB0-E7AD-42BA-82F4-B99B0CA102F7
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Adapter.MsSQL.dll

using Intermech.ImpExp.Interface;
using System.Data;
using System.Data.SqlClient;

#nullable disable
namespace Intermech.ImpExp.Adapter.MsSQL;

internal sealed class MsSQLDataBase : SourceDataBase
{
  public override int MaxInOperator => 2000;

  public MsSQLDataBase()
    : base((IDbConnection) new SqlConnection(), MsSQLDataBaseType.DBType)
  {
  }

  protected override TableListCache GetTableListCache()
  {
    return (TableListCache) new MSSQLTableListCache(this.connection);
  }

  public override IDbDataAdapter GetDataAdapter(string sqlText)
  {
    SqlDataAdapter adapter = new SqlDataAdapter(sqlText, (SqlConnection) this.connection);
    SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(adapter);
    return (IDbDataAdapter) adapter;
  }

  public override string GetIntField(string fieldName, string asFieldName)
  {
    return $"{fieldName} as {asFieldName}";
  }
}
