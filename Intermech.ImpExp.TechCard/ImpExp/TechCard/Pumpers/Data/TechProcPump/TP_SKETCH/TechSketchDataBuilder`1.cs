// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_SKETCH.TechSketchDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_SKETCH;

internal class TechSketchDataBuilder<T>(T pumper) : TechDataBuilderSimple<T>(pumper) where T : TechPumpBase
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
    string sqlText1 = $"SELECT COUNT(*) FROM {tableName2} A {str}";
    string sqlText2 = string.Format("SELECT  \r\n                                                     A.*, \r\n                                                     B.{0}, \r\n                                                     B.{1} \r\n                                                   FROM \r\n                                                     {2} A LEFT JOIN {3} B \r\n                                                     ON A.{4} = B.{5}                                                     \r\n                                                     {6}\r\n                                                   ORDER BY \r\n                                                     A.{4}", (object) "F_SOURCE", (object) "F_BLOB", (object) tableName2, (object) "IM_BLOBS", (object) "F_PICTUREKEY", (object) "F_KEY", (object) str);
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount(sqlText1);
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText2), tableName2, sqlRecordsCount);
  }
}
