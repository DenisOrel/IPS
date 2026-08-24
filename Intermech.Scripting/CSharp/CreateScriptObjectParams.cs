// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.CreateScriptObjectParams
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common;
using System;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Класс контейнера параметров, который содержит все необходимое для
/// создания свободного объекта C#-сценария.
/// </summary>
/// <remarks>Реализация является immutable.</remarks>
[Serializable]
internal sealed class CreateScriptObjectParams(
  object scriptContext,
  Type scriptContextType,
  ScriptInvocationData invocationData,
  bool enableDebugInfo) : ExecuteAgentParams(scriptContext, scriptContextType, invocationData, enableDebugInfo)
{
}
