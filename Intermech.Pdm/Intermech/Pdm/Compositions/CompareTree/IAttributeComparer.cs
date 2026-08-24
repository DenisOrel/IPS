// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.IAttributeComparer
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal interface IAttributeComparer
{
  bool CompareAttribute(
    IUserSession session,
    IMSAttributeType attribute,
    AttributeSourceTypes sourceType,
    CompositionItem item1,
    CompositionItem item2,
    ref IDBAttributable attributable1,
    ref IDBAttributable attributable2,
    bool checkAttributesExists);
}
