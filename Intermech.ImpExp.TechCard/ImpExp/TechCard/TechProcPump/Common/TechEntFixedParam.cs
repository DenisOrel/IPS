// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechEntFixedParam
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

[Obsolete("Use TechParamEntityFixed instead")]
[Serializable]
internal struct TechEntFixedParam : 
  ITechEntParam,
  ITechParamEntity,
  ITechParamBase,
  IComparable<ITechParamBase>,
  IEquatable<ITechParamBase>
{
  private static readonly bool NeedValidateEntCode = true;
  private string _entCode;
  private object _value;
  private bool _isFixed;
  private string _caption;

  public TechEntFixedParam(string entCode, object value, bool isFixed)
    : this(entCode, value, isFixed, string.Empty)
  {
  }

  public TechEntFixedParam(string entCode, object value, bool isFixed, string caption)
  {
    if (TechEntFixedParam.NeedValidateEntCode && entCode.Equals(string.Empty))
      throw new Exception("\"Код понятия\" не может быть пустым!");
    this._entCode = entCode;
    this._value = value;
    this._isFixed = isFixed;
    this._caption = caption;
  }

  public TechEntFixedParam(ITechParamBase obj)
  {
    if (obj is ITechParamEntity techParamEntity)
    {
      this._entCode = techParamEntity.Code;
      this._value = techParamEntity.Value;
      this._isFixed = techParamEntity.IsFixed;
      this._caption = techParamEntity is TechEntFixedParam techEntFixedParam ? techEntFixedParam.Caption : string.Empty;
    }
    else
    {
      this._entCode = string.Empty;
      this._value = (object) null;
      this._isFixed = false;
      this._caption = string.Empty;
    }
  }

  public TechParamType GetTechParamType() => TechParamType.Entity;

  string ITechParamEntity.Code => this._entCode;

  public string EntCode => this._entCode;

  public bool IsFixed => this._isFixed;

  public object Value => this._value;

  public string Caption
  {
    get => this._caption;
    set => this._caption = value;
  }

  public int CompareTo(ITechParamBase other)
  {
    return !(other is ITechParamEntity techParamEntity) ? 1 : string.Compare(this.EntCode, techParamEntity.Code, StringComparison.Ordinal);
  }

  public bool Equals(ITechParamBase other) => this.CompareTo(other) == 0;

  public override bool Equals(object other) => this.CompareTo(other as ITechParamBase) == 0;

  public override int GetHashCode() => this.EntCode.GetHashCode();

  public override string ToString() => $"Понятие:{this.EntCode};Значение:{this.Value}";
}
