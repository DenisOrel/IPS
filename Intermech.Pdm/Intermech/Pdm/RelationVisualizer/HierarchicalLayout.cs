// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.HierarchicalLayout
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Localization;
using Intermech.Map;
using Intermech.Map.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class HierarchicalLayout : ILayoutAlgorithm
{
  private PointF centralNodePosition = (PointF) Point.Empty;
  private int nomNodeInThisLevel;
  private int nomLayerInThisTree;
  private int maxLayersCount;
  private Size winSize;
  private Dictionary<int, List<MapLayoutNetworkNode>> layers = new Dictionary<int, List<MapLayoutNetworkNode>>();
  private List<MapLayoutNetworkNode> nodesList;
  private int maxLevel = int.MinValue;
  private int minLevel = int.MaxValue;
  private int maxCountOnLevel;
  public bool Normalize;
  public const int DefaultVerticalSpace = 70;
  public const int DefaultHorizontalSpace = 250;
  public static int VerticalSpace = 70;
  public static int HorizontalSpace = 250;
  public static double XKoef = 1.0;
  public static double YKoef = 1.0;
  private int levelWidth;
  private int height;

  public bool isShowArrow() => false;

  public int MaxLayersCount
  {
    get => this.maxLayersCount;
    set => this.maxLayersCount = value;
  }

  public virtual string GetAlgorithmName() => HierarchicalLayout.AlgorithmName();

  public static string AlgorithmName() => LocalizationHolder.rm.GetString("Pdm_rv_11");

  public void RegistExistObject() => ++this.nomNodeInThisLevel;

  public event MapLayoutProgressEventHandler LayoutProgress;

  public void NextLayer()
  {
    this.nomNodeInThisLevel = 0;
    ++this.nomLayerInThisTree;
  }

  public PointF CalculateCentralNodePosition(Size winSize)
  {
    this.centralNodePosition = new PointF(100f, (float) (winSize.Height / 2));
    return this.centralNodePosition;
  }

  public void BeginChildLayout() => this.nomLayerInThisTree = 0;

  public void BeginParentLayout() => this.nomLayerInThisTree = 0;

  public void LayoutDocument(MapDocument document, Size winSize)
  {
    this.winSize = winSize;
    RelVisualizerMapLayout visualizerMapLayout = new RelVisualizerMapLayout((ILayoutAlgorithm) this);
    visualizerMapLayout.Document = document;
    visualizerMapLayout.Progress += new MapLayoutProgressEventHandler(this.DocumentLayoutProgress);
    visualizerMapLayout.PerformLayout();
    visualizerMapLayout.Progress -= new MapLayoutProgressEventHandler(this.DocumentLayoutProgress);
  }

  public void UpdateLayoutDocument(MapDocument document, Size winSize)
  {
    this.LayoutDocument(document, winSize);
  }

  private void DocumentLayoutProgress(object sender, MapLayoutProgressEventArgs e)
  {
    if (this.LayoutProgress == null)
      return;
    this.LayoutProgress(sender, e);
  }

  private void CreateLayers(
    MapLayoutNetworkNode parent,
    MapLayoutNetworkNode node,
    int level,
    int layer)
  {
    if (node == null || !(node.MapObject is VisObjectNode mapObject) || !mapObject.Layer.AllowView)
      return;
    this.nodesList.Add(node);
    if (!this.layers.ContainsKey(level))
    {
      this.layers.Add(level, new List<MapLayoutNetworkNode>());
      this.maxLevel = Math.Max(level, this.maxLevel);
      this.minLevel = Math.Min(level, this.minLevel);
    }
    if (this.Normalize && this.layers[level].Count > 10)
    {
      if (level > 0)
        ++layer;
      else
        --layer;
    }
    if (!this.layers.ContainsKey(layer))
    {
      this.layers.Add(layer, new List<MapLayoutNetworkNode>());
      this.maxLevel = Math.Max(layer, this.maxLevel);
      this.minLevel = Math.Min(layer, this.minLevel);
    }
    this.layers[layer].Add(node);
    int count = this.layers[layer].Count;
    if (count > this.maxCountOnLevel)
      this.maxCountOnLevel = count;
    node.NodeData = (object) new MapLayoutRelVisualizerNodeData();
    (node.NodeData as MapLayoutRelVisualizerNodeData).Level = level;
    (node.NodeData as MapLayoutRelVisualizerNodeData).LayerIndex = layer;
    (node.NodeData as MapLayoutRelVisualizerNodeData).Parent = parent;
    if (level >= 0)
    {
      if (node.SourceLinksList == null)
        return;
      foreach (MapLayoutNetworkLink sourceLinks in node.SourceLinksList)
      {
        if (sourceLinks.ToNode != node)
          this.CreateLayers(node, sourceLinks.ToNode, level + 1, layer + 1);
        else
          this.CreateLayers(node, sourceLinks.FromNode, level + 1, layer + 1);
      }
    }
    if (level > 0 || node.DestinationLinksList == null)
      return;
    foreach (MapLayoutNetworkLink destinationLinks in node.DestinationLinksList)
    {
      if (destinationLinks.ToNode != node)
        this.CreateLayers(node, destinationLinks.ToNode, level - 1, layer - 1);
      else
        this.CreateLayers(node, destinationLinks.FromNode, level - 1, layer - 1);
    }
  }

  public bool IsLinesCross(
    float x11,
    float y11,
    float x12,
    float y12,
    float x21,
    float y21,
    float x22,
    float y22)
  {
    float num1 = Math.Max(x11, x12);
    float num2 = Math.Max(y11, y12);
    double num3 = (double) Math.Min(x11, x12);
    float num4 = Math.Min(y11, y12);
    float num5 = Math.Max(x21, x22);
    float num6 = Math.Max(y21, y22);
    float num7 = Math.Min(x21, x22);
    float num8 = Math.Min(y21, y22);
    double num9 = (double) num5;
    if (num3 > num9 || (double) num1 < (double) num7 || (double) num4 > (double) num6 || (double) num2 < (double) num8)
      return false;
    float num10 = x12 - x11;
    float num11 = y12 - y11;
    float num12 = x22 - x21;
    float num13 = y22 - y21;
    float num14 = x11 - x21;
    float num15 = y11 - y21;
    float num16;
    float num17;
    float num18;
    float num19;
    float num20;
    return (double) (num16 = (float) ((double) num13 * (double) num10 - (double) num12 * (double) num11)) != 0.0 && ((double) num16 <= 0.0 || (double) (num17 = (float) ((double) num10 * (double) num15 - (double) num11 * (double) num14)) >= 0.0 && (double) num17 <= (double) num16 && (double) (num18 = (float) ((double) num12 * (double) num15 - (double) num13 * (double) num14)) >= 0.0 && (double) num18 <= (double) num16) && (double) (num19 = -(float) ((double) num10 * (double) num15 - (double) num11 * (double) num14)) >= 0.0 && (double) num19 <= -(double) num16 && (double) (num20 = -(float) ((double) num12 * (double) num15 - (double) num13 * (double) num14)) >= 0.0 && (double) num20 <= -(double) num16;
  }

  public void UpdatePositions(RelVisualizerMapLayout layout, List<MapLayoutNetworkNode> nodes)
  {
    this.layers.Clear();
    this.maxLevel = int.MinValue;
    this.minLevel = int.MaxValue;
    this.maxCountOnLevel = 0;
    this.nodesList = new List<MapLayoutNetworkNode>();
    if (nodes.Count == 0)
      return;
    this.CreateLayers((MapLayoutNetworkNode) null, nodes[0], 0, 0);
    int num1 = this.maxLevel - this.minLevel + 1;
    int num2 = 100;
    int num3 = 5;
    int verticalSpace = HierarchicalLayout.VerticalSpace;
    int horizontalSpace = HierarchicalLayout.HorizontalSpace;
    int val1 = 0;
    if (num1 > 1)
      val1 = (this.winSize.Width - num2 * 2) / (num1 - 1);
    else
      num2 = this.winSize.Width / 2;
    int num4 = Math.Max(val1, horizontalSpace);
    int num5 = Math.Max((this.winSize.Height - num3 * 2) / (this.maxCountOnLevel + 1), verticalSpace);
    int num6 = Math.Max(this.winSize.Height, num3 + (this.maxCountOnLevel + 1) * num5);
    int num7 = num2;
    int num8 = 0;
    for (int minLevel = this.minLevel; minLevel <= this.maxLevel; ++minLevel)
    {
      int key = minLevel;
      if (this.layers.ContainsKey(key))
      {
        List<MapLayoutNetworkNode> layer = this.layers[key];
        int count = layer.Count;
        this.height = (num6 - num3 * 2) / (count + 1);
        this.height = Math.Max(this.height, verticalSpace);
        this.height = (int) ((double) this.height * HierarchicalLayout.YKoef);
        this.levelWidth = num4;
        int num9 = 10;
        int num10 = (int) Math.Ceiling((double) count / (double) num9);
        if (num10 == 0)
          num10 = 1;
        double num11 = 1.0;
        if ((double) key % 2.0 == 0.0)
          num11 = 1.3;
        this.levelWidth = Math.Max((int) ((double) (Math.Min(num4, horizontalSpace) * num10) * num11), num4);
        this.levelWidth = (int) ((double) this.levelWidth * HierarchicalLayout.XKoef);
        int x = key != this.minLevel ? (minLevel <= 0 ? num7 + num8 : num7 + this.levelWidth) : num7;
        num7 = x;
        num8 = this.levelWidth;
        for (int index = 0; index < count; ++index)
        {
          MapLayoutNetworkNode layoutNetworkNode = layer[index];
          int y = num3 + (index + 1) * this.height;
          PointF pointF = new PointF((float) x, (float) y);
          layoutNetworkNode.Center = pointF;
        }
      }
    }
    foreach (MapLayoutNetworkNode node in nodes)
      ;
  }

  private void CheckCenter(MapLayoutNetworkNode node, List<MapLayoutNetworkNode> nodes)
  {
    MapLayoutRelVisualizerNodeData nodeData1 = node.NodeData as MapLayoutRelVisualizerNodeData;
    MapLayoutNetworkNode parent = nodeData1.Parent;
    if (parent == null || nodeData1 == null)
      return;
    foreach (MapLayoutNetworkNode node1 in nodes)
    {
      if (node1 != node && node1.MapObject != null)
      {
        RectangleF bounds = node1.MapObject.Bounds;
        if (!(node1.NodeData is MapLayoutRelVisualizerNodeData nodeData2) || nodeData2.Level + 1 != nodeData1.Level)
        {
          PointF center = parent.Center;
          double x1 = (double) center.X;
          center = parent.Center;
          double y1 = (double) center.Y;
          center = node.Center;
          double x2 = (double) center.X;
          center = node.Center;
          double y2 = (double) center.Y;
          double left = (double) bounds.Left;
          double top1 = (double) bounds.Top;
          double right = (double) bounds.Right;
          double top2 = (double) bounds.Top;
          if (this.IsLinesCross((float) x1, (float) y1, (float) x2, (float) y2, (float) left, (float) top1, (float) right, (float) top2))
          {
            center = parent.Center;
            double num1 = (double) center.Y - ((double) bounds.Bottom + 15.0);
            center = parent.Center;
            double num2 = (double) center.X - (double) bounds.Right;
            double num3 = num1 / num2;
            center = node.Center;
            double x3 = (double) center.X;
            center = parent.Center;
            double x4 = (double) center.X;
            double num4 = x3 - x4;
            double num5 = num3 * num4;
            center = parent.Center;
            double x5 = (double) center.X;
            float y3 = (float) (num5 + x5);
            MapLayoutNetworkNode layoutNetworkNode = node;
            center = node.Center;
            PointF pointF = new PointF(center.X, y3);
            layoutNetworkNode.Center = pointF;
          }
        }
      }
    }
  }

  public void CheckCenterIntersect(MapLayoutNetworkNode node, List<MapLayoutNetworkNode> nodes)
  {
    if (node.MapObject == null)
      return;
    MapLayoutRelVisualizerNodeData nodeData1 = node.NodeData as MapLayoutRelVisualizerNodeData;
    RectangleF bounds1 = node.MapObject.Bounds;
    RectangleF rect = bounds1;
    if (nodeData1 == null)
      return;
    for (int index = 0; index < nodes.Count; ++index)
    {
      MapLayoutNetworkNode node1 = nodes[index];
      if (node1.NodeData is MapLayoutRelVisualizerNodeData nodeData2)
      {
        if (node1 != node)
        {
          if (node1 != null && node1 != node && nodeData2.Level <= nodeData1.Level)
          {
            RectangleF bounds2 = node1.MapObject.Bounds;
            if (bounds2.IntersectsWith(rect) || bounds2.Contains(rect) || rect.Contains(bounds2))
              rect = new RectangleF(rect.X, (float) ((double) bounds2.Y - (double) rect.Height - 20.0), rect.Width, rect.Height);
          }
        }
        else
          break;
      }
    }
    if (!(bounds1 != rect))
      return;
    PointF pointF = new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
    node.Center = pointF;
  }

  public int DistanceBetweenItems => this.levelWidth;
}
