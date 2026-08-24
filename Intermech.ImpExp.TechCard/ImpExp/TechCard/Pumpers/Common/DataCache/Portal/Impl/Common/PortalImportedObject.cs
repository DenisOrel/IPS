// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Common.PortalImportedObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Common;

public abstract class PortalImportedObject
{
  public Guid IpsObjVerGuid { get; set; } = Guid.Empty;

  public long IpsObjVerId { get; set; }

  public Guid IpsObjGuid { get; set; } = Guid.Empty;

  public long IpsObjId { get; set; }
}
