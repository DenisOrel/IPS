// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CheckOutByCellWidget
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class CheckOutByCellWidget : ImagesAndTextCellWidget
{
  private INamedImageList _namedImageList;
  private long _currentUserID;

  public CheckOutByCellWidget(
    Infralution.Controls.VirtualTree.RowWidget rowWidget,
    Column column,
    INamedImageList namedImageList,
    long currentUserID)
    : base(rowWidget, column)
  {
    this._namedImageList = namedImageList;
    this.enableText = false;
    this._currentUserID = currentUserID;
  }

  protected override string GetToolTipText()
  {
    return this.CellData.Value != null ? string.Format($"Объект взят на изменение пользователем {this.CellData.Value}") : string.Empty;
  }

  protected override Image GetImage(CompositionItem node, Style style)
  {
    return node != null && node.CheckOut != 0L ? this._namedImageList.ImageList.Images[this._namedImageList.ImageIndex(node.CheckOut != this._currentUserID ? "imgUserOther" : "imgUserCurrent")] : (Image) null;
  }
}
