// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT.TechMaterialDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_MAT;

internal class TechMaterialDataBuilder<T>(T pumper) : TechBaseUniqueDataBuilder<T>(pumper) where T : TechBaseUniquePump
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
    if (!dopType.Equals(string.Empty))
      return base.CreateDataReader(dopType);
    string str = string.Empty;
    if (this._pumper.LastObjID != 0L)
      str = $" WHERE A.{"F_KEY"} > {this._pumper.LastObjID}";
    string pumpModeCond = this.GetPumpModeCond("", dopType);
    if (pumpModeCond != string.Empty)
      str = str != string.Empty ? $"{str} AND {pumpModeCond}" : " WHERE " + pumpModeCond;
    string sqlText1 = $" SELECT COUNT(*) FROM {tableName2} A {str}";
    string sqlText2 = string.Format("SELECT DISTINCT \r\n\t\t\t\t                                     A.*  , \r\n                                                     B.{0} as {0}_1, \r\n                                                     B.{1} as {1}_1,\r\n                                                     D.{0} as {0}_2\r\n                                                   FROM \r\n                                                     {2} A \r\n                                                     LEFT JOIN {3} B ON \r\n                                                     A.{4} = B.{5} AND B.{6} = {7} \r\n                                                     AND \r\n                                                     ( A.F_DOCTCKEY > 0 AND A.F_DOCTCKEY = B.F_TCKEY \r\n                                                       OR \r\n                                                       A.F_SETKEY   > 0 AND A.F_SETKEY = B.F_SETKEY \r\n                                                     )\r\n\r\n                                                     LEFT JOIN \r\n                                                       (SELECT \r\n                                                          DISTINCT {0} \r\n                                                        FROM   \r\n                                                          {3} C   \r\n                                                        WHERE     \r\n                                                          C.{1} = {7} AND C.{6} = 24\r\n                                                       ) D\r\n                                                     ON          \r\n                                                       A.{4} = D.{0} \r\n                                                     {8}\r\n                                                   ORDER BY\r\n                                                     A.{4}   \r\n                                                   ", (object) "F_PARENTKEY", (object) "F_PARENTTYPE", (object) tableName2, (object) "TP_MAT_LINKS", (object) "F_KEY", (object) "F_CHILDKEY", (object) "F_CHILDTYPE", (object) this._pumper.RecTypeID, (object) str);
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount(sqlText1);
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText2), tableName2, sqlRecordsCount);
  }
}
