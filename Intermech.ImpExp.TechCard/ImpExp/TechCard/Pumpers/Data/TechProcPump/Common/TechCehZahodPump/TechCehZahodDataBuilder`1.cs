// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Common.TechCehZahodPump.TechCehZahodDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Common.TechCehZahodPump;

internal class TechCehZahodDataBuilder<T>(T pumper) : TechDataBuilder<T>(pumper) where T : TechPumpBase
{
  protected override string GetPumpModeCond(string condField, string dopType)
  {
    return TechDataBuilder<PumpClass>.GetPumpModeCond(dopType == "" ? "F_DOCTCKEY" : "F_TCKEY", -2);
  }

  public override TechDataReaderInfo CreateDataReader(string dopType)
  {
    string tableName = this._pumper.TableName;
    if (string.IsNullOrEmpty(tableName))
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage("Имя таблицы не найдено");
      return (TechDataReaderInfo) null;
    }
    string str1 = dopType == string.Empty ? "F_DOCTCKEY" : "DATA.F_TCKEY";
    string str2 = string.Empty;
    if (this._pumper.LastObjID != 0L)
      str2 = string.Format(" AND \r\n                                     {4} >= \r\n                                     (\r\n                                       SELECT \r\n                                         MAX({0})\r\n                                       FROM \r\n                                         {1}\r\n                                       WHERE\r\n                                         {2} = {3}  \r\n                                     )\r\n                                  ", (object) "F_DOCTCKEY", (object) "TP_OPER", (object) "F_KEY", (object) this._pumper.LastObjID, (object) str1);
    string pumpModeCond = this.GetPumpModeCond("", dopType);
    if (pumpModeCond != string.Empty)
      str2 = $"{str2} AND {pumpModeCond}";
    string empty = string.Empty;
    $"SELECT \r\n                                                 COUNT(*) \r\n                                               FROM \r\n                                                 {"TP_OPER"} \r\n                                               WHERE \r\n                                                 {"F_KEY"} != 0\r\n                                                 {str2} ";
    if (dopType == string.Empty)
    {
      string sqlText = string.Format("SELECT \r\n                                            *\r\n                                        FROM \r\n                                            {0} \r\n                                        WHERE \r\n                                            {1} != 0 \r\n                                            {4}     \r\n                                        ORDER BY \r\n                                            {2}, {3}", (object) "TP_OPER", (object) "F_KEY", (object) "F_DOCTCKEY", (object) "F_ORDER", (object) str2);
      int sqlRecordsCount = this._pumper.GetSqlRecordsCount($"SELECT \r\n                                                 COUNT(*) \r\n                                               FROM \r\n                                                 {"TP_OPER"} \r\n                                               WHERE \r\n                                                 {"F_KEY"} != 0\r\n                                                 {str2} ");
      return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText), tableName, sqlRecordsCount);
    }
    string str3 = dopType != string.Empty ? $"{tableName}_{dopType}" : tableName;
    string sqlText1 = string.Format("SELECT \r\n                                            DATA.*\r\n                                        FROM \r\n                                            {0} DATA\r\n                                            LEFT JOIN    \r\n                                            {1} OPER\r\n                                            ON DATA.F_PARENTKEY = OPER.F_KEY\r\n                                        WHERE \r\n                                            DATA.{2} != 0 \r\n                                            {5}     \r\n                                        ORDER BY \r\n                                            DATA.{3}, {4}", (object) str3, (object) "TP_OPER", (object) "F_KEY", (object) "F_TCKEY", dopType != "D" ? (object) "F_ORDER" : (object) "F_ORDER, F_ENTITY, F_ROW", (object) str2);
    int sqlRecordsCount1 = this._pumper.GetSqlRecordsCount($"SELECT \r\n                                        COUNT(*) \r\n                                      FROM \r\n                                        {str3} DATA\r\n                                      WHERE \r\n                                        {"F_KEY"} != 0\r\n                                        {str2} ");
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText1), tableName, sqlRecordsCount1);
  }
}
