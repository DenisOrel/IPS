// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump.ProcRouteInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump;

internal class ProcRouteInfo
{
  public Guid RouteProcId { get; set; } = Guid.NewGuid();

  public IDictionary<int, EntryInfoEx> ZakInfo { get; } = (IDictionary<int, EntryInfoEx>) new Dictionary<int, EntryInfoEx>();
}
