// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.IDocTypeItem
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal interface IDocTypeItem
{
  int DocType { get; }

  string DocCode { get; set; }

  string DocName { get; set; }

  string DocExt { get; set; }

  string Bitmap { get; }

  int DocColor { get; }

  int DrawStamp { get; }

  int Suffix { get; }

  string LinkedExt { get; set; }

  int RefSetup { get; }

  byte[] FileBody { get; }

  string ProtoName { get; }

  string Classif { get; }

  string DTName { get; }

  string DTCode { get; }

  int StrongSign { get; }

  string SignStamp { get; }

  Guid Guid { get; set; }

  Guid ParentID { get; set; }

  Guid DefRelation { get; set; }

  Guid LCScheme { get; set; }

  ObjectVersionModes VersionMode { get; set; }

  bool AnyAttribute { get; set; }

  byte[] Icon { get; set; }
}
