// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.LevelCompositionReaderArgs
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class LevelCompositionReaderArgs
{
  public CompositionItem Item { get; }

  public CompositionFiltrationSettings Filtration { get; }

  public bool Recursive { get; }

  public LevelCompositionReaderArgs(
    CompositionItem item,
    CompositionFiltrationSettings filtration,
    bool recursive)
  {
    this.Filtration = filtration;
    this.Item = item;
    this.Recursive = recursive;
  }
}
