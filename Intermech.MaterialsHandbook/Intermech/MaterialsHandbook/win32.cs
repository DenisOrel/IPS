// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.win32
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class win32
{
  public const int WM_LBUTTONDBLCLK = 515;
  public const int WM_LBUTTONDOWN = 513;
  public const int WM_LBUTTONUP = 514;
  public const int WM_MOUSELEAVE = 675;
  public const int WM_MOUSEMOVE = 512 /*0x0200*/;
  public const int WM_PAINT = 15;
  public const int WM_RBUTTONDOWN = 516;
  public const int WM_HSCROLL = 276;
  public const int WM_VSCROLL = 277;
  public const int WM_PRINT = 791;
  public const int EM_POSFROMCHAR = 214;
  private const long PRF_CLIENT = 4;
  private const long PRF_ERASEBKGND = 8;

  [DllImport("USER32.DLL")]
  public static extern bool PostMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

  [DllImport("USER32.DLL")]
  public static extern uint GetCaretBlinkTime();

  [DllImport("USER32.DLL")]
  public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);

  public static bool CaptureWindow(Control control, ref Bitmap bitmap)
  {
    using (Graphics graphics = Graphics.FromImage((Image) bitmap))
    {
      IntPtr lParam = new IntPtr(12);
      IntPtr hdc = graphics.GetHdc();
      win32.SendMessage(control.Handle, 791, hdc, lParam);
      graphics.ReleaseHdc(hdc);
    }
    return true;
  }
}
