// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.TextEditorCommands
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System.Windows.Input;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal static class TextEditorCommands
{
  public static readonly RoutedCommand IncrementalSearch = new RoutedCommand(nameof (IncrementalSearch), typeof (TextEditorCommands), new InputGestureCollection()
  {
    (InputGesture) new KeyGesture(Key.I, ModifierKeys.Control, "Ctrl+I")
  });
}
