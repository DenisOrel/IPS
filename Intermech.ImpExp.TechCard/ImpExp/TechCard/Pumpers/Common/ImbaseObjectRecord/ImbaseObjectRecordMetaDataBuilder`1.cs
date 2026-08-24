// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord.ImbaseObjectRecordMetaDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord;

internal class ImbaseObjectRecordMetaDataBuilder<T>(T pumper) : TechDataBuilder<T>(pumper) where T : TechPumpBase
{
  protected override string GetPumpModeCond(string condField, string dopType) => string.Empty;

  public override TechDataReaderInfo CreateDataReader(string dopType)
  {
    string tableName1 = this._pumper.TableName;
    if (string.IsNullOrEmpty(tableName1))
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage("Имя таблицы не найдено");
      return (TechDataReaderInfo) null;
    }
    string tableName2 = dopType != string.Empty ? $"{tableName1}_{dopType}" : tableName1;
    string str1 = " b.F_LEVEL";
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    if (dopType == string.Empty)
    {
      stringBuilder1.Append($" SELECT COUNT(*) FROM {tableName2} b ");
      stringBuilder2.Append($" SELECT b.* FROM {tableName2} b ");
    }
    else
    {
      stringBuilder1.Append(" SELECT COUNT(*) FROM ");
      stringBuilder1.Append(tableName2 + " a, ");
      stringBuilder1.Append(this._pumper.TableName + " b  ");
      stringBuilder1.Append(" WHERE ABS(a.F_LEVEL) = b.F_LEVEL");
      stringBuilder2.Append(" SELECT a.*, ");
      stringBuilder2.Append(str1 + " as F_PARENTKEY");
      stringBuilder2.Append($" FROM {tableName2} a, ");
      stringBuilder2.Append(this._pumper.TableName + " b  ");
      stringBuilder2.Append(" WHERE ABS(a.F_LEVEL) = b.F_LEVEL");
    }
    if (this._pumper.LastObjID != 0L)
    {
      string str2 = dopType == string.Empty ? " WHERE " : " AND ";
      stringBuilder1.Append(str2);
      stringBuilder2.Append(str2);
    }
    if (this._pumper.LastObjID != 0L)
    {
      stringBuilder1.Append($"{str1} > {(object) this._pumper.LastObjID}");
      stringBuilder2.Append($"{str1} > {(object) this._pumper.LastObjID}");
    }
    string empty = string.Empty;
    stringBuilder2.Append($" ORDER BY {str1}{empty}");
    string sqlText1 = stringBuilder1.ToString();
    string sqlText2 = stringBuilder2.ToString();
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount(sqlText1);
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText2), tableName2, sqlRecordsCount);
  }
}
