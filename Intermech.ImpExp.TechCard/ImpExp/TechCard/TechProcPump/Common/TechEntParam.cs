// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechEntParam
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

[Obsolete("Use TechParamEntity instead")]
[Serializable]
internal struct TechEntParam : 
  ITechEntParam,
  ITechParamEntity,
  ITechParamBase,
  IComparable<ITechParamBase>,
  IEquatable<ITechParamBase>
{
  private readonly string _entCode;
  private readonly object _value;

  public TechEntParam(string entCode, object value)
  {
    this._entCode = !entCode.Equals(string.Empty) ? entCode : throw new ArgumentException("\"Код понятия\" не может быть пустым!");
    this._value = value;
  }

  public TechEntParam(ITechParamBase obj)
  {
    if (obj is ITechParamEntity techParamEntity)
    {
      this._entCode = techParamEntity.Code;
      this._value = techParamEntity.Value;
    }
    else
    {
      this._entCode = string.Empty;
      this._value = (object) null;
    }
  }

  public TechParamType GetTechParamType() => TechParamType.Entity;

  public string EntCode => this._entCode;

  string ITechParamEntity.Code => this._entCode;

  public bool IsFixed => false;

  public object Value => this._value;

  public int CompareTo(ITechParamBase other)
  {
    return !(other is ITechParamEntity techParamEntity) ? 1 : string.Compare(this.EntCode, techParamEntity.Code, StringComparison.Ordinal);
  }

  public bool Equals(ITechParamBase other) => this.CompareTo(other) == 0;

  public override bool Equals(object other) => this.CompareTo(other as ITechParamBase) == 0;

  public override int GetHashCode() => this.EntCode.GetHashCode();

  public override string ToString() => $"Понятие:{this.EntCode}; Значение:{this.Value}";
}
