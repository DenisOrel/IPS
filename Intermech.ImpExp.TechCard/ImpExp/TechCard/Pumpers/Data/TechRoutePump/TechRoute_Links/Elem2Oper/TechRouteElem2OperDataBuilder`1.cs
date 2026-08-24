// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Elem2Oper.TechRouteElem2OperDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Elem2Oper;

internal class TechRouteElem2OperDataBuilder<T>(T pumper) : TechDataBuilderSimple<T>(pumper) where T : TechPumpBase
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
    string str2 = string.Empty;
    if (this._pumper.LastObjID != 0L)
      str2 = $" AND A.{"F_KEY"} >= {this._pumper.LastObjID}";
    string str3 = TechDataBuilder<PumpClass>.GetPumpModeCond("B.F_STRING_ID", 122);
    if (str3 != string.Empty)
      str3 = " AND " + str3;
    string sqlText = $"SELECT \r\n\t\t\t                      A.F_KEY,   A.F_OPER_KEY,   B.F_TP_VER_KEY,   B.F_STRING_ID FROM TC_NROUTE_TPOPER A, TC_NROUTE_TPLINK B  WHERE A.F_TPLINK_ID = B.F_KEY{str2}{str3} ORDER BY A.F_KEY";
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount($"SELECT \r\n                                   COUNT(*) \r\n                                 FROM TC_NROUTE_TPOPER A, TC_NROUTE_TPLINK B   WHERE A.F_TPLINK_ID = B.F_KEY{str2}{str3}");
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText), string.Empty, sqlRecordsCount);
  }
}
