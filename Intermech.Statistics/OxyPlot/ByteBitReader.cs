// Decompiled with JetBrains decompiler
// Type: OxyPlot.ByteBitReader
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.IO;

#nullable disable
namespace OxyPlot;

public class ByteBitReader : BitReader, IDisposable
{
  private readonly BinaryReader input;
  private int bitPosition;
  private bool disposed;
  private bool isEndOfStream;
  private int nextBits;

  public ByteBitReader(Stream s)
  {
    this.input = s != null ? new BinaryReader(s) : throw new ArgumentException("Argument is null");
    this.bitPosition = 8;
    this.isEndOfStream = false;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  public override int Read()
  {
    if (this.isEndOfStream)
      return -1;
    if (this.bitPosition == 8)
    {
      this.nextBits = (int) this.input.ReadByte();
      if (this.nextBits == -1)
      {
        this.isEndOfStream = true;
        return -1;
      }
      this.bitPosition = 0;
    }
    int num = this.nextBits >> this.bitPosition & 1;
    ++this.bitPosition;
    return num;
  }

  public override int ReadNoEof()
  {
    int num = this.Read();
    return num != -1 ? num : throw new Exception("End of stream reached");
  }

  public override int GetBitPosition() => this.bitPosition % 8;

  public override int ReadByte()
  {
    this.bitPosition = 8;
    return (int) this.input.ReadByte();
  }

  public override void Close() => this.Dispose();

  private void Dispose(bool disposing)
  {
    if (!this.disposed && disposing)
      this.input.Dispose();
    this.disposed = true;
  }
}
