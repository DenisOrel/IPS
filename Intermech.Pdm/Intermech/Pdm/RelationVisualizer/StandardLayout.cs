// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.StandardLayout
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class StandardLayout : VisLayout
{
  internal int maxObjectsPerLevel;

  public override void InitLayout(Size logSize, VisScheme scheme)
  {
    base.InitLayout(logSize, scheme);
    this.maxObjectsPerLevel = logSize.Height / VisLayout._DistY + 2;
    if (scheme.ObjectCount / this.maxObjectsPerLevel * VisLayout._DistX * 1080 <= logSize.Height * 1920)
      return;
    this.maxObjectsPerLevel = this.maxObjectsPerLevel * 4 / 3;
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

  public virtual void CalculateWeight(VisObject vo, int levelNum, bool parent)
  {
    vo.LayoutWeight = parent ? vo.ParentRels.Count * 10 + vo.ChildRels.Count + (levelNum - Math.Abs(vo.Level)) * 100 : vo.ChildRels.Count * 10 + vo.ParentRels.Count + (levelNum - vo.Level) * 100;
  }

  protected void CollectMovingObjects(VisObject vo, bool parent, List<VisObject> movingObjects)
  {
    List<VisRelation> visRelationList = parent ? vo.ParentRels : vo.ChildRels;
    if (visRelationList.Count == 0)
      return;
    foreach (VisRelation visRelation in visRelationList)
    {
      VisObject vo1 = parent ? visRelation.Parent : visRelation.Child;
      if (!movingObjects.Contains(vo1))
      {
        movingObjects.Add(vo1);
        this.CollectMovingObjects(vo1, parent, movingObjects);
      }
    }
  }

  protected void _DoMoveObject(VisObject vo, bool parent, VisScheme scheme)
  {
    List<VisLevel> visLevelList = parent ? scheme.ParentLevels : scheme.ChildLevels;
    int num = parent ? -1 : 1;
    int levelNum = vo.ParentLevel.LevelNum;
    VisLevel visLevel;
    if (visLevelList.Count <= Math.Abs(levelNum))
      visLevelList.Add(visLevel = new VisLevel(levelNum + num, scheme));
    else
      visLevel = visLevelList[Math.Abs(levelNum)];
    vo.ParentLevel.Remove(vo);
    visLevel.Add(vo);
    vo.ParentLevel = visLevel;
  }

  public override void BeforeLayout(BasePredicate cancel, VisScheme scheme)
  {
    for (int levelNum = 0; levelNum < scheme.ParentLevels.Count; ++levelNum)
    {
      if (cancel())
        return;
      this.BeforeLayoutLevel(scheme, levelNum, true);
    }
    for (int levelNum = 0; levelNum < scheme.ChildLevels.Count && !cancel(); ++levelNum)
      this.BeforeLayoutLevel(scheme, levelNum, false);
  }

  public override LayoutKind GetLayoutKind() => LayoutKind.Normal;

  public static LayoutKind GetKind() => LayoutKind.Normal;
}
