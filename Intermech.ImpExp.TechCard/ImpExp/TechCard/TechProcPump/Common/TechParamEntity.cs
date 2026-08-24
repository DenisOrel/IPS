// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamEntity
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

[Serializable]
public class TechParamEntity : 
  TechParamBase,
  ITechParamEntity,
  ITechParamBase,
  IComparable<ITechParamBase>,
  IEquatable<ITechParamBase>
{
  private readonly string _code;

  public TechParamEntity(string code, object value)
  {
    this._code = !string.IsNullOrEmpty(code) ? code : throw new Exception("\"Код понятия\" не может быть пустым!");
    this.Value = value;
    this.IsFixed = false;
  }

  public TechParamEntity(ITechParamBase obj)
  {
    if (obj is ITechParamEntity techParamEntity)
    {
      this._code = techParamEntity.Code;
      this.Value = techParamEntity.Value;
      this.IsFixed = techParamEntity.IsFixed;
    }
    else
    {
      this._code = string.Empty;
      this.Value = (object) null;
      this.IsFixed = false;
    }
  }

  public override TechParamType GetTechParamType() => TechParamType.Entity;

  public string Code => this._code;

  public virtual bool IsFixed
  {
    get => false;
    set
    {
    }
  }

  public override int CompareTo(ITechParamBase other)
  {
    return !(other is ITechParamEntity techParamEntity) ? 1 : string.Compare(this.Code, techParamEntity.Code, StringComparison.Ordinal);
  }

  public override bool Equals(ITechParamBase other) => this.CompareTo(other) == 0;

  public override bool Equals(object other) => this.CompareTo(other as ITechParamBase) == 0;

  public override int GetHashCode() => this.Code.GetHashCode();

  public override string ToString() => $"Понятие:{this.Code}; Значение:{this.Value}";
}
