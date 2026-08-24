// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisRelData
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class VisRelData : IVisRelationData
{
  public long RelationId { get; set; }

  public int RelType { get; set; }

  public long ProjVerId { get; set; }

  public long PartVerId { get; set; }

  public MeasuredValue Quantity { get; set; }

  public CADRelType CADType { get; set; }

  public VisRelData(long relId) => this.RelationId = relId;

  public VisRelData(long relId, int relType, long projVerId, long partVerId)
    : this(relId)
  {
    this.RelType = relType;
    this.ProjVerId = projVerId;
    this.PartVerId = partVerId;
  }

  public VisRelData(
    long relId,
    int relType,
    long projVerId,
    long partVerId,
    MeasuredValue quan,
    CADRelType crType)
    : this(relId, relType, projVerId, partVerId)
  {
    this.Quantity = quan;
    this.CADType = crType;
  }
}
