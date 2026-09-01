// Decompiled with JetBrains decompiler
// Type: Intermech.ApplicationModel.AppAssemblyResolveRequest
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Reflection;

#nullable disable
namespace Intermech.ApplicationModel;

public sealed class AppAssemblyResolveRequest
{
  public AppAssemblyResolveRequest(AssemblyName assemblyName, Assembly requestingAssembly = null)
  {
    this.AssemblyName = assemblyName != null ? assemblyName : throw new ArgumentNullException(nameof (assemblyName));
    this.SimpleName = assemblyName.Name;
    this.PublicKeyToken = assemblyName.GetPublicKeyToken();
    this.RequestingAssembly = requestingAssembly;
  }

  public AssemblyName AssemblyName { get; }

  public string SimpleName { get; }

  public byte[] PublicKeyToken { get; }

  public Assembly RequestingAssembly { get; }

  public bool IsStrongNamed => this.PublicKeyToken != null && this.PublicKeyToken.Length != 0;
}
