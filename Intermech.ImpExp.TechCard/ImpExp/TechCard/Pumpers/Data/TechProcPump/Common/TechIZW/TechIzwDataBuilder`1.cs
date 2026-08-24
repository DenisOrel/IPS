// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Common.TechIZW.TechIzwDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Common.TechIZW;

internal class TechIzwDataBuilder<T>(T pumper) : TechDataBuilderSimple<T>(pumper) where T : TechPumpBase
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
    if (!dopType.Equals(string.Empty))
      return base.CreateDataReader(dopType);
    string str2 = string.Empty;
    if (this._pumper.LastObjID != 0L)
      str2 = $" AND A.{"F_KEY"} > {this._pumper.LastObjID}";
    string pumpModeCond = this.GetPumpModeCond("A.F_KEY", dopType);
    if (pumpModeCond != string.Empty)
      str2 = $"{str2} AND {pumpModeCond}";
    string sqlText = string.Format("SELECT\r\n                                                     b.{2},\r\n                                                     b.{3}, \r\n\t\t\t\t                                     a.{9}, \r\n\t\t\t\t                                     a.{10}, \r\n\t\t\t\t                                     a.{11}, \r\n\t\t\t\t                                     a.{12}, \r\n\t\t\t\t                                     a.{13}, \r\n\t\t\t\t                                     a.{14}, \r\n                                                     a.{15}, \r\n                                                     b.{4}, \r\n                                                     b.{5}, \r\n                                                     b.{6}, \r\n                                                     b.{7},\r\n                                                     b.{16}    \r\n                                                   FROM \r\n                                                     {0} a, \r\n                                                     {1} b \r\n                                                   WHERE \r\n                                                     a.{2} = b.{3} \r\n                                                     {8}\r\n                                                   ORDER BY\r\n                                                     a.{2}", (object) "TC_ARCDOCS", (object) "TC_IZW", (object) "F_KEY", (object) "F_DOCTCKEY", (object) "F_OBJKEYOLD", (object) "F_OBJKEYNEW", (object) "F_RECORDID", (object) "F_ORDER", (object) str2, (object) "F_DESIGNATION", (object) "F_NAME", (object) "F_DOCID", (object) "F_KIND", (object) "F_PRODUCTION", (object) "F_VERSION", (object) "F_FLAGS", (object) "F_USER");
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount($"SELECT \r\n                                                     count(*) \r\n                                                   FROM \r\n                                                     {"TC_ARCDOCS"} a, \r\n                                                     {"TC_IZW"} b \r\n                                                   WHERE \r\n                                                     a.{"F_KEY"} = b.{"F_DOCTCKEY"}\r\n                                                     {str2}");
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText), string.Empty, sqlRecordsCount);
  }
}
