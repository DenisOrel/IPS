// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ServiceProcess.ServiceProcessConnectionParameters
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

#nullable disable
namespace Intermech.Scripting.CSharp.ServiceProcess;

/// <summary>
/// Контейнер для параметров подключения к сервисам компиляции C#-сценариев на основе Roslyn.
/// Реализация поддерживает thread safe через заморозку состояния (<see cref="T:Intermech.FreezableObject" />).
/// </summary>
public class ServiceProcessConnectionParameters : FreezableObject
{
  private bool separateProcessMode;
  private string executablePath;
  private static ServiceProcessConnectionParameters globalInstance = new ServiceProcessConnectionParameters();

  /// <summary>Создает объект.</summary>
  public ServiceProcessConnectionParameters()
  {
    this.separateProcessMode = true;
    this.executablePath = Environment.GetEnvironmentVariable("IPS_ROSLYNSCRIPTCOMPILER");
    if (this.executablePath != null)
      return;
    this.executablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RoslynScriptCompiler\\RoslynScriptCompiler.exe");
  }

  /// <summary>
  /// Включает или выключает режим использования отдельного процесса для
  /// размещения сервисов компиляции C#-сценариев. По умолчанию режим включен.
  /// Если режим выключен то сервисы компиляции C#-сценариев будут размещены в
  /// текущем AppDomain.
  /// </summary>
  public bool SeparateProcessMode
  {
    [DebuggerStepThrough] get => this.separateProcessMode;
    set
    {
      this.RequireNotFrozenBeforePropertyChange(nameof (SeparateProcessMode));
      this.separateProcessMode = value;
    }
  }

  /// <summary>
  /// Возвращает или задает путь к основной сборке компилятора C#-сценариев (RoslynScriptCompiler.exe)
  /// </summary>
  /// <exception cref="T:System.ArgumentException">Значение не должно быть пусто или равно null</exception>
  public string ExecutablePath
  {
    [DebuggerStepThrough] get => this.executablePath;
    set
    {
      if (string.IsNullOrEmpty(value))
        throw new ArgumentException("Не задан путь к исполняемому файлу компилятора C#-сценариев.", nameof (value));
      this.RequireNotFrozenBeforePropertyChange(nameof (ExecutablePath));
      this.executablePath = value;
    }
  }

  /// <summary>
  /// Возвращает или задает глобальный экземпляр параметров подключения.
  /// </summary>
  /// <exception cref="T:System.ArgumentNullException">Значение не должно быть null</exception>
  public static ServiceProcessConnectionParameters Global
  {
    [DebuggerStepThrough] get => ServiceProcessConnectionParameters.globalInstance;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      value.RequireFrozen();
      Interlocked.Exchange<ServiceProcessConnectionParameters>(ref ServiceProcessConnectionParameters.globalInstance, value);
    }
  }
}
