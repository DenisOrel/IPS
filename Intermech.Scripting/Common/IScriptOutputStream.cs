// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.IScriptOutputStream
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Интерфейс объекта для перехвата потоков вывода сценария.
/// </summary>
public interface IScriptOutputStream
{
  /// <summary>
  /// Выводит строку текста и выполняет переход на следующую строку.
  /// </summary>
  /// <param name="line">Строка текста для вывода</param>
  void WriteLine(string line);
}
