// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.FormulaList
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.TechExpPump.Common;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

[Serializable]
public class FormulaList : List<TokenRecord>, ICloneable
{
  public long GetSize() => throw new Exception("Not implemented yet!");

  public void Load(BinaryReader reader, int version)
  {
    short num = reader.ReadInt16();
    for (int index = 0; index < (int) num; ++index)
    {
      TokenRecord tokenRecord = new TokenRecord();
      tokenRecord.Load(reader, version);
      this.Add(tokenRecord);
    }
  }

  public void Load(BinaryReader reader) => this.Load(reader, TechExpert.Const.cnt_FileVersion);

  public bool Load_Raw(BinaryReader reader)
  {
    this.Clear();
    if (reader == null)
      return false;
    short num = reader.ReadInt16();
    if (num == (short) 0)
      return true;
    long position = reader.BaseStream.Position;
    while (reader.BaseStream.Position < position + (long) num)
    {
      TokenRecord tokenRecord = new TokenRecord();
      tokenRecord.Load(reader, TechExpert.Const.cnt_FileVersion - 1);
      this.Add(tokenRecord);
    }
    return reader.BaseStream.Position - position == (long) num;
  }

  public void Save(BinaryReader reader) => throw new Exception("Not implemented yet!");

  public void ProcessRs()
  {
    foreach (TokenRecord tokenRecord in (List<TokenRecord>) this)
    {
      if (tokenRecord.Kind == (byte) 44)
        tokenRecord.Kind = (byte) 43;
    }
  }

  public object Clone()
  {
    FormulaList formulaList = new FormulaList();
    foreach (TokenRecord tokenRecord in (List<TokenRecord>) this)
      formulaList.Add(tokenRecord.Clone() as TokenRecord);
    return (object) formulaList;
  }
}
