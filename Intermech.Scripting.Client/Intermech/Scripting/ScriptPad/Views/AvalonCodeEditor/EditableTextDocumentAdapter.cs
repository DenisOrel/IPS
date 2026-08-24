// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.EditableTextDocumentAdapter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.AvalonEdit.Document;
using Intermech.Scripting.Common.DesignTime;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class EditableTextDocumentAdapter : IEditableTextDocument, ITextDocument
{
  private TextDocument document;
  private Dictionary<DocumentLine, EditableTextLineAdapter> lineAdapters;

  public EditableTextDocumentAdapter(TextDocument document)
  {
    this.document = document;
    this.lineAdapters = new Dictionary<DocumentLine, EditableTextLineAdapter>();
  }

  internal EditableTextLineAdapter GetOrCreateLineAdapter(DocumentLine line)
  {
    EditableTextLineAdapter lineAdapter;
    if (!this.lineAdapters.TryGetValue(line, out lineAdapter))
    {
      lineAdapter = new EditableTextLineAdapter(this, line);
      this.lineAdapters.Add(line, lineAdapter);
    }
    return lineAdapter;
  }

  public int Length => this.document.TextLength;

  public char GetCharAt(int offset) => this.document.GetCharAt(offset);

  public string GetText(int offset, int length) => this.document.GetText(offset, length);

  public void BeginUpdate() => this.document.BeginUpdate();

  public void EndUpdate() => this.document.EndUpdate();

  public void Insert(int offset, string text) => this.document.Insert(offset, text);

  public void Remove(int offset, int length) => this.document.Remove(offset, length);

  public void Replace(int offset, int length, string text)
  {
    this.document.Replace(offset, length, text);
  }

  public IEditableTextLine GetLineByOffset(int offset)
  {
    return (IEditableTextLine) this.GetOrCreateLineAdapter(this.document.GetLineByOffset(offset));
  }
}
