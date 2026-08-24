// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.IScriptCodeEditorControl
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal interface IScriptCodeEditorControl
{
  void Initialize(LanguageInfo languageInfo, string scriptCode, bool readOnlyMode);

  void SetContextMenuActions(IList<ITextEditorUIAction> contextMenuActions = null);

  void SetCodeCompletionProvider(ICodeCompletionProvider codeCompletionProvider = null);

  void SetCodeFoldingProvider(CodeFoldingProvider provider = null);

  void SetCodeHoverInfoProvider(HoverInfoProvider provider = null);

  void SetFont(string fontFamily, int fontSize);

  TextCaretPosition GetCaretPosition();

  int GetCaretOffset();

  void FocusAt(TextCaretPosition caretPosition);

  event EventHandler CaretPositionChanged;

  string GetScriptCode();

  ITextEditor GetScriptCodeAsTextEditor();

  event EventHandler ScriptCodeChanged;

  event EventHandler<ScriptTextChangedEventArgs> ScriptTextChanged;

  void Select(TextCaretPosition startPosition, TextCaretPosition endPosition);

  bool HasSelection();

  TextSegment TryGetSelection();

  event EventHandler SelectionChanged;

  void ShowFindReplaceDialog();

  void UpdateRegionFoldings();
}
