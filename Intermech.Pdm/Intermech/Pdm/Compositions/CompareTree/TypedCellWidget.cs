// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.TypedCellWidget
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

internal abstract class TypedCellWidget : ImagesAndTextCellWidget
{
  protected ICategoryTypeIconService iconsService;
  protected Dictionary<int, Image> cache;

  public TypedCellWidget(
    Infralution.Controls.VirtualTree.RowWidget rowWidget,
    Column column,
    Dictionary<int, Image> cache,
    ICategoryTypeIconService iconsService)
    : base(rowWidget, column)
  {
    this.iconsService = iconsService;
    this.cache = cache;
  }

  protected abstract int categoryID { get; }

  protected abstract int GetTypeID(CompositionItem node);

  protected override Image GetImage(CompositionItem node, Style style)
  {
    int typeId = this.GetTypeID(node);
    Image image1;
    if (this.cache.TryGetValue(typeId, out image1))
      return image1;
    int index = this.iconsService.IndexOf(this.categoryID, typeId);
    if (index < 0)
      return (Image) null;
    Image image2 = this.iconsService.ImageList.Images[index];
    this.cache.Add(typeId, image2);
    return image2;
  }
}
