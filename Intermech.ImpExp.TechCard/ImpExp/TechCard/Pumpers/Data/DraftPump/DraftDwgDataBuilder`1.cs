// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.DraftPump.DraftDwgDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.DraftPump;

internal class DraftDwgDataBuilder<T>(T pumper) : TechDataBuilder<T>(pumper) where T : TechPumpBase
{
  protected override string GetPumpModeCond(string condField, string dopType)
  {
    return TechDataBuilder<PumpClass>.GetPumpModeCond(condField, -2);
  }

  public override TechDataReaderInfo CreateDataReader(string dopType)
  {
    string tableName = this._pumper.TableName;
    if (string.IsNullOrEmpty(tableName))
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage("Имя таблицы не найдено");
      return (TechDataReaderInfo) null;
    }
    if (dopType != string.Empty)
    {
      string str1 = $"{tableName}_{dopType}";
    }
    string str2 = string.Empty;
    if (this._pumper.LastObjID != 0L)
      str2 = $" AND A.{"F_KEY"} > {this._pumper.LastObjID}";
    string pumpModeCond = this.GetPumpModeCond(" A.F_TCKEY", string.Empty);
    if (pumpModeCond != string.Empty)
      str2 = $"{str2} AND {pumpModeCond}";
    string sqlText1;
    string sqlText2;
    if (TechcardConsts.ConnectionManager.DataBaseType == "IntermechConnection.Oracle")
    {
      sqlText1 = string.Format("SELECT \r\n\t\t\t\t                              A.*, \r\n                                              B.{0}, \r\n                                              C.{1}, \r\n                                              (SELECT \r\n                                                 COUNT({4}) \r\n                                               FROM \r\n                                                 {2} D\r\n                                               WHERE \r\n                                                 D.{4} = A.{4}\r\n                                              ) as {6} \r\n                                            FROM \r\n                                              {2} A, \r\n                                              {3} B,\r\n                                              {7} C\r\n                                            WHERE \r\n                                              A.{4} = B.{5}(+) AND\r\n                                              B.{4} = C.{5}(+)\r\n                                              {8} \r\n                                            ORDER BY\r\n                                              A.{5}, A.{9}, A.{10}", (object) "F_VERSION", (object) "F_DOCID", (object) "TP_PIC", (object) "TP_VERSIONS", (object) "F_TCKEY", (object) "F_KEY", (object) "F_COUNT", (object) "TC_ARCDOCS", (object) str2, (object) "F_ORDER", (object) "F_NUMBER");
      if (str2 != string.Empty)
        str2 = " WHERE 1=1 " + str2;
      sqlText2 = $"SELECT COUNT(*) FROM {"TP_PIC"} A {str2}";
    }
    else
    {
      if (str2 != string.Empty)
        str2 = " WHERE 1=1 " + str2;
      sqlText1 = string.Format("SELECT \r\n                                              A.*  , \r\n                                              B.{0}, \r\n                                              C.{1}, \r\n                                              (SELECT \r\n                                                 COUNT({4}) \r\n                                               FROM \r\n                                                 {2} D \r\n                                               WHERE \r\n                                                 D.{4} = A.{4}\r\n                                               ) as {6} \r\n                                            FROM \r\n                                              {2} A \r\n                                              LEFT JOIN {3} B \r\n                                              ON A.{4} = B.{5}\r\n                                              LEFT JOIN {7} C\r\n                                              ON B.{4} = C.{5}\r\n                                              {8}\r\n                                            ORDER BY\r\n                                              A.{5}, A.{9}, A.{10}", (object) "F_VERSION", (object) "F_DOCID", (object) "TP_PIC", (object) "TP_VERSIONS", (object) "F_TCKEY", (object) "F_KEY", (object) "F_COUNT", (object) "TC_ARCDOCS", (object) str2, (object) "F_ORDER", (object) "F_NUMBER");
      sqlText2 = $"SELECT COUNT(*) FROM {"TP_PIC"} A {str2}";
    }
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount(sqlText2);
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText1), string.Empty, sqlRecordsCount);
  }
}
