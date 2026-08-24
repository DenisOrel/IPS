// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.CompiledFormulaList
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
public class CompiledFormulaList : List<CompiledTokenRecord>, ICloneable
{
  public long GetSize() => throw new Exception("Not implemented yet!");

  public void Load(BinaryReader reader, int version)
  {
    if (reader == null)
      return;
    short num = reader.ReadInt16();
    for (int index = 0; index < (int) num; ++index)
    {
      CompiledTokenRecord compiledTokenRecord = new CompiledTokenRecord();
      compiledTokenRecord.Load(reader, version);
      this.Add(compiledTokenRecord);
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
      CompiledTokenRecord compiledTokenRecord = new CompiledTokenRecord();
      compiledTokenRecord.Load(reader, TechExpert.Const.cnt_FileVersion - 1);
      this.Add(compiledTokenRecord);
    }
    return reader.BaseStream.Position - position == (long) num;
  }

  public void Save(BinaryReader reader)
  {
    if (reader != null)
      throw new Exception("Not implemented yet!");
  }

  public void ProcessRS_CMP()
  {
    foreach (CompiledTokenRecord compiledTokenRecord in (List<CompiledTokenRecord>) this)
    {
      if (compiledTokenRecord.Kind == (byte) 44)
        compiledTokenRecord.Kind = (byte) 43;
    }
  }

  public object Clone()
  {
    CompiledFormulaList compiledFormulaList = new CompiledFormulaList();
    foreach (CompiledTokenRecord compiledTokenRecord in (List<CompiledTokenRecord>) this)
      compiledFormulaList.Add(compiledTokenRecord.Clone() as CompiledTokenRecord);
    return (object) compiledFormulaList;
  }
}
