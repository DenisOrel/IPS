// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.BackgroundCompositionReaderArgs
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class BackgroundCompositionReaderArgs
{
  public Guid RuleID { get; }

  public CompositionItem Item1 { get; set; }

  public CompositionFiltrationSettings Filtration1 { get; }

  public CompositionItem Item2 { get; set; }

  public CompositionFiltrationSettings Filtration2 { get; }

  public bool Recursive { get; }

  public BackgroundCompositionReaderArgs(
    Guid ruleID,
    CompositionItem item1,
    CompositionFiltrationSettings filtration1,
    CompositionItem item2,
    CompositionFiltrationSettings filtration2,
    bool recursive)
  {
    this.Filtration1 = filtration1;
    this.Filtration2 = filtration2;
    this.Item1 = item1;
    this.Item2 = item2;
    this.Recursive = recursive;
    this.RuleID = ruleID;
  }
}
