// Decompiled with JetBrains decompiler
// Type: Intermech.Workflow.Design.AutoSizeLabel
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Workflow.Design;

public class AutoSizeLabel : Label
{
  private Image _statusImage;

  protected override void OnLayout(LayoutEventArgs levent)
  {
    using (Graphics graphics = Graphics.FromHwnd(this.Handle))
    {
      Size clientSize = this.ClientSize;
      int width1 = clientSize.Width;
      Padding padding1 = this.Padding;
      int left = padding1.Left;
      int num1 = width1 - left;
      padding1 = this.Padding;
      int right = padding1.Right;
      int width2 = num1 - right;
      int height = (int) graphics.MeasureString(this.Text, this.Font, width2).Height;
      Padding padding2 = this.Padding;
      int top = padding2.Top;
      padding2 = this.Padding;
      int bottom = padding2.Bottom;
      int num2 = top + bottom;
      clientSize = this.ClientSize;
      this.ClientSize = new Size(clientSize.Width, height + num2 + 3);
    }
    base.OnLayout(levent);
  }

  [DefaultValue(null)]
  public Image StatusImage
  {
    get => this._statusImage;
    set
    {
      if (this._statusImage == value)
        return;
      this._statusImage = value;
      if (value != null)
      {
        int left = value.Width + 5;
        Padding padding = this.Padding;
        int top = padding.Top;
        padding = this.Padding;
        int right = padding.Right;
        padding = this.Padding;
        int bottom = padding.Bottom;
        this.Padding = new Padding(left, top, right, bottom);
      }
      else
      {
        int top = this.Padding.Top;
        Padding padding = this.Padding;
        int right = padding.Right;
        padding = this.Padding;
        int bottom = padding.Bottom;
        this.Padding = new Padding(0, top, right, bottom);
      }
      this.PerformLayout();
    }
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    base.OnPaint(e);
    if (this.StatusImage == null)
      return;
    Graphics graphics = e.Graphics;
    Image statusImage = this.StatusImage;
    Padding padding = this.Padding;
    int top1 = padding.Top;
    int height = this.ClientSize.Height;
    padding = this.Padding;
    int top2 = padding.Top;
    int num1 = height - top2;
    padding = this.Padding;
    int bottom = padding.Bottom;
    int num2 = (num1 - bottom) / 2;
    Point point = new Point(0, top1 + num2 - this.StatusImage.Height / 2);
    graphics.DrawImage(statusImage, point);
  }
}
