// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.FindReplaceOwnerWindowAdapter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.UI.Wpf.Controls;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class FindReplaceOwnerWindowAdapter : IFindReplaceTextEditorWindow
{
  private Form form;

  public FindReplaceOwnerWindowAdapter(Form form)
  {
    this.form = form != null ? form : throw new ArgumentNullException(nameof (form));
  }

  public int Left => this.form.Location.X;

  public int Top => this.form.Location.Y;

  public int Width => this.form.Size.Width;

  public int Height => this.form.Size.Height;

  public void SetOwnerWindow(Window findReplaceWindow)
  {
    if (findReplaceWindow == null)
      throw new ArgumentNullException(nameof (findReplaceWindow));
    new WindowInteropHelper(findReplaceWindow).Owner = this.form.Handle;
    ElementHost.EnableModelessKeyboardInterop(findReplaceWindow);
  }
}
