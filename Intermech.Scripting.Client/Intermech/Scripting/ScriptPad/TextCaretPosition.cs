// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.TextCaretPosition
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal sealed class TextCaretPosition
{
  private readonly int line;
  private readonly int column;

  public TextCaretPosition(int line, int column)
  {
    if (line <= 0)
      throw new ArgumentOutOfRangeException(nameof (line));
    if (column <= 0)
      throw new ArgumentOutOfRangeException(nameof (column));
    this.line = line;
    this.column = column;
  }

  public int Line => this.line;

  public int Column => this.column;

  public override int GetHashCode() => this.line + 29 * this.column;

  public override bool Equals(object obj)
  {
    if (this == obj)
      return true;
    if (!(obj is TextCaretPosition textCaretPosition))
      return base.Equals(obj);
    return this.line == textCaretPosition.line && this.column == textCaretPosition.column;
  }
}
