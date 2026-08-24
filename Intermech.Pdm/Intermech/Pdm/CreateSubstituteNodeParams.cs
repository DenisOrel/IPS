// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.CreateSubstituteNodeParams
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Pdm;

public class CreateSubstituteNodeParams : AdvCreateObjectNodeParams
{
  protected long substitutesGroupNoID;
  protected long substituteInGroup;
  protected List<NodeColumnID> attributes;

  public CreateSubstituteNodeParams()
  {
  }

  public CreateSubstituteNodeParams(object source) => this.Assign(source);

  public CreateSubstituteNodeParams(
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
    long modificationID,
    string filtrationOwnerID,
    List<long> contexts,
    int projObjType,
    long projID,
    List<NodeColumnID> attributes,
    object[] values,
    long substitutesGroupNoID,
    long substituteInGroup)
    : base(objTypeId, objId, id, checkedOutBy, prjLinkId, lcStepID, caption, relTypeID, owner, sorting, state, version, baseVersion, siteID, filtrationOwnerID, contexts, projObjType, projID, Guid.Empty, modificationID, (List<int>) null, values)
  {
    this.attributes = attributes;
    this.substitutesGroupNoID = substitutesGroupNoID;
    this.substituteInGroup = substituteInGroup;
  }

  public virtual long SubstitutesGroupNoID
  {
    [DebuggerStepThrough] get => this.substitutesGroupNoID;
    set => this.substitutesGroupNoID = value;
  }

  public virtual long SubstituteInGroup
  {
    [DebuggerStepThrough] get => this.substituteInGroup;
    set => this.substituteInGroup = value;
  }

  public virtual List<NodeColumnID> Attributes
  {
    [DebuggerStepThrough] get => this.attributes;
    set => this.attributes = value;
  }

  public override void Clear()
  {
    base.Clear();
    this.substituteInGroup = 0L;
    this.substitutesGroupNoID = 0L;
    this.attributes = (List<NodeColumnID>) null;
  }

  public override void Assign(object source)
  {
    base.Assign(source);
    if (!(source is CreateSubstituteNodeParams substituteNodeParams))
      return;
    this.substitutesGroupNoID = substituteNodeParams.SubstitutesGroupNoID;
    this.substituteInGroup = substituteNodeParams.SubstituteInGroup;
    this.attributes = substituteNodeParams.Attributes;
  }
}
