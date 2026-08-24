// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.ILayoutAlgorithm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;
using Intermech.Map.Layout;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public interface ILayoutAlgorithm
{
  string GetAlgorithmName();

  void NextLayer();

  int DistanceBetweenItems { get; }

  bool isShowArrow();

  event MapLayoutProgressEventHandler LayoutProgress;

  void LayoutDocument(MapDocument document, Size winSize);

  void UpdateLayoutDocument(MapDocument document, Size winSize);

  void UpdatePositions(RelVisualizerMapLayout layout, List<MapLayoutNetworkNode> nodes);

  void RegistExistObject();
}
