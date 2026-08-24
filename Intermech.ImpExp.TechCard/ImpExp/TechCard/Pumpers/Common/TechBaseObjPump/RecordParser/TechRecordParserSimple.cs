// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.RecordParser.TechRecordParserSimple
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.RecordParser;

internal class TechRecordParserSimple : TechRecordParser
{
  private bool _showWarnings;
  private static TechRecordParserSimple _instance;

  public override object Parse(IDataReader dataReader, int fieldIndex)
  {
    Type type = dataReader != null ? dataReader.GetFieldType(fieldIndex) : throw new ArgumentNullException(nameof (dataReader));
    switch (type.FullName)
    {
      case "System.Char":
        return (object) (char) (dataReader.IsDBNull(fieldIndex) ? 0 : (int) dataReader.GetChar(fieldIndex));
      case "System.DateTime":
        return (object) (dataReader.IsDBNull(fieldIndex) ? DateTime.Now : dataReader.GetDateTime(fieldIndex));
      case "System.Decimal":
        return (object) (dataReader.IsDBNull(fieldIndex) ? 0M : dataReader.GetDecimal(fieldIndex));
      case "System.Double":
        return (object) (dataReader.IsDBNull(fieldIndex) ? 0.0 : BasePumpHelper.ToDouble(dataReader[fieldIndex]));
      case "System.Int16":
        return (object) (short) (dataReader.IsDBNull(fieldIndex) ? (int) Convert.ToInt16(0) : (int) dataReader.GetInt16(fieldIndex));
      case "System.Int32":
        return (object) (dataReader.IsDBNull(fieldIndex) ? 0 : BasePumpHelper.ToInt32(dataReader[fieldIndex]));
      case "System.Int64":
        return (object) (dataReader.IsDBNull(fieldIndex) ? 0L : dataReader.GetInt64(fieldIndex));
      case "System.Single":
        return (object) (float) (dataReader.IsDBNull(fieldIndex) ? 0.0 : (double) dataReader.GetFloat(fieldIndex));
      case "System.String":
        return !dataReader.IsDBNull(fieldIndex) ? (object) dataReader.GetString(fieldIndex) : (object) string.Empty;
      default:
        if (this.ShowWarnings)
          TechcardConsts.Plugin.Idw.AppManager.AddNewWarningMessage("Неизвестный тип записи: " + type.FullName);
        return (object) null;
    }
  }

  public bool ShowWarnings
  {
    get => this._showWarnings;
    set => this._showWarnings = value;
  }

  public static TechRecordParserSimple GetInstance()
  {
    if (TechRecordParserSimple._instance == null)
      TechRecordParserSimple._instance = new TechRecordParserSimple();
    return TechRecordParserSimple._instance;
  }
}
