// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TechEntityFix
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump;

[Serializable]
internal class TechEntityFix
{
  public static string F_CODE = nameof (F_CODE);
  public static string F_TABLE = nameof (F_TABLE);
  public static string F_FIELD = nameof (F_FIELD);
  public static int idx_F_CODE = -1;
  public static int idx_F_TABLE = -1;
  public static int idx_F_FIELD = -1;
  public string TableName = string.Empty;
  public string FieldName = string.Empty;
  public string EntCode = string.Empty;

  public static void ParseSchema(Dictionary<string, int> schema)
  {
    TechEntityFix.idx_F_CODE = schema[TechEntityFix.F_CODE];
    TechEntityFix.idx_F_TABLE = schema[TechEntityFix.F_TABLE];
    TechEntityFix.idx_F_FIELD = schema[TechEntityFix.F_FIELD];
  }

  public static TechEntityFix Parse(IDataReader idr)
  {
    return new TechEntityFix()
    {
      EntCode = idr.GetString(TechEntityFix.idx_F_CODE),
      TableName = idr.GetString(TechEntityFix.idx_F_TABLE),
      FieldName = idr.GetString(TechEntityFix.idx_F_FIELD)
    };
  }
}
