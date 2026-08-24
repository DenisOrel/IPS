// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_ZAG.TechZagDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.TechProcPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_ZAG;

internal class TechZagDataBuilder<T>(T pumper) : TechDataBuilderSimple<T>(pumper) where T : TechPumpBase
{
  private string GetCatalogTableNameByReferenceEntityCode(string entityCode)
  {
    string empty = string.Empty;
    EntityTypeRec recByType = TechPumpData.EntTypeList.GetRecByType(23);
    if (recByType == null || recByType.CodeList == null || !recByType.CodeList.ContainsKey(entityCode))
      return empty;
    Entity code = recByType.CodeList[entityCode];
    int reference = code.EntityReference != null ? code.EntityReference.Reference : 0;
    return reference < 1 ? empty : TechPumpData.Tables.ImTablesData.GetTableName(reference);
  }

  private TechDataReaderInfo GetArtTechDataReader(string dopType)
  {
    string tableName = $"{"TC_ARCARTS"}_{dopType}";
    string str = string.Empty;
    if (this._pumper.LastObjID != 0L)
      str = $" AND A.{"F_KEY"} > {this._pumper.LastObjID}";
    string pumpModeCond = TechDataBuilder<PumpClass>.GetPumpModeCond("F_OBJ_KEY", 3);
    if (pumpModeCond != string.Empty)
      str = $"{str} AND {pumpModeCond}";
    string sqlText = string.Format(" SELECT DISTINCT \t\t\t                                         \r\n                                                     a.{18} as {1}, \r\n                                                     b.{6} ,\r\n                                                     b.{7} ,\r\n                                                     b.{8} ,\r\n                                                     b.{9} ,\r\n                                                     b.{10},\r\n                                                     b.{11},\r\n                                                     b.{12},\r\n                                                     b.{13},\r\n                                                     b.{14},\r\n                                                     b.{15},\r\n                                                     b.{16}, \r\n                                                     b.{0}  \r\n                                                   FROM \r\n                                                     {3} a,    \r\n                                                     {2}_{4} b \r\n                                                   WHERE\r\n                                                     b.{1}  = a.{5} and\r\n                                                     a.{19} = {20}\r\n                                                     {17} \r\n                                                   ORDER BY \r\n                                                     a.{18} ", (object) "F_KEY", (object) "F_PARENTKEY", (object) "TC_ARCARTS", (object) "TC_OBJ2LINK", (object) dopType, (object) "F_ART_TCKEY", (object) "F_ROW", (object) "F1", (object) "F2", (object) "F3", (object) "F4", (object) "F5", (object) "F6", (object) "F7", (object) "F8", (object) "F9", (object) "F10", (object) str, (object) "F_OBJ_KEY", (object) "F_OBJ_TYPE", (object) 3);
    if (dopType == "D")
      sqlText = string.Format("SELECT DISTINCT \r\n                                                b.{0}, \r\n                                                a.{10} as {1}, \r\n                                                b.{6},\r\n                                                b.{7},\r\n                                                b.{8} \r\n                                              from \r\n                                                {2} a,\r\n                                                {3}_{4} b \r\n                                              WHERE\r\n                                                a.{11} = {12} AND\r\n                                                b.{1}  = a.{5}  \r\n                                                {9} \r\n                                              ORDER BY \r\n                                                a.{10}, b.{7}, b.{6}", (object) "F_KEY", (object) "F_PARENTKEY", (object) "TC_OBJ2LINK", (object) "TC_ARCARTS", (object) dopType, (object) "F_ART_TCKEY", (object) "F_ENTITY", (object) "F_ROW", (object) "F_VALUE", (object) str, (object) "F_OBJ_KEY", (object) "F_OBJ_TYPE", (object) 3);
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount(string.Format("SELECT \r\n\t\t\t                                       count(*) \r\n                                                 FROM\r\n                                                   {0} a,\r\n                                                   {1}_{2} b \r\n                                                 WHERE\r\n                                                   a.{5} = {6} AND\r\n                                                   b.{4} = a.{3}\r\n                                                   {7} ", (object) "TC_OBJ2LINK", (object) "TC_ARCARTS", (object) dopType, (object) "F_ART_TCKEY", (object) "F_PARENTKEY", (object) "F_OBJ_TYPE", (object) 3, (object) str));
    return new TechDataReaderInfo("S", dopType, this._pumper.GetCustomDataReader(sqlText), tableName, sqlRecordsCount);
  }

  private TechDataReaderInfo GetMainTechDataReader(string dopType)
  {
    if (!dopType.Equals(string.Empty))
      return base.CreateDataReader(dopType);
    string str1 = string.Empty;
    string str2 = string.Empty;
    if (this._pumper.LastObjID != 0L)
    {
      string str3 = $" A.{"F_KEY"} > {this._pumper.LastObjID}";
      str1 = " AND " + str3;
      str2 = " WHERE " + str3;
    }
    string pumpModeCond = TechDataBuilder<PumpClass>.GetPumpModeCond("A.F_KEY", 3);
    if (pumpModeCond != string.Empty)
    {
      string str4 = $"{str1} AND {str1}";
      str2 = str2 == string.Empty ? " WHERE " + pumpModeCond : $"{str2} AND {str4}";
    }
    string sqlText1 = $"SELECT COUNT(*) FROM {"TP_ZAG"} A {str2} ";
    string referenceEntityCode = this.GetCatalogTableNameByReferenceEntityCode("SRT1");
    if (referenceEntityCode.Equals(string.Empty))
      return base.CreateDataReader(dopType);
    string sqlText2 = string.Format("SELECT DISTINCT\r\n                                                A.{4} , \r\n                                                ( \r\n                                                   SELECT MAX(C.{26}) \r\n                                                   FROM {27} c\r\n                                                   WHERE \r\n                                                   a.{4}  = c.{28} and\r\n                                                   c.{29} = {30}\r\n                                                ) {5} , A.{6} , A.{7} , A.{8} , A.{9} , \r\n                                                A.{10}, A.{11}, A.{12}, A.{13}, A.{14}, A.{15}, A.{16}, A.{17}, A.{18}, A.{19}, \r\n                                                A.{20}, A.{21}, A.{22}, A.{23}, A.{24}, A.{31},\r\n                                                B.{0} \r\n                                            FROM \r\n                                                {1} A left join {2}_rec b \r\n                                                on (A.{3} = B.{4})                                                 \r\n                                            {25} \r\n                                            ORDER BY A.{4}", (object) "F_LEVEL", (object) "TP_ZAG", (object) referenceEntityCode, (object) "F_RECKEY", (object) "F_KEY", (object) "F_ARTTCKEY", (object) "F_RECKEY", (object) "F_TBLKEY", (object) "F_ORDER", (object) "F_FLAGS", (object) "F_DATE", (object) "F_VAR", (object) "F_NAME", (object) "F_ZAGARTKEY", (object) "F_CTLKEY", (object) "F_VERSION", (object) "F_PARENTKEY", (object) "F_STATUS", (object) "F_USERID", (object) "F_PRODUCTION", (object) "F_OWNER", (object) "F_DATA_AKTUAL", (object) "F_USER_CREATOR", (object) "F_OSN_VVODA", (object) "F_DESCR", (object) str2, (object) "F_ART_TCKEY", (object) "TC_OBJ2LINK", (object) "F_OBJ_KEY", (object) "F_OBJ_TYPE", (object) 3, (object) "F_GROUPZAG_KEY");
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount(sqlText1);
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText2), string.Empty, sqlRecordsCount);
  }

  public override TechDataReaderInfo CreateDataReader(string dopType)
  {
    switch (dopType)
    {
      case "S_I":
        return this.GetArtTechDataReader("I");
      case "S_F":
        return this.GetArtTechDataReader("F");
      case "S_S":
        return this.GetArtTechDataReader("S");
      case "S_D":
        return this.GetArtTechDataReader("D");
      default:
        return this.GetMainTechDataReader(dopType);
    }
  }
}
