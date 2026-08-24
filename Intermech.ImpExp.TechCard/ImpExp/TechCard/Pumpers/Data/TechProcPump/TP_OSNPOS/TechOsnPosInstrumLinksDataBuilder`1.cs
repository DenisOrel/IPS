// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_OSNPOS.TechOsnPosInstrumLinksDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_OSNPOS;

internal class TechOsnPosInstrumLinksDataBuilder<T>(T pumper) : TechDataBuilder<T>(pumper) where T : TechPumpBase
{
  public override TechDataReaderInfo CreateDataReader(string dopType)
  {
    string tableName1 = this._pumper.TableName;
    if (string.IsNullOrEmpty(tableName1))
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage("Имя таблицы не найдено");
      return (TechDataReaderInfo) null;
    }
    string tableName2 = dopType != string.Empty ? $"{tableName1}_{dopType}" : tableName1;
    string str = string.Empty;
    if (this._pumper.LastObjID != 0L)
    {
      string format = " AND \r\n                                     {0} >= \r\n                                     (\r\n                                       SELECT \r\n                                         MAX({0})\r\n                                       FROM \r\n                                         {1}\r\n                                       WHERE\r\n                                         {2} > {3}  \r\n                                     )\r\n                                  ";
      str = string.Format(format, (object) "F_OPERKEY", (object) "TP_OSNPOS", (object) "F_KEY", (object) this._pumper.LastObjID) + string.Format(format, (object) "F_PEREHKEY", (object) "TP_OSNPOS", (object) "F_KEY", (object) this._pumper.LastObjID);
    }
    string pumpModeCond = this.GetPumpModeCond("", dopType);
    if (pumpModeCond != string.Empty)
      str = $"{str} AND {pumpModeCond}";
    string sqlText = string.Format("SELECT \r\n                                                 {0}, {1}, {2}, {3}, {4} \r\n                                               FROM \r\n                                                 {5} \r\n                                               WHERE \r\n                                                 {3} > 0\r\n                                                 {6}     \r\n                                               ORDER BY \r\n                                                 {2}, {1}, {3}, {4}", (object) "F_KEY", (object) "F_OPERKEY", (object) "F_PEREHKEY", (object) "F_POS", (object) "F_INSTRUM", (object) "TP_OSNPOS", (object) str);
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount($"SELECT \r\n                                                 COUNT(*) \r\n                                               FROM \r\n                                                 {"TP_OSNPOS"} \r\n                                               WHERE \r\n                                                 {"F_POS"} > 0 \r\n                                                 {str} ");
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText), tableName2, sqlRecordsCount);
  }
}
