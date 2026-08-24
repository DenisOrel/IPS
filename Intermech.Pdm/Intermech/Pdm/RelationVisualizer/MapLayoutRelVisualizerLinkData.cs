// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.MapLayoutRelVisualizerLinkData
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

[Serializable]
public class MapLayoutRelVisualizerLinkData
{
  private float myLength;
  private float myStiffness;

  public float Length
  {
    get => this.myLength;
    set => this.myLength = value;
  }

  public float Stiffness
  {
    get => this.myStiffness;
    set => this.myStiffness = value;
  }
}
