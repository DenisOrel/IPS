// Decompiled with JetBrains decompiler
// Type: OxyPlot.PortableDocumentImage
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class PortableDocumentImage
{
  public PortableDocumentImage(
    int width,
    int height,
    int bitsPerComponent,
    byte[] bits,
    byte[] maskBits = null,
    bool interpolate = true,
    ColorSpace colorSpace = ColorSpace.DeviceRGB)
  {
    this.Width = width;
    this.Height = height;
    this.BitsPerComponent = bitsPerComponent;
    this.ColorSpace = colorSpace;
    this.Bits = bits;
    this.MaskBits = maskBits;
    this.Interpolate = interpolate;
  }

  public int Width { get; private set; }

  public int Height { get; private set; }

  public int BitsPerComponent { get; private set; }

  public ColorSpace ColorSpace { get; private set; }

  public byte[] Bits { get; private set; }

  public byte[] MaskBits { get; private set; }

  public bool Interpolate { get; private set; }
}
