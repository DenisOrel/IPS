// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.ObjectProgress
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class ObjectProgress
{
  internal bool _isInitial;
  internal int _count;
  internal BinaryTypeEnum _expectedType = BinaryTypeEnum.ObjectUrt;
  internal object _expectedTypeInformation;
  internal string _name;
  internal InternalObjectTypeE _objectTypeEnum;
  internal InternalMemberTypeE _memberTypeEnum;
  internal InternalMemberValueE _memberValueEnum;
  internal Type _dtType;
  internal int _numItems;
  internal BinaryTypeEnum _binaryTypeEnum;
  internal object _typeInformation;
  internal int _memberLength;
  internal BinaryTypeEnum[] _binaryTypeEnumA;
  internal object[] _typeInformationA;
  internal string[] _memberNames;
  internal Type[] _memberTypes;
  internal ParseRecord _pr = new ParseRecord();

  internal ObjectProgress()
  {
  }

  internal void Init()
  {
    this._isInitial = false;
    this._count = 0;
    this._expectedType = BinaryTypeEnum.ObjectUrt;
    this._expectedTypeInformation = (object) null;
    this._name = (string) null;
    this._objectTypeEnum = InternalObjectTypeE.Empty;
    this._memberTypeEnum = InternalMemberTypeE.Empty;
    this._memberValueEnum = InternalMemberValueE.Empty;
    this._dtType = (Type) null;
    this._numItems = 0;
    this._typeInformation = (object) null;
    this._memberLength = 0;
    this._binaryTypeEnumA = (BinaryTypeEnum[]) null;
    this._typeInformationA = (object[]) null;
    this._memberNames = (string[]) null;
    this._memberTypes = (Type[]) null;
    this._pr.Init();
  }

  internal void ArrayCountIncrement(int value) => this._count += value;

  internal bool GetNext(out BinaryTypeEnum outBinaryTypeEnum, out object outTypeInformation)
  {
    outBinaryTypeEnum = BinaryTypeEnum.Primitive;
    outTypeInformation = (object) null;
    if (this._objectTypeEnum == InternalObjectTypeE.Array)
    {
      if (this._count == this._numItems)
        return false;
      outBinaryTypeEnum = this._binaryTypeEnum;
      outTypeInformation = this._typeInformation;
      if (this._count == 0)
        this._isInitial = false;
      ++this._count;
      return true;
    }
    if (this._count == this._memberLength && !this._isInitial)
      return false;
    outBinaryTypeEnum = this._binaryTypeEnumA[this._count];
    outTypeInformation = this._typeInformationA[this._count];
    if (this._count == 0)
      this._isInitial = false;
    this._name = this._memberNames[this._count];
    this._dtType = this._memberTypes[this._count];
    ++this._count;
    return true;
  }
}
