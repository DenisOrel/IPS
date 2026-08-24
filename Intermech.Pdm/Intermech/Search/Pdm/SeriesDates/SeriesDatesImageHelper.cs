// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.SeriesDates.SeriesDatesImageHelper
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.SeriesDates;

public static class SeriesDatesImageHelper
{
  public static readonly ImageList ImageList = new ImageList();

  static SeriesDatesImageHelper()
  {
    SeriesDatesImageHelper.ImageList.Images.Add("Series_16x16.png", SeriesDatesImageHelper.GetImage("Series_16x16.png"));
    SeriesDatesImageHelper.ImageList.Images.Add("Dates_16x16.png", SeriesDatesImageHelper.GetImage("Dates_16x16.png"));
  }

  private static Image GetImage(string name)
  {
    return Image.FromStream(typeof (SeriesDatesImageHelper).Assembly.GetManifestResourceStream("Intermech.Pdm.Intermech.Search.Pdm.SeriesDates.Images." + name));
  }
}
