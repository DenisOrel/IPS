// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.ForceDirected
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm.RelationVisualizer;
using Intermech.Localization;
using Intermech.Map;
using Intermech.Map.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class ForceDirected : ILayoutAlgorithm
{
  private PointF centralNodePosition = (PointF) Point.Empty;
  private int nomNodeInThisLevel;
  private int verticalSpace = 100;
  private int horizontalSpace = 180;
  private int nomLayerInThisTree;
  private int maxLayersCount;

  public bool isShowArrow() => false;

  public int MaxLayersCount
  {
    get => this.maxLayersCount;
    set => this.maxLayersCount = value;
  }

  public string GetAlgorithmName() => ForceDirected.AlgoritmName();

  public static string AlgoritmName() => LocalizationHolder.rm.GetString("Pdm_rv_14");

  public void NextLayer()
  {
    this.nomNodeInThisLevel = 0;
    ++this.nomLayerInThisTree;
  }

  public event MapLayoutProgressEventHandler LayoutProgress;

  public void RegistExistObject() => ++this.nomNodeInThisLevel;

  public PointF CalculateCentralNodePosition(Size winSize)
  {
    this.centralNodePosition = new PointF((float) (winSize.Width / 2), (float) (winSize.Height / 2));
    return this.centralNodePosition;
  }

  public void BeginChildLayout() => this.nomLayerInThisTree = 0;

  public void BeginParentLayout() => this.nomLayerInThisTree = 0;

  public PointF CalculateNodePosition(Size winSize, Random rnd, RelVisPred.RelVisLayers layer)
  {
    int num1 = -1;
    if (layer == RelVisPred.RelVisLayers.ParentTree)
      num1 = 1;
    double x = (double) this.centralNodePosition.X - (double) (this.nomLayerInThisTree * this.horizontalSpace * num1);
    float num2 = this.centralNodePosition.Y + (float) (this.nomNodeInThisLevel * this.verticalSpace * (this.nomNodeInThisLevel % 2 * -2 + 1));
    ++this.nomNodeInThisLevel;
    double y = (double) num2;
    return new PointF((float) x, (float) y);
  }

  public void LayoutDocument(MapDocument document, Size winSize)
  {
    MapLayoutForceDirected layoutForceDirected = new MapLayoutForceDirected();
    layoutForceDirected.Document = document;
    layoutForceDirected.RaiseProgress(7f);
    layoutForceDirected.Progress += new MapLayoutProgressEventHandler(this.DocumentLayoutProgress);
    layoutForceDirected.PerformLayout();
    layoutForceDirected.Progress -= new MapLayoutProgressEventHandler(this.DocumentLayoutProgress);
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

  public PointF UpdateGenerateNodePosition(Size winSize, Random rnd, RelVisPred.RelVisLayers layer)
  {
    return this.CalculateNodePosition(winSize, rnd, layer);
  }

  public void UpdatePositions(RelVisualizerMapLayout layout, List<MapLayoutNetworkNode> nodes)
  {
  }

  public int DistanceBetweenItems => 10000;
}
