// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.TextSources.ReadOnlyTextDocumentReader
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;
using System.IO;

#nullable disable
namespace ICSharpCode.NRefactory.TextSources;

internal sealed class ReadOnlyTextDocumentReader : TextReader
{
  private IReadOnlyTextDocument document;
  private int pos;
  private int len;
  private bool isDisposed;

  public ReadOnlyTextDocumentReader(IReadOnlyTextDocument document)
  {
    this.document = document != null ? document : throw new ArgumentNullException(nameof (document));
    this.pos = 0;
    this.len = document.Length;
  }

  protected override void Dispose(bool disposing)
  {
    this.pos = 0;
    this.len = 0;
    this.isDisposed = true;
    base.Dispose(disposing);
  }

  private void CheckNotDisposed()
  {
    if (this.isDisposed)
      throw new ObjectDisposedException(this.GetType().FullName);
  }

  public override int Peek()
  {
    this.CheckNotDisposed();
    return this.pos >= this.len ? -1 : (int) this.document.GetCharAt(this.pos);
  }

  public override int Read()
  {
    this.CheckNotDisposed();
    return this.pos >= this.len ? -1 : (int) this.document.GetCharAt(this.pos++);
  }
}
