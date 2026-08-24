// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ServiceProcess.IScriptLanguageServer
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.CSharp.ServiceProcess;

/// <summary>
/// Базовый интерфейс языковых сервисов C#-сценариев.
/// Реализация должна быть thread safe.
/// </summary>
public interface IScriptLanguageServer
{
  void OpenDocument(Uri documentUri, ScriptParseOptions parseOptions, string scriptText);

  void ChangeDocument(Uri documentUri, List<ScriptTextChange> changes);

  void ChangeParseOptions(Uri documentUri, ScriptParseOptions parseOptions);

  void CloseDocument(Uri documentUri);
}
