// Decompiled with JetBrains decompiler
// Type: OxyPlot.BmpEncoder
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.IO;

#nullable disable
namespace OxyPlot;

public class BmpEncoder : IImageEncoder
{
  private readonly BmpEncoderOptions options;

  public BmpEncoder(BmpEncoderOptions options) => this.options = options;

  public byte[] Encode(OxyColor[,] pixels)
  {
    int length1 = pixels.GetLength(0);
    int length2 = pixels.GetLength(1);
    byte[] buffer = new byte[length1 * length2 * 4];
    int num1 = 0;
    for (int index1 = 0; index1 < length2; ++index1)
    {
      for (int index2 = 0; index2 < length1; ++index2)
      {
        byte[] numArray1 = buffer;
        int index3 = num1;
        int num2 = index3 + 1;
        int b = (int) pixels[index2, index1].B;
        numArray1[index3] = (byte) b;
        byte[] numArray2 = buffer;
        int index4 = num2;
        int num3 = index4 + 1;
        int g = (int) pixels[index2, index1].G;
        numArray2[index4] = (byte) g;
        byte[] numArray3 = buffer;
        int index5 = num3;
        int num4 = index5 + 1;
        int r = (int) pixels[index2, index1].R;
        numArray3[index5] = (byte) r;
        byte[] numArray4 = buffer;
        int index6 = num4;
        num1 = index6 + 1;
        int a = (int) pixels[index2, index1].A;
        numArray4[index6] = (byte) a;
      }
    }
    MemoryStream output = new MemoryStream();
    BinaryWriter w = new BinaryWriter((Stream) output);
    int num5 = 54 + buffer.Length;
    w.Write((byte) 66);
    w.Write((byte) 77);
    w.Write((uint) num5);
    w.Write((ushort) 0);
    w.Write((ushort) 0);
    w.Write(54U);
    BmpEncoder.WriteBitmapInfoHeader(w, length1, length2, 32 /*0x20*/, buffer.Length, this.options.DpiX, this.options.DpiY);
    w.Write(buffer);
    return output.ToArray();
  }

  public byte[] Encode(byte[,] pixels, OxyColor[] palette)
  {
    if (palette.Length == 0)
      throw new ArgumentException("Palette not defined.", nameof (palette));
    if (palette.Length > 256 /*0x0100*/)
      throw new ArgumentException("Too many colors in the palette.", nameof (palette));
    int length1 = pixels.GetLength(0);
    int length2 = pixels.GetLength(1);
    int length3 = length1 * length2;
    MemoryStream output = new MemoryStream();
    BinaryWriter w = new BinaryWriter((Stream) output);
    int num1 = 54 + 4 * palette.Length;
    int num2 = num1 + length3;
    w.Write((byte) 66);
    w.Write((byte) 77);
    w.Write((uint) num2);
    w.Write((ushort) 0);
    w.Write((ushort) 0);
    w.Write((uint) num1);
    BmpEncoder.WriteBitmapInfoHeader(w, length1, length2, 8, length3, this.options.DpiX, this.options.DpiY, palette.Length);
    foreach (OxyColor oxyColor in palette)
    {
      w.Write(oxyColor.B);
      w.Write(oxyColor.G);
      w.Write(oxyColor.R);
      w.Write(oxyColor.A);
    }
    int num3 = (int) Math.Floor((double) (8 * length1 + 31 /*0x1F*/) / 32.0) * 4;
    for (int index1 = 0; index1 < length2; ++index1)
    {
      for (int index2 = 0; index2 < length1; ++index2)
        w.Write(pixels[index2, index1]);
      for (int index3 = length1; index3 < num3; ++index3)
        w.Write((byte) 0);
    }
    return output.ToArray();
  }

  private static void WriteBitmapInfoHeader(
    BinaryWriter w,
    int width,
    int height,
    int bitsPerPixel,
    int length,
    double dpix,
    double dpiy,
    int colors = 0)
  {
    w.Write(40U);
    w.Write((uint) width);
    w.Write((uint) height);
    w.Write((ushort) 1);
    w.Write((ushort) bitsPerPixel);
    w.Write(0U);
    w.Write((uint) length);
    w.Write((uint) (dpix / 0.0254));
    w.Write((uint) (dpiy / 0.0254));
    w.Write((uint) colors);
    w.Write((uint) colors);
  }

  private static void WriteBitmapV4Header(
    BinaryWriter w,
    int width,
    int height,
    int bitsPerPixel,
    int length,
    int dpi,
    int colors = 0)
  {
    uint num = (uint) ((double) dpi / 0.0254);
    w.Write(108U);
    w.Write((uint) width);
    w.Write((uint) height);
    w.Write((ushort) 1);
    w.Write((ushort) bitsPerPixel);
    w.Write(3U);
    w.Write((uint) length);
    w.Write(num);
    w.Write(num);
    w.Write((uint) colors);
    w.Write((uint) colors);
    w.Write(16711680 /*0xFF0000*/);
    w.Write(65280);
    w.Write((int) byte.MaxValue);
    w.Write(4278190080U /*0xFF000000*/);
    w.Write(544106839U);
    w.Write(new byte[36]);
    w.Write(0U);
    w.Write(0U);
    w.Write(0U);
  }
}
