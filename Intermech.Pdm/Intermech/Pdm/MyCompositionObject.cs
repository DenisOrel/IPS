// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.MyCompositionObject
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Pdm;

public sealed class MyCompositionObject : IComparable, IDBRelationID
{
  internal long PrjLinkID;
  internal long ObjectID;
  internal int FRelationType = -1;
  internal long FSorting;

  public override bool Equals(object obj)
  {
    if (obj == null || this.GetType() != obj.GetType())
      return false;
    MyCompositionObject compositionObject = (MyCompositionObject) obj;
    return this.PrjLinkID == compositionObject.PrjLinkID && this.ObjectID == compositionObject.ObjectID && this.FRelationType == compositionObject.FRelationType;
  }

  public override int GetHashCode()
  {
    return this.PrjLinkID.GetHashCode() ^ this.ObjectID.GetHashCode() ^ this.FRelationType.GetHashCode();
  }

  public MyCompositionObject()
  {
  }

  public MyCompositionObject(long APrjLinkID, long AnObjectID)
  {
    this.PrjLinkID = APrjLinkID;
    this.ObjectID = AnObjectID;
    this.FRelationType = -1;
    this.FRelationType = MetaDataHelper.GetRelationType4PrjLinkID((IUserSession) null, this.PrjLinkID);
    if (this.FRelationType != -1)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.FRelationType = MetaDataHelper.GetRelationType4PrjLinkID(sessionKeeper.Session, this.PrjLinkID);
  }

  public int CompareTo(object obj)
  {
    return obj == null || this.GetType() != obj.GetType() || !this.Equals((object) (MyCompositionObject) obj) ? -1 : 0;
  }

  public long Value
  {
    [DebuggerStepThrough] get => this.PrjLinkID;
  }

  public long PartID
  {
    [DebuggerStepThrough] get => this.ObjectID;
  }

  public int RelationType
  {
    [DebuggerStepThrough] get => this.FRelationType;
  }

  public long Sorting
  {
    [DebuggerStepThrough] get => this.FSorting;
  }

  public long ProjID
  {
    [DebuggerStepThrough] get => throw new NotImplementedException();
  }

  public Guid RelGuid
  {
    [DebuggerStepThrough] get => throw new NotImplementedException();
  }
}
