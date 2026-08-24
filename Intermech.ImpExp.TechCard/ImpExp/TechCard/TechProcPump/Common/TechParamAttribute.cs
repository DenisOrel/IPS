// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamAttribute
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

[Serializable]
public class TechParamAttribute : 
  TechParamBase,
  ITechParamAttribute,
  ITechParamBase,
  IComparable<ITechParamBase>,
  IEquatable<ITechParamBase>
{
  [NonSerialized]
  private IAttributeTypeItem _attributeType;
  private readonly int _attributeId;

  public TechParamAttribute(
    IAttributeTypeItem attributeType,
    object value,
    EntitySetting.AttributeBelongs attrBelong)
  {
    this._attributeType = attributeType ?? throw new ArgumentException("\"Тип атрибута\" не определен!");
    this._attributeId = this._attributeType != null ? this._attributeType.ID : 0;
    this.Value = value;
    this.AttributeBelongs = attrBelong;
  }

  public TechParamAttribute(ITechParamBase obj)
  {
    if (obj is ITechParamAttribute techParamAttribute)
    {
      this._attributeType = techParamAttribute.AttributeType;
      this._attributeId = this._attributeType != null ? this._attributeType.ID : 0;
      this.Index = techParamAttribute.Index;
      this.Value = techParamAttribute.Value;
      this.AttributeBelongs = techParamAttribute.AttributeBelongs;
      this.Caption = techParamAttribute.Caption;
    }
    else
    {
      this._attributeType = (IAttributeTypeItem) null;
      this._attributeId = 0;
      this.Index = 0;
      this.Value = (object) null;
      this.AttributeBelongs = EntitySetting.AttributeBelongs.ToObject;
      this.Caption = string.Empty;
    }
  }

  public IAttributeTypeItem AttributeType
  {
    get
    {
      if (this._attributeType != null || this._attributeId == 0 || TechcardConsts.Plugin == null)
        return this._attributeType;
      this._attributeType = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByID(this._attributeId);
      IAttributeTypeItem attributeType = this._attributeType;
      return this._attributeType;
    }
  }

  public int Index { get; set; }

  public virtual string Caption
  {
    get => string.Empty;
    set
    {
    }
  }

  public EntitySetting.AttributeBelongs AttributeBelongs { get; set; }

  public override TechParamType GetTechParamType() => TechParamType.Attribute;

  public override int CompareTo(ITechParamBase other)
  {
    if (!(other is ITechParamAttribute techParamAttribute))
      return 1;
    int num = this._attributeId.CompareTo(techParamAttribute.AttributeType != null ? techParamAttribute.AttributeType.ID : 0);
    return num == 0 ? this.Index.CompareTo(techParamAttribute.Index) : num;
  }

  public override bool Equals(ITechParamBase other)
  {
    return this.CompareTo((ITechParamBase) (other as ITechParamAttribute)) == 0;
  }

  public override bool Equals(object other)
  {
    return this.CompareTo((ITechParamBase) (other as ITechParamAttribute)) == 0;
  }

  public override int GetHashCode() => this._attributeId.GetHashCode();

  public override string ToString()
  {
    return $"Атрибут:{this._attributeId}; Индекс:{this.Index}; Значение:{this.Value}";
  }
}
