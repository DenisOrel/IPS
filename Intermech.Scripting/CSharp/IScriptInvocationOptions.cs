// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.IScriptInvocationOptions
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>Опции выполнения C#-сценария.</summary>
public interface IScriptInvocationOptions
{
  /// <summary>
  /// Включает и выключает добавление отладочной информации в выполняемые сценарии.
  /// </summary>
  bool EnableDebugInfo { get; }

  /// <summary>
  /// Возвращает объект для перехвата отладочного вывода выполняемых сценариев.
  /// Значение свойства может быть не задано.
  /// </summary>
  IScriptOutputStream DebugStream { get; }
}
