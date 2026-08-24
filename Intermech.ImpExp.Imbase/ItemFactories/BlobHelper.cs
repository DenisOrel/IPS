// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.BlobHelper
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.Interfaces;
using Intermech.IO;
using System;
using System.Data;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal static class BlobHelper
{
  public static void WriteBlob(
    IPackedStream packedStreamService,
    BinaryWriter bWriter,
    BlobType blobType,
    ImChunkedStream stream,
    string filename,
    out bool zipped)
  {
    if (blobType != BlobType.Template)
    {
      if (blobType != BlobType.Text)
      {
        try
        {
          if (blobType == BlobType.MaterialProps)
          {
            ImChunkedStream materialStream = BlobHelper.GetMaterialStream(stream);
            stream.Close();
            stream = materialStream;
          }
          using (ImChunkedStream outStream = new ImChunkedStream())
          {
            packedStreamService.PackStream((Stream) outStream, (Stream) stream, 9);
            bWriter.Write(outStream.ToArray());
          }
          zipped = true;
          return;
        }
        catch (Exception ex)
        {
          throw new Exception($"Ошибка при запаковке блоба {filename} : {ex.Message}");
        }
      }
    }
    char[] chars = Encoding.GetEncoding(1251).GetChars(stream.ToArray());
    bWriter.Write(chars, 0, chars.Length);
    zipped = false;
  }

  private static ImChunkedStream GetMaterialStream(ImChunkedStream stream)
  {
    using (ImChunkedStream imChunkedStream = new ImChunkedStream())
    {
      new BinaryWriter((Stream) imChunkedStream, Encoding.UTF8).Write(Encoding.GetEncoding(1251).GetString(stream.ToArray()));
      return BlobHelper.CreateStreamFromStream(imChunkedStream);
    }
  }

  private static ImChunkedStream CreateStreamFromStream(ImChunkedStream stream)
  {
    ImChunkedStream streamFromStream = new ImChunkedStream();
    streamFromStream.Write(stream.ToArray(), 0, Convert.ToInt32(stream.Length));
    streamFromStream.Position = 0L;
    return streamFromStream;
  }

  public static ImChunkedStream ReadBlob(
    IPackedStream packedStreamService,
    IDataReader reader,
    int index,
    string filename)
  {
    bool packed;
    ImChunkedStream packedStream = BlobHelper.ReadBlobFromBase(reader, index, out packed);
    if (!packed)
      return packedStream;
    try
    {
      return BlobHelper.UnpackStream(packedStreamService, packedStream, filename);
    }
    finally
    {
      packedStream.Close();
    }
  }

  private static ImChunkedStream UnpackStream(
    IPackedStream packedStreamService,
    ImChunkedStream packedStream,
    string filename)
  {
    ImChunkedStream outStream = new ImChunkedStream();
    try
    {
      packedStreamService.UnpackStream((Stream) outStream, (Stream) packedStream);
    }
    catch (Exception ex)
    {
      outStream.Close();
      throw new Exception($"Ошибка при распаковке блоба {filename} : {ex.Message}");
    }
    return outStream;
  }

  private static ImChunkedStream ReadBlobFromBase(IDataReader reader, int index, out bool packed)
  {
    packed = false;
    byte[] buffer = new byte[Intermech.Consts.BlobTransferBufferLength];
    int fieldOffset = 0;
    ImChunkedStream imChunkedStream = new ImChunkedStream();
    while (true)
    {
      int bytes = (int) reader.GetBytes(index, (long) fieldOffset, buffer, 0, Intermech.Consts.BlobTransferBufferLength);
      if (bytes > 0)
      {
        int offset = 0;
        if (fieldOffset == 0 && buffer[0] == (byte) 90 && buffer[1] == (byte) 76 && buffer[2] == (byte) 73 && buffer[3] == (byte) 66)
        {
          offset = 4;
          packed = true;
        }
        fieldOffset += bytes;
        imChunkedStream.Write(buffer, offset, bytes - offset);
      }
      else
        break;
    }
    return imChunkedStream;
  }
}
