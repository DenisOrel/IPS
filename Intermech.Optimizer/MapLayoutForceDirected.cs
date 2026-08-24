// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayoutForceDirected
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace Intermech.Map.Layout;

public class MapLayoutForceDirected : MapLayout
{
  private int iterations;
  private int max_iterations;
  private Random myRandom;

  public MapLayoutForceDirected()
  {
    this.max_iterations = 1000;
    this.myRandom = (Random) null;
  }

  protected virtual float ElectricalCharge(MapLayoutNetworkNode node) => 150f;

  protected virtual float ElectricalFieldX(PointF xy) => 0.0f;

  protected virtual float ElectricalFieldY(PointF xy) => 0.0f;

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

  protected virtual float GravitationalFieldX(PointF xy) => 0.0f;

  protected virtual float GravitationalFieldY(PointF xy) => 0.0f;

  protected virtual float GravitationalMass(MapLayoutNetworkNode node) => 0.0f;

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
    SizeF size = this.Document.Size;
    RectangleF bounds = this.Document.ComputeBounds();
    float x = Math.Min(bounds.X, topLeft.X);
    float y = Math.Min(bounds.Y, topLeft.Y);
    float num1 = Math.Max(bounds.X + bounds.Width, topLeft.X + size.Width);
    double num2 = (double) Math.Max(bounds.Y + bounds.Height, topLeft.Y + size.Height);
    float width = num1 - x;
    double num3 = (double) y;
    float height = (float) (num2 - num3);
    if ((double) x < (double) topLeft.X || (double) y < (double) topLeft.Y)
      this.Document.TopLeft = new PointF(x, y);
    if ((double) width <= (double) size.Width && (double) height <= (double) size.Height)
      return;
    this.Document.Size = new SizeF(width, height);
  }

  private MapLayoutForceDirectedLinkData LinkData(MapLayoutNetworkLink link)
  {
    return (MapLayoutForceDirectedLinkData) link.LinkData;
  }

  private MapLayoutForceDirectedNodeData NodeData(MapLayoutNetworkNode node)
  {
    return (MapLayoutForceDirectedNodeData) node.NodeData;
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
    {
      node.NodeData = (object) new MapLayoutForceDirectedNodeData();
      this.NodeData(node).Charge = this.ElectricalCharge(node);
      this.NodeData(node).Mass = this.GravitationalMass(node);
      this.NodeData(node).ChangeX = 0;
      this.NodeData(node).ChangeY = 0;
    }
    foreach (MapLayoutNetworkLink link in this.Network.Links)
    {
      link.LinkData = (object) new MapLayoutForceDirectedLinkData();
      this.LinkData(link).Stiffness = this.SpringStiffness(link);
      this.LinkData(link).Length = this.SpringLength(link);
    }
    this.iterations = 0;
    while (this.iterations < this.max_iterations)
    {
      ++this.iterations;
      if (this.UpdatePositions())
        this.RaiseProgress((float) this.iterations / (float) this.max_iterations);
      else
        break;
    }
    this.LayoutNodesAndLinks();
    this.RaiseProgress(1f);
  }

  protected virtual float SpringLength(MapLayoutNetworkLink link) => 50f;

  protected virtual float SpringStiffness(MapLayoutNetworkLink link) => 0.05f;

  protected virtual bool UpdatePositions()
  {
    ArrayList nodesArray = this.Network.NodesArray;
    for (int index = 0; index < nodesArray.Count; ++index)
    {
      MapLayoutNetworkNode node = (MapLayoutNetworkNode) nodesArray[index];
      this.NodeData(node).ForceX = 0.0f;
      this.NodeData(node).ForceY = 0.0f;
    }
    bool flag = false;
    for (int index1 = 0; index1 < nodesArray.Count; ++index1)
    {
      MapLayoutNetworkNode layoutNetworkNode1 = (MapLayoutNetworkNode) nodesArray[index1];
      PointF center1 = layoutNetworkNode1.Center;
      float num1 = this.NodeData(layoutNetworkNode1).Charge * this.ElectricalFieldX(center1);
      float num2 = this.NodeData(layoutNetworkNode1).Charge * this.ElectricalFieldY(center1);
      MapLayoutForceDirectedNodeData directedNodeData1 = this.NodeData(layoutNetworkNode1);
      this.NodeData(layoutNetworkNode1).ForceX = directedNodeData1.ForceX + num1;
      MapLayoutForceDirectedNodeData directedNodeData2 = this.NodeData(layoutNetworkNode1);
      this.NodeData(layoutNetworkNode1).ForceY = directedNodeData2.ForceY + num2;
      float num3 = this.NodeData(layoutNetworkNode1).Mass * this.GravitationalFieldX(center1);
      float num4 = this.NodeData(layoutNetworkNode1).Mass * this.GravitationalFieldY(center1);
      MapLayoutForceDirectedNodeData directedNodeData3 = this.NodeData(layoutNetworkNode1);
      this.NodeData(layoutNetworkNode1).ForceX = directedNodeData3.ForceX + num3;
      MapLayoutForceDirectedNodeData directedNodeData4 = this.NodeData(layoutNetworkNode1);
      this.NodeData(layoutNetworkNode1).ForceY = directedNodeData4.ForceY + num4;
      for (int index2 = index1 + 1; index2 < nodesArray.Count; ++index2)
      {
        MapLayoutNetworkNode layoutNetworkNode2 = (MapLayoutNetworkNode) nodesArray[index2];
        PointF center2 = layoutNetworkNode2.Center;
        float nodeDistance = this.GetNodeDistance(layoutNetworkNode1, layoutNetworkNode2);
        float num5;
        float num6;
        if ((double) nodeDistance < 1.0)
        {
          if (this.myRandom == null)
            this.myRandom = new Random();
          num5 = (float) this.myRandom.Next(20);
          num6 = (float) this.myRandom.Next(20);
        }
        else
        {
          double num7 = -((double) this.NodeData(layoutNetworkNode1).Charge * (double) this.NodeData(layoutNetworkNode2).Charge) / ((double) nodeDistance * (double) nodeDistance);
          num5 = (float) (num7 * (((double) center2.X - (double) center1.X) / (double) nodeDistance));
          num6 = (float) (num7 * (((double) center2.Y - (double) center1.Y) / (double) nodeDistance));
        }
        MapLayoutForceDirectedNodeData directedNodeData5 = this.NodeData(layoutNetworkNode1);
        this.NodeData(layoutNetworkNode1).ForceX = directedNodeData5.ForceX + num5;
        MapLayoutForceDirectedNodeData directedNodeData6 = this.NodeData(layoutNetworkNode1);
        this.NodeData(layoutNetworkNode1).ForceY = directedNodeData6.ForceY + num6;
        MapLayoutForceDirectedNodeData directedNodeData7 = this.NodeData(layoutNetworkNode2);
        this.NodeData(layoutNetworkNode2).ForceX = directedNodeData7.ForceX - num5;
        MapLayoutForceDirectedNodeData directedNodeData8 = this.NodeData(layoutNetworkNode2);
        this.NodeData(layoutNetworkNode2).ForceY = directedNodeData8.ForceY - num6;
      }
    }
    foreach (MapLayoutNetworkLink link in this.Network.Links)
    {
      MapLayoutNetworkNode fromNode = link.FromNode;
      MapLayoutNetworkNode toNode = link.ToNode;
      PointF center3 = fromNode.Center;
      PointF center4 = toNode.Center;
      float nodeDistance = this.GetNodeDistance(fromNode, toNode);
      float num8;
      float num9;
      if ((double) nodeDistance < 1.0)
      {
        if (this.myRandom == null)
          this.myRandom = new Random();
        num8 = (float) this.myRandom.Next(20);
        num9 = (float) this.myRandom.Next(20);
      }
      else
      {
        double num10 = (double) this.LinkData(link).Stiffness * ((double) nodeDistance - (double) this.LinkData(link).Length);
        num8 = (float) (num10 * (((double) center4.X - (double) center3.X) / (double) nodeDistance));
        num9 = (float) (num10 * (((double) center4.Y - (double) center3.Y) / (double) nodeDistance));
      }
      MapLayoutForceDirectedNodeData directedNodeData9 = this.NodeData(fromNode);
      this.NodeData(fromNode).ForceX = directedNodeData9.ForceX + num8;
      MapLayoutForceDirectedNodeData directedNodeData10 = this.NodeData(fromNode);
      this.NodeData(fromNode).ForceY = directedNodeData10.ForceY + num9;
      MapLayoutForceDirectedNodeData directedNodeData11 = this.NodeData(toNode);
      this.NodeData(toNode).ForceX = directedNodeData11.ForceX - num8;
      MapLayoutForceDirectedNodeData directedNodeData12 = this.NodeData(toNode);
      this.NodeData(toNode).ForceY = directedNodeData12.ForceY - num9;
    }
    for (int index = 0; index < nodesArray.Count; ++index)
    {
      MapLayoutNetworkNode node = (MapLayoutNetworkNode) nodesArray[index];
      PointF center = node.Center;
      if (!this.IsFixed(node))
      {
        float num11 = 1f;
        int val1_1;
        int val1_2;
        do
        {
          val1_1 = (int) Math.Round((double) this.NodeData(node).ForceX * (double) num11);
          val1_2 = (int) Math.Round((double) this.NodeData(node).ForceY * (double) num11);
          num11 *= 1.25f;
        }
        while (val1_1 == 0 && val1_2 == 0 && (double) num11 < 256.0);
        int num12 = Math.Min(Math.Max(val1_1, -50), 50);
        int num13 = Math.Min(Math.Max(val1_2, -50), 50);
        node.Center = new PointF((float) (int) ((double) center.X + (double) num12), (float) (int) ((double) center.Y + (double) num13));
        this.CheckCenter(node, nodesArray, index);
        MapLayoutForceDirectedNodeData directedNodeData13 = this.NodeData(node);
        this.NodeData(node).ChangeX = directedNodeData13.ChangeX + num12;
        MapLayoutForceDirectedNodeData directedNodeData14 = this.NodeData(node);
        this.NodeData(node).ChangeY = directedNodeData14.ChangeY + num13;
        if (this.iterations % 10 == 0)
        {
          if (Math.Abs(this.NodeData(node).ChangeX) > 1 || Math.Abs(this.NodeData(node).ChangeY) > 1)
            flag = true;
          this.NodeData(node).ChangeX = 0;
          this.NodeData(node).ChangeY = 0;
        }
      }
    }
    if (this.iterations % 10 != 0)
      flag = true;
    return flag;
  }

  /// <summary>
  /// Метод устанавливает центр нода исходя из того чтобы эленты не налезали друг на друга
  /// </summary>
  /// <param name="node"></param>
  /// <param name="nodes"></param>
  /// <param name="index"></param>
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

  [Browsable(false)]
  public int CurrentIteration => this.iterations;

  [Description("the maximum number of iterations to perform")]
  [DefaultValue(1000)]
  public int MaxIterations
  {
    get => this.max_iterations;
    set => this.max_iterations = Math.Max(value, 1);
  }

  [Browsable(false)]
  public Random RandomNumberGenerator
  {
    get => this.myRandom;
    set => this.myRandom = value;
  }
}
