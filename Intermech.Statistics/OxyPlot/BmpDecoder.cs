// Decompiled with JetBrains decompiler
// Type: OxyPlot.BmpDecoder
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.IO;

#nullable disable
namespace OxyPlot;

public class BmpDecoder : IImageDecoder
{
  public OxyImageInfo GetImageInfo(byte[] bytes)
  {
    BinaryReader binaryReader = new BinaryReader((Stream) new MemoryStream(bytes));
    binaryReader.ReadBytes(2);
    int num1 = (int) binaryReader.ReadUInt32();
    binaryReader.ReadBytes(4);
    int num2 = (int) binaryReader.ReadUInt32();
    int num3 = (int) binaryReader.ReadUInt32();
    int num4 = binaryReader.ReadInt32();
    int num5 = binaryReader.ReadInt32();
    int num6 = (int) binaryReader.ReadInt16();
    short num7 = binaryReader.ReadInt16();
    binaryReader.ReadInt32();
    binaryReader.ReadInt32();
    int num8 = binaryReader.ReadInt32();
    int num9 = binaryReader.ReadInt32();
    binaryReader.ReadInt32();
    binaryReader.ReadInt32();
    return new OxyImageInfo()
    {
      Width = num4,
      Height = num5,
      DpiX = (double) num8 * 0.0254,
      DpiY = (double) num9 * 0.0254,
      BitsPerPixel = (int) num7
    };
  }

  public OxyColor[,] Decode(byte[] bytes) => throw new NotImplementedException();
}
