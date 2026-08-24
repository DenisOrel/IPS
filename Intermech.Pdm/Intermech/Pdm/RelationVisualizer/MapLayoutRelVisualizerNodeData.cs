// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.MapLayoutRelVisualizerNodeData
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map.Layout;
using System;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

[Serializable]
public class MapLayoutRelVisualizerNodeData
{
  private int layerIndex;
  private MapLayoutNetworkNode parent;
  private int level;

  public int LayerIndex
  {
    get => this.layerIndex;
    set => this.layerIndex = value;
  }

  public MapLayoutNetworkNode Parent
  {
    get => this.parent;
    set => this.parent = value;
  }

  public int Level
  {
    get => this.level;
    set => this.level = value;
  }
}
