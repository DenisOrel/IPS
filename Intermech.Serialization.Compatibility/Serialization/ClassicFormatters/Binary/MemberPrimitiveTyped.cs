// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.MemberPrimitiveTyped
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class MemberPrimitiveTyped : IStreamable
{
  internal InternalPrimitiveTypeE _primitiveTypeEnum;
  internal object _value;

  internal MemberPrimitiveTyped()
  {
  }

  internal void Set(InternalPrimitiveTypeE primitiveTypeEnum, object value)
  {
    this._primitiveTypeEnum = primitiveTypeEnum;
    this._value = value;
  }

  public void Write(BinaryFormatterWriter output)
  {
    output.WriteByte((byte) 8);
    output.WriteByte((byte) this._primitiveTypeEnum);
    output.WriteValue(this._primitiveTypeEnum, this._value);
  }

  public void Read(BinaryParser input)
  {
    this._primitiveTypeEnum = (InternalPrimitiveTypeE) input.ReadByte();
    this._value = input.ReadValue(this._primitiveTypeEnum);
  }
}
