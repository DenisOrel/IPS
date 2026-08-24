// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ScriptInvocationOptions
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>Базовый класс для опций выполнения C#-сценариев.</summary>
[Serializable]
public class ScriptInvocationOptions : IScriptInvocationOptions
{
  private bool enableDebugInfo;
  private IScriptOutputStream debugStream;

  /// <summary>Создает объект.</summary>
  /// <param name="enableDebugInfo">Включает и выключает добавление отладочной информации в выполняемые сценарии</param>
  /// <param name="debugStream">Объект для перехвата отладочного вывода выполняемых сценариев. Параметр может быть не задан</param>
  public ScriptInvocationOptions(bool enableDebugInfo, IScriptOutputStream debugStream = null)
  {
    this.enableDebugInfo = enableDebugInfo;
    this.debugStream = debugStream;
  }

  /// <summary>
  /// Включает и выключает добавление отладочной информации в выполняемые сценарии.
  /// </summary>
  public bool EnableDebugInfo
  {
    [DebuggerStepThrough] get => this.enableDebugInfo;
  }

  /// <summary>
  /// Возвращает объект для перехвата отладочного вывода выполняемых сценариев.
  /// Значение свойства может быть не задано и равно null.
  /// </summary>
  public IScriptOutputStream DebugStream
  {
    [DebuggerStepThrough] get => this.debugStream;
  }
}
