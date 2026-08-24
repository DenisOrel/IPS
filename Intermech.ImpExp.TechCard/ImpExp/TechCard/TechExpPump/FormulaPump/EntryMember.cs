// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.EntryMember
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

internal class EntryMember : BaseMember
{
  public EntryMember Owner;
  public EntryMemberType MemberType;
  public string Code = string.Empty;

  public bool Load_v102(BinaryReader reader)
  {
    int num = this.Load(reader) ? 1 : 0;
    if (num == 0)
      return num != 0;
    this.Conditions.ProcessRs();
    this.CompiledConditions.ProcessRS_CMP();
    return num != 0;
  }

  public override object Clone()
  {
    EntryMember entryMember = new EntryMember();
    foreach (string str in (List<string>) this)
      entryMember.Add(str);
    entryMember.MemberType = this.MemberType;
    entryMember.Code = this.Code;
    entryMember.MemberNo = this.MemberNo;
    entryMember.Conditions = this.Conditions.Clone() as FormulaList;
    entryMember.CompiledConditions = this.CompiledConditions.Clone() as CompiledFormulaList;
    return (object) entryMember;
  }
}
