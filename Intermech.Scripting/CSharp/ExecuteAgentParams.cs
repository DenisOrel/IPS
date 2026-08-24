// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ExecuteAgentParams
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Базовый класс контейнера параметров для методов объекта IScriptExecutorAgent.
/// </summary>
/// <remarks>Реализация является immutable.</remarks>
[Serializable]
internal abstract class ExecuteAgentParams
{
  private object scriptContext;
  private Type scriptContextType;
  private ScriptInvocationData invocationData;
  private bool enableDebugInfo;

  public ExecuteAgentParams(
    object scriptContext,
    Type scriptContextType,
    ScriptInvocationData invocationData,
    bool enableDebugInfo)
  {
    this.scriptContext = scriptContext;
    this.scriptContextType = scriptContextType;
    this.invocationData = invocationData;
    this.enableDebugInfo = enableDebugInfo;
  }

  public object ScriptContext
  {
    [DebuggerStepThrough] get => this.scriptContext;
  }

  public Type ScriptContextType
  {
    [DebuggerStepThrough] get => this.scriptContextType;
  }

  public ScriptInvocationData InvocationData
  {
    [DebuggerStepThrough] get => this.invocationData;
  }

  public bool EnableDebugInfo
  {
    [DebuggerStepThrough] get => this.enableDebugInfo;
  }
}
