// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ListAttributesAlphabeticalSort
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal class ListAttributesAlphabeticalSort : IComparer<CompositionItemAttribute>
{
  public int Compare(CompositionItemAttribute x, CompositionItemAttribute y)
  {
    return x.AttributeName.CompareTo(y.AttributeName);
  }
}
