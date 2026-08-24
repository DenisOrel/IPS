// Decompiled with JetBrains decompiler
// Type: OxyPlot.PlotCommands
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot.Reporting;
using System;

#nullable disable
namespace OxyPlot;

public static class PlotCommands
{
  static PlotCommands()
  {
    PlotCommands.Reset = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandleReset(view, (OxyInputEventArgs) args)));
    PlotCommands.CopyTextReport = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandleCopyTextReport(view, (OxyInputEventArgs) args)));
    PlotCommands.CopyCode = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandleCopyCode(view, (OxyInputEventArgs) args)));
    PlotCommands.ResetAt = (IViewCommand<OxyMouseEventArgs>) new DelegatePlotCommand<OxyMouseEventArgs>((Action<IPlotView, IController, OxyMouseEventArgs>) ((view, controller, args) => PlotCommands.HandleReset(view, (OxyInputEventArgs) args)));
    PlotCommands.PanAt = (IViewCommand<OxyMouseDownEventArgs>) new DelegatePlotCommand<OxyMouseDownEventArgs>((Action<IPlotView, IController, OxyMouseDownEventArgs>) ((view, controller, args) => controller.AddMouseManipulator((IView) view, (ManipulatorBase<OxyMouseEventArgs>) new PanManipulator(view), args)));
    PlotCommands.ZoomRectangle = (IViewCommand<OxyMouseDownEventArgs>) new DelegatePlotCommand<OxyMouseDownEventArgs>((Action<IPlotView, IController, OxyMouseDownEventArgs>) ((view, controller, args) => controller.AddMouseManipulator((IView) view, (ManipulatorBase<OxyMouseEventArgs>) new ZoomRectangleManipulator(view), args)));
    PlotCommands.Track = (IViewCommand<OxyMouseDownEventArgs>) new DelegatePlotCommand<OxyMouseDownEventArgs>((Action<IPlotView, IController, OxyMouseDownEventArgs>) ((view, controller, args) =>
    {
      IController controller1 = controller;
      IPlotView plotView = view;
      TrackerManipulator manipulator = new TrackerManipulator(view);
      manipulator.Snap = false;
      manipulator.PointsOnly = false;
      OxyMouseDownEventArgs args1 = args;
      controller1.AddMouseManipulator((IView) plotView, (ManipulatorBase<OxyMouseEventArgs>) manipulator, args1);
    }));
    PlotCommands.SnapTrack = (IViewCommand<OxyMouseDownEventArgs>) new DelegatePlotCommand<OxyMouseDownEventArgs>((Action<IPlotView, IController, OxyMouseDownEventArgs>) ((view, controller, args) =>
    {
      IController controller2 = controller;
      IPlotView plotView = view;
      TrackerManipulator manipulator = new TrackerManipulator(view);
      manipulator.Snap = true;
      manipulator.PointsOnly = false;
      OxyMouseDownEventArgs args2 = args;
      controller2.AddMouseManipulator((IView) plotView, (ManipulatorBase<OxyMouseEventArgs>) manipulator, args2);
    }));
    PlotCommands.PointsOnlyTrack = (IViewCommand<OxyMouseDownEventArgs>) new DelegatePlotCommand<OxyMouseDownEventArgs>((Action<IPlotView, IController, OxyMouseDownEventArgs>) ((view, controller, args) =>
    {
      IController controller3 = controller;
      IPlotView plotView = view;
      TrackerManipulator manipulator = new TrackerManipulator(view);
      manipulator.Snap = false;
      manipulator.PointsOnly = true;
      OxyMouseDownEventArgs args3 = args;
      controller3.AddMouseManipulator((IView) plotView, (ManipulatorBase<OxyMouseEventArgs>) manipulator, args3);
    }));
    PlotCommands.ZoomWheel = (IViewCommand<OxyMouseWheelEventArgs>) new DelegatePlotCommand<OxyMouseWheelEventArgs>((Action<IPlotView, IController, OxyMouseWheelEventArgs>) ((view, controller, args) => PlotCommands.HandleZoomByWheel(view, args)));
    PlotCommands.ZoomWheelFine = (IViewCommand<OxyMouseWheelEventArgs>) new DelegatePlotCommand<OxyMouseWheelEventArgs>((Action<IPlotView, IController, OxyMouseWheelEventArgs>) ((view, controller, args) => PlotCommands.HandleZoomByWheel(view, args, 0.1)));
    PlotCommands.ZoomInAt = (IViewCommand<OxyMouseEventArgs>) new DelegatePlotCommand<OxyMouseEventArgs>((Action<IPlotView, IController, OxyMouseEventArgs>) ((view, controller, args) => PlotCommands.HandleZoomAt(view, args, 0.05)));
    PlotCommands.ZoomOutAt = (IViewCommand<OxyMouseEventArgs>) new DelegatePlotCommand<OxyMouseEventArgs>((Action<IPlotView, IController, OxyMouseEventArgs>) ((view, controller, args) => PlotCommands.HandleZoomAt(view, args, -0.05)));
    PlotCommands.HoverTrack = (IViewCommand<OxyMouseEventArgs>) new DelegatePlotCommand<OxyMouseEventArgs>((Action<IPlotView, IController, OxyMouseEventArgs>) ((view, controller, args) =>
    {
      IController controller4 = controller;
      IPlotView plotView = view;
      TrackerManipulator manipulator = new TrackerManipulator(view);
      manipulator.LockToInitialSeries = false;
      manipulator.Snap = false;
      manipulator.PointsOnly = false;
      OxyMouseEventArgs args4 = args;
      controller4.AddHoverManipulator((IView) plotView, (ManipulatorBase<OxyMouseEventArgs>) manipulator, args4);
    }));
    PlotCommands.HoverSnapTrack = (IViewCommand<OxyMouseEventArgs>) new DelegatePlotCommand<OxyMouseEventArgs>((Action<IPlotView, IController, OxyMouseEventArgs>) ((view, controller, args) =>
    {
      IController controller5 = controller;
      IPlotView plotView = view;
      TrackerManipulator manipulator = new TrackerManipulator(view);
      manipulator.LockToInitialSeries = false;
      manipulator.Snap = true;
      manipulator.PointsOnly = false;
      OxyMouseEventArgs args5 = args;
      controller5.AddHoverManipulator((IView) plotView, (ManipulatorBase<OxyMouseEventArgs>) manipulator, args5);
    }));
    PlotCommands.HoverPointsOnlyTrack = (IViewCommand<OxyMouseEventArgs>) new DelegatePlotCommand<OxyMouseEventArgs>((Action<IPlotView, IController, OxyMouseEventArgs>) ((view, controller, args) =>
    {
      IController controller6 = controller;
      IPlotView plotView = view;
      TrackerManipulator manipulator = new TrackerManipulator(view);
      manipulator.LockToInitialSeries = false;
      manipulator.Snap = false;
      manipulator.PointsOnly = true;
      OxyMouseEventArgs args6 = args;
      controller6.AddHoverManipulator((IView) plotView, (ManipulatorBase<OxyMouseEventArgs>) manipulator, args6);
    }));
    PlotCommands.PanZoomByTouch = (IViewCommand<OxyTouchEventArgs>) new DelegatePlotCommand<OxyTouchEventArgs>((Action<IPlotView, IController, OxyTouchEventArgs>) ((view, controller, args) => controller.AddTouchManipulator((IView) view, (ManipulatorBase<OxyTouchEventArgs>) new TouchManipulator(view), args)));
    PlotCommands.PanLeft = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandlePan(view, (OxyInputEventArgs) args, -0.1, 0.0)));
    PlotCommands.PanRight = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandlePan(view, (OxyInputEventArgs) args, 0.1, 0.0)));
    PlotCommands.PanUp = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandlePan(view, (OxyInputEventArgs) args, 0.0, -0.1)));
    PlotCommands.PanDown = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandlePan(view, (OxyInputEventArgs) args, 0.0, 0.1)));
    PlotCommands.PanLeftFine = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandlePan(view, (OxyInputEventArgs) args, -0.01, 0.0)));
    PlotCommands.PanRightFine = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandlePan(view, (OxyInputEventArgs) args, 0.01, 0.0)));
    PlotCommands.PanUpFine = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandlePan(view, (OxyInputEventArgs) args, 0.0, -0.01)));
    PlotCommands.PanDownFine = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandlePan(view, (OxyInputEventArgs) args, 0.0, 0.01)));
    PlotCommands.ZoomIn = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandleZoomCenter(view, (OxyInputEventArgs) args, 1.0)));
    PlotCommands.ZoomOut = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandleZoomCenter(view, (OxyInputEventArgs) args, -1.0)));
    PlotCommands.ZoomInFine = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandleZoomCenter(view, (OxyInputEventArgs) args, 0.1)));
    PlotCommands.ZoomOutFine = (IViewCommand<OxyKeyEventArgs>) new DelegatePlotCommand<OxyKeyEventArgs>((Action<IPlotView, IController, OxyKeyEventArgs>) ((view, controller, args) => PlotCommands.HandleZoomCenter(view, (OxyInputEventArgs) args, -0.1)));
  }

  public static IViewCommand<OxyKeyEventArgs> Reset { get; private set; }

  public static IViewCommand<OxyMouseEventArgs> ResetAt { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> CopyTextReport { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> CopyCode { get; private set; }

  public static IViewCommand<OxyTouchEventArgs> PanZoomByTouch { get; private set; }

  public static IViewCommand<OxyMouseDownEventArgs> PanAt { get; private set; }

  public static IViewCommand<OxyMouseDownEventArgs> ZoomRectangle { get; private set; }

  public static IViewCommand<OxyMouseWheelEventArgs> ZoomWheel { get; private set; }

  public static IViewCommand<OxyMouseWheelEventArgs> ZoomWheelFine { get; private set; }

  public static IViewCommand<OxyMouseDownEventArgs> Track { get; private set; }

  public static IViewCommand<OxyMouseDownEventArgs> SnapTrack { get; private set; }

  public static IViewCommand<OxyMouseDownEventArgs> PointsOnlyTrack { get; private set; }

  public static IViewCommand<OxyMouseEventArgs> HoverTrack { get; private set; }

  public static IViewCommand<OxyMouseEventArgs> HoverSnapTrack { get; private set; }

  public static IViewCommand<OxyMouseEventArgs> HoverPointsOnlyTrack { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> PanLeft { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> PanRight { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> PanUp { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> PanDown { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> PanLeftFine { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> PanRightFine { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> PanUpFine { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> PanDownFine { get; private set; }

  public static IViewCommand<OxyMouseEventArgs> ZoomInAt { get; private set; }

  public static IViewCommand<OxyMouseEventArgs> ZoomOutAt { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> ZoomIn { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> ZoomOut { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> ZoomInFine { get; private set; }

  public static IViewCommand<OxyKeyEventArgs> ZoomOutFine { get; private set; }

  private static void HandleReset(IPlotView view, OxyInputEventArgs args)
  {
    args.Handled = true;
    view.ActualModel.ResetAllAxes();
    view.InvalidatePlot(false);
  }

  private static void HandleCopyTextReport(IPlotView view, OxyInputEventArgs args)
  {
    args.Handled = true;
    string textReport = view.ActualModel.CreateTextReport();
    view.SetClipboardText(textReport);
  }

  private static void HandleCopyCode(IPlotView view, OxyInputEventArgs args)
  {
    args.Handled = true;
    string code = view.ActualModel.ToCode();
    view.SetClipboardText(code);
  }

  private static void HandleZoomAt(IPlotView view, OxyMouseEventArgs args, double delta)
  {
    new ZoomStepManipulator(view)
    {
      Step = delta,
      FineControl = args.IsControlDown
    }.Started(args);
  }

  private static void HandleZoomByWheel(IPlotView view, OxyMouseWheelEventArgs args, double factor = 1.0)
  {
    new ZoomStepManipulator(view)
    {
      Step = ((double) args.Delta * 0.001 * factor),
      FineControl = args.IsControlDown
    }.Started((OxyMouseEventArgs) args);
  }

  private static void HandleZoomCenter(IPlotView view, OxyInputEventArgs args, double delta)
  {
    args.Handled = true;
    view.ActualModel.ZoomAllAxes(1.0 + delta * 0.12);
    view.InvalidatePlot(false);
  }

  private static void HandlePan(IPlotView view, OxyInputEventArgs args, double dx, double dy)
  {
    args.Handled = true;
    dx *= view.ActualModel.PlotArea.Width;
    dy *= view.ActualModel.PlotArea.Height;
    view.ActualModel.PanAllAxes(dx, dy);
    view.InvalidatePlot(false);
  }
}
