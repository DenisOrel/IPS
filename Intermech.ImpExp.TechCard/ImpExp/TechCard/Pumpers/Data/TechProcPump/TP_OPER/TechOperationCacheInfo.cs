// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_OPER.TechOperationCacheInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Interfaces;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_OPER;

[Serializable]
internal class TechOperationCacheInfo : ISerializable
{
  private Guid _ownerGuid;
  private long _ownerId;

  public TechOperationCacheInfo()
  {
  }

  protected TechOperationCacheInfo(SerializationInfo info, StreamingContext context)
  {
    if (info == null)
      return;
    string str = info.GetString("ownerGuid");
    this._ownerGuid = GuidHelper.IsGuid(str) ? new Guid(str) : Guid.Empty;
    this._ownerId = info.GetInt64("ownerId");
  }

  public Guid OwnerGuid
  {
    [DebuggerStepThrough] get => this._ownerGuid;
    set => this._ownerGuid = value;
  }

  public long OwnerId
  {
    [DebuggerStepThrough] get => this._ownerId;
    set => this._ownerId = value;
  }

  public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    info.AddValue("ownerGuid", (object) this._ownerGuid.ToString());
    info.AddValue("ownerId", this._ownerId);
  }
}
