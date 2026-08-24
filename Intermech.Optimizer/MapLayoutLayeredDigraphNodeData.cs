// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayoutLayeredDigraphNodeData
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System;

#nullable disable
namespace Intermech.Map.Layout;

[Serializable]
public class MapLayoutLayeredDigraphNodeData
{
  private int myColumn;
  private int myComponent;
  private int myDiscover;
  private int myFinish;
  private int myIndex;
  private int myLayer;
  private bool myValid;

  public int Column
  {
    get => this.myColumn;
    set => this.myColumn = value;
  }

  public int Component
  {
    get => this.myComponent;
    set => this.myComponent = value;
  }

  public int Discover
  {
    get => this.myDiscover;
    set => this.myDiscover = value;
  }

  public int Finish
  {
    get => this.myFinish;
    set => this.myFinish = value;
  }

  public int Index
  {
    get => this.myIndex;
    set => this.myIndex = value;
  }

  public int Layer
  {
    get => this.myLayer;
    set => this.myLayer = value;
  }

  public bool Valid
  {
    get => this.myValid;
    set => this.myValid = value;
  }
}
