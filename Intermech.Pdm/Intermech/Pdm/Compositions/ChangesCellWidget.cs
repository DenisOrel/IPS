// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ChangesCellWidget
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Interfaces.Pdm;
using Intermech.Pdm.Compositions.CompareTree;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class ChangesCellWidget : ImagesAndTextCellWidget
{
  private Dictionary<CompositionItemFlags, Image> _statusIcons;

  public ChangesCellWidget(
    Infralution.Controls.VirtualTree.RowWidget rowWidget,
    Column column,
    Dictionary<CompositionItemFlags, Image> statusIcons)
    : base(rowWidget, column)
  {
    this._statusIcons = statusIcons;
  }

  protected override string GetToolTipText()
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (this.Row != null)
    {
      CompositionItem fromRow = this.GetFromRow(this.Row);
      if ((fromRow.CompositionItemFlag & CompositionItemFlags.AnotherVersion) == CompositionItemFlags.AnotherVersion)
        stringBuilder.AppendLine("Изменение версии");
      if ((fromRow.CompositionItemFlag & CompositionItemFlags.ChangedInComposition) == CompositionItemFlags.ChangedInComposition)
        stringBuilder.AppendLine("Изменения в составе");
      if ((fromRow.CompositionItemFlag & CompositionItemFlags.AttributesChangedInCompositionObject) == CompositionItemFlags.AttributesChangedInCompositionObject)
        stringBuilder.AppendLine("Изменения объекта в составе");
    }
    return stringBuilder.ToString();
  }

  protected override Image GetImage(CompositionItem node, Style style)
  {
    Image statusImage = this.GetStatusImage(node, CompositionItemFlags.AnotherVersion);
    Image image1 = this.GetStatusImage(node, CompositionItemFlags.ChangedInComposition) ?? this.GetStatusImage(node, CompositionItemFlags.AttributesChangedInCompositionObject);
    if (statusImage != null && image1 == null)
      return statusImage;
    if (image1 != null && statusImage == null)
      return image1;
    if (statusImage == null || image1 == null)
      return (Image) null;
    Bitmap image2 = new Bitmap(Math.Max(statusImage.Width, image1.Width) * 2, Math.Max(statusImage.Height, image1.Height));
    Graphics graphics = Graphics.FromImage((Image) image2);
    graphics.DrawImage(statusImage, 0, 0);
    graphics.DrawImage(image1, statusImage.Width, 0);
    return (Image) image2;
  }

  private Image GetStatusImage(CompositionItem node, CompositionItemFlags flag)
  {
    return (node.CompositionItemFlag & flag) == flag ? this._statusIcons[flag] : (Image) null;
  }
}
