// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Production
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard;

internal class Production
{
  public const string TableName = "TC_PRODUCTIONS";
  public const string F_PRODUCTION = "F_PRODUCTION";
  public const string F_NAME = "F_NAME";
  public const string F_VERSION = "F_VERSION";
  public const string F_LITERA = "F_LITERA";
  public const string F_FLAGS = "F_FLAGS";
  public const string F_LOC_LITERA = "F_LOC_LITERA";
  public const string F_NUM_IN_TP = "F_NUM_IN_TP";
  public static int idx_F_PRODUCTION;
  public static int idx_F_NAME;
  public static int idx_F_VERSION;
  public static int idx_F_LITERA;
  public static int idx_F_FLAGS;
  public static int idx_F_LOC_LITERA;
  public static int idx_F_NUM_IN_TP;

  public static void ParseSchema(Dictionary<string, int> schema)
  {
    Production.idx_F_PRODUCTION = schema["F_PRODUCTION"];
    Production.idx_F_NAME = schema["F_NAME"];
    Production.idx_F_VERSION = schema["F_VERSION"];
    Production.idx_F_LITERA = schema["F_LITERA"];
    Production.idx_F_FLAGS = schema["F_FLAGS"];
    Production.idx_F_LOC_LITERA = schema["F_LOC_LITERA"];
    if (schema.TryGetValue("F_NUM_IN_TP", out Production.idx_F_NUM_IN_TP))
      return;
    Production.idx_F_NUM_IN_TP = -1;
  }

  public static ProductInfo Parse(IDataReader reader)
  {
    ProductInfo productInfo = (ProductInfo) null;
    if (!reader.IsDBNull(Production.idx_F_NAME))
    {
      productInfo = new ProductInfo();
      productInfo.ProductionID = reader.IsDBNull(Production.idx_F_PRODUCTION) ? 0 : BasePumpHelper.ToInt32(reader[Production.idx_F_PRODUCTION]);
      productInfo.Name = reader.IsDBNull(Production.idx_F_NAME) ? string.Empty : reader.GetString(Production.idx_F_NAME);
      productInfo.Version = reader.IsDBNull(Production.idx_F_VERSION) ? 0 : BasePumpHelper.ToInt32(reader[Production.idx_F_VERSION]);
      productInfo.Litera = reader.IsDBNull(Production.idx_F_LITERA) ? string.Empty : reader.GetString(Production.idx_F_LITERA);
      productInfo.Flags = reader.IsDBNull(Production.idx_F_FLAGS) ? 0 : BasePumpHelper.ToInt32(reader[Production.idx_F_FLAGS]);
      productInfo.Loc_Litera = reader.IsDBNull(Production.idx_F_LOC_LITERA) ? string.Empty : reader.GetString(Production.idx_F_LOC_LITERA);
      if (Production.idx_F_NUM_IN_TP != -1)
        productInfo.NumInTP = reader.IsDBNull(Production.idx_F_NUM_IN_TP) ? 0 : BasePumpHelper.ToInt32(reader[Production.idx_F_NUM_IN_TP]);
    }
    return productInfo;
  }
}
