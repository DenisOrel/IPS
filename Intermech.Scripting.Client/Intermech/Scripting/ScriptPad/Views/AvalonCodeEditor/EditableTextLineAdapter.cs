// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.EditableTextLineAdapter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.AvalonEdit.Document;
using Intermech.Scripting.Common.DesignTime;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class EditableTextLineAdapter : IEditableTextLine
{
  private EditableTextDocumentAdapter documentAdapter;
  private DocumentLine line;

  public EditableTextLineAdapter(EditableTextDocumentAdapter documentAdapter, DocumentLine line)
  {
    this.documentAdapter = documentAdapter;
    this.line = line;
  }

  public int Offset => this.line.Offset;

  public int Length => this.line.Length;

  public IEditableTextLine TryGetPreviousLine()
  {
    DocumentLine previousLine = this.line.PreviousLine;
    return previousLine != null ? (IEditableTextLine) this.documentAdapter.GetOrCreateLineAdapter(previousLine) : (IEditableTextLine) null;
  }

  public IEditableTextLine TryGetNextLine()
  {
    DocumentLine nextLine = this.line.NextLine;
    return nextLine != null ? (IEditableTextLine) this.documentAdapter.GetOrCreateLineAdapter(nextLine) : (IEditableTextLine) null;
  }
}
