// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.UniqueIdGenerator
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Threading;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Сервис для генерации случайных идентификаторов, который гарантирует уникальность
/// даже при вызове из разных AppDomain.
/// </summary>
/// <remarks>
/// Реализация класса является thread safe, а также может вызываться из изолированных AppDomain.
/// </remarks>
public sealed class UniqueIdGenerator : ScriptExecutorServiceBase
{
  private string baseString;
  private static long nextIdGenerator;

  /// <summary>Создает объект.</summary>
  public UniqueIdGenerator() => this.baseString = Guid.NewGuid().ToString("N");

  /// <summary>
  /// Создает и возвращает случайный уникальный идентификатор.
  /// </summary>
  /// <returns>Случайный уникальный идентификатор</returns>
  public string CreateId()
  {
    return $"{this.baseString}_{Interlocked.Increment(ref UniqueIdGenerator.nextIdGenerator).ToString()}";
  }
}
