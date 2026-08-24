// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.CondsPump.TechExpCond
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.CondsPump;

public static class TechExpCond
{
  public static class SQL
  {
    private const string CondKeySql = " SELECT F_CTLCONDKEY FROM TC_OSNCOND ";
    private const string FieldCondKeySql = " SELECT F_CTLCONDKEY FROM TC_OSNFLDCOND ";
    private const string CondCommon = " SELECT {0} FROM TC_CTLCOND WHERE  ( F_KEY IN ( SELECT F_CTLCONDKEY FROM TC_OSNCOND ) OR F_KEY IN ( SELECT F_CTLCONDKEY FROM TC_OSNFLDCOND )     ) {1} ";
    private const string CondCommon2 = " SELECT CC0.{0} FROM TC_CTLCOND CC0, TC_OSNCOND CC1  WHERE  (  CC0.F_KEY = CC1.F_CTLCONDKEY ) {1}  UNION  SELECT CC0.{0} FROM TC_CTLCOND CC0, TC_OSNFLDCOND CC1  WHERE  (  CC0.F_KEY = CC1.F_CTLCONDKEY ) {1} ";
    public static readonly string Cond = $" SELECT {"*"} FROM TC_CTLCOND WHERE  ( F_KEY IN ( SELECT F_CTLCONDKEY FROM TC_OSNCOND ) OR F_KEY IN ( SELECT F_CTLCONDKEY FROM TC_OSNFLDCOND )     ) {"{0}"} " + " ORDER BY F_KEY";
    public static readonly string CondCount = $" SELECT {"COUNT(*)"} FROM TC_CTLCOND WHERE  ( F_KEY IN ( SELECT F_CTLCONDKEY FROM TC_OSNCOND ) OR F_KEY IN ( SELECT F_CTLCONDKEY FROM TC_OSNFLDCOND )     ) {"{0}"} ";
    private static readonly string CondBlobCommon = $" SELECT   {{0}}  FROM  TC_CTLCONDBLOBS WHERE F_KEY IN   (    {string.Format(" SELECT CC0.{0} FROM TC_CTLCOND CC0, TC_OSNCOND CC1  WHERE  (  CC0.F_KEY = CC1.F_CTLCONDKEY ) {1}  UNION  SELECT CC0.{0} FROM TC_CTLCOND CC0, TC_OSNFLDCOND CC1  WHERE  (  CC0.F_KEY = CC1.F_CTLCONDKEY ) {1} ", (object) "F_COND", (object) "{1}")}  )  OR  F_KEY IN   (      {string.Format(" SELECT CC0.{0} FROM TC_CTLCOND CC0, TC_OSNCOND CC1  WHERE  (  CC0.F_KEY = CC1.F_CTLCONDKEY ) {1}  UNION  SELECT CC0.{0} FROM TC_CTLCOND CC0, TC_OSNFLDCOND CC1  WHERE  (  CC0.F_KEY = CC1.F_CTLCONDKEY ) {1} ", (object) "F_CONDCMP", (object) "{2}")}  )      ";
    public static readonly string CondBlob = string.Format(TechExpCond.SQL.CondBlobCommon, new object[3]
    {
      (object) "*",
      (object) "{0}",
      (object) "{1}"
    }) + " ORDER BY F_KEY";
    public static readonly string CondBlobCount = string.Format(TechExpCond.SQL.CondBlobCommon, new object[3]
    {
      (object) "COUNT(*)",
      (object) "{0}",
      (object) "{1}"
    });
    private static readonly string CondParamsCommon = $" SELECT   {{0}}    FROM   TC_CTLCONDPARMS WHERE  F_CONDKEY IN  ( {string.Format(" SELECT CC0.{0} FROM TC_CTLCOND CC0, TC_OSNCOND CC1  WHERE  (  CC0.F_KEY = CC1.F_CTLCONDKEY ) {1}  UNION  SELECT CC0.{0} FROM TC_CTLCOND CC0, TC_OSNFLDCOND CC1  WHERE  (  CC0.F_KEY = CC1.F_CTLCONDKEY ) {1} ", (object) "F_KEY", (object) "{1}")} ) ";
    public static readonly string CondParams = string.Format(TechExpCond.SQL.CondParamsCommon, new object[2]
    {
      (object) "*",
      (object) "{0}"
    }) + " ORDER BY F_CONDKEY, F_INDEX";
    public static readonly string CondParamsCount = string.Format(TechExpCond.SQL.CondParamsCommon, new object[2]
    {
      (object) "COUNT(*)",
      (object) "{0}"
    });
  }

  [Serializable]
  public class TC_CTLCOND
  {
    public const string TableName = "TC_CTLCOND";
    public const string F_KEY = "F_KEY";
    private const string F_RESTYPE = "F_RESTYPE";
    private const string F_CTLKEY = "F_CTLKEY";
    public const string F_COND = "F_COND";
    public const string F_CONDCMP = "F_CONDCMP";
    private static int idx_F_KEY;
    private static int idx_F_RESTYPE;
    private static int idx_F_CTLKEY;
    private static int idx_F_COND;
    private static int idx_F_CONDCMP;
    public readonly int fKey;
    public readonly int fResType;
    private readonly int fCtlKey;
    public readonly int fCond;
    public readonly int fCondCmp;

    public static void ParseSchema(Dictionary<string, int> schema)
    {
      TechExpCond.TC_CTLCOND.idx_F_KEY = schema["F_KEY"];
      TechExpCond.TC_CTLCOND.idx_F_RESTYPE = schema["F_RESTYPE"];
      TechExpCond.TC_CTLCOND.idx_F_CTLKEY = schema["F_CTLKEY"];
      TechExpCond.TC_CTLCOND.idx_F_COND = schema["F_COND"];
      TechExpCond.TC_CTLCOND.idx_F_CONDCMP = schema["F_CONDCMP"];
    }

    public TC_CTLCOND(IDataReader dr)
    {
      this.fKey = dr.IsDBNull(TechExpCond.TC_CTLCOND.idx_F_KEY) ? 0 : BasePumpHelper.ToInt32(dr[TechExpCond.TC_CTLCOND.idx_F_KEY]);
      this.fResType = dr.IsDBNull(TechExpCond.TC_CTLCOND.idx_F_RESTYPE) ? 0 : BasePumpHelper.ToInt32(dr[TechExpCond.TC_CTLCOND.idx_F_RESTYPE]);
      this.fCtlKey = dr.IsDBNull(TechExpCond.TC_CTLCOND.idx_F_CTLKEY) ? 0 : BasePumpHelper.ToInt32(dr[TechExpCond.TC_CTLCOND.idx_F_CTLKEY]);
      this.fCond = dr.IsDBNull(TechExpCond.TC_CTLCOND.idx_F_COND) ? 0 : BasePumpHelper.ToInt32(dr[TechExpCond.TC_CTLCOND.idx_F_COND]);
      this.fCondCmp = dr.IsDBNull(TechExpCond.TC_CTLCOND.idx_F_CONDCMP) ? 0 : BasePumpHelper.ToInt32(dr[TechExpCond.TC_CTLCOND.idx_F_CONDCMP]);
    }
  }

  [Serializable]
  public class TC_CTLCONDBLOBS
  {
    public const string TableName = "TC_CTLCONDBLOBS";
    public const string F_KEY = "F_KEY";
    private const string F_BLOB = "F_BLOB";
    private static int idx_F_KEY;
    private static int idx_F_BLOB;
    public readonly int fKey;
    public readonly byte[] fBlob;

    public static void ParseSchema(Dictionary<string, int> schema)
    {
      TechExpCond.TC_CTLCONDBLOBS.idx_F_KEY = schema["F_KEY"];
      TechExpCond.TC_CTLCONDBLOBS.idx_F_BLOB = schema["F_BLOB"];
    }

    public TC_CTLCONDBLOBS(IDataReader dr)
    {
      this.fKey = dr.IsDBNull(TechExpCond.TC_CTLCONDBLOBS.idx_F_KEY) ? 0 : BasePumpHelper.ToInt32(dr[TechExpCond.TC_CTLCONDBLOBS.idx_F_KEY]);
      int bytes = dr.IsDBNull(TechExpCond.TC_CTLCONDBLOBS.idx_F_BLOB) ? 0 : (int) dr.GetBytes(TechExpCond.TC_CTLCONDBLOBS.idx_F_BLOB, 0L, (byte[]) null, 0, 0);
      this.fBlob = new byte[bytes];
      if (bytes == 0)
        return;
      dr.GetBytes(TechExpCond.TC_CTLCONDBLOBS.idx_F_BLOB, 0L, this.fBlob, 0, bytes);
    }
  }

  [Serializable]
  public class TC_CTLCONDPARMS
  {
    public const string TableName = "TC_CTLCONDPARMS";
    public const string F_CONDKEY = "F_CONDKEY";
    public const string F_INDEX = "F_INDEX";
    private const string F_CODE = "F_CODE";
    private const string F_ACTTYPE = "F_ACTTYPE";
    private static int idx_F_CONDKEY;
    private static int idx_F_INDEX;
    private static int idx_F_CODE;
    private static int idx_F_ACTTYPE;
    public readonly int fCondKey;
    private int fIndex;
    public readonly string fCode;
    private int fActType;

    public static void ParseSchema(Dictionary<string, int> schema)
    {
      TechExpCond.TC_CTLCONDPARMS.idx_F_CONDKEY = schema["F_CONDKEY"];
      TechExpCond.TC_CTLCONDPARMS.idx_F_INDEX = schema["F_INDEX"];
      TechExpCond.TC_CTLCONDPARMS.idx_F_CODE = schema["F_CODE"];
      TechExpCond.TC_CTLCONDPARMS.idx_F_ACTTYPE = schema["F_ACTTYPE"];
    }

    public TC_CTLCONDPARMS(IDataReader dr)
    {
      this.fCondKey = dr.IsDBNull(TechExpCond.TC_CTLCONDPARMS.idx_F_CONDKEY) ? 0 : BasePumpHelper.ToInt32(dr[TechExpCond.TC_CTLCONDPARMS.idx_F_CONDKEY]);
      this.fIndex = dr.IsDBNull(TechExpCond.TC_CTLCONDPARMS.idx_F_INDEX) ? 0 : BasePumpHelper.ToInt32(dr[TechExpCond.TC_CTLCONDPARMS.idx_F_INDEX]);
      this.fCode = dr.IsDBNull(TechExpCond.TC_CTLCONDPARMS.idx_F_CODE) ? "" : dr.GetString(TechExpCond.TC_CTLCONDPARMS.idx_F_CODE);
      this.fActType = dr.IsDBNull(TechExpCond.TC_CTLCONDPARMS.idx_F_ACTTYPE) ? 0 : BasePumpHelper.ToInt32(dr[TechExpCond.TC_CTLCONDPARMS.idx_F_ACTTYPE]);
    }
  }
}
