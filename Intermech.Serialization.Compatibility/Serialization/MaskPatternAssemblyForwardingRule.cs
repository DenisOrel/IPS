// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.MaskPatternAssemblyForwardingRule
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using Intermech.Text;
using System;
using System.Text.RegularExpressions;

#nullable disable
namespace Intermech.Serialization;

public sealed class MaskPatternAssemblyForwardingRule : TypeForwardingRule
{
  private Regex typeNamePattern;
  private Regex assemblyNamePattern;
  private string forwardedAssemblyName;

  public MaskPatternAssemblyForwardingRule(
    URTKind runtimeKind,
    string typeNamePattern,
    string assemblyNamePattern,
    string forwardedAssemblyName)
    : base(runtimeKind)
  {
    if (typeNamePattern == null)
      throw new ArgumentNullException(nameof (typeNamePattern));
    if (assemblyNamePattern == null)
      throw new ArgumentNullException(nameof (assemblyNamePattern));
    if (forwardedAssemblyName == null)
      throw new ArgumentNullException(nameof (forwardedAssemblyName));
    this.typeNamePattern = MaskPatterns.ToRegex(typeNamePattern);
    this.assemblyNamePattern = MaskPatterns.ToRegex(assemblyNamePattern);
    this.forwardedAssemblyName = forwardedAssemblyName;
  }

  public override bool TryApply(
    string typeName,
    string assemblyName,
    out string resultTypeName,
    out string resultAssemblyName)
  {
    if (this.typeNamePattern.IsMatch(typeName) && this.assemblyNamePattern.IsMatch(assemblyName))
    {
      resultTypeName = typeName;
      resultAssemblyName = this.forwardedAssemblyName;
      return true;
    }
    resultTypeName = (string) null;
    resultAssemblyName = (string) null;
    return false;
  }
}
