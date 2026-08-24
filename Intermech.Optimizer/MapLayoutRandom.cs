// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayoutRandom
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace Intermech.Map.Layout;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[Serializable]
public class MapLayoutRandom : MapLayout
{
  private int maxx;
  private int maxy;
  private int minx;
  private int miny;
  private Random r;

  public MapLayoutRandom() => this.r = new Random();

  protected virtual bool IsFixed(MapLayoutNetworkNode node) => false;

  public override void PerformLayout()
  {
    if (this.Document == null)
      throw new InvalidOperationException("Must set the Document property to non-null");
    if (this.Network == null)
      this.Network = new MapLayoutNetwork((IMapCollection) this.Document);
    this.RaiseProgress(0.0f);
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      if (!this.IsFixed(node))
      {
        PointF pointF = new PointF();
        int num1 = this.r.Next();
        int num2 = this.r.Next();
        pointF.X = (float) (this.minx + num1 % (this.maxx - this.minx + 1));
        pointF.Y = (float) (this.miny + num2 % (this.maxy - this.miny + 1));
        node.Center = pointF;
        node.CommitPosition();
      }
    }
    this.RaiseProgress(1f);
  }

  public int MaxX
  {
    get => this.maxx;
    set => this.maxx = value;
  }

  public int MaxY
  {
    get => this.maxy;
    set => this.maxy = value;
  }

  public int MinX
  {
    get => this.minx;
    set => this.minx = value;
  }

  public int MinY
  {
    get => this.miny;
    set => this.miny = value;
  }
}
