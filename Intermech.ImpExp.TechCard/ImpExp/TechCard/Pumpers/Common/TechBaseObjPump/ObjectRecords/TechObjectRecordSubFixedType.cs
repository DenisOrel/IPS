// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords.TechObjectRecordSubFixedType
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;

internal class TechObjectRecordSubFixedType : TechObjectRecordSub
{
  private static readonly int[] IdxFx = new int[10];
  protected const string Fld_Fx = "F{0}";

  protected virtual int[] Idx_Fx => TechObjectRecordSubFixedType.IdxFx;

  protected override int GetFieldsCapacity() => 10;

  public override void ParseSchema(IDictionary<string, int> schema)
  {
    base.ParseSchema(schema);
    for (int index = 1; index <= 10; ++index)
      this.Idx_Fx[index - 1] = schema[$"F{index}"];
  }
}
