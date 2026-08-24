// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.BooleanAttributeComparer
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class BooleanAttributeComparer : AttributableAttributeComparer<bool>
{
  protected override bool GetAttributeValue(IDBAttribute attribute) => attribute.AsBoolean;

  protected override bool OnCompareAttributes(
    CompositionItemAttribute compareAttribute1,
    CompositionItemAttribute compareAttribute2)
  {
    return CompareValuesHelper.CompareBoolValues(compareAttribute1.Value, compareAttribute2.Value);
  }
}
