// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.IArticleTypesItem
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal interface IArticleTypesItem
{
  int SectionId { get; }

  string SectName { get; set; }

  int DocType { get; }

  string ArtKind { get; }

  string Bitmap { get; }

  string Note { get; }

  int TransAct { get; }

  string MuOn { get; }

  string ImbaseOnly { get; }

  int OrderId { get; }

  int PrId { get; }

  int Multidesignatio { get; }

  int ControlDelete { get; }

  Guid Guid { get; }

  byte[] Icon { get; set; }

  ObjectVersionModes VersionMode { get; set; }

  List<AtricleTypeField> CfgData { get; }

  Guid ParentID { get; set; }

  Guid DefRelation { get; set; }

  Guid LCScheme { get; set; }

  IDictionary<int, IArticleTypesItem> Parents { get; }

  IDictionary<int, IArticleTypesItem> Childs { get; }

  bool IsTreeRoot { get; set; }

  bool AnyAttribute { get; set; }

  IDictionary<int, IArticleTypesItem> TreeChilds { get; }
}
