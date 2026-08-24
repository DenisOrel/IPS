// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.TextEditorHotkeyHelper
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Windows.Input;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal class TextEditorHotkeyHelper
{
  public static void PatchHotkeys(TextEditor textEditor)
  {
    TextArea textArea = textEditor != null ? textEditor.TextArea : throw new ArgumentNullException(nameof (textEditor));
    if (textArea == null)
      throw new NullReferenceException("TextArea");
    foreach (CommandBinding commandBinding in textArea.CommandBindings)
    {
      if (commandBinding.Command is RoutedCommand command && command.Equals((object) ApplicationCommands.Find))
        commandBinding.Command = (ICommand) TextEditorCommands.IncrementalSearch;
    }
    KeyGesture gesture1 = new KeyGesture(Key.I, ModifierKeys.Control, "Ctrl+I");
    KeyGesture gesture2 = new KeyGesture(Key.Oem6, ModifierKeys.Control, "Ctrl+]");
    textArea.InputBindings.Add(new InputBinding((ICommand) TextEditorCommands.IncrementalSearch, (InputGesture) gesture1));
    textArea.InputBindings.Add(new InputBinding((ICommand) AvalonEditCommands.IndentSelection, (InputGesture) gesture2));
  }

  private static void ReplaceCommandHokeys(RoutedCommand command, InputGesture hotkey)
  {
    command.InputGestures.Clear();
    command.InputGestures.Add(hotkey);
  }
}
