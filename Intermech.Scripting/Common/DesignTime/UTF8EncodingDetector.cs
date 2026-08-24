// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.UTF8EncodingDetector
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

public sealed class UTF8EncodingDetector
{
  private static readonly byte[] utf8bom = new byte[3]
  {
    (byte) 239,
    (byte) 187,
    (byte) 191
  };
  private Encoding utf8Encoding;
  private Encoding oemEncoding;

  public UTF8EncodingDetector()
  {
    this.utf8Encoding = Encoding.UTF8;
    this.oemEncoding = Encoding.Default;
  }

  public Encoding Detect(byte[] bytes)
  {
    if (bytes == null)
      throw new ArgumentNullException(nameof (bytes));
    return bytes.Length == 0 || bytes.Length >= 3 && this.StartsWithUtf8Bom(bytes) ? this.utf8Encoding : this.oemEncoding;
  }

  public Encoding Detect(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (stream.Length == 0L)
      return this.utf8Encoding;
    if (stream.Length >= 3L)
    {
      byte[] buffer = new byte[3];
      stream.Read(buffer, 0, buffer.Length);
      if (this.StartsWithUtf8Bom(buffer))
        return this.utf8Encoding;
    }
    return this.oemEncoding;
  }

  private bool StartsWithUtf8Bom(byte[] buffer)
  {
    for (int index = 0; index < UTF8EncodingDetector.utf8bom.Length; ++index)
    {
      if ((int) buffer[index] != (int) UTF8EncodingDetector.utf8bom[index])
        return false;
    }
    return true;
  }
}
