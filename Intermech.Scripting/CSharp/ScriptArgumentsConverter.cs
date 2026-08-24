// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ScriptArgumentsConverter
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Базовый класс для валидации и преобразования аргументов вызовов C#-сценариев.
/// Реализация класса должна быть thread safe.
/// </summary>
public class ScriptArgumentsConverter
{
  /// <summary>
  /// Проверяет и, при необходимости, выполняет преобразование аргументов вызова сценария.
  /// Метод вызывается перед каждым выполнением сценария.
  /// </summary>
  /// <param name="arguments">Аргументы вызова сценария</param>
  /// <exception cref="T:System.ArgumentNullException">Параметр <paramref name="arguments" /> не должен быть равен null</exception>
  public void Convert(IList<object> arguments)
  {
    if (arguments == null)
      throw new ArgumentNullException(nameof (arguments));
    if (arguments.Count == 0)
      return;
    this.DoConvert(arguments);
  }

  /// <summary>
  /// Проверяет и, при необходимости, выполняет преобразование аргументов вызова сценария.
  /// Метод вызывается перед каждым выполнением сценария.
  /// </summary>
  /// <param name="arguments">Аргументы вызова сценария</param>
  protected virtual void DoConvert(IList<object> arguments)
  {
  }
}
