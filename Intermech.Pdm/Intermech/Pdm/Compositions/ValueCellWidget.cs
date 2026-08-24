// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ValueCellWidget
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class ValueCellWidget(RowWidget rowWidget, Column column) : CellWidget(rowWidget, column)
{
  protected int textBoundsShift;

  protected override string GetToolTipText()
  {
    if (this.CellData.Value != null)
    {
      string text = this.CellData.Value.ToString();
      if ((double) this.CalculateTextBounds((Control) this.Tree, text).Width + (double) this.textBoundsShift > (double) this.Bounds.Width)
        return text;
    }
    return string.Empty;
  }

  private SizeF CalculateTextBounds(Control control, string text)
  {
    using (Graphics graphics = control.CreateGraphics())
    {
      int width = Screen.PrimaryScreen.WorkingArea.Width / 100 * 50;
      return graphics.MeasureString(text, control.Font, width, StringFormat.GenericDefault);
    }
  }
}
