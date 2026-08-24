// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.LCLevelCellWidget
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class LCLevelCellWidget(
  Infralution.Controls.VirtualTree.RowWidget rowWidget,
  Column column,
  ICategoryTypeIconService objtypesIcons,
  Dictionary<int, Image> levelIcons) : TypedCellWidget(rowWidget, column, levelIcons, objtypesIcons)
{
  protected override Image GetImage(CompositionItem node, Style style)
  {
    Image image1 = base.GetImage(node, style);
    if (image1 == null)
      return (Image) null;
    Rectangle srcRect = new Rectangle(0, 0, image1.Width, image1.Height);
    using (Bitmap bitmap = new Bitmap(image1))
    {
      Bitmap image2 = new Bitmap(srcRect.Width, srcRect.Height);
      using (Graphics graphics = Graphics.FromImage((Image) image2))
        graphics.DrawImage((Image) bitmap, new Rectangle(0, 0, image2.Width, image2.Height), srcRect, GraphicsUnit.Pixel);
      return (Image) image2;
    }
  }

  protected override int categoryID => 8;

  protected override int GetTypeID(CompositionItem node) => node.Level;
}
