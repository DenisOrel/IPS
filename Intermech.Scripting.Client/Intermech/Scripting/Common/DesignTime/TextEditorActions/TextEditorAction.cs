// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.TextEditorActions.TextEditorAction
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime.TextEditorActions;

internal abstract class TextEditorAction : ITextEditorAction
{
  public void Invoke(ITextEditor textEditor)
  {
    if (textEditor == null)
      throw new ArgumentNullException(nameof (textEditor));
    this.DoInvoke(textEditor);
  }

  protected abstract void DoInvoke(ITextEditor textEditor);

  protected List<int> GetSelectionLineOffsets(ITextEditor textEditor)
  {
    List<int> selectionLineOffsets = new List<int>();
    if (textEditor.SelectionLength == 0)
    {
      IEditableTextLine lineByOffset = textEditor.Document.GetLineByOffset(textEditor.CaretOffset);
      selectionLineOffsets.Add(lineByOffset.Offset);
    }
    else
    {
      int selectionStart = textEditor.SelectionStart;
      int num1 = selectionStart + textEditor.SelectionLength;
      IEditableTextLine editableTextLine = textEditor.Document.GetLineByOffset(selectionStart);
      int num2 = editableTextLine.Offset;
      do
      {
        selectionLineOffsets.Add(num2);
        editableTextLine = editableTextLine.TryGetNextLine();
        num2 = editableTextLine != null ? editableTextLine.Offset : num1;
      }
      while (num2 < num1);
    }
    return selectionLineOffsets;
  }
}
