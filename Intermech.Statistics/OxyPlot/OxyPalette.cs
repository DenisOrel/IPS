// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxyPalette
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot;

public class OxyPalette
{
  public OxyPalette() => this.Colors = (IList<OxyColor>) new List<OxyColor>();

  public OxyPalette(params OxyColor[] colors)
  {
    this.Colors = (IList<OxyColor>) new List<OxyColor>((IEnumerable<OxyColor>) colors);
  }

  public OxyPalette(IEnumerable<OxyColor> colors)
  {
    this.Colors = (IList<OxyColor>) new List<OxyColor>(colors);
  }

  public IList<OxyColor> Colors { get; set; }

  public static OxyPalette Interpolate(int paletteSize, params OxyColor[] colors)
  {
    OxyColor[] oxyColorArray = new OxyColor[paletteSize];
    for (int index1 = 0; index1 < paletteSize; ++index1)
    {
      double num = (double) index1 / (double) (paletteSize - 1) * (double) (colors.Length - 1);
      int index2 = (int) num;
      int index3 = index2 + 1 < colors.Length ? index2 + 1 : index2;
      oxyColorArray[index1] = OxyColor.Interpolate(colors[index2], colors[index3], num - (double) index2);
    }
    return new OxyPalette(oxyColorArray);
  }

  public OxyPalette Reverse() => new OxyPalette(this.Colors.Reverse<OxyColor>());
}
