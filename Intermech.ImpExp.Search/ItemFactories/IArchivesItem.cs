// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.IArchivesItem
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal interface IArchivesItem
{
  int ArchiveID { get; }

  string Alias { get; }

  string FileName { get; }

  int StrongSign { get; }

  string SignStamp { get; }

  string Descriptio { get; }

  int MakeRvis { get; }

  Dictionary<string, string> CfgData { get; }

  int PersonId { get; }

  int ParentID { get; }

  int ChkRights { get; }

  int StorageId { get; }

  long ObjectID { get; set; }
}
