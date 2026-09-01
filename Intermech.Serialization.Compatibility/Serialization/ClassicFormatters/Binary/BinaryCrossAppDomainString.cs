// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.BinaryCrossAppDomainString
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class BinaryCrossAppDomainString : IStreamable
{
  internal int _objectId;
  internal int _value;

  internal BinaryCrossAppDomainString()
  {
  }

  public void Write(BinaryFormatterWriter output)
  {
    output.WriteByte((byte) 19);
    output.WriteInt32(this._objectId);
    output.WriteInt32(this._value);
  }

  public void Read(BinaryParser input)
  {
    this._objectId = input.ReadInt32();
    this._value = input.ReadInt32();
  }
}
