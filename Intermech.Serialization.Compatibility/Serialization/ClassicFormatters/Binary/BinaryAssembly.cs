// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.BinaryAssembly
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class BinaryAssembly : IStreamable
{
  internal int _assemId;
  internal string _assemblyString;

  internal BinaryAssembly()
  {
  }

  internal void Set(int assemId, string assemblyString)
  {
    this._assemId = assemId;
    this._assemblyString = assemblyString;
  }

  public void Write(BinaryFormatterWriter output)
  {
    output.WriteByte((byte) 12);
    output.WriteInt32(this._assemId);
    output.WriteString(this._assemblyString);
  }

  public void Read(BinaryParser input)
  {
    this._assemId = input.ReadInt32();
    this._assemblyString = input.ReadString();
  }
}
