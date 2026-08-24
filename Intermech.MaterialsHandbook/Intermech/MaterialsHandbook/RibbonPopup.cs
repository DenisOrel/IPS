// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonPopup
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

[ToolboxItem(false)]
public class RibbonPopup : Control
{
  public event EventHandler Closed;

  public event ToolStripDropDownClosingEventHandler Closing;

  public event CancelEventHandler Opening;

  public event EventHandler Showed;

  [Browsable(false)]
  public int BorderRoundness { get; set; }

  internal RibbonWrappedDropDown WrappedDropDown { get; set; }

  public RibbonPopup()
  {
    this.SetStyle(ControlStyles.Opaque, true);
    this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
    this.SetStyle(ControlStyles.Selectable, false);
    this.BorderRoundness = 4;
  }

  private void OnToolStripDropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
  {
    this.OnClosed(EventArgs.Empty);
  }

  private void OnToolStripDropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
  {
    this.OnClosing(e);
  }

  private void OnToolStripDropDown_Opening(object sender, CancelEventArgs e) => this.OnOpening(e);

  public void Close()
  {
    if (this.WrappedDropDown == null)
      return;
    this.WrappedDropDown.Close();
  }

  public void Show(Point screenLocation)
  {
    ToolStripControlHost stripControlHost = new ToolStripControlHost((Control) this);
    this.WrappedDropDown = new RibbonWrappedDropDown();
    this.WrappedDropDown.AutoClose = RibbonDesigner.Current != null;
    this.WrappedDropDown.Items.Add((ToolStripItem) stripControlHost);
    this.WrappedDropDown.Padding = Padding.Empty;
    this.WrappedDropDown.Margin = Padding.Empty;
    stripControlHost.Padding = Padding.Empty;
    stripControlHost.Margin = Padding.Empty;
    this.WrappedDropDown.Opening += new CancelEventHandler(this.OnToolStripDropDown_Opening);
    this.WrappedDropDown.Closing += new ToolStripDropDownClosingEventHandler(this.OnToolStripDropDown_Closing);
    this.WrappedDropDown.Closed += new ToolStripDropDownClosedEventHandler(this.OnToolStripDropDown_Closed);
    this.WrappedDropDown.Size = this.Size;
    this.WrappedDropDown.Show(screenLocation);
    RibbonPopupManager.Register(this);
    this.OnShowed(EventArgs.Empty);
  }

  protected override CreateParams CreateParams
  {
    get
    {
      CreateParams createParams = base.CreateParams;
      createParams.ClassStyle |= 131072 /*0x020000*/;
      return createParams;
    }
  }

  protected override void OnKeyUp(KeyEventArgs e)
  {
    base.OnKeyUp(e);
    if (27 != e.KeyValue)
      return;
    this.Close();
  }

  protected override void OnLostFocus(EventArgs e)
  {
    base.OnLostFocus(e);
    this.Close();
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    base.OnPaint(e);
    using (GraphicsPath path = RibbonRenderer.RoundRectangle(new Rectangle(Point.Empty, this.Size), this.BorderRoundness, RibbonRenderer.Corners.All))
    {
      using (Region region = new Region(path))
        this.WrappedDropDown.Region = region;
    }
  }

  protected virtual void OnClosed(EventArgs e)
  {
    RibbonPopupManager.Unregister(this);
    EventHandler closed = this.Closed;
    if (closed == null)
      return;
    closed((object) this, e);
  }

  protected virtual void OnClosing(ToolStripDropDownClosingEventArgs e)
  {
    ToolStripDropDownClosingEventHandler closing = this.Closing;
    if (closing == null)
      return;
    closing((object) this, e);
  }

  protected virtual void OnOpening(CancelEventArgs e)
  {
    CancelEventHandler opening = this.Opening;
    if (opening == null)
      return;
    opening((object) this, e);
  }

  protected virtual void OnShowed(EventArgs e)
  {
    EventHandler showed = this.Showed;
    if (showed == null)
      return;
    showed((object) this, e);
  }
}
