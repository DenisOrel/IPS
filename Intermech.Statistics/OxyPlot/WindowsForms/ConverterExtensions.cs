// Decompiled with JetBrains decompiler
// Type: OxyPlot.WindowsForms.ConverterExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace OxyPlot.WindowsForms;

public static class ConverterExtensions
{
  public static double DistanceTo(this Point p1, Point p2)
  {
    double num1 = (double) (p1.X - p2.X);
    double num2 = (double) (p1.Y - p2.Y);
    return Math.Sqrt(num1 * num1 + num2 * num2);
  }

  public static Brush ToBrush(this OxyColor c) => (Brush) new SolidBrush(c.ToColor());

  public static Color ToColor(this OxyColor c)
  {
    return Color.FromArgb((int) c.A, (int) c.R, (int) c.G, (int) c.B);
  }

  public static OxyPlot.HorizontalAlignment ToHorizontalTextAlign(this System.Windows.Forms.HorizontalAlignment alignment)
  {
    if (alignment == System.Windows.Forms.HorizontalAlignment.Right)
      return OxyPlot.HorizontalAlignment.Right;
    return alignment == System.Windows.Forms.HorizontalAlignment.Center ? OxyPlot.HorizontalAlignment.Center : OxyPlot.HorizontalAlignment.Left;
  }

  public static OxyColor ToOxyColor(this Color color)
  {
    return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
  }

  public static OxyColor ToOxyColor(this Brush brush)
  {
    return !(brush is SolidBrush solidBrush) ? OxyColors.Undefined : solidBrush.Color.ToOxyColor();
  }

  public static Point ToPoint(this ScreenPoint pt, bool aliased)
  {
    return aliased ? new Point((int) pt.X, (int) pt.Y) : new Point((int) Math.Round(pt.X), (int) Math.Round(pt.Y));
  }

  public static Rectangle ToRect(this OxyRect r, bool aliased)
  {
    if (!aliased)
      return new Rectangle((int) Math.Round(r.Left), (int) Math.Round(r.Top), (int) Math.Round(r.Width), (int) Math.Round(r.Height));
    int left = (int) r.Left;
    int top = (int) r.Top;
    int right = (int) r.Right;
    int bottom = (int) r.Bottom;
    return new Rectangle(left, top, right - left, bottom - top);
  }

  public static ScreenPoint ToScreenPoint(this Point pt)
  {
    return new ScreenPoint((double) pt.X, (double) pt.Y);
  }

  public static ScreenPoint[] ToScreenPointArray(this Point[] points)
  {
    if (points == null)
      return (ScreenPoint[]) null;
    ScreenPoint[] screenPointArray = new ScreenPoint[points.Length];
    for (int index = 0; index < points.Length; ++index)
      screenPointArray[index] = points[index].ToScreenPoint();
    return screenPointArray;
  }

  public static OxyMouseButton Convert(this MouseButtons button)
  {
    switch (button)
    {
      case MouseButtons.Left:
        return OxyMouseButton.Left;
      case MouseButtons.Right:
        return OxyMouseButton.Right;
      case MouseButtons.Middle:
        return OxyMouseButton.Middle;
      case MouseButtons.XButton1:
        return OxyMouseButton.XButton1;
      case MouseButtons.XButton2:
        return OxyMouseButton.XButton2;
      default:
        return OxyMouseButton.None;
    }
  }

  public static OxyMouseWheelEventArgs ToMouseWheelEventArgs(
    this MouseEventArgs e,
    OxyModifierKeys modifiers)
  {
    OxyMouseWheelEventArgs mouseWheelEventArgs = new OxyMouseWheelEventArgs();
    mouseWheelEventArgs.Position = e.Location.ToScreenPoint();
    mouseWheelEventArgs.ModifierKeys = modifiers;
    mouseWheelEventArgs.Delta = e.Delta;
    return mouseWheelEventArgs;
  }

  public static OxyMouseDownEventArgs ToMouseDownEventArgs(
    this MouseEventArgs e,
    OxyModifierKeys modifiers)
  {
    OxyMouseDownEventArgs mouseDownEventArgs = new OxyMouseDownEventArgs();
    mouseDownEventArgs.ChangedButton = e.Button.Convert();
    mouseDownEventArgs.ClickCount = e.Clicks;
    mouseDownEventArgs.Position = e.Location.ToScreenPoint();
    mouseDownEventArgs.ModifierKeys = modifiers;
    return mouseDownEventArgs;
  }

  public static OxyMouseEventArgs ToMouseUpEventArgs(
    this MouseEventArgs e,
    OxyModifierKeys modifiers)
  {
    OxyMouseEventArgs mouseUpEventArgs = new OxyMouseEventArgs();
    mouseUpEventArgs.Position = e.Location.ToScreenPoint();
    mouseUpEventArgs.ModifierKeys = modifiers;
    return mouseUpEventArgs;
  }

  public static OxyMouseEventArgs ToMouseEventArgs(this MouseEventArgs e, OxyModifierKeys modifiers)
  {
    OxyMouseEventArgs mouseEventArgs = new OxyMouseEventArgs();
    mouseEventArgs.Position = e.Location.ToScreenPoint();
    mouseEventArgs.ModifierKeys = modifiers;
    return mouseEventArgs;
  }

  public static OxyMouseEventArgs ToMouseEventArgs(this EventArgs e, OxyModifierKeys modifiers)
  {
    OxyMouseEventArgs mouseEventArgs = new OxyMouseEventArgs();
    mouseEventArgs.ModifierKeys = modifiers;
    return mouseEventArgs;
  }

  public static OxyKey Convert(this Keys k)
  {
    switch (k)
    {
      case Keys.Back:
        return OxyKey.Backspace;
      case Keys.Tab:
        return OxyKey.Tab;
      case Keys.Return:
        return OxyKey.Enter;
      case Keys.Escape:
        return OxyKey.Escape;
      case Keys.Space:
        return OxyKey.Space;
      case Keys.Prior:
        return OxyKey.PageUp;
      case Keys.Next:
        return OxyKey.PageDown;
      case Keys.End:
        return OxyKey.End;
      case Keys.Home:
        return OxyKey.Home;
      case Keys.Left:
        return OxyKey.Left;
      case Keys.Up:
        return OxyKey.Up;
      case Keys.Right:
        return OxyKey.Right;
      case Keys.Down:
        return OxyKey.Down;
      case Keys.Insert:
        return OxyKey.Insert;
      case Keys.Delete:
        return OxyKey.Delete;
      case Keys.D0:
        return OxyKey.D0;
      case Keys.D1:
        return OxyKey.D1;
      case Keys.D2:
        return OxyKey.D2;
      case Keys.D3:
        return OxyKey.D3;
      case Keys.D4:
        return OxyKey.D4;
      case Keys.D5:
        return OxyKey.D5;
      case Keys.D6:
        return OxyKey.D6;
      case Keys.D7:
        return OxyKey.D7;
      case Keys.D8:
        return OxyKey.D8;
      case Keys.D9:
        return OxyKey.D9;
      case Keys.A:
        return OxyKey.A;
      case Keys.B:
        return OxyKey.B;
      case Keys.C:
        return OxyKey.C;
      case Keys.D:
        return OxyKey.D;
      case Keys.E:
        return OxyKey.E;
      case Keys.F:
        return OxyKey.F;
      case Keys.G:
        return OxyKey.G;
      case Keys.H:
        return OxyKey.H;
      case Keys.I:
        return OxyKey.I;
      case Keys.J:
        return OxyKey.J;
      case Keys.K:
        return OxyKey.K;
      case Keys.L:
        return OxyKey.L;
      case Keys.M:
        return OxyKey.M;
      case Keys.N:
        return OxyKey.N;
      case Keys.O:
        return OxyKey.O;
      case Keys.P:
        return OxyKey.P;
      case Keys.Q:
        return OxyKey.Q;
      case Keys.R:
        return OxyKey.R;
      case Keys.S:
        return OxyKey.S;
      case Keys.T:
        return OxyKey.T;
      case Keys.U:
        return OxyKey.U;
      case Keys.V:
        return OxyKey.V;
      case Keys.W:
        return OxyKey.W;
      case Keys.X:
        return OxyKey.X;
      case Keys.Y:
        return OxyKey.Y;
      case Keys.Z:
        return OxyKey.Z;
      case Keys.NumPad0:
        return OxyKey.NumPad0;
      case Keys.NumPad1:
        return OxyKey.NumPad1;
      case Keys.NumPad2:
        return OxyKey.NumPad2;
      case Keys.NumPad3:
        return OxyKey.NumPad3;
      case Keys.NumPad4:
        return OxyKey.NumPad4;
      case Keys.NumPad5:
        return OxyKey.NumPad5;
      case Keys.NumPad6:
        return OxyKey.NumPad6;
      case Keys.NumPad7:
        return OxyKey.NumPad7;
      case Keys.NumPad8:
        return OxyKey.NumPad8;
      case Keys.NumPad9:
        return OxyKey.NumPad9;
      case Keys.Multiply:
        return OxyKey.Multiply;
      case Keys.Add:
        return OxyKey.Add;
      case Keys.Subtract:
        return OxyKey.Subtract;
      case Keys.Decimal:
        return OxyKey.Decimal;
      case Keys.Divide:
        return OxyKey.Divide;
      case Keys.F1:
        return OxyKey.F1;
      case Keys.F2:
        return OxyKey.F2;
      case Keys.F3:
        return OxyKey.F3;
      case Keys.F4:
        return OxyKey.F4;
      case Keys.F5:
        return OxyKey.F5;
      case Keys.F6:
        return OxyKey.F6;
      case Keys.F7:
        return OxyKey.F7;
      case Keys.F8:
        return OxyKey.F8;
      case Keys.F9:
        return OxyKey.F9;
      case Keys.F10:
        return OxyKey.F10;
      case Keys.F11:
        return OxyKey.F11;
      case Keys.F12:
        return OxyKey.F12;
      default:
        return OxyKey.Unknown;
    }
  }
}
