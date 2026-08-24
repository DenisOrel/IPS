// Decompiled with JetBrains decompiler
// Type: OxyPlot.PngEncoder
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace OxyPlot;

public class PngEncoder : IImageEncoder
{
  private static readonly ulong[] CrcTable = new ulong[256 /*0x0100*/];
  private readonly PngEncoderOptions options;

  static PngEncoder()
  {
    for (int index1 = 0; index1 < 256 /*0x0100*/; ++index1)
    {
      ulong num = (ulong) index1;
      for (int index2 = 0; index2 < 8; ++index2)
      {
        if (((long) num & 1L) != 0L)
          num = 3988292384UL ^ num >> 1;
        else
          num >>= 1;
      }
      PngEncoder.CrcTable[index1] = num;
    }
  }

  public PngEncoder(PngEncoderOptions options) => this.options = options;

  public byte[] Encode(OxyColor[,] pixels)
  {
    int length1 = pixels.GetLength(0);
    int length2 = pixels.GetLength(1);
    byte[] bytes = new byte[length1 * length2 * 4 + length2];
    int num1 = 0;
    for (int index1 = 0; index1 < length2; ++index1)
    {
      bytes[num1++] = (byte) 0;
      for (int index2 = 0; index2 < length1; ++index2)
      {
        byte[] numArray1 = bytes;
        int index3 = num1;
        int num2 = index3 + 1;
        int r = (int) pixels[index2, index1].R;
        numArray1[index3] = (byte) r;
        byte[] numArray2 = bytes;
        int index4 = num2;
        int num3 = index4 + 1;
        int g = (int) pixels[index2, index1].G;
        numArray2[index4] = (byte) g;
        byte[] numArray3 = bytes;
        int index5 = num3;
        int num4 = index5 + 1;
        int b = (int) pixels[index2, index1].B;
        numArray3[index5] = (byte) b;
        byte[] numArray4 = bytes;
        int index6 = num4;
        num1 = index6 + 1;
        int a = (int) pixels[index2, index1].A;
        numArray4[index6] = (byte) a;
      }
    }
    PngEncoder.MemoryWriter w = new PngEncoder.MemoryWriter();
    w.Write((byte) 137);
    w.Write("PNG\r\n\u001A\n".ToCharArray());
    PngEncoder.WriteChunk((BinaryWriter) w, "IHDR", PngEncoder.CreateHeaderData(length1, length2));
    PngEncoder.WriteChunk((BinaryWriter) w, "pHYs", PngEncoder.CreatePhysicalDimensionsData(this.options.DpiX, this.options.DpiY));
    PngEncoder.WriteChunk((BinaryWriter) w, "IDAT", PngEncoder.CreateUncompressedBlocks(bytes));
    PngEncoder.WriteChunk((BinaryWriter) w, "IEND", new byte[0]);
    return w.ToArray();
  }

  public byte[] Encode(byte[,] pixels, OxyColor[] palette) => throw new NotImplementedException();

  internal static uint Adler32(IEnumerable<byte> data)
  {
    uint num1 = 1;
    uint num2 = 0;
    foreach (byte num3 in data)
    {
      num1 = (num1 + (uint) num3) % 65521U;
      num2 = (num2 + num1) % 65521U;
    }
    return num2 << 16 /*0x10*/ | num1;
  }

  private static byte[] CreateHeaderData(int width, int height)
  {
    PngEncoder.MemoryWriter w = new PngEncoder.MemoryWriter();
    PngEncoder.WriteBigEndian((BinaryWriter) w, width);
    PngEncoder.WriteBigEndian((BinaryWriter) w, height);
    w.Write((byte) 8);
    w.Write((byte) 6);
    w.Write((byte) 0);
    w.Write((byte) 0);
    w.Write((byte) 0);
    return w.ToArray();
  }

  private static byte[] CreatePhysicalDimensionsData(double dpix, double dpiy)
  {
    int num1 = (int) (dpix / 0.0254);
    int num2 = (int) (dpiy / 0.0254);
    PngEncoder.MemoryWriter w = new PngEncoder.MemoryWriter();
    PngEncoder.WriteBigEndian((BinaryWriter) w, num1);
    PngEncoder.WriteBigEndian((BinaryWriter) w, num2);
    w.Write((byte) 1);
    return w.ToArray();
  }

  private static byte[] CreateUncompressedBlocks(byte[] bytes)
  {
    PngEncoder.MemoryWriter w = new PngEncoder.MemoryWriter();
    w.Write((byte) 8);
    w.Write((byte) 29);
    for (int index = 0; index < bytes.Length; index += (int) ushort.MaxValue)
    {
      ushort count = (ushort) Math.Min(bytes.Length - index, (int) ushort.MaxValue);
      byte num1 = index + (int) count < bytes.Length ? (byte) 0 : (byte) 1;
      w.Write(num1);
      w.Write((byte) ((uint) count & (uint) byte.MaxValue));
      w.Write((byte) ((int) count >> 8 & (int) byte.MaxValue));
      int num2 = (int) ~count;
      w.Write((byte) (num2 & (int) byte.MaxValue));
      w.Write((byte) (num2 >> 8 & (int) byte.MaxValue));
      w.Write(bytes, index, (int) count);
    }
    PngEncoder.WriteBigEndian((BinaryWriter) w, PngEncoder.Adler32((IEnumerable<byte>) bytes));
    return w.ToArray();
  }

  private static ulong UpdateCrc(ulong crc, IEnumerable<byte> data)
  {
    return data.Aggregate<byte, ulong>(crc, (Func<ulong, byte, ulong>) ((current, x) => PngEncoder.CrcTable[checked ((ulong) ((unchecked ((long) current) ^ (long) x) & (long) byte.MaxValue))] ^ current >> 8));
  }

  private static void WriteBigEndian(BinaryWriter w, int value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    w.Write(bytes[3]);
    w.Write(bytes[2]);
    w.Write(bytes[1]);
    w.Write(bytes[0]);
  }

  private static void WriteBigEndian(BinaryWriter w, uint value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    w.Write(bytes[3]);
    w.Write(bytes[2]);
    w.Write(bytes[1]);
    w.Write(bytes[0]);
  }

  private static void WriteChunk(BinaryWriter w, string type, byte[] data)
  {
    byte[] array = ((IEnumerable<char>) type.ToCharArray()).Select<char, byte>((Func<char, byte>) (ch => (byte) ch)).ToArray<byte>();
    PngEncoder.WriteBigEndian(w, data.Length);
    w.Write(array);
    w.Write(data);
    uint num = (uint) PngEncoder.UpdateCrc((ulong) (uint) PngEncoder.UpdateCrc((ulong) uint.MaxValue, (IEnumerable<byte>) array), (IEnumerable<byte>) data) ^ uint.MaxValue;
    PngEncoder.WriteBigEndian(w, num);
  }

  private class MemoryWriter : BinaryWriter
  {
    public MemoryWriter()
      : base((Stream) new MemoryStream())
    {
    }

    public byte[] ToArray()
    {
      this.BaseStream.Flush();
      return ((MemoryStream) this.BaseStream).ToArray();
    }
  }
}
