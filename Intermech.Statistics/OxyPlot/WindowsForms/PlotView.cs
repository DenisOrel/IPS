// Decompiled with JetBrains decompiler
// Type: OxyPlot.WindowsForms.PlotView
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace OxyPlot.WindowsForms;

[Serializable]
public class PlotView : Control, IPlotView, IView
{
  private const string OxyPlotCategory = "OxyPlot";
  private readonly object invalidateLock = new object();
  private readonly object modelLock = new object();
  private readonly object renderingLock = new object();
  private readonly GraphicsRenderContext renderContext;
  [NonSerialized]
  private Label trackerLabel;
  [NonSerialized]
  private PlotModel currentModel;
  private bool isModelInvalidated;
  private PlotModel model;
  private IPlotController defaultController;
  private bool updateDataFlag = true;
  private Rectangle zoomRectangle;

  public PlotView()
  {
    this.renderContext = new GraphicsRenderContext();
    this.DoubleBuffered = true;
    this.PanCursor = Cursors.Hand;
    this.ZoomRectangleCursor = Cursors.SizeNWSE;
    this.ZoomHorizontalCursor = Cursors.SizeWE;
    this.ZoomVerticalCursor = Cursors.SizeNS;
  }

  OxyPlot.Model IView.ActualModel => (OxyPlot.Model) this.Model;

  public PlotModel ActualModel => this.Model;

  IController IView.ActualController => (IController) this.ActualController;

  public OxyRect ClientArea
  {
    get
    {
      Rectangle clientRectangle = this.ClientRectangle;
      double left = (double) clientRectangle.Left;
      clientRectangle = this.ClientRectangle;
      double top = (double) clientRectangle.Top;
      clientRectangle = this.ClientRectangle;
      double width = (double) clientRectangle.Width;
      clientRectangle = this.ClientRectangle;
      double height = (double) clientRectangle.Height;
      return new OxyRect(left, top, width, height);
    }
  }

  public IPlotController ActualController
  {
    get
    {
      return this.Controller ?? this.defaultController ?? (this.defaultController = (IPlotController) new PlotController());
    }
  }

  [Browsable(false)]
  [DefaultValue(null)]
  [Category("OxyPlot")]
  public PlotModel Model
  {
    get => this.model;
    set
    {
      if (this.model == value)
        return;
      this.model = value;
      this.OnModelChanged();
    }
  }

  [Browsable(false)]
  [DefaultValue(null)]
  [Category("OxyPlot")]
  public IPlotController Controller { get; set; }

  [Category("OxyPlot")]
  public Cursor PanCursor { get; set; }

  [Category("OxyPlot")]
  public Cursor ZoomHorizontalCursor { get; set; }

  [Category("OxyPlot")]
  public Cursor ZoomRectangleCursor { get; set; }

  [Category("OxyPlot")]
  public Cursor ZoomVerticalCursor { get; set; }

  public void HideTracker()
  {
    if (this.trackerLabel == null)
      return;
    this.trackerLabel.Visible = false;
  }

  public void HideZoomRectangle()
  {
    this.zoomRectangle = Rectangle.Empty;
    this.Invalidate();
  }

  public void InvalidatePlot(bool updateData)
  {
    lock (this.invalidateLock)
    {
      this.isModelInvalidated = true;
      this.updateDataFlag |= updateData;
    }
    this.Invalidate();
  }

  public void OnModelChanged()
  {
    lock (this.modelLock)
    {
      if (this.currentModel != null)
      {
        ((IPlotModel) this.currentModel).AttachPlotView((IPlotView) null);
        this.currentModel = (PlotModel) null;
      }
      if (this.Model != null)
      {
        ((IPlotModel) this.Model).AttachPlotView((IPlotView) this);
        this.currentModel = this.Model;
      }
    }
    this.InvalidatePlot(true);
  }

  public void SetCursorType(CursorType cursorType)
  {
    switch (cursorType)
    {
      case CursorType.Pan:
        this.Cursor = this.PanCursor;
        break;
      case CursorType.ZoomRectangle:
        this.Cursor = this.ZoomRectangleCursor;
        break;
      case CursorType.ZoomHorizontal:
        this.Cursor = this.ZoomHorizontalCursor;
        break;
      case CursorType.ZoomVertical:
        this.Cursor = this.ZoomVerticalCursor;
        break;
      default:
        this.Cursor = Cursors.Arrow;
        break;
    }
  }

  public void ShowTracker(TrackerHitResult data)
  {
    if (this.trackerLabel == null)
    {
      Label label = new Label();
      label.Parent = (Control) this;
      label.BackColor = Color.LightSkyBlue;
      label.AutoSize = true;
      this.trackerLabel = label;
    }
    this.trackerLabel.Text = data.ToString();
    this.trackerLabel.Visible = true;
    if ((int) data.Position.X + this.trackerLabel.Width >= this.Bounds.Width)
    {
      Rectangle bounds = this.Bounds;
      if (bounds.Width - this.trackerLabel.Width - 4 > 0)
      {
        Label trackerLabel = this.trackerLabel;
        bounds = this.Bounds;
        int num = bounds.Width - this.trackerLabel.Width - 4;
        trackerLabel.Left = num;
      }
      else
        this.trackerLabel.Left = (int) data.Position.X;
    }
    else
      this.trackerLabel.Left = (int) data.Position.X;
    this.trackerLabel.Top = (int) data.Position.Y;
  }

  public void ShowZoomRectangle(OxyRect rectangle)
  {
    this.zoomRectangle = new Rectangle((int) rectangle.Left, (int) rectangle.Top, (int) rectangle.Width, (int) rectangle.Height);
    this.Invalidate();
  }

  public void SetClipboardText(string text)
  {
    try
    {
      Clipboard.SetText(text);
    }
    catch (ExternalException ex)
    {
      int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "OxyPlot");
    }
  }

  protected override void OnMouseDown(MouseEventArgs e)
  {
    base.OnMouseDown(e);
    this.Focus();
    this.Capture = true;
    this.ActualController.HandleMouseDown((IView) this, e.ToMouseDownEventArgs(PlotView.GetModifiers()));
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    this.ActualController.HandleMouseMove((IView) this, ConverterExtensions.ToMouseEventArgs(e, PlotView.GetModifiers()));
  }

  protected override void OnMouseUp(MouseEventArgs e)
  {
    base.OnMouseUp(e);
    this.Capture = false;
    this.ActualController.HandleMouseUp((IView) this, e.ToMouseUpEventArgs(PlotView.GetModifiers()));
  }

  protected override void OnMouseEnter(EventArgs e)
  {
    base.OnMouseEnter(e);
    this.ActualController.HandleMouseEnter((IView) this, e.ToMouseEventArgs(PlotView.GetModifiers()));
  }

  protected override void OnMouseLeave(EventArgs e)
  {
    base.OnMouseLeave(e);
    this.ActualController.HandleMouseLeave((IView) this, e.ToMouseEventArgs(PlotView.GetModifiers()));
  }

  protected override void OnMouseWheel(MouseEventArgs e)
  {
    base.OnMouseWheel(e);
    this.ActualController.HandleMouseWheel((IView) this, e.ToMouseWheelEventArgs(PlotView.GetModifiers()));
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    base.OnPaint(e);
    try
    {
      lock (this.invalidateLock)
      {
        if (this.isModelInvalidated)
        {
          if (this.model != null)
          {
            ((IPlotModel) this.model).Update(this.updateDataFlag);
            this.updateDataFlag = false;
          }
          this.isModelInvalidated = false;
        }
      }
      lock (this.renderingLock)
      {
        this.renderContext.SetGraphicsTarget(e.Graphics);
        if (this.model != null)
        {
          if (!this.model.Background.IsUndefined())
          {
            using (SolidBrush solidBrush = new SolidBrush(this.model.Background.ToColor()))
              e.Graphics.FillRectangle((Brush) solidBrush, e.ClipRectangle);
          }
          ((IPlotModel) this.model).Render((IRenderContext) this.renderContext, (double) this.Width, (double) this.Height);
        }
        if (!(this.zoomRectangle != Rectangle.Empty))
          return;
        using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(64 /*0x40*/, (int) byte.MaxValue, (int) byte.MaxValue, 0)))
        {
          using (Pen pen = new Pen(Color.Black))
          {
            pen.DashPattern = new float[2]{ 3f, 1f };
            e.Graphics.FillRectangle((Brush) solidBrush, this.zoomRectangle);
            e.Graphics.DrawRectangle(pen, this.zoomRectangle);
          }
        }
      }
    }
    catch (Exception ex)
    {
      StackTrace stackTrace = new StackTrace(ex);
      using (Font font = new Font("Arial", 10f))
      {
        e.Graphics.ResetTransform();
        e.Graphics.DrawString("OxyPlot paint exception: " + ex.Message, font, Brushes.Red, (float) ((double) this.Width * 0.5), (float) ((double) this.Height * 0.5), new StringFormat()
        {
          Alignment = StringAlignment.Center,
          LineAlignment = StringAlignment.Center
        });
      }
    }
  }

  protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
  {
    base.OnPreviewKeyDown(e);
    OxyKeyEventArgs args = new OxyKeyEventArgs();
    args.ModifierKeys = PlotView.GetModifiers();
    args.Key = e.KeyCode.Convert();
    this.ActualController.HandleKeyDown((IView) this, args);
  }

  protected override void OnResize(EventArgs e)
  {
    base.OnResize(e);
    this.InvalidatePlot(false);
  }

  private static OxyModifierKeys GetModifiers()
  {
    OxyModifierKeys modifiers = OxyModifierKeys.None;
    if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
      modifiers |= OxyModifierKeys.Shift;
    if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
      modifiers |= OxyModifierKeys.Control;
    if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
      modifiers |= OxyModifierKeys.Alt;
    return modifiers;
  }
}
