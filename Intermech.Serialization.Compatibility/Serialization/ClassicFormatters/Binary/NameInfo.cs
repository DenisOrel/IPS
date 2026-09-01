// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.NameInfo
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class NameInfo
{
  internal string _fullName;
  internal long _objectId;
  internal long _assemId;
  internal InternalPrimitiveTypeE _primitiveTypeEnum;
  internal Type _type;
  internal bool _isSealed;
  internal bool _isArray;
  internal bool _isArrayItem;
  internal bool _transmitTypeOnObject;
  internal bool _transmitTypeOnMember;
  internal bool _isParentTypeOnObject;
  internal InternalArrayTypeE _arrayEnum;
  private bool _sealedStatusChecked;

  internal NameInfo()
  {
  }

  internal void Init()
  {
    this._fullName = (string) null;
    this._objectId = 0L;
    this._assemId = 0L;
    this._primitiveTypeEnum = InternalPrimitiveTypeE.Invalid;
    this._type = (Type) null;
    this._isSealed = false;
    this._transmitTypeOnObject = false;
    this._transmitTypeOnMember = false;
    this._isParentTypeOnObject = false;
    this._isArray = false;
    this._isArrayItem = false;
    this._arrayEnum = InternalArrayTypeE.Empty;
    this._sealedStatusChecked = false;
  }

  public bool IsSealed
  {
    get
    {
      if (!this._sealedStatusChecked)
      {
        this._isSealed = this._type.IsSealed;
        this._sealedStatusChecked = true;
      }
      return this._isSealed;
    }
  }

  public string NIname
  {
    get => this._fullName ?? (this._fullName = this._type?.FullName);
    set => this._fullName = value;
  }
}
