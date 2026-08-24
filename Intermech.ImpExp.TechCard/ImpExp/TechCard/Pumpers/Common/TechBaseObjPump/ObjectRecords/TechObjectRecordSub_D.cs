// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords.TechObjectRecordSub_D
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;

internal class TechObjectRecordSub_D : TechObjectRecordSub
{
  public string Entity = string.Empty;

  public TechObjectRecordSub_D() => this.TablePrefix = "_D";

  private int idx_F_ENTITY
  {
    get => this.GetFieldIndex("F_ENTITY");
    set => this.SetFieldIndex("F_ENTITY", value);
  }

  private int idx_F_VALUE
  {
    get => this.GetFieldIndex("F_VALUE");
    set => this.SetFieldIndex("F_VALUE", value);
  }

  public object Value
  {
    get => this.GetFieldValue("F_ENTITY");
    set => this.SetFieldValue("F_ENTITY", value);
  }

  public override void Clear()
  {
    base.Clear();
    this.Entity = string.Empty;
  }

  public override void Assign(object source)
  {
    base.Assign(source);
    if (!(source is TechObjectRecordSub_D objectRecordSubD))
      return;
    this.Entity = objectRecordSubD.Entity;
  }

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    this.idx_F_ENTITY = schema["F_ENTITY"];
    if (!schema.ContainsKey("F_VALUE"))
      return;
    this.idx_F_VALUE = schema["F_VALUE"];
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    this.Entity = dataReader.IsDBNull(this.idx_F_ENTITY) ? string.Empty : dataReader.GetString(this.idx_F_ENTITY);
    if (this.idx_F_VALUE == -1)
      return;
    this.Value = dataReader.IsDBNull(this.idx_F_VALUE) ? (object) string.Empty : (object) dataReader.GetString(this.idx_F_VALUE);
  }
}
