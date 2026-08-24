// Decompiled with JetBrains decompiler
// Type: OxyPlot.PlotController
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class PlotController : ControllerBase, IPlotController, IController
{
  public PlotController()
  {
    this.BindMouseDown(OxyMouseButton.Middle, PlotCommands.ZoomRectangle);
    this.BindMouseDown(OxyMouseButton.Right, OxyModifierKeys.Control, PlotCommands.ZoomRectangle);
    this.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Control | OxyModifierKeys.Alt, PlotCommands.ZoomRectangle);
    this.BindMouseDown(OxyMouseButton.Middle, OxyModifierKeys.None, 2, (IViewCommand<OxyMouseDownEventArgs>) PlotCommands.ResetAt);
    this.BindMouseDown(OxyMouseButton.Right, OxyModifierKeys.Control, 2, (IViewCommand<OxyMouseDownEventArgs>) PlotCommands.ResetAt);
    this.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Control | OxyModifierKeys.Alt, 2, (IViewCommand<OxyMouseDownEventArgs>) PlotCommands.ResetAt);
    this.BindKeyDown(OxyKey.A, PlotCommands.Reset);
    this.BindKeyDown(OxyKey.C, OxyModifierKeys.Control | OxyModifierKeys.Alt, PlotCommands.CopyCode);
    this.BindKeyDown(OxyKey.R, OxyModifierKeys.Control | OxyModifierKeys.Alt, PlotCommands.CopyTextReport);
    this.BindKeyDown(OxyKey.Home, PlotCommands.Reset);
    this.BindCore((OxyInputGesture) new OxyShakeGesture(), (IViewCommand) PlotCommands.Reset);
    this.BindMouseDown(OxyMouseButton.Right, PlotCommands.PanAt);
    this.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Alt, PlotCommands.PanAt);
    this.BindKeyDown(OxyKey.Left, PlotCommands.PanLeft);
    this.BindKeyDown(OxyKey.Right, PlotCommands.PanRight);
    this.BindKeyDown(OxyKey.Up, PlotCommands.PanUp);
    this.BindKeyDown(OxyKey.Down, PlotCommands.PanDown);
    this.BindKeyDown(OxyKey.Left, OxyModifierKeys.Control, PlotCommands.PanLeftFine);
    this.BindKeyDown(OxyKey.Right, OxyModifierKeys.Control, PlotCommands.PanRightFine);
    this.BindKeyDown(OxyKey.Up, OxyModifierKeys.Control, PlotCommands.PanUpFine);
    this.BindKeyDown(OxyKey.Down, OxyModifierKeys.Control, PlotCommands.PanDownFine);
    this.BindTouchDown(PlotCommands.PanZoomByTouch);
    this.BindMouseDown(OxyMouseButton.Left, PlotCommands.SnapTrack);
    this.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Control, PlotCommands.Track);
    this.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Shift, PlotCommands.PointsOnlyTrack);
    this.BindMouseDown(OxyMouseButton.XButton1, (IViewCommand<OxyMouseDownEventArgs>) PlotCommands.ZoomInAt);
    this.BindMouseDown(OxyMouseButton.XButton2, (IViewCommand<OxyMouseDownEventArgs>) PlotCommands.ZoomOutAt);
    this.BindMouseWheel(PlotCommands.ZoomWheel);
    this.BindMouseWheel(OxyModifierKeys.Control, PlotCommands.ZoomWheelFine);
    this.BindKeyDown(OxyKey.Add, PlotCommands.ZoomIn);
    this.BindKeyDown(OxyKey.Subtract, PlotCommands.ZoomOut);
    this.BindKeyDown(OxyKey.PageUp, PlotCommands.ZoomIn);
    this.BindKeyDown(OxyKey.PageDown, PlotCommands.ZoomOut);
    this.BindKeyDown(OxyKey.Add, OxyModifierKeys.Control, PlotCommands.ZoomInFine);
    this.BindKeyDown(OxyKey.Subtract, OxyModifierKeys.Control, PlotCommands.ZoomOutFine);
    this.BindKeyDown(OxyKey.PageUp, OxyModifierKeys.Control, PlotCommands.ZoomInFine);
    this.BindKeyDown(OxyKey.PageDown, OxyModifierKeys.Control, PlotCommands.ZoomOutFine);
  }
}
