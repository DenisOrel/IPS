// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump.LinkedObjDescr
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump;

internal struct LinkedObjDescr(int objKey, LinkedObjectType objType) : IEquatable<LinkedObjDescr>
{
  public int ObjKeyKey { get; } = objKey;

  public LinkedObjectType ObjType { get; } = objType;

  public bool Equals(LinkedObjDescr other)
  {
    return this.ObjKeyKey == other.ObjKeyKey && this.ObjType == other.ObjType;
  }

  public override bool Equals(object obj) => obj is LinkedObjDescr other && this.Equals(other);

  public override int GetHashCode() => (int) ((LinkedObjectType) this.ObjKeyKey ^ this.ObjType);
}
