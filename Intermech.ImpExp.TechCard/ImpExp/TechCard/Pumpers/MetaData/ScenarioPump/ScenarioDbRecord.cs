// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump.ScenarioDbRecord
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;

internal static class ScenarioDbRecord
{
  [Serializable]
  public class TC_ZSCEN
  {
    public const string TableName = "TC_ZSCEN";
    private const string F_SCENKEY = "F_SCENKEY";
    private const string F_ZAGCODE = "F_ZAGCODE";
    private const string F_TYPENAME = "F_TYPENAME";
    private const string F_VIDDET = "F_VIDDET";
    private const string F_CALCSCEN = "F_CALCSCEN";
    private const string F_PRODUCTION = "F_PRODUCTION";
    private static int idx_F_SCENKEY;
    private static int idx_F_ZAGCODE;
    private static int idx_F_TYPENAME;
    private static int idx_F_VIDDET;
    private static int idx_F_CALCSCEN;
    private static int idx_F_PRODUCTION;
    public readonly int Key;
    public readonly int ZagCode;
    public readonly int VidDet;
    public readonly int Production;
    public readonly string TypeName;

    public TC_ZSCEN(IDataReader reader)
    {
      this.Key = reader.IsDBNull(ScenarioDbRecord.TC_ZSCEN.idx_F_SCENKEY) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_ZSCEN.idx_F_SCENKEY]);
      this.ZagCode = reader.IsDBNull(ScenarioDbRecord.TC_ZSCEN.idx_F_ZAGCODE) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_ZSCEN.idx_F_ZAGCODE]);
      this.VidDet = reader.IsDBNull(ScenarioDbRecord.TC_ZSCEN.idx_F_VIDDET) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_ZSCEN.idx_F_VIDDET]);
      this.Production = reader.IsDBNull(ScenarioDbRecord.TC_ZSCEN.idx_F_PRODUCTION) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_ZSCEN.idx_F_PRODUCTION]);
      this.TypeName = reader.IsDBNull(ScenarioDbRecord.TC_ZSCEN.idx_F_TYPENAME) ? string.Empty : reader.GetString(ScenarioDbRecord.TC_ZSCEN.idx_F_TYPENAME);
    }

    public static void ParseSchema(IDataReader reader)
    {
      ScenarioDbRecord.TC_ZSCEN.idx_F_SCENKEY = reader.GetOrdinal("F_SCENKEY");
      ScenarioDbRecord.TC_ZSCEN.idx_F_ZAGCODE = reader.GetOrdinal("F_ZAGCODE");
      ScenarioDbRecord.TC_ZSCEN.idx_F_TYPENAME = reader.GetOrdinal("F_TYPENAME");
      ScenarioDbRecord.TC_ZSCEN.idx_F_VIDDET = reader.GetOrdinal("F_VIDDET");
      ScenarioDbRecord.TC_ZSCEN.idx_F_CALCSCEN = reader.GetOrdinal("F_CALCSCEN");
      ScenarioDbRecord.TC_ZSCEN.idx_F_PRODUCTION = reader.GetOrdinal("F_PRODUCTION");
    }
  }

  public class TC_SCRIPTRAS
  {
    public const string TableName = "TC_SCRIPTRAS";
    private const string F_SCEN = "F_SCEN";
    private const string F_CELL = "F_CELL";
    private const string F_CODE = "F_CODE";
    private const string F_ORDER = "F_ORDER";
    private static int idx_F_SCEN;
    private static int idx_F_CELL;
    private static int idx_F_CODE;
    private static int idx_F_ORDER;
    public readonly int Scen;
    public readonly int Cell;
    public readonly string Code;
    public readonly int Order;

    public TC_SCRIPTRAS(IDataReader reader)
    {
      this.Scen = reader.IsDBNull(ScenarioDbRecord.TC_SCRIPTRAS.idx_F_SCEN) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCRIPTRAS.idx_F_SCEN]);
      this.Cell = reader.IsDBNull(ScenarioDbRecord.TC_SCRIPTRAS.idx_F_CELL) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCRIPTRAS.idx_F_CELL]);
      this.Code = reader.IsDBNull(ScenarioDbRecord.TC_SCRIPTRAS.idx_F_CODE) ? string.Empty : reader.GetString(ScenarioDbRecord.TC_SCRIPTRAS.idx_F_CODE);
      this.Order = reader.IsDBNull(ScenarioDbRecord.TC_SCRIPTRAS.idx_F_CELL) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCRIPTRAS.idx_F_CELL]);
    }

    public static void ParseSchema(Dictionary<string, int> schema)
    {
      ScenarioDbRecord.TC_SCRIPTRAS.idx_F_SCEN = schema["F_SCEN"];
      ScenarioDbRecord.TC_SCRIPTRAS.idx_F_CELL = schema["F_CELL"];
      ScenarioDbRecord.TC_SCRIPTRAS.idx_F_CODE = schema["F_CODE"];
      ScenarioDbRecord.TC_SCRIPTRAS.idx_F_ORDER = schema["F_ORDER"];
    }
  }

  public class TC_SCRIPTS_XREF
  {
    public const string TableName = "TC_SCRIPTS_XREF";
    private const string F_KEY = "F_KEY";
    private const string F_CTLKEY = "F_CTLKEY";
    private const string F_LEVEL = "F_LEVEL";
    private const string F_SCRIPT = "F_SCRIPT";
    private const string F_PRODUCTION = "F_PRODUCTION";
    private static int idx_F_KEY;
    private static int idx_F_CTLKEY;
    private static int idx_F_LEVEL;
    private static int idx_F_SCRIPT;
    private static int idx_F_PRODUCTION;
    public readonly int Script;
    public readonly int Catalog;
    public readonly int Level;
    public readonly int Production;

    public TC_SCRIPTS_XREF(IDataReader reader)
    {
      this.Script = reader.IsDBNull(ScenarioDbRecord.TC_SCRIPTS_XREF.idx_F_SCRIPT) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCRIPTS_XREF.idx_F_SCRIPT]);
      this.Catalog = reader.IsDBNull(ScenarioDbRecord.TC_SCRIPTS_XREF.idx_F_CTLKEY) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCRIPTS_XREF.idx_F_CTLKEY]);
      this.Level = reader.IsDBNull(ScenarioDbRecord.TC_SCRIPTS_XREF.idx_F_LEVEL) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCRIPTS_XREF.idx_F_LEVEL]);
      this.Production = reader.IsDBNull(ScenarioDbRecord.TC_SCRIPTS_XREF.idx_F_PRODUCTION) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCRIPTS_XREF.idx_F_PRODUCTION]);
    }

    public static void ParseSchema(Dictionary<string, int> schema)
    {
      ScenarioDbRecord.TC_SCRIPTS_XREF.idx_F_KEY = schema["F_KEY"];
      ScenarioDbRecord.TC_SCRIPTS_XREF.idx_F_CTLKEY = schema["F_CTLKEY"];
      ScenarioDbRecord.TC_SCRIPTS_XREF.idx_F_LEVEL = schema["F_LEVEL"];
      ScenarioDbRecord.TC_SCRIPTS_XREF.idx_F_SCRIPT = schema["F_SCRIPT"];
      ScenarioDbRecord.TC_SCRIPTS_XREF.idx_F_PRODUCTION = schema["F_PRODUCTION"];
    }
  }

  public class TC_SCRIPTS
  {
    public const string TableName = "TC_SCRIPTS";
    private const string F_KEY = "F_KEY";
    private const string F_NAME = "F_NAME";
    private const string F_ROWCOUNT = "F_ROWCOUNT";
    private const string F_COLCOUNT = "F_COLCOUNT";
    private const string F_KIND = "F_KIND";
    private const string F_FLAGS = "F_FLAGS";
    private const string F_SLIDEID = "F_SLIDEID";
    private const string F_USED = "F_USED";
    private static int idx_F_KEY;
    private static int idx_F_NAME;
    private static int idx_F_ROWCOUNT;
    private static int idx_F_COLCOUNT;
    private static int idx_F_KIND;
    private static int idx_F_FLAGS;
    private static int idx_F_SLIDEID;
    private static int idx_F_USED;
    public readonly int Key;
    public readonly string Name;
    public readonly int RowCount;
    public readonly int ColCount;
    public readonly int Kind;
    public readonly int Flags;
    public readonly int SlideID;
    public readonly int Used;

    public TC_SCRIPTS(IDataReader dr)
    {
      this.Key = dr.IsDBNull(ScenarioDbRecord.TC_SCRIPTS.idx_F_KEY) ? 0 : BasePumpHelper.ToInt32(dr[ScenarioDbRecord.TC_SCRIPTS.idx_F_KEY]);
      this.Name = dr.IsDBNull(ScenarioDbRecord.TC_SCRIPTS.idx_F_NAME) ? string.Empty : dr.GetString(ScenarioDbRecord.TC_SCRIPTS.idx_F_NAME);
      this.RowCount = dr.IsDBNull(ScenarioDbRecord.TC_SCRIPTS.idx_F_ROWCOUNT) ? 0 : BasePumpHelper.ToInt32(dr[ScenarioDbRecord.TC_SCRIPTS.idx_F_ROWCOUNT]);
      this.ColCount = dr.IsDBNull(ScenarioDbRecord.TC_SCRIPTS.idx_F_COLCOUNT) ? 0 : BasePumpHelper.ToInt32(dr[ScenarioDbRecord.TC_SCRIPTS.idx_F_COLCOUNT]);
      this.Kind = dr.IsDBNull(ScenarioDbRecord.TC_SCRIPTS.idx_F_KIND) ? 0 : BasePumpHelper.ToInt32(dr[ScenarioDbRecord.TC_SCRIPTS.idx_F_KIND]);
      this.Flags = dr.IsDBNull(ScenarioDbRecord.TC_SCRIPTS.idx_F_FLAGS) ? 0 : BasePumpHelper.ToInt32(dr[ScenarioDbRecord.TC_SCRIPTS.idx_F_FLAGS]);
      this.SlideID = dr.IsDBNull(ScenarioDbRecord.TC_SCRIPTS.idx_F_SLIDEID) ? 0 : BasePumpHelper.ToInt32(dr[ScenarioDbRecord.TC_SCRIPTS.idx_F_SLIDEID]);
      this.Used = dr.IsDBNull(ScenarioDbRecord.TC_SCRIPTS.idx_F_USED) ? 0 : BasePumpHelper.ToInt32(dr[ScenarioDbRecord.TC_SCRIPTS.idx_F_USED]);
    }

    public static void ParseSchema(Dictionary<string, int> schema)
    {
      ScenarioDbRecord.TC_SCRIPTS.idx_F_KEY = schema["F_KEY"];
      ScenarioDbRecord.TC_SCRIPTS.idx_F_NAME = schema["F_NAME"];
      ScenarioDbRecord.TC_SCRIPTS.idx_F_ROWCOUNT = schema["F_ROWCOUNT"];
      ScenarioDbRecord.TC_SCRIPTS.idx_F_COLCOUNT = schema["F_COLCOUNT"];
      ScenarioDbRecord.TC_SCRIPTS.idx_F_KIND = schema["F_KIND"];
      ScenarioDbRecord.TC_SCRIPTS.idx_F_FLAGS = schema["F_FLAGS"];
      ScenarioDbRecord.TC_SCRIPTS.idx_F_SLIDEID = schema["F_SLIDEID"];
      ScenarioDbRecord.TC_SCRIPTS.idx_F_USED = schema["F_USED"];
    }
  }

  public class TC_SCCELLS
  {
    public const string TableName = "TC_SCCELLS";
    private const string F_KEY = "F_KEY";
    private const string F_SCEN = "F_SCEN";
    private const string F_NCOL = "F_NCOL";
    private const string F_NROW = "F_NROW";
    private const string F_DEFAULT = "F_DEFAULT";
    private const string F_CODE = "F_CODE";
    private const string F_FLAGS = "F_FLAGS";
    private const string F_FLTENTITY = "F_FLTENTITY";
    private static int idx_F_KEY;
    private static int idx_F_SCEN;
    private static int idx_F_NCOL;
    private static int idx_F_NROW;
    private static int idx_F_DEFAULT;
    private static int idx_F_CODE;
    private static int idx_F_FLAGS;
    private static int idx_F_FLTENTITY;
    public readonly int Key;
    public readonly int Scen;
    public readonly int NCol;
    public readonly int NRow;
    public readonly string Default;
    public readonly string Code;
    public readonly int Flags;
    public readonly string FltEntity;

    public TC_SCCELLS(IDataReader reader)
    {
      this.Key = reader.IsDBNull(ScenarioDbRecord.TC_SCCELLS.idx_F_KEY) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCCELLS.idx_F_KEY]);
      this.Scen = reader.IsDBNull(ScenarioDbRecord.TC_SCCELLS.idx_F_SCEN) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCCELLS.idx_F_SCEN]);
      this.NCol = reader.IsDBNull(ScenarioDbRecord.TC_SCCELLS.idx_F_NCOL) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCCELLS.idx_F_NCOL]);
      this.NRow = reader.IsDBNull(ScenarioDbRecord.TC_SCCELLS.idx_F_NROW) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCCELLS.idx_F_NROW]);
      this.Default = reader.IsDBNull(ScenarioDbRecord.TC_SCCELLS.idx_F_DEFAULT) ? string.Empty : reader.GetString(ScenarioDbRecord.TC_SCCELLS.idx_F_DEFAULT);
      this.Code = reader.IsDBNull(ScenarioDbRecord.TC_SCCELLS.idx_F_CODE) ? string.Empty : reader.GetString(ScenarioDbRecord.TC_SCCELLS.idx_F_CODE);
      this.Flags = reader.IsDBNull(ScenarioDbRecord.TC_SCCELLS.idx_F_FLAGS) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCCELLS.idx_F_FLAGS]);
      this.FltEntity = reader.IsDBNull(ScenarioDbRecord.TC_SCCELLS.idx_F_FLTENTITY) ? string.Empty : reader.GetString(ScenarioDbRecord.TC_SCCELLS.idx_F_FLTENTITY);
    }

    public static void ParseSchema(Dictionary<string, int> schema)
    {
      ScenarioDbRecord.TC_SCCELLS.idx_F_KEY = schema["F_KEY"];
      ScenarioDbRecord.TC_SCCELLS.idx_F_SCEN = schema["F_SCEN"];
      ScenarioDbRecord.TC_SCCELLS.idx_F_NCOL = schema["F_NCOL"];
      ScenarioDbRecord.TC_SCCELLS.idx_F_NROW = schema["F_NROW"];
      ScenarioDbRecord.TC_SCCELLS.idx_F_DEFAULT = schema["F_DEFAULT"];
      ScenarioDbRecord.TC_SCCELLS.idx_F_CODE = schema["F_CODE"];
      ScenarioDbRecord.TC_SCCELLS.idx_F_FLAGS = schema["F_FLAGS"];
      ScenarioDbRecord.TC_SCCELLS.idx_F_FLTENTITY = schema["F_FLTENTITY"];
    }
  }

  public class TC_SCNAMECOL
  {
    public const string TableName = "TC_SCNAMECOL";
    private const string F_SCEN = "F_SCEN";
    private const string F_NCOL = "F_NCOL";
    private const string F_WIDTH = "F_WIDTH";
    private const string F_NAME = "F_NAME";
    private static int idx_F_SCEN;
    private static int idx_F_NCOL;
    private static int idx_F_WIDTH;
    private static int idx_F_NAME;
    public readonly int Scen;
    public readonly int NCol;
    public readonly int Width;
    public readonly string Name;

    public TC_SCNAMECOL(IDataReader reader)
    {
      this.Scen = reader.IsDBNull(ScenarioDbRecord.TC_SCNAMECOL.idx_F_SCEN) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCNAMECOL.idx_F_SCEN]);
      this.NCol = reader.IsDBNull(ScenarioDbRecord.TC_SCNAMECOL.idx_F_NCOL) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCNAMECOL.idx_F_NCOL]);
      this.Width = reader.IsDBNull(ScenarioDbRecord.TC_SCNAMECOL.idx_F_WIDTH) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCNAMECOL.idx_F_WIDTH]);
      this.Name = reader.IsDBNull(ScenarioDbRecord.TC_SCNAMECOL.idx_F_NAME) ? string.Empty : reader.GetString(ScenarioDbRecord.TC_SCNAMECOL.idx_F_NAME);
    }

    public static void ParseSchema(Dictionary<string, int> schema)
    {
      ScenarioDbRecord.TC_SCNAMECOL.idx_F_SCEN = schema["F_SCEN"];
      ScenarioDbRecord.TC_SCNAMECOL.idx_F_NCOL = schema["F_NCOL"];
      ScenarioDbRecord.TC_SCNAMECOL.idx_F_WIDTH = schema["F_WIDTH"];
      ScenarioDbRecord.TC_SCNAMECOL.idx_F_NAME = schema["F_NAME"];
    }
  }

  public class TC_SCNAMEROW
  {
    public const string TableName = "TC_SCNAMEROW";
    private const string F_KEY = "F_KEY";
    private const string F_SCEN = "F_SCEN";
    private const string F_NROW = "F_NROW";
    private const string F_NAME = "F_NAME";
    private const string F_FLAGS = "F_FLAGS";
    private static int idx_F_KEY;
    private static int idx_F_SCEN;
    private static int idx_F_NROW;
    private static int idx_F_NAME;
    private static int idx_F_FLAGS;
    public readonly int Key;
    public readonly int Scen;
    public readonly int NRow;
    public readonly string Name;
    public readonly int Flags;

    public TC_SCNAMEROW(IDataReader reader)
    {
      this.Key = reader.IsDBNull(ScenarioDbRecord.TC_SCNAMEROW.idx_F_KEY) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCNAMEROW.idx_F_KEY]);
      this.Scen = reader.IsDBNull(ScenarioDbRecord.TC_SCNAMEROW.idx_F_SCEN) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCNAMEROW.idx_F_SCEN]);
      this.NRow = reader.IsDBNull(ScenarioDbRecord.TC_SCNAMEROW.idx_F_NROW) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCNAMEROW.idx_F_NROW]);
      this.Name = reader.IsDBNull(ScenarioDbRecord.TC_SCNAMEROW.idx_F_NAME) ? string.Empty : reader.GetString(ScenarioDbRecord.TC_SCNAMEROW.idx_F_NAME);
      this.Flags = reader.IsDBNull(ScenarioDbRecord.TC_SCNAMEROW.idx_F_FLAGS) ? 0 : BasePumpHelper.ToInt32(reader[ScenarioDbRecord.TC_SCNAMEROW.idx_F_FLAGS]);
    }

    public static void ParseSchema(Dictionary<string, int> schema)
    {
      ScenarioDbRecord.TC_SCNAMEROW.idx_F_KEY = schema["F_KEY"];
      ScenarioDbRecord.TC_SCNAMEROW.idx_F_SCEN = schema["F_SCEN"];
      ScenarioDbRecord.TC_SCNAMEROW.idx_F_NROW = schema["F_NROW"];
      ScenarioDbRecord.TC_SCNAMEROW.idx_F_NAME = schema["F_NAME"];
      ScenarioDbRecord.TC_SCNAMEROW.idx_F_FLAGS = schema["F_FLAGS"];
    }
  }
}
