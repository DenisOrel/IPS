// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump.ImFieldInfoFactory
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;

internal class ImFieldInfoFactory : TechItemFactoryBase<ImFieldInfo>
{
  private const string cnt_field_Key = "F_KEY";
  private const string cnt_field_TableId = "F_TABLE_ID";
  private const string cnt_field_Field = "F_FIELD";
  private const string cnt_field_LongName = "F_LONGNAME";
  private const string cnt_field_ShortName = "F_SHORTNAME";
  private const string cnt_field_Units = "F_UNITS";
  private const string cnt_field_Sort = "F_SORT";
  private const string cnt_field_Width = "F_WIDTH";
  private const string cnt_field_Flags = "F_FLAGS";
  private const string cnt_field_Type = "F_TYPE";
  private const string cnt_field_Required = "F_REQUIRED";
  private const string cnt_field_DataType = "F_DATATYPE";
  private const string cnt_field_EnterMode = "F_ENTERMODE";
  private const string cnt_field_Data = "F_DATA";
  private int _idx_field_Key = -1;
  private int _idx_field_TableId = -1;
  private int _idx_field_Field = -1;
  private int _idx_field_LongName = -1;
  private int _idx_field_ShortName = -1;
  private int _idx_field_Units = -1;
  private int _idx_field_Sort = -1;
  private int _idx_field_Width = -1;
  private int _idx_field_Flags = -1;
  private int _idx_field_Type = -1;
  private int _idx_field_Required = -1;
  private int _idx_field_DataType = -1;
  private int _idx_field_EnterMode = -1;
  private int _idx_field_Data = -1;

  public ImFieldInfoFactory(IDataReader dataReader)
    : base("IM_FIELDS", dataReader)
  {
    this._idx_field_Key = dataReader != null ? dataReader.GetOrdinal("F_KEY") : throw new ArgumentNullException(nameof (dataReader));
    this._idx_field_TableId = dataReader.GetOrdinal("F_TABLE_ID");
    this._idx_field_Field = dataReader.GetOrdinal("F_FIELD");
    this._idx_field_LongName = dataReader.GetOrdinal("F_LONGNAME");
    this._idx_field_ShortName = dataReader.GetOrdinal("F_SHORTNAME");
    this._idx_field_Units = dataReader.GetOrdinal("F_UNITS");
    this._idx_field_Sort = dataReader.GetOrdinal("F_SORT");
    this._idx_field_Width = dataReader.GetOrdinal("F_WIDTH");
    this._idx_field_Flags = dataReader.GetOrdinal("F_FLAGS");
    this._idx_field_Type = dataReader.GetOrdinal("F_TYPE");
    this._idx_field_Required = dataReader.GetOrdinal("F_REQUIRED");
    this._idx_field_DataType = dataReader.GetOrdinal("F_DATATYPE");
    this._idx_field_EnterMode = dataReader.GetOrdinal("F_ENTERMODE");
    this._idx_field_Data = dataReader.GetOrdinal("F_DATA");
  }

  public override ImFieldInfo CreateItem(IDataReader idr)
  {
    return new ImFieldInfo(this.getInt32(idr, this._idx_field_Key), this.getInt32(idr, this._idx_field_TableId), this.getString(idr, this._idx_field_Field), this.getString(idr, this._idx_field_LongName), this.getString(idr, this._idx_field_ShortName), this.getString(idr, this._idx_field_Units), this.getInt32(idr, this._idx_field_Sort), this.getInt32(idr, this._idx_field_Flags), (ImDataMode) this.getInt32(idr, this._idx_field_Type), this.getInt32(idr, this._idx_field_Required), (ImDataTypeEx) this.getInt32(idr, this._idx_field_DataType), (long) this.getInt32(idr, this._idx_field_Width), (ImEnterMode) this.getInt32(idr, this._idx_field_EnterMode), this.getString(idr, this._idx_field_Data));
  }
}
