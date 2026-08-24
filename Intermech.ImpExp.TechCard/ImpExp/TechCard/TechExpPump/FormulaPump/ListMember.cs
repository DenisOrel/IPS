// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.ListMember
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.TechExpPump.Common;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

internal class ListMember : BaseMember
{
  public FormulaList Formula;
  public CompiledFormulaList CMPFormula;
  public string Remark;
  public string Number;

  public ListMember(string remark, string number)
  {
    this.Formula = new FormulaList();
    this.Formula.Clear();
    this.CMPFormula = new CompiledFormulaList();
    this.CMPFormula.Clear();
    this.Remark = remark;
    this.Number = number;
  }

  public override bool Load(BinaryReader reader)
  {
    this.Remark = string.Empty;
    this.Number = string.Empty;
    if (reader == null)
      return false;
    this.Remark = TechExpert.Utils.TechReadByteString(reader);
    this.Number = TechExpert.Utils.TechReadByteString(reader);
    this.MemberNo = reader.ReadInt32();
    short num = reader.ReadInt16();
    for (int index = 1; index <= (int) num; ++index)
      this.Add(TechExpert.Utils.TechReadByteString(reader));
    return this.Formula.Load_Raw(reader) && this.Conditions.Load_Raw(reader) && this.CMPFormula.Load_Raw(reader) && this.CompiledConditions.Load_Raw(reader);
  }

  public override object Clone()
  {
    ListMember listMember1 = new ListMember(this.Remark, this.Number);
    listMember1.MemberNo = this.MemberNo;
    ListMember listMember2 = listMember1;
    foreach (string str in (List<string>) this)
      listMember2.Add(str);
    listMember2.Formula = this.Formula.Clone() as FormulaList;
    listMember2.Conditions = this.Conditions.Clone() as FormulaList;
    listMember2.CMPFormula = this.CMPFormula.Clone() as CompiledFormulaList;
    listMember2.CompiledConditions = this.CompiledConditions.Clone() as CompiledFormulaList;
    return (object) listMember2;
  }

  protected override void Clear()
  {
    base.Clear();
    this.Formula.Clear();
    this.CMPFormula.Clear();
  }
}
