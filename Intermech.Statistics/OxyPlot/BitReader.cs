// Decompiled with JetBrains decompiler
// Type: OxyPlot.BitReader
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.IO;

#nullable disable
namespace OxyPlot;

public abstract class BitReader
{
  public abstract int Read();

  public abstract int ReadNoEof();

  public abstract void Close();

  public abstract int GetBitPosition();

  public abstract int ReadByte();

  public int ReadBits(int bits)
  {
    int num1 = 0;
    for (int index = 0; index < bits; ++index)
    {
      int num2 = this.Read();
      if (num2 == -1)
        throw new IOException();
      num1 += num2 << index;
    }
    return num1;
  }
}
