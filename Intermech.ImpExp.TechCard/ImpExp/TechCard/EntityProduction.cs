// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.EntityProduction
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard;

[Serializable]
internal class EntityProduction
{
  public const string TableName = "TC_ENTITY_PR";
  private const string F_CODE = "F_CODE";
  private const string F_PRODUCTION = "F_PRODUCTION";
  private static int idx_F_CODE;
  private static int idx_F_PRODUCTION;
  public string Code = string.Empty;
  public int Production;

  public static void ParseSchema(Dictionary<string, int> schema)
  {
    EntityProduction.idx_F_CODE = schema["F_CODE"];
    EntityProduction.idx_F_PRODUCTION = schema["F_PRODUCTION"];
  }

  public static EntityProduction Parse(IDataReader idr)
  {
    return new EntityProduction()
    {
      Code = idr.IsDBNull(EntityProduction.idx_F_CODE) ? string.Empty : idr.GetString(EntityProduction.idx_F_CODE),
      Production = idr.IsDBNull(EntityProduction.idx_F_PRODUCTION) ? 0 : BasePumpHelper.ToInt32(idr[EntityProduction.idx_F_PRODUCTION])
    };
  }

  public override string ToString() => $"{this.Code}";
}
