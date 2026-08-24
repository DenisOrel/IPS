// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.PickedPair
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class PickedPair
{
  public MapGeneralNodePort Port { get; set; }

  public bool On { get; set; }

  public PickedPair(MapGeneralNodePort port, bool on)
  {
    this.Port = port;
    this.On = on;
  }
}
