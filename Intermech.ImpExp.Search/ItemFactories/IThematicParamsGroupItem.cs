// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.IThematicParamsGroupItem
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal interface IThematicParamsGroupItem
{
  int GroupId { get; }

  string Label { get; set; }

  Guid Guid { get; }

  string Note { get; set; }
}
