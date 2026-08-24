// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ContainsNodeID
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;

#nullable disable
namespace Intermech.Pdm.Compositions;

public class ContainsNodeID : NodeID
{
  public object ID;

  public ContainsNodeID(object id, CreateObjectNodeParams pars)
    : base(pars)
  {
    this.ID = id;
  }

  public override int GetHashCode() => base.GetHashCode();

  public override bool Equals(object obj)
  {
    return obj is ContainsNodeID containsNodeId ? object.Equals(containsNodeId.ID, this.ID) : base.Equals(obj);
  }
}
