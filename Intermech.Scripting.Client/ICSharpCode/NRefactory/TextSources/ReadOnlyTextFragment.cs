// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.TextSources.ReadOnlyTextFragment
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;
using System.Diagnostics;

#nullable disable
namespace ICSharpCode.NRefactory.TextSources;

internal sealed class ReadOnlyTextFragment : IReadOnlyTextDocument, ITextDocument
{
  private IReadOnlyTextDocument document;
  private int fragmentOffset;
  private int fragmentLength;

  public ReadOnlyTextFragment(IReadOnlyTextDocument document, int offset, int length)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (offset < 0 || offset >= document.Length)
      throw new ArgumentOutOfRangeException(nameof (offset));
    if (length <= 0 || offset + length > document.Length)
      throw new ArgumentOutOfRangeException(nameof (length));
    this.document = document;
    this.fragmentOffset = offset;
    this.fragmentLength = length;
  }

  public int Length
  {
    [DebuggerStepThrough] get => this.fragmentLength;
  }

  public char GetCharAt(int offset)
  {
    if (offset < 0 || offset >= this.fragmentLength)
      throw new ArgumentOutOfRangeException(nameof (offset));
    return this.document.GetCharAt(this.fragmentOffset + offset);
  }

  public string GetText(int offset, int length)
  {
    if (offset < 0 || offset >= this.fragmentLength)
      throw new ArgumentOutOfRangeException(nameof (offset));
    if (length <= 0 || offset + length > this.fragmentLength)
      throw new ArgumentOutOfRangeException(nameof (length));
    return this.document.GetText(this.fragmentOffset + offset, length);
  }
}
