// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.TextEditorActions.UncommentSelectionAction
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime.TextEditorActions;

internal sealed class UncommentSelectionAction : TextEditorAction
{
  private string lineCommentMarker;

  public UncommentSelectionAction(string lineCommentMarker)
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
      for (int index = 0; index < selectionLineOffsets.Count; ++index)
      {
        IEditableTextLine lineByOffset = textEditor.Document.GetLineByOffset(selectionLineOffsets[index]);
        string text = textEditor.Document.GetText(selectionLineOffsets[index], lineByOffset.Length);
        string str = text.TrimStart();
        if (str.StartsWith(this.lineCommentMarker))
        {
          int num = text.Length - str.Length;
          textEditor.Document.Remove(selectionLineOffsets[index] + num, this.lineCommentMarker.Length);
        }
      }
    }
    finally
    {
      textEditor.Document.EndUpdate();
    }
  }
}
