// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.CurrentProcessTempDirectoryService
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Сервис для генерации случайной временной папки для текущего процесса с возможностью
/// удаления файлов, оставшихся от предыдущих запусков этого процесса.
/// </summary>
/// <remarks>
/// Реализация класса является thread safe, а также может вызываться из изолированных AppDomain.
/// </remarks>
public class CurrentProcessTempDirectoryService : ScriptExecutorServiceBase
{
  private static readonly char[] DirectoryNameSeparator = new char[1]
  {
    '#'
  };
  private readonly string basePath;
  private readonly string directoryPath;

  /// <summary>Создает объект.</summary>
  /// <param name="basePath">Путь к базовой папке, внути которой будет создана временная папка для текущего процесса</param>
  public CurrentProcessTempDirectoryService(string basePath)
  {
    this.basePath = !string.IsNullOrEmpty(basePath) ? basePath : throw new ArgumentException("Не задан путь к папке для временных файлов исполнителя сценариев", nameof (basePath));
    this.directoryPath = Path.Combine(this.basePath, this.CreateDirectoryName(CurrentProcessHelper.LocationKey, CurrentProcessHelper.ProcessId.ToString()));
  }

  private string CreateDirectoryName(string locationKey, string processId)
  {
    return locationKey + (object) CurrentProcessTempDirectoryService.DirectoryNameSeparator[0] + processId;
  }

  /// <summary>
  /// Возвращает путь к базовой папке, внутри которой будет создана временная папка для текущего процесса
  /// </summary>
  public string BasePath
  {
    [DebuggerStepThrough] get => this.basePath;
  }

  /// <summary>
  /// Возвращает путь к временной папке для текущего процесса.
  /// </summary>
  public string DirectoryPath
  {
    [DebuggerStepThrough] get => this.directoryPath;
  }

  /// <summary>
  /// Создает временную папку для текущего процесса, а также удаляет файлы, оставшиеся от предыдущих запусков процесса.
  /// </summary>
  public void CreateDirectory()
  {
    if (!Directory.Exists(this.directoryPath))
      Directory.CreateDirectory(this.directoryPath);
    List<string> previousDirectories = this.GetPreviousDirectories();
    if (previousDirectories.Count == 0)
      return;
    foreach (string str in previousDirectories)
    {
      FileUtils.DeleteFilesSilently(str, true);
      FileUtils.DeleteDirectorySilently(str, true);
    }
  }

  /// <summary>
  /// Возвращает пути к временным папкам текущего процесса, оставшиеся от предыдущих запусков процесса.
  /// </summary>
  /// <returns>Список путей</returns>
  public List<string> GetPreviousDirectories()
  {
    List<string> previousDirectories = new List<string>();
    foreach (string directory in Directory.GetDirectories(this.basePath, this.CreateDirectoryName(CurrentProcessHelper.LocationKey, "*"), SearchOption.TopDirectoryOnly))
    {
      if (!PathUtils.IsSamePath(directory, this.directoryPath))
      {
        string[] strArray = directory.Split(CurrentProcessTempDirectoryService.DirectoryNameSeparator);
        int result;
        if (strArray.Length == 2 && strArray[1] != null && int.TryParse(strArray[1], out result) && this.TryGetProcessById(result) == null)
          previousDirectories.Add(directory);
      }
    }
    return previousDirectories;
  }

  private Process TryGetProcessById(int processId)
  {
    try
    {
      return Process.GetProcessById(processId);
    }
    catch
    {
      return (Process) null;
    }
  }
}
