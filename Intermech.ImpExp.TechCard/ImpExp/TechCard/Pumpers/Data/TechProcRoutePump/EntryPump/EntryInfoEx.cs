// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump.EntryInfoEx
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump;

internal class EntryInfoEx
{
  public EntryInfoEx(int zakKey) => this.ZakKey = zakKey;

  public int ZakKey { get; }

  public ISet<int> Sbs { get; } = (ISet<int>) new HashSet<int>();

  public bool Processed { get; set; }
}
