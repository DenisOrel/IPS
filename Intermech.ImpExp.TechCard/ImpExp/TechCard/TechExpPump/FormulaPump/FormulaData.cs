// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.FormulaData
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.TechExpPump.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

[Serializable]
public class FormulaData
{
  public short ResType;
  public readonly FormulaList Data;
  public readonly CompiledFormulaList CData;
  public readonly List<string> ID;

  private void InitData()
  {
  }

  public FormulaData(short resultType)
  {
    this.Data = new FormulaList();
    this.CData = new CompiledFormulaList();
    this.ID = new List<string>();
    this.ResType = resultType;
    this.InitData();
  }

  public FormulaData(
    short resultType,
    List<string> id,
    FormulaList data,
    CompiledFormulaList cdata = null)
  {
    this.ResType = resultType;
    this.ID = id ?? new List<string>();
    this.Data = data ?? new FormulaList();
    this.CData = cdata ?? new CompiledFormulaList();
    this.InitData();
  }

  public void ResetClass(short resultType)
  {
    this.ResType = resultType;
    this.Clear();
  }

  public string GetOriginal() => throw new Exception("Not implemented yet!");

  public void Copy(FormulaData source) => throw new Exception("Not implemented yet!");

  public bool Compile() => throw new Exception("Not implemented yet!");

  public void Clear()
  {
    this.Data.Clear();
    this.CData.Clear();
    this.ID.Clear();
  }

  public void Load(Stream stream)
  {
    if (stream == null)
      return;
    BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding(1251));
    try
    {
      this.Load(reader);
    }
    finally
    {
      reader.Close();
    }
  }

  public void Load(BinaryReader reader, int version)
  {
    if (reader == null)
      return;
    this.ResType = reader.ReadInt16();
    this.Data.Load(reader, version);
    this.CData.Load(reader, version);
    this.ID.Clear();
    ushort num = reader.ReadUInt16();
    for (ushort index = 0; (int) index < (int) num; ++index)
      this.ID.Add(TechExpert.Utils.TechReadString(reader, version));
  }

  public void Load(BinaryReader reader) => this.Load(reader, TechExpert.Const.cnt_FileVersion);

  public void Save(Stream stream) => throw new Exception("Not implemented yet!");
}
