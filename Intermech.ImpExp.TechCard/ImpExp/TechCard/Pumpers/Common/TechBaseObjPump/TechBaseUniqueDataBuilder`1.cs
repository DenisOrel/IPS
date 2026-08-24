// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.TechBaseUniqueDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

internal class TechBaseUniqueDataBuilder<T>(T pumper) : TechDataBuilder<T>(pumper) where T : TechBaseUniquePump
{
  protected override string GetPumpModeCond(string condField, string dopType)
  {
    return this.PumpModeCondFunc != null ? this.PumpModeCondFunc(condField, dopType) : TechDataBuilder<PumpClass>.GetPumpModeCond(dopType == "" ? "F_DOCTCKEY" : "F_TCKEY", -2);
  }

  public override TechDataReaderInfo CreateDataReader(string dopType)
  {
    string tableName1 = this._pumper.TableName;
    if (string.IsNullOrEmpty(tableName1))
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage("Имя таблицы не найдено");
      return (TechDataReaderInfo) null;
    }
    string tableName2 = dopType != string.Empty ? $"{tableName1}_{dopType}" : tableName1;
    string str1 = string.Empty;
    string sqlText1;
    string sqlText2;
    if (dopType == "")
    {
      if (this._pumper.LastObjID != 0L)
        str1 = $" WHERE {this._pumper.FldRecKey} >= {this._pumper.LastRecKey} ";
      string pumpModeCond = this.GetPumpModeCond(string.Empty, dopType);
      if (pumpModeCond != string.Empty)
        str1 = !(str1 != string.Empty) ? " WHERE " + pumpModeCond : $"{str1} AND {pumpModeCond}";
      sqlText1 = $"SELECT \r\n                                                          * \r\n\t\t\t\t\t                                    FROM \r\n                                                          {this._pumper.TableName} {str1} \r\n                                                        ORDER BY \r\n                                                          {this._pumper.FldRecKey}, {this._pumper.FldTblKey}, {"F_KEY"}";
      sqlText2 = $"SELECT COUNT(*) FROM {this._pumper.TableName} {str1}";
    }
    else
    {
      string str2 = string.Empty;
      if (dopType == "D")
        str2 = ", a.F_ENTITY, a.F_ROW";
      if (this._pumper.LastObjID != 0L)
        str1 = $" AND B.{this._pumper.FldRecKey} >= {this._pumper.LastRecKey} ";
      string pumpModeCond = this.GetPumpModeCond(string.Empty, dopType);
      if (pumpModeCond != string.Empty)
        str1 = $"{str1} AND {pumpModeCond}";
      sqlText1 = string.Format("SELECT DISTINCT\r\n                                                  A.*,\r\n                                                  B.{5}, B.{6}\r\n                                                FROM \r\n                                                  {0} A,\r\n                                                  {1} B \r\n                                                WHERE \r\n                                                  A.{2} = b.{3}\r\n                                                  {4} \r\n                                                ORDER BY \r\n                                                  B.{5}, B.{6}, A.{2} {7}", (object) tableName2, (object) this._pumper.TableName, (object) "F_PARENTKEY", (object) "F_KEY", (object) str1, (object) this._pumper.FldRecKey, (object) this._pumper.FldTblKey, (object) str2);
      sqlText2 = $"SELECT \r\n                                                      COUNT(*) \r\n                                                    FROM \r\n                                                      {tableName2} A, \r\n                                                      {this._pumper.TableName} B \r\n                                                    WHERE \r\n                                                      A.{"F_PARENTKEY"} = b.{"F_KEY"} \r\n                                                      {str1}";
    }
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount(sqlText2);
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText1), tableName2, sqlRecordsCount);
  }
}
