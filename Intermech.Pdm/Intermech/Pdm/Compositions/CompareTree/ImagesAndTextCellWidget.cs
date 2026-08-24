// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ImagesAndTextCellWidget
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Interfaces.Pdm;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal abstract class ImagesAndTextCellWidget : CompareCellWidget
{
  protected bool enableText = true;

  public ImagesAndTextCellWidget(Infralution.Controls.VirtualTree.RowWidget rowWidget, Column column)
    : base(rowWidget, column)
  {
  }

  protected abstract Image GetImage(CompositionItem node, Style style);

  protected override void PaintForeground(Graphics graphics, Style style, bool printing)
  {
    if (this.Row == null)
      return;
    CompositionItem fromRow = this.GetFromRow(this.Row);
    if (fromRow == null || fromRow.Empty)
      return;
    Rectangle textBounds = this.GetTextBounds();
    Image image = this.GetImage(fromRow, style);
    if (image != null)
    {
      Rectangle rect = new Rectangle(this.Bounds.X + 1, this.Bounds.Y + (this.Bounds.Height - 16 /*0x10*/) / 2, image.Width, image.Height);
      graphics.DrawImage(image, rect);
      this.textBoundsShift = rect.Width + 2;
      textBounds.X += this.textBoundsShift;
      textBounds.Width -= this.textBoundsShift;
    }
    if (!this.enableText)
      return;
    string text = this.GetText(fromRow);
    if (this.Tree.SelectionMode == SelectionMode.MainCellText && this.Row.Selected && this.RowWidget.MainColumn == this.Column && !printing)
    {
      style = this.GetSelectedStyle();
      Rectangle actualTextBounds = this.GetActualTextBounds(graphics, textBounds, style, text);
      this.PaintSelectedTextBackground(graphics, actualTextBounds, style);
    }
    this.PaintText(graphics, textBounds, style, text);
  }

  protected virtual string GetText(CompositionItem node) => this.Text;
}
