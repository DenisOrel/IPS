// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Advanced.ToolTipDialog
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Advanced;

public class ToolTipDialog : Form
{
  private const int C_QUOTEH = 25;
  private const int C_ARCH = 5;
  private const int C_SHADOW = 5;
  protected GraphicsPath m_path;
  protected Region m_rOuterFrame;
  protected Region m_rInnerFrame;
  protected bool m_bShowFrame = true;
  protected bool m_bIsShadow;
  protected bool m_bHasShadow;
  protected Color m_cTopLeft = Color.Aqua;
  protected Color m_cBotRite = Color.Lime;
  private ToolTipDialog.alAlign m_tipAlign;
  private ToolTipDialog.sdSide m_tipSide;
  public Point m_ptTipPosition = new Point(0, 0);
  public RectangleF m_rtBalloonF = new RectangleF(0.0f, 0.0f, 1f, 1f);
  private Point m_ptCenter = new Point(0, 0);
  protected int m_nTailOffset = 30;
  protected Rectangle m_rtAnchor = new Rectangle(0, 0, 0, 0);
  private Form m_fmParent;
  private ToolTipDialog m_fmShadow;
  public EventHandler m_evhMove;

  public ToolTipDialog()
  {
    this.AutoScaleBaseSize = new Size(5, 13);
    this.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/);
    this.FormBorderStyle = FormBorderStyle.None;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ToolTipDialog);
    this.ShowInTaskbar = false;
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.StartPosition = FormStartPosition.Manual;
  }

  public void reSizeMe()
  {
    this.m_path = new GraphicsPath();
    int width = this.Width;
    int height = this.Height;
    int y = 25;
    int x = 30;
    Point point1 = new Point();
    Point point2 = new Point();
    Point point3 = new Point();
    Point point4 = new Point();
    Point point5 = new Point();
    Point point6 = new Point();
    switch (this.m_tipSide)
    {
      case ToolTipDialog.sdSide.sd_top:
        point1.Y = y;
        point3.Y = 0;
        point5.Y = y;
        point2.Y = point1.Y + 4;
        point4.Y = point3.Y + 4;
        point6.Y = point5.Y + 4;
        goto case ToolTipDialog.sdSide.sd_horizontal;
      case ToolTipDialog.sdSide.sd_left:
        point1.X = y;
        point5.X = y;
        point3.X = 0;
        point2.X = point1.X + 4;
        point6.X = point5.X + 4;
        point4.X = point3.X + 4;
        goto case ToolTipDialog.sdSide.sd_vertical;
      case ToolTipDialog.sdSide.sd_bottom:
        point1.Y = height - y;
        point5.Y = height - y;
        point3.Y = height;
        point2.Y = point1.Y - 4;
        point6.Y = point5.Y - 4;
        point4.Y = point3.Y - 4;
        goto case ToolTipDialog.sdSide.sd_horizontal;
      case ToolTipDialog.sdSide.sd_right:
        point1.X = width - y;
        point5.X = width - y;
        point3.X = width;
        point2.X = point1.X - 4;
        point6.X = point5.X - 4;
        point4.X = point3.X - 4;
        goto case ToolTipDialog.sdSide.sd_vertical;
      case ToolTipDialog.sdSide.sd_horizontal:
        if (this.m_tipAlign == ToolTipDialog.alAlign.al_lhs)
        {
          point1.X = x + this.m_nTailOffset;
          point3.X = point1.X - 10;
          point5.X = point1.X + 30;
          point2.X = point1.X + 4;
          point4.X = point3.X + 3;
          point6.X = point5.X + 2;
          break;
        }
        if (this.m_tipAlign == ToolTipDialog.alAlign.al_mid)
        {
          point3.X = width / 2;
          point1.X = point3.X - 10;
          point5.X = point3.X + 10;
          point2.X = point1.X;
          point4.X = point3.X;
          point6.X = point5.X;
          break;
        }
        point5.X = width - x - this.m_nTailOffset;
        point1.X = point5.X - 30;
        point3.X = point5.X + 10;
        point2.X = point1.X - 2;
        point4.X = point3.X - 3;
        point6.X = point5.X - 4;
        break;
      case ToolTipDialog.sdSide.sd_vertical:
        if (this.m_tipAlign == ToolTipDialog.alAlign.al_lhs)
        {
          point1.Y = x + this.m_nTailOffset;
          point5.Y = point1.Y + 30;
          point3.Y = point1.Y - 10;
          point2.Y = point1.Y + 4;
          point4.Y = point3.Y + 3;
          point6.Y = point5.Y + 2;
          break;
        }
        if (this.m_tipAlign == ToolTipDialog.alAlign.al_mid)
        {
          point3.Y = height / 2;
          point1.Y = point3.Y - 10;
          point5.Y = point3.Y + 10;
          point2.Y = point1.Y;
          point4.Y = point3.Y;
          point6.Y = point5.Y;
          break;
        }
        point5.Y = height - x - this.m_nTailOffset;
        point1.Y = point5.Y - 30;
        point3.Y = point5.Y + 10;
        point2.Y = point1.Y - 2;
        point4.Y = point3.Y - 3;
        point6.Y = point5.Y - 4;
        break;
    }
    this.m_ptTipPosition = point3;
    this.m_path.AddLines(new Point[3]
    {
      point1,
      point3,
      point5
    });
    this.m_path.CloseFigure();
    GraphicsPath path = new GraphicsPath();
    path.AddLines(new Point[3]{ point2, point4, point6 });
    path.CloseFigure();
    Size size = new Size(width - (x + x), height - (y + y));
    Point location = new Point(x, y);
    this.m_rOuterFrame = new Region(new Rectangle(location, size));
    size.Width += 4;
    size.Height -= 2;
    location.X -= 2;
    ++location.Y;
    this.m_rOuterFrame.Union(new Rectangle(location, size));
    size.Width += 2;
    size.Height -= 2;
    --location.X;
    ++location.Y;
    this.m_rOuterFrame.Union(new Rectangle(location, size));
    size.Width += 2;
    size.Height -= 2;
    --location.X;
    ++location.Y;
    this.m_rOuterFrame.Union(new Rectangle(location, size));
    size.Width += 2;
    size.Height -= 4;
    --location.X;
    location.Y += 2;
    this.m_rOuterFrame.Union(new Rectangle(location, size));
    this.m_rOuterFrame.Union(this.m_path);
    if (this.m_bIsShadow)
    {
      Region region = this.m_rOuterFrame.Clone();
      region.Translate(-5, -5);
      this.m_rOuterFrame.Exclude(region);
    }
    this.Region = this.m_rOuterFrame;
    Graphics g = Graphics.FromHwnd(this.Handle);
    this.m_rtBalloonF = this.Region.GetBounds(g);
    this.m_ptCenter = new Point((int) ((double) this.m_rtBalloonF.Width / 2.0 + (double) this.m_rtBalloonF.X), (int) ((double) this.m_rtBalloonF.Height / 2.0 + (double) this.m_rtBalloonF.Y));
    g.Dispose();
    Rectangle rect = new Rectangle(31 /*0x1F*/, 29, width - 62, height - 58);
    Region region1 = new Region(rect);
    --rect.X;
    ++rect.Y;
    rect.Width += 2;
    rect.Height -= 2;
    region1.Union(rect);
    --rect.X;
    ++rect.Y;
    rect.Width += 2;
    rect.Height -= 2;
    region1.Union(rect);
    this.m_rInnerFrame = this.m_rOuterFrame.Clone();
    this.m_rInnerFrame.Exclude(region1);
    this.m_rInnerFrame.Exclude(path);
    this.Invalidate();
  }

  private bool bCalcTailPos(Point ptA, bool bOnlyBelow)
  {
    int width1 = this.Width;
    int num1 = this.Height + 5;
    Screen[] allScreens = Screen.AllScreens;
    int width2 = allScreens[0].Bounds.Width;
    int height = allScreens[0].Bounds.Height;
    if (!bOnlyBelow)
    {
      this.tailSide = ToolTipDialog.sdSide.sd_bottom;
    }
    else
    {
      if (ptA.Y + num1 >= height)
        return false;
      this.tailSide = ToolTipDialog.sdSide.sd_top;
    }
    int num2 = width2 - ptA.X;
    int num3 = this.Width + 5 - 25;
    if (num2 > num3)
    {
      this.tailOffset = 5;
      this.tailAlign = ToolTipDialog.alAlign.al_lhs;
    }
    else
    {
      int num4 = num3 - num2;
      if (num4 < (int) Math.Round((double) num3 / 3.0))
      {
        this.tailOffset = num4 + 5;
        this.tailAlign = ToolTipDialog.alAlign.al_lhs;
      }
      else if (num4 > (int) Math.Round((double) (num3 - 30) / 2.0))
      {
        int num5 = num3 - num4 - 30;
        if (num5 < 0)
          num5 = 0;
        this.tailOffset = num5 + 5;
        this.tailAlign = ToolTipDialog.alAlign.al_rhs;
      }
      else
      {
        this.tailOffset = 0;
        this.tailAlign = ToolTipDialog.alAlign.al_mid;
      }
    }
    this.Location = new Point(0, 0)
    {
      X = ptA.X - this.m_ptTipPosition.X,
      Y = ptA.Y - this.m_ptTipPosition.Y
    };
    return true;
  }

  public void setPosition(Form onForm, Control onControl)
  {
    if (this.m_fmShadow == null)
      this.Owner = onForm;
    else
      this.m_fmShadow.Owner = onForm;
    this.m_rtAnchor = new Rectangle(onControl.Location, onControl.Size);
    Point location = this.m_rtAnchor.Location;
    Point p = location;
    location.Y += this.m_rtAnchor.Height;
    if (!this.bCalcTailPos(onForm.PointToScreen(location), true))
      this.bCalcTailPos(onForm.PointToScreen(p), false);
    this.m_fmParent = onForm;
    this.m_evhMove = new EventHandler(this.Parent_Move);
    onForm.Move += this.m_evhMove;
  }

  public void setPosition(Form onForm, Point atPoint)
  {
    if (this.m_fmShadow == null)
      this.Owner = onForm;
    else
      this.m_fmShadow.Owner = onForm;
    this.m_rtAnchor = new Rectangle(atPoint, new Size(0, 0));
    Point screen = onForm.PointToScreen(this.m_rtAnchor.Location);
    if (!this.bCalcTailPos(screen, true))
      this.bCalcTailPos(onForm.PointToScreen(screen), false);
    this.m_fmParent = onForm;
    this.m_evhMove = new EventHandler(this.Parent_Move);
    onForm.Move += this.m_evhMove;
  }

  public void Parent_Move(object sender, EventArgs e)
  {
    if (!(sender is Form))
      return;
    Point location = this.m_rtAnchor.Location;
    Point p = location;
    p.Y += this.m_rtAnchor.Height;
    if (this.bCalcTailPos(((Control) sender).PointToScreen(p), true))
      return;
    this.bCalcTailPos(((Control) sender).PointToScreen(location), false);
  }

  private bool bMakeShadow()
  {
    if (this.m_bIsShadow || !this.m_bHasShadow || this.DesignMode)
      return false;
    if (this.m_fmShadow == null)
    {
      this.m_fmShadow = new ToolTipDialog();
      this.Owner = (Form) this.m_fmShadow;
      this.m_fmShadow.Width = this.Width;
      this.m_fmShadow.Height = this.Height;
      this.m_fmShadow.StartPosition = FormStartPosition.Manual;
      this.m_fmShadow.Location = new Point(this.Location.X + 5, this.Location.Y + 5);
      this.m_fmShadow.IsShadow = true;
      this.m_fmShadow.Opacity = 0.6;
    }
    return true;
  }

  private void DestroyShadow()
  {
    if (this.m_fmShadow == null)
      return;
    if (this.m_fmShadow.Owner != null)
    {
      this.Owner = this.m_fmShadow.Owner;
      this.m_fmShadow.RemoveOwnedForm((Form) this);
      this.Owner.RemoveOwnedForm((Form) this.m_fmShadow);
    }
    this.m_fmShadow.Close();
    this.m_fmShadow.Dispose();
    this.m_fmShadow = (ToolTipDialog) null;
  }

  public void reDrawMe()
  {
    if (!this.m_bShowFrame)
      return;
    Graphics graphics = Graphics.FromHwnd(this.Handle);
    graphics.RenderingOrigin = this.m_ptCenter;
    graphics.FillRegion((Brush) new LinearGradientBrush(this.m_rtBalloonF, this.m_cTopLeft, this.m_cBotRite, 45f, true), this.m_rInnerFrame);
    graphics.Dispose();
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    base.OnPaint(e);
    this.reDrawMe();
  }

  protected override void OnClick(EventArgs e)
  {
    base.OnClick(e);
    if (this.m_bIsShadow)
      return;
    this.Close();
  }

  protected override void OnResize(EventArgs e)
  {
    base.OnResize(e);
    this.reSizeMe();
  }

  protected override void OnVisibleChanged(EventArgs e)
  {
    base.OnVisibleChanged(e);
    if (!this.m_bHasShadow || !this.Visible || this.m_fmShadow == null)
      return;
    this.m_fmShadow.Show();
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    base.OnClosing(e);
    if (this.Owner != null)
    {
      this.Owner.RemoveOwnedForm((Form) this);
      this.Owner = (Form) null;
    }
    if (this.m_fmShadow != null)
    {
      if (this.m_fmShadow.Owner != null)
      {
        this.m_fmShadow.Owner.RemoveOwnedForm((Form) this.m_fmShadow);
        this.m_fmShadow.Owner = (Form) null;
      }
      this.m_fmShadow.Close();
    }
    if (this.m_fmParent == null)
      return;
    this.m_fmParent.Move -= this.m_evhMove;
    this.m_fmParent = (Form) null;
  }

  protected override void OnMove(EventArgs e)
  {
    base.OnMove(e);
    if (this.m_fmShadow == null)
      return;
    ToolTipDialog fmShadow = this.m_fmShadow;
    Point location = this.Location;
    int x = location.X + 5;
    location = this.Location;
    int y = location.Y + 5;
    Point point = new Point(x, y);
    fmShadow.Location = point;
  }

  public bool IsShadow
  {
    set
    {
      if (value && !this.m_bIsShadow)
      {
        this.ShowFrame = false;
        this.BackColor = Color.DarkGray;
      }
      this.m_bIsShadow = value;
    }
    get => this.m_bIsShadow;
  }

  public bool HasShadow
  {
    set
    {
      this.m_bHasShadow = value;
      if (this.m_bHasShadow)
        this.bMakeShadow();
      else
        this.DestroyShadow();
    }
    get => this.m_bHasShadow;
  }

  public bool ShowFrame
  {
    set
    {
      if (this.m_bShowFrame == value)
        return;
      this.m_bShowFrame = value;
      this.reDrawMe();
    }
    get => this.m_bShowFrame;
  }

  public ToolTipDialog.sdSide tailSide
  {
    set
    {
      this.m_tipSide = value;
      this.reSizeMe();
      if (!this.m_bHasShadow || this.m_fmShadow == null)
        return;
      this.m_fmShadow.tailSide = value;
    }
    get => this.m_tipSide;
  }

  public int tailOffset
  {
    set
    {
      this.m_nTailOffset = value;
      this.reSizeMe();
      if (this.m_fmShadow == null)
        return;
      this.m_fmShadow.tailOffset = value;
    }
    get => this.m_nTailOffset;
  }

  public ToolTipDialog.alAlign tailAlign
  {
    set
    {
      this.m_tipAlign = value;
      this.reSizeMe();
      if (this.m_fmShadow == null)
        return;
      this.m_fmShadow.tailAlign = value;
    }
    get => this.m_tipAlign;
  }

  public Color FrameTopLeft
  {
    set
    {
      this.m_cTopLeft = value;
      this.reDrawMe();
    }
    get => this.m_cTopLeft;
  }

  public Color FrameBottomRight
  {
    set
    {
      this.m_cBotRite = value;
      this.reDrawMe();
    }
    get => this.m_cBotRite;
  }

  private void InitializeComponent()
  {
    this.SuspendLayout();
    this.ClientSize = new Size(292, 273);
    this.Name = nameof (ToolTipDialog);
    this.ResumeLayout(false);
  }

  public enum sdSide
  {
    sd_top,
    sd_left,
    sd_bottom,
    sd_right,
    sd_horizontal,
    sd_vertical,
  }

  public enum alAlign
  {
    al_lhs,
    al_mid,
    al_rhs,
  }
}
