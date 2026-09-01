// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.ValueFixup
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Reflection;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class ValueFixup
{
  internal ValueFixupEnum _valueFixupEnum;
  internal Array _arrayObj;
  internal int[] _indexMap;
  internal object _memberObject;
  internal ReadObjectInfo _objectInfo;
  internal string _memberName;

  internal ValueFixup(Array arrayObj, int[] indexMap)
  {
    this._valueFixupEnum = ValueFixupEnum.Array;
    this._arrayObj = arrayObj;
    this._indexMap = indexMap;
  }

  internal ValueFixup(object memberObject, string memberName, ReadObjectInfo objectInfo)
  {
    this._valueFixupEnum = ValueFixupEnum.Member;
    this._memberObject = memberObject;
    this._memberName = memberName;
    this._objectInfo = objectInfo;
  }

  internal void Fixup(ParseRecord record, ParseRecord parent)
  {
    object newObj = record._newObj;
    switch (this._valueFixupEnum)
    {
      case ValueFixupEnum.Array:
        this._arrayObj.SetValue(newObj, this._indexMap);
        break;
      case ValueFixupEnum.Header:
        throw new PlatformNotSupportedException();
      case ValueFixupEnum.Member:
        if (this._objectInfo._isSi)
        {
          this._objectInfo._objectManager.RecordDelayedFixup(parent._objectId, this._memberName, record._objectId);
          break;
        }
        MemberInfo memberInfo = this._objectInfo.GetMemberInfo(this._memberName);
        if (!(memberInfo != (MemberInfo) null))
          break;
        this._objectInfo._objectManager.RecordFixup(parent._objectId, memberInfo, record._objectId);
        break;
    }
  }
}
