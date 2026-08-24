// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ItemFactories.ISelectionItem
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.SearchData.ItemFactories;

internal interface ISelectionItem
{
  int SampleID { get; }

  int ArchID { get; }

  string SampleName { get; }

  List<string> SampleFLT { get; set; }

  DateTime SampleDate { get; }

  int UserID { get; }

  bool IsSample { get; }

  bool CanAnyEdit { get; }

  int IsCommon { get; }

  int SampleHMN { get; }

  int Smp_IN { get; }

  bool Inherit { get; }

  int DelControl { get; }

  string Description { get; }
}
