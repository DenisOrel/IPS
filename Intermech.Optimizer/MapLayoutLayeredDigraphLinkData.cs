// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayoutLayeredDigraphLinkData
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System;

#nullable disable
namespace Intermech.Map.Layout;

[Serializable]
public class MapLayoutLayeredDigraphLinkData
{
  private bool myForest;
  private int myPortFromColOffset;
  private int myPortFromPos;
  private int myPortToColOffset;
  private int myPortToPos;
  private bool myRev;
  private bool myValid;

  public bool Forest
  {
    get => this.myForest;
    set => this.myForest = value;
  }

  public int PortFromColOffset
  {
    get => this.myPortFromColOffset;
    set => this.myPortFromColOffset = value;
  }

  public int PortFromPos
  {
    get => this.myPortFromPos;
    set => this.myPortFromPos = value;
  }

  public int PortToColOffset
  {
    get => this.myPortToColOffset;
    set => this.myPortToColOffset = value;
  }

  public int PortToPos
  {
    get => this.myPortToPos;
    set => this.myPortToPos = value;
  }

  public bool Rev
  {
    get => this.myRev;
    set => this.myRev = value;
  }

  public bool Valid
  {
    get => this.myValid;
    set => this.myValid = value;
  }
}
