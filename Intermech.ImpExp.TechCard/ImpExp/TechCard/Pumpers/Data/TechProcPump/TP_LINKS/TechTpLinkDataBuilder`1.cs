// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_LINKS.TechTpLinkDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_LINKS;

internal class TechTpLinkDataBuilder<T>(T pumper) : TechDataBuilder<T>(pumper) where T : TechPumpBase
{
  protected override string GetPumpModeCond(string condField, string dopType)
  {
    string pumpModeCond = string.Empty;
    string str = TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.TechProc) ? TechDataBuilder<PumpClass>.GetPumpModeCond("F_DOC_TCKEY", -2) : string.Empty;
    if (str != string.Empty)
      pumpModeCond = str;
    return pumpModeCond;
  }

  public override TechDataReaderInfo CreateDataReader(string dopType)
  {
    string tableName = this._pumper.TableName;
    if (string.IsNullOrEmpty(tableName))
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage("Имя таблицы не найдено");
      return (TechDataReaderInfo) null;
    }
    if (!string.IsNullOrEmpty(dopType))
      return (TechDataReaderInfo) null;
    string str = string.Empty;
    if (this._pumper.LastObjID != 0L)
      str = $" TP D2Z.{"F_KEY"} >= {this._pumper.LastObjID}";
    string pumpModeCond = this.GetPumpModeCond("", dopType);
    if (pumpModeCond != string.Empty)
      str = $"{str} AND {pumpModeCond}";
    string sqlText = string.Format("\r\n                                                SELECT \r\n                                                  TP.{1}, \r\n                                                  TP.F_DOC_TCKEY, \r\n                                                  TP.F_ART_KEY, \r\n                                                  TP.F_SOURCE_KEY, \r\n                                                  TP.F_SOURCE_TYPE, \r\n                                                  TP.F_TARGET_KEY,\r\n                                                  TP.{2},\r\n                                                  TP.F_TARGET_DOC_TCKEY,\r\n                                                  MIN(LINK.{3}) {5} \r\n                                                FROM \r\n                                                  {0} TP \r\n                                                  LEFT JOIN             \r\n                                                  {4} LINK\r\n                                                  ON\r\n                                                  TP.F_TARGET_DOC_TCKEY = LINK.F_OBJ_KEY AND\r\n                                                  LINK.F_OBJ_TYPE = 1\r\n                                                WHERE\r\n                                                  TP.{1} != 0\r\n                                                  {6}\r\n                                                GROUP BY \r\n                                                  TP.{1}, \r\n                                                  TP.F_DOC_TCKEY, \r\n                                                  TP.F_ART_KEY, \r\n                                                  TP.F_SOURCE_KEY, \r\n                                                  TP.F_SOURCE_TYPE, \r\n                                                  TP.F_TARGET_KEY,\r\n                                                  TP.{2},\r\n                                                  TP.F_TARGET_DOC_TCKEY\r\n                                                ORDER BY\r\n                                                  TP.{1}", (object) "TP_LINKS", (object) "F_KEY", (object) "F_ORDER", (object) "F_ART_TCKEY", (object) "TC_OBJ2LINK", (object) TechDbConsts.fld_TargetArtTcKey, (object) str);
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount($"SELECT \r\n                                                 COUNT(*) \r\n                                               FROM \r\n                                                 {"TP_LINKS"} TP\r\n                                               WHERE \r\n                                                 TP.{"F_KEY"} != 0\r\n                                                 {str} ");
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText), tableName, sqlRecordsCount);
  }
}
