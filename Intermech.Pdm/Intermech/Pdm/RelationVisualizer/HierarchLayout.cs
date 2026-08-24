// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.HierarchLayout
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class HierarchLayout : VisLayout
{
  public override void InitLayout(Size logSize, VisScheme scheme)
  {
    base.InitLayout(logSize, scheme);
  }

  public override void BeforeLayoutLevel(VisScheme scheme, int levelNum, bool parent)
  {
  }

  public override void BeforeLayout(BasePredicate cancel, VisScheme scheme)
  {
  }

  public override LayoutKind GetLayoutKind() => LayoutKind.Hierarchical;

  public static LayoutKind GetKind() => LayoutKind.Hierarchical;
}
