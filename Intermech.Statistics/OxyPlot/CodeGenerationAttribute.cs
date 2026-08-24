// Decompiled with JetBrains decompiler
// Type: OxyPlot.CodeGenerationAttribute
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

[AttributeUsage(AttributeTargets.Property)]
public class CodeGenerationAttribute : Attribute
{
  public CodeGenerationAttribute(bool generateCode) => this.GenerateCode = generateCode;

  public bool GenerateCode { get; set; }
}
