// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ListAttributesAlphabeticalAndStateSort
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class ListAttributesAlphabeticalAndStateSort : IComparer<CompositionItemAttribute>
{
  public int Compare(CompositionItemAttribute x, CompositionItemAttribute y)
  {
    CompositionAttributeState state1 = x.State;
    if ((state1 & CompositionAttributeState.Dummy) == CompositionAttributeState.Dummy)
      state1 &= ~CompositionAttributeState.Dummy;
    CompositionAttributeState state2 = y.State;
    if ((state2 & CompositionAttributeState.Dummy) == CompositionAttributeState.Dummy)
      state2 &= ~CompositionAttributeState.Dummy;
    int num = state2.CompareTo((object) state1);
    return num == 0 ? x.AttributeName.CompareTo(y.AttributeName) : num;
  }
}
