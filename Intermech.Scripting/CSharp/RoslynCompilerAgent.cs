// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.RoslynCompilerAgent
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common;
using Intermech.Scripting.CSharp.ServiceProcess;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Базовый класс для агентов компиляции C#-сценариев на основе Roslyn.
/// Реализация класса не является thread safe.
/// </summary>
internal sealed class RoslynCompilerAgent : ScriptAgentBase, IScriptCompilerAgent
{
  private CompilerClient compilerClient;

  public RoslynCompilerAgent() => this.compilerClient = new CompilerClient();

  public void Compile(string scriptCode, ScriptCodeKey scriptCodeKey, CompileParams compileParams)
  {
    string filePath = this.ExecutorServices.TempFileNameGenerator.CreateFilePath();
    File.WriteAllText(filePath, scriptCode, Encoding.UTF8);
    string file = this.CompileToFile(filePath, compileParams.EnableDebugInfo);
    new ScriptAssemblyChecker().CheckAssembly(this.LoadScriptAssembly(file));
    CompiledCodeInfo compiledCodeInfo = new CompiledCodeInfo(file, compileParams.EnableDebugInfo);
    this.CompiledCodeCacheService.Update(scriptCodeKey, compiledCodeInfo);
  }

  private string CompileToFile(string scriptFilePath, bool enableDebugInfo)
  {
    ScriptCompilerOptions options = new ScriptCompilerOptions();
    options.EnableDebugInfo = enableDebugInfo;
    options.AutoReferencedAssemblies.AddRange((IEnumerable<string>) this.ExecutorServices.AutoReferencedAssemblies);
    options.SearchPathList.AddRange((IEnumerable<string>) this.ExecutorServices.SearchPathListProvider.GetSearchPathList());
    return this.compilerClient.GetScriptCompiler().CompileToFile(scriptFilePath, options);
  }
}
