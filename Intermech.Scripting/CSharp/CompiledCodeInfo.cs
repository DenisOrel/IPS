// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.CompiledCodeInfo
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.CSharp;

[Serializable]
internal sealed class CompiledCodeInfo
{
  public CompiledCodeInfo(string assemblyFilePath, bool enableDebugInfo)
  {
    this.AssemblyFilePath = assemblyFilePath;
    this.EnableDebugInfo = enableDebugInfo;
  }

  public string AssemblyFilePath { get; private set; }

  public bool EnableDebugInfo { get; private set; }
}
