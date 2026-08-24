// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.TextSources.ReadOnlyTextSource
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.Editor;
using Intermech.Scripting.Common.DesignTime;
using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace ICSharpCode.NRefactory.TextSources;

internal class ReadOnlyTextSource : ITextSource
{
  private IReadOnlyTextDocument document;
  private ReadOnlyTextDocumentSearchTool searchTool;
  private ITextSourceVersion version;

  public ReadOnlyTextSource(IReadOnlyTextDocument document)
  {
    this.document = document != null ? document : throw new ArgumentNullException(nameof (document));
  }

  public ReadOnlyTextSource(IReadOnlyTextDocument document, ITextSourceVersion version)
  {
    this.document = document != null ? document : throw new ArgumentNullException(nameof (document));
    this.version = version;
  }

  public ITextSourceVersion Version
  {
    [DebuggerStepThrough] get => this.version;
  }

  public int TextLength
  {
    [DebuggerStepThrough] get => this.document.Length;
  }

  public string Text
  {
    [DebuggerStepThrough] get => this.document.GetText(0, this.document.Length);
  }

  public ITextSource CreateSnapshot() => (ITextSource) this;

  public ITextSource CreateSnapshot(int offset, int length)
  {
    return (ITextSource) new ReadOnlyTextSource((IReadOnlyTextDocument) new ReadOnlyTextFragment(this.document, offset, length));
  }

  public TextReader CreateReader() => (TextReader) new ReadOnlyTextDocumentReader(this.document);

  public TextReader CreateReader(int offset, int length)
  {
    return (TextReader) new ReadOnlyTextDocumentReader((IReadOnlyTextDocument) new ReadOnlyTextFragment(this.document, offset, length));
  }

  public void WriteTextTo(TextWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    for (int offset = 0; offset < this.document.Length; ++offset)
      writer.Write(this.document.GetCharAt(offset));
  }

  public void WriteTextTo(TextWriter writer, int offset, int length)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    ReadOnlyTextFragment onlyTextFragment = new ReadOnlyTextFragment(this.document, offset, length);
    for (int offset1 = 0; offset1 < onlyTextFragment.Length; ++offset1)
      writer.Write(onlyTextFragment.GetCharAt(offset1));
  }

  public char GetCharAt(int offset) => this.document.GetCharAt(offset);

  public string GetText(int offset, int length) => this.document.GetText(offset, length);

  public string GetText(ISegment segment)
  {
    if (segment == null)
      throw new ArgumentNullException(nameof (segment));
    return this.document.GetText(segment.Offset, segment.Length);
  }

  public int IndexOf(char c, int startIndex, int count)
  {
    return this.GetSearchTool().IndexOf(c, startIndex, count);
  }

  public int IndexOfAny(char[] anyOf, int startIndex, int count)
  {
    return this.GetSearchTool().IndexOfAny(anyOf, startIndex, count);
  }

  public int IndexOf(
    string searchText,
    int startIndex,
    int count,
    StringComparison comparisonType)
  {
    return this.GetSearchTool().IndexOf(searchText, startIndex, count, comparisonType);
  }

  public int LastIndexOf(char c, int startIndex, int count)
  {
    return this.GetSearchTool().LastIndexOf(c, startIndex + count - 1, count);
  }

  public int LastIndexOf(
    string searchText,
    int startIndex,
    int count,
    StringComparison comparisonType)
  {
    return this.GetSearchTool().LastIndexOf(searchText, startIndex + count - 1, count, comparisonType);
  }

  private ReadOnlyTextDocumentSearchTool GetSearchTool()
  {
    if (this.searchTool == null)
      this.searchTool = new ReadOnlyTextDocumentSearchTool(this.document);
    return this.searchTool;
  }
}
