// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayoutLayeredDigraph
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;

#nullable disable
namespace Intermech.Map.Layout;

public class MapLayoutLayeredDigraph : MapLayout
{
  private MapLayoutLayeredDigraphAggressive aggressiveOption;
  private int columnSpacing;
  private int component;
  private MapLayoutLayeredDigraphCycleRemove cycleremoveOption;
  private int DepthFirstSearchCycleRemovalTime;
  private MapLayoutDirection directionOption;
  private int[] indices;
  private MapLayoutLayeredDigraphInitIndices initializeOption;
  private int iterations;
  private MapLayoutLayeredDigraphLayering layeringOption;
  private int layerSpacing;
  private int maxColumn;
  private int maxIndex;
  private int maxIndexLayer;
  private int maxLayer;
  private int minIndexLayer;
  private MapLayoutNetworkNode[][] myCachedNodeArrays;
  private int[] myColumnsPS;
  private int[] myCrossings;
  private float[] myMedians;
  private int[] mySavedLayout;

  public MapLayoutLayeredDigraph()
  {
    this.layerSpacing = 25;
    this.columnSpacing = 25;
    this.directionOption = MapLayoutDirection.Right;
    this.cycleremoveOption = MapLayoutLayeredDigraphCycleRemove.DepthFirst;
    this.layeringOption = MapLayoutLayeredDigraphLayering.OptimalLinkLength;
    this.initializeOption = MapLayoutLayeredDigraphInitIndices.DepthFirstOut;
    this.iterations = 4;
    this.aggressiveOption = MapLayoutLayeredDigraphAggressive.Less;
    this.myCachedNodeArrays = new MapLayoutNetworkNode[100][];
  }

  protected virtual bool AdjacentExchangeCrossingReductionBendStraighten(
    int unfixedLayer,
    int directionCR,
    bool straighten,
    int directionBS)
  {
    MapLayoutNetworkNode[] cachedNodeArray = this.GetCachedNodeArray(unfixedLayer);
    int[] numArray1 = this.CrossingMatrix(unfixedLayer, directionCR);
    float[] numArray2 = this.Barycenters(unfixedLayer, -1);
    if (!straighten || directionBS > 0)
    {
      for (int index = 0; index < this.indices[unfixedLayer]; ++index)
        numArray2[index] = -1f;
    }
    float[] numArray3 = this.Barycenters(unfixedLayer, 1);
    if (!straighten || directionBS < 0)
    {
      for (int index = 0; index < this.indices[unfixedLayer]; ++index)
        numArray3[index] = -1f;
    }
    bool flag1 = false;
    bool flag2 = true;
    while (flag2)
    {
      flag2 = false;
      for (int index1 = 0; index1 < this.indices[unfixedLayer] - 1; ++index1)
      {
        int num1 = numArray1[this.NodeData(cachedNodeArray[index1]).Index * this.indices[unfixedLayer] + this.NodeData(cachedNodeArray[index1 + 1]).Index];
        int num2 = numArray1[this.NodeData(cachedNodeArray[index1 + 1]).Index * this.indices[unfixedLayer] + this.NodeData(cachedNodeArray[index1]).Index];
        int num3 = 0;
        int num4 = 0;
        int column1 = this.NodeData(cachedNodeArray[index1]).Column;
        int column2 = this.NodeData(cachedNodeArray[index1 + 1]).Column;
        int num5 = this.NodeMinColumnSpace(cachedNodeArray[index1]);
        int num6 = this.NodeMinColumnSpace(cachedNodeArray[index1 + 1]);
        int num7 = column1 - num5 + num6;
        int num8 = column2 - num5 + num6;
        float num9 = 0.0f;
        float num10 = 0.0f;
        if (straighten && (directionBS < 0 || directionBS == 0))
        {
          foreach (MapLayoutNetworkLink sourceLinks in cachedNodeArray[index1].SourceLinksList)
          {
            if (this.LinkData(sourceLinks).Valid && this.NodeData(sourceLinks.FromNode).Layer != unfixedLayer)
            {
              float num11 = this.LinkStraightenWeight(sourceLinks);
              int portFromColOffset = this.LinkData(sourceLinks).PortFromColOffset;
              int portToColOffset = this.LinkData(sourceLinks).PortToColOffset;
              int column3 = this.NodeData(sourceLinks.FromNode).Column;
              num9 += (float) (Math.Abs(column1 + portToColOffset - (column3 + portFromColOffset)) + 1) * num11;
              num10 += (float) (Math.Abs(num8 + portToColOffset - (column3 + portFromColOffset)) + 1) * num11;
            }
          }
        }
        foreach (MapLayoutNetworkLink sourceLinks in cachedNodeArray[index1].SourceLinksList)
        {
          if (this.LinkData(sourceLinks).Valid && this.NodeData(sourceLinks.FromNode).Layer == unfixedLayer)
          {
            MapLayoutNetworkNode fromNode = sourceLinks.FromNode;
            int index2 = 0;
            while (cachedNodeArray[index2] != fromNode)
              ++index2;
            if (index2 < index1)
            {
              num3 += 2 * (index1 - index2);
              num4 += 2 * (index1 + 1 - index2);
            }
            if (index2 == index1 + 1)
              ++num3;
            if (index2 > index1 + 1)
            {
              num3 += 4 * (index2 - index1);
              num4 += 4 * (index2 - (index1 + 1));
            }
          }
        }
        if (straighten && (directionBS > 0 || directionBS == 0))
        {
          foreach (MapLayoutNetworkLink destinationLinks in cachedNodeArray[index1].DestinationLinksList)
          {
            if (this.LinkData(destinationLinks).Valid && this.NodeData(destinationLinks.ToNode).Layer != unfixedLayer)
            {
              float num12 = this.LinkStraightenWeight(destinationLinks);
              int portFromColOffset = this.LinkData(destinationLinks).PortFromColOffset;
              int portToColOffset = this.LinkData(destinationLinks).PortToColOffset;
              int column4 = this.NodeData(destinationLinks.ToNode).Column;
              num9 += (float) (Math.Abs(column1 + portFromColOffset - (column4 + portToColOffset)) + 1) * num12;
              num10 += (float) (Math.Abs(num8 + portFromColOffset - (column4 + portToColOffset)) + 1) * num12;
            }
          }
        }
        foreach (MapLayoutNetworkLink destinationLinks in cachedNodeArray[index1].DestinationLinksList)
        {
          if (this.LinkData(destinationLinks).Valid && this.NodeData(destinationLinks.ToNode).Layer == unfixedLayer)
          {
            MapLayoutNetworkNode toNode = destinationLinks.ToNode;
            int index3 = 0;
            while (cachedNodeArray[index3] != toNode)
              ++index3;
            if (index3 == index1 + 1)
              ++num4;
          }
        }
        if (straighten && (directionBS < 0 || directionBS == 0))
        {
          foreach (MapLayoutNetworkLink sourceLinks in cachedNodeArray[index1 + 1].SourceLinksList)
          {
            if (this.LinkData(sourceLinks).Valid && this.NodeData(sourceLinks.FromNode).Layer != unfixedLayer)
            {
              float num13 = this.LinkStraightenWeight(sourceLinks);
              int portFromColOffset = this.LinkData(sourceLinks).PortFromColOffset;
              int portToColOffset = this.LinkData(sourceLinks).PortToColOffset;
              int column5 = this.NodeData(sourceLinks.FromNode).Column;
              num9 += (float) (Math.Abs(column2 + portToColOffset - (column5 + portFromColOffset)) + 1) * num13;
              num10 += (float) (Math.Abs(num7 + portToColOffset - (column5 + portFromColOffset)) + 1) * num13;
            }
          }
        }
        foreach (MapLayoutNetworkLink sourceLinks in cachedNodeArray[index1 + 1].SourceLinksList)
        {
          if (this.LinkData(sourceLinks).Valid && this.NodeData(sourceLinks.FromNode).Layer == unfixedLayer)
          {
            MapLayoutNetworkNode fromNode = sourceLinks.FromNode;
            int index4 = 0;
            while (cachedNodeArray[index4] != fromNode)
              ++index4;
            if (index4 < index1)
            {
              num3 += 2 * (index1 + 1 - index4);
              num4 += 2 * (index1 - index4);
            }
            if (index4 == index1)
              ++num4;
            if (index4 > index1 + 1)
            {
              num3 += 4 * (index4 - (index1 + 1));
              num4 += 4 * (index4 - index1);
            }
          }
        }
        if (straighten && (directionBS > 0 || directionBS == 0))
        {
          foreach (MapLayoutNetworkLink destinationLinks in cachedNodeArray[index1 + 1].DestinationLinksList)
          {
            if (this.LinkData(destinationLinks).Valid && this.NodeData(destinationLinks.ToNode).Layer != unfixedLayer)
            {
              float num14 = this.LinkStraightenWeight(destinationLinks);
              int portFromColOffset = this.LinkData(destinationLinks).PortFromColOffset;
              int portToColOffset = this.LinkData(destinationLinks).PortToColOffset;
              int column6 = this.NodeData(destinationLinks.ToNode).Column;
              num9 += (float) (Math.Abs(column2 + portFromColOffset - (column6 + portToColOffset)) + 1) * num14;
              num10 += (float) (Math.Abs(num7 + portFromColOffset - (column6 + portToColOffset)) + 1) * num14;
            }
          }
        }
        foreach (MapLayoutNetworkLink destinationLinks in cachedNodeArray[index1 + 1].DestinationLinksList)
        {
          if (this.LinkData(destinationLinks).Valid && this.NodeData(destinationLinks.ToNode).Layer == unfixedLayer)
          {
            MapLayoutNetworkNode toNode = destinationLinks.ToNode;
            int index5 = 0;
            while (cachedNodeArray[index5] != toNode)
              ++index5;
            if (index5 == index1)
              ++num3;
          }
        }
        float num15 = 0.0f;
        float num16 = 0.0f;
        float num17 = numArray2[this.NodeData(cachedNodeArray[index1]).Index];
        float num18 = numArray3[this.NodeData(cachedNodeArray[index1]).Index];
        float num19 = numArray2[this.NodeData(cachedNodeArray[index1 + 1]).Index];
        float num20 = numArray3[this.NodeData(cachedNodeArray[index1 + 1]).Index];
        if ((double) num17 != -1.0)
        {
          num15 += Math.Abs(num17 - (float) column1);
          num16 += Math.Abs(num17 - (float) num8);
        }
        if ((double) num18 != -1.0)
        {
          num15 += Math.Abs(num18 - (float) column1);
          num16 += Math.Abs(num18 - (float) num8);
        }
        if ((double) num19 != -1.0)
        {
          num15 += Math.Abs(num19 - (float) column2);
          num16 += Math.Abs(num19 - (float) num7);
        }
        if ((double) num20 != -1.0)
        {
          num15 += Math.Abs(num20 - (float) column2);
          num16 += Math.Abs(num20 - (float) num7);
        }
        if (num4 < num3 || num4 == num3 && num2 < num1 || num4 == num3 && num2 == num1 && (double) num10 < (double) num9 || num4 == num3 && num2 == num1 && (double) num10 == (double) num9 && (double) num16 < (double) num15)
        {
          flag1 = true;
          flag2 = true;
          this.NodeData(cachedNodeArray[index1]).Column = num8;
          this.NodeData(cachedNodeArray[index1 + 1]).Column = num7;
          MapLayoutNetworkNode layoutNetworkNode = cachedNodeArray[index1];
          cachedNodeArray[index1] = cachedNodeArray[index1 + 1];
          cachedNodeArray[index1 + 1] = layoutNetworkNode;
        }
      }
    }
    for (int index = 0; index < this.indices[unfixedLayer]; ++index)
      this.NodeData(cachedNodeArray[index]).Index = index;
    this.FreeCachedNodeArray(unfixedLayer, cachedNodeArray);
    return flag1;
  }

  protected virtual void AssignLayers()
  {
    switch (this.LayeringOption)
    {
      case MapLayoutLayeredDigraphLayering.LongestPathSink:
        this.LongestPathSinkLayering();
        break;
      case MapLayoutLayeredDigraphLayering.LongestPathSource:
        this.LongestPathSourceLayering();
        break;
      default:
        this.OptimalLinkLengthLayering();
        break;
    }
  }

  private void AssignLayersInternal()
  {
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      this.NodeData(node).Layer = -1;
    this.maxLayer = -1;
    this.AssignLayers();
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      this.maxLayer = Math.Max(this.maxLayer, this.NodeData(node).Layer);
  }

  protected virtual float[] Barycenters(int unfixedLayer, int direction)
  {
    MapLayoutNetworkNode[] cachedNodeArray = this.GetCachedNodeArray(unfixedLayer);
    float[] numArray = new float[this.indices[unfixedLayer]];
    for (int index1 = 0; index1 < this.indices[unfixedLayer]; ++index1)
    {
      ArrayList arrayList1 = (ArrayList) null;
      if (direction < 0 || direction == 0)
        arrayList1 = cachedNodeArray[index1].SourceLinksList;
      ArrayList arrayList2 = (ArrayList) null;
      if (direction > 0 || direction == 0)
        arrayList2 = cachedNodeArray[index1].DestinationLinksList;
      float num1 = 0.0f;
      int num2 = 0;
      if (arrayList1 != null)
      {
        for (int index2 = 0; index2 < arrayList1.Count; ++index2)
        {
          MapLayoutNetworkLink link = (MapLayoutNetworkLink) arrayList1[index2];
          if (this.LinkData(link).Valid && this.NodeData(link.FromNode).Layer != unfixedLayer)
          {
            num1 += (float) (this.NodeData(link.FromNode).Column + this.LinkData(link).PortFromColOffset);
            ++num2;
          }
        }
      }
      if (arrayList2 != null)
      {
        for (int index3 = 0; index3 < arrayList2.Count; ++index3)
        {
          MapLayoutNetworkLink link = (MapLayoutNetworkLink) arrayList2[index3];
          if (this.LinkData(link).Valid && this.NodeData(link.ToNode).Layer != unfixedLayer)
          {
            num1 += (float) (this.NodeData(link.ToNode).Column + this.LinkData(link).PortToColOffset);
            ++num2;
          }
        }
      }
      numArray[index1] = num2 != 0 ? num1 / (float) num2 : -1f;
    }
    this.FreeCachedNodeArray(unfixedLayer, cachedNodeArray);
    return numArray;
  }

  protected virtual float Bends(int unfixedLayer, int direction, bool weighted)
  {
    MapLayoutNetworkNode[] cachedNodeArray = this.GetCachedNodeArray(unfixedLayer);
    float num1 = 0.0f;
    for (int index1 = 0; index1 < this.indices[unfixedLayer]; ++index1)
    {
      ArrayList arrayList1 = (ArrayList) null;
      if (direction < 0 || direction == 0)
        arrayList1 = cachedNodeArray[index1].SourceLinksList;
      ArrayList arrayList2 = (ArrayList) null;
      if (direction > 0 || direction == 0)
        arrayList2 = cachedNodeArray[index1].DestinationLinksList;
      if (arrayList1 != null)
      {
        for (int index2 = 0; index2 < arrayList1.Count; ++index2)
        {
          MapLayoutNetworkLink link = (MapLayoutNetworkLink) arrayList1[index2];
          if (this.LinkData(link).Valid && this.NodeData(link.FromNode).Layer != unfixedLayer)
          {
            float num2 = (float) (this.NodeData(link.FromNode).Column + this.LinkData(link).PortFromColOffset);
            float num3 = (float) (this.NodeData(link.ToNode).Column + this.LinkData(link).PortToColOffset);
            if (weighted)
              num1 += Math.Abs(num2 - num3) * this.LinkStraightenWeight(link);
            else
              num1 += Math.Abs(num2 - num3);
          }
        }
      }
      if (arrayList2 != null)
      {
        for (int index3 = 0; index3 < arrayList2.Count; ++index3)
        {
          MapLayoutNetworkLink link = (MapLayoutNetworkLink) arrayList2[index3];
          if (this.LinkData(link).Valid && this.NodeData(link.ToNode).Layer != unfixedLayer)
          {
            float num4 = (float) (this.NodeData(link.FromNode).Column + this.LinkData(link).PortFromColOffset);
            float num5 = (float) (this.NodeData(link.ToNode).Column + this.LinkData(link).PortToColOffset);
            if (weighted)
              num1 += (Math.Abs(num4 - num5) + 1f) * this.LinkStraightenWeight(link);
            else
              num1 += Math.Abs(num4 - num5);
          }
        }
      }
    }
    this.FreeCachedNodeArray(unfixedLayer, cachedNodeArray);
    return num1;
  }

  protected virtual bool BendStraighten(int unfixedLayer, int direction)
  {
    bool flag = false;
    while (this.ShiftBendStraighten(unfixedLayer, direction) || this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, 0, true, direction))
      flag = true;
    return flag;
  }

  private void ClearCaches()
  {
    this.maxIndex = -1;
    this.minIndexLayer = 0;
    this.maxIndexLayer = 0;
    this.mySavedLayout = (int[]) null;
    this.myMedians = (float[]) null;
    this.myColumnsPS = (int[]) null;
    this.myCrossings = (int[]) null;
    for (int index = 0; index < this.myCachedNodeArrays.Length; ++index)
      this.myCachedNodeArrays[index] = (MapLayoutNetworkNode[]) null;
  }

  protected virtual void ComponentPack(int direction)
  {
    this.TightPack();
    if (direction > 0)
    {
      for (int column = 0; column <= this.maxColumn; ++column)
      {
        int[] layout = this.SaveLayout();
        float num1 = this.CountBends(true);
        float num2 = num1 + 1f;
        while ((double) num1 < (double) num2)
        {
          num2 = num1;
          this.ComponentPackAux(column, 1);
          float num3 = this.CountBends(true);
          if ((double) num3 > (double) num1)
            this.RestoreLayout(layout);
          else if ((double) num3 < (double) num1)
          {
            num1 = num3;
            layout = this.SaveLayout();
          }
        }
      }
    }
    if (direction < 0)
    {
      for (int maxColumn = this.maxColumn; maxColumn >= 0; --maxColumn)
      {
        int[] layout = this.SaveLayout();
        float num4 = this.CountBends(true);
        float num5 = num4 + 1f;
        while ((double) num4 < (double) num5)
        {
          num5 = num4;
          this.ComponentPackAux(maxColumn, -1);
          float num6 = this.CountBends(true);
          if ((double) num6 > (double) num4)
            this.RestoreLayout(layout);
          else if ((double) num6 < (double) num4)
          {
            num4 = num6;
            layout = this.SaveLayout();
          }
        }
      }
    }
    this.Normalize();
  }

  protected virtual bool ComponentPackAux(int column, int direction)
  {
    this.component = 0;
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      this.NodeData(node).Component = -1;
    if (direction > 0)
    {
      foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      {
        if (this.NodeData(node).Column - this.NodeMinColumnSpace(node) <= column)
          this.NodeData(node).Component = this.component;
      }
    }
    if (direction < 0)
    {
      foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      {
        if (this.NodeData(node).Column + this.NodeMinColumnSpace(node) >= column)
          this.NodeData(node).Component = this.component;
      }
    }
    ++this.component;
    MapLayoutNodeEnumerator nodes = this.Network.Nodes;
    foreach (MapLayoutNetworkNode node in nodes)
    {
      if (this.NodeData(node).Component == -1)
      {
        this.ComponentUnset(node, this.component, -1, true, true);
        ++this.component;
      }
    }
    bool[] flagArray1 = new bool[this.component * this.component];
    for (int index = 0; index < this.component * this.component; ++index)
      flagArray1[index] = false;
    int[] numArray = new int[(this.maxLayer + 1) * (this.maxColumn + 1)];
    for (int index = 0; index < (this.maxLayer + 1) * (this.maxColumn + 1); ++index)
      numArray[index] = -1;
    nodes = this.Network.Nodes;
    foreach (MapLayoutNetworkNode node in nodes)
    {
      int layer = this.NodeData(node).Layer;
      int num1 = this.NodeMinColumnSpace(node);
      int num2 = Math.Max(0, this.NodeData(node).Column - num1);
      int num3 = Math.Min(this.maxColumn, this.NodeData(node).Column + num1);
      for (int index = num2; index <= num3; ++index)
        numArray[layer * (this.maxColumn + 1) + index] = this.NodeData(node).Component;
    }
    for (int index1 = 0; index1 <= this.maxLayer; ++index1)
    {
      if (direction > 0)
      {
        for (int index2 = 0; index2 < this.maxColumn; ++index2)
        {
          if (numArray[index1 * (this.maxColumn + 1) + index2] != -1 && numArray[index1 * (this.maxColumn + 1) + index2 + 1] != -1 && numArray[index1 * (this.maxColumn + 1) + index2] != numArray[index1 * (this.maxColumn + 1) + index2 + 1])
            flagArray1[numArray[index1 * (this.maxColumn + 1) + index2] * this.component + numArray[index1 * (this.maxColumn + 1) + index2 + 1]] = true;
        }
      }
      if (direction < 0)
      {
        for (int maxColumn = this.maxColumn; maxColumn > 0; --maxColumn)
        {
          if (numArray[index1 * (this.maxColumn + 1) + maxColumn] != -1 && numArray[index1 * (this.maxColumn + 1) + maxColumn - 1] != -1 && numArray[index1 * (this.maxColumn + 1) + maxColumn] != numArray[index1 * (this.maxColumn + 1) + maxColumn - 1])
            flagArray1[numArray[index1 * (this.maxColumn + 1) + maxColumn] * this.component + numArray[index1 * (this.maxColumn + 1) + maxColumn - 1]] = true;
        }
      }
    }
    bool[] flagArray2 = new bool[this.component];
    for (int index = 0; index < this.component; ++index)
      flagArray2[index] = true;
    ArrayList arrayList = new ArrayList();
    arrayList.Add((object) 0);
    while (arrayList.Count != 0)
    {
      int index3 = (int) arrayList[arrayList.Count - 1];
      arrayList.RemoveAt(arrayList.Count - 1);
      if (flagArray2[index3])
      {
        flagArray2[index3] = false;
        for (int index4 = 0; index4 < this.component; ++index4)
        {
          if (flagArray1[index3 * this.component + index4])
            arrayList.Insert(0, (object) index4);
        }
      }
    }
    bool flag = false;
    if (direction > 0)
    {
      nodes = this.Network.Nodes;
      foreach (MapLayoutNetworkNode node in nodes)
      {
        if (flagArray2[this.NodeData(node).Component])
        {
          MapLayoutLayeredDigraphNodeData layeredDigraphNodeData = this.NodeData(node);
          this.NodeData(node).Column = layeredDigraphNodeData.Column - 1;
          flag = true;
        }
      }
    }
    if (direction >= 0)
      return flag;
    nodes = this.Network.Nodes;
    foreach (MapLayoutNetworkNode node in nodes)
    {
      if (flagArray2[this.NodeData(node).Component])
      {
        MapLayoutLayeredDigraphNodeData layeredDigraphNodeData = this.NodeData(node);
        this.NodeData(node).Column = layeredDigraphNodeData.Column + 1;
        flag = true;
      }
    }
    return flag;
  }

  protected virtual void ComponentUnset(
    MapLayoutNetworkNode node,
    int component,
    int unset,
    bool forward,
    bool backward)
  {
    if (this.NodeData(node).Component != unset)
      return;
    this.NodeData(node).Component = component;
    if (forward)
    {
      foreach (MapLayoutNetworkLink destinationLinks in node.DestinationLinksList)
        this.ComponentUnset(destinationLinks.ToNode, component, unset, forward, backward);
    }
    if (!backward)
      return;
    foreach (MapLayoutNetworkLink sourceLinks in node.SourceLinksList)
      this.ComponentUnset(sourceLinks.FromNode, component, unset, forward, backward);
  }

  protected virtual float CountBends(bool weighted)
  {
    float num = 0.0f;
    for (int unfixedLayer = 0; unfixedLayer <= this.maxLayer; ++unfixedLayer)
      num += this.Bends(unfixedLayer, 1, weighted);
    return num;
  }

  protected virtual int CountCrossings()
  {
    int num = 0;
    for (int unfixedLayer = 0; unfixedLayer <= this.maxLayer; ++unfixedLayer)
    {
      int[] numArray = this.CrossingMatrix(unfixedLayer, 1);
      for (int index1 = 0; index1 < this.indices[unfixedLayer]; ++index1)
      {
        for (int index2 = index1; index2 < this.indices[unfixedLayer]; ++index2)
          num += numArray[index1 * this.indices[unfixedLayer] + index2];
      }
    }
    return num;
  }

  protected virtual int[] CrossingMatrix(int unfixedLayer, int direction)
  {
    MapLayoutNetworkNode[] cachedNodeArray = this.GetCachedNodeArray(unfixedLayer);
    if (this.myCrossings == null || this.myCrossings.Length < this.indices[unfixedLayer] * this.indices[unfixedLayer])
      this.myCrossings = new int[this.indices[unfixedLayer] * this.indices[unfixedLayer]];
    int[] crossings = this.myCrossings;
    for (int index1 = 0; index1 < this.indices[unfixedLayer]; ++index1)
    {
      int num1 = 0;
      if (direction > 0 || direction == 0)
      {
        ArrayList sourceLinksList = cachedNodeArray[index1].SourceLinksList;
        for (int index2 = 0; index2 < sourceLinksList.Count; ++index2)
        {
          MapLayoutNetworkLink link1 = (MapLayoutNetworkLink) sourceLinksList[index2];
          if (this.LinkData(link1).Valid && this.NodeData(link1.FromNode).Layer != unfixedLayer)
          {
            int index3 = this.NodeData(link1.FromNode).Index;
            int portToPos1 = this.LinkData(link1).PortToPos;
            int portFromPos1 = this.LinkData(link1).PortFromPos;
            for (int index4 = index2 + 1; index4 < sourceLinksList.Count; ++index4)
            {
              MapLayoutNetworkLink link2 = (MapLayoutNetworkLink) sourceLinksList[index4];
              if (this.LinkData(link2).Valid && this.NodeData(link2.FromNode).Layer != unfixedLayer)
              {
                int index5 = this.NodeData(link2.FromNode).Index;
                int portToPos2 = this.LinkData(link2).PortToPos;
                int portFromPos2 = this.LinkData(link2).PortFromPos;
                if (portToPos1 < portToPos2 && (index3 > index5 || index3 == index5 && portFromPos1 > portFromPos2))
                  ++num1;
                if (portToPos2 < portToPos1 && (index5 > index3 || index5 == index3 && portFromPos2 > portFromPos1))
                  ++num1;
              }
            }
          }
        }
      }
      if (direction < 0 || direction == 0)
      {
        ArrayList destinationLinksList = cachedNodeArray[index1].DestinationLinksList;
        for (int index6 = 0; index6 < destinationLinksList.Count; ++index6)
        {
          MapLayoutNetworkLink link3 = (MapLayoutNetworkLink) destinationLinksList[index6];
          if (this.LinkData(link3).Valid && this.NodeData(link3.ToNode).Layer != unfixedLayer)
          {
            int index7 = this.NodeData(link3.ToNode).Index;
            int portToPos3 = this.LinkData(link3).PortToPos;
            int portFromPos3 = this.LinkData(link3).PortFromPos;
            for (int index8 = index6 + 1; index8 < destinationLinksList.Count; ++index8)
            {
              MapLayoutNetworkLink link4 = (MapLayoutNetworkLink) destinationLinksList[index8];
              if (this.LinkData(link4).Valid && this.NodeData(link4.ToNode).Layer != unfixedLayer)
              {
                int index9 = this.NodeData(link4.ToNode).Index;
                int portToPos4 = this.LinkData(link4).PortToPos;
                int portFromPos4 = this.LinkData(link4).PortFromPos;
                if (portFromPos3 < portFromPos4 && (index7 > index9 || index7 == index9 && portToPos3 > portToPos4))
                  ++num1;
                if (portFromPos4 < portFromPos3 && (index9 > index7 || index9 == index7 && portToPos4 > portToPos3))
                  ++num1;
              }
            }
          }
        }
      }
      crossings[index1 * this.indices[unfixedLayer] + index1] = num1;
      for (int index10 = index1 + 1; index10 < this.indices[unfixedLayer]; ++index10)
      {
        int num2 = 0;
        int num3 = 0;
        if (direction > 0 || direction == 0)
        {
          ArrayList sourceLinksList1 = cachedNodeArray[index1].SourceLinksList;
          ArrayList sourceLinksList2 = cachedNodeArray[index10].SourceLinksList;
          for (int index11 = 0; index11 < sourceLinksList1.Count; ++index11)
          {
            MapLayoutNetworkLink link5 = (MapLayoutNetworkLink) sourceLinksList1[index11];
            if (this.LinkData(link5).Valid && this.NodeData(link5.FromNode).Layer != unfixedLayer)
            {
              int index12 = this.NodeData(link5.FromNode).Index;
              int portToPos5 = this.LinkData(link5).PortToPos;
              int portFromPos5 = this.LinkData(link5).PortFromPos;
              for (int index13 = 0; index13 < sourceLinksList2.Count; ++index13)
              {
                MapLayoutNetworkLink link6 = (MapLayoutNetworkLink) sourceLinksList2[index13];
                if (this.LinkData(link6).Valid && this.NodeData(link6.FromNode).Layer != unfixedLayer)
                {
                  int index14 = this.NodeData(link6.FromNode).Index;
                  int portToPos6 = this.LinkData(link6).PortToPos;
                  int portFromPos6 = this.LinkData(link6).PortFromPos;
                  if (index12 < index14 || index12 == index14 && portFromPos5 < portFromPos6)
                    ++num3;
                  if (index14 < index12 || index14 == index12 && portFromPos6 < portFromPos5)
                    ++num2;
                }
              }
            }
          }
        }
        if (direction < 0 || direction == 0)
        {
          ArrayList destinationLinksList1 = cachedNodeArray[index1].DestinationLinksList;
          ArrayList destinationLinksList2 = cachedNodeArray[index10].DestinationLinksList;
          for (int index15 = 0; index15 < destinationLinksList1.Count; ++index15)
          {
            MapLayoutNetworkLink link7 = (MapLayoutNetworkLink) destinationLinksList1[index15];
            if (this.LinkData(link7).Valid && this.NodeData(link7.ToNode).Layer != unfixedLayer)
            {
              int index16 = this.NodeData(link7.ToNode).Index;
              int portToPos7 = this.LinkData(link7).PortToPos;
              int portFromPos7 = this.LinkData(link7).PortFromPos;
              for (int index17 = 0; index17 < destinationLinksList2.Count; ++index17)
              {
                MapLayoutNetworkLink link8 = (MapLayoutNetworkLink) destinationLinksList2[index17];
                if (this.LinkData(link8).Valid && this.NodeData(link8.ToNode).Layer != unfixedLayer)
                {
                  int index18 = this.NodeData(link8.ToNode).Index;
                  int portToPos8 = this.LinkData(link8).PortToPos;
                  int portFromPos8 = this.LinkData(link8).PortFromPos;
                  if (index16 < index18 || index16 == index18 && portToPos7 < portToPos8)
                    ++num3;
                  if (index18 < index16 || index18 == index16 && portToPos8 < portToPos7)
                    ++num2;
                }
              }
            }
          }
        }
        crossings[index1 * this.indices[unfixedLayer] + index10] = num2;
        crossings[index10 * this.indices[unfixedLayer] + index1] = num3;
      }
    }
    this.FreeCachedNodeArray(unfixedLayer, cachedNodeArray);
    return crossings;
  }

  protected virtual void DepthFirstInInitializeIndices()
  {
    for (int index = 0; index <= this.maxLayer; ++index)
    {
      foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      {
        if (this.NodeData(node).Layer == index && this.NodeData(node).Index == -1)
          this.DepthFirstInInitializeIndicesVisit(node);
      }
    }
  }

  protected virtual void DepthFirstInInitializeIndicesVisit(MapLayoutNetworkNode node)
  {
    int layer = this.NodeData(node).Layer;
    this.NodeData(node).Index = this.indices[layer];
    ++this.indices[layer];
    object[] array = node.SourceLinksList.ToArray();
    bool flag = true;
    while (flag)
    {
      flag = false;
      for (int index = 0; index < array.Length - 1; ++index)
      {
        MapLayoutNetworkLink link1 = (MapLayoutNetworkLink) array[index];
        MapLayoutNetworkLink link2 = (MapLayoutNetworkLink) array[index + 1];
        if (this.LinkData(link1).PortToColOffset > this.LinkData(link2).PortToColOffset)
        {
          flag = true;
          array[index] = (object) link2;
          array[index + 1] = (object) link1;
        }
      }
    }
    for (int index = 0; index < array.Length; ++index)
    {
      MapLayoutNetworkLink link = (MapLayoutNetworkLink) array[index];
      if (this.LinkData(link).Valid)
      {
        MapLayoutNetworkNode fromNode = link.FromNode;
        if (this.NodeData(fromNode).Index == -1)
          this.DepthFirstInInitializeIndicesVisit(fromNode);
      }
    }
  }

  protected virtual void DepthFirstOutInitializeIndices()
  {
    for (int maxLayer = this.maxLayer; maxLayer >= 0; --maxLayer)
    {
      foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      {
        if (this.NodeData(node).Layer == maxLayer && this.NodeData(node).Index == -1)
          this.DepthFirstOutInitializeIndicesVisit(node);
      }
    }
  }

  protected virtual void DepthFirstOutInitializeIndicesVisit(MapLayoutNetworkNode node)
  {
    int layer = this.NodeData(node).Layer;
    this.NodeData(node).Index = this.indices[layer];
    ++this.indices[layer];
    object[] array = node.DestinationLinksList.ToArray();
    bool flag = true;
    while (flag)
    {
      flag = false;
      for (int index = 0; index < array.Length - 1; ++index)
      {
        MapLayoutNetworkLink link1 = (MapLayoutNetworkLink) array[index];
        MapLayoutNetworkLink link2 = (MapLayoutNetworkLink) array[index + 1];
        if (this.LinkData(link1).PortFromColOffset > this.LinkData(link2).PortFromColOffset)
        {
          flag = true;
          array[index] = (object) link2;
          array[index + 1] = (object) link1;
        }
      }
    }
    for (int index = 0; index < array.Length; ++index)
    {
      MapLayoutNetworkLink link = (MapLayoutNetworkLink) array[index];
      if (this.LinkData(link).Valid)
      {
        MapLayoutNetworkNode toNode = link.ToNode;
        if (this.NodeData(toNode).Index == -1)
          this.DepthFirstOutInitializeIndicesVisit(toNode);
      }
    }
  }

  protected virtual void DepthFirstSearchCycleRemoval()
  {
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      this.NodeData(node).Discover = -1;
      this.NodeData(node).Finish = -1;
    }
    foreach (MapLayoutNetworkLink link in this.Network.Links)
      this.LinkData(link).Forest = false;
    this.DepthFirstSearchCycleRemovalTime = 0;
    MapLayoutNodeEnumerator nodes = this.Network.Nodes;
    foreach (MapLayoutNetworkNode node in nodes)
    {
      if (node.SourceLinksList.Count == 0)
        this.DepthFirstSearchCycleRemovalVisit(node);
    }
    nodes = this.Network.Nodes;
    foreach (MapLayoutNetworkNode node in nodes)
    {
      if (this.NodeData(node).Discover == -1)
        this.DepthFirstSearchCycleRemovalVisit(node);
    }
    foreach (MapLayoutNetworkLink link in this.Network.Links)
    {
      if (!this.LinkData(link).Forest)
      {
        MapLayoutNetworkNode fromNode = link.FromNode;
        int discover1 = this.NodeData(fromNode).Discover;
        int finish1 = this.NodeData(fromNode).Finish;
        MapLayoutNetworkNode toNode = link.ToNode;
        int discover2 = this.NodeData(toNode).Discover;
        int finish2 = this.NodeData(toNode).Finish;
        int num = discover1;
        if (discover2 < num && finish1 < finish2)
        {
          this.Network.ReverseLink(link);
          this.LinkData(link).Rev = true;
        }
      }
    }
  }

  protected virtual void DepthFirstSearchCycleRemovalVisit(MapLayoutNetworkNode node)
  {
    this.NodeData(node).Discover = this.DepthFirstSearchCycleRemovalTime;
    ++this.DepthFirstSearchCycleRemovalTime;
    foreach (MapLayoutNetworkLink destinationLinks in node.DestinationLinksList)
    {
      MapLayoutNetworkNode toNode = destinationLinks.ToNode;
      if (this.NodeData(toNode).Discover == -1)
      {
        this.LinkData(destinationLinks).Forest = true;
        this.DepthFirstSearchCycleRemovalVisit(toNode);
      }
    }
    this.NodeData(node).Finish = this.DepthFirstSearchCycleRemovalTime;
    ++this.DepthFirstSearchCycleRemovalTime;
  }

  protected virtual bool EqualLayout(int[] layoutA, int[] layoutB)
  {
    bool flag = true;
    if (layoutA.Length != layoutB.Length)
      return false;
    for (int index = 0; index < layoutA.Length; ++index)
      flag = flag && layoutA[index] == layoutB[index];
    return flag;
  }

  private void FreeCachedNodeArray(int unfixedLayer, MapLayoutNetworkNode[] nodes)
  {
    this.myCachedNodeArrays[this.indices[unfixedLayer]] = nodes;
  }

  private MapLayoutNetworkNode[] GetCachedNodeArray(int unfixedLayer)
  {
    if (this.indices[unfixedLayer] >= this.myCachedNodeArrays.Length)
    {
      MapLayoutNetworkNode[][] layoutNetworkNodeArray = new MapLayoutNetworkNode[this.indices[unfixedLayer] + 50][];
      for (int index = 0; index < this.myCachedNodeArrays.Length; ++index)
        layoutNetworkNodeArray[index] = this.myCachedNodeArrays[index];
      this.myCachedNodeArrays = layoutNetworkNodeArray;
    }
    MapLayoutNetworkNode[] cachedNodeArray;
    if (this.myCachedNodeArrays[this.indices[unfixedLayer]] == null)
    {
      cachedNodeArray = new MapLayoutNetworkNode[this.indices[unfixedLayer]];
    }
    else
    {
      cachedNodeArray = this.myCachedNodeArrays[this.indices[unfixedLayer]];
      this.myCachedNodeArrays[this.indices[unfixedLayer]] = (MapLayoutNetworkNode[]) null;
    }
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      if (this.NodeData(node).Layer == unfixedLayer)
        cachedNodeArray[this.NodeData(node).Index] = node;
    }
    return cachedNodeArray;
  }

  public virtual int[] GetIndices() => this.indices;

  protected virtual void GreedyCycleRemoval()
  {
    int index1 = 0;
    int index2 = this.Network.NodeCount - 1;
    MapLayoutNetworkNode[] layoutNetworkNodeArray = new MapLayoutNetworkNode[index2 + 1];
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      this.NodeData(node).Valid = true;
    while (this.GreedyCycleRemovalFindNode(this.Network) != null)
    {
      for (MapLayoutNetworkNode sink = this.GreedyCycleRemovalFindSink(this.Network); sink != null; sink = this.GreedyCycleRemovalFindSink(this.Network))
      {
        layoutNetworkNodeArray[index2] = sink;
        --index2;
        this.NodeData(sink).Valid = false;
      }
      for (MapLayoutNetworkNode source = this.GreedyCycleRemovalFindSource(this.Network); source != null; source = this.GreedyCycleRemovalFindSource(this.Network))
      {
        layoutNetworkNodeArray[index1] = source;
        ++index1;
        this.NodeData(source).Valid = false;
      }
      MapLayoutNetworkNode nodeMaxDegDiff = this.GreedyCycleRemovalFindNodeMaxDegDiff(this.Network);
      if (nodeMaxDegDiff != null)
      {
        layoutNetworkNodeArray[index1] = nodeMaxDegDiff;
        ++index1;
        this.NodeData(nodeMaxDegDiff).Valid = false;
      }
    }
    for (int index3 = 0; index3 < this.Network.NodeCount; ++index3)
      this.NodeData(layoutNetworkNodeArray[index3]).Index = index3;
    foreach (MapLayoutNetworkLink link in this.Network.Links)
    {
      if (this.NodeData(link.FromNode).Index > this.NodeData(link.ToNode).Index)
      {
        this.Network.ReverseLink(link);
        this.LinkData(link).Rev = true;
      }
    }
  }

  protected virtual MapLayoutNetworkNode GreedyCycleRemovalFindNode(MapLayoutNetwork network)
  {
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      if (this.NodeData(node).Valid)
        return node;
    }
    return (MapLayoutNetworkNode) null;
  }

  protected virtual MapLayoutNetworkNode GreedyCycleRemovalFindNodeMaxDegDiff(
    MapLayoutNetwork network)
  {
    MapLayoutNetworkNode nodeMaxDegDiff = (MapLayoutNetworkNode) null;
    int num1 = 0;
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      if (this.NodeData(node).Valid)
      {
        int num2 = 0;
        foreach (MapLayoutNetworkLink destinationLinks in node.DestinationLinksList)
        {
          if (this.NodeData(destinationLinks.ToNode).Valid)
            ++num2;
        }
        int num3 = 0;
        foreach (MapLayoutNetworkLink sourceLinks in node.SourceLinksList)
        {
          if (this.NodeData(sourceLinks.FromNode).Valid)
            ++num3;
        }
        if (nodeMaxDegDiff == null || num1 < num2 - num3)
        {
          nodeMaxDegDiff = node;
          num1 = num2 - num3;
        }
      }
    }
    return nodeMaxDegDiff;
  }

  protected virtual MapLayoutNetworkNode GreedyCycleRemovalFindSink(MapLayoutNetwork network)
  {
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      if (this.NodeData(node).Valid)
      {
        bool flag = true;
        foreach (MapLayoutNetworkLink destinationLinks in node.DestinationLinksList)
        {
          if (this.NodeData(destinationLinks.ToNode).Valid)
            flag = false;
        }
        if (flag)
          return node;
      }
    }
    return (MapLayoutNetworkNode) null;
  }

  protected virtual MapLayoutNetworkNode GreedyCycleRemovalFindSource(MapLayoutNetwork network)
  {
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      if (this.NodeData(node).Valid)
      {
        bool flag = true;
        foreach (MapLayoutNetworkLink sourceLinks in node.SourceLinksList)
        {
          if (this.NodeData(sourceLinks.FromNode).Valid)
            flag = false;
        }
        if (flag)
          return node;
      }
    }
    return (MapLayoutNetworkNode) null;
  }

  protected virtual void InitializeColumns()
  {
    this.maxColumn = -1;
    for (int unfixedLayer = 0; unfixedLayer <= this.maxLayer; ++unfixedLayer)
    {
      MapLayoutNetworkNode[] cachedNodeArray = this.GetCachedNodeArray(unfixedLayer);
      int num1 = 0;
      for (int index = 0; index < this.indices[unfixedLayer]; ++index)
      {
        int num2 = this.NodeMinColumnSpace(cachedNodeArray[index]);
        int num3 = num1 + num2;
        this.NodeData(cachedNodeArray[index]).Column = num3;
        num1 = num3 + 1 + num2;
      }
      this.maxColumn = Math.Max(this.maxColumn, num1 - 1);
      this.FreeCachedNodeArray(unfixedLayer, cachedNodeArray);
    }
  }

  protected virtual void InitializeIndices()
  {
    switch (this.InitializeOption)
    {
      case MapLayoutLayeredDigraphInitIndices.DepthFirstOut:
        this.DepthFirstOutInitializeIndices();
        break;
      case MapLayoutLayeredDigraphInitIndices.DepthFirstIn:
        this.DepthFirstInInitializeIndices();
        break;
      default:
        this.NaiveInitializeIndices();
        break;
    }
  }

  private void InitializeIndicesInternal()
  {
    this.indices = new int[this.maxLayer + 1];
    for (int index = 0; index <= this.maxLayer; ++index)
      this.indices[index] = 0;
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      this.NodeData(node).Index = -1;
    this.InitializeIndices();
    this.maxIndex = -1;
    this.minIndexLayer = 0;
    this.maxIndexLayer = 0;
    for (int index = 0; index <= this.maxLayer; ++index)
    {
      if (this.indices[index] > this.indices[this.maxIndexLayer])
      {
        this.maxIndex = this.indices[index] - 1;
        this.maxIndexLayer = index;
      }
      if (this.indices[index] < this.indices[this.minIndexLayer])
        this.minIndexLayer = index;
    }
  }

  protected virtual void LayoutLinks()
  {
    foreach (MapLayoutNetworkLink link in this.Network.Links)
    {
      if (link.MapObject != null && !this.LinkData(link).Valid)
      {
        MapStroke stroke = link.Stroke;
        IMapLink mapObject = link.MapObject as IMapLink;
        stroke.ClearPoints();
        int i1 = 1;
        bool flag = false;
        if (stroke is MapLink mapLink)
        {
          mapLink.CalculateStroke();
          i1 = stroke.FirstPickIndex + 1;
          if (mapLink.Orthogonal)
          {
            flag = true;
            mapLink.RemovePoint(2);
            mapLink.RemovePoint(2);
          }
        }
        else if (mapObject != null)
        {
          IMapPort fromPort = mapObject.FromPort;
          if (fromPort != null && fromPort.MapObject != null)
            stroke.AddPoint(fromPort.MapObject.Center);
          IMapPort toPort = mapObject.ToPort;
          if (toPort != null && toPort.MapObject != null)
            stroke.AddPoint(toPort.MapObject.Center);
        }
        MapLayoutNetworkNode layoutNetworkNode1 = link.FromNode;
        MapLayoutNetworkNode layoutNetworkNode2 = link.ToNode;
        if (!this.LinkData(link).Rev)
        {
          MapLayoutNetworkNode layoutNetworkNode3;
          for (; layoutNetworkNode1 != null && layoutNetworkNode1 != layoutNetworkNode2; layoutNetworkNode1 = layoutNetworkNode3)
          {
            layoutNetworkNode3 = (MapLayoutNetworkNode) null;
            foreach (MapLayoutNetworkLink destinationLinks in layoutNetworkNode1.DestinationLinksList)
            {
              if (destinationLinks.MapObject == link.MapObject)
                layoutNetworkNode3 = destinationLinks.ToNode;
            }
            if (layoutNetworkNode3 != layoutNetworkNode2)
            {
              PointF point = stroke.GetPoint(i1 - 1);
              PointF center = layoutNetworkNode3.Center;
              if (flag)
              {
                int num = 0;
                if (this.DirectionOption == MapLayoutDirection.Left || this.DirectionOption == MapLayoutDirection.Right)
                {
                  float x = (float) (((double) point.X + (double) center.X) / 2.0);
                  if ((double) point.X != (double) center.X)
                    x += (float) num;
                  int i2 = i1 + 1;
                  stroke.InsertPoint(i2, new PointF(x, point.Y));
                  i1 = i2 + 1;
                  stroke.InsertPoint(i1, new PointF(x, center.Y));
                }
                else
                {
                  float y = (float) (((double) point.Y + (double) center.Y) / 2.0);
                  if ((double) point.Y != (double) center.Y)
                    y += (float) num;
                  int i3 = i1 + 1;
                  stroke.InsertPoint(i3, new PointF(point.X, y));
                  i1 = i3 + 1;
                  stroke.InsertPoint(i1, new PointF(center.X, y));
                }
              }
              else
              {
                ++i1;
                stroke.InsertPoint(i1, center);
              }
            }
          }
          if (flag)
          {
            PointF point1 = stroke.GetPoint(i1 - 1);
            PointF point2 = stroke.GetPoint(i1);
            if (this.DirectionOption == MapLayoutDirection.Left || this.DirectionOption == MapLayoutDirection.Right)
            {
              int i4 = i1 + 1;
              stroke.InsertPoint(i4, new PointF(point2.X, point1.Y));
              int i5 = i4 + 1;
              stroke.InsertPoint(i5, point2);
            }
            else
            {
              int i6 = i1 + 1;
              stroke.InsertPoint(i6, new PointF(point1.X, point2.Y));
              int i7 = i6 + 1;
              stroke.InsertPoint(i7, point2);
            }
          }
        }
        else
        {
          MapLayoutNetworkNode layoutNetworkNode4;
          for (; layoutNetworkNode2 != null && layoutNetworkNode1 != layoutNetworkNode2; layoutNetworkNode2 = layoutNetworkNode4)
          {
            layoutNetworkNode4 = (MapLayoutNetworkNode) null;
            foreach (MapLayoutNetworkLink sourceLinks in layoutNetworkNode2.SourceLinksList)
            {
              if (sourceLinks.MapObject == link.MapObject)
                layoutNetworkNode4 = sourceLinks.FromNode;
            }
            if (layoutNetworkNode4 != layoutNetworkNode1)
            {
              PointF point = stroke.GetPoint(i1 - 1);
              PointF center = layoutNetworkNode4.Center;
              if (flag)
              {
                int num = 0;
                if (this.DirectionOption == MapLayoutDirection.Left || this.DirectionOption == MapLayoutDirection.Right)
                {
                  if (i1 == 2)
                  {
                    int i8 = i1 + 1;
                    stroke.InsertPoint(i8, new PointF(point.X, point.Y));
                    i1 = i8 + 1;
                    stroke.InsertPoint(i1, new PointF(point.X, center.Y));
                  }
                  else
                  {
                    float x = (float) (((double) point.X + (double) center.X) / 2.0);
                    if ((double) point.X != (double) center.X)
                      x += (float) num;
                    int i9 = i1 + 1;
                    stroke.InsertPoint(i9, new PointF(x, point.Y));
                    i1 = i9 + 1;
                    stroke.InsertPoint(i1, new PointF(x, center.Y));
                  }
                }
                else if (i1 == 2)
                {
                  int i10 = i1 + 1;
                  stroke.InsertPoint(i10, new PointF(point.X, point.Y));
                  i1 = i10 + 1;
                  stroke.InsertPoint(i1, new PointF(center.X, point.Y));
                }
                else
                {
                  float y = (float) (((double) point.Y + (double) center.Y) / 2.0);
                  if ((double) point.Y != (double) center.Y)
                    y += (float) num;
                  int i11 = i1 + 1;
                  stroke.InsertPoint(i11, new PointF(point.X, y));
                  i1 = i11 + 1;
                  stroke.InsertPoint(i1, new PointF(center.X, y));
                }
              }
              else
              {
                ++i1;
                stroke.InsertPoint(i1, center);
              }
            }
          }
          if (flag)
          {
            PointF point3 = stroke.GetPoint(i1 - 1);
            PointF point4 = stroke.GetPoint(i1);
            if (this.DirectionOption == MapLayoutDirection.Left || this.DirectionOption == MapLayoutDirection.Right)
            {
              if ((double) point3.Y == (double) point4.Y)
              {
                int i12 = i1 + 1;
                stroke.InsertPoint(i12, new PointF(point3.X, point3.Y - 30f));
                int i13 = i12 + 1;
                stroke.InsertPoint(i13, new PointF(point4.X, point3.Y - 30f));
                int i14 = i13 + 1;
                stroke.InsertPoint(i14, new PointF(point4.X, point3.Y));
                int i15 = i14 + 1;
                stroke.InsertPoint(i15, point4);
              }
              else
              {
                int i16 = i1 + 1;
                stroke.InsertPoint(i16, new PointF(point4.X, point3.Y));
                int i17 = i16 + 1;
                stroke.InsertPoint(i17, point4);
              }
            }
            else if ((double) point3.X == (double) point4.X)
            {
              int i18 = i1 + 1;
              stroke.InsertPoint(i18, new PointF(point3.X - 30f, point3.Y));
              int i19 = i18 + 1;
              stroke.InsertPoint(i19, new PointF(point3.X - 30f, point4.Y));
              int i20 = i19 + 1;
              stroke.InsertPoint(i20, new PointF(point3.X, point4.Y));
              int i21 = i20 + 1;
              stroke.InsertPoint(i21, point4);
            }
            else
            {
              int i22 = i1 + 1;
              stroke.InsertPoint(i22, new PointF(point3.X, point4.Y));
              int i23 = i22 + 1;
              stroke.InsertPoint(i23, point4);
            }
          }
        }
        if (mapObject != null)
        {
          if (mapObject.FromPort is MapPort fromPort)
            stroke.SetPoint(0, fromPort.GetFromLinkPoint(mapObject));
          if (mapObject.ToPort is MapPort toPort)
            stroke.SetPoint(stroke.PointsCount - 1, toPort.GetToLinkPoint(mapObject));
        }
        link.CommitPosition();
      }
    }
  }

  protected virtual void LayoutNodes()
  {
    int[] numArray1 = new int[this.maxLayer + 1];
    for (int index = 0; index <= this.maxLayer; ++index)
      numArray1[index] = 0;
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      int layer = this.NodeData(node).Layer;
      int val2 = this.NodeMinLayerSpace(node);
      numArray1[layer] = Math.Max(numArray1[layer], val2);
    }
    int[] numArray2 = new int[this.maxLayer + 1];
    int num1 = 0;
    for (int index = 0; index <= this.maxLayer; ++index)
    {
      int num2 = num1 + numArray1[index];
      numArray2[index] = num2;
      num1 = num2 + 1 + numArray1[index];
    }
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      int layer = this.NodeData(node).Layer;
      int column = this.NodeData(node).Column;
      int x;
      int y;
      switch (this.DirectionOption)
      {
        case MapLayoutDirection.Down:
          x = this.ColumnSpacing * (column + 1);
          y = this.LayerSpacing * (num1 - numArray2[layer]);
          break;
        case MapLayoutDirection.Left:
          x = this.LayerSpacing * (numArray2[layer] + 1);
          y = this.ColumnSpacing * (column + 1);
          break;
        case MapLayoutDirection.Up:
          x = this.ColumnSpacing * (column + 1);
          y = this.LayerSpacing * (numArray2[layer] + 1);
          break;
        default:
          x = this.LayerSpacing * (num1 - numArray2[layer]);
          y = this.ColumnSpacing * (column + 1);
          break;
      }
      node.Center = new PointF((float) x, (float) y);
      node.CommitPosition();
    }
  }

  public virtual void LayoutNodesAndLinks()
  {
    this.Document.RaiseChanging(220, 0, (object) null);
    this.Document.SuspendsUpdates = true;
    this.LayoutNodes();
    this.LayoutLinks();
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

  private MapLayoutLayeredDigraphLinkData LinkData(MapLayoutNetworkLink link)
  {
    return (MapLayoutLayeredDigraphLinkData) link.LinkData;
  }

  protected virtual float LinkLengthWeight(MapLayoutNetworkLink link) => 1f;

  protected virtual int LinkMinLength(MapLayoutNetworkLink link)
  {
    MapLayoutNetworkNode fromNode = link.FromNode;
    MapLayoutNetworkNode toNode = link.ToNode;
    int num = 0;
    foreach (MapLayoutNetworkLink destinationLinks in fromNode.DestinationLinksList)
    {
      if (destinationLinks.ToNode == toNode)
        ++num;
    }
    return num > 1 ? 2 : 1;
  }

  protected virtual float LinkStraightenWeight(MapLayoutNetworkLink link)
  {
    MapLayoutNetworkNode fromNode = link.FromNode;
    MapLayoutNetworkNode toNode = link.ToNode;
    if (fromNode.MapObject == null && toNode.MapObject == null)
      return 8f;
    return fromNode.MapObject == null || toNode.MapObject == null ? 4f : 1f;
  }

  protected virtual void LongestPathSinkLayering()
  {
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      this.maxLayer = Math.Max(this.LongestPathSinkLayeringLength(node), this.maxLayer);
  }

  protected virtual int LongestPathSinkLayeringLength(MapLayoutNetworkNode node)
  {
    int val1 = 0;
    if (this.NodeData(node).Layer != -1)
      return this.NodeData(node).Layer;
    foreach (MapLayoutNetworkLink destinationLinks in node.DestinationLinksList)
    {
      int num = this.LinkMinLength(destinationLinks);
      val1 = Math.Max(val1, this.LongestPathSinkLayeringLength(destinationLinks.ToNode) + num);
    }
    this.NodeData(node).Layer = val1;
    return val1;
  }

  protected virtual void LongestPathSourceLayering()
  {
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      this.maxLayer = Math.Max(this.LongestPathSourceLayeringLength(node), this.maxLayer);
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      this.NodeData(node).Layer = this.maxLayer - this.NodeData(node).Layer;
  }

  protected virtual int LongestPathSourceLayeringLength(MapLayoutNetworkNode node)
  {
    int val1 = 0;
    if (this.NodeData(node).Layer != -1)
      return this.NodeData(node).Layer;
    foreach (MapLayoutNetworkLink sourceLinks in node.SourceLinksList)
    {
      int num = this.LinkMinLength(sourceLinks);
      val1 = Math.Max(val1, this.LongestPathSourceLayeringLength(sourceLinks.FromNode) + num);
    }
    this.NodeData(node).Layer = val1;
    return val1;
  }

  protected virtual void MakeProper()
  {
    ArrayList linksArray = this.Network.LinksArray;
    for (int index = 0; index < linksArray.Count; ++index)
      this.LinkData((MapLayoutNetworkLink) linksArray[index]).Valid = false;
    for (int index1 = 0; index1 < linksArray.Count; ++index1)
    {
      MapLayoutNetworkLink link1 = (MapLayoutNetworkLink) linksArray[index1];
      if (!this.LinkData(link1).Valid)
      {
        int num1 = 0;
        int num2 = num1;
        int num3 = num2;
        int num4 = num3;
        if (link1.MapObject != null)
        {
          PointF center1 = link1.FromNode.Center;
          PointF center2 = link1.ToNode.Center;
          MapObject mapObject1 = link1.FromPort.MapObject;
          MapObject mapObject2 = link1.ToPort.MapObject;
          PointF pointF1 = mapObject1 != null ? mapObject1.Center : center1;
          PointF pointF2 = mapObject2 != null ? mapObject2.Center : center2;
          switch (this.DirectionOption)
          {
            case MapLayoutDirection.Down:
            case MapLayoutDirection.Up:
              num4 = (int) Math.Round(((double) pointF1.X - (double) center1.X) / (double) this.ColumnSpacing);
              num2 = (int) pointF1.X;
              num3 = (int) Math.Round(((double) pointF2.X - (double) center2.X) / (double) this.ColumnSpacing);
              num1 = (int) pointF2.X;
              break;
            default:
              num4 = (int) Math.Round(((double) pointF1.Y - (double) center1.Y) / (double) this.ColumnSpacing);
              num2 = (int) pointF1.Y;
              num3 = (int) Math.Round(((double) pointF2.Y - (double) center2.Y) / (double) this.ColumnSpacing);
              num1 = (int) pointF2.Y;
              break;
          }
          this.LinkData(link1).PortFromColOffset = num4;
          this.LinkData(link1).PortFromPos = num2;
          this.LinkData(link1).PortToColOffset = num3;
          this.LinkData(link1).PortToPos = num1;
        }
        else
        {
          this.LinkData(link1).PortFromColOffset = 0;
          this.LinkData(link1).PortFromPos = 0;
          this.LinkData(link1).PortToColOffset = 0;
          this.LinkData(link1).PortToPos = 0;
        }
        MapLayoutNetworkNode fromNode1 = link1.FromNode;
        MapLayoutNetworkNode toNode = link1.ToNode;
        int layer1 = this.NodeData(fromNode1).Layer;
        int layer2 = this.NodeData(toNode).Layer;
        int num5 = (!(link1.Stroke is MapLink stroke) ? 0 : (stroke.Orthogonal ? 1 : 0)) == 0 || !this.LinkData(link1).Rev ? 1 : 0;
        if (layer1 - layer2 > num5 && layer1 > 0)
        {
          this.LinkData(link1).Valid = false;
          MapLayoutNetworkNode layoutNetworkNode = new MapLayoutNetworkNode();
          layoutNetworkNode.Network = this.Network;
          layoutNetworkNode.MapObject = (MapObject) null;
          layoutNetworkNode.NodeData = (object) new MapLayoutLayeredDigraphNodeData();
          this.NodeData(layoutNetworkNode).Layer = layer1 - 1;
          this.Network.AddNode(layoutNetworkNode);
          MapLayoutNetworkLink link2 = this.Network.LinkNodes(fromNode1, layoutNetworkNode, link1.MapObject);
          link2.LinkData = (object) new MapLayoutLayeredDigraphLinkData();
          this.LinkData(link2).Valid = true;
          this.LinkData(link2).Rev = this.LinkData(link1).Rev;
          this.LinkData(link2).PortFromColOffset = num4;
          this.LinkData(link2).PortToColOffset = 0;
          this.LinkData(link2).PortFromPos = num2;
          this.LinkData(link2).PortToPos = 0;
          MapLayoutNetworkNode fromNode2 = layoutNetworkNode;
          for (int index2 = layer1 - 1; index2 - layer2 > num5 && index2 > 0; --index2)
          {
            layoutNetworkNode = new MapLayoutNetworkNode();
            layoutNetworkNode.Network = this.Network;
            layoutNetworkNode.MapObject = (MapObject) null;
            layoutNetworkNode.NodeData = (object) new MapLayoutLayeredDigraphNodeData();
            this.NodeData(layoutNetworkNode).Layer = index2 - 1;
            this.Network.AddNode(layoutNetworkNode);
            MapLayoutNetworkLink link3 = this.Network.LinkNodes(fromNode2, layoutNetworkNode, link1.MapObject);
            link3.LinkData = (object) new MapLayoutLayeredDigraphLinkData();
            this.LinkData(link3).Valid = true;
            this.LinkData(link3).Rev = this.LinkData(link1).Rev;
            this.LinkData(link3).PortFromColOffset = 0;
            this.LinkData(link3).PortToColOffset = 0;
            this.LinkData(link3).PortFromPos = 0;
            this.LinkData(link3).PortToPos = 0;
            fromNode2 = layoutNetworkNode;
          }
          MapLayoutNetworkLink link4 = this.Network.LinkNodes(layoutNetworkNode, toNode, link1.MapObject);
          link4.LinkData = (object) new MapLayoutLayeredDigraphLinkData();
          this.LinkData(link4).Valid = true;
          this.LinkData(link4).Rev = this.LinkData(link1).Rev;
          this.LinkData(link4).PortFromColOffset = 0;
          this.LinkData(link4).PortToColOffset = num3;
          this.LinkData(link4).PortFromPos = 0;
          this.LinkData(link4).PortToPos = num1;
        }
        else
          this.LinkData(link1).Valid = true;
      }
    }
  }

  protected virtual bool MedianBarycenterCrossingReduction(int unfixedLayer, int direction)
  {
    bool flag1 = false;
    MapLayoutNetworkNode[] cachedNodeArray = this.GetCachedNodeArray(unfixedLayer);
    float[] numArray1 = this.Medians(unfixedLayer, direction);
    float[] numArray2 = this.Barycenters(unfixedLayer, direction);
    for (int index = 0; index < this.indices[unfixedLayer]; ++index)
    {
      if ((double) numArray2[index] == -1.0)
        numArray2[index] = (float) this.NodeData(cachedNodeArray[index]).Column;
      if ((double) numArray1[index] == -1.0)
        numArray1[index] = (float) this.NodeData(cachedNodeArray[index]).Column;
    }
    bool flag2 = true;
    while (flag2)
    {
      flag2 = false;
      for (int index = 0; index < this.indices[unfixedLayer] - 1; ++index)
      {
        if ((double) numArray1[index + 1] < (double) numArray1[index] || (double) numArray1[index + 1] == (double) numArray1[index] && (double) numArray2[index + 1] < (double) numArray2[index])
        {
          flag1 = true;
          flag2 = true;
          float num1 = numArray1[index];
          numArray1[index] = numArray1[index + 1];
          numArray1[index + 1] = num1;
          float num2 = numArray2[index];
          numArray2[index] = numArray2[index + 1];
          numArray2[index + 1] = num2;
          MapLayoutNetworkNode layoutNetworkNode = cachedNodeArray[index];
          cachedNodeArray[index] = cachedNodeArray[index + 1];
          cachedNodeArray[index + 1] = layoutNetworkNode;
        }
      }
    }
    for (int index = 0; index < this.indices[unfixedLayer]; ++index)
      this.NodeData(cachedNodeArray[index]).Index = index;
    int num3 = 0;
    for (int index = 0; index < this.indices[unfixedLayer]; ++index)
    {
      int num4 = this.NodeMinColumnSpace(cachedNodeArray[index]);
      int num5 = num3 + num4;
      this.NodeData(cachedNodeArray[index]).Column = num5;
      num3 = num5 + 1 + num4;
    }
    this.FreeCachedNodeArray(unfixedLayer, cachedNodeArray);
    return flag1;
  }

  protected virtual float[] Medians(int unfixedLayer, int direction)
  {
    MapLayoutNetworkNode[] cachedNodeArray = this.GetCachedNodeArray(unfixedLayer);
    if (this.myMedians == null || this.myMedians.Length < this.indices[unfixedLayer])
      this.myMedians = new float[2 * this.indices[unfixedLayer]];
    float[] medians = this.myMedians;
    for (int index1 = 0; index1 < this.indices[unfixedLayer]; ++index1)
    {
      ArrayList arrayList1 = (ArrayList) null;
      if (direction < 0 || direction == 0)
        arrayList1 = cachedNodeArray[index1].SourceLinksList;
      ArrayList arrayList2 = (ArrayList) null;
      if (direction > 0 || direction == 0)
        arrayList2 = cachedNodeArray[index1].DestinationLinksList;
      int num1 = 0;
      if (arrayList1 != null)
      {
        for (int index2 = 0; index2 < arrayList1.Count; ++index2)
        {
          MapLayoutNetworkLink link = (MapLayoutNetworkLink) arrayList1[index2];
          if (this.LinkData(link).Valid && this.NodeData(link.FromNode).Layer != unfixedLayer)
            ++num1;
        }
      }
      if (arrayList2 != null)
      {
        for (int index3 = 0; index3 < arrayList2.Count; ++index3)
        {
          MapLayoutNetworkLink link = (MapLayoutNetworkLink) arrayList2[index3];
          if (this.LinkData(link).Valid && this.NodeData(link.ToNode).Layer != unfixedLayer)
            ++num1;
        }
      }
      if (num1 == 0)
      {
        medians[index1] = -1f;
      }
      else
      {
        if (this.myColumnsPS == null || this.myColumnsPS.Length < num1)
          this.myColumnsPS = new int[2 * num1];
        int index4 = 0;
        if (arrayList1 != null)
        {
          for (int index5 = 0; index5 < arrayList1.Count; ++index5)
          {
            MapLayoutNetworkLink link = (MapLayoutNetworkLink) arrayList1[index5];
            if (this.LinkData(link).Valid && this.NodeData(link.FromNode).Layer != unfixedLayer)
            {
              this.myColumnsPS[index4] = this.NodeData(link.FromNode).Column + this.LinkData(link).PortFromColOffset;
              ++index4;
            }
          }
        }
        if (arrayList2 != null)
        {
          for (int index6 = 0; index6 < arrayList2.Count; ++index6)
          {
            MapLayoutNetworkLink link = (MapLayoutNetworkLink) arrayList2[index6];
            if (this.LinkData(link).Valid && this.NodeData(link.ToNode).Layer != unfixedLayer)
            {
              this.myColumnsPS[index4] = this.NodeData(link.ToNode).Column + this.LinkData(link).PortToColOffset;
              ++index4;
            }
          }
        }
        bool flag = true;
        while (flag)
        {
          flag = false;
          for (int index7 = 0; index7 < index4 - 1; ++index7)
          {
            if (this.myColumnsPS[index7] > this.myColumnsPS[index7 + 1])
            {
              flag = true;
              int num2 = this.myColumnsPS[index7 + 1];
              this.myColumnsPS[index7 + 1] = this.myColumnsPS[index7];
              this.myColumnsPS[index7] = num2;
            }
          }
        }
        int index8 = index4 / 2;
        medians[index1] = index4 % 2 != 1 ? (float) (((double) this.myColumnsPS[index8 - 1] + (double) this.myColumnsPS[index8]) / 2.0) : (float) this.myColumnsPS[index8];
      }
    }
    this.FreeCachedNodeArray(unfixedLayer, cachedNodeArray);
    return medians;
  }

  protected virtual bool MedianStraighten(int unfixedLayer, int direction)
  {
    MapLayoutNetworkNode[] cachedNodeArray = this.GetCachedNodeArray(unfixedLayer);
    float[] numArray1 = this.Medians(unfixedLayer, direction);
    int[] numArray2 = new int[this.indices[unfixedLayer]];
    for (int index = 0; index < this.indices[unfixedLayer]; ++index)
      numArray2[index] = (int) numArray1[index];
    bool flag1 = false;
    bool flag2 = true;
    while (flag2)
    {
      flag2 = false;
      for (int index = 0; index < this.indices[unfixedLayer]; ++index)
      {
        int column1 = this.NodeData(cachedNodeArray[index]).Column;
        int num1 = this.NodeMinColumnSpace(cachedNodeArray[index]);
        int num2 = 0;
        if (numArray2[index] == -1)
        {
          if (index == 0 && index == this.indices[unfixedLayer] - 1)
            num2 = column1;
          else if (index == 0)
            num2 = this.NodeData(cachedNodeArray[index + 1]).Column - column1 != num1 + this.NodeMinColumnSpace(cachedNodeArray[index + 1]) ? column1 : column1 - 1;
          else if (index == this.indices[unfixedLayer] - 1)
          {
            int column2 = this.NodeData(cachedNodeArray[index - 1]).Column;
            num2 = column1 - column2 != num1 + this.NodeMinColumnSpace(cachedNodeArray[index - 1]) ? column1 : column1 + 1;
          }
          else
            num2 = (this.NodeData(cachedNodeArray[index - 1]).Column + this.NodeMinColumnSpace(cachedNodeArray[index - 1]) + num1 + 1 + (this.NodeData(cachedNodeArray[index + 1]).Column - this.NodeMinColumnSpace(cachedNodeArray[index + 1]) - num1 - 1)) / 2;
        }
        else if (index == 0 && index == this.indices[unfixedLayer] - 1)
          num2 = numArray2[index];
        else if (index == 0)
        {
          int val2 = this.NodeData(cachedNodeArray[index + 1]).Column - this.NodeMinColumnSpace(cachedNodeArray[index + 1]) - num1 - 1;
          num2 = Math.Min(numArray2[index], val2);
        }
        else if (index == this.indices[unfixedLayer] - 1)
        {
          int val2 = this.NodeData(cachedNodeArray[index - 1]).Column + this.NodeMinColumnSpace(cachedNodeArray[index - 1]) + num1 + 1;
          num2 = Math.Max(numArray2[index], val2);
        }
        else
        {
          int num3 = this.NodeData(cachedNodeArray[index - 1]).Column + this.NodeMinColumnSpace(cachedNodeArray[index - 1]) + num1 + 1;
          int num4 = this.NodeData(cachedNodeArray[index + 1]).Column - this.NodeMinColumnSpace(cachedNodeArray[index + 1]) - num1 - 1;
          if (num3 < numArray2[index] && numArray2[index] < num4)
            num2 = numArray2[index];
          else if (num3 >= numArray2[index])
            num2 = num3;
          else if (num4 <= numArray2[index])
            num2 = num4;
        }
        if (num2 != column1)
        {
          flag1 = true;
          flag2 = true;
          this.NodeData(cachedNodeArray[index]).Column = num2;
        }
      }
    }
    this.FreeCachedNodeArray(unfixedLayer, cachedNodeArray);
    this.Normalize();
    return flag1;
  }

  protected virtual void NaiveInitializeIndices()
  {
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      int layer = this.NodeData(node).Layer;
      this.NodeData(node).Index = this.indices[layer];
      ++this.indices[layer];
    }
  }

  private MapLayoutLayeredDigraphNodeData NodeData(MapLayoutNetworkNode node)
  {
    return (MapLayoutLayeredDigraphNodeData) node.NodeData;
  }

  protected virtual int NodeMinColumnSpace(MapLayoutNetworkNode node)
  {
    int num = 0;
    if (node.MapObject == null)
      return num;
    MapObject mapObject = node.MapObject;
    switch (this.DirectionOption)
    {
      case MapLayoutDirection.Down:
      case MapLayoutDirection.Up:
        return (int) mapObject.Width / 2 / this.ColumnSpacing + 1;
      default:
        return (int) mapObject.Height / 2 / this.ColumnSpacing + 1;
    }
  }

  protected virtual int NodeMinLayerSpace(MapLayoutNetworkNode node)
  {
    int num = 0;
    if (node.MapObject == null)
      return num;
    MapObject mapObject = node.MapObject;
    switch (this.DirectionOption)
    {
      case MapLayoutDirection.Down:
      case MapLayoutDirection.Up:
        return (int) mapObject.Height / 2 / this.LayerSpacing + 1;
      default:
        return (int) mapObject.Width / 2 / this.LayerSpacing + 1;
    }
  }

  protected virtual void Normalize()
  {
    int val1 = int.MaxValue;
    this.maxColumn = -1;
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      int num = this.NodeMinColumnSpace(node);
      val1 = Math.Min(val1, this.NodeData(node).Column - num);
      this.maxColumn = Math.Max(this.maxColumn, this.NodeData(node).Column + num);
    }
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      MapLayoutLayeredDigraphNodeData layeredDigraphNodeData = this.NodeData(node);
      this.NodeData(node).Column = layeredDigraphNodeData.Column - val1;
    }
    this.maxColumn -= val1;
  }

  protected virtual void OptimalLinkLengthLayering()
  {
    this.LongestPathSinkLayering();
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      this.NodeData(node).Valid = false;
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      if (node.SourceLinksList.Count == 0)
        this.OptimalLinkLengthLayeringDepthFirstSearch(node);
    }
    int val1 = int.MaxValue;
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      val1 = Math.Min(val1, this.NodeData(node).Layer);
    this.maxLayer = -1;
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      MapLayoutLayeredDigraphNodeData layeredDigraphNodeData = this.NodeData(node);
      this.NodeData(node).Layer = layeredDigraphNodeData.Layer - val1;
      this.maxLayer = Math.Max(this.maxLayer, this.NodeData(node).Layer);
    }
  }

  protected virtual void OptimalLinkLengthLayeringDepthFirstSearch(MapLayoutNetworkNode node)
  {
    if (this.NodeData(node).Valid)
      return;
    this.NodeData(node).Valid = true;
    foreach (MapLayoutNetworkLink destinationLinks in node.DestinationLinksList)
      this.OptimalLinkLengthLayeringDepthFirstSearch(destinationLinks.ToNode);
    this.OptimalLinkLengthLayeringPull(node);
    this.OptimalLinkLengthLayeringPush(node);
  }

  protected virtual void OptimalLinkLengthLayeringPull(MapLayoutNetworkNode node)
  {
    foreach (MapLayoutNetworkNode node1 in this.Network.Nodes)
      this.NodeData(node1).Component = -1;
    foreach (MapLayoutNetworkLink sourceLinks in node.SourceLinksList)
    {
      int num = this.LinkMinLength(sourceLinks);
      if (this.NodeData(sourceLinks.FromNode).Layer - this.NodeData(sourceLinks.ToNode).Layer > num)
        this.TightComponentUnset(sourceLinks.FromNode, 0, -1, true, false);
    }
    this.TightComponentUnset(node, 1, -1, true, true);
    while (this.NodeData(node).Component != 0)
    {
      float num1 = 0.0f;
      int val1 = int.MaxValue;
      float num2 = 0.0f;
      MapLayoutNetworkNode node2 = (MapLayoutNetworkNode) null;
      foreach (MapLayoutNetworkNode node3 in this.Network.Nodes)
      {
        if (this.NodeData(node3).Component == 1)
        {
          float num3 = 0.0f;
          bool flag = false;
          foreach (MapLayoutNetworkLink sourceLinks in node3.SourceLinksList)
          {
            MapLayoutNetworkNode fromNode = sourceLinks.FromNode;
            num3 += this.LinkLengthWeight(sourceLinks);
            if (this.NodeData(fromNode).Component != 1)
            {
              num1 += this.LinkLengthWeight(sourceLinks);
              int num4 = this.NodeData(fromNode).Layer - this.NodeData(node3).Layer;
              int num5 = this.LinkMinLength(sourceLinks);
              val1 = Math.Min(val1, num4 - num5);
            }
          }
          foreach (MapLayoutNetworkLink destinationLinks in node3.DestinationLinksList)
          {
            MapLayoutNetworkNode toNode = destinationLinks.ToNode;
            num3 -= this.LinkLengthWeight(destinationLinks);
            if (this.NodeData(toNode).Component != 1)
              num1 -= this.LinkLengthWeight(destinationLinks);
            else
              flag = true;
          }
          if ((node2 == null || (double) num3 < (double) num2) && !flag)
          {
            node2 = node3;
            num2 = num3;
          }
        }
      }
      if ((double) num1 > 0.0)
      {
        foreach (MapLayoutNetworkNode node4 in this.Network.Nodes)
        {
          if (this.NodeData(node4).Component == 1)
          {
            MapLayoutLayeredDigraphNodeData layeredDigraphNodeData = this.NodeData(node4);
            this.NodeData(node4).Layer = layeredDigraphNodeData.Layer + val1;
          }
        }
        this.NodeData(node).Component = 0;
      }
      else
        this.NodeData(node2).Component = 0;
    }
  }

  protected virtual void OptimalLinkLengthLayeringPush(MapLayoutNetworkNode node)
  {
    foreach (MapLayoutNetworkNode node1 in this.Network.Nodes)
      this.NodeData(node1).Component = -1;
    this.TightComponentUnset(node, 1, -1, true, false);
    while (this.NodeData(node).Component != 0)
    {
      float num1 = 0.0f;
      int val1 = int.MaxValue;
      float num2 = 0.0f;
      MapLayoutNetworkNode node2 = (MapLayoutNetworkNode) null;
      foreach (MapLayoutNetworkNode node3 in this.Network.Nodes)
      {
        if (this.NodeData(node3).Component == 1)
        {
          float num3 = 0.0f;
          bool flag = false;
          foreach (MapLayoutNetworkLink sourceLinks in node3.SourceLinksList)
          {
            MapLayoutNetworkNode fromNode = sourceLinks.FromNode;
            num3 += this.LinkLengthWeight(sourceLinks);
            if (this.NodeData(fromNode).Component != 1)
              num1 += this.LinkLengthWeight(sourceLinks);
            else
              flag = true;
          }
          foreach (MapLayoutNetworkLink destinationLinks in node3.DestinationLinksList)
          {
            MapLayoutNetworkNode toNode = destinationLinks.ToNode;
            num3 -= this.LinkLengthWeight(destinationLinks);
            if (this.NodeData(toNode).Component != 1)
            {
              num1 -= this.LinkLengthWeight(destinationLinks);
              int num4 = this.NodeData(node3).Layer - this.NodeData(toNode).Layer;
              int num5 = this.LinkMinLength(destinationLinks);
              val1 = Math.Min(val1, num4 - num5);
            }
          }
          if ((node2 == null || (double) num3 > (double) num2) && !flag)
          {
            node2 = node3;
            num2 = num3;
          }
        }
      }
      if ((double) num1 < 0.0)
      {
        foreach (MapLayoutNetworkNode node4 in this.Network.Nodes)
        {
          if (this.NodeData(node4).Component == 1)
          {
            MapLayoutLayeredDigraphNodeData layeredDigraphNodeData = this.NodeData(node4);
            this.NodeData(node4).Layer = layeredDigraphNodeData.Layer - val1;
          }
        }
        this.NodeData(node).Component = 0;
      }
      else
        this.NodeData(node2).Component = 0;
    }
  }

  protected virtual void Pack()
  {
    for (int column = 0; column <= this.maxColumn; ++column)
    {
      do
        ;
      while (this.PackAux(column, 1));
    }
    this.Normalize();
  }

  protected virtual bool PackAux(int column, int direction)
  {
    bool flag1 = true;
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      int num = this.NodeMinColumnSpace(node);
      if (this.NodeData(node).Column - num <= column && this.NodeData(node).Column + num >= column)
        flag1 = false;
    }
    bool flag2 = false;
    if (!flag1)
      return flag2;
    if (direction > 0)
    {
      foreach (MapLayoutNetworkNode node in this.Network.Nodes)
      {
        if (this.NodeData(node).Column > column)
        {
          MapLayoutLayeredDigraphNodeData layeredDigraphNodeData = this.NodeData(node);
          this.NodeData(node).Column = layeredDigraphNodeData.Column - 1;
          flag2 = true;
        }
      }
    }
    if (direction >= 0)
      return flag2;
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      if (this.NodeData(node).Column < column)
      {
        MapLayoutLayeredDigraphNodeData layeredDigraphNodeData = this.NodeData(node);
        this.NodeData(node).Column = layeredDigraphNodeData.Column + 1;
        flag2 = true;
      }
    }
    return flag2;
  }

  public override void PerformLayout()
  {
    if (this.Document == null)
      throw new InvalidOperationException("Must set the Document property to non-null");
    if (this.Network == null)
      this.Network = new MapLayoutNetwork((IMapCollection) this.Document);
    this.ClearCaches();
    this.RaiseProgress(0.0f);
    if (this.Network.NodeCount <= 0)
    {
      this.RaiseProgress(1f);
    }
    else
    {
      this.Network.DeleteSelfLinks();
      foreach (MapLayoutNetworkNode node in this.Network.Nodes)
        node.NodeData = (object) new MapLayoutLayeredDigraphNodeData();
      foreach (MapLayoutNetworkLink link in this.Network.Links)
        link.LinkData = (object) new MapLayoutLayeredDigraphLinkData();
      this.RemoveCycles();
      this.RaiseProgress(0.1f);
      this.AssignLayersInternal();
      this.RaiseProgress(0.25f);
      this.MakeProper();
      this.RaiseProgress(0.3f);
      this.InitializeIndicesInternal();
      this.RaiseProgress(0.35f);
      this.InitializeColumns();
      this.RaiseProgress(0.4f);
      this.ReduceCrossings();
      this.RaiseProgress(0.6f);
      this.StraightenAndPack();
      this.RaiseProgress(0.85f);
      this.LayoutNodesAndLinks();
      this.RaiseProgress(1f);
    }
  }

  protected virtual void PrintNetworkData()
  {
    FileStream fileStream = (FileStream) null;
    StreamWriter streamWriter = (StreamWriter) null;
    try
    {
      fileStream = new FileStream("csout.txt", FileMode.Append);
      streamWriter = new StreamWriter((Stream) fileStream);
      streamWriter.Write("Link Data\r\n\r\n");
      MapLayoutLinkEnumerator enumerator1 = this.Network.Links.GetEnumerator();
      while (enumerator1.MoveNext())
      {
        MapLayoutNetworkLink current = enumerator1.Current;
        if (this.LinkData(current).Valid)
          streamWriter.Write("1,");
        else
          streamWriter.Write("0,");
        if (this.LinkData(current).Rev)
          streamWriter.Write("1,");
        else
          streamWriter.Write("0,");
        if (this.LinkData(current).Forest)
          streamWriter.Write("1,");
        else
          streamWriter.Write("0,");
        streamWriter.Write("{0}, {1}\r\n", (object) this.LinkData(current).PortFromColOffset, (object) this.LinkData(current).PortToColOffset);
      }
      streamWriter.Write("\r\n\r\nNode Data\r\n\r\n");
      MapLayoutNodeEnumerator enumerator2 = this.Network.Nodes.GetEnumerator();
      while (enumerator2.MoveNext())
      {
        MapLayoutNetworkNode current = enumerator2.Current;
        streamWriter.Write("{0}, {1}, {2}", (object) this.NodeData(current).Layer, (object) this.NodeData(current).Column, (object) this.NodeData(current).Index);
        if (this.NodeData(current).Valid)
          streamWriter.Write(",1,");
        else
          streamWriter.Write(",0,");
        streamWriter.Write("{0}, {1}, {2}\r\n", (object) this.NodeData(current).Discover, (object) this.NodeData(current).Finish, (object) this.NodeData(current).Component);
      }
      streamWriter.Write("\r\n\r\n");
    }
    catch (IOException ex)
    {
    }
    finally
    {
      streamWriter?.Close();
      fileStream?.Close();
    }
  }

  protected virtual void ReduceCrossings()
  {
    int num1 = this.CountCrossings();
    int[] layout = this.SaveLayout();
    for (int index = 0; index < this.Iterations; ++index)
    {
      for (int unfixedLayer = 0; unfixedLayer <= this.maxLayer; ++unfixedLayer)
      {
        this.MedianBarycenterCrossingReduction(unfixedLayer, 1);
        this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, 1, false, 1);
      }
      int num2 = this.CountCrossings();
      if (num2 < num1)
      {
        num1 = num2;
        layout = this.SaveLayout();
      }
      for (int maxLayer = this.maxLayer; maxLayer >= 0; --maxLayer)
      {
        this.MedianBarycenterCrossingReduction(maxLayer, -1);
        this.AdjacentExchangeCrossingReductionBendStraighten(maxLayer, -1, false, -1);
      }
      int num3 = this.CountCrossings();
      if (num3 < num1)
      {
        num1 = num3;
        layout = this.SaveLayout();
      }
    }
    this.RestoreLayout(layout);
    for (int index = 0; index < this.Iterations; ++index)
    {
      for (int unfixedLayer = 0; unfixedLayer <= this.maxLayer; ++unfixedLayer)
      {
        this.MedianBarycenterCrossingReduction(unfixedLayer, 0);
        this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, 0, false, 0);
      }
      int num4 = this.CountCrossings();
      if (num4 < num1)
      {
        num1 = num4;
        layout = this.SaveLayout();
      }
      for (int maxLayer = this.maxLayer; maxLayer >= 0; --maxLayer)
      {
        this.MedianBarycenterCrossingReduction(maxLayer, 0);
        this.AdjacentExchangeCrossingReductionBendStraighten(maxLayer, 0, false, 0);
      }
      int num5 = this.CountCrossings();
      if (num5 < num1)
      {
        num1 = num5;
        layout = this.SaveLayout();
      }
    }
    this.RestoreLayout(layout);
    switch (this.AggressiveOption)
    {
      case MapLayoutLayeredDigraphAggressive.More:
        int num6 = num1 + 1;
        while (this.CountCrossings() < num6)
        {
          num6 = this.CountCrossings();
          for (int maxLayer = this.maxLayer; maxLayer >= 0; --maxLayer)
          {
            for (int index = 0; index <= maxLayer; ++index)
            {
              bool flag1 = true;
              while (flag1)
              {
                flag1 = false;
                for (int unfixedLayer = maxLayer; unfixedLayer >= index; --unfixedLayer)
                  flag1 = this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, -1, false, -1) || flag1;
              }
              int num7 = this.CountCrossings();
              if (num7 >= num1)
              {
                this.RestoreLayout(layout);
              }
              else
              {
                num1 = num7;
                layout = this.SaveLayout();
              }
              bool flag2 = true;
              while (flag2)
              {
                flag2 = false;
                for (int unfixedLayer = maxLayer; unfixedLayer >= index; --unfixedLayer)
                  flag2 = this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, 1, false, 1) || flag2;
              }
              int num8 = this.CountCrossings();
              if (num8 >= num1)
              {
                this.RestoreLayout(layout);
              }
              else
              {
                num1 = num8;
                layout = this.SaveLayout();
              }
              bool flag3 = true;
              while (flag3)
              {
                flag3 = false;
                for (int unfixedLayer = index; unfixedLayer <= maxLayer; ++unfixedLayer)
                  flag3 = this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, 1, false, 1) || flag3;
              }
              if (num8 >= num1)
              {
                this.RestoreLayout(layout);
              }
              else
              {
                num1 = num8;
                layout = this.SaveLayout();
              }
              bool flag4 = true;
              while (flag4)
              {
                flag4 = false;
                for (int unfixedLayer = index; unfixedLayer <= maxLayer; ++unfixedLayer)
                  flag4 = this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, -1, false, -1) || flag4;
              }
              if (num8 >= num1)
              {
                this.RestoreLayout(layout);
              }
              else
              {
                num1 = num8;
                layout = this.SaveLayout();
              }
              bool flag5 = true;
              while (flag5)
              {
                flag5 = false;
                for (int unfixedLayer = maxLayer; unfixedLayer >= index; --unfixedLayer)
                  flag5 = this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, 0, false, 0) || flag5;
              }
              if (num8 >= num1)
              {
                this.RestoreLayout(layout);
              }
              else
              {
                num1 = num8;
                layout = this.SaveLayout();
              }
              bool flag6 = true;
              while (flag6)
              {
                flag6 = false;
                for (int unfixedLayer = index; unfixedLayer <= maxLayer; ++unfixedLayer)
                  flag6 = this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, 0, false, 0) || flag6;
              }
              if (num8 >= num1)
              {
                this.RestoreLayout(layout);
              }
              else
              {
                num1 = num8;
                layout = this.SaveLayout();
              }
            }
          }
        }
        break;
      default:
        int maxLayer1 = this.maxLayer;
        int num9 = 0;
        int num10 = num1 + 1;
        while (this.CountCrossings() < num10)
        {
          num10 = this.CountCrossings();
          bool flag7 = true;
          while (flag7)
          {
            flag7 = false;
            for (int unfixedLayer = maxLayer1; unfixedLayer >= num9; --unfixedLayer)
              flag7 = this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, -1, false, -1) || flag7;
          }
          int num11 = this.CountCrossings();
          if (num11 >= num1)
          {
            this.RestoreLayout(layout);
          }
          else
          {
            num1 = num11;
            layout = this.SaveLayout();
          }
          bool flag8 = true;
          while (flag8)
          {
            flag8 = false;
            for (int unfixedLayer = maxLayer1; unfixedLayer >= num9; --unfixedLayer)
              flag8 = this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, 1, false, 1) || flag8;
          }
          int num12 = this.CountCrossings();
          if (num12 >= num1)
          {
            this.RestoreLayout(layout);
          }
          else
          {
            num1 = num12;
            layout = this.SaveLayout();
          }
          bool flag9 = true;
          while (flag9)
          {
            flag9 = false;
            for (int unfixedLayer = num9; unfixedLayer <= maxLayer1; ++unfixedLayer)
              flag9 = this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, 1, false, 1) || flag9;
          }
          if (num12 >= num1)
          {
            this.RestoreLayout(layout);
          }
          else
          {
            num1 = num12;
            layout = this.SaveLayout();
          }
          bool flag10 = true;
          while (flag10)
          {
            flag10 = false;
            for (int unfixedLayer = num9; unfixedLayer <= maxLayer1; ++unfixedLayer)
              flag10 = this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, -1, false, -1) || flag10;
          }
          if (num12 >= num1)
          {
            this.RestoreLayout(layout);
          }
          else
          {
            num1 = num12;
            layout = this.SaveLayout();
          }
          bool flag11 = true;
          while (flag11)
          {
            flag11 = false;
            for (int unfixedLayer = maxLayer1; unfixedLayer >= num9; --unfixedLayer)
              flag11 = this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, 0, false, 0) || flag11;
          }
          if (num12 >= num1)
          {
            this.RestoreLayout(layout);
          }
          else
          {
            num1 = num12;
            layout = this.SaveLayout();
          }
          bool flag12 = true;
          while (flag12)
          {
            flag12 = false;
            for (int unfixedLayer = num9; unfixedLayer <= maxLayer1; ++unfixedLayer)
              flag12 = this.AdjacentExchangeCrossingReductionBendStraighten(unfixedLayer, 0, false, 0) || flag12;
          }
          if (num12 >= num1)
          {
            this.RestoreLayout(layout);
          }
          else
          {
            num1 = num12;
            layout = this.SaveLayout();
          }
        }
        break;
    }
    this.RestoreLayout(layout);
  }

  protected virtual void RemoveCycles()
  {
    foreach (MapLayoutNetworkLink link in this.Network.Links)
      this.LinkData(link).Rev = false;
    switch (this.CycleRemoveOption)
    {
      case MapLayoutLayeredDigraphCycleRemove.DepthFirst:
        this.DepthFirstSearchCycleRemoval();
        break;
      default:
        this.GreedyCycleRemoval();
        break;
    }
  }

  protected virtual void RestoreLayout(int[] layout)
  {
    int index1 = 0;
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      this.NodeData(node).Layer = layout[index1];
      int index2 = index1 + 1;
      this.NodeData(node).Column = layout[index2];
      int index3 = index2 + 1;
      this.NodeData(node).Index = layout[index3];
      index1 = index3 + 1;
    }
  }

  protected virtual int[] SaveLayout()
  {
    if (this.mySavedLayout == null)
      this.mySavedLayout = new int[3 * this.Network.NodeCount];
    int index1 = 0;
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      this.mySavedLayout[index1] = this.NodeData(node).Layer;
      int index2 = index1 + 1;
      this.mySavedLayout[index2] = this.NodeData(node).Column;
      int index3 = index2 + 1;
      this.mySavedLayout[index3] = this.NodeData(node).Index;
      index1 = index3 + 1;
    }
    return this.mySavedLayout;
  }

  protected virtual void SetComponents(
    MapLayoutNetworkNode node,
    int component,
    bool forward,
    bool backward)
  {
    if (this.NodeData(node).Component == component)
      return;
    this.NodeData(node).Component = component;
    if (forward)
    {
      foreach (MapLayoutNetworkLink destinationLinks in node.DestinationLinksList)
        this.SetComponents(destinationLinks.ToNode, component, forward, backward);
    }
    if (!backward)
      return;
    foreach (MapLayoutNetworkLink sourceLinks in node.SourceLinksList)
      this.SetComponents(sourceLinks.FromNode, component, forward, backward);
  }

  protected virtual bool ShiftBendStraighten(int unfixedLayer, int direction)
  {
    MapLayoutNetworkNode[] cachedNodeArray = this.GetCachedNodeArray(unfixedLayer);
    float[] numArray1 = this.Barycenters(unfixedLayer, -1);
    if (direction > 0)
    {
      for (int index = 0; index < this.indices[unfixedLayer]; ++index)
        numArray1[index] = -1f;
    }
    float[] numArray2 = this.Barycenters(unfixedLayer, 1);
    if (direction < 0)
    {
      for (int index = 0; index < this.indices[unfixedLayer]; ++index)
        numArray2[index] = -1f;
    }
    bool flag1 = false;
    bool flag2 = true;
    while (flag2)
    {
      flag2 = false;
      for (int index = 0; index < this.indices[unfixedLayer]; ++index)
      {
        int column1 = this.NodeData(cachedNodeArray[index]).Column;
        int num1 = this.NodeMinColumnSpace(cachedNodeArray[index]);
        int num2 = index - 1 < 0 || column1 - this.NodeData(cachedNodeArray[index - 1]).Column - 1 > num1 + this.NodeMinColumnSpace(cachedNodeArray[index - 1]) ? column1 - 1 : column1;
        int num3 = index + 1 >= this.indices[unfixedLayer] || this.NodeData(cachedNodeArray[index + 1]).Column - column1 - 1 > num1 + this.NodeMinColumnSpace(cachedNodeArray[index + 1]) ? column1 + 1 : column1;
        float num4 = 0.0f;
        float num5 = 0.0f;
        float num6 = 0.0f;
        if (direction < 0 || direction == 0)
        {
          foreach (MapLayoutNetworkLink sourceLinks in cachedNodeArray[index].SourceLinksList)
          {
            if (this.LinkData(sourceLinks).Valid && this.NodeData(sourceLinks.FromNode).Layer != unfixedLayer)
            {
              float num7 = this.LinkStraightenWeight(sourceLinks);
              int portFromColOffset = this.LinkData(sourceLinks).PortFromColOffset;
              int portToColOffset = this.LinkData(sourceLinks).PortToColOffset;
              int column2 = this.NodeData(sourceLinks.FromNode).Column;
              num4 += (float) (Math.Abs(column1 + portToColOffset - (column2 + portFromColOffset)) + 1) * num7;
              num5 += (float) (Math.Abs(num2 + portToColOffset - (column2 + portFromColOffset)) + 1) * num7;
              num6 += (float) (Math.Abs(num3 + portToColOffset - (column2 + portFromColOffset)) + 1) * num7;
            }
          }
        }
        if (direction > 0 || direction == 0)
        {
          foreach (MapLayoutNetworkLink destinationLinks in cachedNodeArray[index].DestinationLinksList)
          {
            if (this.LinkData(destinationLinks).Valid && this.NodeData(destinationLinks.ToNode).Layer != unfixedLayer)
            {
              float num8 = this.LinkStraightenWeight(destinationLinks);
              int portFromColOffset = this.LinkData(destinationLinks).PortFromColOffset;
              int portToColOffset = this.LinkData(destinationLinks).PortToColOffset;
              int column3 = this.NodeData(destinationLinks.ToNode).Column;
              num4 += (float) (Math.Abs(column1 + portFromColOffset - (column3 + portToColOffset)) + 1) * num8;
              num5 += (float) (Math.Abs(num2 + portFromColOffset - (column3 + portToColOffset)) + 1) * num8;
              num6 += (float) (Math.Abs(num3 + portFromColOffset - (column3 + portToColOffset)) + 1) * num8;
            }
          }
        }
        float num9 = 0.0f;
        float num10 = 0.0f;
        float num11 = 0.0f;
        float num12 = numArray1[this.NodeData(cachedNodeArray[index]).Index];
        float num13 = numArray2[this.NodeData(cachedNodeArray[index]).Index];
        if ((double) num12 != -1.0)
        {
          num9 += Math.Abs(num12 - (float) column1);
          num10 += Math.Abs(num12 - (float) num2);
          num11 += Math.Abs(num12 - (float) num3);
        }
        if ((double) num13 != -1.0)
        {
          num9 += Math.Abs(num13 - (float) column1);
          num10 += Math.Abs(num13 - (float) num2);
          num11 += Math.Abs(num13 - (float) num3);
        }
        if ((double) num5 < (double) num4 || (double) num5 == (double) num4 && (double) num10 < (double) num9)
        {
          flag1 = true;
          flag2 = true;
          this.NodeData(cachedNodeArray[index]).Column = num2;
        }
        if ((double) num6 < (double) num4 || (double) num6 == (double) num4 && (double) num11 < (double) num9)
        {
          flag1 = true;
          flag2 = true;
          this.NodeData(cachedNodeArray[index]).Column = num3;
        }
      }
    }
    this.FreeCachedNodeArray(unfixedLayer, cachedNodeArray);
    this.Normalize();
    return flag1;
  }

  protected virtual void StraightenAndPack()
  {
    int[] numArray = new int[this.maxLayer + 1];
    for (int index = 0; index <= this.maxLayer; ++index)
      numArray[index] = 0;
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      int layer = this.NodeData(node).Layer;
      int column = this.NodeData(node).Column;
      int num = this.NodeMinColumnSpace(node);
      numArray[layer] = Math.Max(numArray[layer], column + num);
    }
    foreach (MapLayoutNetworkNode node in this.Network.Nodes)
    {
      int layer = this.NodeData(node).Layer;
      int column = this.NodeData(node).Column;
      this.NodeData(node).Column = (this.maxColumn - numArray[layer]) * 8 / 2 + column * 8;
    }
    this.maxColumn *= 8;
    bool flag1;
    for (bool flag2 = true; flag2; flag2 = this.BendStraighten(this.maxIndexLayer, 0) || flag1)
    {
      flag1 = false;
      for (int unfixedLayer = this.maxIndexLayer + 1; unfixedLayer <= this.maxLayer; ++unfixedLayer)
        flag1 = this.BendStraighten(unfixedLayer, 1) || flag1;
      for (int unfixedLayer = this.maxIndexLayer - 1; unfixedLayer >= 0; --unfixedLayer)
        flag1 = this.BendStraighten(unfixedLayer, -1) || flag1;
    }
    for (int unfixedLayer = this.maxIndexLayer + 1; unfixedLayer <= this.maxLayer; ++unfixedLayer)
      this.MedianStraighten(unfixedLayer, 1);
    for (int unfixedLayer = this.maxIndexLayer - 1; unfixedLayer >= 0; --unfixedLayer)
      this.MedianStraighten(unfixedLayer, -1);
    this.MedianStraighten(this.maxIndexLayer, 0);
    this.ComponentPack(-1);
    this.ComponentPack(1);
    bool flag3 = true;
    while (flag3)
    {
      bool flag4 = false;
      flag3 = this.BendStraighten(this.maxIndexLayer, 0) || flag4;
      for (int unfixedLayer = this.maxIndexLayer + 1; unfixedLayer <= this.maxLayer; ++unfixedLayer)
        flag3 = this.BendStraighten(unfixedLayer, 0) || flag3;
      for (int unfixedLayer = this.maxIndexLayer - 1; unfixedLayer >= 0; --unfixedLayer)
        flag3 = this.BendStraighten(unfixedLayer, 0) || flag3;
    }
  }

  protected virtual void TightComponent(
    MapLayoutNetworkNode node,
    int component,
    bool forward,
    bool backward)
  {
    if (this.NodeData(node).Component == component)
      return;
    this.NodeData(node).Component = component;
    if (forward)
    {
      foreach (MapLayoutNetworkLink destinationLinks in node.DestinationLinksList)
      {
        MapLayoutNetworkNode toNode = destinationLinks.ToNode;
        if (this.NodeData(node).Layer - this.NodeData(toNode).Layer == this.LinkMinLength(destinationLinks))
          this.TightComponent(toNode, component, forward, backward);
      }
    }
    if (!backward)
      return;
    foreach (MapLayoutNetworkLink sourceLinks in node.SourceLinksList)
    {
      MapLayoutNetworkNode fromNode = sourceLinks.FromNode;
      if (this.NodeData(fromNode).Layer - this.NodeData(node).Layer == this.LinkMinLength(sourceLinks))
        this.TightComponent(fromNode, component, forward, backward);
    }
  }

  protected virtual void TightComponentUnset(
    MapLayoutNetworkNode node,
    int component,
    int unset,
    bool forward,
    bool backward)
  {
    if (this.NodeData(node).Component != unset)
      return;
    this.NodeData(node).Component = component;
    if (forward)
    {
      foreach (MapLayoutNetworkLink destinationLinks in node.DestinationLinksList)
      {
        MapLayoutNetworkNode toNode = destinationLinks.ToNode;
        if (this.NodeData(node).Layer - this.NodeData(toNode).Layer == this.LinkMinLength(destinationLinks))
          this.TightComponentUnset(toNode, component, unset, forward, backward);
      }
    }
    if (!backward)
      return;
    foreach (MapLayoutNetworkLink sourceLinks in node.SourceLinksList)
    {
      MapLayoutNetworkNode fromNode = sourceLinks.FromNode;
      if (this.NodeData(fromNode).Layer - this.NodeData(node).Layer == this.LinkMinLength(sourceLinks))
        this.TightComponentUnset(fromNode, component, unset, forward, backward);
    }
  }

  protected virtual void TightPack()
  {
    this.Pack();
    for (int column = 0; column < this.maxColumn; ++column)
    {
      do
        ;
      while (this.TightPackAux(column, 1));
    }
    this.Normalize();
  }

  protected virtual bool TightPackAux(int column, int direction)
  {
    int num1 = column;
    if (direction > 0)
      num1 = column + 1;
    if (direction < 0)
      num1 = column - 1;
    bool[] flagArray1 = new bool[this.maxLayer + 1];
    bool[] flagArray2 = new bool[this.maxLayer + 1];
    for (int index = 0; index <= this.maxLayer; ++index)
    {
      flagArray1[index] = false;
      flagArray2[index] = false;
    }
    MapLayoutNodeEnumerator nodes = this.Network.Nodes;
    foreach (MapLayoutNetworkNode node in nodes)
    {
      int num2 = this.NodeMinColumnSpace(node);
      if (this.NodeData(node).Column - num2 <= column && this.NodeData(node).Column + num2 >= column)
        flagArray1[this.NodeData(node).Layer] = true;
      if (this.NodeData(node).Column - num2 <= num1 && this.NodeData(node).Column + num2 >= num1)
        flagArray2[this.NodeData(node).Layer] = true;
    }
    bool flag1 = true;
    bool flag2 = false;
    for (int index = 0; index <= this.maxLayer; ++index)
      flag1 = !flagArray1[index] ? flag1 : !flagArray2[index];
    if (!flag1)
      return flag2;
    if (direction > 0)
    {
      nodes = this.Network.Nodes;
      foreach (MapLayoutNetworkNode node in nodes)
      {
        if (this.NodeData(node).Column > column)
        {
          MapLayoutLayeredDigraphNodeData layeredDigraphNodeData = this.NodeData(node);
          this.NodeData(node).Column = layeredDigraphNodeData.Column - 1;
          flag2 = true;
        }
      }
    }
    if (direction >= 0)
      return flag2;
    nodes = this.Network.Nodes;
    foreach (MapLayoutNetworkNode node in nodes)
    {
      if (this.NodeData(node).Column < column)
      {
        MapLayoutLayeredDigraphNodeData layeredDigraphNodeData = this.NodeData(node);
        this.NodeData(node).Column = layeredDigraphNodeData.Column + 1;
        flag2 = true;
      }
    }
    return flag2;
  }

  [DefaultValue(0)]
  [Description("how aggressive to be about looking for link crossings")]
  public virtual MapLayoutLayeredDigraphAggressive AggressiveOption
  {
    get => this.aggressiveOption;
    set => this.aggressiveOption = value;
  }

  [Description("the size of each column")]
  [DefaultValue(25)]
  public virtual int ColumnSpacing
  {
    get => this.columnSpacing;
    set
    {
      if (value <= 0)
        return;
      this.columnSpacing = value;
    }
  }

  [DefaultValue(0)]
  [Description("which cycle removal option is being used")]
  public virtual MapLayoutLayeredDigraphCycleRemove CycleRemoveOption
  {
    get => this.cycleremoveOption;
    set => this.cycleremoveOption = value;
  }

  [Description("in which direction is the graph laid out")]
  [DefaultValue(0)]
  public virtual MapLayoutDirection DirectionOption
  {
    get => this.directionOption;
    set => this.directionOption = value;
  }

  [Description("which indices initialization option is being used")]
  [DefaultValue(0)]
  public virtual MapLayoutLayeredDigraphInitIndices InitializeOption
  {
    get => this.initializeOption;
    set => this.initializeOption = value;
  }

  [Description("the number of iterations are to be done")]
  [DefaultValue(4)]
  public virtual int Iterations
  {
    get => this.iterations;
    set => this.iterations = Math.Max(value, 0);
  }

  [DefaultValue(0)]
  [Description("which layering option is being used")]
  public virtual MapLayoutLayeredDigraphLayering LayeringOption
  {
    get => this.layeringOption;
    set => this.layeringOption = value;
  }

  [Description("the size of each layer")]
  [DefaultValue(25)]
  public virtual int LayerSpacing
  {
    get => this.layerSpacing;
    set
    {
      if (value <= 0)
        return;
      this.layerSpacing = value;
    }
  }

  [Browsable(false)]
  public virtual int MaxColumn => this.maxColumn;

  [Browsable(false)]
  public virtual int MaxIndex => this.maxIndex;

  [Browsable(false)]
  public virtual int MaxIndexLayer => this.maxIndexLayer;

  [Browsable(false)]
  public virtual int MaxLayer => this.maxLayer;

  [Browsable(false)]
  public virtual int MinIndexLayer => this.minIndexLayer;
}
