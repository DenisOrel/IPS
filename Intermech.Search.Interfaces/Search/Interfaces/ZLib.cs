// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Interfaces.ZLib
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.IO;

#nullable disable
namespace Intermech.Search.Interfaces;

/// <summary>Класс для ускорения работы с ZLib</summary>
public class ZLib
{
  /// <summary>Распаковать массив</summary>
  /// <param name="input">входной массив (пакованный)</param>
  /// <returns>выходной массив (непакованный)</returns>
  public static byte[] Unpack(byte[] input)
  {
    MemoryStream memoryStream = new MemoryStream();
    InflaterInputStream inflaterInputStream = new InflaterInputStream((Stream) new MemoryStream(input));
    byte[] buffer = new byte[4096 /*0x1000*/];
    while (true)
    {
      int count = inflaterInputStream.Read(buffer, 0, 4096 /*0x1000*/);
      if (!count.Equals(0))
        memoryStream.Write(buffer, 0, count);
      else
        break;
    }
    return memoryStream.ToArray();
  }

  /// <summary>Запаковать массив</summary>
  /// <param name="input">входной массив (непакованный)</param>
  /// <returns>выходной массив (пакованный)</returns>
  public static byte[] Pack(byte[] input)
  {
    MemoryStream baseOutputStream = new MemoryStream();
    DeflaterOutputStream deflaterOutputStream = new DeflaterOutputStream((Stream) baseOutputStream, new Deflater(9));
    deflaterOutputStream.Write(input, 0, input.Length);
    deflaterOutputStream.Flush();
    deflaterOutputStream.Finish();
    return baseOutputStream.ToArray();
  }
}
