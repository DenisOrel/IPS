// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords.TechObjectRecordSub_I
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;

internal class TechObjectRecordSub_I : TechObjectRecordSubFixedType
{
  private static readonly int[] IdxFx = new int[10];

  protected override int[] Idx_Fx => TechObjectRecordSub_I.IdxFx;

  public TechObjectRecordSub_I() => this.TablePrefix = "_I";

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    for (int index = 1; index <= 10; ++index)
    {
      if (!dataReader.IsDBNull(this.Idx_Fx[index - 1]))
        this.SetFieldValue($"F{index}", (object) BasePumpHelper.ToInt32(dataReader[this.Idx_Fx[index - 1]]));
    }
  }

  public int GetInt32Value(string fieldName) => Convert.ToInt32(this.GetFieldValue(fieldName));
}
