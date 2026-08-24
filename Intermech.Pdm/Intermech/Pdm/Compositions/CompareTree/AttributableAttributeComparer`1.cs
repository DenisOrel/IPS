// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.AttributableAttributeComparer`1
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal abstract class AttributableAttributeComparer<TValue> : IAttributeComparer
{
  public bool CompareAttribute(
    IUserSession session,
    IMSAttributeType attribute,
    AttributeSourceTypes sourceType,
    CompositionItem item1,
    CompositionItem item2,
    ref IDBAttributable attributable1,
    ref IDBAttributable attributable2,
    bool checkAttributesExists)
  {
    CompositionItemAttribute compareAttribute1;
    CompositionItemAttribute compareAttribute2;
    switch (this.CheckCompareNeed(session, attribute, sourceType, item1, item2, ref attributable1, ref attributable2, out compareAttribute1, out compareAttribute2, checkAttributesExists))
    {
      case AttributableAttributeComparer<TValue>.CheckResult.Equal:
        return true;
      case AttributableAttributeComparer<TValue>.CheckResult.Different:
        return false;
      default:
        if (this.OnCompareAttributes(compareAttribute1, compareAttribute2))
          return true;
        compareAttribute1.State = CompositionAttributeState.Changed;
        compareAttribute2.State = CompositionAttributeState.Changed;
        return false;
    }
  }

  private IDBAttributable GetAttributable(
    IUserSession session,
    CompositionItem item,
    AttributeSourceTypes sourceType)
  {
    IDBAttributable attributable = (IDBAttributable) null;
    switch (sourceType)
    {
      case AttributeSourceTypes.Object:
        attributable = (IDBAttributable) session.GetObject(item.ObjectID, true);
        break;
      case AttributeSourceTypes.Relation:
        attributable = (IDBAttributable) session.GetRelation(item.PrjLinkID, true);
        break;
    }
    return attributable;
  }

  private bool CheckAttributeExists(
    IUserSession session,
    int attributeID,
    AttributeSourceTypes sourceType,
    CompositionItem item,
    CompositionItemAttribute compareAttribute,
    ref IDBAttributable attributable)
  {
    if (compareAttribute == null)
      return false;
    if (compareAttribute.Value != DBNull.Value)
      return true;
    if (attributable == null)
      attributable = this.GetAttributable(session, item, sourceType);
    return attributable.GetAttributeByID(attributeID) != null;
  }

  private CompositionItemAttribute CreateCompositionItemAttribute(
    IDBAttributable attributable,
    int attributeID,
    AttributeSourceTypes sourceType,
    CompositionAttributeState addedState)
  {
    bool exist;
    string description;
    TValue attributeValue = this.GetAttributeValue(attributable, attributeID, out exist, out description);
    CompositionAttributeState state = exist ? CompositionAttributeState.Equal : addedState | CompositionAttributeState.Dummy;
    return new CompositionItemAttribute(attributeID, sourceType, (object) attributeValue, description, state);
  }

  private TValue GetAttributeValue(
    IDBAttributable attributable,
    int attributeID,
    out bool exist,
    out string description)
  {
    IDBAttribute attributeById = attributable.GetAttributeByID(attributeID);
    exist = attributeById != null;
    description = exist ? attributeById.Description : (string) null;
    return attributeById == null ? default (TValue) : this.GetAttributeValue(attributeById);
  }

  private CompositionItemAttribute GetCompareAttribute(
    IUserSession session,
    IMSAttributeType attribute,
    AttributeSourceTypes sourceType,
    CompositionItem item,
    ref IDBAttributable attributable,
    CompositionAttributeState state,
    out bool attributeExist,
    bool checkAttributesExists)
  {
    CompositionItemAttribute compositionItemAttribute = item.Attributes.Find((Predicate<CompositionItemAttribute>) (x => x.AttributeID == attribute.AttributeID));
    if (compositionItemAttribute == null)
    {
      if (attributable == null)
        attributable = this.GetAttributable(session, item, sourceType);
      compositionItemAttribute = this.CreateCompositionItemAttribute(attributable, attribute.AttributeID, sourceType, state);
      attributeExist = (compositionItemAttribute.State & CompositionAttributeState.Dummy) == CompositionAttributeState.None;
      item.AddAttribute(compositionItemAttribute);
    }
    else
      attributeExist = !checkAttributesExists || this.CheckAttributeExists(session, attribute.AttributeID, sourceType, item, compositionItemAttribute, ref attributable);
    return compositionItemAttribute;
  }

  protected AttributableAttributeComparer<TValue>.CheckResult CheckCompareNeed(
    IUserSession session,
    IMSAttributeType attribute,
    AttributeSourceTypes sourceType,
    CompositionItem item1,
    CompositionItem item2,
    ref IDBAttributable attributable1,
    ref IDBAttributable attributable2,
    out CompositionItemAttribute compareAttribute1,
    out CompositionItemAttribute compareAttribute2,
    bool checkAttributesExists)
  {
    bool attributeExist1;
    compareAttribute1 = this.GetCompareAttribute(session, attribute, sourceType, item1, ref attributable1, CompositionAttributeState.Added, out attributeExist1, checkAttributesExists);
    bool attributeExist2;
    compareAttribute2 = this.GetCompareAttribute(session, attribute, sourceType, item2, ref attributable2, CompositionAttributeState.Removed, out attributeExist2, checkAttributesExists);
    if (!attributeExist1 && !attributeExist2)
      return AttributableAttributeComparer<TValue>.CheckResult.Equal;
    if (!attributeExist1 & attributeExist2)
    {
      if (compareAttribute1 == null)
        item1.AddAttribute(new CompositionItemAttribute(attribute.AttributeID, sourceType, (object) null, (string) null, CompositionAttributeState.Added | CompositionAttributeState.Dummy));
      else
        compareAttribute1.State = CompositionAttributeState.Added | CompositionAttributeState.Dummy;
      compareAttribute2.State = CompositionAttributeState.Added;
      return AttributableAttributeComparer<TValue>.CheckResult.Different;
    }
    if (!attributeExist1 || attributeExist2)
      return AttributableAttributeComparer<TValue>.CheckResult.NeedCompare;
    if (compareAttribute2 == null)
      item2.AddAttribute(new CompositionItemAttribute(attribute.AttributeID, sourceType, (object) null, (string) null, CompositionAttributeState.Removed | CompositionAttributeState.Dummy));
    else
      compareAttribute2.State = CompositionAttributeState.Removed | CompositionAttributeState.Dummy;
    compareAttribute1.State = CompositionAttributeState.Removed;
    return AttributableAttributeComparer<TValue>.CheckResult.Different;
  }

  protected abstract bool OnCompareAttributes(
    CompositionItemAttribute compareAttribute1,
    CompositionItemAttribute compareAttribute2);

  protected abstract TValue GetAttributeValue([NotNull] IDBAttribute attribute);

  protected enum CheckResult
  {
    NeedCompare,
    Equal,
    Different,
  }
}
