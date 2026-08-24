// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.QualityLayout
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class QualityLayout : StandardLayout
{
  internal int MaxMoveY { get; set; }

  internal int MinMoveY { get; set; }

  internal int LogHeight { get; set; }

  public override void InitLayout(Size logSize, VisScheme scheme)
  {
    base.InitLayout(logSize, scheme);
    this.LogHeight = logSize.Height;
  }

  public override void SetInitialCoords(BasePredicate cancel, VisScheme scheme, Point centerPoint)
  {
    base.SetInitialCoords(cancel, scheme, centerPoint);
    this.MaxMoveY = scheme.RootObj.Org.Y + this.LogHeight / 2;
    this.MinMoveY = scheme.RootObj.Org.Y - this.LogHeight / 2;
  }

  public override void BeforeLayoutLevel(VisScheme scheme, int levelNum, bool parent)
  {
    VisLevel source = parent ? scheme.ParentLevels[levelNum] : scheme.ChildLevels[levelNum];
    if (source.Count<VisObject>() <= this.maxObjectsPerLevel)
      return;
    source.ForEach((Action<VisObject>) (vo => this.CalculateWeight(vo, levelNum + 1, parent)));
    List<VisObject> list = source.SelectVisible().ToList<VisObject>();
    list.Sort((Comparison<VisObject>) ((x, y) => y.LayoutWeight - x.LayoutWeight));
    List<VisLevel> visLevelList = parent ? scheme.ParentLevels : scheme.ChildLevels;
    VisLevel visLevel;
    if (visLevelList.Count <= levelNum + 1)
      visLevelList.Add(visLevel = new VisLevel(source.LevelNum + 1, scheme));
    else
      visLevel = visLevelList[levelNum + 1];
    List<VisObject> movingObjects = new List<VisObject>();
    for (int index = list.Count - 1; index >= 0; --index)
    {
      VisObject vo = list[index];
      source.Remove(vo);
      visLevel.Add(vo);
      vo.ParentLevel = visLevel;
      this.CollectMovingObjects(vo, parent, movingObjects);
      if (source.Count <= this.maxObjectsPerLevel)
        break;
    }
    foreach (VisObject vo in movingObjects)
      this._DoMoveObject(vo, parent, scheme);
  }

  public override void CalculateWeight(VisObject vo, int levelNum, bool parent)
  {
    vo.LayoutWeight = parent ? vo.ParentRels.Count * 10 + vo.ChildRels.Count + (levelNum - Math.Abs(vo.Level)) * 100 : vo.ChildRels.Count * 10 + vo.ParentRels.Count + (levelNum - vo.Level) * 100;
  }

  public override void DoLayout(BasePredicate cancel, VisScheme scheme)
  {
    if (scheme.ChildLevels != null)
    {
      foreach (VisLevel childLevel in scheme.ChildLevels)
      {
        if (childLevel.LevelNum != 1)
          this.DoLayoutLevel(childLevel);
      }
    }
    if (scheme.ParentLevels == null)
      return;
    foreach (VisLevel parentLevel in scheme.ParentLevels)
    {
      if (parentLevel.LevelNum != -1)
        this.DoLayoutLevel(parentLevel);
    }
  }

  public virtual void DoLayoutLevel(VisLevel level)
  {
    bool flag = level.LevelNum > 0;
    List<QualityLayout.LayoutInfo> layList = new List<QualityLayout.LayoutInfo>();
    foreach (VisObject vo in (List<VisObject>) level)
    {
      int num1 = 0;
      int num2 = 0;
      if (flag)
      {
        foreach (VisRelation parentRel in vo.ParentRels)
        {
          num1 += parentRel.Parent.Org.Y;
          ++num2;
        }
      }
      else
      {
        foreach (VisRelation childRel in vo.ChildRels)
        {
          num1 += childRel.Child.Org.Y;
          ++num2;
        }
      }
      layList.Add(new QualityLayout.LayoutInfo(vo, num1 / num2));
    }
    layList.Sort((Comparison<QualityLayout.LayoutInfo>) ((x, y) => x.FutureY - y.FutureY));
    int num3 = layList.Count;
    for (int index = 0; index < layList.Count; ++index)
    {
      if (layList[index].FutureY > 0)
      {
        num3 = index;
        break;
      }
    }
    QualityLayout.FixFirstElems(layList, num3);
    if (num3 < layList.Count)
      QualityLayout.FixConflicts(layList, num3, true);
    if (num3 > 0)
      QualityLayout.FixConflicts(layList, num3 - 1, false);
    VisScheme parentScheme = level.ParentScheme;
    List<QualityLayout.RelationStru> relStruList = this.GetRelStruList(parentScheme);
    List<VisObject> allObjects = this.GetAllObjects(parentScheme);
    List<QualityLayout.SumCostInfo> sumList = new List<QualityLayout.SumCostInfo>();
    Dictionary<long, QualityLayout.SumCostInfo> idList = new Dictionary<long, QualityLayout.SumCostInfo>();
    for (int index = 0; index < layList.Count; ++index)
    {
      QualityLayout.SumCostInfo sumCostInfo = new QualityLayout.SumCostInfo(index, layList[index], relStruList, allObjects);
      sumList.Add(sumCostInfo);
      idList.Add(layList[index].VisObj.ObjVerId, sumCostInfo);
    }
    sumList.Sort((Comparison<QualityLayout.SumCostInfo>) ((x, y) => (int) (y.SumCost - x.SumCost)));
    foreach (QualityLayout.SumCostInfo sci in sumList)
    {
      VisObject visObj1 = sci.VisObj;
      Point org = sci.VisObj.Org;
      Point point1 = new Point(org.X, sci.FutureY);
      visObj1.Org = point1;
      if (sci.SumCost >= (long) VisLayout.ALLOWED_LOSS)
      {
        QualityLayout.OptResult optResult = new QualityLayout.OptResult(QualityLayout.OptOperation.None, 0, sci.SumCost);
        QualityLayout.OptResult other1 = this.TryMoveClose(sci, layList, true);
        if (other1.Oper != QualityLayout.OptOperation.None)
          optResult.Assign(other1);
        if (optResult.Cost > (long) VisLayout.ALLOWED_LOSS)
        {
          QualityLayout.OptResult other2 = this.TryMoveClose(sci, layList, false);
          if (other2.Oper != QualityLayout.OptOperation.None && other2.Cost < optResult.Cost)
            optResult.Assign(other2);
        }
        if (optResult.Cost > (long) VisLayout.ALLOWED_LOSS)
        {
          QualityLayout.OptResult other3 = this.TrySwap(sci, layList, idList, true);
          if (other3.Oper != QualityLayout.OptOperation.None && other3.Cost < optResult.Cost)
            optResult.Assign(other3);
        }
        if (optResult.Cost > (long) VisLayout.ALLOWED_LOSS)
        {
          QualityLayout.OptResult other4 = this.TrySwap(sci, layList, idList, false);
          if (other4.Oper != QualityLayout.OptOperation.None && other4.Cost < optResult.Cost)
            optResult.Assign(other4);
        }
        if (optResult.Cost > (long) VisLayout.ALLOWED_LOSS)
        {
          QualityLayout.OptResult other5 = this.TryMoveFar(sci, layList, true);
          if (other5.Oper != QualityLayout.OptOperation.None && other5.Cost < optResult.Cost)
            optResult.Assign(other5);
        }
        if (optResult.Cost > (long) VisLayout.ALLOWED_LOSS)
        {
          QualityLayout.OptResult other6 = this.TryMoveFar(sci, layList, false);
          if (other6.Oper != QualityLayout.OptOperation.None && other6.Cost < optResult.Cost)
            optResult.Assign(other6);
        }
        if (optResult.Oper != QualityLayout.OptOperation.None)
        {
          switch (optResult.Oper)
          {
            case QualityLayout.OptOperation.Move:
              sci.FutureY = optResult.Param;
              sci.SumCost = optResult.Cost;
              layList[sci.Index].FutureY = optResult.Param;
              VisObject visObj2 = sci.VisObj;
              org = sci.VisObj.Org;
              Point point2 = new Point(org.X, optResult.Param);
              visObj2.Org = point2;
              continue;
            case QualityLayout.OptOperation.Swap:
              sci.SumCost = optResult.Cost;
              int index1 = sci.Index;
              int index2 = optResult.Param;
              int futureY1 = sci.FutureY;
              int futureY2 = layList[index2].FutureY;
              layList[index1].FutureY = futureY2;
              VisObject visObj3 = layList[index1].VisObj;
              org = sci.VisObj.Org;
              Point point3 = new Point(org.X, futureY2);
              visObj3.Org = point3;
              layList[index2].FutureY = futureY1;
              VisObject visObj4 = layList[index2].VisObj;
              org = sci.VisObj.Org;
              Point point4 = new Point(org.X, futureY1);
              visObj4.Org = point4;
              continue;
            case QualityLayout.OptOperation.LongMove:
              sci.FutureY = optResult.Param;
              sci.SumCost = optResult.Cost;
              QualityLayout.LayoutInfo layoutInfo = layList[sci.Index];
              layoutInfo.FutureY = optResult.Param;
              VisObject visObj5 = sci.VisObj;
              org = sci.VisObj.Org;
              Point point5 = new Point(org.X, optResult.Param);
              visObj5.Org = point5;
              layList.RemoveAt(sci.Index);
              int newIndex = FindNewIndex(optResult.Param);
              layList.Insert(newIndex, layoutInfo);
              if (newIndex < sci.Index)
                MoveIndices(newIndex, sci.Index, 1);
              else if (newIndex > sci.Index)
                MoveIndices(sci.Index, newIndex, -1);
              sci.Index = newIndex;
              continue;
            default:
              continue;
          }
        }
      }
      else
        break;
    }
    layList.Sort((Comparison<QualityLayout.LayoutInfo>) ((x, y) =>
    {
      Point org = x.VisObj.Org;
      int y1 = org.Y;
      org = y.VisObj.Org;
      int y2 = org.Y;
      return y1 - y2;
    }));
    for (int index = 0; index < layList.Count; ++index)
    {
      VisObject visObj = layList[index].VisObj;
      List<VisRelation> visRelationList = flag ? visObj.ParentRels : visObj.ChildRels;
      if (visRelationList.Count <= 1)
      {
        VisObject visObject1 = flag ? visRelationList[0].Parent : visRelationList[0].Child;
        Point org = visObject1.Org;
        int y3 = org.Y;
        org = visObj.Org;
        if (org.Y == y3 && Math.Abs(visObj.Level - visObject1.Level) <= 1)
        {
          int num4;
          if (index >= layList.Count - 1)
          {
            num4 = this.MaxMoveY;
          }
          else
          {
            org = layList[index + 1].VisObj.Org;
            num4 = org.Y;
          }
          int num5 = num4;
          org = visObj.Org;
          int y4 = org.Y;
          if (num5 - y4 > VisLayout._DistY * 3 / 2)
          {
            VisObject visObject2 = visObj;
            org = visObj.Org;
            int x = org.X;
            org = visObj.Org;
            int y5 = org.Y + VisLayout._DistY / 2;
            Point point = new Point(x, y5);
            visObject2.Org = point;
          }
          else
          {
            int num6;
            if (index <= 0)
            {
              num6 = this.MinMoveY;
            }
            else
            {
              org = layList[index - 1].VisObj.Org;
              num6 = org.Y;
            }
            int num7 = num6;
            org = visObj.Org;
            if (org.Y - num7 > VisLayout._DistY * 3 / 2)
            {
              VisObject visObject3 = visObj;
              org = visObj.Org;
              int x = org.X;
              org = visObj.Org;
              int y6 = org.Y - VisLayout._DistY / 2;
              Point point = new Point(x, y6);
              visObject3.Org = point;
            }
          }
        }
      }
    }

    int FindNewIndex(int y)
    {
      int newIndex = 0;
      int num = layList.Count;
      while (newIndex < num)
      {
        int index = (newIndex + num) / 2;
        if (layList[index].FutureY < y)
          newIndex = index + 1;
        else
          num = index;
      }
      return newIndex;
    }

    void MoveIndices(int from, int to, int shift)
    {
      foreach (QualityLayout.SumCostInfo sum in sumList)
      {
        if (sum.Index >= from && sum.Index < to)
          sum.Index += shift;
      }
    }
  }

  internal QualityLayout.OptResult TryMoveClose(
    QualityLayout.SumCostInfo sci,
    List<QualityLayout.LayoutInfo> layList,
    bool up)
  {
    int index = sci.Index;
    QualityLayout.OptResult optResult = new QualityLayout.OptResult(QualityLayout.OptOperation.None, 0, sci.SumCost);
    int num1 = up ? VisLayout._DistY : -VisLayout._DistY;
    int border = up ? this.MaxMoveY : this.MinMoveY;
    if (up)
    {
      if (index + 1 < layList.Count)
        border = layList[index + 1].FutureY;
      border -= VisLayout._DistY;
    }
    else
    {
      if (index - 1 >= 0)
        border = layList[index - 1].FutureY;
      border += VisLayout._DistY;
    }
    int futureY = sci.FutureY;
    try
    {
      for (sci.FutureY += num1; NoBorder(sci.FutureY); sci.FutureY += num1)
      {
        long num2 = sci.CalcSumCost();
        if (num2 < optResult.Cost)
        {
          optResult.Oper = QualityLayout.OptOperation.Move;
          optResult.Param = sci.FutureY;
          optResult.Cost = num2;
        }
      }
    }
    finally
    {
      sci.FutureY = futureY;
    }
    return optResult;

    bool NoBorder(int y)
    {
      if (up && y < border)
        return true;
      return !up && y > border;
    }
  }

  internal QualityLayout.OptResult TrySwap(
    QualityLayout.SumCostInfo sci,
    List<QualityLayout.LayoutInfo> layList,
    Dictionary<long, QualityLayout.SumCostInfo> idList,
    bool up)
  {
    int index1 = sci.Index;
    QualityLayout.OptResult optResult = new QualityLayout.OptResult(QualityLayout.OptOperation.None, 0, sci.SumCost);
    int num1 = up ? 1 : -1;
    int index2 = index1 + num1;
    if (index2 >= 0 && index2 < layList.Count)
    {
      long objVerId = layList[index2].VisObj.ObjVerId;
      if (idList.ContainsKey(objVerId))
      {
        QualityLayout.SumCostInfo id = idList[objVerId];
        long sumCost1 = sci.SumCost;
        long sumCost2 = id.SumCost;
        int futureY1 = id.FutureY;
        id.FutureY = sci.FutureY;
        sci.FutureY = futureY1;
        try
        {
          long num2 = sci.CalcSumCost();
          long num3 = id.CalcSumCost();
          if (num2 + num3 < sumCost1 + sumCost2)
          {
            optResult.Oper = QualityLayout.OptOperation.Swap;
            optResult.Param = index2;
            optResult.Cost = num3 + num2 - sumCost2;
          }
        }
        finally
        {
          int futureY2 = id.FutureY;
          id.FutureY = sci.FutureY;
          sci.FutureY = futureY2;
        }
      }
    }
    return optResult;
  }

  internal QualityLayout.OptResult TryMoveFar(
    QualityLayout.SumCostInfo sci,
    List<QualityLayout.LayoutInfo> layList,
    bool up)
  {
    int index = sci.Index;
    QualityLayout.OptResult optResult = new QualityLayout.OptResult(QualityLayout.OptOperation.None, 0, sci.SumCost);
    int num1 = up ? VisLayout._DistY : -VisLayout._DistY;
    int num2 = up ? this.MaxMoveY - VisLayout._DistY : this.MinMoveY + VisLayout._DistY;
    int num3 = up ? 1 : -1;
    int futureY = sci.FutureY;
    int num4 = index + num3;
    if (num4 >= 0 && num4 < layList.Count)
      sci.FutureY = layList[num4].FutureY;
    try
    {
      while (true)
      {
        num4 += num3;
        int cBorder = IsValidIndex(num4) ? layList[num4].FutureY - num3 * VisLayout._DistY : num2;
        for (sci.FutureY += num1; NoBorder(sci.FutureY, cBorder); sci.FutureY += num1)
        {
          long num5 = sci.CalcSumCost();
          if (num5 < optResult.Cost)
          {
            optResult.Oper = QualityLayout.OptOperation.LongMove;
            optResult.Param = sci.FutureY;
            optResult.Cost = num5;
          }
        }
        if (IsValidIndex(num4))
          sci.FutureY = layList[num4].FutureY;
        else
          break;
      }
    }
    finally
    {
      sci.FutureY = futureY;
    }
    return optResult;

    bool IsValidIndex(int i) => i >= 0 && i < layList.Count;

    bool NoBorder(int y, int cBorder)
    {
      if (up && y < cBorder)
        return true;
      return !up && y > cBorder;
    }
  }

  public static void TestFixingConflicts(List<int> yCoords)
  {
    List<QualityLayout.LayoutInfo> layList = new List<QualityLayout.LayoutInfo>();
    int num = yCoords.Count;
    for (int index = 0; index < yCoords.Count; ++index)
    {
      if (yCoords[index] >= 0 && num == yCoords.Count)
        num = index;
      layList.Add(new QualityLayout.LayoutInfo((VisObject) null, yCoords[index]));
    }
    QualityLayout.FixFirstElems(layList, num);
    if (num < layList.Count)
      QualityLayout.FixConflicts(layList, num, true);
    if (num <= 0)
      return;
    QualityLayout.FixConflicts(layList, num - 1, false);
  }

  internal static void FixFirstElems(List<QualityLayout.LayoutInfo> layList, int firstPositiveIndex)
  {
    if (firstPositiveIndex <= 0 || firstPositiveIndex >= layList.Count)
      return;
    int futureY1 = layList[firstPositiveIndex].FutureY;
    int futureY2 = layList[firstPositiveIndex - 1].FutureY;
    if (futureY1 - futureY2 >= VisLayout._DistY)
      return;
    int num1 = (VisLayout._DistY - futureY1 + futureY2) / 2 + 1;
    int num2 = layList[firstPositiveIndex].FutureY + num1;
    int num3 = layList[firstPositiveIndex - 1].FutureY - num1;
    layList[firstPositiveIndex].FutureY = num2;
    layList[firstPositiveIndex - 1].FutureY = num3;
    for (int index = firstPositiveIndex + 1; index < layList.Count && layList[index].FutureY < num2; ++index)
    {
      num2 += num1;
      layList[index].FutureY = num2;
    }
    for (int index = firstPositiveIndex - 2; index >= 0 && layList[index].FutureY > num3; --index)
    {
      num3 -= num1;
      layList[index].FutureY = num3;
    }
  }

  internal static void FixConflicts(
    List<QualityLayout.LayoutInfo> layList,
    int firstIndex,
    bool positive)
  {
    int delta = positive ? 1 : -1;
    int freeSpace = 0;
    for (int index = firstIndex; NormalIndex(index); index += delta)
    {
      if (index == firstIndex)
        freeSpace = Math.Max(Math.Abs(layList[index].FutureY) - VisLayout._DistY, 0);
      else
        freeSpace += Math.Abs(layList[index].FutureY - layList[index - delta].FutureY) - VisLayout._DistY;
      if (NormalIndex(index + delta))
      {
        int num1 = ConflictDist(layList[index].FutureY, layList[index + delta].FutureY);
        if (num1 < VisLayout._DistY)
        {
          int num2 = VisLayout._DistY - num1;
          if (freeSpace > 0)
          {
            int moveDist = Math.Min(freeSpace, num2 / 2);
            PushObjectsDown(index, moveDist, -delta);
            int currY = layList[index].FutureY + VisLayout._DistY;
            PushObjectsUp(index + delta, currY);
          }
          else
            PushObjectsUp(index + delta, layList[index].FutureY + VisLayout._DistY);
        }
      }
    }

    static int ConflictDist(int y1, int y2) => Math.Abs(y1 - y2);

    bool NormalIndex(int index) => index >= 0 && index < layList.Count;

    void PushObjectsDown(int forIndex, int moveDist, int newDelta)
    {
      if (moveDist > freeSpace)
        moveDist = freeSpace;
      for (int index = forIndex; NormalIndex(index) && moveDist > 0; index += newDelta)
      {
        int futureY1 = layList[index].FutureY;
        if (NormalIndex(index + newDelta))
        {
          int futureY2 = layList[index + newDelta].FutureY;
          int num = Math.Abs(futureY1 - futureY2) - VisLayout._DistY;
          if (num > 0)
          {
            layList[index].FutureY += newDelta * num;
            freeSpace -= num;
            moveDist -= num;
          }
        }
        else
        {
          layList[index].FutureY += newDelta * moveDist;
          freeSpace -= moveDist;
          moveDist = 0;
        }
      }
    }

    void PushObjectsUp(int forIndex, int currY)
    {
      int index = forIndex;
      while (NormalIndex(index))
      {
        layList[index].FutureY = currY;
        currY += VisLayout._DistY;
        index += delta;
        if (NormalIndex(index) && layList[index].FutureY >= currY)
          break;
      }
    }
  }

  internal List<QualityLayout.RelationStru> GetRelStruList(VisScheme scheme)
  {
    List<QualityLayout.RelationStru> relStruList = new List<QualityLayout.RelationStru>();
    if (scheme.ParentLevels != null)
    {
      foreach (VisRelation parentRel in scheme.RootObj.ParentRels)
        relStruList.Add(new QualityLayout.RelationStru(parentRel));
      foreach (List<VisObject> parentLevel in scheme.ParentLevels)
      {
        foreach (VisObject visObject in parentLevel)
        {
          foreach (VisRelation parentRel in visObject.ParentRels)
            relStruList.Add(new QualityLayout.RelationStru(parentRel));
        }
      }
    }
    if (scheme.ChildLevels != null)
    {
      foreach (VisRelation childRel in scheme.RootObj.ChildRels)
        relStruList.Add(new QualityLayout.RelationStru(childRel));
      foreach (List<VisObject> childLevel in scheme.ChildLevels)
      {
        foreach (VisObject visObject in childLevel)
        {
          foreach (VisRelation childRel in visObject.ChildRels)
            relStruList.Add(new QualityLayout.RelationStru(childRel));
        }
      }
    }
    return relStruList;
  }

  internal List<VisObject> GetAllObjects(VisScheme scheme)
  {
    List<VisObject> allObjects = new List<VisObject>();
    allObjects.Add(scheme.RootObj);
    if (scheme.ChildLevels != null)
    {
      foreach (List<VisObject> childLevel in scheme.ChildLevels)
      {
        foreach (VisObject visObject in childLevel)
          allObjects.Add(visObject);
      }
    }
    if (scheme.ParentLevels != null)
    {
      foreach (List<VisObject> parentLevel in scheme.ParentLevels)
      {
        foreach (VisObject visObject in parentLevel)
          allObjects.Add(visObject);
      }
    }
    return allObjects;
  }

  internal static long CostFunction(
    VisRelation rel,
    List<QualityLayout.RelationStru> allRelations,
    List<VisObject> allObjects,
    int parentOrgX,
    int parentOrgY,
    int childOrgX,
    int childOrgY)
  {
    long num = 0;
    int level1 = rel.Parent.Level;
    int level2 = rel.Child.Level;
    foreach (QualityLayout.RelationStru allRelation in allRelations)
    {
      if (allRelation.rel != rel && allRelation.MinLevel <= level2 && allRelation.MinLevel >= level1 && VisLayout.DoLinesCross(parentOrgX, parentOrgY, childOrgX, childOrgY, allRelation.ParentOrg, allRelation.ChildOrg))
        num += 10L;
    }
    foreach (VisObject allObject in allObjects)
      num += VisLayout.RelCrossObjectCost(allObject.Org, parentOrgX, parentOrgY, childOrgX, childOrgY);
    return num;
  }

  internal class LayoutInfo
  {
    public int FutureY { get; set; }

    public VisObject VisObj { get; set; }

    public LayoutInfo(VisObject vo, int futureY)
    {
      this.VisObj = vo;
      this.FutureY = futureY;
    }
  }

  internal class SumCostInfo
  {
    internal List<QualityLayout.RelationStru> _rels;
    internal List<VisObject> _objs;

    public int Index { get; set; }

    public long SumCost { get; set; }

    public VisObject VisObj { get; set; }

    public int FutureY { get; set; }

    public SumCostInfo(
      int index,
      QualityLayout.LayoutInfo li,
      List<QualityLayout.RelationStru> rels,
      List<VisObject> objs)
    {
      this.Index = index;
      this.VisObj = li.VisObj;
      this.FutureY = li.FutureY;
      this._rels = rels;
      this._objs = objs;
      this.SumCost = this.CalcSumCost();
    }

    public long CalcSumCost()
    {
      long num1 = 0;
      if (this.VisObj.Level > 0)
      {
        foreach (VisRelation parentRel in this.VisObj.ParentRels)
        {
          long num2 = num1;
          VisRelation rel = parentRel;
          List<QualityLayout.RelationStru> rels = this._rels;
          List<VisObject> objs = this._objs;
          Point org = parentRel.Parent.Org;
          int x1 = org.X;
          org = parentRel.Parent.Org;
          int y = org.Y;
          org = parentRel.Child.Org;
          int x2 = org.X;
          int futureY = this.FutureY;
          long num3 = QualityLayout.CostFunction(rel, rels, objs, x1, y, x2, futureY);
          num1 = num2 + num3;
        }
      }
      else
      {
        foreach (VisRelation childRel in this.VisObj.ChildRels)
        {
          long num4 = num1;
          VisRelation rel = childRel;
          List<QualityLayout.RelationStru> rels = this._rels;
          List<VisObject> objs = this._objs;
          Point org = childRel.Parent.Org;
          int x3 = org.X;
          int futureY = this.FutureY;
          org = childRel.Child.Org;
          int x4 = org.X;
          org = childRel.Child.Org;
          int y = org.Y;
          long num5 = QualityLayout.CostFunction(rel, rels, objs, x3, futureY, x4, y);
          num1 = num4 + num5;
        }
      }
      return num1;
    }
  }

  internal enum OptOperation
  {
    None,
    Move,
    Swap,
    LongMove,
  }

  internal class OptResult
  {
    public QualityLayout.OptOperation Oper { get; set; }

    public int Param { get; set; }

    public long Cost { get; set; }

    public OptResult(QualityLayout.OptOperation oper, int param, long cost)
    {
      this.Oper = oper;
      this.Param = param;
      this.Cost = cost;
    }

    public void Assign(QualityLayout.OptResult other)
    {
      this.Oper = other.Oper;
      this.Cost = other.Cost;
      this.Param = other.Param;
    }
  }

  internal class RelationStru
  {
    public VisRelation rel;

    public int MinLevel { get; set; }

    public int MaxLevel { get; set; }

    public Point ParentOrg { get; }

    public Point ChildOrg { get; }

    public RelationStru(VisRelation Rel)
    {
      this.rel = Rel;
      this.MinLevel = Rel.Parent.Level;
      this.MaxLevel = Rel.Child.Level;
      this.ParentOrg = Rel.Parent.Org;
      this.ChildOrg = Rel.Child.Org;
    }
  }
}
