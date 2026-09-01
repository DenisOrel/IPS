// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.BinaryObject
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class BinaryObject : IStreamable
{
  internal int _objectId;
  internal int _mapId;

  internal BinaryObject()
  {
  }

  internal void Set(int objectId, int mapId)
  {
    this._objectId = objectId;
    this._mapId = mapId;
  }

  public void Write(BinaryFormatterWriter output)
  {
    output.WriteByte((byte) 1);
    output.WriteInt32(this._objectId);
    output.WriteInt32(this._mapId);
  }

  public void Read(BinaryParser input)
  {
    this._objectId = input.ReadInt32();
    this._mapId = input.ReadInt32();
  }
}
