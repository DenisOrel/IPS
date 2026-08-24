// Decompiled with JetBrains decompiler
// Type: OxyPlot.UIElement
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public abstract class UIElement : SelectableElement
{
  public event EventHandler<OxyKeyEventArgs> KeyDown;

  public event EventHandler<OxyMouseDownEventArgs> MouseDown;

  public event EventHandler<OxyMouseEventArgs> MouseMove;

  public event EventHandler<OxyMouseEventArgs> MouseUp;

  public event EventHandler<OxyTouchEventArgs> TouchStarted;

  public event EventHandler<OxyTouchEventArgs> TouchDelta;

  public event EventHandler<OxyTouchEventArgs> TouchCompleted;

  public HitTestResult HitTest(HitTestArguments args) => this.HitTestOverride(args);

  protected internal virtual void OnMouseDown(OxyMouseDownEventArgs e)
  {
    EventHandler<OxyMouseDownEventArgs> mouseDown = this.MouseDown;
    if (mouseDown == null)
      return;
    mouseDown((object) this, e);
  }

  protected internal virtual void OnMouseMove(OxyMouseEventArgs e)
  {
    EventHandler<OxyMouseEventArgs> mouseMove = this.MouseMove;
    if (mouseMove == null)
      return;
    mouseMove((object) this, e);
  }

  protected internal virtual void OnKeyDown(OxyKeyEventArgs e)
  {
    EventHandler<OxyKeyEventArgs> keyDown = this.KeyDown;
    if (keyDown == null)
      return;
    keyDown((object) this, e);
  }

  protected internal virtual void OnMouseUp(OxyMouseEventArgs e)
  {
    EventHandler<OxyMouseEventArgs> mouseUp = this.MouseUp;
    if (mouseUp == null)
      return;
    mouseUp((object) this, e);
  }

  protected internal virtual void OnTouchStarted(OxyTouchEventArgs e)
  {
    EventHandler<OxyTouchEventArgs> touchStarted = this.TouchStarted;
    if (touchStarted == null)
      return;
    touchStarted((object) this, e);
  }

  protected internal virtual void OnTouchDelta(OxyTouchEventArgs e)
  {
    EventHandler<OxyTouchEventArgs> touchDelta = this.TouchDelta;
    if (touchDelta == null)
      return;
    touchDelta((object) this, e);
  }

  protected internal virtual void OnTouchCompleted(OxyTouchEventArgs e)
  {
    EventHandler<OxyTouchEventArgs> touchCompleted = this.TouchCompleted;
    if (touchCompleted == null)
      return;
    touchCompleted((object) this, e);
  }

  protected virtual HitTestResult HitTestOverride(HitTestArguments args) => (HitTestResult) null;
}
