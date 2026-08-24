// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ServiceProcess.LanguageServerClient
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Remoting.Ipc;
using Intermech.Scripting.Common;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.CSharp.ServiceProcess;

/// <summary>
/// Клиент для доступа к языковым сервисам для C#-сценариев, работающим в изолированном процессе
/// Реализация является thread safe.
/// </summary>
public class LanguageServerClient
{
  private readonly object syncRoot;
  private readonly ServiceProcessConnectionParameters serviceProcessParameters;
  private readonly IpcConnector<ILanguageServerRoot> remoteServiceConnector;
  private readonly LocalServiceConnector<ILanguageServerRoot> localServiceConnector;

  /// <summary>Создает объект.</summary>
  public LanguageServerClient()
  {
    this.syncRoot = new object();
    this.serviceProcessParameters = ServiceProcessConnectionParameters.Global;
    this.remoteServiceConnector = new IpcConnector<ILanguageServerRoot>();
    this.remoteServiceConnector.ConnectionInfo = new IpcConnectionInfo("RoslynScriptCompiler", "LanguageServer", this.serviceProcessParameters.ExecutablePath, $"--mode=LanguageServer --parent-process-id={CurrentProcessHelper.ProcessId}");
    this.remoteServiceConnector.EnableCommandLineSeparation = true;
    this.localServiceConnector = new LocalServiceConnector<ILanguageServerRoot>(this.serviceProcessParameters.ExecutablePath, "RoslynScriptCompiler.CSharp.LanguageServerRoot");
  }

  /// <summary>Возвращает признак, что подключение установлено.</summary>
  public bool IsConnected
  {
    [DebuggerStepThrough] get => this.TestIfConnected();
  }

  /// <summary>Возвращает языковые сервисы C#-сценариев.</summary>
  /// <returns>Объект сервиса</returns>
  public IScriptLanguageServer GetScriptLanguageServer()
  {
    return (IScriptLanguageServer) this.GetOrConnectLanguageServerRoot();
  }

  /// <summary>Возвращает сервис парсера C#-сценариев.</summary>
  /// <returns>Объект сервиса</returns>
  public IScriptParser GetParserService() => (IScriptParser) this.GetOrConnectLanguageServerRoot();

  private bool TestIfConnected()
  {
    lock (this.syncRoot)
      return this.serviceProcessParameters.SeparateProcessMode ? this.remoteServiceConnector.IsConnected : this.localServiceConnector.IsConnected;
  }

  private ILanguageServerRoot GetOrConnectLanguageServerRoot()
  {
    lock (this.syncRoot)
      return this.serviceProcessParameters.SeparateProcessMode ? this.remoteServiceConnector.GetOrConnect() : this.localServiceConnector.GetOrConnect();
  }
}
