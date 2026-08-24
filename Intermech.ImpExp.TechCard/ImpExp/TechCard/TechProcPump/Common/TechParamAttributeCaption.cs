// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamAttributeCaption
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface.DataWriter;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

[Serializable]
public class TechParamAttributeCaption : TechParamAttribute
{
  public TechParamAttributeCaption(
    IAttributeTypeItem attributeType,
    object value,
    string caption,
    EntitySetting.AttributeBelongs attrBelong)
    : base(attributeType, value, attrBelong)
  {
    this.Caption = caption;
  }

  public TechParamAttributeCaption(ITechParamBase obj)
    : base(obj)
  {
  }

  public override string Caption { get; set; }
}
