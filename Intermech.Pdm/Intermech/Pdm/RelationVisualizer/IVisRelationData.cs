// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.IVisRelationData
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public interface IVisRelationData
{
  long RelationId { get; set; }

  int RelType { get; set; }

  long ProjVerId { get; set; }

  long PartVerId { get; set; }

  MeasuredValue Quantity { get; set; }

  CADRelType CADType { get; set; }
}
