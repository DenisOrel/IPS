// Decompiled with JetBrains decompiler
// Type: OxyPlot.PngDecoder
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace OxyPlot;

public class PngDecoder : IImageDecoder
{
  public OxyImageInfo GetImageInfo(byte[] bytes)
  {
    MemoryStream input = new MemoryStream(bytes);
    BinaryReader r = new BinaryReader((Stream) input);
    r.ReadBytes(8);
    int num1 = (int) r.ReadBigEndianUInt32();
    r.ReadString(4);
    int num2 = (int) r.ReadBigEndianUInt32();
    int num3 = (int) r.ReadBigEndianUInt32();
    byte num4 = r.ReadByte();
    int num5 = (int) r.ReadByte();
    int num6 = (int) r.ReadByte();
    int num7 = (int) r.ReadByte();
    int num8 = (int) r.ReadByte();
    int num9 = (int) r.ReadBigEndianUInt32();
    double num10 = 96.0;
    double num11 = 96.0;
    while (true)
    {
      int num12 = (int) r.ReadBigEndianUInt32();
      switch (r.ReadString(4))
      {
        case "IEND":
          goto label_7;
        case "pHYs":
          if (num12 == 9)
          {
            uint num13 = r.ReadBigEndianUInt32();
            uint num14 = r.ReadBigEndianUInt32();
            int num15 = (int) r.ReadByte();
            num10 = (double) num13 * 0.0254;
            num11 = (double) num14 * 0.0254;
            break;
          }
          goto label_3;
        default:
          input.Position += (long) num12;
          break;
      }
      int num16 = (int) r.ReadBigEndianUInt32();
    }
label_3:
    throw new FormatException("Wrong length of pHYs chunk.");
label_7:
    return new OxyImageInfo()
    {
      Width = num2,
      Height = num3,
      DpiX = num10,
      DpiY = num11,
      BitsPerPixel = (int) num4
    };
  }

  public OxyColor[,] Decode(byte[] bytes)
  {
    BinaryReader r1 = new BinaryReader((Stream) new MemoryStream(bytes));
    byte[] numArray1 = r1.ReadBytes(8);
    if (numArray1[0] != (byte) 137 || numArray1[1] != (byte) 80 /*0x50*/ || numArray1[2] != (byte) 78 || numArray1[3] != (byte) 71 || numArray1[4] != (byte) 13 || numArray1[5] != (byte) 10 || numArray1[6] != (byte) 26 || numArray1[7] != (byte) 10)
      throw new FormatException("Invalid signature.");
    if (r1.ReadBigEndianUInt32() != 13U)
      throw new FormatException("Header not supported.");
    int length1 = !(r1.ReadString(4) != "IHDR") ? (int) r1.ReadBigEndianUInt32() : throw new FormatException("Invalid header.");
    int length2 = (int) r1.ReadBigEndianUInt32();
    byte num1 = r1.ReadByte();
    ColorType colorType = (ColorType) r1.ReadByte();
    CompressionMethod compressionMethod = (CompressionMethod) r1.ReadByte();
    FilterMethod filterMethod = (FilterMethod) r1.ReadByte();
    InterlaceMethod interlaceMethod = (InterlaceMethod) r1.ReadByte();
    int num2 = (int) r1.ReadBigEndianUInt32();
    if (num1 != (byte) 8)
      throw new NotImplementedException();
    if (colorType != ColorType.TrueColorWithAlpha)
      throw new NotImplementedException();
    if (compressionMethod != CompressionMethod.Deflate)
      throw new NotImplementedException();
    if (filterMethod != FilterMethod.None)
      throw new NotImplementedException();
    if (interlaceMethod != InterlaceMethod.None)
      throw new NotImplementedException();
    MemoryStream memoryStream = new MemoryStream();
    while (true)
    {
      int count = (int) r1.ReadBigEndianUInt32();
      switch (r1.ReadString(4))
      {
        case "IEND":
          goto label_24;
        case "PLTE":
          goto label_18;
        case "IDAT":
          int num3 = (int) r1.ReadByte();
          int num4 = (int) r1.ReadByte();
          byte[] bytes1 = r1.ReadBytes(count - 6);
          uint num5 = r1.ReadBigEndianUInt32();
          byte[] numArray2 = PngDecoder.Deflate(bytes1);
          if ((int) PngEncoder.Adler32((IEnumerable<byte>) numArray2) == (int) num5)
          {
            memoryStream.Write(numArray2, 0, numArray2.Length);
            break;
          }
          goto label_20;
        default:
          r1.ReadBytes(count);
          break;
      }
      int num6 = (int) r1.ReadBigEndianUInt32();
    }
label_18:
    throw new NotImplementedException();
label_20:
    throw new FormatException("Invalid checksum.");
label_24:
    OxyColor[,] oxyColorArray = new OxyColor[length1, length2];
    memoryStream.Position = 0L;
    for (int index1 = length2 - 1; index1 >= 0; --index1)
    {
      memoryStream.ReadByte();
      for (int index2 = 0; index2 < length1; ++index2)
      {
        byte r2 = (byte) memoryStream.ReadByte();
        byte g = (byte) memoryStream.ReadByte();
        byte b = (byte) memoryStream.ReadByte();
        byte a = (byte) memoryStream.ReadByte();
        oxyColorArray[index2, index1] = OxyColor.FromArgb(a, r2, g, b);
      }
    }
    if (memoryStream.Position != memoryStream.Length)
      throw new InvalidOperationException();
    return oxyColorArray;
  }

  private static byte[] Deflate(byte[] bytes) => OxyPlot.Deflate.Decompress(bytes);
}
