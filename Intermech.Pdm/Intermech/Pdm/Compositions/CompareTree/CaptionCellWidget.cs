// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CaptionCellWidget
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.DBObjects;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class CaptionCellWidget : ImagesAndTextCellWidget
{
  private ICategoryTypeIconService _iconsService;
  private Dictionary<int, Image> _objTypesIcons;

  public CaptionCellWidget(
    Infralution.Controls.VirtualTree.RowWidget rowWidget,
    Column column,
    Dictionary<int, Image> objTypesIcons,
    ICategoryTypeIconService iconsService)
    : base(rowWidget, column)
  {
    this._iconsService = iconsService;
    this._objTypesIcons = objTypesIcons;
  }

  protected override string GetToolTipText()
  {
    if (this.Row != null)
    {
      CompositionItem fromRow = this.GetFromRow(this.Row);
      if (fromRow != null && !fromRow.Empty)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Тип объекта: {MetaDataHelper.GetObjectTypeName(fromRow.ObjectTypeID)}");
        stringBuilder.AppendLine($"Заголовок: {fromRow.Caption}");
        stringBuilder.AppendLine($"Номер версии: {fromRow.Version}");
        stringBuilder.AppendLine($"Базовая версия: {(fromRow.BaseVersion == 1 ? (object) "да" : (object) "нет")}");
        stringBuilder.AppendLine($"Идентификатор версии объекта: {fromRow.ObjectID}");
        stringBuilder.AppendLine($"Идентификатор объекта:  {fromRow.ID}");
        return stringBuilder.ToString();
      }
    }
    return string.Empty;
  }

  protected override Image GetImage(CompositionItem node, Style style)
  {
    return this.GetObjectTypeImage(node);
  }

  private Image GetObjectTypeImage(CompositionItem node)
  {
    Image objectTypeImage;
    if (this._objTypesIcons.TryGetValue(node.ObjectTypeID, out objectTypeImage))
      return objectTypeImage;
    Image image = this._iconsService.ImageList.Images[this._iconsService.IndexOf(4, node.ObjectTypeID)];
    this._objTypesIcons.Add(node.ObjectTypeID, image);
    return image;
  }

  protected override string GetText(CompositionItem node)
  {
    return CaptionTransform.GetCaption(this.Text, (long) node.Version);
  }
}
