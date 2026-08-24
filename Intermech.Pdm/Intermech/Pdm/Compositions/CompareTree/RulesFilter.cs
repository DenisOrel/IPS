// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.RulesFilter
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class RulesFilter
{
  private List<Guid> _types;

  public RulesFilter(int objectType)
  {
    this._types = new List<Guid>();
    Guid objectTypeGuid = MetaDataHelper.GetObjectTypeGuid(objectType);
    this._types.Add(objectTypeGuid);
    if (MetaDataHelper.GetObjectTypeParentsGuid(objectTypeGuid).Count <= 0)
      return;
    this._types.AddRange((IEnumerable<Guid>) MetaDataHelper.GetObjectTypeParentsGuid(objectTypeGuid));
  }

  public bool InFilter(IDBAttribute attribute)
  {
    if (attribute.IsNull || attribute.ValuesCount == 0)
      return true;
    for (int index = 0; index < attribute.ValuesCount; ++index)
    {
      attribute.Index = index;
      string asString = attribute.AsString;
      if (!string.IsNullOrEmpty(asString) && this._types.Contains(new Guid(asString)))
        return false;
    }
    return true;
  }
}
