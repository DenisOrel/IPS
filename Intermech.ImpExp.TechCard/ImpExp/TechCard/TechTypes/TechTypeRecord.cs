// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechTypes.TechTypeRecord
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechTypes;

public class TechTypeRecord
{
  public static string F_TYPE = nameof (F_TYPE);
  public static string F_NAME = nameof (F_NAME);
  public static string F_DOPTYPES = nameof (F_DOPTYPES);
  public static string F_SAVING = nameof (F_SAVING);
  public static string F_PREDEFID = nameof (F_PREDEFID);
  public static string F_RECORDID = nameof (F_RECORDID);
  public static int idx_F_TYPE = -1;
  public static int idx_F_NAME = -1;
  public static int idx_F_DOPTYPES = -1;
  public static int idx_F_SAVING = -1;
  public static int idx_F_PREDEFID = -1;
  public static int idx_F_RECORDID = -1;

  public static void ParseSchema(Dictionary<string, int> schema)
  {
    TechTypeRecord.idx_F_TYPE = schema[TechTypeRecord.F_TYPE];
    TechTypeRecord.idx_F_NAME = schema[TechTypeRecord.F_NAME];
    TechTypeRecord.idx_F_DOPTYPES = schema[TechTypeRecord.F_DOPTYPES];
    TechTypeRecord.idx_F_SAVING = schema[TechTypeRecord.F_SAVING];
    TechTypeRecord.idx_F_PREDEFID = schema[TechTypeRecord.F_PREDEFID];
    TechTypeRecord.idx_F_RECORDID = schema[TechTypeRecord.F_RECORDID];
  }

  public static TechTypeInfo Parse(IDataReader idr)
  {
    return new TechTypeInfo()
    {
      Type = idr.IsDBNull(TechTypeRecord.idx_F_TYPE) ? string.Empty : idr.GetString(TechTypeRecord.idx_F_TYPE),
      Name = idr.IsDBNull(TechTypeRecord.idx_F_NAME) ? string.Empty : idr.GetString(TechTypeRecord.idx_F_NAME),
      DopTypes = idr.IsDBNull(TechTypeRecord.idx_F_DOPTYPES) ? string.Empty : idr.GetString(TechTypeRecord.idx_F_DOPTYPES),
      Saving = !idr.IsDBNull(TechTypeRecord.idx_F_SAVING) && idr.GetString(TechTypeRecord.idx_F_SAVING).Equals("T"),
      PredefID = idr.IsDBNull(TechTypeRecord.idx_F_PREDEFID) ? 0 : BasePumpHelper.ToInt32(idr[TechTypeRecord.idx_F_PREDEFID]),
      RecordID = idr.IsDBNull(TechTypeRecord.idx_F_RECORDID) ? 0 : BasePumpHelper.ToInt32(idr[TechTypeRecord.idx_F_RECORDID])
    };
  }
}
