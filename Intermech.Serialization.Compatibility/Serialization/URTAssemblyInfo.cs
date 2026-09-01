// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.URTAssemblyInfo
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Reflection;

#nullable disable
namespace Intermech.Serialization;

public static class URTAssemblyInfo
{
  public static readonly Assembly Assembly = typeof (object).Assembly;
  public static readonly string AssemblyName = URTAssemblyInfo.Assembly.FullName;
  public static readonly bool IsNETFX = URTAssemblyInfo.AssemblyName.StartsWith("mscorlib");
  public static readonly bool IsNETCore = URTAssemblyInfo.AssemblyName.StartsWith("System.Private.CoreLib");
  public static readonly string mscorlibAssemblyName = "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
  public static readonly Version mscorlibAssemblyVersion = new Version(4, 0, 0, 0);

  public static URTKind GetRuntimeKind()
  {
    return !URTAssemblyInfo.IsNETCore ? URTKind.NETFX : URTKind.NETCore;
  }
}
