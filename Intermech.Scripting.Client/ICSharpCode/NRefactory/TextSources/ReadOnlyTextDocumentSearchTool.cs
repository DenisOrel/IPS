// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.TextSources.ReadOnlyTextDocumentSearchTool
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;

#nullable disable
namespace ICSharpCode.NRefactory.TextSources;

internal sealed class ReadOnlyTextDocumentSearchTool
{
  private IReadOnlyTextDocument document;

  public ReadOnlyTextDocumentSearchTool(IReadOnlyTextDocument document)
  {
    this.document = document != null ? document : throw new ArgumentNullException(nameof (document));
  }

  public int IndexOf(char c, int startIndex, int count)
  {
    for (int offset = startIndex; offset < startIndex + count; ++offset)
    {
      if ((int) this.document.GetCharAt(offset) == (int) c)
        return offset;
    }
    return -1;
  }

  public int IndexOfAny(char[] anyOf, int startIndex, int count)
  {
    for (int offset = startIndex; offset < startIndex + count; ++offset)
    {
      if (Array.IndexOf<char>(anyOf, this.document.GetCharAt(offset)) >= 0)
        return offset;
    }
    return -1;
  }

  public int IndexOf(
    string searchText,
    int startIndex,
    int count,
    StringComparison comparisonType)
  {
    throw new NotImplementedException();
  }

  public int LastIndexOf(char c, int startIndex, int count)
  {
    for (int offset = startIndex + count - 1; offset >= 0; --offset)
    {
      if ((int) this.document.GetCharAt(offset) == (int) c)
        return offset;
    }
    return -1;
  }

  public int LastIndexOf(
    string searchText,
    int startIndex,
    int count,
    StringComparison comparisonType)
  {
    throw new NotImplementedException();
  }
}
