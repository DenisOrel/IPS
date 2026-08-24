// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.ImageHolder
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Controls;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

internal static class ImageHolder
{
  public static INamedImageList _namedImageList;
  public static ICategoryTypeIconService _categoryImages;
  public static Dictionary<int, Image> _fixedTypeImages;
  public static Dictionary<int, int> _lcLevelImgIndexes;
  private static readonly Color emptyColor = Color.FromArgb(0, 0, 0, 0);

  static ImageHolder()
  {
    ImageHolder._namedImageList = ServicesManager.GetService<INamedImageList>();
    ImageHolder._categoryImages = ServicesManager.GetService<ICategoryTypeIconService>();
    ImageHolder._fixedTypeImages = new Dictionary<int, Image>();
    ImageHolder._lcLevelImgIndexes = new Dictionary<int, int>();
  }

  public static Icon GetTypeIcon(int objTypeId)
  {
    return ImageHolder._categoryImages.GetIcon(4, objTypeId);
  }

  public static Image GetTypeImage(int objTypeId)
  {
    Image typeImage1 = (Image) null;
    if (ImageHolder._fixedTypeImages.TryGetValue(objTypeId, out typeImage1))
      return typeImage1;
    Bitmap typeImage2 = ImageHolder.Get8StripBitmap(ImageHolder.ConvertImage(Images32x16_Cache.GetImage32x16(4, objTypeId, (NavigatorTreeNode) null)) as Bitmap);
    ImageHolder._fixedTypeImages.Add(objTypeId, (Image) typeImage2);
    return (Image) typeImage2;
  }

  internal static Image ConvertIcon(Icon icon)
  {
    Bitmap bitmap = icon.ToBitmap();
    return icon.Height > 16 /*0x10*/ ? (Image) new Bitmap((Image) bitmap, 16 /*0x10*/, 16 /*0x10*/) : (Image) bitmap;
  }

  internal static Image ConvertImage(Image img)
  {
    Bitmap bmp = img as Bitmap;
    return ImageHolder.RightSideEmpty(bmp) ? (Image) bmp.Clone(new Rectangle(0, 0, 16 /*0x10*/, 16 /*0x10*/), PixelFormat.Undefined) : (Image) bmp;
  }

  internal static Bitmap Get8StripBitmap(Bitmap bmp)
  {
    Bitmap bitmap = new Bitmap(bmp.Width + 8, bmp.Height, bmp.PixelFormat);
    using (Graphics graphics = Graphics.FromImage((Image) bitmap))
      graphics.DrawImage((Image) bmp, new Point(8, 0));
    return bitmap;
  }

  internal static bool RightSideEmpty(Bitmap bmp)
  {
    for (int x = 16 /*0x10*/; x < 32 /*0x20*/; ++x)
    {
      for (int y = 0; y < 16 /*0x10*/; ++y)
      {
        if (!bmp.GetPixel(x, y).Equals((object) ImageHolder.emptyColor))
          return false;
      }
    }
    return true;
  }

  public static Image GetLCLevelImage(int lcLevelId)
  {
    int index;
    if (!ImageHolder._lcLevelImgIndexes.TryGetValue(lcLevelId, out index))
    {
      index = ImageHolder._categoryImages.IndexOf(8, lcLevelId);
      ImageHolder._lcLevelImgIndexes.Add(lcLevelId, index);
    }
    return index >= 0 ? ImageHolder._categoryImages.ImageList.Images[index] : (Image) null;
  }
}
