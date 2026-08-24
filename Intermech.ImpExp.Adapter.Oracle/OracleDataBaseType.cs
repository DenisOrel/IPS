// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Adapter.Oracle.OracleDataBaseType
// Assembly: Intermech.ImpExp.Adapter.Oracle, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D553EB52-5206-4E60-A4A5-05A894FA883B
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Adapter.Oracle.dll

using Intermech.ImpExp.Interface;

#nullable disable
namespace Intermech.ImpExp.Adapter.Oracle;

internal sealed class OracleDataBaseType : IDataBaseType
{
  public static string DBType = "IntermechConnection.Oracle";

  public string DataBaseType() => OracleDataBaseType.DBType;

  public IDataBase GetNewDataBase() => (IDataBase) new OracleDataBase();
}
