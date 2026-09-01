// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.TopLevelAssemblyTypeResolver
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Reflection;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class TopLevelAssemblyTypeResolver
{
  private readonly Assembly _topLevelAssembly;

  public TopLevelAssemblyTypeResolver(Assembly topLevelAssembly)
  {
    this._topLevelAssembly = topLevelAssembly;
  }

  public Type ResolveType(Assembly assembly, string simpleTypeName, bool ignoreCase)
  {
    if (assembly == (Assembly) null)
      assembly = this._topLevelAssembly;
    return assembly.GetType(simpleTypeName, false, ignoreCase);
  }
}
