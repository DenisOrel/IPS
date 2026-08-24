// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2Art.TechRoute2ArtDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2Art;

internal class TechRoute2ArtDataBuilder<T>(T pumper) : TechDataBuilderSimple<T>(pumper) where T : TechPumpBase
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
    if (this._pumper.LastObjID != 0L)
      str2 = $" WHERE {"F_KEY"} >= {this._pumper.LastObjID}";
    string str3 = this.GetPumpModeCond("F_ART_TCKEY", string.Empty);
    if (str3 != string.Empty)
      str3 = !(str2 != string.Empty) ? " WHERE " + str3 : " AND " + str3;
    string sqlText = string.Format($" SELECT   *  from      {{1}}  {str2}{str3}   order by {{0}}  ", (object) "F_KEY", (object) "TC_OBJ2LINK");
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount($"select count(*) from {"TC_OBJ2LINK"} {str2} {str3}");
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlText), string.Empty, sqlRecordsCount);
  }
}
