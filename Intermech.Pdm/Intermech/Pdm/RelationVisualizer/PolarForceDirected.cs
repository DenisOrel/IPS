// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.PolarForceDirected
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map.Layout;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class PolarForceDirected : MapLayoutForceDirected
{
  protected override float GravitationalMass(MapLayoutNetworkNode node)
  {
    return base.GravitationalMass(node);
  }

  protected override float ElectricalCharge(MapLayoutNetworkNode node)
  {
    return base.ElectricalCharge(node);
  }

  protected override float ElectricalFieldY(PointF xy) => base.ElectricalFieldY(xy);

  protected override float ElectricalFieldX(PointF xy) => base.ElectricalFieldX(xy);

  protected override float GravitationalFieldY(PointF xy) => base.GravitationalFieldY(xy) + 150f;

  protected override float GravitationalFieldX(PointF xy) => base.GravitationalFieldX(xy);
}
