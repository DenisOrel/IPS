// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisLayout
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public abstract class VisLayout : IVisLayout
{
  internal Dictionary<long, VisLayout.LayObjInfo> layDict;
  internal static readonly int _DistX = 160 /*0xA0*/;
  internal static readonly int _DistY = 60;
  internal static readonly int ALLOWED_LOSS = 100;

  protected static float GetNodeDistance(PointF a, PointF b)
  {
    double num1 = (double) a.X - (double) b.X;
    float num2 = a.Y - b.Y;
    return (float) Math.Sqrt(num1 * num1 + (double) num2 * (double) num2);
  }

  protected static long GetSquaredDistance(Point a, Point b)
  {
    int num1 = a.X - b.X;
    int num2 = a.Y - b.Y;
    return (long) (num1 * num1) + (long) (num2 * num2);
  }

  protected static bool DoLinesCross(PointF A, PointF B, PointF C, PointF D)
  {
    float num1 = Math.Max(A.X, B.X);
    float num2 = Math.Max(A.Y, B.Y);
    double num3 = (double) Math.Min(A.X, B.X);
    float num4 = Math.Min(A.Y, B.Y);
    float num5 = Math.Max(C.X, D.X);
    float num6 = Math.Max(C.Y, D.Y);
    float num7 = Math.Min(C.X, D.X);
    float num8 = Math.Min(C.Y, D.Y);
    double num9 = (double) num5;
    if (num3 > num9 || (double) num1 < (double) num7 || (double) num4 > (double) num6 || (double) num2 < (double) num8)
      return false;
    float num10 = B.X - A.X;
    float num11 = B.Y - A.Y;
    float num12 = D.X - C.X;
    float num13 = D.Y - C.Y;
    float num14 = A.X - C.X;
    float num15 = A.Y - C.Y;
    float num16;
    float num17;
    float num18;
    float num19;
    float num20;
    return (double) (num16 = (float) ((double) num13 * (double) num10 - (double) num12 * (double) num11)) != 0.0 && ((double) num16 <= 0.0 || (double) (num17 = (float) ((double) num10 * (double) num15 - (double) num11 * (double) num14)) >= 0.0 && (double) num17 <= (double) num16 && (double) (num18 = (float) ((double) num12 * (double) num15 - (double) num13 * (double) num14)) >= 0.0 && (double) num18 <= (double) num16) && (double) (num19 = -(float) ((double) num10 * (double) num15 - (double) num11 * (double) num14)) >= 0.0 && (double) num19 <= -(double) num16 && (double) (num20 = -(float) ((double) num12 * (double) num15 - (double) num13 * (double) num14)) >= 0.0 && (double) num20 <= -(double) num16;
  }

  protected static bool DoLinesCross(int aX, int aY, int bX, int bY, Point c, Point d)
  {
    int num1 = Math.Max(aX, bX);
    int num2 = Math.Max(aY, bY);
    int num3 = Math.Min(aX, bX);
    int num4 = Math.Min(aY, bY);
    int num5 = Math.Max(c.X, d.X);
    int num6 = Math.Max(c.Y, d.Y);
    int num7 = Math.Min(c.X, d.X);
    int num8 = Math.Min(c.Y, d.Y);
    int num9 = num5;
    if (num3 > num9 || num1 < num7 || num4 > num6 || num2 < num8)
      return false;
    int num10 = bX - aX;
    int num11 = bY - aY;
    int num12 = d.X - c.X;
    int num13 = d.Y - c.Y;
    int num14 = aX - c.X;
    int num15 = aY - c.Y;
    long num16;
    long num17;
    long num18;
    long num19;
    long num20;
    return (num16 = (long) num13 * (long) num10 - (long) num12 * (long) num11) != 0L && (num16 <= 0L || (num17 = (long) num10 * (long) num15 - (long) num11 * (long) num14) >= 0L && num17 <= num16 && (num18 = (long) num12 * (long) num15 - (long) num13 * (long) num14) >= 0L && num18 <= num16) && (num19 = -(long) num10 * (long) num15 - (long) num11 * (long) num14) >= 0L && num19 <= -num16 && (num20 = -(long) num12 * (long) num15 - (long) num13 * (long) num14) >= 0L && num20 <= -num16;
  }

  protected static long RelCrossObjectCost(Point Org, int aX, int aY, int bX, int bY)
  {
    long num1 = (long) (Org.X - aX);
    long num2 = (long) (Org.Y - aY);
    long num3 = (long) (bX - aX);
    long num4 = (long) (bY - aY);
    long num5 = num3;
    long num6 = num1 * num5 + num2 * num4;
    long num7 = num3 * num3 + num4 * num4;
    if (num6 <= 0L || num6 >= num7)
      return 0;
    long num8 = (long) (Org.X - aX) - num6 * num3 / num7;
    long num9 = (long) (Org.Y - aY) - num6 * num4 / num7;
    long num10 = num8 * num8 + num9 * num9;
    if (num10 == 0L)
      return 1000;
    return num10 > 900L ? 0L : (900L - num10) * 1000L / 900L;
  }

  public VisLayout()
  {
    this.layDict = new Dictionary<long, VisLayout.LayObjInfo>();
    this.Vertical = false;
  }

  public virtual void InitLayout(Size logSize, VisScheme scheme) => this.layDict.Clear();

  public abstract void BeforeLayoutLevel(VisScheme scheme, int levelNum, bool parent);

  public abstract void BeforeLayout(BasePredicate cancel, VisScheme scheme);

  public bool Vertical { get; set; }

  public virtual void SetInitialLevelCoords(VisLevel level, int levNum, Point centerPoint)
  {
    level.ForEach((Action<VisObject>) (vo =>
    {
      if (this.layDict.ContainsKey(vo.ObjVerId))
        return;
      this.layDict.Add(vo.ObjVerId, new VisLayout.LayObjInfo(vo));
    }));
    List<VisObject> list = level.ToList<VisObject>();
    list.Sort(new Comparison<VisObject>(this.CompareVisObjects));
    this._DoSetInitialLevelCoords(level, list, levNum, centerPoint);
  }

  internal int CompareVisObjects(VisObject vo1, VisObject vo2)
  {
    return (vo1.Visible ? this.layDict[vo1.ObjVerId].Sum : int.MaxValue) - (vo2.Visible ? this.layDict[vo2.ObjVerId].Sum : int.MaxValue);
  }

  public virtual void _DoSetInitialLevelCoords(
    VisLevel level,
    List<VisObject> sortList,
    int levNum,
    Point centerPoint)
  {
    int num1 = 0;
    for (int index = 0; index < sortList.Count; ++index)
    {
      VisObject sort = sortList[index];
      if (sort.Visible)
      {
        int num2 = -1;
        if (index % 2 != 0)
        {
          num1 += VisLayout._DistY;
          if (sort.Preview != null)
            num1 += 8;
          num2 = 1;
        }
        int num3 = Math.Abs(levNum) % 2 * VisLayout._DistY / 2;
        sort.Org = new Point(levNum * VisLayout._DistX + centerPoint.X, num2 * num1 + centerPoint.Y + num3);
      }
    }
    level.Clear();
    level.AddRange((IEnumerable<VisObject>) sortList.OrderBy<VisObject, int>((Func<VisObject, int>) (vo => vo.Org.Y)));
  }

  public virtual void SetInitialCoords(BasePredicate cancel, VisScheme scheme, Point centerPoint)
  {
    scheme.RootObj.Org = centerPoint;
    int num1 = 1;
    if (scheme.ChildLevels != null)
    {
      for (int index = 0; index < scheme.ChildLevels.Count; ++index)
      {
        VisLevel childLevel = scheme.ChildLevels[index];
        if (childLevel.Count<VisObject>() > 0)
          this.SetInitialLevelCoords(childLevel, num1++, centerPoint);
        if (cancel())
          return;
      }
    }
    int num2 = -1;
    if (scheme.ParentLevels == null)
      return;
    for (int index = 0; index < scheme.ParentLevels.Count; ++index)
    {
      VisLevel parentLevel = scheme.ParentLevels[index];
      if (parentLevel.Count<VisObject>() > 0)
        this.SetInitialLevelCoords(parentLevel, num2--, centerPoint);
      if (cancel())
        break;
    }
  }

  private Dictionary<long, int> _CalcExpectedPlaces(VisLevel level)
  {
    Dictionary<long, int> dictionary = new Dictionary<long, int>();
    int levelNum = level.LevelNum;
    bool flag = levelNum > 0;
    foreach (VisObject visObject in (List<VisObject>) level)
    {
      int count = 0;
      int multiplier = 0;
      int Sum = 0;
      if (flag)
      {
        visObject.ParentRels.ForEach((Action<VisRelation>) (rel =>
        {
          int num = levelNum - rel.Parent.ParentLevel.LevelNum - 1;
          multiplier = num == 0 ? 1 : num * 5;
          Sum += rel.Parent.Org.Y * multiplier;
          count += multiplier;
        }));
        Sum /= count;
      }
      else
      {
        visObject.ChildRels.ForEach((Action<VisRelation>) (rel =>
        {
          int num = rel.Child.ParentLevel.LevelNum - 1 - levelNum;
          multiplier = num == 0 ? 1 : num * 5;
          Sum += rel.Parent.Org.Y * multiplier;
          count += multiplier;
        }));
        Sum /= count;
      }
      dictionary.Add(visObject.ObjVerId, Sum);
    }
    return dictionary;
  }

  public virtual void DoLayout(BasePredicate cancel, VisScheme scheme)
  {
    if (scheme.Loaded.ChildsLoaded)
    {
      foreach (VisLevel childLevel in scheme.ChildLevels)
      {
        int levelNum = childLevel.LevelNum;
      }
    }
    if (!scheme.Loaded.ParentsLoaded)
      return;
    foreach (VisLevel childLevel in scheme.ChildLevels)
    {
      int levelNum = childLevel.LevelNum;
    }
  }

  public virtual LayoutKind GetLayoutKind() => LayoutKind.Unknown;

  public static string GetDescription(LayoutKind kind)
  {
    switch (kind)
    {
      case LayoutKind.Normal:
        return "-Н";
      case LayoutKind.Hierarchical:
        return "-И";
      case LayoutKind.Custom:
        return "-О";
      default:
        return "-?";
    }
  }

  public void ChangeSizes(VisScheme scheme, int xCoef, int yCoef)
  {
    PointF location = scheme.RootObj.Node.Location;
    if (scheme.ChildLevels != null)
    {
      foreach (VisLevel childLevel in scheme.ChildLevels)
        this._processLevel(childLevel, location, xCoef, yCoef);
    }
    if (scheme.ParentLevels == null)
      return;
    foreach (VisLevel parentLevel in scheme.ParentLevels)
      this._processLevel(parentLevel, location, xCoef, yCoef);
  }

  private void _processLevel(VisLevel level, PointF rootOrg, int xCoef, int yCoef)
  {
    foreach (VisObject visObject in (List<VisObject>) level)
    {
      if (visObject.Node != null)
      {
        float num1 = visObject.Node.Location.X - rootOrg.X;
        float num2 = visObject.Node.Location.Y - rootOrg.Y;
        visObject.Node.Location = new PointF(rootOrg.X + (float) ((double) num1 * (double) xCoef / 100.0), rootOrg.Y + (float) ((double) num2 * (double) yCoef / 100.0));
      }
    }
  }

  public void RestoreLevels(VisScheme scheme)
  {
    if (scheme.ParentLevels != null)
    {
      foreach (VisLevel parentLevel1 in scheme.ParentLevels)
      {
        for (int index = parentLevel1.Count - 1; index >= 0; --index)
        {
          VisObject visObject = parentLevel1[index];
          if (visObject.Level != parentLevel1.LevelNum)
          {
            parentLevel1.RemoveAt(index);
            VisLevel parentLevel2 = scheme.ParentLevels[Math.Abs(visObject.Level) - 1];
            parentLevel2.Add(visObject);
            visObject.ParentLevel = parentLevel2;
          }
        }
      }
    }
    if (scheme.ChildLevels == null)
      return;
    foreach (VisLevel childLevel1 in scheme.ChildLevels)
    {
      for (int index = childLevel1.Count - 1; index >= 0; --index)
      {
        VisObject visObject = childLevel1[index];
        if (visObject.Level != childLevel1.LevelNum)
        {
          childLevel1.RemoveAt(index);
          VisLevel childLevel2 = scheme.ChildLevels[visObject.Level - 1];
          childLevel2.Add(visObject);
          visObject.ParentLevel = childLevel2;
        }
      }
    }
  }

  public void ProcessInvisible(VisScheme scheme)
  {
    scheme.RootObj.ChildsParentsVisible = scheme.RootObj.ChildsOpen;
    foreach (List<VisObject> childLevel in scheme.ChildLevels)
    {
      foreach (VisObject visObject in childLevel)
      {
        bool visible = visObject.Visible;
        try
        {
          visObject.Visible = false;
          visObject.ChildsParentsVisible = false;
          foreach (VisRelation parentRel in visObject.ParentRels)
          {
            if (parentRel.Parent.ChildsParentsVisible)
            {
              visObject.Visible = true;
              visObject.ChildsParentsVisible = visObject.ChildsOpen;
              break;
            }
          }
        }
        finally
        {
          if (visObject.Visible != visible)
            visObject.VisibleChanged = true;
        }
      }
    }
    scheme.RootObj.ChildsParentsVisible = scheme.RootObj.ParentsOpen;
    foreach (List<VisObject> parentLevel in scheme.ParentLevels)
    {
      foreach (VisObject visObject in parentLevel)
      {
        bool visible = visObject.Visible;
        try
        {
          visObject.Visible = false;
          visObject.ChildsParentsVisible = false;
          foreach (VisRelation childRel in visObject.ChildRels)
          {
            if (childRel.Child.ChildsParentsVisible)
            {
              visObject.Visible = true;
              visObject.ChildsParentsVisible = visObject.ParentsOpen;
              break;
            }
          }
        }
        finally
        {
          if (visObject.Visible != visible)
            visObject.VisibleChanged = true;
        }
      }
    }
  }

  public class LayObjInfo
  {
    public int Sum { get; set; }

    public int Priority { get; set; }

    public VisObject VisObj { get; set; }

    public LayObjInfo(VisObject obj)
    {
      int num1 = obj.ParentLevel.LevelNum > 0 ? 1 : 0;
      this.Priority = obj.ChildRels.Count + obj.ParentRels.Count;
      this.VisObj = obj;
      this.Sum = 0;
      if (num1 != 0)
        obj.ChildRels.ForEach((Action<VisRelation>) (rel =>
        {
          int num2 = rel.ChildLevelNum - 2;
          if (num2 == 0)
            --this.Sum;
          else
            this.Sum += num2 * 5;
        }));
      else
        obj.ParentRels.ForEach((Action<VisRelation>) (rel =>
        {
          int num3 = 2 - rel.ParentLevelNum;
          if (num3 == 0)
            --this.Sum;
          else
            this.Sum += num3 * 5;
        }));
    }
  }
}
