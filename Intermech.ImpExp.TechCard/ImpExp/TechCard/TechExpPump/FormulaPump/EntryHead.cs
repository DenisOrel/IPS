// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.EntryHead
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.TechExpPump.Common;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

internal class EntryHead : List<ListMember>
{
  public EntryCont Cont;
  public string Name = string.Empty;

  public EntryHead() => this.Cont = new EntryCont();

  public bool Load(BinaryReader reader)
  {
    if (reader == null)
      return false;
    if (!this.Cont.Load(reader))
    {
      this.Name = string.Empty;
      return false;
    }
    this.Name = TechExpert.Utils.TechReadByteString(reader);
    for (int index = 1; index <= (int) this.Cont.FormAm; ++index)
    {
      ListMember listMember = new ListMember(string.Empty, string.Empty);
      if (!listMember.Load(reader))
        return false;
      this.Add(listMember);
    }
    return true;
  }

  public bool Load_v102(BinaryReader reader)
  {
    bool flag = this.Load(reader);
    if (flag)
    {
      foreach (ListMember listMember in (List<ListMember>) this)
      {
        --listMember.MemberNo;
        listMember.Formula.ProcessRs();
        listMember.Conditions.ProcessRs();
        listMember.CMPFormula.ProcessRS_CMP();
        listMember.CompiledConditions.ProcessRS_CMP();
      }
    }
    return flag;
  }
}
