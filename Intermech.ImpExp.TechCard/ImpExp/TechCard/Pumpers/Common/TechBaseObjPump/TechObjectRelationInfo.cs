// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.TechObjectRelationInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

internal class TechObjectRelationInfo
{
  private readonly ObjectRecord _objRec;
  private readonly ObjectRecord _objEntRec;
  private readonly TechObjectRecord _record;
  private long _objRecId;
  private long _objEntRecId;

  public TechObjectRelationInfo(
    ObjectRecord objRec,
    ObjectRecord objEntRec,
    TechObjectRecord record,
    string joinGroupId)
  {
    this._objRec = objRec;
    this._objEntRec = objEntRec;
    this._record = record;
    this.JoinGroupId = joinGroupId;
  }

  public ObjectRecord ObjRec => this._objRec;

  public ObjectRecord ObjEntRec => this._objEntRec;

  public TechObjectRecord Record => this._record;

  public long ObjRecID
  {
    get => this._objRecId;
    set => this._objRecId = value;
  }

  public long ObjEntRecID
  {
    get => this._objEntRecId;
    set => this._objEntRecId = value;
  }

  public string JoinGroupId { get; }

  public override bool Equals(object obj)
  {
    if (!(obj is TechObjectRelationInfo objectRelationInfo))
      return false;
    if (objectRelationInfo.ObjRec == null || this.ObjRec == null)
      return objectRelationInfo.ObjRec == this.ObjRec;
    if (objectRelationInfo.ObjEntRec == null || this.ObjEntRec == null)
      return objectRelationInfo.ObjEntRec == this.ObjEntRec;
    if (string.IsNullOrEmpty(objectRelationInfo.JoinGroupId) || string.IsNullOrEmpty(this.JoinGroupId))
      return objectRelationInfo.JoinGroupId == this.JoinGroupId;
    return (Guid) objectRelationInfo.ObjRec.ObjectGuid == (Guid) this.ObjRec.ObjectGuid && (Guid) objectRelationInfo.ObjEntRec.ObjectGuid == (Guid) this.ObjEntRec.ObjectGuid;
  }

  public override int GetHashCode()
  {
    if (this.ObjRec != null)
      return this.ObjRec.ObjectGuid.GetHashCode();
    return this._objEntRec != null ? this.ObjEntRec.ObjectGuid.GetHashCode() : base.GetHashCode();
  }
}
