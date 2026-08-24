// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxyImage
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.IO;

#nullable disable
namespace OxyPlot;

public class OxyImage
{
  private readonly byte[] data;
  private OxyColor[,] pixels;

  public OxyImage(Stream s)
    : this(OxyImage.GetBytes(s))
  {
  }

  public OxyImage(byte[] bytes)
  {
    this.data = bytes;
    this.Format = OxyImage.GetImageFormat(bytes);
    this.UpdateImageInfo();
  }

  public ImageFormat Format { get; private set; }

  public int Width { get; private set; }

  public int Height { get; private set; }

  public int BitsPerPixel { get; private set; }

  public double DpiX { get; private set; }

  public double DpiY { get; private set; }

  public static OxyImage Create(
    byte[,] pixels,
    OxyColor[] palette,
    ImageFormat format,
    ImageEncoderOptions encoderOptions = null)
  {
    return new OxyImage(OxyImage.GetEncoder(format, encoderOptions).Encode(pixels, palette));
  }

  public static OxyImage Create(
    OxyColor[,] pixels,
    ImageFormat format,
    ImageEncoderOptions encoderOptions = null)
  {
    return new OxyImage(OxyImage.GetEncoder(format, encoderOptions).Encode(pixels))
    {
      pixels = pixels
    };
  }

  public byte[] GetData() => this.data;

  public OxyColor[,] GetPixels()
  {
    return this.pixels != null ? this.pixels : OxyImage.GetDecoder(this.Format).Decode(this.data);
  }

  private static IImageDecoder GetDecoder(ImageFormat format)
  {
    switch (format)
    {
      case ImageFormat.Png:
        return (IImageDecoder) new PngDecoder();
      case ImageFormat.Bmp:
        return (IImageDecoder) new BmpDecoder();
      case ImageFormat.Jpeg:
        throw new NotImplementedException();
      default:
        throw new InvalidOperationException("Image format not supported");
    }
  }

  private static IImageEncoder GetEncoder(ImageFormat format, ImageEncoderOptions encoderOptions)
  {
    switch (format)
    {
      case ImageFormat.Png:
        if (encoderOptions == null)
          encoderOptions = (ImageEncoderOptions) new PngEncoderOptions();
        return encoderOptions is PngEncoderOptions ? (IImageEncoder) new PngEncoder((PngEncoderOptions) encoderOptions) : throw new ArgumentException(nameof (encoderOptions));
      case ImageFormat.Bmp:
        if (encoderOptions == null)
          encoderOptions = (ImageEncoderOptions) new BmpEncoderOptions();
        return encoderOptions is BmpEncoderOptions ? (IImageEncoder) new BmpEncoder((BmpEncoderOptions) encoderOptions) : throw new ArgumentException(nameof (encoderOptions));
      case ImageFormat.Jpeg:
        throw new NotImplementedException();
      default:
        throw new InvalidOperationException("Image format not supported");
    }
  }

  private static ImageFormat GetImageFormat(byte[] bytes)
  {
    if (bytes.Length >= 2 && bytes[0] == byte.MaxValue && bytes[1] == (byte) 216)
      return ImageFormat.Jpeg;
    if (bytes.Length >= 2 && bytes[0] == (byte) 66 && bytes[1] == (byte) 77)
      return ImageFormat.Bmp;
    return bytes.Length >= 4 && bytes[0] == (byte) 137 && bytes[1] == (byte) 80 /*0x50*/ && bytes[2] == (byte) 78 && bytes[3] == (byte) 71 ? ImageFormat.Png : ImageFormat.Unknown;
  }

  private static byte[] GetBytes(Stream s)
  {
    using (MemoryStream destination = new MemoryStream())
    {
      s.CopyTo((Stream) destination);
      return destination.ToArray();
    }
  }

  private void UpdateImageInfo()
  {
    OxyImageInfo imageInfo = OxyImage.GetDecoder(this.Format).GetImageInfo(this.data);
    if (imageInfo == null)
      return;
    this.Width = imageInfo.Width;
    this.Height = imageInfo.Height;
    this.BitsPerPixel = imageInfo.BitsPerPixel;
    this.DpiX = imageInfo.DpiX;
    this.DpiY = imageInfo.DpiY;
  }
}
