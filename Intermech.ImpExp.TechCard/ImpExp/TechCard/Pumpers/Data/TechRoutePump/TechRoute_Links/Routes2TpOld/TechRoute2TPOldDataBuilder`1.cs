// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2TpOld.TechRoute2TPOldDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2TpOld;

internal class TechRoute2TPOldDataBuilder<T>(T pumper) : TechDataBuilder<T>(pumper) where T : TechPumpBase
{
  protected override string GetPumpModeCond(string condField, string dopType)
  {
    string pumpModeCond1 = string.Empty;
    string pumpModeCond2 = TechDataBuilder<PumpClass>.GetPumpModeCond("routes.F_KEY", 2);
    if (pumpModeCond2 != string.Empty)
      pumpModeCond1 = pumpModeCond2;
    string pumpModeCond3 = TechDataBuilder<PumpClass>.GetPumpModeCond("links.F_TP_VER_KEY", -2);
    if (pumpModeCond3 != string.Empty)
      pumpModeCond1 = pumpModeCond1 != string.Empty ? $"{pumpModeCond1} AND {pumpModeCond3}" : pumpModeCond3;
    if (pumpModeCond1 != string.Empty)
      pumpModeCond1 = " AND " + pumpModeCond1;
    return pumpModeCond1;
  }

  public override TechDataReaderInfo CreateDataReader(string dopType)
  {
    string str1 = string.Empty;
    if (this._pumper.LastObjID != 0L)
      str1 = $" AND {"F_KEY"} >= {this._pumper.LastObjID}";
    string pumpModeCond = this.GetPumpModeCond(string.Empty, dopType);
    string str2 = $"SELECT DISTINCT links.F_KEY, routes.F_KEY  AS {TechDbConsts.ROUTE_KEY}, links.F_TP_VER_KEY AS {TechDbConsts.TP_KEY} FROM TC_NROUTES routes, TC_NROUTE_TPLINK links WHERE ((routes.F_TMP_OBRABOTKI = links.F_TEMPLATE_ID) or (routes.F_TMP_SBORKI = links.F_TEMPLATE_ID)) {str1}{pumpModeCond}";
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount($"SELECT COUNT(*) from ({str2}) tmp");
    string sqlText = str2 + " ORDER BY F_KEY";
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText), string.Empty, sqlRecordsCount);
  }
}
