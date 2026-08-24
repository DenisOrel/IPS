// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayoutForceDirectedNodeData
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System;

#nullable disable
namespace Intermech.Map.Layout;

[Serializable]
public class MapLayoutForceDirectedNodeData
{
  private int myChangeX;
  private int myChangeY;
  private float myCharge;
  private float myForceX;
  private float myForceY;
  private float myMass;

  public int ChangeX
  {
    get => this.myChangeX;
    set => this.myChangeX = value;
  }

  public int ChangeY
  {
    get => this.myChangeY;
    set => this.myChangeY = value;
  }

  public float Charge
  {
    get => this.myCharge;
    set => this.myCharge = value;
  }

  public float ForceX
  {
    get => this.myForceX;
    set => this.myForceX = value;
  }

  public float ForceY
  {
    get => this.myForceY;
    set => this.myForceY = value;
  }

  public float Mass
  {
    get => this.myMass;
    set => this.myMass = value;
  }
}
