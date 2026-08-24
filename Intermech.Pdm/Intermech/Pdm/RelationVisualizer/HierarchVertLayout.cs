// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.HierarchVertLayout
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class HierarchVertLayout : HierarchLayout
{
  public HierarchVertLayout() => this.Vertical = true;

  public override void _DoSetInitialLevelCoords(
    VisLevel level,
    List<VisObject> sortList,
    int levNum,
    Point centerPoint)
  {
    int num1 = 0;
    for (int index = 0; index < sortList.Count; ++index)
    {
      VisObject sort = sortList[index];
      int num2 = -1;
      if (index % 2 != 0)
      {
        num1 += VisLayout._DistX;
        num2 = 1;
      }
      int num3 = index % 2 * VisLayout._DistY / 2;
      Point point = new Point(num2 * num1 + centerPoint.X, levNum * VisLayout._DistY + centerPoint.Y + num3);
      sort.Org = point;
    }
    level.Clear();
    level.AddRange((IEnumerable<VisObject>) sortList.OrderBy<VisObject, int>((Func<VisObject, int>) (vo => vo.Org.X)));
  }

  public override LayoutKind GetLayoutKind() => LayoutKind.VertHier;

  public new static LayoutKind GetKind() => LayoutKind.VertHier;
}
