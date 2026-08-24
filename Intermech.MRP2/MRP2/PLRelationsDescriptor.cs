// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.PLRelationsDescriptor
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP2;

internal class PLRelationsDescriptor(
  int categoryID,
  int typeID,
  string filtrationOwnerID,
  List<long> contexts,
  long objID,
  int objType,
  int relationTypeID,
  string caption,
  long checkedOutBy,
  long owner,
  long sorting,
  int lcStepID,
  List<int> attributes,
  long version,
  long baseVersion) : AdvRelationsDescriptor(categoryID, typeID, filtrationOwnerID, contexts, objID, objType, relationTypeID, caption, checkedOutBy, owner, sorting, lcStepID, attributes, version, baseVersion)
{
  public override INode GetChild(INodeID nodeID)
  {
    return !(nodeID is AdvRelationsNodeID advRelationsNodeId) ? base.GetChild(nodeID) : (INode) new PLRelationsNode((CreateObjectNodeParams) new AdvCreateObjectNodeParams(advRelationsNodeId.ObjectTypeID, advRelationsNodeId.ObjectID, advRelationsNodeId.ID, advRelationsNodeId.CheckedOutBy, advRelationsNodeId.PrjLinkID, advRelationsNodeId.LCStepID, advRelationsNodeId.Caption, advRelationsNodeId.RelationTypeID > 0 ? advRelationsNodeId.RelationTypeID : this.RelationTypeID, advRelationsNodeId.Owner, advRelationsNodeId.Sorting, advRelationsNodeId.State, advRelationsNodeId.Version, advRelationsNodeId.BaseVersion, advRelationsNodeId.SiteID, advRelationsNodeId.FiltrationOwnerID, advRelationsNodeId.Contexts, advRelationsNodeId.ProjObjType, advRelationsNodeId.ProjID, advRelationsNodeId.RelGuid, advRelationsNodeId.ModificationID, advRelationsNodeId.Attributes, advRelationsNodeId.Values));
  }
}
