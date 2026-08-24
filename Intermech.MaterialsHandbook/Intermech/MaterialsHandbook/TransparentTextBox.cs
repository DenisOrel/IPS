// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.TransparentTextBox
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class TransparentTextBox : TextBox
{
  private TransparentTextBox.uPictureBox _pictBox = new TransparentTextBox.uPictureBox();
  private Bitmap _bmp;
  private Bitmap _alphaBmp;
  private Color _backColor = Color.White;
  private bool _upToDate;
  private bool _caretUpToDate;
  private bool _paintedFirstTime;
  private bool _caretState = true;
  private int _fontHeight = 10;
  private Timer _timer;
  private System.ComponentModel.Container components;

  public new Color BackColor
  {
    get
    {
      Color backColor = base.BackColor;
      int r = (int) backColor.R;
      backColor = base.BackColor;
      int g = (int) backColor.G;
      backColor = base.BackColor;
      int b = (int) backColor.B;
      return Color.FromArgb(r, g, b);
    }
    set
    {
      this._backColor = value;
      base.BackColor = value;
      this._upToDate = false;
    }
  }

  public TransparentTextBox()
  {
    this.InitializeComponent();
    this.BorderStyle = BorderStyle.None;
    this.Multiline = true;
    this.SetStyle(ControlStyles.UserPaint, false);
    this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
    this.Controls.Add((Control) this._pictBox);
    this._pictBox.Dock = DockStyle.Fill;
  }

  protected override void OnChangeUICues(UICuesEventArgs e)
  {
    base.OnChangeUICues(e);
    this._upToDate = false;
    this.Invalidate();
  }

  protected override void OnFontChanged(EventArgs e)
  {
    if (this._paintedFirstTime)
      this.SetStyle(ControlStyles.UserPaint, false);
    base.OnFontChanged(e);
    if (this._paintedFirstTime)
      this.SetStyle(ControlStyles.UserPaint, true);
    this._fontHeight = TextRenderer.MeasureText("X", this.Font).Height;
    this._upToDate = false;
    this.Invalidate();
  }

  protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
  {
    base.OnGiveFeedback(gfbevent);
    this._upToDate = false;
    this.Invalidate();
  }

  protected override void OnGotFocus(EventArgs e)
  {
    base.OnGotFocus(e);
    this._caretUpToDate = this._upToDate = false;
    this.Invalidate();
    this._timer = new Timer((IContainer) this.components)
    {
      Interval = (int) win32.GetCaretBlinkTime()
    };
    this._timer.Tick += new EventHandler(this.OnTimer_Tick);
    this._timer.Enabled = true;
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    base.OnKeyDown(e);
    this._upToDate = false;
    this.Invalidate();
  }

  protected override void OnKeyPress(KeyPressEventArgs e)
  {
    base.OnKeyPress(e);
    this._upToDate = false;
    this.Invalidate();
  }

  protected override void OnKeyUp(KeyEventArgs e)
  {
    base.OnKeyUp(e);
    this._upToDate = false;
    this.Invalidate();
  }

  protected override void OnLostFocus(EventArgs e)
  {
    base.OnLostFocus(e);
    this._caretUpToDate = false;
    this._upToDate = false;
    this.Invalidate();
    this._timer.Dispose();
  }

  protected override void OnMouseLeave(EventArgs e)
  {
    Point position = Cursor.Position;
    if (this.Bounds.Contains(this.FindForm().PointToClient(position)))
      return;
    base.OnMouseLeave(e);
  }

  protected override void OnResize(EventArgs e)
  {
    base.OnResize(e);
    Rectangle clientRectangle1 = this.ClientRectangle;
    int width1 = clientRectangle1.Width;
    clientRectangle1 = this.ClientRectangle;
    int height1 = clientRectangle1.Height;
    this._bmp = new Bitmap(width1, height1);
    Rectangle clientRectangle2 = this.ClientRectangle;
    int width2 = clientRectangle2.Width;
    clientRectangle2 = this.ClientRectangle;
    int height2 = clientRectangle2.Height;
    this._alphaBmp = new Bitmap(width2, height2);
    this._upToDate = false;
    this.Invalidate();
  }

  protected override void OnTextChanged(EventArgs e)
  {
    base.OnTextChanged(e);
    this._upToDate = false;
    this.Invalidate();
  }

  protected override void WndProc(ref Message m)
  {
    base.WndProc(ref m);
    if (m.Msg == 15)
    {
      this._paintedFirstTime = true;
      if (!this._upToDate || !this._caretUpToDate)
        this.GetBitmaps();
      this._upToDate = this._caretUpToDate = true;
      if (this._pictBox.Image != null)
        this._pictBox.Image.Dispose();
      this._pictBox.Image = (Image) this._alphaBmp.Clone();
    }
    else if (m.Msg == 276 || m.Msg == 277)
    {
      this._upToDate = false;
      this.Invalidate();
    }
    else if (m.Msg == 513 || m.Msg == 516 || m.Msg == 515)
    {
      this._upToDate = false;
      this.Invalidate();
    }
    else
    {
      if (m.Msg != 512 /*0x0200*/ || m.WParam.ToInt32() == 0)
        return;
      this._upToDate = false;
      this.Invalidate();
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void GetBitmaps()
  {
    if (this._bmp == null || this._alphaBmp == null || this._bmp.Width != this.Width || this._bmp.Height != this.Height || this._alphaBmp.Width != this.Width || this._alphaBmp.Height != this.Height)
    {
      this._bmp = (Bitmap) null;
      this._alphaBmp = (Bitmap) null;
    }
    if (this._bmp == null)
    {
      this._bmp = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
      this._upToDate = false;
    }
    if (!this._upToDate)
    {
      this.SetStyle(ControlStyles.UserPaint, false);
      win32.CaptureWindow((Control) this, ref this._bmp);
      this.SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
      this.BackColor = Color.FromArgb(0, this._backColor);
    }
    Rectangle rectangle;
    ref Rectangle local = ref rectangle;
    Rectangle clientRectangle = this.ClientRectangle;
    int width1 = clientRectangle.Width;
    clientRectangle = this.ClientRectangle;
    int height1 = clientRectangle.Height;
    local = new Rectangle(0, 0, width1, height1);
    using (ImageAttributes imageAttributes = new ImageAttributes())
    {
      ColorMap[] map = new ColorMap[1]{ new ColorMap() };
      map[0].OldColor = Color.FromArgb((int) byte.MaxValue, this._backColor);
      map[0].NewColor = Color.FromArgb(0, this._backColor);
      imageAttributes.SetRemapTable(map);
      if (this._alphaBmp != null)
        this._alphaBmp.Dispose();
      clientRectangle = this.ClientRectangle;
      int width2 = clientRectangle.Width;
      clientRectangle = this.ClientRectangle;
      int height2 = clientRectangle.Height;
      this._alphaBmp = new Bitmap(width2, height2);
      using (Graphics graphics1 = Graphics.FromImage((Image) this._alphaBmp))
      {
        Graphics graphics2 = graphics1;
        Bitmap bmp = this._bmp;
        Rectangle destRect = rectangle;
        clientRectangle = this.ClientRectangle;
        int width3 = clientRectangle.Width;
        clientRectangle = this.ClientRectangle;
        int height3 = clientRectangle.Height;
        ImageAttributes imageAttr = imageAttributes;
        graphics2.DrawImage((Image) bmp, destRect, 0, 0, width3, height3, GraphicsUnit.Pixel, imageAttr);
      }
    }
    if (!this.Focused || this.SelectionLength != 0 || !this._caretState)
      return;
    using (Graphics graphics = Graphics.FromImage((Image) this._alphaBmp))
    {
      Point caret = this.findCaret();
      using (Pen pen = new Pen(this.ForeColor, 1f))
        graphics.DrawLine(pen, caret.X, caret.Y, caret.X, caret.Y + this._fontHeight);
    }
  }

  private void OnTimer_Tick(object sender, EventArgs e)
  {
    this._caretState = !this._caretState;
    this._caretUpToDate = false;
    this.Invalidate();
  }

  private Point findCaret()
  {
    Point caret = new Point(0);
    int selectionStart = this.SelectionStart;
    IntPtr wParam = new IntPtr(selectionStart);
    caret = new Point(win32.SendMessage(this.Handle, 214, wParam, IntPtr.Zero));
    if (selectionStart == 0)
      caret = new Point(0);
    else if (selectionStart >= this.Text.Length)
    {
      wParam = new IntPtr(selectionStart - 1);
      caret = new Point(win32.SendMessage(this.Handle, 214, wParam, IntPtr.Zero));
      using (Graphics graphics = this.CreateGraphics())
      {
        string text = this.Text.Substring(this.Text.Length - 1, 1) + "X";
        int num = (int) ((double) graphics.MeasureString(text, this.Font).Width - (double) graphics.MeasureString("X", this.Font).Width) + 1;
        caret.X += num;
      }
      if (selectionStart == this.Text.Length && this.Text.Substring(this.Text.Length - 1, 1) == "\n")
      {
        caret.X = 1;
        caret.Y += this._fontHeight;
      }
    }
    return caret;
  }

  private void InitializeComponent() => this.components = new System.ComponentModel.Container();

  private class uPictureBox : PictureBox
  {
    public uPictureBox()
    {
      this.SetStyle(ControlStyles.Selectable, false);
      this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
      this.Cursor = (Cursor) null;
      this.Enabled = true;
      this.SizeMode = PictureBoxSizeMode.Normal;
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == 513 || m.Msg == 516 || m.Msg == 515 || m.Msg == 675 || m.Msg == 512 /*0x0200*/)
        win32.PostMessage(this.Parent.Handle, (uint) m.Msg, m.WParam, m.LParam);
      else if (m.Msg == 514)
        this.Parent.Invalidate();
      base.WndProc(ref m);
    }
  }
}
