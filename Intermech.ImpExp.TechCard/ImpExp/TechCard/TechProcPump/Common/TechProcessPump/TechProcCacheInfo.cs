// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump.TechProcCacheInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Interfaces;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump;

[Serializable]
internal class TechProcCacheInfo : ISerializable
{
  protected int _objTypeId = -1;
  protected int _cehCode;
  private int _productionCode;
  private int _userCode;
  private long _ownerId;
  private Guid _ownerGuid;

  public TechProcCacheInfo(int objTypeId)
    : this(objTypeId, 0)
  {
  }

  public TechProcCacheInfo(int objTypeId, int cehCode)
  {
    this._objTypeId = objTypeId;
    this._cehCode = cehCode;
  }

  protected TechProcCacheInfo(SerializationInfo info, StreamingContext context)
  {
    if (info == null)
      return;
    this._objTypeId = info.GetInt32("objTypeID");
    this._cehCode = info.GetInt32("cehCode");
    foreach (SerializationEntry serializationEntry in info)
    {
      switch (serializationEntry.Name)
      {
        case "productionCode":
          this._productionCode = Convert.ToInt32(serializationEntry.Value);
          continue;
        case "userCode":
          this._userCode = Convert.ToInt32(serializationEntry.Value);
          continue;
        case "ownerId":
          this._ownerId = Convert.ToInt64(serializationEntry.Value);
          continue;
        case "ownerGuid":
          string str = Convert.ToString(serializationEntry.Value);
          if (GuidHelper.IsGuid(str))
          {
            this._ownerGuid = new Guid(str);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  public int ObjTypeId
  {
    [DebuggerStepThrough] get => this._objTypeId;
  }

  public int CehCode
  {
    [DebuggerStepThrough] get => this._cehCode;
  }

  public int ProductionCode
  {
    [DebuggerStepThrough] get => this._productionCode;
    [DebuggerStepThrough] set => this._productionCode = value;
  }

  public int UserCode
  {
    [DebuggerStepThrough] get => this._userCode;
    [DebuggerStepThrough] set => this._userCode = value;
  }

  public long OwnerId
  {
    [DebuggerStepThrough] get => this._ownerId;
    [DebuggerStepThrough] set => this._ownerId = value;
  }

  public Guid OwnerGuid
  {
    [DebuggerStepThrough] get => this._ownerGuid;
    [DebuggerStepThrough] set => this._ownerGuid = value;
  }

  public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    info.AddValue("objTypeID", this._objTypeId);
    info.AddValue("cehCode", this._cehCode);
    info.AddValue("productionCode", this._productionCode);
    info.AddValue("userCode", this._userCode);
    info.AddValue("ownerId", this._ownerId);
    info.AddValue("ownerGuid", (object) this._ownerGuid.ToString());
  }
}
