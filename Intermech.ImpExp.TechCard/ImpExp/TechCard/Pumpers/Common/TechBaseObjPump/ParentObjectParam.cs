// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ParentObjectParam
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

internal class ParentObjectParam
{
  private TechParentObject.TechParamType _paramType;
  private object _paramValue;

  public TechParentObject.TechParamType ParamType
  {
    get => this._paramType;
    set => this._paramType = value;
  }

  public object ParamValue
  {
    get => this._paramValue;
    set => this._paramValue = value;
  }

  public enum ParentObjectParamType
  {
    Inheritable,
    Personal,
  }
}
