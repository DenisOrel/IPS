// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ServiceProcess.ILanguageServerRoot
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Remoting.Ipc;

#nullable disable
namespace Intermech.Scripting.CSharp.ServiceProcess;

/// <summary>
/// Интерфейс головного объекта языковых сервисов C#-сценариев, работающих в изолированном процессе.
/// Реализация должна быть thread safe.
/// </summary>
public interface ILanguageServerRoot : IReliableIpcObject, IScriptLanguageServer, IScriptParser
{
}
