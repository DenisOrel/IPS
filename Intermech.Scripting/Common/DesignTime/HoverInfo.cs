// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.HoverInfo
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>Всплывающая подсказка для текста под курсором ввода.</summary>
public class HoverInfo
{
  /// <summary>Создает объект</summary>
  /// <param name="text">Текст подсказки (может быть пустым)</param>
  /// <exception cref="T:System.ArgumentNullException">параметр <paramref name="text" /> содержит null</exception>
  public HoverInfo(string text)
  {
    this.Text = text != null ? text : throw new ArgumentNullException(nameof (text));
  }

  /// <summary>
  /// Возвращает текст подсказки.
  /// Значение свойства может быть пустым.
  /// </summary>
  public string Text { get; }
}
