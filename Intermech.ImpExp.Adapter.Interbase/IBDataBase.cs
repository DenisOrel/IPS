// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Adapter.Interbase.IBDataBase
// Assembly: Intermech.ImpExp.Adapter.Interbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B97FBD89-71A5-4417-A5DC-2CB918616870
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Adapter.Interbase.dll

using Intermech.ImpExp.Interface;
using System.Data;
using System.Data.OleDb;

#nullable disable
namespace Intermech.ImpExp.Adapter.Interbase;

internal sealed class IBDataBase : SourceDataBase
{
  public override int MaxInOperator => 1500;

  public IBDataBase()
    : base((IDbConnection) new OleDbConnection(), "IntermechConnection.Interbase")
  {
  }

  protected override TableListCache GetTableListCache()
  {
    return (TableListCache) new IBTableListCache(this.connection);
  }

  public override IDbDataAdapter GetDataAdapter(string sqlText)
  {
    OleDbDataAdapter adapter = new OleDbDataAdapter(sqlText, this.connection as OleDbConnection);
    OleDbCommandBuilder dbCommandBuilder = new OleDbCommandBuilder(adapter);
    return (IDbDataAdapter) adapter;
  }

  public override string GetIntField(string fieldName, string asFieldName)
  {
    return $"{fieldName} as {asFieldName}";
  }
}
