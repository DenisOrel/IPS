// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Adapter.MsSQL.MSSQLTableListCache
// Assembly: Intermech.ImpExp.Adapter.MsSQL, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: AC488FB0-E7AD-42BA-82F4-B99B0CA102F7
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Adapter.MsSQL.dll

using Intermech.ImpExp.Interface;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Adapter.MsSQL;

internal sealed class MSSQLTableListCache(IDbConnection connection) : TableListCache(connection, "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'")
{
}
