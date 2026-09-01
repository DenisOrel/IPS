// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.BinaryCrossAppDomainMap
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class BinaryCrossAppDomainMap : IStreamable
{
  internal int _crossAppDomainArrayIndex;

  public void Write(BinaryFormatterWriter output)
  {
    output.WriteByte((byte) 18);
    output.WriteInt32(this._crossAppDomainArrayIndex);
  }

  public void Read(BinaryParser input) => this._crossAppDomainArrayIndex = input.ReadInt32();
}
