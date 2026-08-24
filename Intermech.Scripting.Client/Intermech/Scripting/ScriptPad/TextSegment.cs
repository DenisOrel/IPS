// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.TextSegment
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal sealed class TextSegment
{
  private readonly int offset;
  private readonly int length;

  public TextSegment(int offset, int length)
  {
    if (offset < 0)
      throw new ArgumentOutOfRangeException(nameof (offset));
    if (length <= 0)
      throw new ArgumentOutOfRangeException(nameof (length));
    this.offset = offset;
    this.length = length;
  }

  public int Offset
  {
    [DebuggerStepThrough] get => this.offset;
  }

  public int Length
  {
    [DebuggerStepThrough] get => this.length;
  }
}
