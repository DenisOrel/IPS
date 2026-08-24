// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ZPCEntryPump.ArtProdCopies
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ZPCEntryPump;

internal class ArtProdCopies
{
  public Dictionary<long, ZPCProcRouteInfo> ProcRoutes { get; } = new Dictionary<long, ZPCProcRouteInfo>();

  public bool Processed { get; set; }
}
