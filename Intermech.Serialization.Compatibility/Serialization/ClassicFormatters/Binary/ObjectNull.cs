// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.ObjectNull
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class ObjectNull : IStreamable
{
  internal int _nullCount;

  internal ObjectNull()
  {
  }

  internal void SetNullCount(int nullCount) => this._nullCount = nullCount;

  public void Write(BinaryFormatterWriter output)
  {
    if (this._nullCount == 1)
      output.WriteByte((byte) 10);
    else if (this._nullCount < 256 /*0x0100*/)
    {
      output.WriteByte((byte) 13);
      output.WriteByte((byte) this._nullCount);
    }
    else
    {
      output.WriteByte((byte) 14);
      output.WriteInt32(this._nullCount);
    }
  }

  public void Read(BinaryParser input) => this.Read(input, BinaryHeaderEnum.ObjectNull);

  public void Read(BinaryParser input, BinaryHeaderEnum binaryHeaderEnum)
  {
    switch (binaryHeaderEnum)
    {
      case BinaryHeaderEnum.ObjectNull:
        this._nullCount = 1;
        break;
      case BinaryHeaderEnum.ObjectNullMultiple256:
        this._nullCount = (int) input.ReadByte();
        break;
      case BinaryHeaderEnum.ObjectNullMultiple:
        this._nullCount = input.ReadInt32();
        break;
    }
  }
}
