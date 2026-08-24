// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.BaseMember
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.TechExpPump.Common;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

internal class BaseMember : List<string>, ICloneable
{
  public int MemberNo;

  protected BaseMember()
  {
    this.Conditions = new FormulaList();
    this.Conditions.Clear();
    this.CompiledConditions = new CompiledFormulaList();
    this.CompiledConditions.Clear();
  }

  public FormulaList Conditions { get; protected set; }

  public CompiledFormulaList CompiledConditions { get; protected set; }

  public virtual bool Load(BinaryReader reader)
  {
    if (reader == null)
      return false;
    short num = reader.ReadInt16();
    for (int index = 1; index <= (int) num; ++index)
      this.Add(TechExpert.Utils.TechReadByteString(reader));
    return this.Conditions.Load_Raw(reader) && this.CompiledConditions.Load_Raw(reader);
  }

  public virtual object Clone()
  {
    BaseMember baseMember = new BaseMember();
    foreach (string str in (List<string>) this)
      baseMember.Add(str);
    baseMember.MemberNo = this.MemberNo;
    baseMember.Conditions = this.Conditions.Clone() as FormulaList;
    baseMember.CompiledConditions = this.CompiledConditions.Clone() as CompiledFormulaList;
    return (object) baseMember;
  }

  protected new virtual void Clear()
  {
    base.Clear();
    this.Conditions.Clear();
    this.CompiledConditions.Clear();
  }
}
