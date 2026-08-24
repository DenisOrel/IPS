// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ServiceProcess.CompilerClient
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
/// Клиент для доступа к компилятору C#-сценариев, работающему в изолированном процессе.
/// Реализация является thread safe.
/// </summary>
public class CompilerClient
{
  private readonly object syncRoot;
  private readonly ServiceProcessConnectionParameters serviceProcessParameters;
  private readonly IpcConnector<ICompilerRoot> remoteServiceConnector;
  private readonly LocalServiceConnector<ICompilerRoot> localServiceConnector;

  /// <summary>Создает объект.</summary>
  public CompilerClient()
  {
    this.syncRoot = new object();
    this.serviceProcessParameters = ServiceProcessConnectionParameters.Global;
    this.remoteServiceConnector = new IpcConnector<ICompilerRoot>();
    this.remoteServiceConnector.ConnectionInfo = new IpcConnectionInfo("RoslynScriptCompiler", "Compiler", this.serviceProcessParameters.ExecutablePath, $"--mode=Compiler --parent-process-id={CurrentProcessHelper.ProcessId}");
    this.remoteServiceConnector.EnableCommandLineSeparation = true;
    this.localServiceConnector = new LocalServiceConnector<ICompilerRoot>(this.serviceProcessParameters.ExecutablePath, "RoslynScriptCompiler.CSharp.CompilerRoot");
  }

  /// <summary>Возвращает признак, что подключение установлено.</summary>
  public bool IsConnected
  {
    [DebuggerStepThrough] get => this.TestIfConnected();
  }

  /// <summary>Возвращает объект компилятора C#-сценариев.</summary>
  /// <returns>Объект сервиса</returns>
  public IScriptCompiler GetScriptCompiler() => (IScriptCompiler) this.GetOrConnectCompilerRoot();

  private bool TestIfConnected()
  {
    lock (this.syncRoot)
      return this.serviceProcessParameters.SeparateProcessMode ? this.remoteServiceConnector.IsConnected : this.localServiceConnector.IsConnected;
  }

  private ICompilerRoot GetOrConnectCompilerRoot()
  {
    lock (this.syncRoot)
      return this.serviceProcessParameters.SeparateProcessMode ? this.remoteServiceConnector.GetOrConnect() : this.localServiceConnector.GetOrConnect();
  }
}
