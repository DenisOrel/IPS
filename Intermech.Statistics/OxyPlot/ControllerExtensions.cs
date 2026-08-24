// Decompiled with JetBrains decompiler
// Type: OxyPlot.ControllerExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public static class ControllerExtensions
{
  public static void BindKeyDown(
    this IController controller,
    OxyKey key,
    IViewCommand<OxyKeyEventArgs> command)
  {
    controller.Bind(new OxyKeyGesture(key), command);
  }

  public static void BindKeyDown(
    this IController controller,
    OxyKey key,
    OxyModifierKeys modifiers,
    IViewCommand<OxyKeyEventArgs> command)
  {
    controller.Bind(new OxyKeyGesture(key, modifiers), command);
  }

  public static void BindMouseDown(
    this IController controller,
    OxyMouseButton mouseButton,
    IViewCommand<OxyMouseDownEventArgs> command)
  {
    controller.Bind(new OxyMouseDownGesture(mouseButton), command);
  }

  public static void BindMouseDown(
    this IController controller,
    OxyMouseButton mouseButton,
    OxyModifierKeys modifiers,
    IViewCommand<OxyMouseDownEventArgs> command)
  {
    controller.Bind(new OxyMouseDownGesture(mouseButton, modifiers), command);
  }

  public static void BindMouseDown(
    this IController controller,
    OxyMouseButton mouseButton,
    OxyModifierKeys modifiers,
    int clickCount,
    IViewCommand<OxyMouseDownEventArgs> command)
  {
    controller.Bind(new OxyMouseDownGesture(mouseButton, modifiers, clickCount), command);
  }

  public static void BindTouchDown(
    this IController controller,
    IViewCommand<OxyTouchEventArgs> command)
  {
    controller.Bind(new OxyTouchGesture(), command);
  }

  public static void BindMouseEnter(
    this IController controller,
    IViewCommand<OxyMouseEventArgs> command)
  {
    controller.Bind(new OxyMouseEnterGesture(), command);
  }

  public static void BindMouseWheel(
    this IController controller,
    IViewCommand<OxyMouseWheelEventArgs> command)
  {
    controller.Bind(new OxyMouseWheelGesture(), command);
  }

  public static void BindMouseWheel(
    this IController controller,
    OxyModifierKeys modifiers,
    IViewCommand<OxyMouseWheelEventArgs> command)
  {
    controller.Bind(new OxyMouseWheelGesture(modifiers), command);
  }

  public static void UnbindMouseDown(
    this IController controller,
    OxyMouseButton mouseButton,
    OxyModifierKeys modifiers = OxyModifierKeys.None,
    int clickCount = 1)
  {
    controller.Unbind((OxyInputGesture) new OxyMouseDownGesture(mouseButton, modifiers, clickCount));
  }

  public static void UnbindKeyDown(
    this IController controller,
    OxyKey key,
    OxyModifierKeys modifiers = OxyModifierKeys.None)
  {
    controller.Unbind((OxyInputGesture) new OxyKeyGesture(key, modifiers));
  }

  public static void UnbindMouseEnter(this IController controller)
  {
    controller.Unbind((OxyInputGesture) new OxyMouseEnterGesture());
  }

  public static void UnbindTouchDown(this IController controller)
  {
    controller.Unbind((OxyInputGesture) new OxyTouchGesture());
  }

  public static void UnbindMouseWheel(this IController controller)
  {
    controller.Unbind((OxyInputGesture) new OxyMouseWheelGesture());
  }
}
