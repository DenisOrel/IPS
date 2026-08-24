// Decompiled with JetBrains decompiler
// Type: OxyPlot.ControllerBase
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot;

public abstract class ControllerBase : IController
{
  private readonly object syncRoot = new object();

  protected ControllerBase()
  {
    this.InputCommandBindings = new List<InputCommandBinding>();
    this.MouseDownManipulators = (IList<ManipulatorBase<OxyMouseEventArgs>>) new List<ManipulatorBase<OxyMouseEventArgs>>();
    this.MouseHoverManipulators = (IList<ManipulatorBase<OxyMouseEventArgs>>) new List<ManipulatorBase<OxyMouseEventArgs>>();
    this.TouchManipulators = (IList<ManipulatorBase<OxyTouchEventArgs>>) new List<ManipulatorBase<OxyTouchEventArgs>>();
  }

  public List<InputCommandBinding> InputCommandBindings { get; private set; }

  protected IList<ManipulatorBase<OxyMouseEventArgs>> MouseDownManipulators { get; private set; }

  protected IList<ManipulatorBase<OxyMouseEventArgs>> MouseHoverManipulators { get; private set; }

  protected IList<ManipulatorBase<OxyTouchEventArgs>> TouchManipulators { get; private set; }

  public bool HandleGesture(IView view, OxyInputGesture gesture, OxyInputEventArgs args)
  {
    return this.HandleCommand(this.GetCommand(gesture), view, args);
  }

  public virtual bool HandleMouseDown(IView view, OxyMouseDownEventArgs args)
  {
    lock (this.GetSyncRoot(view))
    {
      if (view.ActualModel != null)
      {
        view.ActualModel.HandleMouseDown((object) this, args);
        if (args.Handled)
          return true;
      }
      return this.HandleCommand(this.GetCommand((OxyInputGesture) new OxyMouseDownGesture(args.ChangedButton, args.ModifierKeys, args.ClickCount)), view, (OxyInputEventArgs) args);
    }
  }

  public virtual bool HandleMouseEnter(IView view, OxyMouseEventArgs args)
  {
    lock (this.GetSyncRoot(view))
    {
      if (view.ActualModel != null)
      {
        view.ActualModel.HandleMouseEnter((object) this, args);
        if (args.Handled)
          return true;
      }
      return this.HandleCommand(this.GetCommand((OxyInputGesture) new OxyMouseEnterGesture(args.ModifierKeys)), view, (OxyInputEventArgs) args);
    }
  }

  public virtual bool HandleMouseLeave(IView view, OxyMouseEventArgs args)
  {
    lock (this.GetSyncRoot(view))
    {
      if (view.ActualModel != null)
      {
        view.ActualModel.HandleMouseLeave((object) this, args);
        if (args.Handled)
          return true;
      }
      foreach (ManipulatorBase<OxyMouseEventArgs> manipulatorBase in this.MouseHoverManipulators.ToArray<ManipulatorBase<OxyMouseEventArgs>>())
      {
        manipulatorBase.Completed(args);
        this.MouseHoverManipulators.Remove(manipulatorBase);
      }
      return args.Handled;
    }
  }

  public virtual bool HandleMouseMove(IView view, OxyMouseEventArgs args)
  {
    lock (this.GetSyncRoot(view))
    {
      if (view.ActualModel != null)
      {
        view.ActualModel.HandleMouseMove((object) this, args);
        if (args.Handled)
          return true;
      }
      foreach (ManipulatorBase<OxyMouseEventArgs> mouseDownManipulator in (IEnumerable<ManipulatorBase<OxyMouseEventArgs>>) this.MouseDownManipulators)
        mouseDownManipulator.Delta(args);
      foreach (ManipulatorBase<OxyMouseEventArgs> hoverManipulator in (IEnumerable<ManipulatorBase<OxyMouseEventArgs>>) this.MouseHoverManipulators)
        hoverManipulator.Delta(args);
      return args.Handled;
    }
  }

  public virtual bool HandleMouseUp(IView view, OxyMouseEventArgs args)
  {
    lock (this.GetSyncRoot(view))
    {
      if (view.ActualModel != null)
      {
        view.ActualModel.HandleMouseUp((object) this, args);
        if (args.Handled)
          return true;
      }
      foreach (ManipulatorBase<OxyMouseEventArgs> manipulatorBase in this.MouseDownManipulators.ToArray<ManipulatorBase<OxyMouseEventArgs>>())
      {
        manipulatorBase.Completed(args);
        this.MouseDownManipulators.Remove(manipulatorBase);
      }
      return args.Handled;
    }
  }

  public virtual bool HandleMouseWheel(IView view, OxyMouseWheelEventArgs args)
  {
    lock (this.GetSyncRoot(view))
      return this.HandleCommand(this.GetCommand((OxyInputGesture) new OxyMouseWheelGesture(args.ModifierKeys)), view, (OxyInputEventArgs) args);
  }

  public bool HandleTouchStarted(IView view, OxyTouchEventArgs args)
  {
    lock (this.GetSyncRoot(view))
    {
      if (view.ActualModel != null)
      {
        view.ActualModel.HandleTouchStarted((object) this, args);
        if (args.Handled)
          return true;
      }
      return this.HandleCommand(this.GetCommand((OxyInputGesture) new OxyTouchGesture()), view, (OxyInputEventArgs) args);
    }
  }

  public bool HandleTouchDelta(IView view, OxyTouchEventArgs args)
  {
    lock (this.GetSyncRoot(view))
    {
      if (view.ActualModel != null)
      {
        view.ActualModel.HandleTouchDelta((object) this, args);
        if (args.Handled)
          return true;
      }
      foreach (ManipulatorBase<OxyTouchEventArgs> touchManipulator in (IEnumerable<ManipulatorBase<OxyTouchEventArgs>>) this.TouchManipulators)
        touchManipulator.Delta(args);
      return args.Handled;
    }
  }

  public bool HandleTouchCompleted(IView view, OxyTouchEventArgs args)
  {
    lock (this.GetSyncRoot(view))
    {
      if (view.ActualModel != null)
      {
        view.ActualModel.HandleTouchCompleted((object) this, args);
        if (args.Handled)
          return true;
      }
      foreach (ManipulatorBase<OxyTouchEventArgs> manipulatorBase in this.TouchManipulators.ToArray<ManipulatorBase<OxyTouchEventArgs>>())
      {
        manipulatorBase.Completed(args);
        this.TouchManipulators.Remove(manipulatorBase);
      }
      return args.Handled;
    }
  }

  public virtual bool HandleKeyDown(IView view, OxyKeyEventArgs args)
  {
    lock (this.GetSyncRoot(view))
    {
      if (view.ActualModel == null)
        return false;
      view.ActualModel.HandleKeyDown((object) this, args);
      return args.Handled || this.HandleCommand(this.GetCommand((OxyInputGesture) new OxyKeyGesture(args.Key, args.ModifierKeys)), view, (OxyInputEventArgs) args);
    }
  }

  public virtual void AddMouseManipulator(
    IView view,
    ManipulatorBase<OxyMouseEventArgs> manipulator,
    OxyMouseDownEventArgs args)
  {
    this.MouseDownManipulators.Add(manipulator);
    manipulator.Started((OxyMouseEventArgs) args);
  }

  public virtual void AddHoverManipulator(
    IView view,
    ManipulatorBase<OxyMouseEventArgs> manipulator,
    OxyMouseEventArgs args)
  {
    this.MouseHoverManipulators.Add(manipulator);
    manipulator.Started(args);
  }

  public virtual void AddTouchManipulator(
    IView view,
    ManipulatorBase<OxyTouchEventArgs> manipulator,
    OxyTouchEventArgs args)
  {
    this.TouchManipulators.Add(manipulator);
    manipulator.Started(args);
  }

  public virtual void Bind(OxyMouseDownGesture gesture, IViewCommand<OxyMouseDownEventArgs> command)
  {
    this.BindCore((OxyInputGesture) gesture, (IViewCommand) command);
  }

  public virtual void Bind(OxyMouseEnterGesture gesture, IViewCommand<OxyMouseEventArgs> command)
  {
    this.BindCore((OxyInputGesture) gesture, (IViewCommand) command);
  }

  public virtual void Bind(
    OxyMouseWheelGesture gesture,
    IViewCommand<OxyMouseWheelEventArgs> command)
  {
    this.BindCore((OxyInputGesture) gesture, (IViewCommand) command);
  }

  public virtual void Bind(OxyTouchGesture gesture, IViewCommand<OxyTouchEventArgs> command)
  {
    this.BindCore((OxyInputGesture) gesture, (IViewCommand) command);
  }

  public virtual void Bind(OxyKeyGesture gesture, IViewCommand<OxyKeyEventArgs> command)
  {
    this.BindCore((OxyInputGesture) gesture, (IViewCommand) command);
  }

  public virtual void Unbind(OxyInputGesture gesture)
  {
    foreach (InputCommandBinding inputCommandBinding in this.InputCommandBindings.Where<InputCommandBinding>((Func<InputCommandBinding, bool>) (icb => icb.Gesture.Equals(gesture))).ToArray<InputCommandBinding>())
      this.InputCommandBindings.Remove(inputCommandBinding);
  }

  public virtual void Unbind(IViewCommand command)
  {
    foreach (InputCommandBinding inputCommandBinding in this.InputCommandBindings.Where<InputCommandBinding>((Func<InputCommandBinding, bool>) (icb => icb.Command == command)).ToArray<InputCommandBinding>())
      this.InputCommandBindings.Remove(inputCommandBinding);
  }

  public virtual void UnbindAll() => this.InputCommandBindings.Clear();

  protected void BindCore(OxyInputGesture gesture, IViewCommand command)
  {
    InputCommandBinding inputCommandBinding = this.InputCommandBindings.FirstOrDefault<InputCommandBinding>((Func<InputCommandBinding, bool>) (icb => icb.Gesture.Equals(gesture)));
    if (inputCommandBinding != null)
      this.InputCommandBindings.Remove(inputCommandBinding);
    if (command == null)
      return;
    this.InputCommandBindings.Add(new InputCommandBinding(gesture, command));
  }

  protected virtual IViewCommand GetCommand(OxyInputGesture gesture)
  {
    return this.InputCommandBindings.FirstOrDefault<InputCommandBinding>((Func<InputCommandBinding, bool>) (b => b.Gesture.Equals(gesture)))?.Command;
  }

  protected virtual bool HandleCommand(IViewCommand command, IView view, OxyInputEventArgs args)
  {
    if (command == null)
      return false;
    command.Execute(view, (IController) this, args);
    return args.Handled;
  }

  protected object GetSyncRoot(IView view)
  {
    return view.ActualModel == null ? this.syncRoot : view.ActualModel.SyncRoot;
  }
}
