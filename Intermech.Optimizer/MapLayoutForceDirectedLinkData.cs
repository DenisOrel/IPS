// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayoutForceDirectedLinkData
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System;

#nullable disable
namespace Intermech.Map.Layout;

[Serializable]
public class MapLayoutForceDirectedLinkData
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
