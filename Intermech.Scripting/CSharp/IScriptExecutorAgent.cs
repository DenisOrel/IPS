// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.IScriptExecutorAgent
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common;

#nullable disable
namespace Intermech.Scripting.CSharp;

internal interface IScriptExecutorAgent
{
  ScriptExecutorServices ExecutorServices { get; set; }

  object Execute(string scriptCode, ScriptCodeKey scriptCodeKey, ExecuteParams executeParams);

  IScriptObjectKeeper CreateScriptObject(
    string scriptCode,
    ScriptCodeKey scriptCodeKey,
    CreateScriptObjectParams createParams);
}
