// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.IVisLayout
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

internal interface IVisLayout
{
  void InitLayout(Size logSize, VisScheme scheme);

  void BeforeLayoutLevel(VisScheme scheme, int levelNum, bool parent);

  void BeforeLayout(BasePredicate cancel, VisScheme scheme);

  void SetInitialLevelCoords(VisLevel level, int levNum, Point centerPoint);

  void SetInitialCoords(BasePredicate cancel, VisScheme scheme, Point centerPoint);

  void DoLayout(BasePredicate cancel, VisScheme scheme);

  LayoutKind GetLayoutKind();

  void ChangeSizes(VisScheme scheme, int xCoef, int yCoef);

  void RestoreLevels(VisScheme scheme);

  bool Vertical { get; set; }

  void ProcessInvisible(VisScheme scheme);
}
