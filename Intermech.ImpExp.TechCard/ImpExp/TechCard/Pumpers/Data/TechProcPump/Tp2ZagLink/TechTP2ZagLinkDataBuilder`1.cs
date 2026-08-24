// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Tp2ZagLink.TechTP2ZagLinkDataBuilder`1
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
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Tp2ZagLink;

internal class TechTP2ZagLinkDataBuilder<T>(T pumper) : TechDataBuilder<T>(pumper) where T : TechPumpBase
{
  protected override string GetPumpModeCond(string condField, string dopType)
  {
    string pumpModeCond = string.Empty;
    string str1 = TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.TechProc) ? TechDataBuilder<PumpClass>.GetPumpModeCond("D2Z.F_DOCTCKEY", -2) : string.Empty;
    if (str1 != string.Empty)
      pumpModeCond = str1;
    string str2 = TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.Zagot) ? TechDataBuilder<PumpClass>.GetPumpModeCond("D2Z.F_ZAGOTKEY", 3) : string.Empty;
    if (str2 != string.Empty)
      pumpModeCond = pumpModeCond != string.Empty ? $"{pumpModeCond} AND {str2}" : str2;
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
      str = $" AND D2Z.{"F_KEY"} >= {this._pumper.LastObjID}";
    string pumpModeCond = this.GetPumpModeCond("", dopType);
    if (pumpModeCond != string.Empty)
      str = $"{str} AND {pumpModeCond}";
    string sqlText = string.Format("SELECT \r\n                                                 D2Z.{1}, D2Z.{2}, D2Z.{3}, D2Z.{4}, MIN(LINK.{5}) AS {5} \r\n                                               FROM \r\n                                                 {0} D2Z\r\n                                                 LEFT JOIN \r\n                                                 {6} LINK       \r\n                                                 ON \r\n                                                   D2Z.{3} = LINK.F_OBJ_KEY AND\r\n                                                   LINK.F_OBJ_TYPE = 3\r\n                                                 LEFT JOIN  \r\n                                                   TP_VERSIONS TP_VER\r\n                                                   ON D2Z.F_DOCTCKEY = TP_VER.F_KEY    \r\n                                                 LEFT JOIN TC_ARCDOCS TP_DOC\r\n                                                   ON TP_VER.F_TCKEY = TP_DOC.F_KEY                                                         \r\n                                               WHERE \r\n                                                 TP_DOC.F_KIND <> 12 \r\n                                                 AND   \r\n                                                 D2Z.{1} != 0 \r\n                                                 {7} \r\n                                               GROUP BY\r\n                                                 D2Z.{1}, D2Z.{2}, D2Z.{3}, D2Z.{4}     \r\n                                               ORDER BY \r\n                                                 D2Z.{1}", (object) "TP_DOC_ZAG", (object) "F_KEY", (object) "F_DOCTCKEY", (object) "F_ZAGOTKEY", (object) "F_ORDER", (object) "F_ART_TCKEY", (object) "TC_OBJ2LINK", (object) str);
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount($"SELECT \r\n                                                 COUNT(*) \r\n                                               FROM \r\n                                                 {"TP_DOC_ZAG"} D2Z\r\n                                                 LEFT JOIN  \r\n                                                   TP_VERSIONS TP_VER\r\n                                                   ON D2Z.F_DOCTCKEY = TP_VER.F_KEY    \r\n                                                 LEFT JOIN TC_ARCDOCS TP_DOC\r\n                                                   ON TP_VER.F_TCKEY = TP_DOC.F_KEY\r\n                                               WHERE \r\n                                                 TP_DOC.F_KIND <> 12\r\n                                                 AND\r\n                                                 D2Z.{"F_KEY"} != 0\r\n                                                 {str} ");
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText), tableName, sqlRecordsCount);
  }
}
