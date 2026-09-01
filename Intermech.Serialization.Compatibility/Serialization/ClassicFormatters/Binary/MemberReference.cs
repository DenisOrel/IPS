// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.MemberReference
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class MemberReference : IStreamable
{
  internal int _idRef;

  internal MemberReference()
  {
  }

  internal void Set(int idRef) => this._idRef = idRef;

  public void Write(BinaryFormatterWriter output)
  {
    output.WriteByte((byte) 9);
    output.WriteInt32(this._idRef);
  }

  public void Read(BinaryParser input) => this._idRef = input.ReadInt32();
}
