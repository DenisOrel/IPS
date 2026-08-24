// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.TechDataBuilderSimple`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

internal class TechDataBuilderSimple<T>(T pumper) : TechDataBuilder<T>(pumper) where T : TechPumpBase
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
    string condField = dopType == string.Empty ? "F_KEY" : "F_PARENTKEY";
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    stringBuilder1.Append(" SELECT COUNT(*) FROM " + tableName2);
    stringBuilder2.Append(" SELECT * FROM " + tableName2);
    bool flag = false;
    if (this._pumper.LastObjID != 0L)
    {
      stringBuilder1.Append(" WHERE ");
      stringBuilder2.Append(" WHERE ");
      flag = true;
    }
    if (this._pumper.LastObjID != 0L)
    {
      stringBuilder1.Append($"{condField} > {(object) this._pumper.LastObjID}");
      stringBuilder2.Append($"{condField} > {(object) this._pumper.LastObjID}");
    }
    string pumpModeCond = this.GetPumpModeCond(condField, dopType);
    if (pumpModeCond != string.Empty)
    {
      if (flag)
      {
        stringBuilder1.Append(" AND ");
        stringBuilder2.Append(" AND ");
      }
      else
      {
        stringBuilder1.Append(" WHERE ");
        stringBuilder2.Append(" WHERE ");
      }
      stringBuilder1.Append(pumpModeCond);
      stringBuilder2.Append(pumpModeCond);
    }
    string str = string.Empty;
    if (dopType == "D")
      str = ",F_ENTITY,F_ROW";
    stringBuilder2.Append($" ORDER BY {condField}{str}");
    string sqlText1 = stringBuilder1.ToString();
    string sqlText2 = stringBuilder2.ToString();
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount(sqlText1);
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText2), tableName2, sqlRecordsCount);
  }
}
