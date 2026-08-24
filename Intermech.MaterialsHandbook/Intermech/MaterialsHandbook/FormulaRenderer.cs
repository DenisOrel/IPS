// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.FormulaRenderer
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Drawing;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class FormulaRenderer
{
  private string _strMiddle = string.Empty;
  private string _strTop = string.Empty;
  private string _strBottom = string.Empty;

  private void Clear()
  {
    this._strMiddle = string.Empty;
    this._strTop = string.Empty;
    this._strBottom = string.Empty;
  }

  private void DrawOneLine(Graphics g, Font f, Size clientSize)
  {
    SizeF sizeF = g.MeasureString(this._strMiddle, f);
    int width = (int) sizeF.Width;
    int height = (int) sizeF.Height;
    if (width <= 0 || height <= 0)
      return;
    int x = clientSize.Width > width ? clientSize.Width / 2 - width / 2 : 1;
    int y = clientSize.Height > height ? clientSize.Height / 2 - height / 2 : 1;
    using (SolidBrush solidBrush = new SolidBrush(SystemColors.WindowText))
      g.DrawString(this._strMiddle, f, (Brush) solidBrush, (float) x, (float) y);
  }

  private void DarwTwoLine(Graphics g, Font f, Size clientSize)
  {
    SizeF sizeF1 = g.MeasureString(this._strMiddle + this._strTop, f);
    SizeF sizeF2 = g.MeasureString(this._strMiddle + this._strBottom, f);
    SizeF sizeF3 = g.MeasureString(this._strMiddle, f);
    SizeF sizeF4 = g.MeasureString(this._strTop, f);
    SizeF sizeF5 = g.MeasureString(this._strBottom, f);
    int num1 = (double) sizeF1.Width > (double) sizeF2.Width ? (int) sizeF1.Width : (int) sizeF2.Width;
    int num2 = (int) ((double) sizeF1.Height + (double) sizeF2.Height + 5.0);
    int x1 = clientSize.Width > num1 ? clientSize.Width / 2 - num1 / 2 : 1;
    int y1 = clientSize.Height > num2 ? clientSize.Height / 2 - (int) sizeF3.Height / 2 : 1;
    int x1_1 = x1 + (int) sizeF3.Width;
    int num3 = y1 + (int) sizeF3.Height / 2;
    int num4 = num1 - (int) sizeF3.Width;
    int x2 = x1_1 + num4 / 2 - (int) sizeF4.Width / 2;
    int y2 = num3 - 2 - (int) sizeF4.Height;
    int x3 = x1_1 + num4 / 2 - (int) sizeF5.Width / 2;
    int y3 = num3 + 2;
    using (SolidBrush solidBrush = new SolidBrush(SystemColors.WindowText))
    {
      g.DrawString(this._strMiddle, f, (Brush) solidBrush, (float) x1, (float) y1);
      g.DrawString(this._strTop, f, (Brush) solidBrush, (float) x2, (float) y2);
      g.DrawString(this._strBottom, f, (Brush) solidBrush, (float) x3, (float) y3);
    }
    using (Pen pen = new Pen(SystemColors.WindowText))
      g.DrawLine(pen, x1_1, num3, x1_1 + num4, num3);
  }

  public void Draw(Graphics g, Font f, Size clientSize)
  {
    if (string.IsNullOrEmpty(this._strBottom))
      this.DrawOneLine(g, f, clientSize);
    else
      this.DarwTwoLine(g, f, clientSize);
  }

  public void SetData(string classValue, string formula)
  {
    this.Clear();
    if (string.IsNullOrEmpty(formula))
      return;
    this._strMiddle = formula;
    if (formula.Contains("/"))
    {
      int length = formula.IndexOf("/", StringComparison.Ordinal);
      string str = formula.Substring(0, length);
      this._strBottom = formula.Substring(length + 1);
      if (string.IsNullOrEmpty(this._strBottom))
        this._strMiddle = str;
      else if (!string.IsNullOrEmpty(classValue) && str.Contains(classValue))
      {
        this._strMiddle = classValue + "  ";
        this._strTop = str.Replace(classValue, string.Empty).Trim();
      }
      else
        this._strTop = str;
    }
    this._strMiddle = this._strMiddle.Replace('\\', '/');
    this._strTop = this._strTop.Replace('\\', '/');
    this._strBottom = this._strBottom.Replace('\\', '/');
  }
}
