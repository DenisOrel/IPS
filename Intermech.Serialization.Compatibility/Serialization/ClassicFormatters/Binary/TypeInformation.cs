// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.TypeInformation
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class TypeInformation
{
  internal TypeInformation(string fullTypeName, string assemblyString, bool hasTypeForwardedFrom)
  {
    this.FullTypeName = fullTypeName;
    this.AssemblyString = assemblyString;
    this.HasTypeForwardedFrom = hasTypeForwardedFrom;
  }

  internal string FullTypeName { get; }

  internal string AssemblyString { get; }

  internal bool HasTypeForwardedFrom { get; }
}
