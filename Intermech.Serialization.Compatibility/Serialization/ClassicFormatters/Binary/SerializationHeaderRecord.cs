// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.SerializationHeaderRecord
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.IO;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class SerializationHeaderRecord : IStreamable
{
  internal const int BinaryFormatterMajorVersion = 1;
  internal const int BinaryFormatterMinorVersion = 0;
  internal BinaryHeaderEnum _binaryHeaderEnum;
  internal int _topId;
  internal int _headerId;
  internal int _majorVersion;
  internal int _minorVersion;

  internal SerializationHeaderRecord()
  {
  }

  internal SerializationHeaderRecord(
    BinaryHeaderEnum binaryHeaderEnum,
    int topId,
    int headerId,
    int majorVersion,
    int minorVersion)
  {
    this._binaryHeaderEnum = binaryHeaderEnum;
    this._topId = topId;
    this._headerId = headerId;
    this._majorVersion = majorVersion;
    this._minorVersion = minorVersion;
  }

  public void Write(BinaryFormatterWriter output)
  {
    this._majorVersion = 1;
    this._minorVersion = 0;
    output.WriteByte((byte) this._binaryHeaderEnum);
    output.WriteInt32(this._topId);
    output.WriteInt32(this._headerId);
    output.WriteInt32(1);
    output.WriteInt32(0);
  }

  private static int GetInt32(byte[] buffer, int index)
  {
    return (int) buffer[index] | (int) buffer[index + 1] << 8 | (int) buffer[index + 2] << 16 /*0x10*/ | (int) buffer[index + 3] << 24;
  }

  public void Read(BinaryParser input)
  {
    byte[] buffer = input.ReadBytes(17);
    this._majorVersion = buffer.Length >= 17 ? SerializationHeaderRecord.GetInt32(buffer, 9) : throw new EndOfStreamException(SR2.IO_EOF_ReadBeyondEOF);
    if (this._majorVersion > 1)
      throw new SerializationException(SR.Format(SR2.Serialization_InvalidFormat, (object) BitConverter.ToString(buffer)));
    this._binaryHeaderEnum = (BinaryHeaderEnum) buffer[0];
    this._topId = SerializationHeaderRecord.GetInt32(buffer, 1);
    this._headerId = SerializationHeaderRecord.GetInt32(buffer, 5);
    this._minorVersion = SerializationHeaderRecord.GetInt32(buffer, 13);
  }
}
