// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SubstitutesNodeID
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Pdm;

public class SubstitutesNodeID : AdvRelationsNodeID
{
  private IServiceProvider _services;

  public IServiceProvider Services
  {
    [DebuggerStepThrough] get => this._services;
  }

  public long SubstitutesGroupNoID
  {
    [DebuggerStepThrough] get => (this.pars as CreateSubstituteNodeParams).SubstitutesGroupNoID;
  }

  public long SubstituteInGroup
  {
    [DebuggerStepThrough] get => (this.pars as CreateSubstituteNodeParams).SubstituteInGroup;
  }

  public List<NodeColumnID> Attributes
  {
    [DebuggerStepThrough] get => (this.pars as CreateSubstituteNodeParams).Attributes;
  }

  public new object[] Values
  {
    [DebuggerStepThrough] get => (this.pars as CreateSubstituteNodeParams).Values;
  }

  public new object this[int attributeID]
  {
    get
    {
      for (int index = 0; index < (this.pars as CreateSubstituteNodeParams).Attributes.Count; ++index)
      {
        if ((this.pars as CreateSubstituteNodeParams).Attributes[index].ID.Equals((object) attributeID))
          return (this.pars as CreateSubstituteNodeParams).Values[index];
      }
      return (object) null;
    }
  }

  public SubstitutesNodeID(CreateObjectNodeParams e, IServiceProvider services)
    : base(e)
  {
    this._services = services;
    this.pars = (CreateObjectNodeParams) new CreateSubstituteNodeParams((object) e);
  }

  public override bool Equals(object obj)
  {
    return !(obj is SubstitutesNodeID substitutesNodeId) ? base.Equals(obj) : substitutesNodeId.PrjLinkID == this.PrjLinkID;
  }

  [DebuggerStepThrough]
  public override int GetHashCode() => this.PrjLinkID.GetHashCode();
}
