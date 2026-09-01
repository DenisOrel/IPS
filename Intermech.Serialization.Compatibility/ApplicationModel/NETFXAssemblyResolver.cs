// Decompiled with JetBrains decompiler
// Type: Intermech.ApplicationModel.NETFXAssemblyResolver
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Intermech.ApplicationModel;

public class NETFXAssemblyResolver : AppAssemblyResolver
{
  private static readonly byte[] netcorePublicKeyToken = new byte[8]
  {
    (byte) 176 /*0xB0*/,
    (byte) 63 /*0x3F*/,
    (byte) 95,
    (byte) 127 /*0x7F*/,
    (byte) 17,
    (byte) 213,
    (byte) 10,
    (byte) 58
  };
  private static readonly Version netfxLastVersion = new Version(4, 0, 0, 0);

  protected override Assembly DoTryRedirectStrongNamedAssembly(AppAssemblyResolveRequest request)
  {
    if (!((IEnumerable<byte>) request.PublicKeyToken).SequenceEqual<byte>((IEnumerable<byte>) NETFXAssemblyResolver.netcorePublicKeyToken) || object.Equals((object) request.AssemblyName.Version, (object) NETFXAssemblyResolver.netfxLastVersion))
      return base.DoTryRedirectStrongNamedAssembly(request);
    AssemblyName assemblyRef = (AssemblyName) request.AssemblyName.Clone();
    assemblyRef.Version = NETFXAssemblyResolver.netfxLastVersion;
    return Assembly.Load(assemblyRef);
  }

  protected override Assembly DoTryResolveStrongNamedAssembly(AppAssemblyResolveRequest request)
  {
    Assembly strongNamedAssembly = this.TryGetLoadedStrongNamedAssembly(request);
    if (strongNamedAssembly != (Assembly) null)
      return strongNamedAssembly;
    Assembly assembly = this.DoTryAutoLoadAssembly(request);
    return assembly != (Assembly) null ? assembly : base.DoTryResolveStrongNamedAssembly(request);
  }

  protected override Assembly DoTryResolveSimpleNamedAssembly(AppAssemblyResolveRequest request)
  {
    Assembly simpleNamedAssembly = this.TryGetLoadedSimpleNamedAssembly(request);
    if (simpleNamedAssembly != (Assembly) null)
      return simpleNamedAssembly;
    Assembly assembly = this.DoTryAutoLoadAssembly(request);
    return assembly != (Assembly) null ? assembly : base.DoTryResolveSimpleNamedAssembly(request);
  }
}
