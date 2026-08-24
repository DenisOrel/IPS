// Decompiled with JetBrains decompiler
// Type: OxyPlot.CircularDictionary
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.IO;

#nullable disable
namespace OxyPlot;

internal class CircularDictionary
{
  private readonly byte[] data;
  private readonly int mask;
  private int index;

  public CircularDictionary(int size)
  {
    this.data = new byte[size];
    this.index = 0;
    if (size > 0 && (size & size - 1) == 0)
      this.mask = size - 1;
    else
      this.mask = 0;
  }

  public void Append(int b)
  {
    this.data[this.index] = (byte) b;
    if (this.mask != 0)
      this.index = this.index + 1 & this.mask;
    else
      this.index = (this.index + 1) % this.data.Length;
  }

  public void Copy(int dist, int len, BinaryWriter w)
  {
    if (len < 0 || dist < 1 || dist > this.data.Length)
      throw new Exception();
    if (this.mask != 0)
    {
      int index1 = this.index - dist + this.data.Length & this.mask;
      for (int index2 = 0; index2 < len; ++index2)
      {
        w.Write(this.data[index1]);
        this.data[this.index] = this.data[index1];
        index1 = index1 + 1 & this.mask;
        this.index = this.index + 1 & this.mask;
      }
    }
    else
    {
      int index3 = (this.index - dist + this.data.Length) % this.data.Length;
      for (int index4 = 0; index4 < len; ++index4)
      {
        w.Write(this.data[index3]);
        this.data[this.index] = this.data[index3];
        index3 = (index3 + 1) % this.data.Length;
        this.index = (this.index + 1) % this.data.Length;
      }
    }
  }
}
