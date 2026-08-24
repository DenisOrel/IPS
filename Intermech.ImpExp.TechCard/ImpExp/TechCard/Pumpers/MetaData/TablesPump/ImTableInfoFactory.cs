// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump.ImTableInfoFactory
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;

internal class ImTableInfoFactory : TechItemFactoryBase<ImTableInfo>
{
  private int _idx_field_RecordID = -1;
  private int _idx_field_RecordName = -1;
  private int _idx_field_TableKey = -1;
  private int _idx_field_TableName = -1;

  public ImTableInfoFactory(IDataReader dataReader)
    : base("IM_TABLES", dataReader)
  {
    this._idx_field_RecordID = dataReader != null ? dataReader.GetOrdinal("F_ID") : throw new ArgumentNullException(nameof (dataReader));
    this._idx_field_RecordName = dataReader.GetOrdinal("F_NAME");
    this._idx_field_TableKey = dataReader.GetOrdinal("F_TBLKEY");
    this._idx_field_TableName = dataReader.GetOrdinal("F_TABLE");
  }

  public override ImTableInfo CreateItem(IDataReader idr)
  {
    return new ImTableInfo(this.getInt32(idr, this._idx_field_TableKey), this.getString(idr, this._idx_field_TableName), this.getInt32(idr, this._idx_field_RecordID), this.getString(idr, this._idx_field_RecordName));
  }
}
