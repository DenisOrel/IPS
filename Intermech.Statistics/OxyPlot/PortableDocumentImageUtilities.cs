// Decompiled with JetBrains decompiler
// Type: OxyPlot.PortableDocumentImageUtilities
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public static class PortableDocumentImageUtilities
{
  public static PortableDocumentImage Convert(OxyImage image, bool interpolate)
  {
    OxyColor[,] pixels;
    try
    {
      pixels = image.GetPixels();
    }
    catch
    {
      return (PortableDocumentImage) null;
    }
    byte[] bits = new byte[image.Width * image.Height * 3];
    byte[] maskBits = new byte[image.Width * image.Height];
    int num1 = 0;
    int num2 = 0;
    for (int index1 = 0; index1 < image.Height; ++index1)
    {
      for (int index2 = 0; index2 < image.Width; ++index2)
      {
        maskBits[num2++] = pixels[index2, index1].A;
        byte[] numArray1 = bits;
        int index3 = num1;
        int num3 = index3 + 1;
        int r = (int) pixels[index2, index1].R;
        numArray1[index3] = (byte) r;
        byte[] numArray2 = bits;
        int index4 = num3;
        int num4 = index4 + 1;
        int g = (int) pixels[index2, index1].G;
        numArray2[index4] = (byte) g;
        byte[] numArray3 = bits;
        int index5 = num4;
        num1 = index5 + 1;
        int b = (int) pixels[index2, index1].B;
        numArray3[index5] = (byte) b;
      }
    }
    return new PortableDocumentImage(image.Width, image.Height, 8, bits, maskBits, interpolate);
  }
}
