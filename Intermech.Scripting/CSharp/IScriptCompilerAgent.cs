// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.IScriptCompilerAgent
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common;

#nullable disable
namespace Intermech.Scripting.CSharp;

internal interface IScriptCompilerAgent
{
  ScriptExecutorServices ExecutorServices { get; set; }

  void Compile(string scriptCode, ScriptCodeKey scriptCodeKey, CompileParams compileParams);
}
