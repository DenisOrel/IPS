// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechAttrParam
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

[Obsolete("Use TechParamAttribute instead")]
[Serializable]
internal struct TechAttrParam : 
  ITechAttrParam,
  ITechParamAttribute,
  ITechParamBase,
  IComparable<ITechParamBase>,
  IEquatable<ITechParamBase>
{
  [NonSerialized]
  private IAttributeTypeItem _attrType;
  private readonly int _attrId;
  private int _index;
  private readonly object _value;
  private byte _attrBelong;

  public TechAttrParam(IAttributeTypeItem attrType, object value, byte attrBelong)
  {
    this._attrType = attrType != null ? attrType : throw new ArgumentNullException(nameof (attrType));
    this._attrId = this._attrType != null ? this._attrType.ID : 0;
    this._index = 0;
    this._value = value;
    this._attrBelong = attrBelong;
  }

  public TechAttrParam(ITechParamBase obj)
  {
    if (obj is ITechParamAttribute techParamAttribute)
    {
      this._attrType = techParamAttribute.AttributeType;
      this._attrId = this._attrType != null ? this._attrType.ID : 0;
      this._index = techParamAttribute.Index;
      this._value = techParamAttribute.Value;
      this._attrBelong = (byte) techParamAttribute.AttributeBelongs;
    }
    else
    {
      this._attrType = (IAttributeTypeItem) null;
      this._attrId = 0;
      this._index = 0;
      this._value = (object) null;
      this._attrBelong = (byte) 0;
    }
  }

  public IAttributeTypeItem AttrType
  {
    get
    {
      if (this._attrType != null || this._attrId == 0 || TechcardConsts.Plugin == null)
        return this._attrType;
      this._attrType = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByID(this._attrId);
      return this._attrType;
    }
  }

  public int Index
  {
    get => this._index;
    set => this._index = value;
  }

  public object Value => this._value;

  public string Caption
  {
    get => string.Empty;
    set
    {
    }
  }

  public byte AttrBelong
  {
    get => this._attrBelong;
    set => this._attrBelong = value;
  }

  public TechParamType GetTechParamType() => TechParamType.Attribute;

  public int CompareTo(ITechParamBase other)
  {
    if (!(other is ITechParamAttribute techParamAttribute))
      return 1;
    int num = this._attrId.CompareTo(techParamAttribute.AttributeType != null ? techParamAttribute.AttributeType.ID : 0);
    return num == 0 ? this.Index.CompareTo(techParamAttribute.Index) : num;
  }

  public bool Equals(ITechParamBase other)
  {
    return this.CompareTo((ITechParamBase) (other as ITechParamAttribute)) == 0;
  }

  EntitySetting.AttributeBelongs ITechParamAttribute.AttributeBelongs
  {
    get => (EntitySetting.AttributeBelongs) this.AttrBelong;
  }

  IAttributeTypeItem ITechParamAttribute.AttributeType => this.AttrType;

  public override bool Equals(object other)
  {
    return this.CompareTo((ITechParamBase) (other as ITechParamAttribute)) == 0;
  }

  public override int GetHashCode() => this._attrId.GetHashCode();

  public override string ToString()
  {
    return $"Атрибут:{this._attrId}; Индекс:{this.Index}; Значение:{this.Value}";
  }
}
