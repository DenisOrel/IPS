// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.AttributeFieldTypeAttribute
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AttributeFieldTypeAttribute : Attribute
{
  public readonly FieldTypes[] AttributeFieldTypes;

  public AttributeFieldTypeAttribute(params FieldTypes[] attributeFieldTypes)
  {
    this.AttributeFieldTypes = attributeFieldTypes;
  }
}
