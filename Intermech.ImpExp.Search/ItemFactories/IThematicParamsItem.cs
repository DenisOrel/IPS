// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.IThematicParamsItem
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal interface IThematicParamsItem
{
  int ParamId { get; }

  string Label { get; set; }

  int GroupId { get; }

  Guid Guid { get; set; }

  ThematicParamsType ParamType { get; }

  string AliasDoc { get; }

  string AliasArt { get; }

  int ArtOrDoc { get; }

  string SrcAlias { get; }

  string SrcField { get; }

  string SrcBd { get; }

  string UName { get; }

  string BdPwd { get; }

  FieldTypes NewFieldType { get; }

  string DefValue { get; }

  List<string> LisValues { get; }

  int Size { get; set; }
}
