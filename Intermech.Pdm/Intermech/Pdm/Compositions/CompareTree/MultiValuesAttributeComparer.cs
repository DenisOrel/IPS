// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.MultiValuesAttributeComparer
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class MultiValuesAttributeComparer : AttributableAttributeComparer<object[]>
{
  protected override object[] GetAttributeValue(IDBAttribute attribute) => attribute.Values;

  protected override bool OnCompareAttributes(
    CompositionItemAttribute compareAttribute1,
    CompositionItemAttribute compareAttribute2)
  {
    int num = CompareValuesHelper.CompareCollections<object>((ICollection<object>) (compareAttribute1.Value as object[]), (ICollection<object>) (compareAttribute2.Value as object[])) ? 1 : 0;
    if (num != 0)
      return num != 0;
    if (compareAttribute1.Count > compareAttribute2.Count)
    {
      this.AlignmentSize(compareAttribute1, compareAttribute2);
      return num != 0;
    }
    if (compareAttribute2.Count <= compareAttribute1.Count)
      return num != 0;
    this.AlignmentSize(compareAttribute2, compareAttribute1);
    return num != 0;
  }

  private void AlignmentSize(
    CompositionItemAttribute compareAttribute1,
    CompositionItemAttribute compareAttribute2)
  {
    int num = compareAttribute1.Count - compareAttribute2.Count;
    for (int index = 0; index < num; ++index)
      compareAttribute2.Add(CompositionItemAttributeValue.CreateDummy(compareAttribute2));
  }
}
