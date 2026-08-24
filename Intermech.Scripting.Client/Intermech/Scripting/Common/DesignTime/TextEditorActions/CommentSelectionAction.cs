// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.TextEditorActions.CommentSelectionAction
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime.TextEditorActions;

internal sealed class CommentSelectionAction : TextEditorAction
{
  private string lineCommentMarker;

  public CommentSelectionAction(string lineCommentMarker)
  {
    this.lineCommentMarker = !string.IsNullOrEmpty(lineCommentMarker) ? lineCommentMarker : throw new ArgumentException("Не задан признак строкового комментария.", nameof (lineCommentMarker));
  }

  protected override void DoInvoke(ITextEditor textEditor)
  {
    List<int> selectionLineOffsets = this.GetSelectionLineOffsets(textEditor);
    selectionLineOffsets.Reverse();
    textEditor.Document.BeginUpdate();
    try
    {
      int num1 = -1;
      for (int index = 0; index < selectionLineOffsets.Count; ++index)
      {
        IEditableTextLine lineByOffset = textEditor.Document.GetLineByOffset(selectionLineOffsets[index]);
        string text = textEditor.Document.GetText(selectionLineOffsets[index], lineByOffset.Length);
        int num2 = text.Length - text.TrimStart().Length;
        if (num1 == -1)
          num1 = num2;
        else if (num1 > num2)
          num1 = num2;
      }
      for (int index = 0; index < selectionLineOffsets.Count; ++index)
        textEditor.Document.Insert(selectionLineOffsets[index] + num1, this.lineCommentMarker);
    }
    finally
    {
      textEditor.Document.EndUpdate();
    }
  }
}
