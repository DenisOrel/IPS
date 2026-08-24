// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.IIDEView
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Mvp;
using Intermech.Mvp.Components;
using Intermech.Scripting.Common;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal interface IIDEView : IView
{
  IScriptWindowCollection ScriptWindows { get; }

  IErrorsView ErrorsView { get; }

  IScriptOutputStream OutputView { get; }

  bool MaximizedAtStartup { get; set; }

  IMultiCommand NewCommand { get; }

  IClickCommand OpenCommand { get; }

  IClickCommand ReplaceWithCommand { get; }

  IClickCommand SaveCommand { get; }

  IClickCommand SaveAsCommand { get; }

  IClickCommand SaveCopyCommand { get; }

  IClickCommand CutCommand { get; }

  IClickCommand CopyCommand { get; }

  IClickCommand PasteCommand { get; }

  IClickCommand UndoCommand { get; }

  IClickCommand RedoCommand { get; }

  IClickCommand CommentSelectionCommand { get; }

  IClickCommand UncommentSelectionCommand { get; }

  IClickCommand FormatIndentsCommand { get; }

  IClickCommand FindReplaceCommand { get; }

  IClickCommand RunCommand { get; }

  IClickCommand EditRunParametersCommand { get; }

  IClickCommand StopRunCommand { get; }

  IClickCommand EditIDESettingsCommand { get; }

  IMultiCommand EditExecutorSettingsCommand { get; }

  void ShowScriptLanguage(string languageName);

  void ShowScriptEncoding(string encoding);

  void ShowScriptCodeEditorCaretPosition(string line, string column);

  event KeyEventHandler HotkeyPressed;

  event EventHandler<CancelEventArgs> ViewClosing;
}
