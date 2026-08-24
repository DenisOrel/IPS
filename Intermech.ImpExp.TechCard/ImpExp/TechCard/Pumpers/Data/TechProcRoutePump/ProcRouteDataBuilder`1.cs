// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.ProcRouteDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump;

internal class ProcRouteDataBuilder<T>(T pumper) : TechDataBuilderSimple<T>(pumper) where T : TechPumpBase
{
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
    if (TechSettingsHelper.PumpDataType == TechPumpDataType.None)
    {
      str2 = " WHERE  1 = 2 ";
    }
    else
    {
      if (this._pumper.LastObjID != 0L)
        str2 = $" WHERE A.{"F_KEY"} >= {this._pumper.LastObjID}";
      if (TechSettingsHelper.PumpMode != TechPumpMode.tpmAll)
      {
        string pumpModeCond = this.GetPumpModeCond("A.F_ART_TCKEY", dopType);
        if (pumpModeCond != string.Empty)
          str2 = str2 != string.Empty ? " AND " + pumpModeCond : " WHERE " + pumpModeCond;
      }
    }
    string sqlText = string.Format(" SELECT   a.*,    b.{0}, b.{1},   b.{2} as {3},   c.{2} as {4},   d.{2} as {5}   from              {6} a left join {7} b    on a.{8} = b.{9}      left join {7} c       on a.{10} <> 0 AND       a.{10} = c.{9}     left join {7} d       on a.{11} <> 0 AND       a.{11} = d.{9}     {12}                  order by a.{9}     ", (object) "F_DESIGNATION", (object) "F_NAME", (object) "F_VER", (object) "F_ART_VER", (object) "F_PROJ_VER", (object) "F_ZAK_VER", (object) "TC_OBJ2LINK", (object) "TC_ARCARTS", (object) "F_ART_TCKEY", (object) "F_KEY", (object) "F_PROJ_TCKEY", (object) "F_ZAK_TCKEY", (object) str2);
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount($"select count(*) from {"TC_OBJ2LINK"} A {str2}");
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText), string.Empty, sqlRecordsCount);
  }
}
