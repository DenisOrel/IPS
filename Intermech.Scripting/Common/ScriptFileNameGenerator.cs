// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.ScriptFileNameGenerator
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Сервис для генерации случайных имен файлов сценариев, который гарантирует уникальность имени,
/// даже при вызове из разных AppDomain.
/// </summary>
/// <remarks>
/// Реализация класса является thread safe, а также может вызываться из изолированных AppDomain.
/// </remarks>
public sealed class ScriptFileNameGenerator : ScriptExecutorServiceBase
{
  private string baseDirectory;
  private string baseFileName;
  private string @extension;
  private static long nextIdGenerator;

  /// <summary>Создает объект.</summary>
  /// <param name="baseDirectory">Путь к папке для временных файлов исполнителя сценариев</param>
  /// <param name="baseFileName">Префикс имен файлов для временных файлов исполнителя сценариев</param>
  /// <param name="extension">Расширение для временных файлов исполнителя сценариев</param>
  public ScriptFileNameGenerator(string baseDirectory, string baseFileName, string @extension)
  {
    if (string.IsNullOrEmpty(baseDirectory))
      throw new ArgumentException("Не задан путь к папке для временных файлов исполнителя сценариев", nameof (baseDirectory));
    if (string.IsNullOrEmpty(baseFileName))
      throw new ArgumentException("Не задан префикс имен файлов для временных файлов исполнителя сценариев", nameof (baseFileName));
    if (string.IsNullOrEmpty(@extension))
      throw new ArgumentException("Не задано расширение для временных файлов исполнителя сценариев", nameof (@extension));
    this.baseDirectory = baseDirectory;
    this.baseFileName = $"{baseFileName}_{Guid.NewGuid().ToString("N")}";
    this.@extension = @extension;
  }

  /// <summary>
  /// Возвращает путь к папке для временных файлов исполнителя сценариев.
  /// </summary>
  public string BaseDirectory
  {
    [DebuggerStepThrough] get => this.baseDirectory;
  }

  /// <summary>
  /// Возвращает маску имен файлов для временных файлов исполнителя сценариев.
  /// </summary>
  /// <returns>Маска имен файлов</returns>
  public string GetFileNameMask() => $"{this.baseFileName}_*";

  /// <summary>
  /// Создает и возвращает случайное имя файла без расширения и пути.
  /// </summary>
  /// <returns>Имя файла без расширения и пути</returns>
  public string CreateFileNameWithoutExtension()
  {
    return $"{this.baseFileName}_{Interlocked.Increment(ref ScriptFileNameGenerator.nextIdGenerator).ToString()}";
  }

  /// <summary>
  /// Создает и возвращает случайное имя файла с расширением, но без пути.
  /// </summary>
  /// <returns>Имя файла с расширением, но без пути</returns>
  public string CreateFileName()
  {
    return $"{this.baseFileName}_{Interlocked.Increment(ref ScriptFileNameGenerator.nextIdGenerator).ToString()}{this.@extension}";
  }

  /// <summary>
  /// Создает и возвращает случайное имя файла с расширением и путем.
  /// </summary>
  /// <returns>Имя файла с расширением и путем</returns>
  public string CreateFilePath() => Path.Combine(this.baseDirectory, this.CreateFileName());
}
