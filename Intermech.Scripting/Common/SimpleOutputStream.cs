// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.SimpleOutputStream
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Простой объект для перехвата потоков вывода сценария.
/// Реализация не является thread safe.
/// </summary>
[Serializable]
public sealed class SimpleOutputStream : IScriptOutputStream
{
  private List<string> lines;

  /// <summary>Создает объект.</summary>
  public SimpleOutputStream() => this.lines = new List<string>();

  /// <summary>Копирует весь вывод сценария в указанный поток.</summary>
  /// <param name="destinationStream">Поток, куда должен быть скопирован вывод сценария</param>
  /// <exception cref="T:System.ArgumentNullException">Параметр <paramref name="destinationStream" /> не должен быть равен null</exception>
  public void CopyTo(IScriptOutputStream destinationStream)
  {
    if (destinationStream == null)
      throw new ArgumentNullException(nameof (destinationStream));
    foreach (string line in this.lines)
      destinationStream.WriteLine(line);
  }

  /// <summary>Возвращает весь вывод сценария в виде массива строк.</summary>
  /// <returns>Вывод сценария в виде массива строк</returns>
  public string[] ToArray() => this.lines.ToArray();

  /// <summary>
  /// Выводит строку текста и выполняет переход на следующую строку.
  /// </summary>
  /// <param name="line">Строка текста для вывода</param>
  public void WriteLine(string line)
  {
    if (line == null)
      return;
    this.lines.Add(line);
  }
}
