// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ZPCEntryPump.ZPCEntryInfoEx
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ZPCEntryPump;

internal class ZPCEntryInfoEx
{
  public Dictionary<int, ZPCSbList> ExitSbsPrjLinkIds { get; } = new Dictionary<int, ZPCSbList>();

  public bool Processed { get; set; }
}
