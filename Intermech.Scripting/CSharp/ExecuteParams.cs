// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ExecuteParams
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
/// Класс контейнера параметров, который содержит все необходимое для
/// выполнения метода Execute в C#-сценарии.
/// </summary>
/// <remarks>Реализация является immutable.</remarks>
[Serializable]
internal sealed class ExecuteParams : ExecuteAgentParams
{
  private object[] arguments;
  private IScriptOutputStream debugStream;

  public ExecuteParams(
    object scriptContext,
    Type scriptContextType,
    object[] arguments,
    ScriptInvocationData invocationData,
    bool enableDebugInfo,
    IScriptOutputStream debugStream)
    : base(scriptContext, scriptContextType, invocationData, enableDebugInfo)
  {
    this.arguments = arguments;
    this.debugStream = debugStream;
  }

  public object[] Arguments
  {
    [DebuggerStepThrough] get => this.arguments;
  }

  public IScriptOutputStream DebugStream
  {
    [DebuggerStepThrough] get => this.debugStream;
  }
}
