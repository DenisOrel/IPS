// Decompiled with JetBrains decompiler
// Type: OxyPlot.BinaryReaderExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace OxyPlot;

public static class BinaryReaderExtensions
{
  public static string ReadString(this BinaryReader r, int length, Encoding encoding = null)
  {
    if (encoding == null)
      encoding = Encoding.UTF8;
    return encoding.GetString(r.ReadBytes(length), 0, length);
  }

  [CLSCompliant(false)]
  public static uint ReadUInt32(this BinaryReader r, bool isLittleEndian)
  {
    return !isLittleEndian ? r.ReadBigEndianUInt32() : r.ReadUInt32();
  }

  public static int ReadInt32(this BinaryReader r, bool isLittleEndian)
  {
    return !isLittleEndian ? r.ReadBigEndianInt32() : r.ReadInt32();
  }

  [CLSCompliant(false)]
  public static ushort ReadUInt16(this BinaryReader r, bool isLittleEndian)
  {
    return !isLittleEndian ? r.ReadBigEndianUInt16() : r.ReadUInt16();
  }

  public static double ReadDouble(this BinaryReader r, bool isLittleEndian)
  {
    return !isLittleEndian ? r.ReadBigEndianDouble() : r.ReadDouble();
  }

  [CLSCompliant(false)]
  public static uint[] ReadUInt32Array(this BinaryReader r, int count, bool isLittleEndian)
  {
    uint[] numArray = new uint[count];
    for (int index = 0; index < count; ++index)
      numArray[index] = isLittleEndian ? r.ReadUInt32() : r.ReadBigEndianUInt32();
    return numArray;
  }

  [CLSCompliant(false)]
  public static ushort[] ReadUInt16Array(this BinaryReader r, int count, bool isLittleEndian)
  {
    ushort[] numArray = new ushort[count];
    for (int index = 0; index < count; ++index)
      numArray[index] = isLittleEndian ? r.ReadUInt16() : r.ReadBigEndianUInt16();
    return numArray;
  }

  [CLSCompliant(false)]
  public static uint ReadBigEndianUInt32(this BinaryReader r)
  {
    byte[] numArray = r.ReadBytes(4);
    Array.Reverse((Array) numArray);
    return BitConverter.ToUInt32(numArray, 0);
  }

  public static int ReadBigEndianInt32(this BinaryReader r)
  {
    byte[] numArray = r.ReadBytes(4);
    Array.Reverse((Array) numArray);
    return BitConverter.ToInt32(numArray, 0);
  }

  [CLSCompliant(false)]
  public static ushort ReadBigEndianUInt16(this BinaryReader r)
  {
    byte[] numArray = r.ReadBytes(2);
    Array.Reverse((Array) numArray);
    return BitConverter.ToUInt16(numArray, 0);
  }

  public static double ReadBigEndianDouble(this BinaryReader r)
  {
    byte[] numArray = r.ReadBytes(8);
    Array.Reverse((Array) numArray);
    return BitConverter.ToDouble(numArray, 0);
  }
}
