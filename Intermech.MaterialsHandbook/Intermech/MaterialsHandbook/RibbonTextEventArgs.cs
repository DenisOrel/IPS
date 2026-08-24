// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonTextEventArgs
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System.Drawing;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class RibbonTextEventArgs : RibbonItemBoundsEventArgs
{
  public Color Color { get; set; }

  public StringFormat Format { get; set; }

  public FontStyle Style { get; set; }

  public string Text { get; set; }

  public RibbonTextEventArgs(
    Ribbon owner,
    Graphics g,
    Rectangle clip,
    RibbonItem item,
    Rectangle bounds,
    string text)
    : base(owner, g, clip, item, bounds)
  {
    this.Text = text;
    this.Format = new StringFormat();
    this.Style = FontStyle.Regular;
    this.Color = Color.Empty;
  }

  public RibbonTextEventArgs(
    Ribbon owner,
    Graphics g,
    Rectangle clip,
    RibbonItem item,
    Rectangle bounds,
    string text,
    FontStyle style)
    : base(owner, g, clip, item, bounds)
  {
    this.Text = text;
    this.Style = style;
    this.Format = new StringFormat();
    this.Color = Color.Empty;
  }

  public RibbonTextEventArgs(
    Ribbon owner,
    Graphics g,
    Rectangle clip,
    RibbonItem item,
    Rectangle bounds,
    string text,
    StringFormat format)
    : base(owner, g, clip, item, bounds)
  {
    this.Text = text;
    this.Format = format;
    this.Style = FontStyle.Regular;
    this.Color = Color.Empty;
  }

  public RibbonTextEventArgs(
    Ribbon owner,
    Graphics g,
    Rectangle clip,
    RibbonItem item,
    Rectangle bounds,
    string text,
    Color color,
    FontStyle style,
    StringFormat format)
    : base(owner, g, clip, item, bounds)
  {
    this.Text = text;
    this.Style = style;
    this.Format = format;
    this.Color = color;
  }
}
