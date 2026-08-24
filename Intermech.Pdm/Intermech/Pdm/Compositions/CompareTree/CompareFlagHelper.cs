// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareFlagHelper
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal static class CompareFlagHelper
{
  public static void SetStateFlagRecursiveUp(
    CompositionItem item,
    CompositionItemFlags flag,
    bool parentOnly = false)
  {
    if (!parentOnly)
      item.CompositionItemFlag |= flag;
    if (item.Parent == null)
      return;
    CompareFlagHelper.SetStateFlagRecursiveUp(item.Parent, flag);
  }

  public static void SetStateFlagRecursiveDown(CompositionItem item, CompositionItemFlags flag)
  {
    item.CompositionItemFlag |= flag;
    foreach (CompositionItem compositionItem in (List<CompositionItem>) item)
      CompareFlagHelper.SetStateFlagRecursiveDown(compositionItem, flag);
  }
}
