// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamEntityFixed
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

[Serializable]
public class TechParamEntityFixed : TechParamEntity
{
  public TechParamEntityFixed(string code, object value, bool isFixed, string caption = null)
    : base(code, value)
  {
    this.IsFixed = isFixed;
    this.Caption = caption;
  }

  public TechParamEntityFixed(ITechParamBase obj)
    : base(obj)
  {
    if (obj is TechParamEntityFixed paramEntityFixed)
      this.Caption = paramEntityFixed.Caption;
    else
      this.Caption = string.Empty;
  }

  public string Caption { get; set; }

  public override bool IsFixed { get; set; }
}
