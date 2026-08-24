// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ObjectIDCellWidget
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class ObjectIDCellWidget : ImagesAndTextCellWidget
{
  private INamedImageList _namedImageList;

  public ObjectIDCellWidget(Infralution.Controls.VirtualTree.RowWidget rowWidget, Column column, INamedImageList namedImageList)
    : base(rowWidget, column)
  {
    this._namedImageList = namedImageList;
  }

  private int VersionsImageIndex(CompositionItem node)
  {
    if (UISettings.NavigatorWindowBaseVersionsMode == NavigatorWindowBaseVersionsMode.Hidden)
      return -1;
    return node.BaseVersion == 0 ? ((UISettings.NavigatorWindowBaseVersionsMode & NavigatorWindowBaseVersionsMode.ShowOtherVersions) == NavigatorWindowBaseVersionsMode.ShowOtherVersions ? this._namedImageList.ImageIndex("imgNonBaseVersion") : this._namedImageList.ImageIndex("imgBaseVersionEmpty")) : ((UISettings.NavigatorWindowBaseVersionsMode & NavigatorWindowBaseVersionsMode.ShowBaseVersions) == NavigatorWindowBaseVersionsMode.ShowBaseVersions ? this._namedImageList.ImageIndex("imgBaseVersion") : this._namedImageList.ImageIndex("imgBaseVersionEmpty"));
  }

  protected override Image GetImage(CompositionItem node, Style style)
  {
    int index = this.VersionsImageIndex(node);
    return index == -1 ? (Image) null : this._namedImageList.ImageList.Images[index];
  }
}
