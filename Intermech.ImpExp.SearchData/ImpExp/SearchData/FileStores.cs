// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.FileStores
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public static class FileStores
{
  public static IDbConnection MainDBConnection = (IDbConnection) null;

  public static FileStoreList FS { get; } = new FileStoreList();
}
