// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.TablesPump.TableBody
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.TablesPump;

[Serializable]
public class TableBody
{
  public readonly LayerData HCond = new LayerData();
  public readonly LayerData VCond = new LayerData();
  public int Cols;
  public int Rows;
  public int CondCols;
  public int CondRows;
  public int Layers;
  public int BodyCols;
  public int BodyRows;
  public short[] ColWidth;
  public byte[] ColDefSigns;
  public byte[] RowDefSigns;
  public readonly List<LayerData> LayerList = new List<LayerData>();

  public void Load(BinaryReader br, ushort version, TableInfo tInfo)
  {
    this.Load(br, version, tInfo, true);
  }

  public void Load(BinaryReader br, ushort version, TableInfo tInfo, bool changeStreamPos)
  {
    if (changeStreamPos)
      br.BaseStream.Position = (long) tInfo.BodyAddr;
    byte length = br.ReadByte();
    if (!new string(br.ReadChars(12)).Substring(0, (int) length).Equals("TBLB00000000"))
      return;
    int dSize1 = (int) tInfo.Dims[0].dSize;
    int count1 = tInfo.Dims[0].dType.Equals((object) DimensionType.Cond) ? tInfo.Dims[0].ArgList.Count : 0;
    int dSize2 = (int) tInfo.Dims[1].dSize;
    int count2 = tInfo.Dims[1].dType.Equals((object) DimensionType.Cond) ? tInfo.Dims[1].ArgList.Count : 0;
    if (!tInfo.Dims[1].ArgList.Count.Equals(0))
    {
      int dSize3 = (int) tInfo.Dims[2].dSize;
      for (int index = 0; index < dSize3; ++index)
      {
        LayerData layerData = new LayerData();
        layerData.SetSizes(dSize2, dSize1);
        this.LayerList.Add(layerData);
      }
      this.HCond.SetSizes(dSize2, count2);
      this.VCond.SetSizes(count1, dSize1);
      this.Cols = dSize2;
      this.Rows = dSize1;
      this.CondCols = count1;
      this.CondRows = count2;
      this.Layers = dSize3;
      this.BodyCols = this.CondCols;
      this.BodyRows = this.CondRows + 1;
    }
    this.ColWidth = new short[this.Cols + this.CondCols];
    for (int index = 0; index < this.ColWidth.Length; ++index)
      this.ColWidth[index] = br.ReadInt16();
    this.ColDefSigns = new byte[this.CondCols];
    for (int index = 0; index < this.ColDefSigns.Length; ++index)
      this.ColDefSigns[index] = br.ReadByte();
    this.RowDefSigns = new byte[this.CondRows];
    for (int index = 0; index < this.RowDefSigns.Length; ++index)
      this.RowDefSigns[index] = br.ReadByte();
    this.VCond.Load(br, version);
    this.HCond.Load(br, version);
    for (int index = 0; index < this.Layers; ++index)
      this.LayerList[index].Load(br, version);
  }
}
