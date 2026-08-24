// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.TParamValue
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal sealed class TParamValue
{
  public int ParameterID { get; private set; }

  public object Value { get; set; }

  public TParamValue(int parameterID)
    : this(parameterID, (object) null)
  {
  }

  public TParamValue(int parameterID, object value)
  {
    this.ParameterID = parameterID;
    this.Value = value;
  }
}
