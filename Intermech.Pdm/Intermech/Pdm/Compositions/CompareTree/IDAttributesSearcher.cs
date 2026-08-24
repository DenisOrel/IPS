// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.IDAttributesSearcher
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class IDAttributesSearcher
{
  private readonly List<int> _attributeIDs;
  private readonly AttributeSourceTypes _sourceType;

  public IDAttributesSearcher(List<int> attributeIDs, AttributeSourceTypes sourceType)
  {
    this._attributeIDs = attributeIDs;
    this._sourceType = sourceType;
  }

  public CompositionItem Find(CompositionItem item, List<CompositionItem> collection)
  {
    if (this._attributeIDs == null || this._attributeIDs.Count == 0)
      return (CompositionItem) null;
    List<Tuple<int, AttributeSourceTypes, object>> searchValues = this.GetSearchValues(item);
    if (searchValues.Count > 0)
    {
      foreach (CompositionItem compositionItem in collection)
      {
        if (compositionItem.LevelIndex < 0 && !compositionItem.Empty)
        {
          foreach (Tuple<int, AttributeSourceTypes, object> tuple in searchValues)
          {
            Tuple<int, AttributeSourceTypes, object> searchValue = tuple;
            if (compositionItem.Attributes.Find((Predicate<CompositionItemAttribute>) (x => x.AttributeID.Equals(searchValue.Item1) && x.SourceType.Equals((object) searchValue.Item2) && object.Equals(x.Value, searchValue.Item3))) != null)
              return compositionItem;
          }
        }
      }
    }
    return (CompositionItem) null;
  }

  private List<Tuple<int, AttributeSourceTypes, object>> GetSearchValues(CompositionItem item)
  {
    List<Tuple<int, AttributeSourceTypes, object>> searchValues = new List<Tuple<int, AttributeSourceTypes, object>>();
    foreach (int attributeId in this._attributeIDs)
    {
      int attribute = attributeId;
      CompositionItemAttribute compositionItemAttribute = item.Attributes.Find((Predicate<CompositionItemAttribute>) (x => x.AttributeID.Equals(attribute) && x.SourceType.Equals((object) this._sourceType)));
      if (compositionItemAttribute != null)
        searchValues.Add(new Tuple<int, AttributeSourceTypes, object>(attribute, this._sourceType, compositionItemAttribute.Value));
    }
    return searchValues;
  }
}
