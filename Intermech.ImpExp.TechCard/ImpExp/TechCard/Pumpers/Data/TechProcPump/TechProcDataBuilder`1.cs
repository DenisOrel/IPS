// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TechProcDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump;

internal class TechProcDataBuilder<T>(T pumper) : TechDataBuilderSimple<T>(pumper) where T : TechPumpBase
{
  internal void GetTechDataReaderSqlCommands(out string sqlCommon, out string sqlCount)
  {
    string str1 = string.Empty;
    if (this._pumper.LastObjID != 0L)
      str1 = $" AND A.{"F_KEY"} > {this._pumper.LastObjID}";
    string pumpModeCond = TechDataBuilder<PumpClass>.GetPumpModeCond("A.F_KEY", -2);
    if (pumpModeCond != string.Empty)
      str1 = $"{str1} AND {pumpModeCond}";
    string str2 = " count(*) ";
    string str3 = "  a.*,  b.F_KEY F_KEY1,  b.F_DESIGNATION as F_DESIGNATION1, b.F_NAME as F_NAME1, F_PRODUCTION,  b.F_KIND,  b.F_DOCID";
    string format = !(TechcardConsts.ConnectionManager.DataBaseType == "IntermechConnection.Oracle") ? $"select    {{0}}  from    TP_VERSIONS a left join  TC_ARCDOCS b  on (a.F_TCKEY = b.F_KEY)  where  ( b.F_KIND IN ( {TechProcDataBuilder<T>.GetTechProcKindSqlCond()})) {str1}" : $"select    {{0}}  from    TP_VERSIONS a, TC_ARCDOCS b  where ( a.F_TCKEY = b.F_KEY(+)) and ( b.F_KIND IN ({TechProcDataBuilder<T>.GetTechProcKindSqlCond()})) {str1}";
    sqlCommon = string.Format(format, (object) str3);
    sqlCount = string.Format(format, (object) str2);
  }

  protected override string GetPumpModeCond(string condField, string dopType)
  {
    return TechDataBuilder<PumpClass>.GetPumpModeCond(condField, -2);
  }

  public override TechDataReaderInfo CreateDataReader(string dopType)
  {
    if (!dopType.Equals(string.Empty))
      return base.CreateDataReader(dopType);
    string sqlCommon;
    string sqlCount;
    this.GetTechDataReaderSqlCommands(out sqlCommon, out sqlCount);
    string sqlText = sqlCommon + " order by a.F_KEY";
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount(sqlCount);
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText), string.Empty, sqlRecordsCount);
  }

  internal static string GetTechProcKindSqlCond()
  {
    return string.Join(",", 1.ToString(), 4.ToString(), 6.ToString(), 7.ToString());
  }
}
