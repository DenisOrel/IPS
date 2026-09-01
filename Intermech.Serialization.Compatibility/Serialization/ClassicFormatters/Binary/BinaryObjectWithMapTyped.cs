// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.BinaryObjectWithMapTyped
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class BinaryObjectWithMapTyped : IStreamable
{
  internal BinaryHeaderEnum _binaryHeaderEnum;
  internal int _objectId;
  internal string _name;
  internal int _numMembers;
  internal string[] _memberNames;
  internal BinaryTypeEnum[] _binaryTypeEnumA;
  internal object[] _typeInformationA;
  internal int[] _memberAssemIds;
  internal int _assemId;

  internal BinaryObjectWithMapTyped()
  {
  }

  internal BinaryObjectWithMapTyped(BinaryHeaderEnum binaryHeaderEnum)
  {
    this._binaryHeaderEnum = binaryHeaderEnum;
  }

  internal void Set(
    int objectId,
    string name,
    int numMembers,
    string[] memberNames,
    BinaryTypeEnum[] binaryTypeEnumA,
    object[] typeInformationA,
    int[] memberAssemIds,
    int assemId)
  {
    this._objectId = objectId;
    this._assemId = assemId;
    this._name = name;
    this._numMembers = numMembers;
    this._memberNames = memberNames;
    this._binaryTypeEnumA = binaryTypeEnumA;
    this._typeInformationA = typeInformationA;
    this._memberAssemIds = memberAssemIds;
    this._assemId = assemId;
    this._binaryHeaderEnum = assemId > 0 ? BinaryHeaderEnum.ObjectWithMapTypedAssemId : BinaryHeaderEnum.ObjectWithMapTyped;
  }

  public void Write(BinaryFormatterWriter output)
  {
    output.WriteByte((byte) this._binaryHeaderEnum);
    output.WriteInt32(this._objectId);
    output.WriteString(this._name);
    output.WriteInt32(this._numMembers);
    for (int index = 0; index < this._numMembers; ++index)
      output.WriteString(this._memberNames[index]);
    for (int index = 0; index < this._numMembers; ++index)
      output.WriteByte((byte) this._binaryTypeEnumA[index]);
    for (int index = 0; index < this._numMembers; ++index)
      BinaryTypeConverter.WriteTypeInfo(this._binaryTypeEnumA[index], this._typeInformationA[index], this._memberAssemIds[index], output);
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
    this._binaryTypeEnumA = new BinaryTypeEnum[this._numMembers];
    this._typeInformationA = new object[this._numMembers];
    this._memberAssemIds = new int[this._numMembers];
    for (int index = 0; index < this._numMembers; ++index)
      this._memberNames[index] = input.ReadString();
    for (int index = 0; index < this._numMembers; ++index)
      this._binaryTypeEnumA[index] = (BinaryTypeEnum) input.ReadByte();
    for (int index = 0; index < this._numMembers; ++index)
    {
      if (this._binaryTypeEnumA[index] != BinaryTypeEnum.ObjectUrt && this._binaryTypeEnumA[index] != BinaryTypeEnum.ObjectUser)
        this._typeInformationA[index] = BinaryTypeConverter.ReadTypeInfo(this._binaryTypeEnumA[index], input, out this._memberAssemIds[index]);
      else
        BinaryTypeConverter.ReadTypeInfo(this._binaryTypeEnumA[index], input, out this._memberAssemIds[index]);
    }
    if (this._binaryHeaderEnum != BinaryHeaderEnum.ObjectWithMapTypedAssemId)
      return;
    this._assemId = input.ReadInt32();
  }
}
