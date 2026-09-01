// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.BinaryObjectWithMap
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class BinaryObjectWithMap : IStreamable
{
  internal BinaryHeaderEnum _binaryHeaderEnum;
  internal int _objectId;
  internal string _name;
  internal int _numMembers;
  internal string[] _memberNames;
  internal int _assemId;

  internal BinaryObjectWithMap()
  {
  }

  internal BinaryObjectWithMap(BinaryHeaderEnum binaryHeaderEnum)
  {
    this._binaryHeaderEnum = binaryHeaderEnum;
  }

  internal void Set(int objectId, string name, int numMembers, string[] memberNames, int assemId)
  {
    this._objectId = objectId;
    this._name = name;
    this._numMembers = numMembers;
    this._memberNames = memberNames;
    this._assemId = assemId;
    this._binaryHeaderEnum = assemId > 0 ? BinaryHeaderEnum.ObjectWithMapAssemId : BinaryHeaderEnum.ObjectWithMap;
  }

  public void Write(BinaryFormatterWriter output)
  {
    output.WriteByte((byte) this._binaryHeaderEnum);
    output.WriteInt32(this._objectId);
    output.WriteString(this._name);
    output.WriteInt32(this._numMembers);
    for (int index = 0; index < this._numMembers; ++index)
      output.WriteString(this._memberNames[index]);
    if (this._assemId <= 0)
      return;
    output.WriteInt32(this._assemId);
  }

  public void Read(BinaryParser input)
  {
    this._objectId = input.ReadInt32();
    this._name = input.ReadString();
    this._numMembers = input.ReadInt32();
    this._memberNames = new string[this._numMembers];
    for (int index = 0; index < this._numMembers; ++index)
      this._memberNames[index] = input.ReadString();
    if (this._binaryHeaderEnum != BinaryHeaderEnum.ObjectWithMapAssemId)
      return;
    this._assemId = input.ReadInt32();
  }
}
