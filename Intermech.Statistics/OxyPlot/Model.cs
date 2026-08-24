// Decompiled with JetBrains decompiler
// Type: OxyPlot.Model
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot;

public abstract class Model
{
  internal static readonly OxyColor DefaultSelectionColor = OxyColors.Yellow;
  private readonly object syncRoot = new object();
  private const double MouseHitTolerance = 10.0;
  private UIElement currentMouseEventElement;
  private UIElement currentTouchEventElement;

  protected Model() => this.SelectionColor = OxyColors.Yellow;

  public object SyncRoot => this.syncRoot;

  public OxyColor SelectionColor { get; set; }

  public IEnumerable<HitTestResult> HitTest(HitTestArguments args)
  {
    foreach (UIElement hitTestElement in this.GetHitTestElements())
    {
      HitTestResult hitTestResult = hitTestElement.HitTest(args);
      if (hitTestResult != null)
        yield return hitTestResult;
    }
  }

  protected abstract IEnumerable<PlotElement> GetHitTestElements();

  public event EventHandler<OxyKeyEventArgs> KeyDown;

  public event EventHandler<OxyMouseDownEventArgs> MouseDown;

  public event EventHandler<OxyMouseEventArgs> MouseMove;

  public event EventHandler<OxyMouseEventArgs> MouseUp;

  public event EventHandler<OxyMouseEventArgs> MouseEnter;

  public event EventHandler<OxyMouseEventArgs> MouseLeave;

  public event EventHandler<OxyTouchEventArgs> TouchStarted;

  public event EventHandler<OxyTouchEventArgs> TouchDelta;

  public event EventHandler<OxyTouchEventArgs> TouchCompleted;

  public virtual void HandleMouseDown(object sender, OxyMouseDownEventArgs e)
  {
    foreach (HitTestResult hitTestResult in this.HitTest(new HitTestArguments(e.Position, 10.0)))
    {
      e.HitTestResult = hitTestResult;
      hitTestResult.Element.OnMouseDown(e);
      if (e.Handled)
      {
        this.currentMouseEventElement = hitTestResult.Element;
        return;
      }
    }
    if (e.Handled)
      return;
    this.OnMouseDown(sender, e);
  }

  public virtual void HandleMouseMove(object sender, OxyMouseEventArgs e)
  {
    if (this.currentMouseEventElement != null)
      this.currentMouseEventElement.OnMouseMove(e);
    if (e.Handled)
      return;
    this.OnMouseMove(sender, e);
  }

  public virtual void HandleMouseUp(object sender, OxyMouseEventArgs e)
  {
    if (this.currentMouseEventElement != null)
    {
      this.currentMouseEventElement.OnMouseUp(e);
      this.currentMouseEventElement = (UIElement) null;
    }
    if (e.Handled)
      return;
    this.OnMouseUp(sender, e);
  }

  public virtual void HandleMouseEnter(object sender, OxyMouseEventArgs e)
  {
    if (e.Handled)
      return;
    this.OnMouseEnter(sender, e);
  }

  public virtual void HandleMouseLeave(object sender, OxyMouseEventArgs e)
  {
    if (e.Handled)
      return;
    this.OnMouseLeave(sender, e);
  }

  public virtual void HandleTouchStarted(object sender, OxyTouchEventArgs e)
  {
    foreach (HitTestResult hitTestResult in this.HitTest(new HitTestArguments(e.Position, 10.0)))
    {
      hitTestResult.Element.OnTouchStarted(e);
      if (e.Handled)
      {
        this.currentTouchEventElement = hitTestResult.Element;
        return;
      }
    }
    if (e.Handled)
      return;
    this.OnTouchStarted(sender, e);
  }

  public virtual void HandleTouchDelta(object sender, OxyTouchEventArgs e)
  {
    if (this.currentTouchEventElement != null)
      this.currentTouchEventElement.OnTouchDelta(e);
    if (e.Handled)
      return;
    this.OnTouchDelta(sender, e);
  }

  public virtual void HandleTouchCompleted(object sender, OxyTouchEventArgs e)
  {
    if (this.currentTouchEventElement != null)
    {
      this.currentTouchEventElement.OnTouchCompleted(e);
      this.currentTouchEventElement = (UIElement) null;
    }
    if (e.Handled)
      return;
    this.OnTouchCompleted(sender, e);
  }

  public virtual void HandleKeyDown(object sender, OxyKeyEventArgs e)
  {
    foreach (UIElement hitTestElement in this.GetHitTestElements())
    {
      hitTestElement.OnKeyDown(e);
      if (e.Handled)
        break;
    }
    if (e.Handled)
      return;
    this.OnKeyDown(sender, e);
  }

  protected virtual void OnKeyDown(object sender, OxyKeyEventArgs e)
  {
    EventHandler<OxyKeyEventArgs> keyDown = this.KeyDown;
    if (keyDown == null)
      return;
    keyDown(sender, e);
  }

  protected virtual void OnMouseDown(object sender, OxyMouseDownEventArgs e)
  {
    EventHandler<OxyMouseDownEventArgs> mouseDown = this.MouseDown;
    if (mouseDown == null)
      return;
    mouseDown(sender, e);
  }

  protected virtual void OnMouseMove(object sender, OxyMouseEventArgs e)
  {
    EventHandler<OxyMouseEventArgs> mouseMove = this.MouseMove;
    if (mouseMove == null)
      return;
    mouseMove(sender, e);
  }

  protected virtual void OnMouseUp(object sender, OxyMouseEventArgs e)
  {
    EventHandler<OxyMouseEventArgs> mouseUp = this.MouseUp;
    if (mouseUp == null)
      return;
    mouseUp(sender, e);
  }

  protected virtual void OnMouseEnter(object sender, OxyMouseEventArgs e)
  {
    EventHandler<OxyMouseEventArgs> mouseEnter = this.MouseEnter;
    if (mouseEnter == null)
      return;
    mouseEnter(sender, e);
  }

  protected virtual void OnMouseLeave(object sender, OxyMouseEventArgs e)
  {
    EventHandler<OxyMouseEventArgs> mouseLeave = this.MouseLeave;
    if (mouseLeave == null)
      return;
    mouseLeave(sender, e);
  }

  protected virtual void OnTouchStarted(object sender, OxyTouchEventArgs e)
  {
    EventHandler<OxyTouchEventArgs> touchStarted = this.TouchStarted;
    if (touchStarted == null)
      return;
    touchStarted(sender, e);
  }

  protected virtual void OnTouchDelta(object sender, OxyTouchEventArgs e)
  {
    EventHandler<OxyTouchEventArgs> touchDelta = this.TouchDelta;
    if (touchDelta == null)
      return;
    touchDelta(sender, e);
  }

  protected virtual void OnTouchCompleted(object sender, OxyTouchEventArgs e)
  {
    EventHandler<OxyTouchEventArgs> touchCompleted = this.TouchCompleted;
    if (touchCompleted == null)
      return;
    touchCompleted(sender, e);
  }
}
