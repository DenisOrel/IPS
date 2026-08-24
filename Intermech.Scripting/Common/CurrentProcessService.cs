// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.CurrentProcessService
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Сервис для получения информации о текущем процессе операционной системы.
/// </summary>
/// <remarks>Реализация класса является thread safe.</remarks>
internal sealed class CurrentProcessService : ScriptExecutorServiceBase
{
  private int processId;
  private string locationKey;

  /// <summary>Создает объект.</summary>
  public CurrentProcessService()
  {
    using (Process currentProcess = Process.GetCurrentProcess())
    {
      this.processId = currentProcess.Id;
      this.locationKey = this.CreateLocationKey(currentProcess);
    }
  }

  private string CreateLocationKey(Process process)
  {
    StringBuilder stringBuilder = new StringBuilder(Convert.ToBase64String(new SHA1TextHasher(260).ComputeHash(process.MainModule.FileName.ToUpper())));
    stringBuilder.Replace('+', '-');
    stringBuilder.Replace('/', '_');
    return stringBuilder.ToString();
  }

  /// <summary>Возвращает идентификатор текущего процесса.</summary>
  public int ProcessId
  {
    [DebuggerStepThrough] get => this.processId;
  }

  /// <summary>
  /// Возвращает уникальный ключ расположения текущего процесса на диске.
  /// </summary>
  public string LocationKey
  {
    [DebuggerStepThrough] get => this.locationKey;
  }
}
