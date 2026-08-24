// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.FilterItemSurrogate
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators.Electrical;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.MG.Integrator;

[DefaultProperty("ParameterValue")]
internal sealed class FilterItemSurrogate : ICloneable
{
  private string _parameterValue;
  private CompositionVariants _variant;

  [DisplayName("Значение параметра")]
  [Description("Значение параметра компонента схемы или платы")]
  public string ParameterValue
  {
    get => this._parameterValue;
    set => this._parameterValue = value;
  }

  [DisplayName("Вариант состава")]
  [Description("Вариант состава, в который попадет компонент схемы или платы при текущем значении параметра")]
  public CompositionVariants Variant
  {
    get => this._variant;
    set => this._variant = value;
  }

  public FilterItemSurrogate Clone()
  {
    return new FilterItemSurrogate()
    {
      _parameterValue = this._parameterValue,
      _variant = this._variant
    };
  }

  object ICloneable.Clone() => (object) this.Clone();

  public override string ToString() => "Вариант";

  public override int GetHashCode()
  {
    int num = 0;
    if (this._parameterValue != null)
      num ^= this._parameterValue.GetHashCode();
    return num ^ this._variant.GetHashCode();
  }

  public override bool Equals(object obj)
  {
    if (!(obj is FilterItemSurrogate filterItemSurrogate))
      return base.Equals(obj);
    return !(filterItemSurrogate._parameterValue != this._parameterValue) && filterItemSurrogate._variant == this._variant;
  }
}
