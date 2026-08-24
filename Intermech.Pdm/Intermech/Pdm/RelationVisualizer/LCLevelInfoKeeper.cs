// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.LCLevelInfoKeeper
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class LCLevelInfoKeeper
{
  public static readonly LCLevelInfoKeeper lcIK = new LCLevelInfoKeeper();
  private Dictionary<int, string> _lcNameCollect;

  private void _InitLifecycleLevels(IUserSession session)
  {
    IDBLifecycleLevelCollection lifecycleLevelCollection = session.GetLifecycleLevelCollection();
    if (lifecycleLevelCollection == null)
      return;
    DataTable dataTable = lifecycleLevelCollection.Select("F_LEVEL_NAME", (object) string.Empty);
    if (dataTable == null)
      return;
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      int int32 = Convert.ToInt32(row["F_LEVEL_ID"]);
      string str = Convert.ToString(row["F_LEVEL_NAME"]);
      if (!LCLevelInfoKeeper.lcIK._lcNameCollect.ContainsKey(int32))
        LCLevelInfoKeeper.lcIK._lcNameCollect.Add(int32, str);
    }
  }

  public static void Init()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      LCLevelInfoKeeper.Init(sessionKeeper.Session);
  }

  public static void Init(IUserSession ius)
  {
    if (LCLevelInfoKeeper.lcIK._lcNameCollect == null)
      LCLevelInfoKeeper.lcIK._lcNameCollect = new Dictionary<int, string>();
    LCLevelInfoKeeper.lcIK._InitLifecycleLevels(ius);
  }

  public static void Clear(IUserSession ius = null)
  {
    if (LCLevelInfoKeeper.lcIK._lcNameCollect != null)
      LCLevelInfoKeeper.lcIK._lcNameCollect.Clear();
    else
      LCLevelInfoKeeper.lcIK._lcNameCollect = new Dictionary<int, string>();
    if (ius == null)
      return;
    LCLevelInfoKeeper.lcIK._InitLifecycleLevels(ius);
  }

  public static string GetLCName(int lcLevel)
  {
    string lcName = (string) null;
    LCLevelInfoKeeper.lcIK._lcNameCollect.TryGetValue(lcLevel, out lcName);
    return lcName;
  }
}
