// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.MemberPrimitiveUnTyped
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class MemberPrimitiveUnTyped : IStreamable
{
  internal InternalPrimitiveTypeE _typeInformation;
  internal object _value;

  internal MemberPrimitiveUnTyped()
  {
  }

  internal void Set(InternalPrimitiveTypeE typeInformation, object value)
  {
    this._typeInformation = typeInformation;
    this._value = value;
  }

  internal void Set(InternalPrimitiveTypeE typeInformation)
  {
    this._typeInformation = typeInformation;
  }

  public void Write(BinaryFormatterWriter output)
  {
    output.WriteValue(this._typeInformation, this._value);
  }

  public void Read(BinaryParser input) => this._value = input.ReadValue(this._typeInformation);
}
