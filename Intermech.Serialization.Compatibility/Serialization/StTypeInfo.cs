// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.StTypeInfo
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Serialization;

public class StTypeInfo : IEquatable<StTypeInfo>
{
  protected string typeName;
  protected string assemblyName;
  private int? hashCode;
  public static readonly StTypeInfo Empty = new StTypeInfo(string.Empty, string.Empty);

  public StTypeInfo(string typeName, string assemblyName)
  {
    if (typeName == null)
      throw new ArgumentNullException(nameof (typeName));
    if (assemblyName == null)
      throw new ArgumentNullException(nameof (assemblyName));
    this.typeName = typeName;
    this.assemblyName = assemblyName;
  }

  public string TypeName
  {
    [DebuggerStepThrough] get => this.typeName;
  }

  public string AssemblyName
  {
    [DebuggerStepThrough] get => this.assemblyName;
  }

  public bool IsEmpty
  {
    [DebuggerStepThrough] get => this.typeName == string.Empty;
  }

  public bool Equals(StTypeInfo other)
  {
    if (this == other)
      return true;
    return other != null && other.typeName == this.typeName && other.assemblyName == this.assemblyName;
  }

  public override bool Equals(object obj) => this == obj || this.Equals(obj as StTypeInfo);

  public override int GetHashCode()
  {
    if (!this.hashCode.HasValue)
      this.hashCode = new int?(this.typeName.GetHashCode() ^ this.assemblyName.GetHashCode());
    return this.hashCode.Value;
  }
}
