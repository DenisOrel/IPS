// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Interfaces.Signs.LengthedCryptoStream
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Intermech.Search.Interfaces.Signs;

/// <summary>
/// Стандартный CryptoStream не работает с классами распаковки, потому что распаковка дергает Length независимо от CryptoStream.CanSeek
/// </summary>
internal class LengthedCryptoStream(
  Stream stream,
  ICryptoTransform transform,
  CryptoStreamMode mode) : CryptoStream(stream, transform, mode)
{
  public override long Length => 0;
}
