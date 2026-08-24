// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.AttributesCompareFacade
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class AttributesCompareFacade
{
  private readonly Dictionary<FieldTypes, IAttributeComparer> _comparers;
  private readonly IAttributeComparer _multivalueComparer;
  private readonly IAttributeComparer _stringComparer;
  private readonly Guid _ruleID;

  public AttributesCompareFacade(Guid ruleID)
  {
    IntAttributeComparer attributeComparer = new IntAttributeComparer();
    this._comparers = new Dictionary<FieldTypes, IAttributeComparer>()
    {
      {
        FieldTypes.ftMemo,
        (IAttributeComparer) new MemoAttributeComparer()
      },
      {
        FieldTypes.ftBoolean,
        (IAttributeComparer) new MemoAttributeComparer()
      },
      {
        FieldTypes.ftDateTime,
        (IAttributeComparer) new DateTimeAttributeComparer()
      },
      {
        FieldTypes.ftDouble,
        (IAttributeComparer) new DoubleAttributeComparer()
      },
      {
        FieldTypes.ftAutoInc,
        (IAttributeComparer) attributeComparer
      },
      {
        FieldTypes.ftInteger,
        (IAttributeComparer) attributeComparer
      },
      {
        FieldTypes.ftObjectLink,
        (IAttributeComparer) attributeComparer
      },
      {
        FieldTypes.ftMeasured,
        (IAttributeComparer) new MeasureAttributeComparer()
      }
    };
    this._multivalueComparer = (IAttributeComparer) new MultiValuesAttributeComparer();
    this._stringComparer = (IAttributeComparer) new StringAttributeComparer();
    this._ruleID = ruleID;
  }

  public void CompareChildItems(
    IUserSession session,
    CompositionItem item1,
    CompositionItem item2,
    bool checkAttributesExists)
  {
    for (int index = 0; index < item1.Count; ++index)
    {
      if (!item1[index].Empty && !item2[index].Empty)
        this.CompareItems(session, item1[index], item2[index], checkAttributesExists);
    }
  }

  public void CompareItems(
    IUserSession session,
    CompositionItem item1,
    CompositionItem item2,
    bool checkAttributesExists)
  {
    if (!this.Compare(session, item1, item2, checkAttributesExists))
    {
      item2.CompositionItemFlag |= CompositionItemFlags.AttributesChanged;
      CompareFlagHelper.SetStateFlagRecursiveUp(item2, CompositionItemFlags.AttributesChangedInCompositionObject, true);
    }
    this.CompareChildItems(session, item1, item2, checkAttributesExists);
  }

  private bool Compare(
    IUserSession session,
    CompositionItem item1,
    CompositionItem item2,
    bool checkAttributesExists)
  {
    ICompareTreeSettingsService service = (ICompareTreeSettingsService) ServicesManager.GetService(typeof (ICompareTreeSettingsService));
    List<int> compareAttributes1 = service.GetObjectCompareAttributes(this._ruleID, item1.ObjectTypeID);
    bool flag = true;
    if (compareAttributes1 != null && compareAttributes1.Count > 0 && !this.CompareAttributes(session, compareAttributes1, AttributeSourceTypes.Object, item1, item2, checkAttributesExists))
      flag = false;
    List<int> compareAttributes2 = service.GetRelationCompareAttributes(this._ruleID, item1.RelationTypeID);
    if (compareAttributes2 != null && compareAttributes2.Count > 0 && !this.CompareAttributes(session, compareAttributes2, AttributeSourceTypes.Relation, item1, item2, checkAttributesExists))
      flag = false;
    return flag;
  }

  private bool CompareAttributes(
    IUserSession session,
    List<int> attributes,
    AttributeSourceTypes sourceType,
    CompositionItem item1,
    CompositionItem item2,
    bool checkAttributesExists)
  {
    bool flag = true;
    IDBAttributable attributable1 = (IDBAttributable) null;
    IDBAttributable attributable2 = (IDBAttributable) null;
    foreach (int attribute in attributes)
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attribute);
      IAttributeComparer attributeComparer;
      if (attributeType.MultiValueMode == MultiValueModes.MultiValues || attributeType.MultiValueMode == MultiValueModes.MultiValuesFromList)
        attributeComparer = this._multivalueComparer;
      else if (!this._comparers.TryGetValue(attributeType.FieldType, out attributeComparer))
        attributeComparer = this._stringComparer;
      if (!attributeComparer.CompareAttribute(session, attributeType, sourceType, item1, item2, ref attributable1, ref attributable2, checkAttributesExists))
        flag = false;
    }
    return flag;
  }
}
