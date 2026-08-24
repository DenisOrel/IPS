// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_SKETCH.TechSketchDwgDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_SKETCH;

internal class TechSketchDwgDataBuilder<T>(T pumper) : TechDataBuilderSimple<T>(pumper) where T : TechPumpBase
{
  protected override string GetPumpModeCond(string condField, string dopType)
  {
    return TechDataBuilder<PumpClass>.GetPumpModeCond(dopType == "" ? "F_DOCTCKEY" : "F_TCKEY", -2);
  }

  public override TechDataReaderInfo CreateDataReader(string dopType)
  {
    if (string.IsNullOrEmpty(this._pumper.TableName))
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage("Имя таблицы не найдено");
      return (TechDataReaderInfo) null;
    }
    if (dopType != string.Empty)
      return base.CreateDataReader(dopType);
    string str = string.Empty;
    if (this._pumper.LastObjID != 0L)
      str = $" AND A.{"F_KEY"} > {this._pumper.LastObjID}";
    string pumpModeCond = this.GetPumpModeCond(" A.F_DOCTCKEY", string.Empty);
    if (pumpModeCond != string.Empty)
      str = $"{str} AND {pumpModeCond}";
    string sqlText1;
    string sqlText2;
    if (TechcardConsts.ConnectionManager.DataBaseType == "IntermechConnection.Oracle")
    {
      sqlText1 = string.Format("SELECT \r\n\t\t\t\t                              A.*, \r\n                                              A.{4} AS {10},  \r\n                                              B.{0}, \r\n                                              C.{1}, \r\n                                              (SELECT \r\n                                                 COUNT({4}) \r\n                                               FROM \r\n                                                 {2} D\r\n                                               WHERE \r\n                                                 D.{4}  = A.{4} AND\r\n                                                 D.{11} = 1\r\n                                              ) as {6} \r\n                                            FROM \r\n                                              {2} A, \r\n                                              {3} B,\r\n                                              {7} C\r\n                                            WHERE \r\n                                              A.{4}  = B.{5}(+) AND\r\n                                              B.{10} = C.{5}(+) AND\r\n                                              A.{11} = 1         \r\n                                              {8} \r\n                                            ORDER BY\r\n                                              A.{5}, A.{9}", (object) "F_VERSION", (object) "F_DOCID", (object) "TP_SKETCH", (object) "TP_VERSIONS", (object) "F_DOCTCKEY", (object) "F_KEY", (object) "F_COUNT", (object) "TC_ARCDOCS", (object) str, (object) "F_ORDER", (object) "F_TCKEY", (object) "F_TYPE");
      if (str != string.Empty)
        str = " WHERE 1=1 " + str;
      sqlText2 = $"SELECT COUNT(*) FROM {"TP_SKETCH"} A {str}";
    }
    else
    {
      sqlText1 = string.Format("SELECT \r\n                                              A.*  , \r\n                                              A.{4} AS {10},\r\n                                              B.{0}, \r\n                                              C.{1}, \r\n                                              (SELECT \r\n                                                 COUNT({4}) \r\n                                               FROM \r\n                                                 {2} D \r\n                                               WHERE \r\n                                                 D.{4}  = A.{4} AND\r\n                                                 D.{11} = 1 \r\n                                               ) as {6} \r\n                                            FROM \r\n                                              {2} A \r\n                                              LEFT JOIN {3} B \r\n                                              ON A.{4} = B.{5}\r\n                                              LEFT JOIN {7} C\r\n                                              ON B.{10} = C.{5}\r\n                                            WHERE \r\n                                              A.{11} = 1\r\n                                              {8}\r\n                                            ORDER BY\r\n                                              A.{5}, A.{9}", (object) "F_VERSION", (object) "F_DOCID", (object) "TP_SKETCH", (object) "TP_VERSIONS", (object) "F_DOCTCKEY", (object) "F_KEY", (object) "F_COUNT", (object) "TC_ARCDOCS", (object) str, (object) "F_ORDER", (object) "F_TCKEY", (object) "F_TYPE");
      if (str != string.Empty)
        str = " WHERE 1=1 " + str;
      sqlText2 = $"SELECT COUNT(*) FROM {"TP_SKETCH"} A {str}";
    }
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount(sqlText2);
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText1), string.Empty, sqlRecordsCount);
  }
}
