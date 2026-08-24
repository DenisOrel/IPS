// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.VirtualCompoitionSettings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

public sealed class VirtualCompoitionSettings : CompoitionSettings
{
  public static Dictionary<Guid, string> VirtualSchemes = new Dictionary<Guid, string>()
  {
    {
      new Guid("{F358B7CB-0985-4D40-87D9-6D721BBF6EE4}"),
      "Cравнение составов по умолчанию"
    }
  };

  public VirtualCompoitionSettings() => this.CheckExistsAttributes = false;

  public override List<int> GetChildTypes(int parentTypeID, int relationTypeID)
  {
    return MetaDataHelper.GetApplicabilityChildObjectTypes(parentTypeID, relationTypeID).ConvertAll<int>((Converter<IMSObjectType, int>) (objType => objType.ObjectTypeID));
  }

  public override List<int> GetIDObjectAttributes(int objectTypeID)
  {
    return base.GetIDObjectAttributes(objectTypeID);
  }

  public override List<int> GetIDRelationAttributes(int parentTypeID, int relationTypeID)
  {
    return base.GetIDRelationAttributes(parentTypeID, relationTypeID);
  }

  public override List<int> GetObjectCompareAttributes(int objectTypeID)
  {
    return base.GetObjectCompareAttributes(objectTypeID);
  }

  public override List<int> GetRelationCompareAttributes(int relationTypeID)
  {
    return base.GetRelationCompareAttributes(relationTypeID);
  }

  public override List<Tuple<int, AttributeSourceTypes>> GetSortedAttributes(int parentTypeID)
  {
    return base.GetSortedAttributes(parentTypeID);
  }

  public override List<int> GetRelationTypes(int objectTypeID)
  {
    return new List<int>((IEnumerable<int>) new int[1]
    {
      MetaDataHelper.GetObjectType(objectTypeID).DefaultRelation
    });
  }
}
