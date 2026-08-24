// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.HeaderDescription
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class HeaderDescription
{
  private Font _font;
  private Size _textSize = Size.Empty;
  private Rectangle _bounds = Rectangle.Empty;
  private Padding _padding = Padding.Empty;

  public int ArrowWidth { get; private set; }

  public Rectangle Bounds
  {
    get => this._bounds;
    set
    {
      this._bounds = value;
      this.CalcHeight();
    }
  }

  public string HeaderEdit
  {
    get
    {
      string text = this.Text;
      this.Text = string.Empty;
      return text;
    }
  }

  public Font Font
  {
    get => this._font;
    set => this._font = value;
  }

  public string Text { get; private set; }

  public Rectangle TextBounds { get; private set; }

  public bool Visible { get; set; }

  public bool ReadOnly { get; set; }

  public HeaderDescription()
  {
    this.ArrowWidth = 0;
    this.TextBounds = Rectangle.Empty;
    this.Visible = true;
  }

  public HeaderDescription(Font font, string text, SizeF autoScaleFactor)
    : this()
  {
    this._font = font;
    this.SetText(text);
    this.CalcPadding(autoScaleFactor);
  }

  public void CalcPadding(SizeF autoScaleFactor)
  {
    int int32_1 = Convert.ToInt32(Math.Ceiling(3.0 * (double) autoScaleFactor.Width));
    int int32_2 = Convert.ToInt32(Math.Ceiling(3.0 * (double) autoScaleFactor.Height));
    int num1 = int32_1 < 0 ? 0 : int32_1;
    int num2 = int32_2 < 0 ? 0 : int32_2;
    this._padding = new Padding(num1, num2, num1, num2);
    this.ArrowWidth = Convert.ToInt32(20f * autoScaleFactor.Width);
  }

  private void CalcHeight()
  {
    int x = this._bounds.X + this._padding.Left + this.ArrowWidth;
    int width = this._bounds.Width - this.ArrowWidth - this._padding.Horizontal;
    int int32 = Convert.ToInt32(Math.Ceiling((double) this._textSize.Width / (double) width));
    this.TextBounds = new Rectangle(x, this._bounds.Y + this._padding.Top, width, this._textSize.Height * int32);
    this._bounds.Height = this.TextBounds.Height + this._padding.Vertical;
  }

  public void SetText(string text)
  {
    this.Text = text;
    if (string.IsNullOrEmpty(text))
      text = "W";
    this._textSize = TextRenderer.MeasureText(text, this._font);
  }
}
