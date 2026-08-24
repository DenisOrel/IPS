// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.SignatureSearch
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.Interfaces;
using System;
using System.IO;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal static class SignatureSearch
{
  public static bool ContainsSignature(ProEngineerContentPattern pattern, Stream stream)
  {
    long length = stream != null ? stream.Length : 0L;
    if (pattern == null || string.IsNullOrEmpty(pattern.Value) || stream == null || length < (long) (pattern.Value.Length / 2) || length <= pattern.Ofs || pattern.Ofs < 0L && (length + pattern.Ofs < 0L || pattern.Ofs + (long) (pattern.Value.Length / 2) > 0L))
      return false;
    stream.Seek(pattern.Ofs, pattern.Ofs >= 0L ? SeekOrigin.Begin : SeekOrigin.End);
    byte[] numArray = new byte[pattern.Value.Length / 2];
    stream.Read(numArray, 0, pattern.Value.Length / 2);
    return StringComparer.InvariantCultureIgnoreCase.Compare(StringsHelper.Bytes2HEX(numArray), pattern.Value) == 0;
  }
}
