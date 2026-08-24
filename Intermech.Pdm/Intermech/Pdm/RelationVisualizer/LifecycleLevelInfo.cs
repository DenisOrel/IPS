// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.LifecycleLevelInfo
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class LifecycleLevelInfo
{
  private static Dictionary<int, string> LevelNameCollect = new Dictionary<int, string>();

  public static void InitLifecycleLevel(IUserSession session)
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
      if (!LifecycleLevelInfo.LevelNameCollect.ContainsKey(int32))
        LifecycleLevelInfo.LevelNameCollect.Add(int32, str);
    }
  }

  public static string GetLCLevelName(int id)
  {
    string str = (string) null;
    return LifecycleLevelInfo.LevelNameCollect.TryGetValue(id, out str) ? str : (string) null;
  }
}
