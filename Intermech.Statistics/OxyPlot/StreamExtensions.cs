// Decompiled with JetBrains decompiler
// Type: OxyPlot.StreamExtensions
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.IO;

#nullable disable
namespace OxyPlot;

public static class StreamExtensions
{
  public static void CopyTo(this Stream input, Stream output)
  {
    byte[] buffer = new byte[32768 /*0x8000*/];
    int count;
    while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
      output.Write(buffer, 0, count);
  }
}
