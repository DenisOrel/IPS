// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxyPalettes
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public static class OxyPalettes
{
  static OxyPalettes()
  {
    OxyPalettes.BlueWhiteRed31 = OxyPalettes.BlueWhiteRed(31 /*0x1F*/);
    OxyPalettes.Hot64 = OxyPalettes.Hot(64 /*0x40*/);
    OxyPalettes.Hue64 = OxyPalettes.Hue(64 /*0x40*/);
  }

  public static OxyPalette BlueWhiteRed31 { get; private set; }

  public static OxyPalette Hot64 { get; private set; }

  public static OxyPalette Hue64 { get; private set; }

  public static OxyPalette BlackWhiteRed(int numberOfColors)
  {
    return OxyPalette.Interpolate(numberOfColors, OxyColors.Black, OxyColors.White, OxyColors.Red);
  }

  public static OxyPalette BlueWhiteRed(int numberOfColors)
  {
    return OxyPalette.Interpolate(numberOfColors, OxyColors.Blue, OxyColors.White, OxyColors.Red);
  }

  public static OxyPalette Cool(int numberOfColors)
  {
    return OxyPalette.Interpolate(numberOfColors, OxyColors.Cyan, OxyColors.Magenta);
  }

  public static OxyPalette Gray(int numberOfColors)
  {
    return OxyPalette.Interpolate(numberOfColors, OxyColors.Black, OxyColors.White);
  }

  public static OxyPalette Hot(int numberOfColors)
  {
    return OxyPalette.Interpolate(numberOfColors, OxyColors.Black, OxyColor.FromRgb((byte) 127 /*0x7F*/, (byte) 0, (byte) 0), OxyColor.FromRgb(byte.MaxValue, (byte) 127 /*0x7F*/, (byte) 0), OxyColor.FromRgb(byte.MaxValue, byte.MaxValue, (byte) 127 /*0x7F*/), OxyColors.White);
  }

  public static OxyPalette Hue(int numberOfColors)
  {
    return OxyPalette.Interpolate(numberOfColors, OxyColors.Red, OxyColors.Yellow, OxyColors.Green, OxyColors.Cyan, OxyColors.Blue, OxyColors.Magenta, OxyColors.Red);
  }

  public static OxyPalette HueDistinct(int numberOfColors)
  {
    return OxyPalette.Interpolate(numberOfColors, OxyColors.Magenta, OxyColors.Blue, OxyColors.Cyan, OxyColors.Green, OxyColors.Yellow, OxyColors.Red);
  }

  public static OxyPalette Jet(int numberOfColors)
  {
    return OxyPalette.Interpolate(numberOfColors, OxyColors.DarkBlue, OxyColors.Cyan, OxyColors.Yellow, OxyColors.Orange, OxyColors.DarkRed);
  }

  public static OxyPalette Rainbow(int numberOfColors)
  {
    return OxyPalette.Interpolate(numberOfColors, OxyColors.Violet, OxyColors.Indigo, OxyColors.Blue, OxyColors.Green, OxyColors.Yellow, OxyColors.Orange, OxyColors.Red);
  }
}
