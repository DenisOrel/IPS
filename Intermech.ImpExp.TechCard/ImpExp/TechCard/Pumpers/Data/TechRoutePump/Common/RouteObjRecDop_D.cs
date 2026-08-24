// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.Common.RouteObjRecDop_D
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.Common;

internal class RouteObjRecDop_D : TechObjectRecordSub_D
{
  public const string F_KIND = "F_KIND";
  public const string F_STR_VALUE = "F_STR_VALUE";
  public const string F_NUM_VALUE = "F_NUM_VALUE";
  public static int idx_F_KIND;
  public static int idx_F_STR_VALUE;
  public static int idx_F_NUM_VALUE;

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    RouteObjRecDop_D.idx_F_KIND = schema["F_KIND"];
    RouteObjRecDop_D.idx_F_STR_VALUE = schema["F_STR_VALUE"];
    RouteObjRecDop_D.idx_F_NUM_VALUE = schema["F_NUM_VALUE"];
  }

  public override void Parse(IDataReader idr)
  {
    base.Parse(idr);
    switch (idr.IsDBNull(RouteObjRecDop_D.idx_F_KIND) ? 1L : (long) BasePumpHelper.ToInt32(idr[RouteObjRecDop_D.idx_F_KIND]))
    {
      case 1:
        this.Value = (object) Convert.ToDouble(idr.IsDBNull(RouteObjRecDop_D.idx_F_NUM_VALUE) ? 0.0 : BasePumpHelper.ToDouble(idr[RouteObjRecDop_D.idx_F_NUM_VALUE]));
        break;
      case 2:
        this.Value = idr.IsDBNull(RouteObjRecDop_D.idx_F_STR_VALUE) ? (object) string.Empty : (object) idr.GetString(RouteObjRecDop_D.idx_F_STR_VALUE);
        break;
    }
  }
}
