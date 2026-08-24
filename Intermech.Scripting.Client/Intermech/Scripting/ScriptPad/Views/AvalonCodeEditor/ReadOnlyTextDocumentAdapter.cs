// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.ReadOnlyTextDocumentAdapter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.AvalonEdit.Document;
using Intermech.Scripting.Common.DesignTime;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class ReadOnlyTextDocumentAdapter : IReadOnlyTextDocument, ITextDocument
{
  private TextDocument document;

  public ReadOnlyTextDocumentAdapter(TextDocument document) => this.document = document;

  public int Length => this.document.TextLength;

  public char GetCharAt(int offset) => this.document.GetCharAt(offset);

  public string GetText(int offset, int length) => this.document.GetText(offset, length);
}
