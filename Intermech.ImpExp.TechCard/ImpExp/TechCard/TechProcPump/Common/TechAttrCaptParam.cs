// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechAttrCaptParam
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

[Obsolete("Use TechParamAttributeCaption instead")]
[Serializable]
internal class TechAttrCaptParam : 
  ITechAttrParam,
  ITechParamAttribute,
  ITechParamBase,
  IComparable<ITechParamBase>,
  IEquatable<ITechParamBase>
{
  [NonSerialized]
  private IAttributeTypeItem _attrType;
  private readonly int _attrId;
  private readonly object _value;
  private int _index;
  private string _caption = string.Empty;
  private byte _attrBelong;

  public TechAttrCaptParam(
    IAttributeTypeItem attrType,
    object value,
    string caption,
    byte attrBelong)
  {
    this._attrType = attrType != null ? attrType : throw new ArgumentNullException(nameof (attrType));
    this._attrId = this._attrType != null ? attrType.ID : 0;
    this._value = value;
    this._caption = caption;
    this._attrBelong = attrBelong;
  }

  public TechAttrCaptParam(ITechParamBase obj)
  {
    if (!(obj is ITechParamAttribute techParamAttribute))
      return;
    this._attrType = techParamAttribute.AttributeType;
    this._attrId = this._attrType != null ? this._attrType.ID : 0;
    this._index = techParamAttribute.Index;
    this._value = techParamAttribute.Value;
    this._caption = techParamAttribute.Caption;
    this._attrBelong = (byte) techParamAttribute.AttributeBelongs;
  }

  public IAttributeTypeItem AttrType
  {
    get
    {
      if (this._attrType != null || this._attrId == 0 || TechcardConsts.Plugin == null)
        return this._attrType;
      this._attrType = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByID(this._attrId);
      IAttributeTypeItem attrType = this._attrType;
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
    get => this._caption;
    set => this._caption = value;
  }

  public byte AttrBelong
  {
    get => this._attrBelong;
    set => this._attrBelong = value;
  }

  EntitySetting.AttributeBelongs ITechParamAttribute.AttributeBelongs
  {
    get => (EntitySetting.AttributeBelongs) this.AttrBelong;
  }

  IAttributeTypeItem ITechParamAttribute.AttributeType => this.AttrType;

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

  public TechParamType GetTechParamType() => TechParamType.Attribute;

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
