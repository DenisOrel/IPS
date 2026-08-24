// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.TablesPump.TableInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.TechExpPump.Common;
using Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;
using System;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.TablesPump;

[Serializable]
public class TableInfo
{
  public ushort HierLevel;
  public int BodyAddr;
  public int BodySize;
  public string Name;
  public string Code;
  public FormulaData Cond;
  public ushort Entries;
  public bool UsedForMultiStrings;
  public DimensionInfo[] Dims = new DimensionInfo[3];
  public int TableIdx = -1;

  public bool HasBody => !this.IsDummy && this.BodySize > 0 && this.BodyAddr > 0;

  public bool IsDummy
  {
    get
    {
      bool isDummy = this.Dims[1].ArgList.Count.Equals(0);
      if (this.Entries >= (ushort) 1)
        isDummy = isDummy || this.Dims[0].ArgList.Count.Equals(0);
      if (this.Entries == (ushort) 2)
        isDummy = isDummy || this.Dims[2].ArgList.Count.Equals(0);
      return isDummy;
    }
  }

  public void Load(BinaryReader br, ushort version)
  {
    this.Name = TechExpert.Utils.ReadString(br, version);
    this.Code = TechExpert.Utils.ReadString(br, version);
    this.HierLevel = br.ReadUInt16();
    this.BodyAddr = br.ReadInt32();
    int num = br.ReadInt32();
    this.BodySize = Math.Abs(num);
    this.UsedForMultiStrings = num < 0;
    this.LoadFormulaData(br, version);
    this.Entries = br.ReadUInt16();
    for (int index = 0; index <= 2; ++index)
    {
      this.Dims[index] = new DimensionInfo();
      this.Dims[index].LoadFormFile(br, version);
    }
  }

  public void LoadFormulaData(BinaryReader br, ushort version)
  {
    if (br.ReadUInt16().Equals((ushort) 0))
      return;
    this.Cond = new FormulaData((short) 0);
    this.Cond.Load(br, (int) version);
  }
}
