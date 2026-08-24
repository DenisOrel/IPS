// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisLevel
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class VisLevel : List<VisObject>
{
  public int LevelNum { get; set; }

  public VisScheme ParentScheme { get; set; }

  public VisLevel(int levNum, VisScheme scheme)
  {
    this.LevelNum = levNum;
    this.ParentScheme = scheme;
  }

  public bool ContainsObjId(long objId)
  {
    return this.Exists((Predicate<VisObject>) (vo => vo.ObjVerId == objId));
  }

  public int VisibleCount()
  {
    int num = 0;
    foreach (VisObject visObject in (List<VisObject>) this)
    {
      if (visObject.Visible)
        ++num;
    }
    return num;
  }

  public void ForEachVisible(Action<VisObject> action)
  {
    foreach (VisObject visObject in (List<VisObject>) this)
    {
      if (visObject.Visible)
        action(visObject);
    }
  }

  public IEnumerable<VisObject> SelectVisible()
  {
    return this.Where<VisObject>((Func<VisObject, bool>) (vo => vo.Visible));
  }
}
