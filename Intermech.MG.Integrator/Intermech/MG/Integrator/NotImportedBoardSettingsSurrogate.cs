// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.NotImportedBoardSettingsSurrogate
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators.Electrical;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.MG.Integrator;

[DefaultProperty("ParameterName")]
internal sealed class NotImportedBoardSettingsSurrogate : ICloneable, IListParamValuesSettings
{
  private string _parameterValue;
  private string _parameterName;

  [DisplayName("Имя параметра")]
  [Description("Имя параметра компонента схемы или платы")]
  public string ParameterName
  {
    get => this._parameterName;
    set => this._parameterName = value;
  }

  [DisplayName("Значение параметра")]
  [Description("Значение параметра, при котором Board не будет импортирован в IPS ")]
  public string ParameterValue
  {
    get => this._parameterValue;
    set => this._parameterValue = value;
  }

  public NotImportedBoardSettingsSurrogate Clone()
  {
    return new NotImportedBoardSettingsSurrogate()
    {
      _parameterValue = this._parameterValue,
      _parameterName = this._parameterName
    };
  }

  object ICloneable.Clone() => (object) this.Clone();

  public override string ToString() => "Настройка";

  public override int GetHashCode()
  {
    int hashCode = 0;
    if (this._parameterValue != null)
      hashCode ^= this._parameterValue.GetHashCode();
    if (this._parameterName != null)
      hashCode ^= this._parameterName.GetHashCode();
    return hashCode;
  }

  public override bool Equals(object obj)
  {
    if (!(obj is NotImportedBoardSettingsSurrogate settingsSurrogate))
      return base.Equals(obj);
    return !(settingsSurrogate._parameterValue != this._parameterValue) && !(settingsSurrogate._parameterName != this._parameterName);
  }
}
