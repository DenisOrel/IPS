// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump.EntryContent
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump;

internal class EntryContent : IEquatable<EntryContent>
{
  public ISet<LinkedObjDescr> Content { get; } = (ISet<LinkedObjDescr>) new HashSet<LinkedObjDescr>();

  public bool Equals(EntryContent other)
  {
    if (other == null)
      return false;
    if (this == other)
      return true;
    if (this.Content.Count != other.Content.Count)
      return false;
    foreach (LinkedObjDescr linkedObjDescr in (IEnumerable<LinkedObjDescr>) this.Content)
    {
      if (!other.Content.Contains(linkedObjDescr))
        return false;
    }
    return true;
  }

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;
    if (this == obj)
      return true;
    return !(obj.GetType() != this.GetType()) && this.Equals((EntryContent) obj);
  }

  public override int GetHashCode()
  {
    int hashCode = this.Content.Count.GetHashCode();
    foreach (LinkedObjDescr linkedObjDescr in (IEnumerable<LinkedObjDescr>) this.Content)
      hashCode ^= linkedObjDescr.GetHashCode();
    return hashCode;
  }
}
