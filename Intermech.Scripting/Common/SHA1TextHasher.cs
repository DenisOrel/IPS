// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.SHA1TextHasher
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Diagnostics;
using System.Security.Cryptography;

#nullable disable
namespace Intermech.Scripting.Common;

internal sealed class SHA1TextHasher : ITextHasher
{
  private static readonly byte[] emptyTextHash = new byte[20]
  {
    (byte) 218,
    (byte) 57,
    (byte) 163,
    (byte) 238,
    (byte) 94,
    (byte) 107,
    (byte) 75,
    (byte) 13,
    (byte) 50,
    (byte) 85,
    (byte) 191,
    (byte) 239,
    (byte) 149,
    (byte) 96 /*0x60*/,
    (byte) 24,
    (byte) 144 /*0x90*/,
    (byte) 175,
    (byte) 216,
    (byte) 7,
    (byte) 9
  };
  private int charBufferLength;
  private byte[] buffer;
  private SHA1 sha1;

  public SHA1TextHasher(int charBufferLength)
  {
    this.charBufferLength = charBufferLength >= 16 /*0x10*/ ? charBufferLength : throw new ArgumentOutOfRangeException(nameof (charBufferLength));
    this.buffer = new byte[charBufferLength * 2];
    this.sha1 = (SHA1) new SHA1CryptoServiceProvider();
  }

  public int CharBufferLength
  {
    [DebuggerStepThrough] get => this.charBufferLength;
  }

  public byte[] ComputeHash(string text)
  {
    switch (text)
    {
      case null:
        throw new ArgumentNullException(nameof (text));
      case "":
        return SHA1TextHasher.emptyTextHash;
      default:
        this.sha1.Initialize();
        int length;
        for (int offset = 0; offset < text.Length; offset += length)
        {
          length = Math.Min(this.charBufferLength, text.Length - offset);
          this.TextToBuffer(text, offset, length);
          this.sha1.TransformBlock(this.buffer, 0, length << 1, this.buffer, 0);
        }
        this.sha1.TransformFinalBlock(this.buffer, 0, 0);
        return this.sha1.Hash;
    }
  }

  private void TextToBuffer(string text, int offset, int length)
  {
    for (int index = 0; index < length; ++index)
    {
      int num = (int) text[offset + index];
      this.buffer[index << 1] = (byte) (num & (int) byte.MaxValue);
      this.buffer[(index << 1) + 1] = (byte) (num >> 8 & (int) byte.MaxValue);
    }
  }
}
