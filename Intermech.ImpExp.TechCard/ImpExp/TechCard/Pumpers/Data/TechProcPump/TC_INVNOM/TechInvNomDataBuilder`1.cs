// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TC_INVNOM.TechInvNomDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.TechProcPump.TC_INVNOM;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TC_INVNOM;

internal class TechInvNomDataBuilder<T>(T pumper) : TechDataBuilder<T>(pumper) where T : TechInvNomPump
{
  private string GetSqlCommonText()
  {
    string str1 = string.Empty;
    string empty = string.Empty;
    HashSet<string> stringSet = new HashSet<string>();
    foreach (InvNomStructRec invNomStructRec in this._pumper.structList)
    {
      switch (invNomStructRec.DataType)
      {
        case -102:
        case -101:
          if (string.IsNullOrEmpty(invNomStructRec.TableName))
          {
            stringSet.Add(invNomStructRec.FieldName);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    foreach (InvNomStructRec invNomStructRec in this._pumper.structList)
    {
      string str2 = string.Empty;
      string str3 = string.Empty;
      switch (invNomStructRec.DataType)
      {
        case -102:
          string str4 = invNomStructRec.FieldName.Replace("MINUS_", "");
          str2 = $" A.{str4} AS {invNomStructRec.FieldName}";
          if (!stringSet.Contains(invNomStructRec.FieldName))
          {
            str3 = string.Format(" LEFT JOIN {0}_REC TBL_{1} \r\n                                                           ON A.{2} = - TBL_{1}.{3} ", (object) invNomStructRec.TableName, (object) invNomStructRec.FieldName, (object) str4, (object) "F_LEVEL");
            break;
          }
          break;
        case -101:
          if (!stringSet.Contains(invNomStructRec.FieldName))
          {
            str3 = string.Format(" LEFT JOIN {0} TBL_{1} \r\n                                                           ON A.{1} = TBL_{1}.{2} ", (object) invNomStructRec.TableName, (object) invNomStructRec.FieldName, (object) "F_LEVEL");
            break;
          }
          break;
        case 111:
        case 121:
          string str5 = invNomStructRec.ImbaseRecId == 0 ? "F_NAME" : TechPumpData.Tables.ImFieldsData.GetFieldName(invNomStructRec.ImbaseRecId);
          str2 = stringSet.Contains(invNomStructRec.KeyField) || string.IsNullOrEmpty(str5) ? $" NULL AS {invNomStructRec.FieldName}" : string.Format(" TBL_{0}.{2} {1}", (object) invNomStructRec.KeyField, (object) invNomStructRec.FieldName, (object) str5);
          break;
      }
      if (!string.IsNullOrEmpty(str2))
        str1 = $"{str1},{str2}";
      empty += str3;
    }
    return string.Format(" SELECT  \r\n\t\t\t                                   A.* {0} \r\n                                             FROM \r\n                                               {2} A {1}\r\n                                             ORDER BY\r\n                                               A.{3}", (object) str1, (object) empty, (object) "TC_INVNOM", (object) "F_KEY");
  }

  private string GetSqlCommonCountText()
  {
    return string.Format("SELECT COUNT(*) FROM {1} A {0}", (object) string.Empty, (object) "TC_INVNOM");
  }

  public override TechDataReaderInfo CreateDataReader(string dopType)
  {
    string sqlCommonText = this.GetSqlCommonText();
    int sqlRecordsCount = this._pumper.GetSqlRecordsCount(this.GetSqlCommonCountText());
    return new TechDataReaderInfo(this._pumper.RecType, dopType, this._pumper.GetCustomDataReader(sqlCommonText), string.Empty, sqlRecordsCount);
  }
}
