// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TC_INVNOM.InvNomStructRec
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TC_INVNOM;

public class InvNomStructRec
{
  public const string InvNomStruct2EntityMask = "Поле:{0}";
  private string _fieldName;
  private string _keyField;
  private int _tableId;
  private string _tableName;
  private int _imbaseRecId;
  private int _dataType;
  private int _sort;
  private string _name;
  private string _entity;
  private InvNomStructRec.Flags _flag;
  private InvNomStructRec.Status _status = InvNomStructRec.Status.FullAccess;
  private string _typeString;
  private string _data;

  private void LoadData(IDataReader dbReader)
  {
    if (dbReader == null)
      return;
    this._fieldName = dbReader.GetString(dbReader.GetOrdinal("F_FIELDNAME"));
    this._keyField = dbReader.IsDBNull(dbReader.GetOrdinal("F_KEYFIELD")) ? string.Empty : dbReader.GetString(dbReader.GetOrdinal("F_KEYFIELD"));
    this._tableId = BasePumpHelper.ToInt32(dbReader[dbReader.GetOrdinal("F_LU_TABLE_ID")]);
    this._tableName = dbReader.IsDBNull(dbReader.GetOrdinal("F_TABLE")) ? string.Empty : dbReader.GetString(dbReader.GetOrdinal("F_TABLE"));
    this._imbaseRecId = BasePumpHelper.ToInt32(dbReader[dbReader.GetOrdinal("F_LU_RESULTFIELD_ID")]);
    this._dataType = BasePumpHelper.ToInt32(dbReader[dbReader.GetOrdinal("F_DATATYPE")]);
    this._sort = BasePumpHelper.ToInt32(dbReader[dbReader.GetOrdinal("F_SORT")]);
    this._name = dbReader.IsDBNull(dbReader.GetOrdinal("F_LONGNAME")) ? string.Empty : dbReader.GetString(dbReader.GetOrdinal("F_LONGNAME"));
    this._entity = dbReader.IsDBNull(dbReader.GetOrdinal("F_ENTITY")) ? string.Empty : dbReader.GetString(dbReader.GetOrdinal("F_ENTITY"));
    this._flag = (InvNomStructRec.Flags) BasePumpHelper.ToInt32(dbReader[dbReader.GetOrdinal("F_FLAGS")]);
    if (BasePumpHelper.ToInt32(dbReader[dbReader.GetOrdinal("F_STATUS")]) == 0)
      this._status = InvNomStructRec.Status.ReadOnly;
    this._typeString = this.GetStringTypeByInvNomDataType(this._dataType);
    this._data = Convert.ToString(dbReader[dbReader.GetOrdinal("F_DATA")]);
  }

  private string GetStringTypeByInvNomDataType(int dataType)
  {
    switch (dataType)
    {
      case 1:
        return "S";
      case 2:
        return "I";
      case 3:
        return "R";
      case 4:
        return "B";
      case 6:
      case 7:
        return "D";
      default:
        return "S";
    }
  }

  public InvNomStructRec(IDataReader dbReader) => this.LoadData(dbReader);

  public string FieldName => this._fieldName;

  public string KeyField => this._keyField;

  public int TableId => this._tableId;

  public string TableName => this._tableName;

  public int ImbaseRecId => this._imbaseRecId;

  public int DataType => this._dataType;

  public int Sort
  {
    get => this._sort;
    internal set => this._sort = value;
  }

  public string Name => this._name;

  public string Entity => this._entity;

  public InvNomStructRec.Flags Flag
  {
    get => this._flag;
    set => this._flag = value;
  }

  public InvNomStructRec.Status Stat => this._status;

  public string TypeString => this._typeString;

  public string Data => this._data;

  public static string GenerateEntityName(string fieldName) => $"Поле:{fieldName}";

  [System.Flags]
  public enum Flags
  {
    None = 0,
    NotEmpty = 1,
    FullDateFormat = 2,
    BoolYesNo = 16, // 0x00000010
    BoolTrueFalse = 32, // 0x00000020
  }

  public enum Status
  {
    ReadOnly,
    FullAccess,
  }
}
