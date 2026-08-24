// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.RelVisualizerMapLayout
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;
using Intermech.Map.Layout;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class RelVisualizerMapLayout : MapLayout
{
  private ILayoutAlgorithm algoritm;

  public RelVisualizerMapLayout(ILayoutAlgorithm algoritm) => this.algoritm = algoritm;

  protected virtual float GetNodeDistance(MapLayoutNetworkNode nodeA, MapLayoutNetworkNode nodeB)
  {
    PointF center1 = nodeA.Center;
    float num1;
    float num2;
    float num3;
    float num4;
    if (nodeA.MapObject != null)
    {
      float num5 = nodeA.MapObject.Width / 2f;
      num1 = center1.X - num5;
      num2 = 2f * num5;
      float num6 = nodeA.MapObject.Height / 2f;
      num3 = center1.Y - num6;
      num4 = 2f * num6;
    }
    else
    {
      num1 = center1.X;
      num2 = 0.0f;
      num3 = center1.Y;
      num4 = 0.0f;
    }
    PointF center2 = nodeB.Center;
    float num7;
    float num8;
    float num9;
    float num10;
    if (nodeB.MapObject != null)
    {
      float num11 = nodeB.MapObject.Width / 2f;
      num7 = center2.X - num11;
      num8 = 2f * num11;
      float num12 = nodeB.MapObject.Height / 2f;
      num9 = center2.Y - num12;
      num10 = 2f * num12;
    }
    else
    {
      num7 = center2.X;
      num8 = 0.0f;
      num9 = center2.Y;
      num10 = 0.0f;
    }
    if ((double) num1 + (double) num2 < (double) num7)
    {
      if ((double) num3 > (double) num9 + (double) num10)
        return (float) Math.Sqrt(Math.Pow((double) num1 + (double) num2 - (double) num7, 2.0) + Math.Pow((double) num3 - ((double) num9 + (double) num10), 2.0));
      return (double) num3 + (double) num4 < (double) num9 ? (float) Math.Sqrt(Math.Pow((double) num1 + (double) num2 - (double) num7, 2.0) + Math.Pow((double) num3 + (double) num4 - (double) num9, 2.0)) : Math.Abs(num1 + num2 - num7);
    }
    if ((double) num1 > (double) num7 + (double) num8)
    {
      if ((double) num3 > (double) num9 + (double) num10)
        return (float) Math.Sqrt(Math.Pow((double) num1 - ((double) num7 + (double) num8), 2.0) + Math.Pow((double) num3 - ((double) num9 + (double) num10), 2.0));
      return (double) num3 + (double) num4 < (double) num9 ? (float) Math.Sqrt(Math.Pow((double) num1 - ((double) num7 + (double) num8), 2.0) + Math.Pow((double) num3 + (double) num4 - (double) num9, 2.0)) : Math.Abs(num1 - (num7 + num8));
    }
    if ((double) num3 > (double) num9 + (double) num10)
      return Math.Abs(num3 - (num9 + num10));
    return (double) num3 + (double) num4 < (double) num9 ? Math.Abs(num3 + num4 - num9) : 0.1f;
  }

  protected virtual bool IsFixed(MapLayoutNetworkNode node) => false;

  public virtual void LayoutNodesAndLinks()
  {
    this.Document.RaiseChanging(220, 0, (object) null);
    this.Document.SuspendsUpdates = true;
    this.Network.CommitNodesAndLinks();
    this.Document.SuspendsUpdates = false;
    this.Document.RaiseChanged(220, 0, (object) null, 0, (object) null, MapObject.NullRect, 0, (object) null, MapObject.NullRect);
    if (this.Document.FixedSize)
      return;
    PointF topLeft = this.Document.TopLeft;
    RectangleF bounds = this.Document.ComputeBounds();
    float x = Math.Min(bounds.X, topLeft.X);
    float y = Math.Min(bounds.Y, topLeft.Y);
    float num1 = bounds.X + bounds.Width;
    double num2 = (double) bounds.Y + (double) bounds.Height;
    float width = num1 - x;
    double num3 = (double) y;
    float height = (float) (num2 - num3);
    if ((double) x < (double) topLeft.X || (double) y < (double) topLeft.Y)
      this.Document.TopLeft = new PointF(x, y);
    this.Document.Size = new SizeF(width, height);
  }

  private MapLayoutRelVisualizerLinkData LinkData(MapLayoutNetworkLink link)
  {
    return (MapLayoutRelVisualizerLinkData) link.LinkData;
  }

  private MapLayoutRelVisualizerNodeData NodeData(MapLayoutNetworkNode node)
  {
    return (MapLayoutRelVisualizerNodeData) node.NodeData;
  }

  public override void PerformLayout()
  {
    if (this.Document == null)
      throw new InvalidOperationException("Must set the Document property to non-null");
    if (this.Network == null)
      this.Network = new MapLayoutNetwork((IMapCollection) this.Document);
    this.RaiseProgress(0.0f);
    this.Network.DeleteSelfLinks();
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      node.NodeData = (object) new MapLayoutRelVisualizerNodeData();
    foreach (MapLayoutNetworkLink link in this.Network.Links)
      link.LinkData = (object) new MapLayoutRelVisualizerLinkData();
    this.UpdatePositions();
    this.LayoutNodesAndLinks();
    this.RaiseProgress(1f);
  }

  protected virtual float SpringLength(MapLayoutNetworkLink link) => 50f;

  protected virtual float SpringStiffness(MapLayoutNetworkLink link) => 0.05f;

  protected virtual bool UpdatePositions()
  {
    List<MapLayoutNetworkNode> nodes1 = new List<MapLayoutNetworkNode>();
    MapLayoutNodeEnumerator nodes2 = this.Network.Nodes;
    while (nodes2.MoveNext())
    {
      MapLayoutNetworkNode current = nodes2.Current;
      nodes1.Add(current);
    }
    this.algoritm.UpdatePositions(this, nodes1);
    for (int index1 = 0; index1 < nodes1.Count; ++index1)
    {
      MapLayoutNetworkNode node = nodes1[index1];
      PointF center1 = node.Center;
      this.NodeData(node);
      for (int index2 = index1 + 1; index2 < nodes1.Count; ++index2)
      {
        PointF center2 = nodes1[index2].Center;
        this.NodeData(node);
      }
    }
    return true;
  }

  public void CheckCenter(MapLayoutNetworkNode node, ArrayList nodes, int index)
  {
    if (node.MapObject == null)
      return;
    RectangleF bounds1 = node.MapObject.Bounds;
    RectangleF rect = bounds1;
    for (int index1 = 0; index1 < index; ++index1)
    {
      MapLayoutNetworkNode node1 = (MapLayoutNetworkNode) nodes[index1];
      if (node1 != null)
      {
        RectangleF bounds2 = node1.MapObject.Bounds;
        if (bounds2.IntersectsWith(rect) || bounds2.Contains(rect) || rect.Contains(bounds2))
          rect = new RectangleF(rect.X, (float) ((double) bounds2.Y - (double) rect.Height - 20.0), rect.Width, rect.Height);
      }
    }
    if (!(bounds1 != rect))
      return;
    PointF pointF = new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
    node.Center = pointF;
  }
}
