// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.TablesPump.LayerData
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.TechExpPump.Common;
using System;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.TablesPump;

[Serializable]
public class LayerData
{
  public string[,] Data;
  public int Cols;
  public int Rows;

  public void SetSizes(int newCols, int newRows)
  {
    this.Data = new string[newCols, newRows];
    this.Cols = newCols;
    this.Rows = newRows;
  }

  public void Load(BinaryReader br, ushort version)
  {
    for (int index1 = 0; index1 < this.Cols; ++index1)
    {
      for (int index2 = 0; index2 < this.Rows; ++index2)
        this.Data[index1, index2] = TechExpert.Utils.TechReadString(br, (int) version);
    }
  }
}
