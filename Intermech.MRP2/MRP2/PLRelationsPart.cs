// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.PLRelationsPart
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP2;

internal class PLRelationsPart(
  int projObjTypeID,
  long projID,
  int relationTypeID,
  string filtrationOwnerID,
  List<long> contexts,
  List<int> attributes,
  IServiceProvider services) : AdvRelationsPart(projObjTypeID, projID, relationTypeID, filtrationOwnerID, contexts, attributes, services)
{
  public override INode GetChild(INodeID nodeID)
  {
    return nodeID is AdvRelationsNodeID advRelationsNodeId ? (INode) new PLRelationsNode((CreateObjectNodeParams) new AdvCreateObjectNodeParams(advRelationsNodeId.ObjectTypeID, advRelationsNodeId.ObjectID, advRelationsNodeId.ID, advRelationsNodeId.CheckedOutBy, advRelationsNodeId.PrjLinkID, advRelationsNodeId.LCStepID, advRelationsNodeId.Caption, advRelationsNodeId.RelationTypeID, advRelationsNodeId.Owner, advRelationsNodeId.Sorting, advRelationsNodeId.State, advRelationsNodeId.Version, advRelationsNodeId.BaseVersion, advRelationsNodeId.SiteID, advRelationsNodeId.FiltrationOwnerID, advRelationsNodeId.Contexts, advRelationsNodeId.ProjObjType, advRelationsNodeId.ProjID, advRelationsNodeId.RelGuid, advRelationsNodeId.ModificationID, advRelationsNodeId.Attributes, advRelationsNodeId.Values)) : (INode) null;
  }

  protected override INodeQuery GetQuery(ConditionStructure[] conditions)
  {
    ConditionStructure[] conditions1 = conditions;
    if (this.RelationTypeID == MRP2Consts.reltypeIdDocumentation)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (MRP2Consts.GetDocConditions(sessionKeeper.Session) == null)
          return (INodeQuery) null;
        conditions1 = ConditionStructure.Join(conditions, MRP2Consts.GetDocConditions(sessionKeeper.Session));
      }
    }
    return base.GetQuery(conditions1);
  }
}
