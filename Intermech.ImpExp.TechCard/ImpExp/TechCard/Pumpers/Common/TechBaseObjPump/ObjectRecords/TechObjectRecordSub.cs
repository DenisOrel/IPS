// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords.TechObjectRecordSub
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;

internal class TechObjectRecordSub : TechObjectRecordBase
{
  public string TablePrefix = string.Empty;
  public int ParentKey;
  public int TcKey;
  public int Row;

  public int Idx_F_PARENTKEY
  {
    get => this.GetFieldIndex("F_PARENTKEY");
    set => this.SetFieldIndex("F_PARENTKEY", value);
  }

  public int Idx_F_TCKEY
  {
    get => this.GetFieldIndex("F_TCKEY");
    set => this.SetFieldIndex("F_TCKEY", value);
  }

  public int Idx_F_ROW
  {
    get => this.GetFieldIndex("F_ROW");
    set => this.SetFieldIndex("F_ROW", value);
  }

  public override void Clear()
  {
    base.Clear();
    this.TablePrefix = string.Empty;
    this.ParentKey = -1;
    this.TcKey = -1;
    this.Row = 0;
  }

  public override void Assign(object source)
  {
    base.Assign(source);
    if (!(source is TechObjectRecordSub techObjectRecordSub))
      return;
    this.TablePrefix = techObjectRecordSub.TablePrefix;
    this.ParentKey = techObjectRecordSub.ParentKey;
    this.TcKey = techObjectRecordSub.TcKey;
    this.Row = techObjectRecordSub.Row;
  }

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    int num;
    this.Idx_F_PARENTKEY = schema.TryGetValue("F_PARENTKEY", out num) ? num : -1;
    this.Idx_F_ROW = schema.TryGetValue("F_ROW", out num) ? num : -1;
    this.Idx_F_TCKEY = schema.TryGetValue("F_TCKEY", out num) ? num : -1;
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    if (this.Idx_F_PARENTKEY != -1)
      this.ParentKey = dataReader.IsDBNull(this.Idx_F_PARENTKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[this.Idx_F_PARENTKEY]);
    if (this.Idx_F_ROW != -1)
      this.Row = dataReader.IsDBNull(this.Idx_F_ROW) ? 0 : BasePumpHelper.ToInt32(dataReader[this.Idx_F_ROW]);
    if (this.Idx_F_TCKEY == -1)
      return;
    this.TcKey = dataReader.IsDBNull(this.Idx_F_TCKEY) ? 0 : BasePumpHelper.ToInt32(dataReader[this.Idx_F_TCKEY]);
  }
}
