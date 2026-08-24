// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ProductionListIDHelper
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.SearchData.Controls;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal sealed class ProductionListIDHelper
{
  public RelationTypesComparison RelationTypes { get; }

  public Dictionary<int, List<int>> EnabledRelationAttributes { get; }

  public int SimpleTypeID { get; }

  public int TechTypeID { get; }

  public int ProdTypeID { get; }

  public int PlplTypeID { get; }

  public ProductionListIDHelper()
  {
    this.RelationTypes = RelationTypesComparison.Instance;
    this.EnabledRelationAttributes = new Dictionary<int, List<int>>();
    this.SimpleTypeID = this.InitRelationType(this.RelationTypes.SimpleType.RelationType);
    this.TechTypeID = this.InitRelationType(this.RelationTypes.TechType.RelationType);
    this.ProdTypeID = this.InitRelationType(this.RelationTypes.ProdType.RelationType);
    this.PlplTypeID = this.InitRelationType(this.RelationTypes.PLPLType.RelationType);
  }

  private int InitRelationType(Guid relationTypeGuid)
  {
    IMSRelationType relationType = MetaDataHelper.GetRelationType(relationTypeGuid);
    if (this.EnabledRelationAttributes.ContainsKey(relationType.RelationTypeID))
      return relationType.RelationTypeID;
    if (relationType.AnyAttributes)
      this.EnabledRelationAttributes.Add(relationType.RelationTypeID, (List<int>) null);
    else
      this.EnabledRelationAttributes.Add(relationType.RelationTypeID, MetaDataHelper.GetAttribute4RelationTypeList(relationType.RelationTypeID).ConvertAll<int>((Converter<IMSAttribute4RelationType, int>) (x => x.AttributeID)));
    return relationType.RelationTypeID;
  }
}
