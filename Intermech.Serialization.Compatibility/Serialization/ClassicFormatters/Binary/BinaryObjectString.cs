// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.BinaryObjectString
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class BinaryObjectString : IStreamable
{
  internal int _objectId;
  internal string _value;

  internal BinaryObjectString()
  {
  }

  internal void Set(int objectId, string value)
  {
    this._objectId = objectId;
    this._value = value;
  }

  public void Write(BinaryFormatterWriter output)
  {
    output.WriteByte((byte) 6);
    output.WriteInt32(this._objectId);
    output.WriteString(this._value);
  }

  public void Read(BinaryParser input)
  {
    this._objectId = input.ReadInt32();
    this._value = input.ReadString();
  }
}
