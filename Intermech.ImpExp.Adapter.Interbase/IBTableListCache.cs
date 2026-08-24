// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Adapter.Interbase.IBTableListCache
// Assembly: Intermech.ImpExp.Adapter.Interbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B97FBD89-71A5-4417-A5DC-2CB918616870
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Adapter.Interbase.dll

using Intermech.ImpExp.Interface;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Adapter.Interbase;

internal sealed class IBTableListCache(IDbConnection connection) : TableListCache(connection, "select RDB$RELATION_NAME from RDB$RELATION_FIELDS GROUP BY  RDB$RELATION_NAME")
{
}
