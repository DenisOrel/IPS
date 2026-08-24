// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.AutoSel.AutoSelCondDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.AutoSel;

internal class AutoSelCondDataBuilder<T>(T pumper) : TechDataBuilder<T>(pumper) where T : TechPumpBase
{
  protected override string GetPumpModeCond(string condField, string dopType) => string.Empty;

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
    string sqlText1;
    string sqlText2;
    if (TechcardConsts.ConnectionManager.DataBaseType == "IntermechConnection.Oracle")
    {
      sqlText1 = string.Format("SELECT DISTINCT \r\n                                              a.* , \r\n                                              b.{0}, \r\n                                              b.{1} AS {1}1, \r\n                                              b.{2}, \r\n                                              b.{3} \r\n                                            FROM \r\n                                              {4} a, \r\n                                              {5} b \r\n                                            WHERE \r\n                                               a.{6} = b.{7}(+) \r\n                                               {8}\r\n                                            ORDER BY \r\n                                               a.{6} ", (object) "F_CTLKEY", (object) "F_LEVEL", (object) "F_AUTOSELECT", (object) "F_WORKTYPE", (object) "TC_OSNCOND", (object) "TC_OSNCOND_LINKS", (object) "F_KEY", (object) "f_osncond_key", (object) str2);
      if (str2 != string.Empty)
        str2 = " WHERE 1=1 " + str2;
      sqlText2 = $"SELECT COUNT(*) FROM {"TC_OSNCOND"} A {str2}";
    }
    else
    {
      if (str2 != string.Empty)
        str2 = " WHERE 1=1 " + str2;
      sqlText1 = string.Format("SELECT DISTINCT \r\n\t\t\t\t                              a.*, \r\n                                              b.{0}, \r\n                                              b.{1} as {1}1, \r\n                                              b.{2}, \r\n                                              b.{3} \r\n                                            FROM \r\n                                              {4} a left join {5} b \r\n                                              on (a.{6} = b.{7}) \r\n                                              {8}     \r\n                                            ORDER \r\n                                              BY a.{6}", (object) "F_CTLKEY", (object) "F_LEVEL", (object) "F_AUTOSELECT", (object) "F_WORKTYPE", (object) "TC_OSNCOND", (object) "TC_OSNCOND_LINKS", (object) "F_KEY", (object) "f_osncond_key", (object) str2);
      sqlText2 = $"SELECT count(*) from {"TC_OSNCOND"} A {str2}";
    }
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount(sqlText2);
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText1), string.Empty, sqlRecordsCount);
  }
}
