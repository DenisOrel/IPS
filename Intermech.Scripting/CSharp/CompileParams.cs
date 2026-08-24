// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.CompileParams
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Класс контейнера параметров, которые описывают параметры компиляции C#-сценария.
/// Объекты этого класса используются для передачи всех необходимых параметров из
/// основного AppDomain приложения в изолированный AppDomain, где C#-сценарий и будет компилироваться.
/// </summary>
[Serializable]
internal sealed class CompileParams
{
  private Type scriptContextType;
  private bool enableDebugInfo;

  public CompileParams(Type scriptContextType, bool enableDebugInfo)
  {
    this.scriptContextType = scriptContextType;
    this.enableDebugInfo = enableDebugInfo;
  }

  public Type ScriptContextType
  {
    [DebuggerStepThrough] get => this.scriptContextType;
  }

  public bool EnableDebugInfo
  {
    [DebuggerStepThrough] get => this.enableDebugInfo;
  }
}
