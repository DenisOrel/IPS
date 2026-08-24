// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.DesignTime.CSharpFormatIndentsAction
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.Common.DesignTime.TextEditorActions;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.CSharp.DesignTime;

internal sealed class CSharpFormatIndentsAction : TextEditorAction
{
  protected override void DoInvoke(ITextEditor textEditor)
  {
    if (textEditor.Document.Length == 0)
      return;
    IEditableTextLine lineByOffset = textEditor.Document.GetLineByOffset(0);
    textEditor.Document.BeginUpdate();
    try
    {
      this.FormatIndents(textEditor, lineByOffset);
    }
    finally
    {
      textEditor.Document.EndUpdate();
    }
  }

  private void FormatIndents(ITextEditor textEditor, IEditableTextLine firstLine)
  {
    List<CSharpFormatIndentsAction.IndentStruct> indents = new List<CSharpFormatIndentsAction.IndentStruct>();
    int indentIndex = 0;
    for (IEditableTextLine editableTextLine = firstLine; editableTextLine != null; editableTextLine = editableTextLine.TryGetNextLine())
    {
      CSharpFormatIndentsAction.IndentStruct indentStruct = new CSharpFormatIndentsAction.IndentStruct();
      indents.Add(indentStruct);
      string text1 = textEditor.Document.GetText(editableTextLine.Offset, editableTextLine.Length);
      string text2 = text1.TrimStart('\t', ' ').TrimEnd(' ');
      if (text2.StartsWith("{"))
        indentStruct.OpenBracket = CSharpFormatIndentsAction.BracketType.Open;
      else if (text2.StartsWith("}"))
        indentStruct.OpenBracket = CSharpFormatIndentsAction.BracketType.Close;
      else if (text2.StartsWith("if") && text2.EndsWith(")"))
        indentStruct.IfStatement = true;
      else if (text2.StartsWith("else") && (text2.EndsWith(")") || text2.EndsWith("else")))
        indentStruct.IfStatement = true;
      CSharpFormatIndentsAction.IndentStruct prevOpenBracket = this.FindPrevOpenBracket((IList<CSharpFormatIndentsAction.IndentStruct>) indents, indentIndex);
      if (prevOpenBracket != null)
      {
        if (indentStruct.OpenBracket == CSharpFormatIndentsAction.BracketType.None || indentStruct.OpenBracket == CSharpFormatIndentsAction.BracketType.Open)
          indentStruct.Indent = prevOpenBracket.Indent + 1;
        if (indentStruct.OpenBracket == CSharpFormatIndentsAction.BracketType.None && indentIndex > 0 && indents[indentIndex - 1].IfStatement)
          ++indentStruct.Indent;
        if (indentStruct.OpenBracket == CSharpFormatIndentsAction.BracketType.Close)
        {
          indentStruct.Indent = prevOpenBracket.Indent;
          prevOpenBracket.Closed = true;
        }
      }
      if (indentStruct.Indent != 0)
        text2 = new string(' ', indentStruct.Indent * 4) + text2;
      if (text2 != text1)
        textEditor.Document.Replace(editableTextLine.Offset, editableTextLine.Length, text2);
      ++indentIndex;
    }
  }

  private CSharpFormatIndentsAction.IndentStruct FindPrevOpenBracket(
    IList<CSharpFormatIndentsAction.IndentStruct> indents,
    int indentIndex)
  {
    for (int index = indentIndex - 1; index >= 0; --index)
    {
      if (!indents[index].Closed && indents[index].OpenBracket == CSharpFormatIndentsAction.BracketType.Open)
        return indents[index];
    }
    return (CSharpFormatIndentsAction.IndentStruct) null;
  }

  private sealed class IndentStruct
  {
    public bool IfStatement { get; set; }

    public int Indent { get; set; }

    public CSharpFormatIndentsAction.BracketType OpenBracket { get; set; }

    public bool Closed { get; set; }
  }

  private enum BracketType
  {
    None,
    Open,
    Close,
  }
}
