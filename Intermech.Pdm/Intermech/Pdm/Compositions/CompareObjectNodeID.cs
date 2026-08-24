// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareObjectNodeID
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Navigator.DBObjects;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions;

public class CompareObjectNodeID(
  int objTypeId,
  long objId,
  long id,
  long checkedOutBy,
  long prjLinkId,
  int lcStepID,
  string caption,
  int relTypeID,
  long owner,
  long sorting,
  ObjectFiltrationState state,
  long version,
  long baseVersion,
  string siteID,
  long modificationID) : NodeID(objTypeId, objId, id, checkedOutBy, prjLinkId, lcStepID, caption, relTypeID, owner, sorting, state, version, baseVersion, siteID, 0L, Guid.Empty, modificationID), IComparable
{
  public override bool Equals(object obj)
  {
    return !(obj is CompareObjectNodeID compareObjectNodeId) ? base.Equals(obj) : compareObjectNodeId.ObjectID == this.ObjectID;
  }

  public override int GetHashCode() => this.ObjectID.GetHashCode();

  public int CompareTo(object obj) => !this.Equals(obj) ? 1 : 0;
}
