// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ContextComposition.ObjectContextCompositionNodePart
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core.Navigator.Classes.ObjectNode;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;

#nullable disable
namespace Intermech.Pdm.ContextComposition;

internal sealed class ObjectContextCompositionNodePart(
  long objectVersionID,
  int objectTypeID,
  IServiceProvider serviceProvider) : RelatedObjectsPart(objectTypeID, objectVersionID, RelatedObjectsRole.Composition, serviceProvider)
{
  protected override INodeQuery GetQuery(ConditionStructure[] conditions)
  {
    INodeQuery query = base.GetQuery(conditions);
    if (!(query is RelatedObjectsQuery relatedObjectsQuery))
      return query;
    relatedObjectsQuery.QueryFilter = (IRelatedObjectQueryFilterMode) new RelatedObjectQueryFilterMode(filterDataByVersionRule: false);
    return query;
  }

  protected override RelatedObjectsQuery QueryConstruction(ConditionStructure[] conditions)
  {
    return (RelatedObjectsQuery) new ObjectContextCompositionNodeQuery((INodeQuerySupport) this, this._objID, this._objTypeID, conditions, MetaDataHelper.GetRelationTypeID("cad00023-306c-11d8-b4e9-00304f19f545"));
  }
}
