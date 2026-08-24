// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.MaterialGroupSubstitute.TechMaterialGroupSubstituteCacheInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.MaterialGroupSubstitute;

[Serializable]
internal class TechMaterialGroupSubstituteCacheInfo : ISerializable
{
  protected int _replaceableParentType;
  protected int _replaceableParentKey;
  private int _replaceableObjectType;
  private int _replaceableObjectKey;
  private int _substituteObjectType;
  private int _substituteObjectKey;
  private int _order;

  public TechMaterialGroupSubstituteCacheInfo()
  {
  }

  protected TechMaterialGroupSubstituteCacheInfo(SerializationInfo info, StreamingContext context)
  {
    if (info == null)
      return;
    this._replaceableParentType = info.GetInt32("replaceableParentType");
    this._replaceableParentKey = info.GetInt32("replaceableParentKey");
    this._replaceableObjectType = info.GetInt32("replaceableObjectType");
    this._replaceableObjectKey = info.GetInt32("replaceableObjectKey");
    this._substituteObjectType = info.GetInt32("substituteObjectType");
    this._substituteObjectKey = info.GetInt32("substituteObjectKey");
    this._order = info.GetInt32("order");
  }

  public int ReplaceableParentType
  {
    [DebuggerStepThrough] get => this._replaceableParentType;
    [DebuggerStepThrough] set => this._replaceableParentType = value;
  }

  public int ReplaceableParentKey
  {
    [DebuggerStepThrough] get => this._replaceableParentKey;
    [DebuggerStepThrough] set => this._replaceableParentKey = value;
  }

  public int ReplaceableObjectType
  {
    [DebuggerStepThrough] get => this._replaceableObjectType;
    [DebuggerStepThrough] set => this._replaceableObjectType = value;
  }

  public int ReplaceableObjectKey
  {
    [DebuggerStepThrough] get => this._replaceableObjectKey;
    [DebuggerStepThrough] set => this._replaceableObjectKey = value;
  }

  public int SubstituteObjectType
  {
    [DebuggerStepThrough] get => this._substituteObjectType;
    [DebuggerStepThrough] set => this._substituteObjectType = value;
  }

  public int SubstituteObjectKey
  {
    [DebuggerStepThrough] get => this._substituteObjectKey;
    [DebuggerStepThrough] set => this._substituteObjectKey = value;
  }

  public int Order
  {
    [DebuggerStepThrough] get => this._order;
    [DebuggerStepThrough] set => this._order = value;
  }

  public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    info.AddValue("replaceableParentType", this._replaceableParentType);
    info.AddValue("replaceableParentKey", this._replaceableParentKey);
    info.AddValue("replaceableObjectType", this._replaceableObjectType);
    info.AddValue("replaceableObjectKey", this._replaceableObjectKey);
    info.AddValue("substituteObjectType", this._substituteObjectType);
    info.AddValue("substituteObjectKey", this._substituteObjectKey);
    info.AddValue("order", this._order);
  }

  public static string GetObjectCacheCode(int objectKey, int objectType)
  {
    return $"{objectType}_{objectKey}";
  }
}
